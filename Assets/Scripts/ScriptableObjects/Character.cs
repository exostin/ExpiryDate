using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New character", menuName = "Character")]
    public class Character : ScriptableObject
    {
        public string characterName;
        public int health;
        public int maxHealth;
        public int initiative;
        public Ability[] abilities;
        public bool isOwnedByPlayer;
        public bool isDead;
        public Sprite artwork;
    }
}