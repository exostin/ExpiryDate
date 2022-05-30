using System;using System.Linq;
using Classes.Citybuilding;
using Classes.Citybuilding.Buildings.DroneSchool;
using Classes.Citybuilding.Buildings.FighterSchool;
using Classes.Citybuilding.Buildings.RobotSchool;
using Classes.Citybuilding.Buildings.ShooterSchool;
using Controllers;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class BuildingPanelController : MonoBehaviour
{
    [CanBeNull] private Building building;
    
    private CitybuildingController cbc;
    private Manager cbm;
    
    [SerializeField] private TextMeshProUGUI nextUpgradeText;
    [SerializeField] private TextMeshProUGUI acquiredUpgradesText;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button buyDefenderButton;

    private void Start()
    {
        cbc = FindObjectOfType<CitybuildingController>();
        cbm = FindObjectOfType<GameManager>().cbm;

        cbc.OnBuildingSelected += CbcOnOnBuildingSelected;
        // I don't think that we need to unsubscribe from this event in OnDestroy because BuildingPanelController should
        // be destroyed when the CityBuildingController is destroyed (when scene ends).
    }

    private void CbcOnOnBuildingSelected(Building newBuilding)
    {
        building = newBuilding; 

        acquiredUpgradesText.text = building!.BoughtUpgrades.Aggregate("",
            (acc, upgrade) => upgrade.Description is not null ? acc + upgrade.Description + "\n" : "");
        nextUpgradeText.text = building.NextUpgrade is null ? "No upgrades available" : building.NextUpgrade.Description;
        
        upgradeButton.interactable = building.CanBeUpgraded;

        var isASchool = building is DroneSchool or FighterSchool or ShooterSchool or RobotSchool;
        buyDefenderButton.interactable = isASchool;
    }

    public void UpgradeButtonClicked()
    {
        // To explain what's going on here:
        // When a building is upgraded, new simulation is created and the old one is destroyed.
        // That means that our reference to the building is no longer valid.
        // So we get index of the building in the list of buildings and use it to get the new reference.
        // Btw list of buildings is generated by CityBuildingController, so it's always up to date.
        // This is a hack and I hate it.
        
        var index = cbc.BuildingsGameObjects.Keys.ToList().IndexOf(building);
        
        building?.Upgrade();
        cbc.UpdateModels();
        
        cbc.SelectBuilding(cbc.BuildingsGameObjects.Keys.ToList()[index]);
    }

    public void BuyDefenderButtonClicked()
    {
        if (building?.DefenderType is null) return;
        try
        {
            cbm.Simulation.BuyDefender(building.DefenderType);
        }
        catch (InvalidOperationException e)
        {

            #if UNITY_EDITOR
            EditorUtility.DisplayDialog("Cannot buy defender", e.Message, "OK");
            #endif

        }
    }
}