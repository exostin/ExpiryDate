using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using DisplayObjectData;
using ScriptableObjects;
using TMPro;
using UnityEditor.U2D;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class BattleMenuController : MonoBehaviour
    {
        public List<Character> playerCharacters;
        public List<Character> enemyCharacters;

        private List<Character> targetsForEnemyPool = new List<Character>();
        private List<Character> targetsForPlayerPool = new List<Character>();

        private List<Character> battleQueue = new List<Character>();
        
        private Enemy enemy = new Enemy();
        private BattleMenuPlayer player = new BattleMenuPlayer();

        private int turnCounter = 0;
        [SerializeField] private TMP_Text turnCounterText;

        private GameManager gm;

        [SerializeField] private GameObject[] skillButtons;

        [HideInInspector] public Character playerSelectedTarget = null;
        [HideInInspector] public Ability playerSelectedAbility = null;

        [SerializeField] private TMP_Text selectedTarget;
        [SerializeField] private TMP_Text selectedAbility;

        private void Start()
        {
            gm = FindObjectOfType<GameManager>();
            targetsForPlayerPool.AddRange(enemyCharacters);
            targetsForEnemyPool.AddRange(playerCharacters);
            
            CreateQueue();
            StartCoroutine(PlayBattle());
        }

        private bool CheckIfAnySideWon()
        {
            // returns `true` if any player character is alive while all enemies are dead OR if all player characters are dead while any enemy is alive
            return playerCharacters.Any(character => character.health > 0) && enemyCharacters.All(character => character.health <= 0) ||
                   playerCharacters.All(character => character.health <= 0) && enemyCharacters.Any(character => character.health > 0);
        }

        private void CreateQueue()
        {
            // Merge playerCharacters and enemyCharacters into one array
            battleQueue = playerCharacters.Concat(enemyCharacters).ToList();
            // Sort the battle queue by initiative
            battleQueue = battleQueue.OrderByDescending(character => character.initiative).ToList();
        }

        private IEnumerator PlayBattle()
        {
            while (!CheckIfAnySideWon())
            {
                StartCoroutine(MakeTurn());
                yield return new WaitUntil(
                    () => gm.stateController.fsm.State == StateController.States.ReadyForNextTurn);
            }
        }
        
        private IEnumerator MakeTurn()
        {
            // if (!CheckIfAnySideWon()) ;
            turnCounter++;
            turnCounterText.text = "Turn: " + turnCounter.ToString();
            
            // using `.ToList()` here to avoid "Collection was modified; enumeration operation may not execute." error
            // https://stackoverflow.com/a/27851493
            foreach (var character in battleQueue.ToList())
            {
                UpdateSelectedAbilityText();
                UpdateSelectedTargetText();
                if (character.health <= 0)
                {
                    if (character.isOwnedByPlayer)
                    {
                        targetsForEnemyPool.Remove(character);
                    }
                    else
                    {
                        targetsForPlayerPool.Remove(character);
                    }
                    battleQueue.Remove(character);
                    continue;
                }
                if (character.isOwnedByPlayer)
                {
                    UpdateSkillButtons(character);
                    gm.stateController.fsm.ChangeState(StateController.States.PlayerTurn);
                    Debug.Log($"{character.characterName} turn!");
                    
                    // Wait until player does his turn and then continue
                    yield return new WaitUntil(() => gm.stateController.fsm.State == StateController.States.PlayerFinalizedHisMove);
                    
                    if (playerSelectedAbility != null && playerSelectedTarget != null)
                    {
                        player.MakeAttack(character, playerSelectedTarget, playerSelectedAbility);
                    }
                    
                    playerSelectedAbility = null;
                    playerSelectedTarget = null;
                }
                else
                {
                    gm.stateController.fsm.ChangeState(StateController.States.EnemyTurn);
                    var randomTargetIndex = Random.Range(0, targetsForEnemyPool.Count);
                    enemy.MakeAttack(characterUsedForAttack:character, target:targetsForEnemyPool[randomTargetIndex]);
                    yield return new WaitForSecondsRealtime(0.5f);
                }
            }
            gm.stateController.fsm.ChangeState(StateController.States.ReadyForNextTurn);
        }

        public void EndPlayerTurn()
        {
            if (playerSelectedAbility != null && playerSelectedTarget != null) gm.stateController.fsm.ChangeState(StateController.States.PlayerFinalizedHisMove);
            else Debug.Log("Player didn't correctly end turn! (No ability and/or target chosen!)");
        }

        public void StartTargetSelectionState()
        {
            gm.stateController.fsm.ChangeState(StateController.States.SelectingTarget);
        }

        private void UpdateSkillButtons(Character currentCharacter)
        {
            for (int i = 0; i <= 3; i++)
            {
                Debug.Log($"Updating ability no. {i}");
                skillButtons[i].GetComponent<DisplayAbilityData>().ability = currentCharacter.abilities[i];
                skillButtons[i].GetComponent<DisplayAbilityData>().UpdateAbilityDisplay();
            }
        }

        public void UpdateSelectedAbilityText()
        {
            selectedAbility.text = playerSelectedAbility != null ? $"Selected ability: {playerSelectedAbility.abilityName}" : $"Selected ability: none";
        }

        public void UpdateSelectedTargetText()
        {
            selectedTarget.text = playerSelectedTarget != null ? $"Selected target: {playerSelectedTarget.characterName}" : $"Selected target: none";
        }
    }
}
