using System;
using System.Collections.Generic;
using System.Text;

namespace CTK.Engine.Rules;

/// <summary>
/// /// Turns cell to end type if there was required ty[e at a local biased position and initial type was the start type
/// </summary>
public sealed class LocalPositionRule : IRule
{
    private readonly (int, int) Bias;
    private readonly Cell StartType, RequiredType, EndType;

    public LocalPositionRule((int, int) bias, Cell startType, Cell requiredType, Cell endType)
    {
        Bias = bias;
        StartType = startType;
        RequiredType = requiredType;
        EndType = endType;
    }

    public Cell Calculate((int, int) position, Field field)
    {
        if(field.Map[position.Item1, position.Item2] != StartType);

        if(field.Map[position.Item1 + Bias.Item1, position.Item2 + Bias.Item2] == RequiredType)
        {
            return EndType;
        }

        return default;
    }
}
