namespace Ecosim
{
    public interface IEntityComponent
    {
        object GetSnapshot();
        void Restore(object snapshot);
        void Reset();
    }
}
