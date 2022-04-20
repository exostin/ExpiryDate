using System.Collections.Generic;
using Classes.Citybuilding.Buildings.DroneSchool;
using Classes.Citybuilding.Buildings.EnergyGenerator;
using Classes.Citybuilding.Buildings.FighterSchool;
using Classes.Citybuilding.Buildings.FoodGenerator;
using Classes.Citybuilding.Buildings.Housing;
using Classes.Citybuilding.Buildings.MainCamp;
using Classes.Citybuilding.Buildings.MedicSchool;
using Classes.Citybuilding.Buildings.RobotSchool;
using Classes.Citybuilding.Buildings.ShooterSchool;
using Classes.Citybuilding.Buildings.TitanGenerator;
using Classes.Citybuilding.Buildings.WaterGenerator;
using UnityEngine;

namespace Classes.Citybuilding
{
    public class Manager
    {
        public bool DefenderBought;

        public Dictionary<DefenderType, Defender> Defenders = new()
        {
            {DefenderType.Drone, new Defender(DefenderType.Drone)},
            {DefenderType.Fighter, new Defender(DefenderType.Fighter)},
            {DefenderType.Robot, new Defender(DefenderType.Robot)},
            {DefenderType.Shooter, new Defender(DefenderType.Shooter)},
            {DefenderType.Medic, new Defender(DefenderType.Medic)}
        };

        public Resources PlayerResources;

        public Simulation Simulation;

        public string DebugStatus =>
            $"FighterSchool: {fighterSchoolLevel} ShooterSchool: {shooterSchoolLevel} RobotSchool: {robotSchoolLevel} " +
            $"DroneSchool: {droneSchoolLevel} MedicSchool: {medicSchoolLevel} MainCamp: {mainCampLevel} " +
            $"FoodGenerator: {foodGeneratorLevel} WaterGenerator: {waterGeneratorLevel} " +
            $"EnergyGenerator: {energyGeneratorLevel} TitanGenerator: {titanGeneratorLevel}\n" +
            $"PlayerResources: Titan: {PlayerResources.Titan} Energy: {PlayerResources.Energy} " +
            $"Food: {PlayerResources.Food} Water: {PlayerResources.Water}\n" +
            $"Defenders: Drone: {Defenders[DefenderType.Drone].Amount} Fighter: {Defenders[DefenderType.Fighter].Amount} " +
            $"Robot: {Defenders[DefenderType.Robot].Amount} Shooter: {Defenders[DefenderType.Shooter].Amount} " +
            $"Medic: {Defenders[DefenderType.Medic].Amount}";

        public void Load()
        {
            PlayerResources = new Resources
            {
                Titan = PlayerPrefs.GetInt("PlayerResources/Titan", 0),
                Energy = PlayerPrefs.GetInt("PlayerResources/Energy", 0),
                Food = PlayerPrefs.GetInt("PlayerResources/Food", 0),
                Water = PlayerPrefs.GetInt("PlayerResources/Water", 0)
            };

            housingLevel = PlayerPrefs.GetInt("PlayerBuildings/Housing", 0);
            titanGeneratorLevel = PlayerPrefs.GetInt("PlayerBuildings/TitanGenerator", 1);
            energyGeneratorLevel = PlayerPrefs.GetInt("PlayerBuildings/EnergyGenerator", 1);
            waterGeneratorLevel = PlayerPrefs.GetInt("PlayerBuildings/WaterGenerator", 1);
            foodGeneratorLevel = PlayerPrefs.GetInt("PlayerBuildings/FoodGenerator", 1);
            fighterSchoolLevel = PlayerPrefs.GetInt("PlayerBuildings/FighterSchool", 1);
            shooterSchoolLevel = PlayerPrefs.GetInt("PlayerBuildings/ShooterSchool", 1);
            robotSchoolLevel = PlayerPrefs.GetInt("PlayerBuildings/RobotSchool", 1);
            droneSchoolLevel = PlayerPrefs.GetInt("PlayerBuildings/DroneSchool", 1);
            medicSchoolLevel = PlayerPrefs.GetInt("PlayerBuildings/MedicSchool", 1);
            mainCampLevel = PlayerPrefs.GetInt("PlayerBuildings/MainCamp", 1);

            RunSimulation();
        }

        public void Save()
        {
            PlayerPrefs.SetInt("PlayerResources/Titan", PlayerResources.Titan);
            PlayerPrefs.SetInt("PlayerResources/Energy", PlayerResources.Energy);
            PlayerPrefs.SetInt("PlayerResources/Food", PlayerResources.Food);
            PlayerPrefs.SetInt("PlayerResources/Water", PlayerResources.Water);

            PlayerPrefs.SetInt("PlayerBuildings/Housing", housingLevel);
            PlayerPrefs.SetInt("PlayerBuildings/TitanGenerator", titanGeneratorLevel);
            PlayerPrefs.SetInt("PlayerBuildings/EnergyGenerator", energyGeneratorLevel);
            PlayerPrefs.SetInt("PlayerBuildings/WaterGenerator", waterGeneratorLevel);
            PlayerPrefs.SetInt("PlayerBuildings/FoodGenerator", foodGeneratorLevel);
            PlayerPrefs.SetInt("PlayerBuildings/FighterSchool", fighterSchoolLevel);
            PlayerPrefs.SetInt("PlayerBuildings/ShooterSchool", shooterSchoolLevel);
            PlayerPrefs.SetInt("PlayerBuildings/RobotSchool", robotSchoolLevel);
            PlayerPrefs.SetInt("PlayerBuildings/DroneSchool", droneSchoolLevel);
            PlayerPrefs.SetInt("PlayerBuildings/MedicSchool", medicSchoolLevel);
        }

        public void RunSimulation()
        {
            Simulation = new Simulation
            {
                Housing = new Housing(housingLevel, newLevel => { housingLevel = newLevel; }) {cbm = this},
                TitanGenerator =
                    new TitanGenerator(titanGeneratorLevel, newLevel => { titanGeneratorLevel = newLevel; })
                        {cbm = this},
                EnergyGenerator =
                    new EnergyGenerator(energyGeneratorLevel, newLevel => { energyGeneratorLevel = newLevel; })
                        {cbm = this},
                WaterGenerator =
                    new WaterGenerator(waterGeneratorLevel, newLevel => { waterGeneratorLevel = newLevel; })
                        {cbm = this},
                FoodGenerator =
                    new FoodGenerator(foodGeneratorLevel, newLevel => { foodGeneratorLevel = newLevel; }) {cbm = this},
                DroneSchool = new DroneSchool(droneSchoolLevel,
                    newLevel => { droneSchoolLevel = newLevel; }) {cbm = this},
                FighterSchool = new FighterSchool(fighterSchoolLevel, newLevel => { fighterSchoolLevel = newLevel; })
                    {cbm = this},
                ShooterSchool = new ShooterSchool(shooterSchoolLevel, newLevel => { shooterSchoolLevel = newLevel; })
                    {cbm = this},
                RobotSchool = new RobotSchool(robotSchoolLevel, newLevel => { robotSchoolLevel = newLevel; })
                    {cbm = this},
                MedicSchool = new MedicSchool(medicSchoolLevel, newLevel => { medicSchoolLevel = newLevel; })
                    {cbm = this},
                MainCamp = new MainCamp(mainCampLevel, newLevel => { mainCampLevel = newLevel; }) {cbm = this},
                Defenders = Defenders,
                cbm = this
            };
            Simulation.Run();
        }

        public void OnNextDay()
        {
            DefenderBought = false;
            RunSimulation();
            Simulation.OnNextDay(this);
        }

        #region Levels

        private int droneSchoolLevel;
        private int energyGeneratorLevel;
        private int fighterSchoolLevel;
        private int foodGeneratorLevel;
        private int housingLevel;
        private int medicSchoolLevel;
        private int robotSchoolLevel;
        private int shooterSchoolLevel;
        private int titanGeneratorLevel;
        private int waterGeneratorLevel;
        private int mainCampLevel;

        #endregion
    }
}