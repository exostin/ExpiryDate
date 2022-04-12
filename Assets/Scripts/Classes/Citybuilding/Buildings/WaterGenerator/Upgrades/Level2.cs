namespace Classes.Citybuilding.Buildings.WaterGenerator.Upgrades
{
    public class Level2 : GeneratorBuildingUpgrade
    {
        public Level2()
        {
            Level = 2;
            ModelName = "WaterGenerator2";
            Output = new Resources
            {
                Water = 20
            };
            BaseCost = new Resources
            {
                Titan = 15
            };
        }
    }
}