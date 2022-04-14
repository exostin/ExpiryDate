namespace Classes.Citybuilding.Buildings.MainCamp.Upgrades
{
    public class Level3 : BuildingUpgrade
    {
        public Level3()
        {
            Level = 3;
            BaseCost = new Resources
            {
                Energy = 120,
                Food = 120,
                Titan = 140,
                Water = 100
            };
            ModelName = "MainCamp3";
        }
    }
}