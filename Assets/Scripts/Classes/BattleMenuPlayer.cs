using ScriptableObjects;
using UnityEngine;

namespace Classes
{
    public class BattleMenuPlayer
    {
        public void MakeAttack(Character characterUsedForAttack, Character target, Ability selectedAbility)
        {
            if (target.health - selectedAbility.damage <= 0)
            {
                target.health = 0;
                target.isDead = true;
            }
            else if (target.health - selectedAbility.damage > target.maxHealth)
            {
                target.health = target.maxHealth;
            }
            else
            {
                target.health -= selectedAbility.damage;
            }

            Debug.Log($"{characterUsedForAttack.name} has dealt {selectedAbility.damage} damage to {target.name}!");
        }
    }
}