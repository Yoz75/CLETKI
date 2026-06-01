using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CTK.Engine;

/// <summary>
/// 8 neighbors of a cell
/// </summary>
public struct Neighbors
{
    public Cell LeftUpper;
    public Cell Upper;
    public Cell RightUpper;

    public Cell LeftDown;
    public Cell Down;
    public Cell RightDown;

    public Cell Right;
    public Cell Left;

    /// <summary>
    /// Get count of neighbors of type <paramref name="type"/>
    /// </summary>
    /// <param name="type">type of neigbhors</param>
    /// <returns></returns>
    public readonly byte OfType(Cell type)
    {
        byte result = 0;
            
        unsafe
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static byte Bool2Byte(bool b)
            {
                return *(byte*)&b;
            }

            result +=  Bool2Byte(LeftUpper == type);
            result += Bool2Byte(Upper == type);
            result += Bool2Byte(RightUpper == type);

            result += Bool2Byte(LeftDown == type);
            result += Bool2Byte(Down == type);
            result += Bool2Byte(RightDown == type);

            result += Bool2Byte(Right == type);
            result += Bool2Byte(Left == type);
        }

        return result;
    }
}
