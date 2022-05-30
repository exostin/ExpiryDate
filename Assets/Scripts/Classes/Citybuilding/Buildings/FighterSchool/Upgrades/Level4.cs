namespace Classes.Citybuilding.Buildings.FighterSchool.Upgrades
{
    public class Level4 : BuildingUpgrade
    {
        public Level4()
        {
            Level = 4;
            BaseCost = new Resources
            {
                Energy = 20,
                Food = 60,
                Titan = 50
            };
            ModelName = "FighterSchool4";
            Description = "Spear Masters achieved Nirvana, though their definition is different and now posses the knowledge to kill every single living and non-living creature (Unlocks Spear Masters Tier 3 + Their cost is 20% lower)";
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Fighter].Tier = 3;
            building.cbm.Defenders[DefenderType.Fighter].CostMultiplier = .8f;
        }
    }
}