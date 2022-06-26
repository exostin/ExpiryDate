using UnityEngine;

namespace RewrittenTurnBasedBattleSystem.ScriptableObjects.BaseAbilityData_ChildClasses
{
    [CreateAssetMenu(fileName = "DamageAndStunSingleTargetAbility", menuName = "Create Ability/Single Target/Damage and stun")]
    public class DamageAndStunSingleTargetAbilityData : BaseAbilityData
    {
        [Header("Math side:")]
        public int minDamageAmount;
        public int maxDamageAmount;
        public int stunDuration;
    }
}