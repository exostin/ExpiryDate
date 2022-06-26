using System.Collections.Generic;
using RewrittenTurnBasedBattleSystem.ScriptableObjects;
using UnityEditor;

namespace RewrittenTurnBasedBattleSystem
{
    public class TeamSpawner
    {
        private readonly CharacterFactory characterFactory = new();
        
        public Team SpawnTeam(IEnumerable<CharacterData> charactersData)
        {
            Team team = new();
            foreach (CharacterData character in charactersData)
            {
                Character createdCharacter = characterFactory.CreateCharacter(character);
                createdCharacter.Initialize();
                team.characters.Add(createdCharacter);
            }
            return team;
        }
    }
}