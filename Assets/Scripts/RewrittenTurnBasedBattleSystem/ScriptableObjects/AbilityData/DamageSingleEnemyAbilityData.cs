using UnityEngine;

namespace RewrittenTurnBasedBattleSystem.ScriptableObjects.AbilityData
{
    [CreateAssetMenu(fileName = "DamageSingleEnemyAbility", menuName = "Create Ability/Single Target/Damage")]
    public class DamageSingleEnemyAbilityData : BaseAbilityData
    {
        public int minDamageAmount;
        public int maxDamageAmount;
    }
}