
using System.Collections.Generic;

namespace CTK.Engine;

/// <summary>
/// Default implementation if <see cref="ICellEngine"/>
/// </summary>
public sealed class CTKEngine : ICellEngine
{
    public readonly (int, int) Resolution;
    private readonly Queue<IAutomatonStage> Stages;

    private readonly Field Field;
    private IAutomatonStage? CurrentStage;

    public CTKEngine((int, int) resolution, Cell startType, Queue<IAutomatonStage> stages)
    {
        Resolution = resolution;
        Stages = stages;

        Field = new(resolution, startType);
    }

    public bool CanUpdate()
    {
        return Stages.Count > 0 || CurrentStage is null || !CurrentStage.IsDone();
    }

    public Field GetState() => Field;

    public void Update()
    {
        if(CurrentStage is null || CurrentStage.IsDone())
        {
            CurrentStage = Stages.Dequeue();
            CurrentStage.Prepare(Field);
        }

        CurrentStage.Update();
    }
}
