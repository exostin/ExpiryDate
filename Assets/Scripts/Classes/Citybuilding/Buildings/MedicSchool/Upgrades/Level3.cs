namespace Classes.Citybuilding.Buildings.MedicSchool.Upgrades
{
    public class Level3 : BuildingUpgrade
    {
        public Level3()
        {
            Level = 3;
            BaseCost = new Resources
            {
                Food = 60,
                Energy = 20,
                Water = 40
            };
            ModelName = "MedicSchool3";
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Medic].Tier = 2;
        }
    }
}