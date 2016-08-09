using System;
using UniversalGrid;
using System.Linq;
using UniversalGrid.Geometry;

namespace Tetrified.ConsoleApp
{
    // https://github.com/davidwhitney/CodeDojos/tree/master/Tetris
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new Engine.Engine();

            engine.Game.Board.ItemMoved += OnChange;

            engine.Start();

            while (true)
            {
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Escape) break;

                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        engine.Game.MoveActiveTetromino(Direction.Left);
                        break;
                    case ConsoleKey.RightArrow:
                        engine.Game.MoveActiveTetromino(Direction.Right);
                        break;
                    case ConsoleKey.UpArrow:
                        engine.Game.RotateActiveTetromino(Direction.Down);
                        break;
                    case ConsoleKey.DownArrow:
                        engine.Game.MoveActiveTetromino(Direction.Down);
                        break;
                }
            }
        }

        private static void OnChange(object sender, UniversalGrid.Events.ObjectEvent<UniversalGrid.Geometry.ISpatial2DThing<char>> e)
        {
            RenderToConsole((UniversalGrid<char>)sender);
        }

        private static void RenderToConsole(UniversalGrid<char> grid)
        {
            Console.Clear();
            Console.WriteLine();

            var r = -1;

            grid.Render((p, m) =>
            {
                if (p.Y > r)
                {
                    //if(r > -1) Console.Write("|");

                    Console.WriteLine();
                    Console.Write(p.Y.ToString().PadRight(3));
                    Console.Write("|");
                }

                if (m.Any())
                {
                    Console.Write("O");
                }
                else
                {
                    Console.Write(" ");
                }

                Console.Write("|");

                r = p.Y;
            });

            Console.WriteLine();
        }
    }
}
