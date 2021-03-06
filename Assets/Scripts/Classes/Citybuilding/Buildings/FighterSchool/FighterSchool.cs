using System;
using Classes.Citybuilding.Buildings.FighterSchool.Upgrades;

namespace Classes.Citybuilding.Buildings.FighterSchool
{
    public class FighterSchool : Building
    {
        public FighterSchool(int level, Action<int> setLevel) : base(level, setLevel)
        {
            Name = "Training grounds (Spear Master)";
            Description = "Uczą hudo i kutarate";
            DefenderType = DefenderType.Fighter;
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