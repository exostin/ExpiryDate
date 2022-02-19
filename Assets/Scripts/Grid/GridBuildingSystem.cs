using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class GridBuildingSystem : MonoBehaviour
    {
        public static GridBuildingSystem current;

        public GridLayout gridLayout;
        public Tilemap mainTilemap;
        public Tilemap tempTilemap;

        private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

        private Building temp;
        private Vector3 prevPos;
        private BoundsInt prevArea;
        
        #region Unity Methods

        private void Awake()
        {
            current = this;
        }

        void Start()
        {
            string tilePath = @"Tiles\";
            tileBases.Add(TileType.Empty, null);
            tileBases.Add(TileType.Gray, Resources.Load<TileBase>(tilePath + "gray"));
            tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
            tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
        }

        private void Update()
        {
            if (!temp)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject(0))
                {
                    return;
                }

                if (!temp.Placed)
                {
                    if (Camera.main != null)
                    {
                        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Vector3Int cellPos = gridLayout.LocalToCell(touchPos);
                        if (prevPos != cellPos)
                        {
                            temp.transform.localPosition =
                                gridLayout.CellToLocalInterpolated((cellPos + new Vector3(.5f, .5f, 0f)));
                            prevPos = cellPos;
                            FollowBuilding();
                        }
                    }
                }
            }
        }

        #endregion

        #region Tilemap Management

        private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
        {
            TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
            int counter = 0;
            foreach (var v in area.allPositionsWithin)
            {
                Vector3Int pos = new Vector3Int(v.x, v.y, 0);
                array[counter] = tilemap.GetTile(pos);
                counter++;
            }

            return array;
        }

        private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
        {
            int size = area.size.x * area.size.y * area.size.z;
            TileBase[] tileArray = new TileBase[size];
            FillTiles(tileArray, type);
            tilemap.SetTilesBlock(area,tileArray);
        }

        private static void FillTiles(TileBase[] arr, TileType type)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = tileBases[type];
            }
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
            TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
            FillTiles(toClear, TileType.Empty);
            tempTilemap.SetTilesBlock(prevArea, toClear);
        }

        private void FollowBuilding()
        {
            ClearArea();

            temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
            BoundsInt buildingArea = temp.area;

            TileBase[] baseArray = GetTilesBlock(buildingArea, mainTilemap);

            int size = baseArray.Length;
            TileBase[] tileArray = new TileBase[size];

            for (int i = 0; i < baseArray.Length; i++)
            {
                if (baseArray[i] == tileBases[TileType.Gray])
                {
                    tileArray[i] = tileBases[TileType.Green];
                }
                else
                {
                    FillTiles(tileArray, TileType.Red);
                    break;
                }
            }
            tempTilemap.SetTilesBlock(buildingArea, tileArray);
            prevArea = buildingArea;
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
