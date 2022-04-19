using Classes.Citybuilding;

namespace Classes
{
    public enum DefenderType
    {
        Shooter,
        Fighter,
        Medic,
        Robot,
        Drone
    }

    public class Defender
    {
        public Defender(DefenderType type)
        {
            Type = type;
        }

        public DefenderType Type { get; }
        public sbyte Tier { get; set; }
        public byte Amount { get; set; }
        public float StatsMultiplier { get; set; }
        public Resources BaseCost { get; set; }
        public float CostMultiplier { get; set; }
        public Resources ActualCost => BaseCost * CostMultiplier;
    }
}