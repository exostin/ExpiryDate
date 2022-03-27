using System;
using System.Linq;

namespace Classes
{
    [Serializable]
    public class Resources
    {
        public int food;
        public int titan;
        public int energy;
        public int water;

        public static Resources operator +(Resources a, Resources b)
        {
            return new Resources
            {
                food = a.food + b.food,
                titan = a.titan + b.titan,
                energy = a.energy + b.energy,
                water = a.water + b.water
            };
        }

        public static Resources operator -(Resources a, Resources b)
        {
            return new Resources
            {
                food = a.food - b.food,
                titan = a.titan - b.titan,
                energy = a.energy - b.energy,
                water = a.water - b.water
            };
        }

        public static Resources operator *(Resources a, int b)
        {
            return new Resources
            {
                food = a.food * b,
                titan = a.titan * b,
                energy = a.energy * b,
                water = a.water * b
            };
        }

        public static Resources operator /(Resources a, int b)
        {
            return new Resources
            {
                food = a.food / b,
                titan = a.titan / b,
                energy = a.energy / b,
                water = a.water / b
            };
        }

        public static int operator /(Resources a, Resources b)
        {
            var foodCount = b.food != 0 ? a.food / b.food : int.MaxValue;
            var titanCount = b.titan != 0 ? a.titan / b.titan : int.MaxValue;
            var energyCount = b.energy != 0 ? a.energy / b.energy : int.MaxValue;
            var waterCount = b.water != 0 ? a.water / b.water : int.MaxValue;

            int[] counts = {foodCount, titanCount, energyCount, waterCount};

            return counts.Min();
        }

        public static bool operator <=(Resources a, Resources b)
        {
            if (a.food > b.food) return false;
            if (a.titan > b.titan) return false;
            if (a.energy > b.energy) return false;
            if (a.water > b.water) return false;
            return true;
        }

        public static bool operator >=(Resources a, Resources b)
        {
            if (a.food < b.food) return false;
            if (a.titan < b.titan) return false;
            if (a.energy < b.energy) return false;
            if (a.water < b.water) return false;
            return true;
        }

        public static bool operator <(Resources a, Resources b)
        {
            if (a.food >= b.food) return false;
            if (a.titan >= b.titan) return false;
            if (a.energy >= b.energy) return false;
            if (a.water >= b.water) return false;
            return true;
        }

        public static bool operator >(Resources a, Resources b)
        {
            if (a.food <= b.food) return false;
            if (a.titan <= b.titan) return false;
            if (a.energy <= b.energy) return false;
            if (a.water <= b.water) return false;
            return true;
        }

        public override string ToString()
        {
            return $"Food={food}, Energy={energy}, Titan={titan}, Water={water}";
        }
    }
}