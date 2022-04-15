namespace Classes.Citybuilding.Buildings.EnergyGenerator.Upgrades
{
    public class Level4 : GeneratorBuildingUpgrade
    {
        public Level4()
        {
            Level = 4;
            ModelName = "EnergyGenerator4";
            BaseCost = new Resources
            {
                Titan = 20,
                Water = 40,
                Food = 50
            };
        }
    }
}