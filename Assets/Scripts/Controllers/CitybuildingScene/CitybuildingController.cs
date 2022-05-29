using System;
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

        [SerializeField] public Texture2D cursorPointerTexture;

        private Manager cbm;
        private GameManager gm;
        private Camera mainCamera;

        public Dictionary<Building, GameObject> BuildingsGameObjects => new()
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

        public Building SelectedBuilding { get; private set; }

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
        }

        public void UpdateModels()
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

                    Debug.Log($"Showing {child.name}");
                    child.gameObject.SetActive(true);
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
            SceneManager.LoadScene("DefenderSelection");
        }

        public void NextDay(bool skipFight)
        {
            cbm.OnNextDay();
            if (!skipFight && cbm.NextEncounter == 0)
            {
                EnterBattleMode();
            }
        }

        public void NextDay()
        {
            cbm.Save();
            NextDay(false);
        }

        public Building GameObjectToBuilding(GameObject go)
        {
            foreach (var building in BuildingsGameObjects)
                if (building.Value == go)
                    return building.Key;
            return null;
        }

        private GameObject BuildingToGameObject(Building building)
        {
            return BuildingsGameObjects[building];
        }

        public event Action<Building> OnBuildingSelected;

        public void SelectBuilding(Building building)
        {
            SelectedBuilding = building;
            OnBuildingSelected?.Invoke(building);
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