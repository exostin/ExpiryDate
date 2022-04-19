namespace Classes.Citybuilding.Buildings.RobotSchool.Upgrades
{
    public class Level3 : BuildingUpgrade
    {
        public Level3()
        {
            Level = 3;
            BaseCost = new Resources
            {
                Food = 20,
                Energy = 40,
                Water = 30,
                Titan = 80
            };
            ModelName = "RobotSchool3";
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Robot].Tier = 2;
        }
    }
}