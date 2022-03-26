using ScriptableObjects;
using UnityEngine;

namespace Classes
{
    public class BattleMenuPlayer
    {
        public void MakeAttack(Character characterUsedForAttack, Character target, Ability selectedAbility)
        {
            target.health -= selectedAbility.damage;
            Debug.Log($"{characterUsedForAttack.name} has dealt {selectedAbility.damage} damage to {target.name}!");
        }
    }
}