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
    }
}