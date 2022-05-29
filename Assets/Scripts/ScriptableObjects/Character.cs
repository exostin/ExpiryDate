using System.Collections.Generic;
using Other.Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New character", menuName = "Character")]
    public class Character : ScriptableObject
    {
        public string characterName;
        public bool isOwnedByPlayer;
        public Sprite artwork;
        [Tooltip("Character's small photo that is shown in the character inspector")]
        public Sprite artworkPortrait;
        public Ability[] abilities;

        #region Character Stats

        [Header("Character Stats")]
        public int maxHealth;
        public int maxShield;
        [Tooltip("Value that determines the order of the character in battle queue (higher is faster)")]
        public int initiative;

        [Tooltip("Chance percentage to break out of stun on each turn")]
        public int chanceToBreakOutOfStun;

        #endregion
        
        public int Health { get; set; }
        public bool IsDead { get; set; }

        [HideInInspector] public List<StatusType> currentlyAppliedStatuses;
        
        public int ShieldPoints { get; set; }

        public int BleedDurationLeft { get; set; }

        public int CumulatedBleedDmg { get; set; }
        public bool DodgeEverythingUntilNextTurn { get; set; }
        public int StunDurationLeft { get; set; }
        
        [Header("Citybuilding")]
        public int costTitan;
        public int costWater;
        public int costFood;
        public int costEnergy;
        
        public void Initialize()
        {
            Health = maxHealth;
            IsDead = false;
            currentlyAppliedStatuses = new List<StatusType>();
            ShieldPoints = 0;
            BleedDurationLeft = 0;
            CumulatedBleedDmg = 0;
            DodgeEverythingUntilNextTurn = false;
            StunDurationLeft = 0;
        }
    }
}