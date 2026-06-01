
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

    /// <summary>
    /// The type of the cell
    /// </summary>
    public byte Type;

    public bool IsValid()
    {
        return Type > 1;
    }
}
