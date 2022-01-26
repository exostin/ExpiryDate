using ScriptableObjects;
using UnityEngine;

namespace Classes
{
    public class Enemy
    {
        public void MakeAttack(Character characterUsedForAttack, Character target)
        {
            var randomAbilityIndex = Random.Range(0, 3);
            target.health -= characterUsedForAttack.abilities[randomAbilityIndex].damage;
        }
    }
}