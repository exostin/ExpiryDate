namespace Classes.Citybuilding.Buildings.RobotSchool.Upgrades
{
    public class Level1 : BuildingUpgrade
    {
        public Level1()
        {
            Level = 1;
            ModelName = "RobotSchool1";
            Description = "Simple factory for Mechs";
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Mech].Tier = 0;
        }
    }
}