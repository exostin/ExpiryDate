using UnityEngine;

namespace Controllers.CitybuildingScene
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private Animation buildingShop;
        private bool shopActivated = false;

        private void PlayShow()
        {
            buildingShop["ShowShop"].time = 0;
            buildingShop["ShowShop"].speed = 1;
            buildingShop.Play("ShowShop");
        }

        private void PlayHide()
        {
            buildingShop["ShowShop"].time = buildingShop["ShowShop"].length;
            buildingShop["ShowShop"].speed = -1;
            buildingShop.Play("ShowShop");
        }

        public void ToggleShopVisibility()
        {
            if (!shopActivated) PlayShow();
            else PlayHide();
        
            shopActivated = !shopActivated;
        }
        
        public void SetShopVisibility(bool visible)
        {
            if(shopActivated == visible) return;
            
            if (visible) PlayShow();
            else PlayHide();

            shopActivated = visible;
        }
    }
}