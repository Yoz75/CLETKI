
using CTK.Engine;
using CTK.Engine.Rules;
using System.Collections.Generic;

namespace CTK.Test;

internal class Program
{
    private delegate CTKEngine EngineMaker(int sizeX, int sizeY);
    static void Main(string[] args)
    {
        int sizeX = 256, sizeY = 256;
        Renderer renderer = new();
        EngineMaker maker = GoLMaker;

        CTKEngine engine = maker(sizeX, sizeY);

        while(engine.CanUpdate())
        {
            engine.Update();
        }
            renderer.Update(engine.GetState());
    }

    static CTKEngine GoLMaker(int sizeX, int sizeY)
    {
        Cell dead = CellTypeRegistrar.Register();
        Cell alive = CellTypeRegistrar.Register();

        RandomWrapperRule<AlwaysRule> spawnAliveRule = new(0.25f, new AlwaysRule(dead, alive));
        AutomatonStage randomStage = new(1, spawnAliveRule);

        NearRule birthRule = new(dead, alive, alive, 3);

        // Alive cells stay being alive when there's 2 or 3 neighbors,
        // the long list of numbers is "reversed" 2, 3 list because we want to affect cells that "should be changed*
        NearRule dieRule = new(alive, alive, dead, 0, 1, 4, 5, 6, 7, 8);

        AutomatonStage golUpdateStage = new(int.MaxValue, birthRule, dieRule);

        Queue<IAutomatonStage> stages = [];
        stages.Enqueue(randomStage);
        stages.Enqueue(golUpdateStage);

        CTKEngine engine = new CTKEngine((sizeX, sizeY), dead, stages);

        return engine;
    }
}
