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
                Food = 3
            };
            BaseCost = new Resources
            {
                Titan = 50,
                Water = 30,
                Energy = 30
            };
        }
    }
}