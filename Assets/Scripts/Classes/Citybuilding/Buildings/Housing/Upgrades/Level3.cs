using System.Linq;

namespace Classes.Citybuilding.Buildings.Housing.Upgrades
{
    public class Level3 : BuildingUpgrade
    {
        public Level3()
        {
            Level = 3;
            ModelName = "Housing3";
            Description = "There's a bingo session everyday (All other buildings are 10% cheaper)";
            BaseCost = new Resources
            {
                Titan = 90,
                Water = 70,
                Food = 50,
                Energy = 40
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