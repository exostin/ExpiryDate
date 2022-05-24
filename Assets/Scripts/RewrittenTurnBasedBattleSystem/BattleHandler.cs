using System;
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
            if(activeCharacter == null)
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
            bool playerTeamWon = true;
            foreach(Character character in enemyTeam.characters)
            {
                if (character.IsAlive)
                {
                    playerTeamWon = false;
                    break;
                }
            }
            bool enemyTeamWon = true;
            foreach (Character character in playerTeam.characters)
            {
                if (character.IsAlive)
                {
                    enemyTeamWon = false;
                    break;
                }
            }
            if(playerTeamWon || enemyTeamWon)
            {
                PlayerTeamWon = playerTeamWon;
                OnBattleEnded();
                Debug.Log($"Battle Ended, {(PlayerTeamWon ? "player" : "enemy")} won");
            }
        }
    }
}
