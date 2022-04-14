namespace Classes.Citybuilding.Buildings.FighterSchool.Upgrades
{
    public class Level3 : BuildingUpgrade
    {
        public Level3()
        {
            Level = 3;
            BaseCost = new Resources
            {
                Energy = 20,
                Food = 60,
                Titan = 50
            };
            ModelName = "FighterSchool3";
        }
    }
}