using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Classes
{
    public class Enemy : Player
    {
        public void MakeMove(Character usedCharacter, Character target)
        {
            // TODO: Need to implement strategic ability choice by AI instead of randomAbilityIndex
            var randomAbilityIndex = Random.Range(1, 4);
            target.Statistics.Health -= usedCharacter.Abilities[randomAbilityIndex].Damage;
        }
    }
}