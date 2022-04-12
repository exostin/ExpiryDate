using System;
using Classes.Citybuilding.Buildings.TitanGenerator.Upgrades;

namespace Classes.Citybuilding.Buildings.TitanGenerator
{
    public class TitanGenerator : GeneratorBuilding
    {
        public TitanGenerator(int level, Action<int> setLevel) : base(level, setLevel)
        {
            Name = "Titan mine";
            Description = "Used for mining Titan";
            Upgrades = new BuildingUpgrade[]
            {
                new Level0(),
                new Level1(),
                new Level2()
            };
        }
    }
}