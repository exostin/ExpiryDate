using System;
using System.Linq;
using Other.Enums;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers.BattleScene
{
    public class StatusHandler
    {
        public void HandleStatuses(Character character, out bool skipThisTurn, GameObject currentCharGameObject)
        {
            skipThisTurn = false;
            if (!character.currentlyAppliedStatuses.Any()) return;
            foreach (StatusType status in character.currentlyAppliedStatuses.ToList())
            {
                Debug.Log($"Resolving status {status}");
                string finalText = null;
                switch (status)
                {
                    case StatusType.Bleed:
                    {
                        character.Health -= character.CumulatedBleedDmg;
                        character.BleedDurationLeft--;
                        finalText = $"-{character.CumulatedBleedDmg} HP from Bleeding";
                        Debug.Log($"{character.name} took {character.CumulatedBleedDmg} damage from bleeding. {character.BleedDurationLeft} bleeding turns left.");
                        if (character.BleedDurationLeft <= 0)
                        {
                            finalText = "Bleeding stopped!";
                            Debug.Log("Bleeding stopped!");
                            character.currentlyAppliedStatuses.Remove(status);
                            character.BleedDurationLeft = 0;
                        }
                        break;
                    }
                    case StatusType.Dodge:
                    {
                        character.DodgeEverythingUntilNextTurn = false;
                        finalText = "No longer dodging!";
                        Debug.Log($"{character.name} no longer dodges attacks.");
                        break;
                    }
                    case StatusType.Stun:
                    {
                        if (character.StunDurationLeft == 0)
                        {
                            finalText = $"No longer stunned!";
                            character.currentlyAppliedStatuses.Remove(status);
                            Debug.Log($"{character.name} is no longer stunned.");
                        }
                        else
                        {
                            var willBreakOut = Random.Range(1f, 100f);
                            if (willBreakOut <= character.chanceToBreakOutOfStun)
                            {
                                character.StunDurationLeft = 0;
                                finalText = $"Broke out of stun!";
                                character.currentlyAppliedStatuses.Remove(status);
                                Debug.Log($"{character.name} broke out of stun!");
                            }
                            else
                            {
                                finalText = $"Still stunned!";
                                skipThisTurn = true;
                                character.StunDurationLeft--;
                                Debug.Log($"{character.name} is stunned for: {character.StunDurationLeft} more turns.");
                            }
                        }
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                currentCharGameObject.GetComponent<NotificationsHandler>().HandleNotification(AbilityType.Status, finalText);
            }
        }
    }
}