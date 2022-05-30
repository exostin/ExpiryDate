namespace Classes.Citybuilding.Buildings.MedicSchool.Upgrades
{
    public class Level4 : BuildingUpgrade
    {
        public Level4()
        {
            Level = 4;
            BaseCost = new Resources
            {
                Food = 60,
                Energy = 20,
                Water = 40
            };
            ModelName = "MedicSchool4";
            Description =
                "Medics ascended into Godhood making their bandages extra good (Unlocks Medics Tier 3  + Their cost is 20% lower)";
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Medic].Tier = 3;
            building.cbm.Defenders[DefenderType.Medic].CostMultiplier = .8f;
        }
    }
}