using Classes;

namespace Other.DefenderSelection
{
    public class SimplifiedDefender
    {
        public DefenderType DefenderType { get; }
        public byte Count { get; }

        public string Name => DefenderType.ToString();
        
        public SimplifiedDefender(DefenderType defenderType, byte count)
        {
            DefenderType = defenderType;
            Count = count;
        }
    }
}