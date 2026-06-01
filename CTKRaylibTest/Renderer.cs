using CTK.Engine;
using System;
using System.Collections.Generic;

namespace CTK.Test
{
    internal class Renderer
    {
        // We support only 8 types, ok?
        Dictionary<byte, char> Type2Color = new()
        {
            { 0, '#' },
            { 1, '@' },
            { 2, '!' },
            { 3, '%' },

            { 4, '$' },
            { 5, '&' },
            { 6, '*' },
            { 7, 'R' },
        };

        public void Update(Field field)
        {
            Console.Clear();
            var start = field.MyBounds.ValidStart;
            var end = field.MyBounds.ValidEnd;

            for(int y = start.Item2; y < end.Item2; y++)
            {
                for (int x = start.Item1; x < end.Item1; x++)
                {
                    Console.Write(Type2Color[field.Map[x, y].Type]);
                }

                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
