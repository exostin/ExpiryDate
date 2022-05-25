namespace Classes.Citybuilding.Buildings.DroneSchool.Upgrades
{
    public class Level4 : BuildingUpgrade
    {
        public Level4()
        {
            Level = 4;
            BaseCost = new Resources
            {
                Food = 50,
                Titan = 20,
                Energy = 50
            };
            ModelName = "DroneSchool4";
            Description = "Drone School Level 4";
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Drone].Tier = 3;
            building.cbm.Defenders[DefenderType.Drone].CostMultiplier = .8f;
        }
    }
}