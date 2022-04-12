using System;
using Classes.Citybuilding.Buildings.MedicSchool.Upgrades;

namespace Classes.Citybuilding.Buildings.MedicSchool
{
    public class MedicSchool : Building
    {
        public MedicSchool(int level, Action<int> setLevel) : base(level, setLevel)
        {
            Name = "Medic school";
            Description = "Studia medyczne";
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