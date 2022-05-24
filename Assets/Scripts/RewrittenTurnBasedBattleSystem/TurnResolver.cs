using System.Linq;
using UnityEngine;

namespace RewrittenTurnBasedBattleSystem
{
    public class TurnResolver : MonoBehaviour
    {
        public Team playerTeam;
        public Team enemyTeam;

        private Team lastSelectedTeam = null;

        internal ICharacterHandler GetCurrentActive()
        {
            if(lastSelectedTeam == null || lastSelectedTeam == enemyTeam)
            {
                lastSelectedTeam = playerTeam;
                return new PlayerCharacterHandler(playerTeam.characters.First());
            }
            else
            {
                lastSelectedTeam = enemyTeam;
                return new AICharacterHandler(enemyTeam.characters.First(), this);
            }
        }
    }
}