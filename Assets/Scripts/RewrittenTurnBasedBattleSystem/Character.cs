using UnityEngine;

namespace RewrittenTurnBasedBattleSystem
{
    public class Character : MonoBehaviour
    {
        public bool IsAlive;
        public IAbility[] abilities => new IAbility[] { new DebugAbility() };
        public int initiative;
    }
}