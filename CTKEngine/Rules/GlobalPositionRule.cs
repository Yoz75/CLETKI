using System;
using System.Collections.Generic;
using System.Text;

namespace CTK.Engine.Rules;

/// <summary>
/// Turns cell at the global position to the end type if initial type was the start type
/// </summary>
public sealed class GlobalPositionRule : IRule
{
    private readonly (int, int) Position;
    private readonly Cell StartType, EndType;

    public GlobalPositionRule((int, int) position, Cell startType, Cell endType)
    {
        Position = position;
        StartType = startType;
        EndType = endType;
    }

    public Cell Calculate((int, int) position, Field field)
    {
        if(position != Position) return default;

        if(field.Map[position.Item1, position.Item2] == StartType) return EndType;
        return default;
    }
}
