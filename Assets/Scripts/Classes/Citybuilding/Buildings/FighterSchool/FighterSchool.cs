using System;
using Classes.Citybuilding.Buildings.FighterSchool.Upgrades;

namespace Classes.Citybuilding.Buildings.FighterSchool
{
    public class FighterSchool : Building
    {
        public FighterSchool(int level, Action<int> setLevel) : base(level, setLevel)
        {
            Name = "Fighter school";
            Description = "Uczą hudo i kutarate";
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