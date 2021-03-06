namespace Classes.Citybuilding.Buildings.MainCamp.Upgrades
{
    public class Level4 : BuildingUpgrade
    {
        public Level4()
        {
            Level = 4;
            BaseCost = new Resources
            {
                Energy = 120,
                Food = 120,
                Titan = 140,
                Water = 100
            };
            ModelName = "MainCamp4";
            Description =
                "Town Hall now hosts birthday parties (Unlocks fourth tier of living quarters, additionally, buff from living quarters is further increased by 1.3x)";
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);
            simulation.Housing.Upgrades[4].Unlocked = true;
            foreach (var generatorBuilding in simulation.GeneratorBuildings) generatorBuilding.OutputMultiplier *= 1.3f;
        }
    }
}