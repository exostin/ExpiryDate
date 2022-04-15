namespace Classes.Citybuilding.Buildings.WaterGenerator.Upgrades
{
    public class Level4 : GeneratorBuildingUpgrade
    {
        public Level4()
        {
            Level = 4;
            ModelName = "WaterGenerator4";
            BaseCost = new Resources
            {
                Titan = 60,
                Food = 20,
                Energy = 30
            };
        }
    }
}