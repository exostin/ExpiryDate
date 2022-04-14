namespace Classes.Citybuilding.Buildings.DroneSchool.Upgrades
{
    public class Level2 : BuildingUpgrade
    {
        public Level2()
        {
            Level = 2;
            BaseCost = new Resources
            {
                Food = 50,
                Titan = 20,
                Energy = 50
            };
            ModelName = "DroneSchool2";
        }
    }
}