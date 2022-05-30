namespace Classes.Citybuilding.Buildings.WaterGenerator.Upgrades
{
    public class Level2 : GeneratorBuildingUpgrade
    {
        public Level2()
        {
            Level = 2;
            ModelName = "WaterGenerator2";
            Description =
                "New type of bacteria is used to better treat water (Production of Water increased from 10 to 14)";
            Output = new Resources
            {
                Water = 3
            };
            BaseCost = new Resources
            {
                Titan = 60,
                Food = 20,
                Energy = 30
            };
        }
    }
}