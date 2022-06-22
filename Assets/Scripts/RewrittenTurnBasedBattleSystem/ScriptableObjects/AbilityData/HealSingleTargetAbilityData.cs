using UnityEngine;

namespace RewrittenTurnBasedBattleSystem.ScriptableObjects.AbilityTypes
{
    [CreateAssetMenu(fileName = "HealSingleTargetAbility", menuName = "Create Ability/Single Target/Heal")]
    public class HealSingleTargetAbilityData : BaseAbilityData
    {
        public int minHealAmount;
        public int maxHealAmount;
    }
}