namespace Classes.Citybuilding.Buildings.EnergyGenerator.Upgrades
{
    public class Level4 : GeneratorBuildingUpgrade
    {
        public Level4()
        {
            Level = 4;
            ModelName = "EnergyGenerator4";
            Description =
                "Multiple hamsters and wheels are added for maximum efficiency (Statistics of Support Drones are increased by 10%)";
            BaseCost = new Resources
            {
                Titan = 20,
                Water = 40,
                Food = 50
            };
            Output = new Resources();
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Drone].StatsMultiplier = 1.1f;
        }
    }
}