namespace Classes.Citybuilding.Buildings.FoodGenerator.Upgrades
{
    public class Level1 : GeneratorBuildingUpgrade
    {
        public Level1()
        {
            Level = 1;
            ModelName = "FoodGenerator1";
            Description = "Simple greenhouse (Produces 10 F)";
            Output = new Resources
            {
                Food = 5
            };
        }
    }
}