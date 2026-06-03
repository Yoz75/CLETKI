
namespace CTK.Engine;

/// <summary>
/// The type, that represents a cell on the field. Create new types using <see cref="CellTypeSequestrator"/>
/// </summary>
public record struct Cell
{
    /// <summary>
    /// Type, that represents an invalid state
    /// </summary>
    public const int InvalidType = 0;

    /// <summary>
    /// Type, that represents a field border
    /// </summary>
    public const int BorderType = 1;

    public readonly static Cell Invalid = new(InvalidType);
    public readonly static Cell Border = new(BorderType);

    /// <summary>
    /// The type of the cell
    /// </summary>
    public byte Type;

    public Cell(byte type) => Type = type;

    public bool IsValid()
    {
        return Type > 1;
    }
}
