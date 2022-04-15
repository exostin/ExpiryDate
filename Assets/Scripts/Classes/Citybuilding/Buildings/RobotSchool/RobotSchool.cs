using System;
using Classes.Citybuilding.Buildings.RobotSchool.Upgrades;

namespace Classes.Citybuilding.Buildings.RobotSchool
{
    public class RobotSchool : Building
    {
        public RobotSchool(int level, Action<int> setLevel) : base(level, setLevel)
        {
            Name = "Robot school";
            Description = "";
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