using Controllers;
using Controllers.CitybuildingScene;
using UnityEngine;

namespace Other.Citybuilding
{
    public class Building : MonoBehaviour
    {
        private Classes.Citybuilding.Building building;
        private CitybuildingController cbc;
        private Texture2D cursorPointerTexture;
        private AnimationController ac;

        private void OnEnable()
        {
            Debug.Log($"OnEnable {gameObject.name}");

            cbc = FindObjectOfType<CitybuildingController>();
            ac = FindObjectOfType<AnimationController>();

            cursorPointerTexture = cbc.cursorPointerTexture;
            building = cbc.GameObjectToBuilding(gameObject);
        }

        private void OnMouseEnter()
        {
            Cursor.SetCursor(cursorPointerTexture, new Vector2(0, 0),
                CursorMode.Auto);
        }

        private void OnMouseExit()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        private void OnMouseUpAsButton()
        {
            cbc.SelectBuilding(building);
            ac.SetShopVisibility(true);
        }
    }
}