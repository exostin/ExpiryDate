using System.Linq;
using Classes;
using ScriptableObjects;
using UnityEngine;
using Resources = UnityEngine.Resources;

namespace Controllers
{
    public class GameManager : MonoBehaviour
    {
        // Definitely could be done better, but the milestone is approaching and I had to do it ad hoc
        public StateController stateController;

        [SerializeField] private GameObject pauseMenu;

        [Header("Citybuilding ScriptableObjects")] [SerializeField]
        private Building citybuildingMainCamp;

        [SerializeField] private Building citybuildingTitanGenerator;
        [SerializeField] private Building citybuildingEnergyGenerator;
        [SerializeField] private Building citybuildingWaterGenerator;
        [SerializeField] private Building citybuildingFoodGenerator;
        [SerializeField] private Building citybuildingFighterSchool;
        [SerializeField] private Building citybuildingShooterSchool;
        [SerializeField] private Building citybuildingDroneSchool;
        [SerializeField] private Building citybuildingMedicSchool;
        [SerializeField] private Building citybuildingRobotSchool;
        public CitybuildingManager cbm;

        private void Start()
        {
            Resources.FindObjectsOfTypeAll<SettingsMenuController>()
                .FirstOrDefault(g => g.CompareTag("PauseMenuSettingsMenu"))!
                .GetComponent<SettingsMenuController>()
                .Initialize();
            stateController = GameObject.FindGameObjectWithTag("StateController").GetComponent<StateController>();
            cbm = new CitybuildingManager
            {
                MainCamp = citybuildingMainCamp,
                TitanGenerator = citybuildingTitanGenerator,
                EnergyGenerator = citybuildingEnergyGenerator,
                WaterGenerator = citybuildingWaterGenerator,
                FoodGenerator = citybuildingFoodGenerator,
                FighterSchool = citybuildingFighterSchool,
                ShooterSchool = citybuildingShooterSchool,
                DroneSchool = citybuildingDroneSchool,
                MedicSchool = citybuildingMedicSchool,
                RobotSchool = citybuildingRobotSchool
            };
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