
using CTK.Engine;
using CTK.Engine.Rules;
using System.Collections.Generic;

namespace CTK.Test;

internal class Program
{
    private delegate CTKEngine EngineMaker(int sizeX, int sizeY);
    static void Main(string[] args)
    {
        int sizeX = 32, sizeY = 32;
        Renderer renderer = new();
        EngineMaker maker = GoLMaker;

        CTKEngine engine = maker(sizeX, sizeY);

        while(engine.CanUpdate())
        {
            engine.Update();
            renderer.Update(engine.GetState());
        }
    }

    static CTKEngine GoLMaker(int sizeX, int sizeY)
    {
        Cell dead = CellTypeRegistrar.Register();
        Cell alive = CellTypeRegistrar.Register();

        // Random wrapper calls wrapped rule in [chance] cases of all
        // Here, wrapper calls AlwaysRule in 25 cases of 100
        RandomWrapperRule<AlwaysRule> spawnAliveRule = new(0.25f, new AlwaysRule(alive));
        AutomatonStage randomStage = new(1, spawnAliveRule);

        // Start type wrapper calls wrapped rule when updated cell has start type
        // Here, wrapper calls NearRule when current cell is dead
        // Usually, you will wrap your rules with this wrapper (because you describe rules for specific types like here
        StartTypeWrapperRule<NearRule> birthRule = new(dead, new(alive, alive, 3));

        // Alive cells stay being alive when there's 2 or 3 neighbors,
        // the long list of numbers is "reversed" 2, 3 list because we want to affect cells that "should be changed*
        StartTypeWrapperRule<NearRule> dieRule = new(alive, new( alive, dead, 0, 1, 4, 5, 6, 7, 8));

        AutomatonStage golUpdateStage = new(int.MaxValue, birthRule, dieRule);

        Queue<IAutomatonStage> stages = [];
        stages.Enqueue(randomStage);
        stages.Enqueue(golUpdateStage);

        CTKEngine engine = new CTKEngine((sizeX, sizeY), dead, stages);

        return engine;
    }
}
