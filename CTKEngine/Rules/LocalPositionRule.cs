
namespace CTK.Engine.Rules;

/// <summary>
/// Turns cell to end type if there was required type at a local biased position. Bias should be in range [-1, 1], 
/// other values are not supposed to be used and probably will brake your program.
/// </summary>
public sealed class LocalPositionRule : IRule
{
    private readonly (int, int) Bias;
    private readonly Cell RequiredType, EndType;

    public LocalPositionRule((int, int) bias, Cell requiredType, Cell endType)
    {
        Bias = bias;
        RequiredType = requiredType;
        EndType = endType;
    }

    public Cell Calculate((int, int) position, Field field)
    {
        if(field.Map[position.Item1 + Bias.Item1, position.Item2 + Bias.Item2] == RequiredType)
        {
            return EndType;
        }

        return default;
    }
}
