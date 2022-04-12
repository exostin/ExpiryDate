namespace Classes.Citybuilding.Buildings.TitanGenerator.Upgrades
{
    public class Level1 : GeneratorBuildingUpgrade
    {
        public Level1()
        {
            Level = 1;
            ModelName = "TitanGenerator1";
            Output = new Resources
            {
                Titan = 10
            };
            BaseCost = new Resources
            {
                Water = 10
            };
        }
    }
}