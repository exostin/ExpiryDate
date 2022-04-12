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
            
            if (target.health - characterUsedForAttack.abilities[randomAbilityIndex].damage <= 0)
            {
                target.health = 0;
                target.isDead = true;
            }
            else
            {
                target.health -= characterUsedForAttack.abilities[randomAbilityIndex].damage;
            }
            
            Debug.Log(
                $"{characterUsedForAttack.name} has dealt {characterUsedForAttack.abilities[randomAbilityIndex].damage} damage to {target.name}!");
        }
    }
}