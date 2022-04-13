using UnityEngine;

namespace Controllers
{
    public class MoveActiveCharacterToCenter : MonoBehaviour
    {
        private Vector3 originalPosition;

        private void Start()
        {
            originalPosition = gameObject.transform.position;
        }
        
        
        public void MoveToCenter(int option)
        {
            // AD HOC, TO BE CHANGED ASAP
            gameObject.transform.localPosition = option == 1 ? new Vector2(897, gameObject.transform.localPosition.y) : new Vector2(-810, gameObject.transform.localPosition.y);
        }

        public void MoveBack()
        {
            gameObject.transform.position = originalPosition;
        }
    }
}