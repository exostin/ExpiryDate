using System.Collections.Generic;
using RewrittenTurnBasedBattleSystem.ScriptableObjects.BaseAbilityData_ChildClasses;
using UnityEngine;

namespace RewrittenTurnBasedBattleSystem.IAbilities
{
    public class DamageMultipleEnemyAbility : IAbility
    {
        private DamageMultipleEnemyAbilityData AbilityData { get;}
        
        public DamageMultipleEnemyAbility(DamageMultipleEnemyAbilityData abilityData)
        {
            this.AbilityData = abilityData;
        }
        
        public void Perform(Team casterTeam, Team enemyTeam, Character selectedTarget)
        {
            foreach (Character character in enemyTeam.characters)
            {
                var damageToDeal = Random.Range(AbilityData.minDamageAmount, AbilityData.maxDamageAmount);
                Debug.Log($"Used {AbilityData.name} on {character.CharacterData.characterName}");
                character.TakeDamage(damageToDeal);
            }
        }
    }
}