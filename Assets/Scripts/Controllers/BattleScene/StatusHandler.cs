using System;
using System.Linq;
using System.Web.UI.WebControls;
using Other.Enums;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers.BattleScene
{
    public class StatusHandler
    {
        public delegate void OnStatusRemoved();
        public static event OnStatusRemoved OnBleedRemoved;
        public static event OnStatusRemoved OnStunRemoved;
        public static event OnStatusRemoved OnDodgeRemoved;
        
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
                            OnBleedRemoved?.Invoke();
                        }
                        break;
                    }
                    case StatusType.Dodge:
                    {
                        character.DodgeEverythingUntilNextTurn = false;
                        finalText = "No longer dodging!";
                        OnDodgeRemoved?.Invoke();
                        Debug.Log($"{character.name} no longer dodges attacks.");
                        break;
                    }
                    case StatusType.Stun:
                    {
                        if (character.StunDurationLeft == 0)
                        {
                            finalText = $"No longer stunned!";
                            character.currentlyAppliedStatuses.Remove(status);
                            OnStunRemoved?.Invoke();
                            Debug.Log($"{character.name} is no longer stunned.");
                        }
                        else
                        {
                            var rollForStunBreak = Random.Range(1f, 100f);
                            if (rollForStunBreak <= character.chanceToBreakOutOfStun)
                            {
                                character.StunDurationLeft = 0;
                                finalText = $"Broke out of stun!";
                                character.currentlyAppliedStatuses.Remove(status);
                                OnStunRemoved?.Invoke();
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
                if (character.CheckIfDead()) return;
            }
        }
    }
}