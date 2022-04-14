namespace Classes.Citybuilding.Buildings.MedicSchool.Upgrades
{
    public class Level1 : BuildingUpgrade
    {
        public Level1()
        {
            Level = 1;
            BaseCost = new Resources
            {
                Food = 60,
                Energy = 20,
                Water = 40
            };
            ModelName = "MedicSchool1";
        }
    }
}