namespace Classes.Citybuilding.Buildings.FoodGenerator.Upgrades
{
    public class Level4 : GeneratorBuildingUpgrade
    {
        public Level4()
        {
            Level = 4;
            ModelName = "FoodGenerator4";
            BaseCost = new Resources
            {
                Titan = 50,
                Water = 30,
                Energy = 30
            };
        }
    }
}