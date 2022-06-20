namespace Classes.Citybuilding.Buildings.MedicSchool.Upgrades
{
    public class Level2 : BuildingUpgrade
    {
        public Level2()
        {
            Level = 2;
            BaseCost = new Resources
            {
                Food = 60,
                Energy = 20,
                Water = 40
            };
            ModelName = "MedicSchool2";
            Description = "Medics wash their hands now (Unlocks Medic Tier 1)";
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Medic].Tier = 1;
        }
    }
}