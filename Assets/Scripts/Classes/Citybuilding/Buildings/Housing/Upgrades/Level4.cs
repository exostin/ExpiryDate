namespace Classes.Citybuilding.Buildings.Housing.Upgrades
{
    public class Level4 : BuildingUpgrade
    {
        public Level4()
        {
            Level = 4;
            ModelName = "Housing4";
            Description =
                "Living Quarters are now filled up with laughing gas (All other building produce 3 more of their respective material)";
            BaseCost = new Resources
            {
                Titan = 100,
                Water = 80,
                Food = 60,
                Energy = 50
            };
            Unlocked = false;
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);
            foreach (var generatorBuilding in simulation.GeneratorBuildings) generatorBuilding.OutputBonus += 3;
        }
    }
}