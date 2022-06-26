using System;
using System.Collections;
using RewrittenTurnBasedBattleSystem.IAbilities;
using RewrittenTurnBasedBattleSystem.ScriptableObjects.BaseAbilityData_ChildClasses;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RewrittenTurnBasedBattleSystem
{
    internal class AICharacterHandler : ICharacterHandler
    {
        private Character character;
        private MonoBehaviour coroutineInvoker;

        public AICharacterHandler(Character character, MonoBehaviour coroutineInvoker)
        {
            this.character = character;
            this.coroutineInvoker = coroutineInvoker;
        }

        public Character Character => character;
        public Team PlayerTeam { get; set; }
        public Team AITeam { get; set; }

        public event Action OnActionFinished;

        public void PerformAction()
        {
            coroutineInvoker.StartCoroutine(PerformActionCoroutine());
        }

        private IEnumerator PerformActionCoroutine()
        {
            yield return new WaitForSeconds(1);
            IAbility selectedAbility = character.Abilities[Random.Range(0, character.Abilities.Length)];
            switch (selectedAbility)
            {
                case DamageSingleEnemyAbility or DamageMultipleEnemyAbility:
                    selectedAbility.Perform(AITeam,PlayerTeam, PlayerTeam.characters[Random.Range(0, PlayerTeam.characters.Count - 1)]);
                    break;
                case HealSelfAbility:
                    selectedAbility.Perform(AITeam,PlayerTeam, character);
                    break;
            }
            OnActionFinished?.Invoke();
        }
    }
}