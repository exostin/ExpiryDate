namespace Classes.Citybuilding.Buildings.WaterGenerator.Upgrades
{
    public class Level4 : GeneratorBuildingUpgrade
    {
        public Level4()
        {
            Level = 4;
            ModelName = "WaterGenerator4";
            Description =
                "To maximize quality of water, it's being split into hydrogen and oxygen, then combined again, and only after that minerals are added (Statistics of Medics are increased by 10%)";
            BaseCost = new Resources
            {
                Titan = 60,
                Food = 20,
                Energy = 30
            };
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Medic].StatsMultiplier = 1.1f;
        }
    }
}