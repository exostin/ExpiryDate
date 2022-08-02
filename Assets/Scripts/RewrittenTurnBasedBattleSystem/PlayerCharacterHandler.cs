using System;
using System.Management;

namespace RewrittenTurnBasedBattleSystem
{
    internal class PlayerCharacterHandler : ICharacterHandler
    {
        public PlayerCharacterHandler(Character character)
        {
            Character = character;
        }

        private IPlayerBattleUI playerBattleUI;

        public Character Character { get; }
        public Team PlayerTeam { get; set; }
        public Team AITeam { get; set; }

        public event Action OnActionFinished;

        public void PerformAction()
        {
            Character.Abilities[0].Perform(PlayerTeam, AITeam, AITeam.characters[0]);
            OnActionFinished?.Invoke();
        }
    }
}