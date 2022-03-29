using System;
using UnityEngine;

namespace Classes
{
    [Serializable]
    public class BuildingUpgrade
    {
        public int level; // this should be always incremented by 1
        public Resources cost;
        public Resources output;
        public GameObject model;
    }
}