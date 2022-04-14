namespace Classes.Citybuilding.Buildings.FighterSchool.Upgrades
{
    public class Level2 : BuildingUpgrade
    {
        public Level2()
        {
            Level = 2;
            BaseCost = new Resources
            {
                Energy = 20,
                Food = 60,
                Titan = 50
            };
            ModelName = "FighterSchool2";
        }
    }
}