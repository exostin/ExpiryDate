using System;

namespace Classes.Citybuilding.Buildings
{
    public abstract class GeneratorBuilding : Building
    {
        public Resources BaseOutput = new();
        public int OutputBonus = 0;
        public float OutputMultiplier = 1f;

        public GeneratorBuilding(int level, Action<int> setLevel) : base(level, setLevel)
        {
        }

        public Resources ActualOutput => BaseOutput * OutputMultiplier + OutputBonus;

        public override void ApplySideEffects(Simulation simulation)
        {
            base.ApplySideEffects(simulation);
            foreach (var upgrade in UnlockedUpgrades)
                if (upgrade is GeneratorBuildingUpgrade generatorUpgrade)
                    generatorUpgrade.ApplyGeneratorSideEffects(simulation, this);
        }

        public override void OnNextDay()
        {
            base.OnNextDay();
            cbm.PlayerResources += ActualOutput;
        }
    }
}