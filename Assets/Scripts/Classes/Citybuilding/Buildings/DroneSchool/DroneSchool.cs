using System;
using Classes.Citybuilding.Buildings.DroneSchool.Upgrades;

namespace Classes.Citybuilding.Buildings.DroneSchool
{
    public class DroneSchool : Building
    {
        public DroneSchool(int level, Action<int> setLevel) : base(level, setLevel)
        {
            Name = "Tech Lab (Support Drone)";
            Description = "Basically DJI shop";
            DefenderType = DefenderType.Drone;
            Upgrades = new BuildingUpgrade[]
            {
                new Level0(),
                new Level1(),
                new Level2(),
                new Level3(),
                new Level4()
            };
        }
    }
}