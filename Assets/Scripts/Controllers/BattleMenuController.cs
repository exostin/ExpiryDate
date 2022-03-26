using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using ScriptableObjects;
using TMPro;
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

        private int turnCounter = 0;
        [SerializeField] private TMP_Text turnCounterText;

        private GameManager gm;
        
        private void Start()
        {
            gm = FindObjectOfType<GameManager>();
            targetsForPlayerPool.AddRange(enemyCharacters);
            targetsForEnemyPool.AddRange(playerCharacters);
            
            CreateQueue();
            StartCoroutine(PlayBattle());
        }

        void Update()
        {
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
                    gm.stateController.fsm.ChangeState(StateController.States.PlayerTurn);
                    Debug.Log($"{character.characterName} turn!");
                    // Wait until player does his turn and then continue
                    yield return new WaitUntil(() => gm.stateController.fsm.State == StateController.States.EnemyTurn);
                    
                    // // --- TEMPORARY
                    // var randomTargetIndex = Random.Range(0, targetsForPlayerPool.Count);
                    // enemy.MakeAttack(characterUsedForAttack:character, target:targetsForPlayerPool[randomTargetIndex]);
                    // // ---
                }
                else
                {
                    var randomTargetIndex = Random.Range(0, targetsForEnemyPool.Count);
                    enemy.MakeAttack(characterUsedForAttack:character, target:targetsForEnemyPool[randomTargetIndex]);
                    yield return new WaitForSecondsRealtime(0.5f);
                }
            }
            gm.stateController.fsm.ChangeState(StateController.States.ReadyForNextTurn);
        }

        public void EndPlayerTurn()
        {
            gm.stateController.fsm.ChangeState(StateController.States.EnemyTurn);
        }
    }
}
