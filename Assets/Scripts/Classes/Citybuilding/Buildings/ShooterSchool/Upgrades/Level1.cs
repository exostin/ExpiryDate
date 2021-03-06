namespace Classes.Citybuilding.Buildings.ShooterSchool.Upgrades
{
    public class Level1 : BuildingUpgrade
    {
        public Level1()
        {
            Level = 1;
            ModelName = "ShooterSchool1";
            Description = "Simple shooting range for Soldiers";
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Shooter].Tier = 0;
        }
    }
}