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
                Food = 20
            };
            BaseCost = new Resources
            {
                Energy = 15
            };
        }
    }
}