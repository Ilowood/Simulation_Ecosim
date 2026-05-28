namespace Ecosim
{
    public interface IWorldCommand
    {
        int Priority { get; }
        void Execute(World simulation);
    }
}
