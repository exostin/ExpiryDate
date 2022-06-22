using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class InputController : MonoBehaviour
    {
        private GameManager gm;
        private Keyboard kb;

        private void Start()
        {
            gm = FindObjectOfType<GameManager>();
            kb = InputSystem.GetDevice<Keyboard>();
        }

        private void Update()
        {
            if (kb.escapeKey.wasReleasedThisFrame) gm.TogglePauseMenu();
        }
    }
}