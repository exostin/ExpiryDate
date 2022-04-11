namespace Grid
{
    public class GridCell
    {
        public GridCell(int gridCellType)
        {
            Type = gridCellType;
        }

        public int Type { get; set; }
    }
}