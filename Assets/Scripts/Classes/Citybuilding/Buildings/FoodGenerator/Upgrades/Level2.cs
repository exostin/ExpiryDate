namespace Classes.Citybuilding.Buildings.FoodGenerator.Upgrades
{
    public class Level2 : GeneratorBuildingUpgrade
    {
        public Level2()
        {
            Level = 2;
            ModelName = "FoodGenerator2";
            Description =
                "New fertilizer is being used to make crops plentiful (Production of Energy increased from 10 to 14)";
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