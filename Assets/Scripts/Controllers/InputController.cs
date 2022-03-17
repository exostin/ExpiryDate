using UnityEngine;
using UnityEngine.InputSystem;


namespace Controllers
{
    public class InputController : MonoBehaviour
    {
        private Keyboard kb;
        private GameManager gm;

        void Start()
        {
            gm = FindObjectOfType<GameManager>();
            kb = InputSystem.GetDevice<Keyboard>();
        }
    
        void Update()
        {
            if (kb.escapeKey.wasReleasedThisFrame)
            {
                gm.TogglePauseMenu();
            }
        }
    }
}
