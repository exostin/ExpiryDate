namespace Classes.Citybuilding.Buildings.WaterGenerator.Upgrades
{
    public class Level3 : GeneratorBuildingUpgrade
    {
        public Level3()
        {
            Level = 3;
            ModelName = "WaterGenerator3";
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