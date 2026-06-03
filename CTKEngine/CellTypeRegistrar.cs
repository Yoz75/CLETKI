using System;

namespace CTK.Engine;

/// <summary>
/// An instrument to register new cell types. Use unique instance of this class for every instance of <see cref="ICellEngine"/> to avoid overlaps
/// </summary>
public class CellTypeRegistrar
{
    private static byte LastId = 2;
    private byte LocalLastId = 2;

    /// <summary>
    /// Count of all existing types (including both reserved and registered by this registrar)
    /// </summary>
    public byte ExistingTypesCount => LocalLastId;

    /// <summary>
    /// Count of registered by this registrar types
    /// </summary>
    public byte RegisteredTypesCount => LocalLastId - 2;

    /// <summary>
    /// Register a new type of cells for a cellular engine. 
    /// </summary>
    /// <returns>an instance of <see cref="Cell"/> with the new type</returns>
    /// <exception cref="OverflowException"></exception>
    public Cell RegisterType()
    {
        if(LocalLastId == byte.MaxValue) throw new OverflowException("Already registered all 254 types. Can not register more!");
        return new Cell() { Type = LocalLastId++ };
    }

    /// <summary>
    /// Register a new global type (this means you have only 254 types for the all project)... I should delete this bullshit but I use semantic version and don't want to have 2.0.0 on the second day of the project bruh
    /// </summary>
    /// <returns></returns>
    /// <exception cref="OverflowException"></exception>
    public static Cell Register()
    {
        if(LastId == byte.MaxValue) throw new OverflowException("Already registered all 254 types. Can not register more!");
        return new Cell() { Type = LastId++ };
    }
}
