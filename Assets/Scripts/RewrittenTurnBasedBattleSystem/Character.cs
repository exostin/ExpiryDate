using UnityEngine;

namespace RewrittenTurnBasedBattleSystem
{
    public class Character : MonoBehaviour
    {
        public bool IsAlive { get; set; }
        public IAbility[] abilities => new IAbility[] { new DebugAbility() };
        public int initiative;
    }
}