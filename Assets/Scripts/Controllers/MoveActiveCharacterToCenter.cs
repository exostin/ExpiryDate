using UnityEngine;

namespace Controllers
{
    public class MoveActiveCharacterToCenter : MonoBehaviour
    {
        private Vector3 originalPosition;
        private GameObject self;

        private void Start()
        {
            self = gameObject;

            originalPosition = self.transform.position;
        }
        
        
        public void MoveToCenter(int option)
        {
            // AD HOC, TO BE CHANGED ASAP
            self.transform.localPosition = option == 1 ? new Vector2(897, self.transform.localPosition.y) : new Vector2(-810, self.transform.localPosition.y);
        }

        public void MoveBack()
        {
            self.transform.position = originalPosition;
        }
    }
}