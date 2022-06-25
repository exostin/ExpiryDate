using System;
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
        public static event Action OnCharacterDeath;
        
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

        private bool CheckIfDead()
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

        public void TakeDamage(int damageToDeal)
        {
            if ((Health + ShieldPoints) - damageToDeal <= 0)
            {
                Health = 0;
                CheckIfDead();
            }
            else
            {
                if (ShieldPoints - damageToDeal < 0)
                {
                    ShieldPoints = 0;
                    Health -= damageToDeal - ShieldPoints;
                }
                else
                {
                    ShieldPoints -= damageToDeal;
                }
            }
            Debug.Log($"Dealt {damageToDeal} damage to {CharacterData.characterName}!");
        }
    }
}