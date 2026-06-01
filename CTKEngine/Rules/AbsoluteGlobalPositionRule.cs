using System;
using System.Collections.Generic;
using System.Text;

namespace CTK.Engine.Rules;

/// <summary>
/// Always turns cell at the global position to the end type
/// </summary>
public sealed class AlwaysGlobalPositionRule : IRule
{
    private readonly (int, int) Position;
    private readonly Cell EndType;

    public AlwaysGlobalPositionRule((int, int) position, Cell endType)
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
