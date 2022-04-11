using UnityEngine;

namespace Grid
{
    public class Building : MonoBehaviour
    {
        public BoundsInt area;
        public bool Placed { get; private set; }

        private void Start()
        {
        }

        #region Build Methods

        public bool CanBePlaced()
        {
            var positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
            var areaTemp = area;
            areaTemp.position = positionInt;
            if (GridBuildingSystem.current.CanTakeArea(areaTemp)) return true;

            return false;
        }

        public void Place()
        {
            var positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
            var areaTemp = area;
            areaTemp.position = positionInt;
            Placed = true;

            GridBuildingSystem.current.TakeArea(areaTemp);
        }

        #endregion
    }
}