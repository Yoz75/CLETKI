
using CTK.Engine;
using CTK.Engine.Rules;
using System;
using System.Collections.Generic;

namespace CTK.Test;

internal class Program
{
    private delegate CTKEngine EngineMaker(int sizeX, int sizeY);
    static void Main()
    {
        int sizeX = 512, sizeY = 512;

        using Renderer renderer = new((uint)sizeX, (uint)sizeY);

        EngineMaker maker = GoLMaker;
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
        var registrar = new CellTypeRegistrar();

        Cell dead = registrar.RegisterType();
        Cell alive = registrar.RegisterType();

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
        StartTypeWrapperRule<NearRule> dieRule = new(alive, new(alive, dead, 0, 1, 4, 5, 6, 7, 8));

        AutomatonStage golUpdateStage = new(int.MaxValue, birthRule, dieRule);

        Queue<IAutomatonStage> stages = [];
        stages.Enqueue(randomStage);
        stages.Enqueue(golUpdateStage);

        CTKEngine engine = new((sizeX, sizeY), dead, stages);

        return engine;
    }

    static CTKEngine FlowersMaker(int sizeX, int sizeY)
    {
        var registrar = new CellTypeRegistrar();

        Cell sky = registrar.RegisterType();
        Cell flower = registrar.RegisterType();
        Cell removeMarker = registrar.RegisterType();
        Cell petal = registrar.RegisterType();

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
        var registrar = new CellTypeRegistrar();

        Cell dead = registrar.RegisterType();
        Cell alive = registrar.RegisterType();

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

    static CTKEngine DayAndNightMaker(int sizeX, int sizeY)
    {
        var registrar = new CellTypeRegistrar();

        Cell dead = registrar.RegisterType();
        Cell alive = registrar.RegisterType();

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
        var registrar = new CellTypeRegistrar();
        Cell sky = registrar.RegisterType();
        Cell wood = registrar.RegisterType();
        // Cell type that will become wood (sprout that won't become leafs root)
        Cell woodCandidate = registrar.RegisterType();
        Cell sprout = registrar.RegisterType();
        Cell leaf = registrar.RegisterType();

        int trunkLength = Random.Shared.Next(sizeY / 2);

        AutomatonStage growTrunk = new(trunkLength,
            new GlobalPositionRule((sizeX / 2, 0), wood),
            new StartTypeWrapperRule<LocalPositionRule>(sky, new(PositionBias.Down, wood, wood)),
            new LocalPositionRule(PositionBias.Left, wood, woodCandidate),
            new LocalPositionRule(PositionBias.Right, wood, woodCandidate));

        AutomatonStage placeSprout = new(1,
            new StartTypeWrapperRule<LocalPositionRule>(wood, new(PositionBias.Upper, sky, sprout)));

        // I'm using trunkLength only to depend the tree crown's size on the trunk size
        AutomatonStage glowLeafs = new(trunkLength,
            new NearRule(sprout, leaf, 1),
            // The more neighbors allowed, the more dense crown will be
            new RandomWrapperRule<NearRule>(0.1f, new(leaf, leaf, 1, 2, 3, 4, 5)));

        AutomatonStage cleanup = new(1,
            new StartTypeWrapperRule<AlwaysRule>(woodCandidate, new(wood)),
            new StartTypeWrapperRule<AlwaysRule>(sprout, new(wood)));

        Queue<IAutomatonStage> stages = [];
        stages.Enqueue(growTrunk);
        stages.Enqueue(placeSprout);
        stages.Enqueue(glowLeafs);
        stages.Enqueue(cleanup);

        return new((sizeX, sizeY), sky, stages);
    }
}
