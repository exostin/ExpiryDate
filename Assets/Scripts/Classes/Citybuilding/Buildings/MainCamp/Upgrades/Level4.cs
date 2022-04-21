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
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);
            simulation.Housing.Upgrades[3].Unlocked = true;
            foreach (var generatorBuilding in simulation.GeneratorBuildings) generatorBuilding.OutputMultiplier *= 1.3f;
        }
    }
}