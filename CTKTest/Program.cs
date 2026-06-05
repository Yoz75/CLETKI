
using CTK.Engine;
using CTK.Engine.Rules;
using System.Collections.Generic;

namespace CTK.Test;

internal class Program
{
    private delegate CTKEngine EngineMaker(int sizeX, int sizeY);
    static void Main()
    {
        int sizeX = 512, sizeY = 512;

        using Renderer renderer = new((uint) sizeX, (uint) sizeY);

        EngineMaker maker = FlowersMaker;
        CTKEngine engine = maker(sizeX, sizeY);

        while(renderer.CanRender())
        {
            if(engine.CanUpdate()) engine.Update();
            // Renderer is very slow, CLETKI  is MUCH faster when launch without it!
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

        CTKEngine engine = new((sizeX, sizeY), dead, stages);

        return engine;
    }

    static CTKEngine FlowersMaker(int sizeX, int sizeY)
    {
        Cell sky = CellTypeRegistrar.Register();
        Cell flower = CellTypeRegistrar.Register();
        Cell removeMarker = CellTypeRegistrar.Register();
        Cell petal = CellTypeRegistrar.Register();

        AutomatonStage seedFlowersStage = new(1, new RandomWrapperRule<AlwaysRule>(0.1f, new(flower)));

        AutomatonStage cleanupBeginStage = new(2, 
            new NearRule(flower, removeMarker, 2, 3, 4, 5, 6, 7, 8),
            new StartTypeWrapperRule<NearRule>(flower, new(removeMarker, sky, 1, 2, 3, 4, 5, 6, 7, 8)));

        AutomatonStage cleanupEndStage = new(1, new StartTypeWrapperRule<AlwaysRule>(removeMarker, new(sky)));

        AutomatonStage growPetalsStage = new(1, new StartTypeWrapperRule<NearRule>(sky, new(flower, petal, 1)));

        Queue<IAutomatonStage> stages = [];
        stages.Enqueue(seedFlowersStage);
        stages.Enqueue(cleanupBeginStage); 
        stages.Enqueue(cleanupEndStage);
        stages.Enqueue(growPetalsStage);

        CTKEngine engine = new((sizeX, sizeY), sky, stages);

        return engine;
    }

    // I didn't see B35 S135 rule on the internet, so lets name it the dumblife, ok?
    static CTKEngine DumbLifeMaker(int sizeX, int sizeY)
    {
        Cell dead = CellTypeRegistrar.Register();
        Cell alive = CellTypeRegistrar.Register();

        // Random wrapper calls wrapped rule in [chance] cases of all
        // Here, wrapper calls AlwaysRule in 25 cases of 100
        RandomWrapperRule<AlwaysRule> spawnAliveRule = new(0.25f, new AlwaysRule(alive));
        AutomatonStage randomStage = new(1, spawnAliveRule);

        // B35
        StartTypeWrapperRule<NearRule> birthRule = new(dead, new(alive, alive, 3, 5));

        // S135
        StartTypeWrapperRule<NearRule> dieRule = new(alive, new(alive, dead, 0, 2, 4, 6, 7, 8));

        AutomatonStage golUpdateStage = new(int.MaxValue, birthRule, dieRule);

        Queue<IAutomatonStage> stages = [];
        stages.Enqueue(randomStage);
        stages.Enqueue(golUpdateStage);

        CTKEngine engine = new((sizeX, sizeY), dead, stages);

        return engine;
    }

    static CTKEngine DayAndNight(int sizeX, int sizeY)
    {
        Cell dead = CellTypeRegistrar.Register();
        Cell alive = CellTypeRegistrar.Register();

        // Random wrapper calls wrapped rule in [chance] cases of all
        // Here, wrapper calls AlwaysRule in 25 cases of 100
        RandomWrapperRule<AlwaysRule> spawnAliveRule = new(0.5f, new AlwaysRule(alive));
        AutomatonStage randomStage = new(1, spawnAliveRule);

        // B35
        StartTypeWrapperRule<NearRule> birthRule = new(dead, new(alive, alive, 3, 6, 7, 8));

        // S135
        StartTypeWrapperRule<NearRule> dieRule = new(alive, new(alive, dead, 0, 1, 2, 5));

        AutomatonStage golUpdateStage = new(int.MaxValue, birthRule, dieRule);

        Queue<IAutomatonStage> stages = [];
        stages.Enqueue(randomStage);
        stages.Enqueue(golUpdateStage);

        CTKEngine engine = new((sizeX, sizeY), dead, stages);

        return engine;
    }

    static CTKEngine TreeMaker(int sizeX, int sizeY)
    { 
        throw new System.Exception();
    }
}
