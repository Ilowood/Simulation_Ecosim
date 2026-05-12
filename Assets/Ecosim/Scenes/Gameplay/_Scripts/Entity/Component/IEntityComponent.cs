namespace Ecosim
{
    public interface IEntityComponent
    {
        IComponentSnapshot GetSnapshot();
        void Restore(IComponentSnapshot snapshot);
        void Reset();
    }
}
