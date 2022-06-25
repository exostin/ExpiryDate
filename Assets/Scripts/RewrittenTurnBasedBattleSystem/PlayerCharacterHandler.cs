using System;

namespace RewrittenTurnBasedBattleSystem
{
    internal class PlayerCharacterHandler : ICharacterHandler
    {
        public PlayerCharacterHandler(Character character)
        {
            Character = character;
        }

        public Character Character { get; }

        public event Action OnActionFinished;

        public void PerformAction()
        {
            Character.Abilities[0].Perform(Character);
            OnActionFinished?.Invoke();
        }
    }
}