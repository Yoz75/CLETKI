# WHAT IS CLETKI

CLETKI, or CTK is a simple to use cellular automaton engine (i.e simplifies automata creation) with multithreading support (CLETKI uses all machine cores by default). It doesn't provides rendering stuff and other things, but you can see [an example of making Game of Life simulation and its rendering](https://github.com/Yoz75/CLETKI/tree/main/CTKTest) in the CTKTest project.

## FAST HELLO WORLD EXAMPLE WITHOUT EXPLANATION
This code shows how to make GoL simulation and render it:
```cs
    internal class Renderer
    {
        // We support only 4 types, ok?
        Dictionary<Cell, char> Type2Symbol = new()
        {
            // REMEMBER: FIRST 2 TYPES (0 AND 1) ARE RESERVER FOR CLETKI'S PURPOSES.
            // ACTUALLY WE SHOULD'NT USE RAW NUMBERS AND PASS TYPES TO THE RENDERER FROM THE CONSTRUCTOR BUT I'M LAZY
            { new Cell() {Type = 2}, '!' },
            { new Cell() {Type = 3}, '%' },
        };

        public void Update(Field field)
        {
            (int, int) start = field.MyBounds.ValidStart;
            (int, int) end = field.MyBounds.ValidEnd;

            for(int y = start.Item2; y < end.Item2; y++)
            {
                for (int x = start.Item1; x < end.Item1; x++)
                {
                    Console.Write(Type2Symbol[field.Map[x, y]]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.SetCursorPosition(0, 0);
        }
    }

    static void Main(string[] args)
    {
        int sizeX = 96, sizeY = 48;

        Renderer renderer = new();

        // CellTypeRegistrar.Register() makes an instance of Cell that has a new type and ensures the new type is correct
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

        // First argument is resolution of the simulation field, second is the start type, third is stages of the automaton
        CTKEngine engine = new((sizeX, sizeY), dead, stages);
        while(engine.CanUpdate())
        {
            engine.Update();
            // Renderer is very slow, CLETKI itself is MUCH faster
            renderer.Update(engine.GetState());
        }
    }
```

## EXPLANATION OF CLETKI:
`ICellularEngine` -- the simulation engine. Consider it as a game-loop. Default implementation is CTKEngine class. Every simulation is consists of a queue of IAutomatonStage<br><br>
`IAutomatonStage` -- a stage of the automaton. Default implementation is the AutomatonStage class. Imagine, you are making a GoL simulation. It consists of 2 stages:
* Spawn random alive cells in a field of dead ones (do once) -- `new AutomatonStage(1, IRule[] weTalkLaterAboutRules)`
* Apply B3/S23 rules for each cell (do as many times as you want, lets say 9999) -- `new AutomatonStage(9999, IRule[] ditto)`
<a/>

`IRule` -- an action executed on the map if a condition is executed on the map. For example, `AlwaysRule(alive)` will ALWAYS change cell to alive type and `NearRule(always, always, 3)` will change the cell to alive type if it has 3 alive neighbors<br><br>
`Wrappers`: <p>in the IRule section I didn't mention randomness or start type of the cell. You can add them using `RandomWrapperRule<T>(float chance, T ruleThatShouldHaveRandomChanceToExecute)` and `StartTypeWrapper<T>(Cell startType, T ruleThatShouldBeExecutedIfCellIsStartType)`.<br>For example, "replace 25% of all cells with alive ones" can be done as `RandomWrapperRule<AlwaysRule>(0.25f, new(alive))`, "if a dead cell has 3 alive neighbors, make it alive too" is `StartTypeWrapper<NearRule>(dead, new(alive, alive, 3))`, and "if an alive cell has less than 2, or greater than 3 neighbors, kill it" is `StartTypeWrapper<NearRule>(alive, new(alive, dead, 1, 4, 5, 6, 7, 8))`</p>

## ALL RULES DOCUMENTATION:
Currently, CLETKI has 6 rules in the `CTK.Engine.Rules namespace`, here are their constructors:
* `AlwaysRule(Cell endType)` -- always changes the updating cell to `endType`
* `GlobalPositionRule((int, int) position, Cell endType)` -- changes the cell at `position` to `endType`
* `LocalPositionRule((int, int) bias, Cell requiredType, Cell endType)` -- changes the cell to `endType`, if the cell at `bias` has `requiredType`. The bias must be in range [-1; 1], probably simulation will crash with IndexOutOfRangeException if you do it (-2; 10) for example.
* `NearRule(Cell requiredType, Cell endType, params byte[] requiredNeighborsCount)` changes cell to `endType` if `requiredNeighborsCount` contains count of cell's neighbors that have `requiredType`
* `RandomWrapperRule<T>(float chance, T rule)` -- rule that needed to add random chance of executing the wrapped rule
* `StartTypeWrapper<T>(Cell startType, T rule)` -- like previous, but for adding start type

# Documentation
Currently, all documentation is described in the Readme.md file (this file). The library is pretty small and I hope this file clearly describes how to use it. You can open CTKTest project to see more examples!

# How to build:
Open the solution file and build the project
