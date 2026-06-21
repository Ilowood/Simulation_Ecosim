namespace Ecosim
{
    public interface IEntityTask : ITicable
    {
        bool IsComplete { get; }
        TaskVariants Variants { get; }

        void Start();
        void End();

        void Puase();
        void Resume();

        ITaskSnapshot GetSnapshot();
    }
}
