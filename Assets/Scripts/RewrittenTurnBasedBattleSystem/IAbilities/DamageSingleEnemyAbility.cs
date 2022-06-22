using RewrittenTurnBasedBattleSystem.ScriptableObjects.AbilityData;
using UnityEngine;

namespace RewrittenTurnBasedBattleSystem.IAbilities
{
    public class DamageSingleEnemyAbility : IAbility
    {
        public DamageSingleEnemyAbility(DamageSingleEnemyAbilityData abilityData)
        {
            this.AbilityData = abilityData;
        }
        public DamageSingleEnemyAbilityData AbilityData { get; set; }
        
        public void Perform(Character target)
        {
            Debug.Log($"Dealt {AbilityData.minDamageAmount} damage to {target}");
        }
    }
}