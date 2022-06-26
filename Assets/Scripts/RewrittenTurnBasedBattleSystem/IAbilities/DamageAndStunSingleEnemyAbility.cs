using System.Collections.Generic;
using Other.Enums;
using RewrittenTurnBasedBattleSystem.ScriptableObjects.BaseAbilityData_ChildClasses;
using UnityEngine;

namespace RewrittenTurnBasedBattleSystem.IAbilities
{
    public class DamageAndStunSingleEnemyAbility : IAbility
    {
        private DamageAndStunSingleTargetAbilityData AbilityData { get;}
        
        public DamageAndStunSingleEnemyAbility(DamageAndStunSingleTargetAbilityData abilityData)
        {
            this.AbilityData = abilityData;
        }
        
        public void Perform(Team casterTeam, Team enemyTeam, Character selectedTarget)
        {
            var damageToDeal = Random.Range(AbilityData.minDamageAmount, AbilityData.maxDamageAmount);
            Debug.Log($"Used {AbilityData.name} on {selectedTarget.CharacterData.characterName}");
            selectedTarget.TakeDamage(damageToDeal);
            selectedTarget.ApplyStun(AbilityData.stunDuration);
        }
    }
}