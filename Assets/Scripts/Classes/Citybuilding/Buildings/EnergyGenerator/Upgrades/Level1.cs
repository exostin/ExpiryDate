namespace Classes.Citybuilding.Buildings.EnergyGenerator.Upgrades
{
    public class Level1 : GeneratorBuildingUpgrade
    {
        public Level1()
        {
            Level = 1;
            ModelName = "EnergyGenerator1";
            Description = "Simple generator (Produces 10 E)";
            Output = new Resources
            {
                Energy = 5
            };
        }
    }
}