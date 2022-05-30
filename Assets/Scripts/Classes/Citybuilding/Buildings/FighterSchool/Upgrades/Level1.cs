namespace Classes.Citybuilding.Buildings.FighterSchool.Upgrades
{
    public class Level1 : BuildingUpgrade
    {
        public Level1()
        {
            Level = 1;
            ModelName = "FighterSchool1";
            Description = "Simple training grounds for Spear Masters";
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Fighter].Tier = 0;
        }
    }
}