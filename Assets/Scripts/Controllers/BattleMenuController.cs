using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Classes;
using Unity.Mathematics;
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
            
            // Array.Sort(battleQueue); ???
            // Sort the battleQueue array descending by the character.Statistics.Initiative value
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
                    // TODO: Need to implement strategic character choice by AI instead of random from 1 to 4
                    enemy.MakeMove(character, player.Characters[Random.Range(1,4)]);
                }
            }
        }
        
        
    }
}
