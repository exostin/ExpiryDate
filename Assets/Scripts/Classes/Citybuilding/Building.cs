using System;
using System.Linq;
using JetBrains.Annotations;

namespace Classes.Citybuilding
{
    public abstract class Building
    {
        public Manager cbm;
        public int CurrentUpgradeLevel;
        public string Description;
        public string Name;
        private readonly Action<int> setLevel;
        public BuildingUpgrade[] Upgrades;

        public Building(int level, Action<int> setLevel)
        {
            CurrentUpgradeLevel = level;
            this.setLevel = setLevel;
        }

        public BuildingUpgrade CurrentUpgrade =>
            Upgrades.ToList().Find(upgrade => upgrade.Level == CurrentUpgradeLevel);

        public BuildingUpgrade[] UnlockedUpgrades
            => Upgrades.ToList().FindAll(upgrade => upgrade.Level <= CurrentUpgradeLevel).ToArray();


        [CanBeNull] public BuildingUpgrade NextUpgrade => Upgrades[CurrentUpgrade.Level + 1];

        public bool CanBeUpgraded => !(NextUpgrade is null) && NextUpgrade.ActualCost <= cbm.PlayerResources;

        public virtual void ApplySideEffects(Simulation simulation)
        {
            foreach (var upgrade in UnlockedUpgrades)
                upgrade.ApplySideEffects(simulation, this);
        }

        public virtual void OnNextDay()
        {
            foreach (var upgrade in UnlockedUpgrades)
                upgrade.OnNextDay(cbm);
        }

        public void Upgrade()
        {
            if (NextUpgrade is null || !CanBeUpgraded) return;
            cbm.PlayerResources -= NextUpgrade.ActualCost;
            setLevel(NextUpgrade.Level);
            cbm.RunSimulation();
        }
    }
}