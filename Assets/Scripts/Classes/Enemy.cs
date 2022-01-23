using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Classes
{
    public class Enemy : Player
    {
        public void MakeAttack(Character characterUsedForAttack, Character target)
        {
            // TODO: Need to implement strategic ability choice by AI instead of randomAbilityIndex
            var randomAbilityIndex = Random.Range(1, 4);
            target.Statistics.Health -= characterUsedForAttack.Abilities[randomAbilityIndex].Damage;
        }
    }
}