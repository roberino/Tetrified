using Tetrified.Engine.Models;
using UniversalGrid.Events;
using UniversalGrid.Geometry;

namespace Tetrified.Engine
{
    public class Engine
    {
        public Engine(Game game = null)
        {
            Game = game ?? new Game();
            Game.Board.ItemAdded += OnNewItem;
        }

        public Game Game { get; private set; }

        public void Start()
        {
            Cycle();
        }

        public void Shutdown()
        {
            Game.Shutdown();
        }

        private void Cycle()
        {
            if (Game.State == GameState.Uninitialised || Game.State == GameState.Running)
            {
                if (Game.ActiveTetromino == null)
                {
                    var next = Game.DropNext();

                    next.Resting += AfterResting;
                }
            }
            else
            {
                if (Game.State == GameState.Finished)
                {
                    return;
                }
            }
        }

        private void AfterResting(object sender, System.EventArgs e)
        {
            Cycle();
        }

        private void OnNewItem(object sender, ObjectEvent<ISpatial2DThing<char>> e)
        {
        }
    }
}
