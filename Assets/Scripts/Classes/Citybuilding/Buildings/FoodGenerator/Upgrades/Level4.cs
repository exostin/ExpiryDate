namespace Classes.Citybuilding.Buildings.FoodGenerator.Upgrades
{
    public class Level4 : GeneratorBuildingUpgrade
    {
        public Level4()
        {
            Level = 4;
            ModelName = "FoodGenerator4";
            Description =
                "Greenhouses now use genetically modified plants that harvest themselves (Statistics of Spear Masters are increased by 10%)";
            BaseCost = new Resources
            {
                Titan = 50,
                Water = 30,
                Energy = 30
            };
            Output = new Resources();
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Fighter].StatsMultiplier = 1.1f;
        }
    }
}