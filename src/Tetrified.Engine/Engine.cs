using Tetrified.Engine.Models;
using UniversalGrid.Events;
using UniversalGrid.Geometry;

namespace Tetrified.Engine
{
    public class Engine
    {
        private bool _isRunning;

        public Engine(Game game = null)
        {
            Game = game ?? new Game();
            Game.Board.ItemAdded += OnNewItem;
        }

        public Game Game { get; private set; }

        public bool IsRunning { get { return _isRunning; } }

        public void Start()
        {
            _isRunning = true;

            Cycle();
        }

        public void Restart()
        {
            _isRunning = false;
        }

        public void Shutdown()
        {
            _isRunning = false;

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
