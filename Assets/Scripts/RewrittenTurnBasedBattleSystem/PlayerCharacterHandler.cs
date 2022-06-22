using System;

namespace RewrittenTurnBasedBattleSystem
{
    internal class PlayerCharacterHandler : ICharacterHandler
    {
        public Character Character => character;
        private Character character;
        
        public PlayerCharacterHandler(Character character)
        {
            this.character = character;
        }

        public event Action OnActionFinished;

        public void PerformAction()
        {
            character.Abilities[0].Perform(character);
            OnActionFinished?.Invoke();
        }
    }
}