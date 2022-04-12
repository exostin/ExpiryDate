using System;
using Classes.Citybuilding.Buildings.FoodGenerator.Upgrades;

namespace Classes.Citybuilding.Buildings.FoodGenerator
{
    public class FoodGenerator : GeneratorBuilding
    {
        public FoodGenerator(int level, Action<int> setLevel) : base(level, setLevel)
        {
            Name = "Mutated dogs farm";
            Description = "Yummy";
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