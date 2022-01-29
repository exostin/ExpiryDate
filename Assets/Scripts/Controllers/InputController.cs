using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

            if (kb.f2Key.wasReleasedThisFrame)
            {
                gm.stateController.fsm.ChangeState(StateController.States.PlayerTurn);
                SceneManager.LoadScene(2);
            }
        }
    }
}
