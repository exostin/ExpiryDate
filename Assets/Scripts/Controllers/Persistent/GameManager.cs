using System.Collections.Generic;
using System.Linq;
using Classes;
using Classes.Citybuilding;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Resources = UnityEngine.Resources;

namespace Controllers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Characters")]
        public List<Character> selectedDefenders;
        [Tooltip("Third character will always be the BOSS!!!")]
        public List<Character> thisEncounterEnemies;
        [Header("Controllers")]
        public StateController stateController;
        
        #region DefenderSelection
        public int lastEnemyTeam;
        #endregion

        #region Start/Pause menu
        [Header("Other")]
        [SerializeField] private GameObject pauseMenu;

        #endregion

        private void Start()
        {
            Resources.FindObjectsOfTypeAll<SettingsMenuController>()
                .FirstOrDefault(g => g.CompareTag("PauseMenuSettingsMenu"))!
                .GetComponent<SettingsMenuController>()
                .Initialize();

            //stateController = GameObject.FindGameObjectWithTag("StateController").GetComponent<StateController>();
            stateController = FindObjectOfType<StateController>();

            cbm = new Manager();

            #region Copy costs of defenders from scriptable objects to cbm

            var characterDefenders = new Dictionary<Character, Defender>
            {
                {fighterCharacter, cbm.Defenders[DefenderType.Fighter]},
                {shooterCharacter, cbm.Defenders[DefenderType.Shooter]},
                {medicCharacter, cbm.Defenders[DefenderType.Medic]},
                {robotCharacter, cbm.Defenders[DefenderType.Mech]},
                {droneCharacter, cbm.Defenders[DefenderType.Drone]}
            };

            foreach (var (character, defender) in characterDefenders)
                defender.BaseCost = new Classes.Citybuilding.Resources
                {
                    Energy = character.costEnergy,
                    Food = character.costFood,
                    Titan = character.costTitan,
                    Water = character.costWater
                };

            #endregion
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

        #region CityBuilding

        [SerializeField] private Character fighterCharacter;
        [SerializeField] private Character shooterCharacter;
        [SerializeField] private Character medicCharacter;
        [SerializeField] private Character robotCharacter;
        [SerializeField] private Character droneCharacter;

        public Manager cbm;

        #endregion

        public void LoadMainMenu()
        {
            SceneManager.LoadScene("Scenes/StartMenu");
        }
    }
}