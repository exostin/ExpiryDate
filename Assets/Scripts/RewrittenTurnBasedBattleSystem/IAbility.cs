using UnityEngine;
namespace RewrittenTurnBasedBattleSystem
{
    public interface IAbility
    {
        void Perform(Character target);
    }
}