namespace Classes.Citybuilding.Buildings.RobotSchool.Upgrades
{
    public class Level4 : BuildingUpgrade
    {
        public Level4()
        {
            Level = 4;
            BaseCost = new Resources
            {
                Food = 20,
                Energy = 40,
                Water = 30,
                Titan = 80
            };
            ModelName = "RobotSchool4";
        }
    }
}