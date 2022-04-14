namespace Classes.Citybuilding.Buildings.RobotSchool.Upgrades
{
    public class Level1 : BuildingUpgrade
    {
        public Level1()
        {
            Level = 1;
            BaseCost = new Resources
            {
                Food = 20,
                Energy = 40,
                Water = 30,
                Titan = 80
            };
            ModelName = "RobotSchool1";
        }
    }
}