namespace Ecosim
{
    public class BuildToolParams : IToolParams
    {
        public readonly long SpecId;

        public BuildToolParams(long specId)
        {
            SpecId = specId;
        }
    }
}
