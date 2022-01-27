using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New ability", menuName = "Ability")]
    public class Ability : ScriptableObject
    {
        public string abilityName;
        public int cost;
        public int damage;
    }
}