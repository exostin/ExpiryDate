namespace Classes.Citybuilding.Buildings.MainCamp.Upgrades
{
    public class Level1 : BuildingUpgrade
    {
        public Level1()
        {
            Level = 1;
            ModelName = "MainCamp1";
            Description = "Simple Town Hall (Unlocks first tier of living quarters)";
        }
        
        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);
            simulation.Housing.Upgrades[1].Unlocked = true;
        }
    }
}