namespace Classes.Citybuilding.Buildings.FoodGenerator.Upgrades
{
    public class Level3 : GeneratorBuildingUpgrade
    {
        public Level3()
        {
            Level = 3;
            ModelName = "FoodGenerator3";
            Description =
                "Farmers are now talking to plants, making them happy (Production of Energy increased from 14 to 16)";
            Output = new Resources
            {
                Food = 2
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