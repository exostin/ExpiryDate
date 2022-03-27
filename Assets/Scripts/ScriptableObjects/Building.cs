using System;
using UnityEngine;
using Resources = Classes.Resources;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Building", menuName = "Building")]
    public class Building : ScriptableObject
    {
        public string buildingName;
        public string description;
        public BuildingUpgrade[] upgrades;
    }

    [Serializable]
    public class BuildingUpgrade
    {
        public int level; // this should be always incremented by 1
        public Resources cost;
        public Resources output;
        public GameObject model;
    }
}