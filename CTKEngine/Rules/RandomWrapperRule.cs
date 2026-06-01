using System;
using System.Collections.Generic;
using System.Text;

namespace CTK.Engine.Rules;

/// <summary>
/// Not a real rule, but wrapps some <typeparamref name="T"/> to add randomness
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class RandomWrapperRule<T> : IRule where T : IRule
{
    private readonly float Chance;
    private readonly T Rule;

    public RandomWrapperRule(float chance, T rule)
    {
        Chance = chance;
        Rule = rule;
    }

    public Cell Calculate((int, int) position, Field field)
    {
        if(Chance >= 1 || Random.Shared.NextSingle() < Chance)
        {
            return Rule.Calculate(position, field);
        }

        return default;
    }
}
