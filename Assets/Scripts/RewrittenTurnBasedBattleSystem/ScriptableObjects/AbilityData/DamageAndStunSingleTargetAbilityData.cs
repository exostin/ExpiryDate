using UnityEngine;

namespace RewrittenTurnBasedBattleSystem.ScriptableObjects.AbilityTypes
{
    [CreateAssetMenu(fileName = "DamageAndStunSingleTargetAbility", menuName = "Create Ability/Single Target/Damage and stun")]
    public class DamageAndStunSingleTargetAbilityData : BaseAbilityData
    {
        public int minDamageAmount;
        public int maxDamageAmount;
        public int stunDuration;
    }
}