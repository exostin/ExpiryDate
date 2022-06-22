using System.Linq;
using UnityEngine;

namespace RewrittenTurnBasedBattleSystem
{
    public class TurnResolver : MonoBehaviour
    {
        public Team playerTeam { get; set; }
        public Team enemyTeam { get; set; }

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