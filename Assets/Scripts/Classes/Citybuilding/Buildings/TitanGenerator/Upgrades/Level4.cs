namespace Classes.Citybuilding.Buildings.TitanGenerator.Upgrades
{
    public class Level4 : GeneratorBuildingUpgrade
    {
        public Level4()
        {
            Level = 4;
            ModelName = "TitanGenerator4";
            BaseCost = new Resources
            {
                Water = 40,
                Energy = 50,
                Food = 20
            };
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Shooter].StatsMultiplier = 1.1f;
        }
    }
}