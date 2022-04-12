namespace Classes.Citybuilding.Buildings.TitanGenerator.Upgrades
{
    public class Level2 : GeneratorBuildingUpgrade
    {
        public Level2()
        {
            Level = 2;
            ModelName = "TitanGenerator2";
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