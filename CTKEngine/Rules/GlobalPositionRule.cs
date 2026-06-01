
namespace CTK.Engine.Rules;

/// <summary>
/// Always turns cell at the global position to the end type
/// </summary>
public sealed class GlobalPositionRule : IRule
{
    private readonly (int, int) Position;
    private readonly Cell EndType;

    public GlobalPositionRule((int, int) position, Cell endType)
    {
        Position = position;
        EndType = endType;
    }

    public Cell Calculate((int, int) position, Field field)
    {
        if(position != Position) return default;
        return EndType;
    }
}
