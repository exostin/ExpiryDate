namespace Classes.Citybuilding.Buildings.FoodGenerator.Upgrades
{
    public class Level1 : GeneratorBuildingUpgrade
    {
        public Level1()
        {
            Level = 1;
            ModelName = "FoodGenerator1";
            Output = new Resources
            {
                Food = 10
            };
            BaseCost = new Resources
            {
                Energy = 10
            };
        }
    }
}