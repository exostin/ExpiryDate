using UnityEngine;

namespace RewrittenTurnBasedBattleSystem
{
    internal class DebugAbility : IAbility
    {
        public void Perform()
        {
            Debug.Log("PERFORMING!");
        }
    }
}