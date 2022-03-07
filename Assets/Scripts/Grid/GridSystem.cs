using UnityEngine;

namespace Grid
{
    public class GridSystem : MonoBehaviour
    {
        public GridCell[,] GridCells { get; set; }
        [SerializeField] private GameObject tilePrefab, gridParent;

        [SerializeField] private int gridHorizontalSize, gridVerticalSize;

        public void PopulateGrid()
        {
            GridCells = new GridCell[gridVerticalSize, gridHorizontalSize];
            for (var i = 0; i < gridHorizontalSize; i++)
            for (var j = 0; j < gridVerticalSize; j++)
            {
                GridCells[j, i] = new GridCell((int) GridCellType.Empty);
                var tile = Instantiate(tilePrefab, gridParent.transform);
            }
        }
    }

    public enum GridCellType
    {
        Empty = 0,
        Populated = 1,
        Temporary = 2
    }
}
