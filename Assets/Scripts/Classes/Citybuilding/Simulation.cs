using System;
using System.Collections.Generic;
using System.Linq;
using Classes.Citybuilding.Buildings;
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

namespace Classes.Citybuilding
{
    public class Simulation
    {
        public Dictionary<DefenderType, Defender> Defenders;
        public bool Simulated;

        public Building[] Buildings => new Building[]
        {
            Housing,
            EnergyGenerator,
            WaterGenerator,
            FoodGenerator,
            TitanGenerator,
            MedicSchool,
            RobotSchool,
            ShooterSchool,
            FighterSchool,
            DroneSchool,
            MainCamp
        };

        public GeneratorBuilding[] GeneratorBuildings => new GeneratorBuilding[]
        {
            TitanGenerator,
            EnergyGenerator,
            WaterGenerator,
            FoodGenerator
        };

        public void Run()
        {
            if (Simulated) throw new InvalidOperationException("Simulation can only be run once.");
            foreach (var defender in Defenders.Select(pair => pair.Value))
            {
                defender.Tier = -1;
                defender.CostMultiplier = 1f;
                defender.StatsMultiplier = 1f;
            }

            foreach (var building in Buildings) building.ApplySideEffects(this);
            Simulated = true;
        }

        public void OnNextDay(Manager cbm)
        {
            foreach (var building in Buildings)
            {
                building.cbm = cbm;
                building.OnNextDay();
            }
        }

        #region Buildings

        public Housing Housing;
        public EnergyGenerator EnergyGenerator;
        public WaterGenerator WaterGenerator;
        public FoodGenerator FoodGenerator;
        public TitanGenerator TitanGenerator;
        public MedicSchool MedicSchool;
        public RobotSchool RobotSchool;
        public ShooterSchool ShooterSchool;
        public FighterSchool FighterSchool;
        public DroneSchool DroneSchool;
        public MainCamp MainCamp;

        #endregion
    }
}