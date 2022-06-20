namespace Classes.Citybuilding.Buildings.TitanGenerator.Upgrades
{
    public class Level1 : GeneratorBuildingUpgrade
    {
        public Level1()
        {
            Level = 1;
            ModelName = "TitanGenerator1";
            Description = "Simple titanium mine (Produces 12 T)";
            Output = new Resources
            {
                Titan = 8
            };
        }
    }
}