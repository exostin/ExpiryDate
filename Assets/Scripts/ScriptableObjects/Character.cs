using System.Collections.Generic;
using Other.Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New character", menuName = "Character")]
    public class Character : ScriptableObject
    {
        public string characterName;
        public int maxHealth;
        public int maxShield;
        public int initiative;
        public bool isOwnedByPlayer;
        public Sprite artwork;
        public Ability[] abilities;
        
        public int Health { get; set; }
        public bool IsDead { get; set; }

        [HideInInspector] public List<StatusType> currentlyAppliedStatuses;
        
        public int ShieldPoints { get; set; }

        public int BleedDurationLeft { get; set; }

        public int CumulatedBleedDmg { get; set; }
        public bool DodgeEverythingUntilNextTurn { get; set; }
        public int StunnedDurationLeft { get; set; }
        
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
            StunnedDurationLeft = 0;
        }
    }
}