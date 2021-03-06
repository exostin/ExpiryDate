namespace Classes.Citybuilding
{
    public abstract class BuildingUpgrade
    {
        public Resources BaseCost = new();
        public int CostBonus = 0;
        public float CostMultiplier = 1f;
        public int Level;
        public string ModelName;
        public bool Unlocked = true;
        public string Description;

        public Resources ActualCost => BaseCost * CostMultiplier + CostBonus;

        public virtual void ApplySideEffects(Simulation simulation, Building building)
        {
        }

        public virtual void OnNextDay(Manager cbm)
        {
        }
    }
}