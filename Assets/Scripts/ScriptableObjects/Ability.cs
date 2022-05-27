using NaughtyAttributes;
using Other.Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New ability", menuName = "Ability")]
    public class Ability : ScriptableObject
    {
        public string abilityName;
        [TextArea] public string abilityDescription;
        public Sprite artwork;
        public AudioClip soundEffect;
        
        public TargetType abilityTarget;
        public AbilityType abilityType;
        [ShowIf("abilityType", AbilityType.Heal)] public int healAmount;
        [ShowIf("abilityType", AbilityType.Shield)] public int shieldAmount;
        [ShowIf("abilityType", AbilityType.Damage)] public int damageAmount;
        
        [BoxGroup("Status")]
        [ShowIf("abilityType", AbilityType.Status)] public StatusType statusType;

        [BoxGroup("Status")]
        [ShowIf("IsStatusAndBleedIsSelected")] public int bleedDuration;
        
        [BoxGroup("Status")]
        [ShowIf("IsStatusAndBleedIsSelected")] public int bleedDmgAmount;
        
        [BoxGroup("Status")]
        [ShowIf("IsStatusAndStunIsSelected")] public int stunDuration;
        
        public bool IsStatusAndBleedIsSelected()
        {
            return abilityType == AbilityType.Status && statusType == StatusType.Bleed;
        }
        public bool IsStatusAndStunIsSelected()
        {
            return abilityType == AbilityType.Status && statusType == StatusType.Stun;
        }
    }
}