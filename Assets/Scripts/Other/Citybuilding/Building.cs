using System;
using Controllers;
using UnityEngine;

namespace Other.Citybuilding
{
    public class Building : MonoBehaviour
    {
        private Texture2D cursorPointerTexture;
        private Classes.Citybuilding.Building building;
        private CitybuildingController cbc;
        
        private void OnEnable()
        {
            Debug.Log($"OnEnable {gameObject.name}");
            
            cbc = GameObject.Find("CitybuildingController").GetComponent<CitybuildingController>();
            
            cursorPointerTexture = cbc.cursorPointerTexture;
            building = cbc.GameObjectToBuilding(gameObject);
        }

        private void OnMouseEnter()
        {
            Cursor.SetCursor(cursorPointerTexture, new Vector2(cursorPointerTexture.width / 2f, 0),
                CursorMode.Auto);
        }

        private void OnMouseExit()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        private void OnMouseUpAsButton()
        {
            building.Upgrade();
            cbc.UpdateModels();
        }
    }
}