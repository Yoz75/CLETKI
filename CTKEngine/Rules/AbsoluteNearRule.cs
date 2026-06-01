using System;
using System.Collections.Generic;
using System.Text;

namespace CTK.Engine.Rules;

/// <summary>
/// Always changes the cell to the end type if count of required type neighbors is in range
/// </summary>
public sealed class AbsoluteNearRule : IRule
{
    private readonly Cell RequiredType, EndType;
    private readonly byte[] RequiredNeighborsCount;

    /// <summary>
    /// Create a new <see cref="AbsoluteNearRule"/>
    /// </summary>
    /// <param name="requiredType">required neighbor type</param>
    /// <param name="endType">end type</param>
    /// <param name="requiredNeighborsCount">if count of <paramref name="requiredType"/> neighbors is contained in this array, the cell will become <paramref name="endType"/></param>
    public AbsoluteNearRule(Cell requiredType, Cell endType, params byte[] requiredNeighborsCount)
    {
        RequiredNeighborsCount = requiredNeighborsCount;
        RequiredType = requiredType;
        EndType = endType;
    }

    public Cell Calculate((int, int) position, Field field)
    {
        Neighbors neighbors = field.GetNeighbors(position);
        byte requiredCount = neighbors.OfType(EndType);

        if(RequiredNeighborsCount.Contains(requiredCount)) return EndType;
        return default;
    }
}
