
namespace CTK.Engine.Rules;

/// <summary>
/// /// A wrapper rule, that allows some <typeparamref name="T"/> to execute only if current cell is the start type
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class StartTypeWrapperRule<T> : IRule where T : IRule
{
    private readonly Cell StartType;
    private readonly T Rule;

    /// <summary>
    /// Creates a new <see cref="StartTypeWrapperRule{T}"/>
    /// </summary>
    /// <param name="startType">the type, that should have cell to be updated</param>
    /// <param name="rule">the rule, that will be executed if cell has start ty[e</param>
    public StartTypeWrapperRule(Cell startType, T rule)
    {
        StartType = startType;
        Rule = rule;
    }

    public Cell Calculate((int, int) position, Field field)
    {
        if(field.Map[position.Item1, position.Item2] == StartType)
        {
            return Rule.Calculate(position, field);
        }

        return default;
    }
}
