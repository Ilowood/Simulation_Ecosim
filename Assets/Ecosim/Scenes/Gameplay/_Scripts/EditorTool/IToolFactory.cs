namespace Ecosim
{
    public enum ToolVariants
    {
        Build = 0,
    }

    public interface IToolParams { }

    public interface IToolFactory
    {
        ToolVariants Variant { get; }
        EditorToolPipeline Create(IToolParams parameters);
    }

    public interface IToolFactory<TParams> : IToolFactory where TParams : IToolParams
    {
        EditorToolPipeline Create(TParams parameters);
    }
}
