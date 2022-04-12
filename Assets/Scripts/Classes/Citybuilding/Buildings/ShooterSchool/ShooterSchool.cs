using System;
using Classes.Citybuilding.Buildings.ShooterSchool.Upgrades;

namespace Classes.Citybuilding.Buildings.ShooterSchool
{
    public class ShooterSchool : Building
    {
        public ShooterSchool(int level, Action<int> setLevel) : base(level, setLevel)
        {
            Name = "Shooter school";
            Description = "Expect a lot of school shootings";
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