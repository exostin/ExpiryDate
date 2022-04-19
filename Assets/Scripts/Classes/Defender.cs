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
        public DefenderType Type { get; }
        public byte Tier { get; set; }
        public byte Amount { get; set; }
        public float Multiplier { get; set; }

        public Defender(DefenderType type)
        {
            Type = type;
        }
    }
}