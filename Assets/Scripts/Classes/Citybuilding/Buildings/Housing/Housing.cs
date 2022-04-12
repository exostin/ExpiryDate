using System;
using Classes.Citybuilding.Buildings.Housing.Upgrades;

namespace Classes.Citybuilding.Buildings.Housing
{
    public class Housing : Building
    {
        public Housing(int level, Action<int> setLevel) : base(level, setLevel)
        {
            Name = "Budynek mieszkalny";
            Description = "Dla twoich niewolników <3";
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