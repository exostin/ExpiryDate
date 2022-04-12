namespace Classes.Citybuilding.Buildings.EnergyGenerator.Upgrades
{
    public class Level3 : GeneratorBuildingUpgrade
    {
        public Level3()
        {
            Level = 3;
            ModelName = "EnergyGenerator3";
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