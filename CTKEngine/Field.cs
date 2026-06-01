
namespace CTK.Engine;

/// <summary>
/// A wrapper over Cell[,]
/// </summary>
public sealed class Field
{
    /// <summary>
    /// Cellular automata very often iterate over neihbors of a cell. Without dummy border cells we whould check every neighbor if it valid or not.
    /// Using borders, we can fill them with the invalid value, return them safely and then user deals with them!
    /// </summary>
    public readonly struct Bounds
    {
        /// <summary>
        /// Valid X start and valid Y start
        /// </summary>
        public readonly (int, int) ValidStart;

        /// <summary>
        /// Valid X end and valid Y end. These two should be considered as length of the field, 
        /// cells with coordinates [Item1, whatever], [whatever, Item2] should not be modified!
        /// </summary>
        public readonly (int, int) ValidEnd;

        public Bounds((int, int) start, (int, int) end)
        {
            ValidStart = start;
            ValidEnd = end;
        }
    }

    public readonly (int, int) Resolution;
    public Cell[,] Map;

    /// <summary>
    /// <see cref="Bounds"/>> of the field where it can be iterated
    /// </summary>
    public Bounds MyBounds;

    public Field((int, int) resolution, Cell startType)
    {
        Resolution = resolution;
        Map = new Cell[Resolution.Item1, Resolution.Item2];
        MyBounds = new((1, 1), (Resolution.Item1 - 1, Resolution.Item2 - 1));

        for(int y = 0; y < resolution.Item2; y++)
        {
            for(int x = 0; x < resolution.Item1; x++)
            {
                if(x == 0 || y == 0)
                {
                    Map[x, y] = new Cell() { Type = Cell.BorderType };
                    continue;
                }

                Map[x, y] = startType;
            }
        }
    }

    /// <summary>
    /// Get Neighbors for a cell at the position
    /// </summary>
    /// <param name="position">The position of checked cell. Method assumes x and Y coordinates are in <see cref="MyBounds"/></param>
    /// <returns>types of neighbors for this position</returns>
    public Neighbors GetNeighbors((int, int) position)
    {
        Neighbors neighbors;

        neighbors.LeftUpper = Map[position.Item1 - 1, position.Item2 - 1];
        neighbors.Upper = Map[position.Item1, position.Item2 - 1];
        neighbors.RightUpper = Map[position.Item1 + 1, position.Item2 - 1];

        neighbors.LeftDown = Map[position.Item1 - 1, position.Item2 + 1];
        neighbors.Down = Map[position.Item1, position.Item2 + 1]; ;
        neighbors.RightDown = Map[position.Item1 + 1, position.Item2 + 1];

        neighbors.Right = Map[position.Item1 + 1, position.Item2];
        neighbors.Left = Map[position.Item1 - 1, position.Item2];

        return neighbors;
    }
}
