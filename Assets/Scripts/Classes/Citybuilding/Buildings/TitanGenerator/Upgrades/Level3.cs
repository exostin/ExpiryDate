namespace Classes.Citybuilding.Buildings.TitanGenerator.Upgrades
{
    public class Level3 : GeneratorBuildingUpgrade
    {
        public Level3()
        {
            Level = 3;
            ModelName = "TitanGenerator3";
            Description = "Titanium-finding technology is being used (Production of Titanium increased from 15 to 20)";
            Output = new Resources
            {
                Titan = 2
            };
            BaseCost = new Resources
            {
                Water = 40,
                Energy = 50,
                Food = 20
            };
        }
    }
}