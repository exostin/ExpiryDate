namespace Classes.Citybuilding.Buildings.WaterGenerator.Upgrades
{
    public class Level1 : GeneratorBuildingUpgrade
    {
        public Level1()
        {
            Level = 1;
            ModelName = "WaterGenerator1";
            Description = "Simple water treatment station (Produces 10 W)";
            Output = new Resources
            {
                Water = 5
            };
        }
    }
}