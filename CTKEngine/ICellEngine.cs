namespace CTK.Engine;

/// <summary>
/// Something that executes the automaton
/// </summary>
public interface ICellEngine
{
    /// <summary>
    /// Update the engine once
    /// </summary>
    public void Update();

    /// <summary>
    /// Is this engine can update?
    /// </summary>
    /// <returns>true when can keep updating and false otherwise</returns>
    public bool CanUpdate();

    /// <summary>
    /// Get current automaton state. Changing something if the returned value results in changing automaton state
    /// </summary>
    /// <returns>current automaton field</returns>
    public Field GetState();
}
