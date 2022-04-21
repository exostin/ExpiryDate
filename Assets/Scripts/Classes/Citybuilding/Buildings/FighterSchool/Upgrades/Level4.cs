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
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Fighter].Tier = 3;
            building.cbm.Defenders[DefenderType.Fighter].CostMultiplier = .8f;
        }
    }
}