namespace Ecosim
{
    public interface ITaskFactory
    {
        TaskVariants Variant { get; }
        IEntityTask Create(Entity owner, ITaskParams parameters);
    }

    public interface ITaskFactory<TParams> : ITaskFactory where TParams : ITaskParams
    {
        IEntityTask Create(Entity owner, TParams parameters);
    }
}
