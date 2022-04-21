using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New character", menuName = "Character")]
    public class Character : ScriptableObject
    {
        public string characterName;
        public int maxHealth;
        public int initiative;
        public bool isOwnedByPlayer;
        public Sprite artwork;
        public Ability[] abilities;

        public int costTitan;
        public int costWater;
        public int costFood;
        public int costEnergy;

        [HideInInspector] public int health;
        [HideInInspector] public bool isDead;

        public void Initialize()
        {
            health = maxHealth;
            isDead = false;
        }
    }
}