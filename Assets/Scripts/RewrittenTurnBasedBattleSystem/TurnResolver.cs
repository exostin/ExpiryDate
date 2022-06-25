using System.Linq;
using UnityEngine;

namespace RewrittenTurnBasedBattleSystem
{
    public class TurnResolver
    {
        public MonoBehaviour CoroutineInvoker { get; set; }
        private Team lastSelectedTeam;
        public Team PlayerTeam { get; set; }
        public Team EnemyTeam { get; set; }

        internal ICharacterHandler GetCurrentActive()
        {
            if (lastSelectedTeam == null || lastSelectedTeam == EnemyTeam)
            {
                lastSelectedTeam = PlayerTeam;
                return new PlayerCharacterHandler(PlayerTeam.characters.First());
            }

            lastSelectedTeam = EnemyTeam;
            return new AICharacterHandler(EnemyTeam.characters.First(), CoroutineInvoker);
        }
    }
}