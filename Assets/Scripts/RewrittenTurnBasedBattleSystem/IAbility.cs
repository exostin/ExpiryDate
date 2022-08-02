using System.Collections.Generic;
using UnityEngine;
namespace RewrittenTurnBasedBattleSystem
{
    public interface IAbility
    {
        void Perform(Team casterTeam, Team enemyTeam, Character selectedTarget);
    }
}