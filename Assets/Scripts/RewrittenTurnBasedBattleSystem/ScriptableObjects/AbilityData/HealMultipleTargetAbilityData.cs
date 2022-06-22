using UnityEngine;

namespace RewrittenTurnBasedBattleSystem.ScriptableObjects.AbilityTypes
{
    [CreateAssetMenu(fileName = "HealMultipleTargetAbility", menuName = "Create Ability/Multiple Target/Heal")]
    public class HealMultipleTargetAbilityData : BaseAbilityData
    {
        public int minHealAmount;
        public int maxHealAmount;
    }
}