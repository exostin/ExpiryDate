using System.Collections.Generic;
using RewrittenTurnBasedBattleSystem.ScriptableObjects.BaseAbilityData_ChildClasses;
using UnityEngine;

namespace RewrittenTurnBasedBattleSystem.IAbilities
{
    public class DamageSingleEnemyAbility : IAbility
    {
        private DamageSingleEnemyAbilityData AbilityData { get;}
        
        public DamageSingleEnemyAbility(DamageSingleEnemyAbilityData abilityData)
        {
            this.AbilityData = abilityData;
        }
        
        public void Perform(Team casterTeam, Team enemyTeam, Character selectedTarget)
        {
            var damageToDeal = Random.Range(AbilityData.minDamageAmount, AbilityData.maxDamageAmount);
            Debug.Log($"Used {AbilityData.name} on {selectedTarget.CharacterData.characterName}");
            selectedTarget.TakeDamage(damageToDeal);
        }
    }
}