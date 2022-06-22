using UnityEngine;

namespace RewrittenTurnBasedBattleSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New character", menuName = "Create a character")]
    public class CharacterData : ScriptableObject
    {
        #region General info
        [Header("General info")]
        
        public string characterName;
        public bool isOwnedByPlayer;
        public Sprite artwork;
        [Tooltip("Character's small photo that is shown in the character inspector")]
        public Sprite artworkPortrait;
        public BaseAbilityData[] abilities;
        
        #endregion
        
        #region Character Stats
        [Header("Character Stats")]
        
        public int maxHealth;
        public int maxShield;
        [Tooltip("Value that determines the order of the character in battle queue (higher is faster)")]
        public int baseInitiative;
        [Tooltip("Chance percentage to break out of stun on each turn")]
        public int chanceToBreakOutOfStun;
        public bool invulnerableToBleed = false;

        #endregion
    }
}