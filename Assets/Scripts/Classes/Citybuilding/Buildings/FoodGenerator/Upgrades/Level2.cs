namespace Classes.Citybuilding.Buildings.FoodGenerator.Upgrades
{
    public class Level2 : GeneratorBuildingUpgrade
    {
        public Level2()
        {
            Level = 2;
            ModelName = "FoodGenerator2";
            Output = new Resources
            {
                Food = 20
            };
            BaseCost = new Resources
            {
                Energy = 15
            };
        }
    }
}