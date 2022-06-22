namespace Classes.Citybuilding.Buildings.FighterSchool.Upgrades
{
    public class Level3 : BuildingUpgrade
    {
        public Level3()
        {
            Level = 3;
            BaseCost = new Resources
            {
                Energy = 20,
                Food = 60,
                Titan = 50
            };
            ModelName = "FighterSchool3";
            Description =
                "Sword Masters are equipped with newer, stronger, shinier weapons(Unlocks Sword Master Tier 2)";
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);

            building.cbm.Defenders[DefenderType.Fighter].Tier = 2;
        }
    }
}