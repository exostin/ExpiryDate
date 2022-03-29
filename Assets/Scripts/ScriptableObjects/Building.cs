using Classes;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Building", menuName = "Building")]
    public class Building : ScriptableObject
    {
        public string buildingName;
        public string description;
        public BuildingUpgrade[] upgrades;
    }
}