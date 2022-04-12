using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New ability", menuName = "Ability")]
    public class Ability : ScriptableObject
    {
        public string abilityName;
        public int damage;
        public string description;
        public Sprite artwork;
        public AudioClip soundEffect;
        public bool canTargetAllies;
    }
}