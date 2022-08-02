using Classes;
namespace Other.DefenderSelection
{
    public class SimplifiedDefender
    {
        public DefenderType DefenderType { get; }
        public byte Count { get; }

        public string Name;

        public SimplifiedDefender(DefenderType defenderType, byte count, string name)
        {
            DefenderType = defenderType;
            Count = count;
            Name = name;
        }
    }
}