using System;
using System.Collections.Generic;
using System.Text;

namespace CTK.Engine.Rules;

/// <summary>
/// Always changes cell to the end type if initial type was the start type
/// </summary>
public sealed class AlwaysRule : IRule
{
    private readonly Cell EndType;

    public AlwaysRule(Cell endType)
    {
        EndType = endType;
    }
    public Cell Calculate((int, int) position, Field field)
    {
        return EndType;
    }
}
