using UnityEngine;

namespace Controllers
{
    public class MoveActiveCharacterToCenter : MonoBehaviour
    {
        private Vector3 originalPosition;
        private readonly float spotlightLocationForPlayerCharacters = 550f;
        private readonly float spotlightLocationForEnemies = -1000f;

        private void Start()
        {
            originalPosition = gameObject.transform.position;
        }
        
        
        public void MoveToCenter(int option)
        {
            // AD HOC, TO BE CHANGED ASAP 
            gameObject.transform.localPosition = option == 1 ? new Vector2(spotlightLocationForPlayerCharacters, gameObject.transform.localPosition.y) : new Vector2(spotlightLocationForEnemies, gameObject.transform.localPosition.y);
        }

        public void MoveBack()
        {
            gameObject.transform.position = originalPosition;
        }
    }
}