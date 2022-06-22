using UnityEngine;

namespace Controllers.BattleScene
{
    public class MoveActiveCharacterToCenter : MonoBehaviour
    {
        private readonly float spotlightLocationForEnemies = -1000f;
        private readonly float spotlightLocationForPlayerCharacters = 600f;
        private Vector3 originalPosition;
        [SerializeField] private GameObject activeCharacterIndicator;

        private void Start()
        {
            originalPosition = gameObject.transform.position;
        }
        
        public void MoveToCenter(int option)
        {
            gameObject.transform.localPosition = option == 1
                ? new Vector2(spotlightLocationForPlayerCharacters, gameObject.transform.localPosition.y)
                : new Vector2(spotlightLocationForEnemies, gameObject.transform.localPosition.y);
            activeCharacterIndicator.SetActive(true);
        }

        public void MoveBack()
        {
            gameObject.transform.position = originalPosition;
            activeCharacterIndicator.SetActive(false);
        }
    }
}