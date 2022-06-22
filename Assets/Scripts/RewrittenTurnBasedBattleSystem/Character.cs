using System.Collections.Generic;
using System.Data.Linq;
using Other.Enums;
using RewrittenTurnBasedBattleSystem.ScriptableObjects;
using UnityEngine;

namespace RewrittenTurnBasedBattleSystem
{
    public class Character
    {
        public Character(IAbility[] abilities, CharacterData data)
        {
            this.Abilities = abilities;
            this.CharacterData = data;
        }
        
        public int Health { get; set; }
        public int ShieldPoints { get; set; }
        public bool IsDead { get; set; }
        public IAbility[] Abilities { get; set; }
        public int Initiative { get; set; }
        
        #region Statuses
        
        public List<StatusType> currentlyAppliedStatuses;
        public int BleedDurationLeft { get; set; }
        public int CumulatedBleedDmg { get; set; }
        public bool DodgeEverythingUntilNextTurn { get; set; }
        public int StunDurationLeft { get; set; }

        #endregion
        
        public CharacterData CharacterData { get; set; }
        public delegate void CharacterEvent();
        public static event CharacterEvent OnCharacterDeath;
        
        public void Initialize()
        {
            Health = CharacterData.maxHealth;
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