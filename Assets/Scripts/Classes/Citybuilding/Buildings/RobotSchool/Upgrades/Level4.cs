namespace Classes.Citybuilding.Buildings.RobotSchool.Upgrades
{
    public class Level4 : BuildingUpgrade
    {
        public Level4()
        {
            Level = 4;
            BaseCost = new Resources
            {
                Food = 20,
                Energy = 40,
                Water = 30,
                Titan = 80
            };
            ModelName = "RobotSchool4";
            Description = "Mechs use WD-40 (Unlocks Mechs Tier 3  + Their cost is 20% lower)";
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Mech].Tier = 3;
            building.cbm.Defenders[DefenderType.Mech].CostMultiplier = .8f;
        }
    }
}