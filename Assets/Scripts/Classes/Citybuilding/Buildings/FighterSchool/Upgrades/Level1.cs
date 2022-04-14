namespace Classes.Citybuilding.Buildings.FighterSchool.Upgrades
{
    public class Level1 : BuildingUpgrade
    {
        public Level1()
        {
            Level = 1;
            BaseCost = new Resources
            {
                Energy = 20,
                Food = 60,
                Titan = 50
            };
            ModelName = "FighterSchool1";
        }
    }
}