namespace Ecosim
{
    public interface IEntityTask
    {
        bool IsComplete { get; }
        TaskVariants Variants { get; }

        void Start();
        void Tick(float deltaTime, float scale);
        void End();

        void Puase();
        void Resume();
    }
}
