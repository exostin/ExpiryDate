using System.Linq;

namespace Classes.Citybuilding
{
    public class Resources
    {
        public int Energy;
        public int Food;
        public int Titan;
        public int Water;

        public static Resources operator +(Resources a, Resources b)
        {
            return new Resources
            {
                Food = a.Food + b.Food,
                Titan = a.Titan + b.Titan,
                Energy = a.Energy + b.Energy,
                Water = a.Water + b.Water
            };
        }

        public static Resources operator -(Resources a, Resources b)
        {
            return new Resources
            {
                Food = a.Food - b.Food,
                Titan = a.Titan - b.Titan,
                Energy = a.Energy - b.Energy,
                Water = a.Water - b.Water
            };
        }

        public static Resources operator +(Resources a, int b)
        {
            return new Resources
            {
                Food = a.Food + b,
                Titan = a.Titan + b,
                Energy = a.Energy + b,
                Water = a.Water + b
            };
        }

        public static Resources operator -(Resources a, int b)
        {
            return new Resources
            {
                Food = a.Food - b,
                Titan = a.Titan - b,
                Energy = a.Energy - b,
                Water = a.Water - b
            };
        }

        public static Resources operator *(Resources a, float b)
        {
            return new Resources
            {
                Food = (int) (a.Food * b),
                Titan = (int) (a.Titan * b),
                Energy = (int) (a.Energy * b),
                Water = (int) (a.Water * b)
            };
        }

        public static Resources operator /(Resources a, float b)
        {
            return new Resources
            {
                Food = (int) (a.Food / b),
                Titan = (int) (a.Titan / b),
                Energy = (int) (a.Energy / b),
                Water = (int) (a.Water / b)
            };
        }

        public static int operator /(Resources a, Resources b)
        {
            var foodCount = b.Food != 0 ? a.Food / b.Food : int.MaxValue;
            var titanCount = b.Titan != 0 ? a.Titan / b.Titan : int.MaxValue;
            var energyCount = b.Energy != 0 ? a.Energy / b.Energy : int.MaxValue;
            var waterCount = b.Water != 0 ? a.Water / b.Water : int.MaxValue;

            int[] counts = {foodCount, titanCount, energyCount, waterCount};

            return counts.Min();
        }

        public static bool operator <=(Resources a, Resources b)
        {
            if (a.Food > b.Food) return false;
            if (a.Titan > b.Titan) return false;
            if (a.Energy > b.Energy) return false;
            if (a.Water > b.Water) return false;
            return true;
        }

        public static bool operator >=(Resources a, Resources b)
        {
            if (a.Food < b.Food) return false;
            if (a.Titan < b.Titan) return false;
            if (a.Energy < b.Energy) return false;
            if (a.Water < b.Water) return false;
            return true;
        }

        public static bool operator <(Resources a, Resources b)
        {
            if (a.Food >= b.Food) return false;
            if (a.Titan >= b.Titan) return false;
            if (a.Energy >= b.Energy) return false;
            if (a.Water >= b.Water) return false;
            return true;
        }

        public static bool operator >(Resources a, Resources b)
        {
            if (a.Food <= b.Food) return false;
            if (a.Titan <= b.Titan) return false;
            if (a.Energy <= b.Energy) return false;
            if (a.Water <= b.Water) return false;
            return true;
        }

        public override string ToString()
        {
            return $"Food={Food}, Energy={Energy}, Titan={Titan}, Water={Water}";
        }
    }
}