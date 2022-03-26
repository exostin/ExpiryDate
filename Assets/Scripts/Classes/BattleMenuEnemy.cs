using ScriptableObjects;
using UnityEngine;

namespace Classes
{
    public class BattleMenuEnemy
    {
        public void MakeAttack(Character characterUsedForAttack, Character target)
        {
            var randomAbilityIndex = Random.Range(0, characterUsedForAttack.abilities.Length);
            target.health -= characterUsedForAttack.abilities[randomAbilityIndex].damage;
            Debug.Log($"{characterUsedForAttack.name} has dealt {characterUsedForAttack.abilities[randomAbilityIndex].damage} damage to {target.name}!");
        }
    }
}