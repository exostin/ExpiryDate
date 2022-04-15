namespace Classes.Citybuilding.Buildings.FighterSchool.Upgrades
{
    public class Level4 : BuildingUpgrade
    {
        public Level4()
        {
            Level = 4;
            BaseCost = new Resources
            {
                Energy = 20,
                Food = 60,
                Titan = 50
            };
            ModelName = "FighterSchool4";
        }
    }
}