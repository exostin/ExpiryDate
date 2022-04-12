namespace Classes.Citybuilding.Buildings.EnergyGenerator.Upgrades
{
    public class Level1 : GeneratorBuildingUpgrade
    {
        public Level1()
        {
            Level = 1;
            ModelName = "EnergyGenerator1";
            Output = new Resources
            {
                Energy = 10
            };
            BaseCost = new Resources
            {
                Titan = 10
            };
        }
    }
}