using System.Linq;

namespace Classes.Citybuilding.Buildings.Housing.Upgrades
{
    public class Level2 : BuildingUpgrade
    {
        public Level2()
        {
            Level = 2;
            ModelName = "Housing2";
            Description = "Living Quarters are renovated (All other buildings are 10% cheaper)";
            BaseCost = new Resources
            {
                Titan = 80,
                Water = 60,
                Food = 40,
                Energy = 30
            };
            Unlocked = false;
        }

        public override void ApplySideEffects(Simulation simulation, Building building)
        {
            base.ApplySideEffects(simulation, building);
            foreach (var otherBuilding in simulation.Buildings.ToList()
                         .FindAll(el => el is not Housing && el.NextUpgrade is not null))
                otherBuilding.NextUpgrade!.CostMultiplier *= .9f;
        }
    }
}