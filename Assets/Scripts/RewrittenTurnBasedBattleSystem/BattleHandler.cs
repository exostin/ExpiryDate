using System;
using System.Linq;
using UnityEngine;

namespace RewrittenTurnBasedBattleSystem
{
    internal class BattleHandler : MonoBehaviour
    {
        public TurnResolver turnResolver;
        public Team playerTeam;
        public Team enemyTeam;

        private ICharacterHandler activeCharacter;

        public event Action OnBattleEnded = delegate { };

        public bool PlayerTeamWon { get; private set; }

        [ContextMenu("Tick")]
        public void Tick()
        {
            if (activeCharacter == null)
            {
                activeCharacter = turnResolver.GetCurrentActive();
                StartTurn();
            }
        }

        private void StartTurn()
        {
            Debug.Log($"Turn of: {activeCharacter.Character.gameObject}");
            activeCharacter.OnActionFinished += ActiveCharacter_OnActionFinished;
            activeCharacter.PerformAction();
        }

        private void ActiveCharacter_OnActionFinished()
        {
            activeCharacter = null;
            var playerTeamWon = enemyTeam.characters.All(character => !character.IsAlive);
            var enemyTeamWon = playerTeam.characters.All(character => !character.IsAlive);
            if (playerTeamWon || enemyTeamWon)
            {
                PlayerTeamWon = playerTeamWon;
                OnBattleEnded();
                Debug.Log($"Battle Ended, {(PlayerTeamWon ? "player" : "enemy")} won");
            }
        }
    }
}
