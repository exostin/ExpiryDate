using System;
using System.Collections.Generic;
using System.Linq;
using Controllers.BattleScene;
using Other.Enums;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Classes
{
    public class BattleActions : MonoBehaviour
    {
        private NotificationsHandler notificationHandlerReference;
        private int finalDamageAmount;
        private int finalHealAmount;

        public delegate void OnStatusApplied();
        public static event OnStatusApplied OnBleedApplied;
        public static event OnStatusApplied OnStunApplied;
        public static event OnStatusApplied OnDodgeApplied;
        public static event OnStatusApplied OnHealAppliedToCureBleed;
        
        private BattleController battleController;

        private void Start()
        {
            battleController = FindObjectOfType<BattleController>();
        }

        /// <summary>
        ///     Deploy a chosen action - damage/heal/buff - onto a chosen target
        /// </summary>
        public void MakeAction(Character target, Ability selectedAbility,
            IEnumerable<Character> allCharacters, bool isPlayerTurn)
        {
            var finalAttackTargets = new List<Character>();
            finalAttackTargets.AddRange(allCharacters.Where(x => !x.IsDead).ToList());
            finalAttackTargets.RemoveAll(x => x.DodgeEverythingUntilNextTurn);
            
            if (selectedAbility.abilityType is AbilityType.DamageOnly ||
                selectedAbility.statusType is StatusType.Bleed or StatusType.Stun)
            {
                finalDamageAmount = Random.Range(selectedAbility.minDamageAmount, selectedAbility.maxDamageAmount);
            }
            
            // If the ability should target only own team
            if (selectedAbility.abilityTarget is TargetType.SingleTeammate or TargetType.MultipleTeammates)
            {
                if (isPlayerTurn) RemoveAllEnemyCharactersFromTargets(finalAttackTargets);
                else RemoveAllPlayerCharactersFromTargets(finalAttackTargets);
            }
            else
            {
                if (isPlayerTurn) RemoveAllPlayerCharactersFromTargets(finalAttackTargets);
                else RemoveAllEnemyCharactersFromTargets(finalAttackTargets);
            }

            if (selectedAbility.abilityTarget is TargetType.MultipleEnemies or TargetType.MultipleTeammates)
            {
                foreach (Character thisIterationTarget in finalAttackTargets)
                {
                    AssignNotificationHandlerReference(battleController.FindCharactersGameObjectByName(thisIterationTarget));
                    switch (selectedAbility.abilityType)
                    {
                        case AbilityType.Status:
                            Deal(thisIterationTarget,selectedAbility);
                            ApplyStatus(thisIterationTarget, selectedAbility);
                            break;
                        case AbilityType.DamageOnly:
                            Deal(thisIterationTarget, selectedAbility);
                            break;
                        case AbilityType.Heal:
                            finalHealAmount = Random.Range(selectedAbility.minHealAmount, selectedAbility.maxHealAmount);
                            Heal(thisIterationTarget, selectedAbility);
                            break;
                        case AbilityType.Shield:
                            Shield(thisIterationTarget, selectedAbility);
                            break;
                        default:
                            Debug.LogError("Ability type not found");
                            break;
                    }
                }
            }
            else
            {
                AssignNotificationHandlerReference(battleController.FindCharactersGameObjectByName(target));
                switch (selectedAbility.abilityType)
                {
                    case AbilityType.Status:
                        Deal(target,selectedAbility);
                        ApplyStatus(target, selectedAbility);
                        break;
                    case AbilityType.DamageOnly:
                        Deal(target, selectedAbility);
                        break;
                    case AbilityType.Heal:
                        Heal(target, selectedAbility);
                        break;
                    case AbilityType.Shield:
                        Shield(target, selectedAbility);
                        break;
                    default:
                        Debug.LogError("Ability type not found");
                        break;
                }
            }
        }

        /// <summary>
        /// This is so wrong my eyes bleed, but we have two days to complete the game... sorry future me and anyone who sees this
        /// </summary>
        /// <param name="character">Character GameObject</param>
        private void AssignNotificationHandlerReference(GameObject character)
        {
            notificationHandlerReference = character.GetComponent<NotificationsHandler>();
        }

        private void VisualizeAction(AbilityType abilityType, string value)
        {
            notificationHandlerReference.HandleNotification(abilityType, value);
        }

        private static void RemoveAllEnemyCharactersFromTargets(List<Character> finalAttackTargets)
        {
            for (var i = 0; i < finalAttackTargets.Count; i++)
            {
                if (i >= finalAttackTargets.Count) break;

                if (!finalAttackTargets[i].isOwnedByPlayer)
                {
                    finalAttackTargets.Remove(finalAttackTargets[i]);
                    i--;
                }
            }
        }

        private static void RemoveAllPlayerCharactersFromTargets(List<Character> finalAttackTargets)
        {
            for (var i = 0; i < finalAttackTargets.Count; i++)
            {
                if (i >= finalAttackTargets.Count) break;

                if (finalAttackTargets[i].isOwnedByPlayer)
                {
                    finalAttackTargets.Remove(finalAttackTargets[i]);
                    i--;
                }
            }
        }

        private void Deal(Character target, Ability selectedAbility)
        {
            if ((target.Health + target.ShieldPoints) - finalDamageAmount <= 0)
            {
                target.Health = 0;
                target.CheckIfDead();
            }
            else
            {
                if (target.ShieldPoints - finalDamageAmount < 0)
                {
                    target.ShieldPoints = 0;
                    target.Health -= finalDamageAmount - target.ShieldPoints;
                    target.CheckIfDead();
                }
                else
                {
                    target.ShieldPoints -= finalDamageAmount;
                }
            }

            VisualizeAction(selectedAbility.abilityType, finalDamageAmount.ToString());
            Debug.Log($"Dealt {finalDamageAmount} damage to {target.name}!");
        }
        private void Heal(Character target, Ability selectedAbility)
        {
            if (target.currentlyAppliedStatuses.Contains(StatusType.Bleed))
            {
                Debug.Log($"Healed bleeding on {target}!");
                target.BleedDurationLeft = 0;
                OnHealAppliedToCureBleed?.Invoke();
            }
            
            if (target.Health + finalHealAmount > target.maxHealth)
            {
                target.Health = target.maxHealth;
            }
            else
            {
                target.Health += finalHealAmount;
            }
            VisualizeAction(selectedAbility.abilityType, finalHealAmount.ToString());
            Debug.Log($"{target.name} has been healed for {finalHealAmount}!");
        }
        private void Shield(Character target, Ability selectedAbility)
        {
            if (target.ShieldPoints + selectedAbility.shieldAmount > target.maxShield)
            {
                target.ShieldPoints = target.maxShield;
            }
            else
            {
                target.ShieldPoints += selectedAbility.shieldAmount;
            }
            VisualizeAction(selectedAbility.abilityType, selectedAbility.shieldAmount.ToString());
            Debug.Log($"{target.name} has been shielded for {selectedAbility.shieldAmount}!");
        }
        

        private void ApplyStatus(Character target, Ability selectedStatus)
        {
            if(!target.currentlyAppliedStatuses.Contains(selectedStatus.statusType))
            {
                target.currentlyAppliedStatuses.Add(selectedStatus.statusType);
            }
            string finalText;
            switch (selectedStatus.statusType)
            {
                case StatusType.Bleed:
                    if (target.invulnerableToBleed)
                    {
                        finalText = "Invulnerable to Bleed";
                        target.currentlyAppliedStatuses.Remove(selectedStatus.statusType);
                        break;
                    }
                    target.BleedDurationLeft += selectedStatus.bleedDuration;
                    target.CumulatedBleedDmg += selectedStatus.bleedDmgAmount;
                    finalText = $"-{finalDamageAmount} HP, Bleed";
                    OnBleedApplied?.Invoke();
                    break;
                case StatusType.Dodge:
                    target.DodgeEverythingUntilNextTurn = true;
                    finalText = "Dodge";
                    OnDodgeApplied?.Invoke();
                    break;
                case StatusType.Stun:
                    target.StunDurationLeft += selectedStatus.stunDuration;
                    finalText = $"-{finalDamageAmount} HP, Stun ({selectedStatus.stunDuration} turns)";
                    OnStunApplied?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(message:"Status type not found", innerException: null);
            }
            VisualizeAction(AbilityType.Status, finalText);
            Debug.Log($"Applied {selectedStatus.statusType} to {target}!");
            target.CheckIfDead();
        }
    }
}