using UnityEngine;

namespace RewrittenTurnBasedBattleSystem.ScriptableObjects.AbilityTypes
{
    [CreateAssetMenu(fileName = "HealSelfAbility", menuName = "Create Ability/Self/Heal")]
    public class HealSelfAbilityData : BaseAbilityData
    {
        public int minHealAmount;
        public int maxHealAmount;
    }
}