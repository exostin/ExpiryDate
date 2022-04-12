namespace Classes.Citybuilding.Buildings.Housing.Upgrades
{
    public class Level3 : BuildingUpgrade
    {
        public Level3()
        {
            Level = 3;
            ModelName = "Housing3";
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