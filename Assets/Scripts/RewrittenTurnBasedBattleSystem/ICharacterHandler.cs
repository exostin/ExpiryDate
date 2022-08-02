using System;

namespace RewrittenTurnBasedBattleSystem
{
    internal interface ICharacterHandler
    {
        Character Character { get; }
        public Team PlayerTeam { get; set; }
        public Team AITeam { get; set; }

        event Action OnActionFinished;

        void PerformAction();
    }
}