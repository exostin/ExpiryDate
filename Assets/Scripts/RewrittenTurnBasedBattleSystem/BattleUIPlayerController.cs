using System;

namespace RewrittenTurnBasedBattleSystem
{
    class BattleUIPlayerController : IPlayerBattleUI
    {
        public void PlayerStartedHisTurn(PlayerCharacterHandler h)
        {
            throw new NotImplementedException();
        }

        public event Action<IAbility, Character> OnChosenActionWithTarget;
        public event Action<IAbility, Character> OnChosenActionOnWholeTeam;
    }
}