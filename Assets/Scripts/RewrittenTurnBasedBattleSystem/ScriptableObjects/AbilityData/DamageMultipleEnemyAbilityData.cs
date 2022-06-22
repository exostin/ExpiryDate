using UnityEngine;

namespace RewrittenTurnBasedBattleSystem.ScriptableObjects.AbilityTypes
{
    [CreateAssetMenu(fileName = "DamageMultipleEnemyAbility", menuName = "Create Ability/Multiple Target/Damage")]
    public class DamageMultipleEnemyAbilityData : BaseAbilityData
    {
        public int minDamageAmount;
        public int maxDamageAmount;
    }
}