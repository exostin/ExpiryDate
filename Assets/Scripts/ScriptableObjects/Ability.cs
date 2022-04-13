using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New ability", menuName = "Ability")]
    public class Ability : ScriptableObject
    {
        public string abilityName = "ability name";
        public string description = "ability description";
        public int damage = 100;
        public int targetCount = 1;
        public bool canOnlyTargetAllies = false;
        public bool usedOnSelf = false;
        public bool buff = false;
        public Sprite artwork;
        public AudioClip soundEffect;
    }
}