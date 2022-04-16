using System.Collections.Generic;
using System.Linq;
using Classes.Citybuilding;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Resources = Classes.Citybuilding.Resources;

namespace Controllers
{
    public class CitybuildingController : MonoBehaviour
    {
        [SerializeField] private GameObject citybuilding;

        [SerializeField] private GameObject titanAmount;
        [SerializeField] private GameObject waterAmount;
        [SerializeField] private GameObject energyAmount;
        [SerializeField] private GameObject foodAmount;

        private Manager cbm;
        private GameManager gm;
        private Camera mainCamera;

        private void Start()
        {
            gm = FindObjectOfType<GameManager>();
            if (gm == null)
            {
                Debug.Log("GameManager not found. Loading Main Menu.");
                SceneManager.LoadScene(0);
                return;
            }

            cbm = gm.cbm;
            mainCamera = Camera.main;

            UpdateModels();
        }

        private void Update()
        {
            if (gm == null) return;

            #region Updating UI

            titanAmount.GetComponent<TextMeshProUGUI>().text = cbm.PlayerResources.Titan.ToString();
            waterAmount.GetComponent<TextMeshProUGUI>().text = cbm.PlayerResources.Water.ToString();
            energyAmount.GetComponent<TextMeshProUGUI>().text = cbm.PlayerResources.Energy.ToString();
            foodAmount.GetComponent<TextMeshProUGUI>().text = cbm.PlayerResources.Food.ToString();

            #endregion

            #region Mouse input for upgrading buildings

            if (mainCamera == null) mainCamera = Camera.main;
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out var hit))
            {
                var colliderGameObject = hit.collider.gameObject;

                if (colliderGameObject == mainCamp)
                {
                    cbm.Simulation.MainCamp.Upgrade();
                    UpdateModels();
                }
                else if (colliderGameObject == droneSchool)
                {
                    cbm.Simulation.DroneSchool.Upgrade();
                    UpdateModels();
                }
                else if (colliderGameObject == fighterSchool)
                {
                    cbm.Simulation.FighterSchool.Upgrade();
                    UpdateModels();
                }
                else if (colliderGameObject == robotSchool)
                {
                    cbm.Simulation.RobotSchool.Upgrade();
                    UpdateModels();
                }
                else if (colliderGameObject == medicSchool)
                {
                    cbm.Simulation.MedicSchool.Upgrade();
                    UpdateModels();
                }
                else if (colliderGameObject == titanGenerator)
                {
                    cbm.Simulation.TitanGenerator.Upgrade();
                    UpdateModels();
                }
                else if (colliderGameObject == waterGenerator)
                {
                    cbm.Simulation.WaterGenerator.Upgrade();
                    UpdateModels();
                }
                else if (colliderGameObject == energyGenerator)
                {
                    cbm.Simulation.EnergyGenerator.Upgrade();
                    UpdateModels();
                }
                else if (colliderGameObject == foodGenerator)
                {
                    cbm.Simulation.FoodGenerator.Upgrade();
                    UpdateModels();
                }
                else if (colliderGameObject == housing)
                {
                    cbm.Simulation.Housing.Upgrade();
                    UpdateModels();
                }

                Debug.Log($"{colliderGameObject.name} was clicked.");
            }

            #endregion
        }

        private void UpdateModels()
        {
            #region Hide all buildings

            var allBuildingsModelNames =
                (from building in cbm.Simulation.Buildings from upgrade in building.Upgrades select upgrade.ModelName)
                .ToList();

            foreach (var modelName in allBuildingsModelNames)
                if (citybuilding.transform.Find(modelName) is null)
                    Debug.LogWarning($"{modelName} is missing!");

            foreach (Transform child in citybuilding.transform)
                if (allBuildingsModelNames.Contains(child.name))
                {
                    child.gameObject.SetActive(false);
                    Debug.Log("Hiding " + child.name);
                }

            #endregion

            #region Show used buildings (and assign them to variables)

            var usedBuildingsModelNames = cbm.Simulation.Buildings.ToList()
                .Select(building => building.CurrentUpgrade.ModelName).ToList();

            foreach (Transform child in citybuilding.transform)
                if (usedBuildingsModelNames.Contains(child.name))
                {
                    Debug.Log($"Showing {child.name}");

                    child.gameObject.SetActive(true);
                    if (child.name.StartsWith("DroneSchool")) droneSchool = child.gameObject;
                    else if (child.name.StartsWith("EnergyGenerator")) energyGenerator = child.gameObject;
                    else if (child.name.StartsWith("FighterSchool")) fighterSchool = child.gameObject;
                    else if (child.name.StartsWith("FoodGenerator")) foodGenerator = child.gameObject;
                    else if (child.name.StartsWith("MainCamp")) mainCamp = child.gameObject;
                    else if (child.name.StartsWith("MedicSchool")) medicSchool = child.gameObject;
                    else if (child.name.StartsWith("RobotSchool")) robotSchool = child.gameObject;
                    else if (child.name.StartsWith("ShooterSchool")) shooterSchool = child.gameObject;
                    else if (child.name.StartsWith("TitanGenerator")) titanGenerator = child.gameObject;
                    else if (child.name.StartsWith("WaterGenerator")) waterGenerator = child.gameObject;
                    else if (child.name.StartsWith("Housing")) housing = child.gameObject;
                }

            #endregion

            #region Check if all buildings have models

            if (droneSchool == null || energyGenerator == null || fighterSchool == null || foodGenerator == null ||
                mainCamp == null || medicSchool == null || robotSchool == null || shooterSchool == null ||
                titanGenerator == null || waterGenerator == null || mainCamp == null)
                Debug.LogError("Not all buildings were found.");

            #endregion

            Debug.Log(cbm.DebugStatus);
        }

        public void EnterBattleMode()
        {
            cbm.Save();
            SceneManager.LoadScene(2);
        }

        public void NextDay(bool skipFight)
        {
            cbm.OnNextDay();
            if (!skipFight) EnterBattleMode();
        }

        public void NextDay()
        {
            cbm.Save();
            NextDay(false);
        }
        
        private Dictionary<Building, GameObject> BuildingsGameObjects => new Dictionary<Building, GameObject>()
        {
            {cbm.Simulation.Housing, housing},
            {cbm.Simulation.DroneSchool, droneSchool},
            {cbm.Simulation.FighterSchool, fighterSchool},
            {cbm.Simulation.MedicSchool, medicSchool},
            {cbm.Simulation.RobotSchool, robotSchool},
            {cbm.Simulation.ShooterSchool, shooterSchool},
            {cbm.Simulation.TitanGenerator, titanGenerator},
            {cbm.Simulation.WaterGenerator, waterGenerator},
            {cbm.Simulation.EnergyGenerator, energyGenerator},
            {cbm.Simulation.FoodGenerator, foodGenerator},
            {cbm.Simulation.MainCamp, mainCamp}
        };
        
        private Building GameObjectToBuilding(GameObject go)
        {
            foreach (var building in BuildingsGameObjects)
                if (building.Value == go) return building.Key;
            return null;
        }
        
        private GameObject BuildingToGameObject(Building building)
        {
            return BuildingsGameObjects[building];
        }

        public void DebugGiveResources()
        {
            cbm.PlayerResources += new Resources {Titan = 100, Water = 100, Energy = 100, Food = 100};
        }

        #region Buildings GameObjects

        private GameObject housing;
        private GameObject mainCamp;
        private GameObject titanGenerator;
        private GameObject energyGenerator;
        private GameObject waterGenerator;
        private GameObject foodGenerator;
        private GameObject fighterSchool;
        private GameObject shooterSchool;
        private GameObject robotSchool;
        private GameObject droneSchool;
        private GameObject medicSchool;

        #endregion
    }
}