using UnityEngine;

namespace RewrittenTurnBasedBattleSystem.ScriptableObjects.BaseAbilityData_ChildClasses
{
    [CreateAssetMenu(fileName = "HealSingleTargetAbility", menuName = "Create Ability/Single Target/Heal")]
    public class HealSingleTargetAbilityData : BaseAbilityData
    {
        [Header("Math side:")]
        public int minHealAmount;
        public int maxHealAmount;
    }
}