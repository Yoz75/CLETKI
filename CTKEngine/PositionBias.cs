
namespace CTK.Engine;

/// <summary>
/// Defines local bias from current cell
/// </summary>
public enum PositionBias : byte
{ 
    /// <summary>
    /// Default enum value. Should be considered as wrong/uninitialized value, not like a "lack of the bias"
    /// </summary>
    None = 0,

    LeftUpper,
    Upper,
    RightUpper,

    LeftDown,
    Down,
    RightDown,

    Right,
    Left,
}