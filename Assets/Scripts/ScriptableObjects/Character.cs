using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New character", menuName = "Character")]
    public class Character : ScriptableObject
    {
        public string characterName = "character name";
        public int maxHealth = 1000;
        public int initiative = 1;
        public bool isOwnedByPlayer = true;
        public Sprite artwork;
        public Ability[] abilities;

        [HideInInspector] public int health;
        [HideInInspector] public bool isDead;
    }
}