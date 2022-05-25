using System;
using System.Linq;
using JetBrains.Annotations;

namespace Classes.Citybuilding
{
    public abstract class Building
    {
        private readonly Action<int> setLevel;
        public Manager cbm;
        public int CurrentUpgradeLevel;
        public string Description;
        public string Name;
        public BuildingUpgrade[] Upgrades; // it's important to have this in the order of the levels
        [CanBeNull] public DefenderType DefenderType;

        public Building(int level, Action<int> setLevel)
        {
            CurrentUpgradeLevel = level;
            this.setLevel = setLevel;
        }

        public BuildingUpgrade CurrentUpgrade =>
            Upgrades.ToList().Find(upgrade => upgrade.Level == CurrentUpgradeLevel);

        public BuildingUpgrade[] BoughtUpgrades
            => Upgrades.ToList().FindAll(upgrade => upgrade.Level <= CurrentUpgradeLevel).ToArray();

        [CanBeNull]
        public BuildingUpgrade NextUpgrade =>
            CurrentUpgrade.Level + 1 < Upgrades.Length ? Upgrades[CurrentUpgrade.Level + 1] : null;

        public bool CanBeUpgraded => NextUpgrade is not null && NextUpgrade.ActualCost <= cbm.PlayerResources &&
                                     NextUpgrade.Unlocked;

        public virtual void ApplySideEffects(Simulation simulation)
        {
            foreach (var upgrade in BoughtUpgrades)
                upgrade.ApplySideEffects(simulation, this);
        }

        public virtual void OnNextDay()
        {
            foreach (var upgrade in BoughtUpgrades)
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