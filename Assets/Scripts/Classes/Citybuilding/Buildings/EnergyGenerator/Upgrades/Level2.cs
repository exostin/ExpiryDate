namespace Classes.Citybuilding.Buildings.EnergyGenerator.Upgrades
{
    public class Level2 : GeneratorBuildingUpgrade
    {
        public Level2()
        {
            Level = 2;
            ModelName = "EnergyGenerator2";
            Output = new Resources
            {
                Energy = 20
            };
            BaseCost = new Resources
            {
                Titan = 15
            };
        }
    }
}