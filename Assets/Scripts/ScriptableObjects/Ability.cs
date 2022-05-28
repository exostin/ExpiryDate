using NaughtyAttributes;
using Other.Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New ability", menuName = "Ability")]
    public class Ability : ScriptableObject
    {
        [Header("Audio-visual side")]
        public string abilityName;
        [TextArea] public string abilityDescription;
        public Sprite artwork;
        public AudioClip soundEffect;
        
        [Header("Logical side")]
        public TargetType abilityTarget;
        public AbilityType abilityType;
        [ShowIf("abilityType", AbilityType.Heal)] public int healAmount;
        [ShowIf("abilityType", AbilityType.Shield)] public int shieldAmount;
        [BoxGroup("Status")]
        [ShowIf("abilityType", AbilityType.Status)] public StatusType statusType;
        
        [ShowIf("IsStunOrBleedOrDamage")] public int damageAmount;
        
        [BoxGroup("Status")]
        [ShowIf("IsStatusAndBleedIsSelected")] public int bleedDuration;
        
        [BoxGroup("Status")]
        [ShowIf("IsStatusAndBleedIsSelected")] public int bleedDmgAmount;
        
        [BoxGroup("Status")]
        [ShowIf("IsStatusAndStunIsSelected")] public int stunDuration;

        private bool IsStatusAndBleedIsSelected()
        {
            return abilityType == AbilityType.Status && statusType == StatusType.Bleed;
        }

        private bool IsStatusAndStunIsSelected()
        {
            return abilityType == AbilityType.Status && statusType == StatusType.Stun;
        }
        private bool IsStunOrBleedOrDamage()
        {
            return abilityType == AbilityType.DamageOnly || IsStatusAndBleedIsSelected() || IsStatusAndStunIsSelected();
        }
    }
}