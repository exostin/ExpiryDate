namespace Classes.Citybuilding.Buildings.ShooterSchool.Upgrades
{
    public class Level4 : BuildingUpgrade
    {
        public Level4()
        {
            Level = 4;
            BaseCost = new Resources
            {
                Food = 30,
                Titan = 70,
                Water = 20
            };
            ModelName = "ShooterSchool4";
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Shooter].Tier = 3;
            building.cbm.Defenders[DefenderType.Shooter].CostMultiplier = .8f;
        }
    }
}