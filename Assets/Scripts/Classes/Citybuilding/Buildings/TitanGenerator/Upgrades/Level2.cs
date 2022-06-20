namespace Classes.Citybuilding.Buildings.TitanGenerator.Upgrades
{
    public class Level2 : GeneratorBuildingUpgrade
    {
        public Level2()
        {
            Level = 2;
            ModelName = "TitanGenerator2";
            Description = "New explosives are being used for mining (Production of Titanium increased from 12 to 15)";
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