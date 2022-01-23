using System;
using System.Linq;
using Classes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class BattleMenuController : MonoBehaviour
    {
        private Player player;
        private Enemy enemy;
        private Character[] battleQueue;

        private void Start()
        {
            CreateQueue();
        }

        private void Update()
        {
            MakeTurn();
        }

        private void CreateQueue()
        {
            var i = 0;
            foreach (var character in player.Characters)
            {
                battleQueue[i] = character;
                character.IsOwnedByPlayer = true;
                i++;
            }
            
            i = 0;
            foreach (var character in enemy.Characters)
            {
                battleQueue[i] = character;
                i++;
            }

            // Sort the battle queue
            battleQueue = battleQueue.OrderByDescending(c => c.Statistics.Initiative).ToArray();
        }
        private void MakeTurn()
        {
            foreach (var character in battleQueue)
            {
                if (character.IsOwnedByPlayer)
                {
                    // TODO: Wait until player does his turn, then continue (State machine?)
                }
                else
                {
                    var randomTargetIndex = Random.Range(0, 3);
                    enemy.MakeAttack(characterUsedForAttack:character, player.Characters[randomTargetIndex]);
                }
            }
        }
        
        
    }
}
