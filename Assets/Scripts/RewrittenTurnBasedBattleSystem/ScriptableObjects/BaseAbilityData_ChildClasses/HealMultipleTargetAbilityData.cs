using UnityEngine;

namespace RewrittenTurnBasedBattleSystem.ScriptableObjects.BaseAbilityData_ChildClasses
{
    [CreateAssetMenu(fileName = "HealMultipleTargetAbility", menuName = "Create Ability/Multiple Target/Heal")]
    public class HealMultipleTargetAbilityData : BaseAbilityData
    {
        [Header("Math side:")]
        public int minHealAmount;
        public int maxHealAmount;
    }
}