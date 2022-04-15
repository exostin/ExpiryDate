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
    }
}