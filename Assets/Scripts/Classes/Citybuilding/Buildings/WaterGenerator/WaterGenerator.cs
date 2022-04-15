using System;
using Classes.Citybuilding.Buildings.WaterGenerator.Upgrades;

namespace Classes.Citybuilding.Buildings.WaterGenerator
{
    public class WaterGenerator : GeneratorBuilding
    {
        public WaterGenerator(int level, Action<int> setLevel) : base(level, setLevel)
        {
            Name = "Water pump";
            Description = "Splashy splash";
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