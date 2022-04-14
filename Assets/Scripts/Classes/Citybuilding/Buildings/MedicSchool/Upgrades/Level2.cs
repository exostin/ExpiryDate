namespace Classes.Citybuilding.Buildings.MedicSchool.Upgrades
{
    public class Level2 : BuildingUpgrade
    {
        public Level2()
        {
            Level = 2;
            BaseCost = new Resources
            {
                Food = 60,
                Energy = 20,
                Water = 40
            };
            ModelName = "MedicSchool2";
        }
    }
}