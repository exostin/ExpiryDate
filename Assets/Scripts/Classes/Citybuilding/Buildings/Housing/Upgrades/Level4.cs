namespace Classes.Citybuilding.Buildings.Housing.Upgrades
{
    public class Level4 : BuildingUpgrade
    {
        public Level4()
        {
            Level = 4;
            ModelName = "Housing4";
            BaseCost = new Resources
            {
                Titan = 100,
                Water = 80,
                Food = 60,
                Energy = 50
            };
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);
            foreach (var generatorBuilding in simulation.GeneratorBuildings) generatorBuilding.OutputBonus += 3;
        }
    }
}