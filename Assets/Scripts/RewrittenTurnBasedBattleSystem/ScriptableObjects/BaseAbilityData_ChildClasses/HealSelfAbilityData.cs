using UnityEngine;

namespace RewrittenTurnBasedBattleSystem.ScriptableObjects.BaseAbilityData_ChildClasses
{
    [CreateAssetMenu(fileName = "HealSelfAbility", menuName = "Create Ability/Self/Heal")]
    public class HealSelfAbilityData : BaseAbilityData
    {
        [Header("Math side:")]
        public int minHealAmount;
        public int maxHealAmount;
    }
}