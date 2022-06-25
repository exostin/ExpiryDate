using RewrittenTurnBasedBattleSystem.ScriptableObjects.AbilityData;
using UnityEngine;

namespace RewrittenTurnBasedBattleSystem.IAbilities
{
    public class DamageSingleEnemyAbility : IAbility
    {
        public DamageSingleEnemyAbilityData AbilityData { get; set; }
        
        public DamageSingleEnemyAbility(DamageSingleEnemyAbilityData abilityData)
        {
            this.AbilityData = abilityData;
        }
        
        public void Perform(Character target)
        {
            int damageToDeal = Random.Range(AbilityData.minDamageAmount, AbilityData.maxDamageAmount);
            target.TakeDamage(damageToDeal);
            Debug.Log($"Dealt {damageToDeal} damage to {target.CharacterData.characterName}, {target.Health}/{target.CharacterData.maxHealth} health remaining");
        }
    }
}