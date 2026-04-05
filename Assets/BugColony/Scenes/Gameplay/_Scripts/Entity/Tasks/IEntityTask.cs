namespace BugColony
{
    public interface IEntityTask
    {
        bool IsComplete { get; }
        TaskVariants Variants { get; }

        void Start();
        void End();
    }
}
