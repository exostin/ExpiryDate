using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class GridBuildingSystem : MonoBehaviour
    {
        public static GridBuildingSystem current;

        private static readonly Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

        public GridLayout gridLayout;
        public Tilemap mainTilemap;
        public Tilemap tempTilemap;
        private BoundsInt prevArea;
        private Vector3 prevPos;

        private Building temp;

        #region Unity Methods

        private void Awake()
        {
            current = this;
        }

        private void Start()
        {
            var tilePath = @"Tiles\";
            tileBases.Add(TileType.Empty, null);
            tileBases.Add(TileType.Gray, Resources.Load<TileBase>(tilePath + "gray"));
            tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
            tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
        }

        private void Update()
        {
            if (!temp) return;

            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject(0)) return;

                if (!temp.Placed)
                    if (Camera.main != null)
                    {
                        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        var cellPos = gridLayout.LocalToCell(touchPos);
                        if (prevPos != cellPos)
                        {
                            temp.transform.localPosition =
                                gridLayout.CellToLocalInterpolated(cellPos + new Vector3(.5f, .5f, 0f));
                            prevPos = cellPos;
                            FollowBuilding();
                        }
                    }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (temp.CanBePlaced()) temp.Place();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClearArea();
                Destroy(temp.gameObject);
            }
        }

        #endregion

        #region Tilemap Management

        private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
        {
            var array = new TileBase[area.size.x * area.size.y * area.size.z];
            var counter = 0;
            foreach (var v in area.allPositionsWithin)
            {
                var pos = new Vector3Int(v.x, v.y, 0);
                array[counter] = tilemap.GetTile(pos);
                counter++;
            }

            return array;
        }

        private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
        {
            var size = area.size.x * area.size.y * area.size.z;
            var tileArray = new TileBase[size];
            FillTiles(tileArray, type);
            tilemap.SetTilesBlock(area, tileArray);
        }

        private static void FillTiles(TileBase[] arr, TileType type)
        {
            for (var i = 0; i < arr.Length; i++) arr[i] = tileBases[type];
        }

        #endregion

        #region Building Placement

        public void InitializeWithBuilding(GameObject building)
        {
            temp = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<Building>();
            FollowBuilding();
        }

        private void ClearArea()
        {
            var toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
            FillTiles(toClear, TileType.Empty);
            tempTilemap.SetTilesBlock(prevArea, toClear);
        }

        private void FollowBuilding()
        {
            ClearArea();

            temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
            var buildingArea = temp.area;

            var baseArray = GetTilesBlock(buildingArea, mainTilemap);

            var size = baseArray.Length;
            var tileArray = new TileBase[size];

            for (var i = 0; i < baseArray.Length; i++)
                if (baseArray[i] == tileBases[TileType.Gray])
                {
                    tileArray[i] = tileBases[TileType.Green];
                }
                else
                {
                    FillTiles(tileArray, TileType.Red);
                    break;
                }

            tempTilemap.SetTilesBlock(buildingArea, tileArray);
            prevArea = buildingArea;
        }

        public bool CanTakeArea(BoundsInt area)
        {
            var baseArray = GetTilesBlock(area, mainTilemap);
            foreach (var b in baseArray)
                if (b != tileBases[TileType.Gray])
                {
                    Debug.Log("Cannot place here");
                    return false;
                }

            return true;
        }

        public void TakeArea(BoundsInt area)
        {
            SetTilesBlock(area, TileType.Empty, tempTilemap);
            SetTilesBlock(area, TileType.Green, mainTilemap);
        }

        #endregion
    }

    public enum TileType
    {
        Empty,
        Gray,
        Green,
        Red
    }
}