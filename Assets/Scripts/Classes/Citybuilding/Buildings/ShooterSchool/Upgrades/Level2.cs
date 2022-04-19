namespace Classes.Citybuilding.Buildings.ShooterSchool.Upgrades
{
    public class Level2 : BuildingUpgrade
    {
        public Level2()
        {
            Level = 2;
            BaseCost = new Resources
            {
                Food = 30,
                Titan = 70,
                Water = 20
            };
            ModelName = "ShooterSchool2";
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Shooter].Tier = 1;
        }
    }
}