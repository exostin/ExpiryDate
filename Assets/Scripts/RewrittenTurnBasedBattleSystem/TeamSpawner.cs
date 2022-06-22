using System;
using RewrittenTurnBasedBattleSystem.IAbilities;
using RewrittenTurnBasedBattleSystem.ScriptableObjects;
using RewrittenTurnBasedBattleSystem.ScriptableObjects.AbilityData;
using RewrittenTurnBasedBattleSystem.ScriptableObjects.AbilityTypes;
using UnityEditor;

namespace RewrittenTurnBasedBattleSystem
{
    public class TeamSpawner
    {
        private readonly CharacterFactory characterFactory = new CharacterFactory();
        
        public Team SpawnTeam(CharacterData[] charactersData)
        {
            Team team = new();
            foreach (CharacterData character in charactersData)
            {
                team.characters.Add(characterFactory.CreateCharacter(character));
            }

            return team;
        }
    }

    internal class CharacterFactory
    {
        public Character CreateCharacter(CharacterData data)
        {
            IAbility[] abilities = new IAbility[data.abilities.Length];
            
            var i = 0;
            foreach (BaseAbilityData abilityData in data.abilities)
            {
                abilities[i] = CreateAbility(abilityData);
                i++;
            }

            return new Character(abilities, data);
        }

        private IAbility CreateAbility(BaseAbilityData abilityData)
        {
            switch (abilityData)
            {
                case DamageSingleEnemyAbilityData data:
                    return new DamageSingleEnemyAbility(data);
                default:
                    throw new Exception("Ability not implemented");
            }
        }
    }
}