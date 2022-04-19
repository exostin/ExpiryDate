using System.Linq;
using Classes.Citybuilding;
using UnityEngine;
using Resources = UnityEngine.Resources;

namespace Controllers
{
    public class GameManager : MonoBehaviour
    {
        public StateController stateController;

        [SerializeField] private GameObject pauseMenu;

        public Manager cbm;

        private void Start()
        {
            Resources.FindObjectsOfTypeAll<SettingsMenuController>()
                .FirstOrDefault(g => g.CompareTag("PauseMenuSettingsMenu"))!
                .GetComponent<SettingsMenuController>()
                .Initialize();
            //stateController = GameObject.FindGameObjectWithTag("StateController").GetComponent<StateController>();
            stateController = FindObjectOfType<StateController>();
            cbm = new Manager();
        }

        public void TogglePauseMenu()
        {
            stateController.fsm.ChangeState(!pauseMenu.activeInHierarchy
                ? StateController.States.Pause
                : StateController.States.Playing);

            pauseMenu.transform.Find("Settings menu").gameObject.SetActive(false);
            pauseMenu.transform.Find("Start menu").gameObject.SetActive(true);
            if (pauseMenu.activeSelf)
                Resources.FindObjectsOfTypeAll<SettingsMenuController>()
                    .FirstOrDefault(g => g.CompareTag("PauseMenuSettingsMenu"))!
                    .GetComponent<SettingsMenuController>().SavePreferences();
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }
}