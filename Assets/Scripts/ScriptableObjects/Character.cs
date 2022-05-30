using System.Collections.Generic;
using Other.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New character", menuName = "Character")]
    public class Character : ScriptableObject
    {
        #region General info
        [Header("General info")]
        
        public string characterName;
        public bool isOwnedByPlayer;
        public Sprite artwork;
        [Tooltip("Character's small photo that is shown in the character inspector")]
        public Sprite artworkPortrait;
        public Ability[] abilities;
        
        #endregion
        
        #region Character Stats
        [Header("Character Stats")]
        
        public int maxHealth;
        public int maxShield;
        [Tooltip("Value that determines the order of the character in battle queue (higher is faster)")]
        public int initiative;
        [Tooltip("Chance percentage to break out of stun on each turn")]
        public int chanceToBreakOutOfStun;

        #endregion

        #region Current game character data

        public int Health { get; set; }
        public bool IsDead { get; set; }
        public int ShieldPoints { get; set; }
        
        #region Statuses
        
        [HideInInspector] public List<StatusType> currentlyAppliedStatuses;
        public int BleedDurationLeft { get; set; }
        public int CumulatedBleedDmg { get; set; }
        public bool DodgeEverythingUntilNextTurn { get; set; }
        public int StunDurationLeft { get; set; }

        #endregion
        #endregion

        public delegate void CharacterEvent();
        public static event CharacterEvent OnCharacterDeath;

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

        public bool CheckIfDead()
        {
            if (Health > 0) return false;
            IsDead = true;
            Health = 0;
            ShieldPoints = 0;
            OnCharacterDeath?.Invoke();
            ClearAllStatuses();
            return true;
        }

        private void ClearAllStatuses()
        {
            currentlyAppliedStatuses.Clear();
            BleedDurationLeft = 0;
            CumulatedBleedDmg = 0;
            DodgeEverythingUntilNextTurn = false;
            StunDurationLeft = 0;
        }
    }
}