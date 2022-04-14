using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New ability", menuName = "Ability")]
    public class Ability : ScriptableObject
    {
        public string abilityName;
        public string description;
        public int damage;
        public bool targetsWholeTeam;
        public bool canOnlyTargetOwnCharacters;
        public bool usedOnlyOnSelf;
        public bool buff;
        public bool heal;
        public Sprite artwork;
        public AudioClip soundEffect;
    }
}