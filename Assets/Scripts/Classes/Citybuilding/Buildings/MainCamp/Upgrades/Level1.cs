namespace Classes.Citybuilding.Buildings.MainCamp.Upgrades
{
    public class Level1 : BuildingUpgrade
    {
        public Level1()
        {
            Level = 1;
            BaseCost = new Resources
            {
                Energy = 80,
                Food = 80,
                Titan = 100,
                Water = 80
            };
            ModelName = "MainCamp1";
        }
    }
}