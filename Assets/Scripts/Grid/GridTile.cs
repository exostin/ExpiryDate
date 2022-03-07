using UnityEngine;
using UnityEngine.EventSystems;

namespace Grid
{
    // IPointerClickHandler
    public class GridTile : MonoBehaviour
    {
        // public void OnPointerClick(PointerEventData eventData)
        // {
        //     if (eventData.button == PointerEventData.InputButton.Left)
        //     {
        //         Debug.Log("Tile was clicked!");
        //     }
        // }

        public void Click()
        {
            Debug.Log("Tile was clicked!");
        }
    }
}