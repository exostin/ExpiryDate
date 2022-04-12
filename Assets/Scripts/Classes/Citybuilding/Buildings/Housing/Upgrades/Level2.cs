namespace Classes.Citybuilding.Buildings.Housing.Upgrades
{
    public class Level2 : BuildingUpgrade
    {
        public Level2()
        {
            Level = 2;
            ModelName = "Housing2";
            BaseCost = new Resources
            {
                Titan = 30
            };
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);
            foreach (var generatorBuilding in simulation.GeneratorBuildings) generatorBuilding.OutputMultiplier -= .5f;
        }
    }
}