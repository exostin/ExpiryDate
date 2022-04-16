namespace Classes.Citybuilding.Buildings.MainCamp.Upgrades
{
    public class Level2 : BuildingUpgrade
    {
        public Level2()
        {
            Level = 2;
            BaseCost = new Resources
            {
                Energy = 100,
                Food = 100,
                Titan = 120,
                Water = 100
            };
            ModelName = "MainCamp2";
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);
            simulation.Housing.Upgrades[1].Unlocked = true;
        }
    }
}