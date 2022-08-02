using System;

namespace RewrittenTurnBasedBattleSystem
{
    internal interface IPlayerBattleUI
    {
        void PlayerStartedHisTurn(PlayerCharacterHandler h);
        event Action<IAbility, Character> OnChosenActionWithTarget;
        event Action<IAbility, Character> OnChosenActionOnWholeTeam;
    }
}