namespace Classes
{
    public class Character
    {
        public int ID { get; set; }
        public CharacterStatistics Statistics { get; set; }
        public Ability[] Abilities { get; set; }
        public bool IsOwnedByPlayer { get; set; } = false;
    }
}