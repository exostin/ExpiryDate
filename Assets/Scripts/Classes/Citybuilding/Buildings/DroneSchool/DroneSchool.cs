using System;
using Classes.Citybuilding.Buildings.DroneSchool.Upgrades;

namespace Classes.Citybuilding.Buildings.DroneSchool
{
    public class DroneSchool : Building
    {
        public DroneSchool(int level, Action<int> setLevel) : base(level, setLevel)
        {
            Name = "Drone school";
            Description = "Basicly DJI sklep";
            Upgrades = new BuildingUpgrade[]
            {
                new Level0(),
                new Level1(),
                new Level2(),
                new Level3()
            };
        }
    }
}