namespace BugColony
{
    public interface ISimulationCommand
    {
        int Priority { get; }
        void Execute(Simulation simulation);
    }
}
