using UnityEngine;

namespace RewrittenTurnBasedBattleSystem
{
    public class TurnResolver
    {
        public MonoBehaviour CoroutineInvoker { get; set; }
        private Team lastSelectedTeam;
        public Team PlayerTeam { get; set; }
        public Team AITeam { get; set; }
        
        private int lastPlayerCharacterIndex = 0;
        private int lastEnemyCharacterIndex = 0;
        
        internal ICharacterHandler GetCurrentActive()
        {
            Character activeCharacter;
            if (lastSelectedTeam is null || lastSelectedTeam == AITeam)
            {
                lastSelectedTeam = PlayerTeam;
                activeCharacter = PlayerTeam.characters[lastEnemyCharacterIndex];
                IncrementCharacterIndex(PlayerTeam, ref lastPlayerCharacterIndex);
                return new PlayerCharacterHandler(activeCharacter);
            }
            
            lastSelectedTeam = AITeam;
            activeCharacter = AITeam.characters[lastEnemyCharacterIndex];
            IncrementCharacterIndex(AITeam, ref lastEnemyCharacterIndex);
            return new AICharacterHandler(activeCharacter, CoroutineInvoker);
        }

        private void IncrementCharacterIndex(Team team, ref int index)
        {
            index++;
            if (index > team.characters.Count - 1)
            {
                index = 0;
            }
        }
    }
}