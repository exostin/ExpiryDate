using System;

namespace RewrittenTurnBasedBattleSystem
{
    internal class PlayerCharacterHandler : ICharacterHandler
    {
        private Character character;

        public PlayerCharacterHandler(Character character)
        {
            this.character = character;
        }

        public Character Character => character;

        public event Action OnActionFinished;

        public void PerformAction()
        {
            character.abilities[0].Perform();
            if (OnActionFinished != null) OnActionFinished();
        }
    }
}