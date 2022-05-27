using System;
using System.Linq;
using Other.Enums;
using ScriptableObjects;
using UnityEngine;

namespace Controllers.BattleScene
{
    public class StatusHandler
    {
        public void HandleStatuses(Character character, out bool skipThisTurn)
        {
            skipThisTurn = false;
            if (!character.currentlyAppliedStatuses.Any()) return;
            foreach (StatusType status in character.currentlyAppliedStatuses.ToList())
            {
                Debug.Log($"Resolving status {status}");
                switch (status)
                {
                    case StatusType.Bleed:
                    {
                        character.Health -= character.CumulatedBleedDmg;
                        character.BleedDurationLeft--;
                        Debug.Log($"{character.name} took {character.CumulatedBleedDmg} damage from bleeding. {character.BleedDurationLeft} bleeding turns left.");
                        if (character.BleedDurationLeft == 0)
                        {
                            Debug.Log("Bleeding stopped!");
                            character.currentlyAppliedStatuses.Remove(status);
                        }
                        break;
                    }
                    case StatusType.Dodge:
                    {
                        character.DodgeEverythingUntilNextTurn = false;
                        Debug.Log($"{character.name} no longer dodges attacks.");
                        break;
                    }
                    case StatusType.Stun:
                    {
                        if (character.StunnedDurationLeft == 0)
                        {
                            character.currentlyAppliedStatuses.Remove(status);
                            Debug.Log($"{character.name} is no longer stunned.");
                        }
                        else
                        {
                            skipThisTurn = true;
                            character.StunnedDurationLeft--;
                            Debug.Log($"{character.name} is stunned for: {character.StunnedDurationLeft} more turns.");
                        }
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}