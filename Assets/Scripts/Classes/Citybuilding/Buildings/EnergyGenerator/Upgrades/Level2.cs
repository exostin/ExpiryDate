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
                Energy = 3
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