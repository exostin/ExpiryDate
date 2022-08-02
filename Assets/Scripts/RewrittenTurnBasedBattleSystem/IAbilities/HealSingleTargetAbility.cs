using System.Collections.Generic;
using RewrittenTurnBasedBattleSystem.ScriptableObjects.BaseAbilityData_ChildClasses;
using UnityEngine;

namespace RewrittenTurnBasedBattleSystem.IAbilities
{
    public class HealSingleTargetAbility : IAbility
    {
        private HealSingleTargetAbilityData AbilityData { get;}
        
        public HealSingleTargetAbility(HealSingleTargetAbilityData abilityData)
        {
            this.AbilityData = abilityData;
        }
        
        public void Perform(Team casterTeam, Team enemyTeam, Character selectedTarget)
        {
            var healAmount = Random.Range(AbilityData.minHealAmount, AbilityData.maxHealAmount);
            Debug.Log($"Used {AbilityData.name} on {selectedTarget.CharacterData.characterName}");
            selectedTarget.Heal(healAmount);
        }
    }
}