namespace Classes.Citybuilding.Buildings.FoodGenerator.Upgrades
{
    public class Level3 : GeneratorBuildingUpgrade
    {
        public Level3()
        {
            Level = 3;
            ModelName = "FoodGenerator3";
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