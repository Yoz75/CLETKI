using System;
using System.Collections.Generic;
using System.Text;

namespace CTK.Engine.Rules;

/// <summary>
/// Changes cell to the end type if initial type was the start type
/// </summary>
public sealed class AlwaysRule : IRule
{
    private Cell StartType, EndType;

    public AlwaysRule(Cell startType, Cell endType)
    {
        StartType = startType;
        EndType = endType;
    }
    public Cell Calculate((int, int) position, Field field)
    {
        if(field.Map[position.Item1, position.Item2] == StartType)
        {
            return EndType;
        }

        return default;
    }
}
