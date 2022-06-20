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
            Description =
                "Support Drones achieved sentience making their movement more unpredictable for enemies (Unlocks Support Drones Tier 3  + Their cost is 20% lower)";
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Drone].Tier = 3;
            building.cbm.Defenders[DefenderType.Drone].CostMultiplier = .8f;
        }
    }
}