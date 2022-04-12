namespace Classes.Citybuilding.Buildings
{
    public abstract class GeneratorBuildingUpgrade : BuildingUpgrade
    {
        public Resources Output;

        public void ApplyGeneratorSideEffects(Simulation simulation, GeneratorBuilding building)
        {
            building.BaseOutput += Output;
        }
    }
}