namespace Classes.Citybuilding.Buildings.DroneSchool.Upgrades
{
    public class Level2 : BuildingUpgrade
    {
        public Level2()
        {
            Level = 2;
            BaseCost = new Resources
            {
                Food = 50,
                Titan = 20,
                Energy = 50
            };
            ModelName = "DroneSchool2";
            Description =
                "Support Drones are regularly checked for damage (Unlocks Support Drone Tier 1)";
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Drone].Tier = 1;
        }
    }
}