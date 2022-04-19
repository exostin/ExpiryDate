namespace Classes.Citybuilding.Buildings.MedicSchool.Upgrades
{
    public class Level1 : BuildingUpgrade
    {
        public Level1()
        {
            Level = 1;
            ModelName = "MedicSchool1";
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Medic].Tier = 0;
        }
    }
}