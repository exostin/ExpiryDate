using UnityEngine;

namespace RewrittenTurnBasedBattleSystem.ScriptableObjects.BaseAbilityData_ChildClasses
{
    [CreateAssetMenu(fileName = "DamageMultipleEnemyAbility", menuName = "Create Ability/Multiple Target/Damage")]
    public class DamageMultipleEnemyAbilityData : BaseAbilityData
    {
        [Header("Math side:")]
        public int minDamageAmount;
        public int maxDamageAmount;
    }
}