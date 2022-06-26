using UnityEngine;

namespace RewrittenTurnBasedBattleSystem.ScriptableObjects.BaseAbilityData_ChildClasses
{
    [CreateAssetMenu(fileName = "DamageSingleEnemyAbility", menuName = "Create Ability/Single Target/Damage")]
    public class DamageSingleEnemyAbilityData : BaseAbilityData
    {
        [Header("Math side:")]
        public int minDamageAmount;
        public int maxDamageAmount;
    }
}