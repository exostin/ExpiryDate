using UnityEngine;
using ScriptableObjects;
namespace RewrittenTurnBasedBattleSystem
{
    public interface IAbility
    {
        void Perform(Character target);
    }
}