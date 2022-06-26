using System.Linq;
using UnityEngine;

namespace RewrittenTurnBasedBattleSystem
{
    public class TurnResolver
    {
        public MonoBehaviour CoroutineInvoker { get; set; }
        private Team lastSelectedTeam;
        public Team PlayerTeam { get; set; }
        public Team AITeam { get; set; }

        internal ICharacterHandler GetCurrentActive()
        {
            if (lastSelectedTeam == null || lastSelectedTeam == AITeam)
            {
                lastSelectedTeam = PlayerTeam;
                return new PlayerCharacterHandler(PlayerTeam.characters.First());
            }

            lastSelectedTeam = AITeam;
            return new AICharacterHandler(AITeam.characters.First(), CoroutineInvoker);
        }
    }
}