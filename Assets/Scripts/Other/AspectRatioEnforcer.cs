using UnityEngine;

namespace Other
{
    public class AspectRatioEnforcer : MonoBehaviour
    {
        // Taken from: https://gamedesigntheory.blogspot.com/2010/09/controlling-aspect-ratio-in-unity.html
        private void Start () 
        {
            // set the desired aspect ratio (the values in this example are
            // hard-coded for 16:9, but you could make them into public
            // variables instead so you can set them at design time)
            const float targetAspect = 16.0f / 9.0f;

            // determine the game window's current aspect ratio
            var windowAspect = (float)Screen.width / (float)Screen.height;

            // current viewport height should be scaled by this amount
            var scaleHeight = windowAspect / targetAspect;

            // obtain camera component so we can modify its viewport
            var cameraComponent = GetComponent<Camera>();

            // if scaled height is less than current height, add letterbox
            if (scaleHeight < 1.0f)
            {  
                Rect rect = cameraComponent.rect;

                rect.width = 1.0f;
                rect.height = scaleHeight;
                rect.x = 0;
                rect.y = (1.0f - scaleHeight) / 2.0f;
        
                cameraComponent.rect = rect;
            }
            // add pillar-box
            else
            {
                var scaleWidth = 1.0f / scaleHeight;

                Rect rect = cameraComponent.rect;

                rect.width = scaleWidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scaleWidth) / 2.0f;
                rect.y = 0;

                cameraComponent.rect = rect;
            }
        }
    }
}
