using System;
using Classes.Citybuilding.Buildings.EnergyGenerator.Upgrades;

namespace Classes.Citybuilding.Buildings.EnergyGenerator
{
    public class EnergyGenerator : GeneratorBuilding
    {
        public EnergyGenerator(int level, Action<int> setLevel) : base(level, setLevel)
        {
            Name = "Energy Generator";
            Description = "10 000 hamsters in a hamster wheel";
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