using System;
using System.Linq;
using UnityEngine;

namespace RewrittenTurnBasedBattleSystem
{
    internal class BattleHandler
    {
        private ICharacterHandler activeCharacter;
        public Team PlayerTeam { get; set; }
        public Team EnemyTeam { get; set; }
        public bool PlayerTeamWon { get; private set; }
        private TurnResolver TurnResolver { get; } = new();
        public event Action OnBattleEnded = delegate { };

        public void Tick()
        {
            if (activeCharacter != null) return;
            TurnResolver.PlayerTeam = PlayerTeam;
            TurnResolver.EnemyTeam = EnemyTeam;
            activeCharacter = TurnResolver.GetCurrentActive();
            StartTurn();
        }

        private void StartTurn()
        {
            Debug.Log($"Turn of: {activeCharacter.Character.CharacterData.characterName}");
            activeCharacter.OnActionFinished += ActiveCharacter_OnActionFinished;
            activeCharacter.PerformAction();
        }

        private void ActiveCharacter_OnActionFinished()
        {
            activeCharacter = null;
            var playerTeamWon = EnemyTeam.characters.All(character => character.IsDead);
            var enemyTeamWon = PlayerTeam.characters.All(character => character.IsDead);
            if (playerTeamWon || enemyTeamWon)
            {
                PlayerTeamWon = playerTeamWon;
                OnBattleEnded?.Invoke();
                Debug.Log($"Battle Ended, {(PlayerTeamWon ? "player" : "enemy")} won");
            }
        }
        
        public void AssignCoroutineInvokerReference(MonoBehaviour coroutineInvoker)
        {
            TurnResolver.CoroutineInvoker = coroutineInvoker;
        }
    }
}