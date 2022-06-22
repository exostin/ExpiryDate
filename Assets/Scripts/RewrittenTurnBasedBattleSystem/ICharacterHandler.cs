using System;

namespace RewrittenTurnBasedBattleSystem
{
    internal interface ICharacterHandler
    {
        Character Character { get; }

        event Action OnActionFinished;

        void PerformAction();
    }
}