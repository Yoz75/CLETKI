using System;
using System.Collections.Generic;
using System.Text;

namespace CTK.Engine.Rules;

/// <summary>
/// Changes the cell to the end type if count of required type neighbors is in range if initial type was the start type
/// </summary>
public sealed class NearRule : IRule
{
    private readonly Cell StartType, RequiredType, EndType;
    private readonly byte[] RequiredNeighborsCount;

    /// <summary>
    /// Create a new <see cref="AbsoluteNearRule"/>
    /// </summary>
    /// <param name="requiredType">required neighbor type</param>
    /// <param name="endType">end type</param>
    /// <param name="requiredNeighborsCount">if count of <paramref name="requiredType"/> neighbors is contained in this array, the cell will become <paramref name="endType"/></param>
    public NearRule(Cell startType, Cell requiredType, Cell endType, params byte[] requiredNeighborsCount)
    {
        StartType = startType;
        RequiredNeighborsCount = requiredNeighborsCount;
        RequiredType = requiredType;
        EndType = endType;
    }

    public Cell Calculate((int, int) position, Field field)
    {
        if(field.Map[position.Item1, position.Item2] != StartType) return default;
        Neighbors neighbors = field.GetNeighbors(position);
        byte requiredCount = neighbors.OfType(RequiredType);

        if(RequiredNeighborsCount.Contains(requiredCount)) return EndType;
        return default;
    }
}
