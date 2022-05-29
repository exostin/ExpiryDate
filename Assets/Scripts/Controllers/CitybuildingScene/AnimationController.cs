using UnityEngine;

namespace Controllers.CitybuildingScene
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private Animation buildingShop;
        // [SerializeField] private Animation notification;
        //private bool notificationActivated;
        private bool shopActivated = false;

        // private void Start()
        // {
        //     ToggleNotificationVisibility();
        // }

        public void ToggleShopVisibility()
        {
            if (!shopActivated)
            {
                buildingShop["ShowShop"].time = 0;
                buildingShop["ShowShop"].speed = 1;
                buildingShop.Play("ShowShop");
            }
            else
            {
                // Play backwards
                buildingShop["ShowShop"].time = buildingShop["ShowShop"].length;
                buildingShop["ShowShop"].speed = -1;
                buildingShop.Play("ShowShop");
            }
        
            shopActivated = !shopActivated;
        }
        
        public void SetShopVisibility(bool visible)
        {
            if(shopActivated == visible) return;
            
            if (visible)
            {
                // Show
                buildingShop["ShowShop"].time = 0;
                buildingShop["ShowShop"].speed = 1;
                buildingShop.Play("ShowShop");
            }
            else
            {
                // Hide
                buildingShop["ShowShop"].time = buildingShop["ShowShop"].length;
                buildingShop["ShowShop"].speed = -1;
                buildingShop.Play("ShowShop");
            }

            shopActivated = visible;
        }

        // public void ToggleNotificationVisibility()
        // {
        //     if (!notificationActivated)
        //     {
        //         notification["ShowNotification"].time = 0;
        //         notification["ShowNotification"].speed = 1;
        //         notification.Play("ShowNotification");
        //     }
        //
        //     notificationActivated = !notificationActivated;
        // }
    }
}