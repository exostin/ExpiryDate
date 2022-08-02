using System;
using RewrittenTurnBasedBattleSystem.IAbilities;
using RewrittenTurnBasedBattleSystem.ScriptableObjects;
using RewrittenTurnBasedBattleSystem.ScriptableObjects.BaseAbilityData_ChildClasses;

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
                case DamageMultipleEnemyAbilityData data:
                    return new DamageMultipleEnemyAbility(data);
                case HealSelfAbilityData data:
                    return new HealSelfAbility(data);
                case HealSingleTargetAbilityData data:
                    return new HealSingleTargetAbility(data);
                case HealMultipleTargetAbilityData data:
                    return new HealMultipleTargetAbility(data);
                case DamageAndStunSingleTargetAbilityData data:
                    return new DamageAndStunSingleEnemyAbility(data);
                default:
                    throw new Exception("Ability not implemented");
            }
        }
    }
}