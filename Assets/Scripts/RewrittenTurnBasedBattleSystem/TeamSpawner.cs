using System.Collections.Generic;
using RewrittenTurnBasedBattleSystem.ScriptableObjects;
using RewrittenTurnBasedBattleSystem.ScriptableObjects.AbilityTypes;
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
                var createdCharacter = characterFactory.CreateCharacter(character);
                createdCharacter.Initialize();
                team.characters.Add(createdCharacter);
            }

            return team;
        }
    }
}