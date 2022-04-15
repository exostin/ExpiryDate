namespace Classes.Citybuilding.Buildings.Housing.Upgrades
{
    public class Level1 : BuildingUpgrade
    {
        public Level1()
        {
            Level = 1;
            ModelName = "Housing1";
            BaseCost = new Resources
            {
                Titan = 150,
                Water = 100,
                Food = 80
            };
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);
            foreach (var generatorBuilding in simulation.GeneratorBuildings) generatorBuilding.OutputMultiplier *= 1.2f;
        }
    }
}