using ScriptableObjects;
using UnityEngine;
using System.Collections.Generic;

namespace Classes
{
    public class BattleMenuEnemy : BattleMenuActions
    {
        // public void MakeAttack(Character characterUsedForAttack, Character target, List<Character> allTargetsList)
        // {
        //     var randomAbilityIndex = Random.Range(0, characterUsedForAttack.abilities.Length);
        //     var abilityUsed = characterUsedForAttack.abilities[randomAbilityIndex];
        //     
        //     if (abilityUsed.targetsAll)
        //     {
        //         foreach (var character in allTargetsList)
        //         {
        //             DealDamage(characterUsedForAttack, character);
        //         }
        //     }
        //     else
        //     {
        //         DealDamage(characterUsedForAttack, target);
        //     }
        // }
        //
        // public void DealDamage(Character characterUsedForAttack, Character target)
        // {
        //     if (target.health - abilityUsed.damage <= 0)
        //     {
        //         target.health = 0;
        //         target.isDead = true;
        //     }
        //     else
        //     {
        //         target.health -= characterUsedForAttack.abilities[randomAbilityIndex].damage;
        //     }
        //
        //     Debug.Log(
        //         $"{characterUsedForAttack.name} has dealt {characterUsedForAttack.abilities[randomAbilityIndex].damage} damage to {target.name}!");
        // }
    }
}