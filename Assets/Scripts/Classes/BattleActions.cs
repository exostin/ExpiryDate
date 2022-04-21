using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;

namespace Classes
{
    public class BattleActions
    {
        /// <summary>
        ///     Deploy a chosen action - damage/heal/buff - onto a chosen target
        /// </summary>
        public void MakeAction(Character characterUsedForAttack, Character target, Ability selectedAbility,
            IEnumerable<Character> allCharacters, bool isPlayerTurn)
        {
            var finalAttackTargets = new List<Character>();
            finalAttackTargets.AddRange(allCharacters.Where(x => !x.isDead).ToList());

            if (isPlayerTurn)
            {
                if (selectedAbility.buff || selectedAbility.heal)
                    // when ability is a buff/heal - remove all non-player characters from targets list
                    RemoveAllEnemyCharactersFromTargets(finalAttackTargets);
                else
                    RemoveAllPlayerCharactersFromTargets(finalAttackTargets);
            }
            else
            {
                if (selectedAbility.buff || selectedAbility.heal)
                    // when ability is a buff/heal - remove all player characters from targets list
                    RemoveAllPlayerCharactersFromTargets(finalAttackTargets);
                else
                    RemoveAllEnemyCharactersFromTargets(finalAttackTargets);
            }

            if (selectedAbility.targetsWholeTeam)
            {
                foreach (var character in finalAttackTargets)
                    if (selectedAbility.buff)
                        Buff(character, selectedAbility);
                    else
                        DealOrHeal(characterUsedForAttack, character, selectedAbility);
            }
            else
            {
                if (selectedAbility.buff)
                    Buff(target, selectedAbility);
                else
                    DealOrHeal(characterUsedForAttack, target, selectedAbility);
            }
        }

        private static void RemoveAllEnemyCharactersFromTargets(List<Character> finalAttackTargets)
        {
            for (var i = 0; i < finalAttackTargets.Count; i++)
            {
                if (i >= finalAttackTargets.Count) break;

                if (!finalAttackTargets[i].isOwnedByPlayer)
                {
                    Debug.Log($"Removing {finalAttackTargets[i].name} from targets list");
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
                    Debug.Log($"Removing {finalAttackTargets[i].name} from targets list");
                    finalAttackTargets.Remove(finalAttackTargets[i]);
                    i--;
                }
            }
        }

        private static void DealOrHeal(Character characterUsedForAttack, Character target, Ability selectedAbility)
        {
            if (target.health - selectedAbility.damage <= 0)
            {
                target.health = 0;
                target.isDead = true;
            }
            else if (target.health - selectedAbility.damage > target.maxHealth)
            {
                target.health = target.maxHealth;
            }
            else
            {
                target.health -= selectedAbility.damage;
            }

            Debug.Log($"{characterUsedForAttack.name} has dealt {selectedAbility.damage} damage to {target.name}!");
        }

        private static void Buff(Character characterUsed, Ability selectedBuff)
        {
            Debug.Log($"Buff {characterUsed.name} with {selectedBuff.name}!");
        }
    }
}