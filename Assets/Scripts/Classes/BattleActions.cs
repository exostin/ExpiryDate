using System;
using System.Collections.Generic;
using System.Linq;
using Other.Enums;
using ScriptableObjects;
using UnityEditor.Rendering;
using UnityEngine;

namespace Classes
{
    public class BattleActions
    {
        /// <summary>
        ///     Deploy a chosen action - damage/heal/buff - onto a chosen target
        /// </summary>
        public void MakeAction(Character target, Ability selectedAbility,
            IEnumerable<Character> allCharacters, bool isPlayerTurn)
        {
            var finalAttackTargets = new List<Character>();
            finalAttackTargets.AddRange(allCharacters.Where(x => !x.IsDead).ToList());
            finalAttackTargets.RemoveAll(x => x.DodgeEverythingUntilNextTurn);

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
                foreach (var thisIterationTarget in finalAttackTargets)
                {
                    switch (selectedAbility.abilityType)
                    {
                        case AbilityType.Status:
                            ApplyStatus(thisIterationTarget, selectedAbility);
                            break;
                        case AbilityType.DamageOnly:
                            Deal(thisIterationTarget, selectedAbility);
                            break;
                        case AbilityType.Heal:
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
                switch (selectedAbility.abilityType)
                {
                    case AbilityType.Status:
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

        private static void Deal(Character target, Ability selectedAbility)
        {
            if ((target.Health + target.ShieldPoints) - selectedAbility.damageAmount <= 0)
            {
                target.ShieldPoints = 0;
                target.Health = 0;
                target.IsDead = true;
            }
            else
            {
                if (target.ShieldPoints - selectedAbility.damageAmount < 0)
                {
                    target.ShieldPoints = 0;
                    target.Health -= selectedAbility.damageAmount - target.ShieldPoints;
                }
                else
                {
                    target.ShieldPoints -= selectedAbility.damageAmount;
                }
            }

            Debug.Log($"Dealt {selectedAbility.damageAmount} damage to {target.name}!");
        }
        private static void Heal(Character target, Ability selectedAbility)
        {
            if (target.currentlyAppliedStatuses.Contains(StatusType.Bleed))
            {
                Debug.Log($"Healed bleeding on {target}!");
                target.BleedDurationLeft = 0;
            }
            
            if (target.Health + selectedAbility.healAmount > target.maxHealth)
            {
                target.Health = target.maxHealth;
            }
            else
            {
                target.Health += selectedAbility.healAmount;
            }

            Debug.Log($"{target.name} has been healed for {selectedAbility.healAmount}!");
        }
        private static void Shield(Character target, Ability selectedAbility)
        {
            if (target.ShieldPoints + selectedAbility.shieldAmount > target.maxShield)
            {
                target.ShieldPoints= target.maxShield;
            }
            else
            {
                target.ShieldPoints += selectedAbility.shieldAmount;
            }

            Debug.Log($"{target.name} has been shielded for {selectedAbility.healAmount}!");
        }
        

        private static void ApplyStatus(Character target, Ability selectedStatus)
        {
            Debug.Log($"Applied {selectedStatus.name} to {target}!");
            
            if(!target.currentlyAppliedStatuses.Contains(selectedStatus.statusType))
            {
                target.currentlyAppliedStatuses.Add(selectedStatus.statusType);
            }
            
            switch (selectedStatus.statusType)
            {
                case StatusType.Bleed:
                    Debug.Log($"Setting bleed status, BEFORE: Bleed duration: {target.BleedDurationLeft}, Bleed dmg: {target.CumulatedBleedDmg} on {target.name}!");
                    target.BleedDurationLeft += selectedStatus.bleedDuration;
                    target.CumulatedBleedDmg += selectedStatus.bleedDmgAmount;
                    Debug.Log($"Setting bleed status, AFTER: Bleed duration: {target.BleedDurationLeft}, Bleed dmg: {target.CumulatedBleedDmg} on {target.name}!");
                    break;
                case StatusType.Dodge:
                    target.DodgeEverythingUntilNextTurn = true;
                    break;
                case StatusType.Stun:
                    target.StunDurationLeft += selectedStatus.stunDuration;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(message:"Status type not found", innerException: null);
            }
        }
    }
}