namespace Ecosim
{
    public interface ITaskFactory
    {
        TaskVariants Variant { get; }
        IEntityTask Create(WorldContext context, Entity owner, ITaskParams parameters);
    }

    public interface ITaskFactory<TParams> : ITaskFactory where TParams : ITaskParams
    {
        IEntityTask Create(WorldContext context, Entity owner, TParams parameters);
    }
}
