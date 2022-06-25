using System;
using RewrittenTurnBasedBattleSystem.IAbilities;
using RewrittenTurnBasedBattleSystem.ScriptableObjects;
using RewrittenTurnBasedBattleSystem.ScriptableObjects.AbilityData;

namespace RewrittenTurnBasedBattleSystem
{
    internal class CharacterFactory
    {
        public Character CreateCharacter(CharacterData data)
        {
            var abilities = new IAbility[data.abilities.Length];
            
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