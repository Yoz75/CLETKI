
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CTK.Engine;

/// <summary>
/// Default implementation of <see cref="IAutomatonStage"/>
/// </summary>
/// 
public sealed class AutomatonStage : IAutomatonStage
{
    private Field? CurrentField;

    private int StepsCount;
    private int CurrentStep;

    private Cell[,]? TempField;
    private readonly IRule[] Rules;
    private readonly ParallelOptions UpdateOptions = new() { MaxDegreeOfParallelism = Environment.ProcessorCount};

    public AutomatonStage(int steps, params IRule[] rules)
    {
        StepsCount = steps;
        Rules = rules;
    }

    public bool IsDone()
    {
        return CurrentStep >= StepsCount;
    }

    public void Prepare(Field field)
    {
        CurrentField = field;
        TempField = new Cell[field.Resolution.Item1, field.Resolution.Item2]; 
    }

    public void SetSteps(int steps) => StepsCount = steps;

    public void Update()
    {
        Debug.Assert(CurrentField is not null, "Update() called without preparing field before! Maybe add Prepare(Field) ?");
        Debug.WriteLineIf(Rules.Length == 0, "Rules count is 0. Maybe forgot to add rules?");

        (int, int) start = CurrentField.MyBounds.ValidStart;
        (int, int) end = CurrentField.MyBounds.ValidEnd;

        Parallel.For(start.Item2, end.Item2, UpdateOptions, (y) =>
        {
            Parallel.For(start.Item1, end.Item1, UpdateOptions, (x) =>
            {
                foreach(var rule in Rules)
                {
                    Cell cell = rule.Calculate((x, y), CurrentField);

                    if(cell != default)
                    {
                        TempField![x, y] = cell;
                        goto nextCell;
                    }
                }

                TempField![x, y] = CurrentField.Map[x, y];

            nextCell:;
            });
        });

        (TempField, CurrentField.Map) = (CurrentField.Map, TempField);
        CurrentStep++;
    }

}
