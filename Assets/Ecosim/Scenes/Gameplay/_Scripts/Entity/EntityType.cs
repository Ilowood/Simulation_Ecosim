namespace Ecosim
{
    public enum EntityType
    {
        None = 0,
        Unit = 1 << 0,
        Resource = 2 << 0,
        Workplace = 3 << 0,
        Residential = 4 << 0,
        Prop = 5 << 0,
    }
}
