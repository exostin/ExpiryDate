namespace Classes.Citybuilding.Buildings.EnergyGenerator.Upgrades
{
    public class Level3 : GeneratorBuildingUpgrade
    {
        public Level3()
        {
            Level = 3;
            ModelName = "EnergyGenerator3";
            Description = "Moving parts are sprayed with WD-40 (Production of Energy increased from 14 to 16)";
            Output = new Resources
            {
                Energy = 2
            };
            BaseCost = new Resources
            {
                Titan = 20,
                Water = 40,
                Food = 50
            };
        }
    }
}