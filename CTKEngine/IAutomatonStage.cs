
namespace CTK.Engine;

/// <summary>
/// A stage of the automaton with a fixed bunch of rules.
/// </summary>
public interface IAutomatonStage
{
    /// <summary>
    /// Update the stage once. 
    /// If a stage started updating, it won't stop while !IsDone()
    /// (this means you can assume the whole field is yours between <see cref="Prepare(Field)"/> and <see cref="IsDone"/> == true)
    /// </summary>
    public void Update();

    /// <summary>
    /// Prepare the stage before executing it.
    /// </summary>
    /// <param name="field">the field where stage will be executed</param>
    public void Prepare(Field field);

    /// <summary>
    /// Is this stage done and won't update?
    /// </summary>
    /// <returns>true if done, false otherwise</returns>
    public bool IsDone();
}
