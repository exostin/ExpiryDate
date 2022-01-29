using UnityEngine;

namespace Controllers
{
    public class GameManager : MonoBehaviour
    {
        // Definitely could be done better, but the milestone is approaching and I had to do it ad hoc
        public StateController stateController;

        [SerializeField] private GameObject pauseMenu;

        private void Start()
        {
            stateController = GameObject.FindGameObjectWithTag("StateController").GetComponent<StateController>();
        }

        public void TogglePauseMenu()
        {
            if (!pauseMenu.activeInHierarchy)
            {
                stateController.fsm.ChangeState(StateController.States.Pause);
            }
            else
            {
                stateController.fsm.ChangeState(StateController.States.Playing);
            }
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }
}
