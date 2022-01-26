using System;
using System.Linq;
using Classes;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class BattleMenuController : MonoBehaviour
    {
        public Character[] playerCharacters;
        public Character[] enemyCharacters;
        private Enemy enemy = new Enemy();
        private Character[] battleQueue;

        private void Start()
        {
            battleQueue = new Character[8];
            CreateQueue();
            while (CheckIfAnyoneAlive())
            {
                MakeTurn();
            }
        }

        private bool CheckIfAnyoneAlive()
        {
            // returns `true` if any character's health is over 0, and `false` if there are none
            return battleQueue.Any(character => character.health > 0);
        }

        private void CreateQueue()
        {
            var i = 0;
            foreach (var character in playerCharacters)
            {
                battleQueue[i] = character;
                i++;
            }
            foreach (var character in enemyCharacters)
            {
                battleQueue[i] = character;
                i++;
            }

            // Sort the battle queue by initiative
            battleQueue = battleQueue.OrderByDescending(character => character.initiative).ToArray();
        }
        private void MakeTurn()
        {
            foreach (var character in battleQueue)
            {
                if (character.health > 0) continue;
                if (character.isOwnedByPlayer)
                {
                    // TODO: Wait until player does his turn, then continue (State machine?)
                    
                    // --- TEMPORARY
                    var randomTargetIndex = Random.Range(0, 3);
                    enemy.MakeAttack(characterUsedForAttack:character, target:enemyCharacters[randomTargetIndex]);
                    // ---
                }
                else
                {
                    var randomTargetIndex = Random.Range(0, 3);
                    enemy.MakeAttack(characterUsedForAttack:character, target:playerCharacters[randomTargetIndex]);
                }
            }
        }
    }
}
