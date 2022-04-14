namespace Classes.Citybuilding.Buildings.ShooterSchool.Upgrades
{
    public class Level1 : BuildingUpgrade
    {
        public Level1()
        {
            Level = 1;
            BaseCost = new Resources
            {
                Food = 30,
                Titan = 70,
                Water = 20
            };
            ModelName = "ShooterSchool1";
        }
    }
}