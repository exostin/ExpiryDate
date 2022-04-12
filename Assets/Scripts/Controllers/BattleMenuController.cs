using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using DisplayObjectData;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class BattleMenuController : MonoBehaviour
    {
        public List<Character> playerCharacters;
        public List<Character> enemyCharacters;
        [SerializeField] private TMP_Text turnCounterText;
        [SerializeField] private GameObject[] skillButtons;
        [HideInInspector] public Character playerSelectedTarget;
        [HideInInspector] public Ability playerSelectedAbility;
        [SerializeField] private TMP_Text selectedTargetSign;
        [SerializeField] private TMP_Text selectedAbilitySign;
        [SerializeField] private TMP_Text currentCharacterSign;
        private readonly BattleMenuEnemy enemy = new BattleMenuEnemy();
        private readonly BattleMenuPlayer player = new BattleMenuPlayer();
        private readonly List<Character> targetsForEnemyPool = new List<Character>();
        private readonly List<Character> targetsForPlayerPool = new List<Character>();
        private List<Character> battleQueue = new List<Character>();
        private GameManager gm;
        private int turnCounter;
        
        [SerializeField] private GameObject targetIndicator;
        [SerializeField] private GameObject abilityIndicator;

        private void Start()
        {
            gm = FindObjectOfType<GameManager>();
            targetsForPlayerPool.AddRange(enemyCharacters);
            targetsForEnemyPool.AddRange(playerCharacters);

            CreateQueue();
            StartCoroutine(PlayBattle());
            ToggleSkillButtonsVisibility(false);
        }

        private bool CheckIfAnySideWon()
        {
            // returns `true` if any player character is alive while all enemies are dead OR if all player characters are dead while any enemy is alive
            return (playerCharacters.Any(character => character.health > 0) && enemyCharacters.All(character => character.health <= 0))
                   || (playerCharacters.All(character => character.health <= 0) && enemyCharacters.Any(character => character.health > 0));
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

            Debug.Log("Game end!");
            StartCoroutine(GameEnd());
        }

        private IEnumerator MakeTurn()
        {
            turnCounter++;
            turnCounterText.text = "Turn: " + turnCounter;

            // using `.ToList()` here to avoid "Collection was modified; enumeration operation may not execute." error
            // https://stackoverflow.com/a/27851493
            foreach (var character in battleQueue.ToList())
            {
                if (CheckIfAnySideWon()) break;
                UpdateSelectedAbilityText();
                UpdateSelectedTarget();
                if (character.isDead)
                {
                    if (character.isOwnedByPlayer)
                        targetsForEnemyPool.Remove(character);
                    else
                        targetsForPlayerPool.Remove(character);
                    battleQueue.Remove(character);
                    continue;
                }
                
                Debug.Log($"{character.characterName} turn!");
                currentCharacterSign.text = $"It's currently: {character.characterName} turn!";
                
                if (character.isOwnedByPlayer)
                {
                    ToggleSkillButtonsVisibility(true);
                    UpdateSkillButtons(character);
                    gm.stateController.fsm.ChangeState(StateController.States.PlayerTurn);
                    // Wait until player does his turn and then continue
                    yield return new WaitUntil(() =>
                        gm.stateController.fsm.State == StateController.States.PlayerFinalizedHisMove);
                    ToggleSkillButtonsVisibility(false);
                    player.MakeAttack(character, playerSelectedTarget, playerSelectedAbility);
                    playerSelectedAbility = null;
                    playerSelectedTarget = null;
                }
                else
                {
                    gm.stateController.fsm.ChangeState(StateController.States.EnemyTurn);
                    yield return new WaitForSecondsRealtime(1f);
                    var randomTargetIndex = Random.Range(0, targetsForEnemyPool.Count);
                    enemy.MakeAttack(character, targetsForEnemyPool[randomTargetIndex]);
                    yield return new WaitForSecondsRealtime(1f);
                }
            }
            DisableSelectionIndicators();
            gm.stateController.fsm.ChangeState(StateController.States.ReadyForNextTurn);
        }

        public void EndPlayerTurn()
        {
            if 
            (
                // if player is in selecting target state
                gm.stateController.fsm.State == StateController.States.SelectingTarget &&
                // and he selected both a target and an ability
                (playerSelectedAbility != null && playerSelectedTarget != null) &&
                // and the target is either a targetable foe
                ((targetsForPlayerPool.Contains(playerSelectedTarget) ||
                // OR an ally WHEN the ability can target allies
                (playerSelectedTarget.isOwnedByPlayer && playerSelectedAbility.canTargetAllies)))
            )
            {
                gm.stateController.fsm.ChangeState(StateController.States.PlayerFinalizedHisMove);
            }
            else
            {
                Debug.Log("Player didn't correctly end turn! (No ability and/or target chosen!)");
            }
        }

        public void StartTargetSelectionState()
        {
            gm.stateController.fsm.ChangeState(StateController.States.SelectingTarget);
        }

        private void UpdateSkillButtons(Character currentCharacter)
        {
            Debug.Log($"Updating ability info");
            for (var i = 0; i <= 3; i++)
            {
                skillButtons[i].GetComponent<DisplayAbilityData>().ability = currentCharacter.abilities[i];
                skillButtons[i].GetComponent<DisplayAbilityData>().UpdateAbilityDisplay();
            }
        }

        private void ToggleSkillButtonsVisibility(bool enable)
        {
            Debug.Log($"Setting visibility of abilities to {enable}");
            for (var i = 0; i <= 3; i++)
            {
                skillButtons[i].gameObject.SetActive(enable);
            }
        }

        public void UpdateSelectedAbilityText()
        {
            selectedAbilitySign.text = playerSelectedAbility != null
                ? $"Selected ability: {playerSelectedAbility.abilityName}"
                : "Selected ability: none";
        }

        public void UpdateSelectedTarget()
        {
            selectedTargetSign.text = playerSelectedTarget != null
                ? $"Selected target: {playerSelectedTarget.characterName}"
                : "Selected target: none";
        }

        public void DisableSelectionIndicators()
        {
            targetIndicator.SetActive(false);
            abilityIndicator.SetActive(false);
        }

        private IEnumerator GameEnd()
        {
            Debug.Log("Game ended!");
            gm.stateController.fsm.ChangeState(StateController.States.Playing);
            yield return new WaitForSecondsRealtime(2f);
            SceneManager.LoadScene(1);
        }
    }
}