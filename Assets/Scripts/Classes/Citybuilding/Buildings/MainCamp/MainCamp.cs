using System;
using Classes.Citybuilding.Buildings.MainCamp.Upgrades;

namespace Classes.Citybuilding.Buildings.MainCamp
{
    public class MainCamp : Building
    {
        public MainCamp(int level, Action<int> setLevel) : base(level, setLevel)
        {
            Name = "Main camp";
            Description = "The main camp is the center of the city. It is where the citizens live and work.";
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