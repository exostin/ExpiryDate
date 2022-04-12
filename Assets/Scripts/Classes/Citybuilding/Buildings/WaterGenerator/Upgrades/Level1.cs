namespace Classes.Citybuilding.Buildings.WaterGenerator.Upgrades
{
    public class Level1 : GeneratorBuildingUpgrade
    {
        public Level1()
        {
            Level = 1;
            ModelName = "WaterGenerator2";
            Output = new Resources
            {
                Water = 10
            };
            BaseCost = new Resources
            {
                Titan = 10
            };
        }
    }
}