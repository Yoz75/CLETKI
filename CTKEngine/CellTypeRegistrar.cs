using System;

namespace CTK.Engine;

/// <summary>
/// An instrument to register new cell types
/// </summary>
public static class CellTypeRegistrar
{
    private static byte LastId = 2;

    public static Cell Register()
    {
        if(LastId == byte.MaxValue) throw new OverflowException("Already registered all 254 types. Can not register more~");
        return new Cell() { Type = LastId++ };
    }
}
