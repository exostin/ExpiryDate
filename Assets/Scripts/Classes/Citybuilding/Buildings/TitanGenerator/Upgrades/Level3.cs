namespace Classes.Citybuilding.Buildings.TitanGenerator.Upgrades
{
    public class Level3 : GeneratorBuildingUpgrade
    {
        public Level3()
        {
            Level = 3;
            ModelName = "TitanGenerator3";
            Output = new Resources
            {
                Titan = 20
            };
            BaseCost = new Resources
            {
                Water = 15
            };
        }
    }
}