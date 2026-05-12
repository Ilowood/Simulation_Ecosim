namespace Ecosim
{
    public interface ISimulationCommand
    {
        int Priority { get; }
        void Execute(World simulation);
    }
}
