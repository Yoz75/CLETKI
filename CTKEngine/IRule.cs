
namespace CTK.Engine;

/// <summary>
/// A rule that modifies cell with some condition
/// </summary>
public interface IRule
{
    /// <summary>
    /// Calculate the new type for current cell
    /// </summary>
    /// <param name="position">the position of updating cell</param>
    /// <param name="field">the field where cell is placed</param>
    /// <returns>Cell.default when didn't modified and other value when modified the cell.</returns>
    public Cell Calculate((int, int) position, Field field);
}
