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
        public Manager cbm;
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

        public void OnNextDay(Manager cityBuildingManager)
        {
            foreach (Building building in Buildings)
            {
                building.cbm = cityBuildingManager;
                building.OnNextDay();
            }
        }

        public void BuyDefender(DefenderType defenderType)
        {
            // TODO: Make custom exceptions that future ui can catch

            if (!Simulated) throw new InvalidOperationException("Simulation must be run before buying defenders.");
            if (Defenders[defenderType].Amount >= 3)
                throw new InvalidOperationException("You can't buy more than 3 defenders of the same type.");
            if (cbm.DefenderBought) throw new InvalidOperationException("You can buy only one defender per round.");
            if (Defenders[defenderType].ActualCost > cbm.PlayerResources)
                throw new InvalidOperationException("Not enough resources.");

            cbm.PlayerResources -= Defenders[defenderType].ActualCost;
            Defenders[defenderType].Amount++;
            cbm.DefenderBought = true;
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