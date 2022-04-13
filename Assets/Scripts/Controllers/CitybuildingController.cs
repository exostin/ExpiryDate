using Classes;
using Classes.Citybuilding;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class CitybuildingController : MonoBehaviour
    {
        private GameObject droneSchool;

        private GameObject energyGenerator;
        private GameObject fighterSchool;
        private GameObject foodGenerator;
        private GameObject mainCamp;
        private GameObject medicSchool;
        private GameObject robotSchool;
        private GameObject shooterSchool;
        private GameObject titanGenerator;
        private GameObject waterGenerator;

        [Header("Resources UI GameObjects")] [SerializeField]
        private GameObject titanAmount;

        [SerializeField] private GameObject waterAmount;
        [SerializeField] private GameObject energyAmount;
        [SerializeField] private GameObject foodAmount;

        private Manager cbm;
        private GameManager gm;
        private Camera mainCamera;

        private void Start()
        {
            gm = FindObjectOfType<GameManager>();
            cbm = gm.cbm;
            mainCamera = Camera.main;
        }

        private void Update()
        {
            #region Updating UI

            titanAmount.GetComponent<TextMeshProUGUI>().text = cbm.PlayerResources.titan.ToString();
            waterAmount.GetComponent<TextMeshProUGUI>().text = cbm.PlayerResources.water.ToString();
            energyAmount.GetComponent<TextMeshProUGUI>().text = cbm.PlayerResources.energy.ToString();
            foodAmount.GetComponent<TextMeshProUGUI>().text = cbm.PlayerResources.food.ToString();

            #endregion

            #region Mouse input for upgrading buildings

            if (mainCamera == null) mainCamera = Camera.main;
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out var hit))
            {
                var colliderGameObject = hit.collider.gameObject;

                if (colliderGameObject == energyGenerator) cbm.UpgradeEnergyGenerator();
                else if (colliderGameObject == fighterSchool) cbm.UpgradeFighterSchool();
                else if (colliderGameObject == foodGenerator) cbm.UpgradeFoodGenerator();
                else if (colliderGameObject == mainCamp) cbm.UpgradeMainCamp();
                else if (colliderGameObject == medicSchool) cbm.UpgradeMedicSchool();
                else if (colliderGameObject == robotSchool) cbm.UpgradeRobotSchool();
                else if (colliderGameObject == shooterSchool) cbm.UpgradeShooterSchool();
                else if (colliderGameObject == titanGenerator) cbm.UpgradeTitanGenerator();
                else if (colliderGameObject == waterGenerator) cbm.UpgradeWaterGenerator();
                else if (colliderGameObject == droneSchool) cbm.UpgradeDroneSchool();
            }

            #endregion
        }

        public void EnterBattleMode()
        {
            cbm.Save();
            gm.stateController.fsm.ChangeState(StateController.States.PlayerTurn);
            SceneManager.LoadScene(2);
        }

        public void NextDay()
        {
            cbm.NextDay();
            EnterBattleMode();
        }
    }
}