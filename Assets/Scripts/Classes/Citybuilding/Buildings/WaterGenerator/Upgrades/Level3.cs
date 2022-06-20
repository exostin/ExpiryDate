namespace Classes.Citybuilding.Buildings.WaterGenerator.Upgrades
{
    public class Level3 : GeneratorBuildingUpgrade
    {
        public Level3()
        {
            Level = 3;
            ModelName = "WaterGenerator3";
            Description = "Water is being sterilized with Gamma Rays (Production of Water increased from 14 to 16)";
            Output = new Resources
            {
                Water = 2
            };
            BaseCost = new Resources
            {
                Titan = 60,
                Food = 20,
                Energy = 30
            };
        }
    }
}