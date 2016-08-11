using System;
using System.Linq;
using UniversalGrid;
using UniversalGrid.Events;
using UniversalGrid.Geometry;

namespace Tetrified.Engine.Models
{
    public class Game
    {
        private TetrominoGenerator _generator;

        public Game(int width = 10, int height = 22)
        {
            Board = new UniversalGrid<char>(width, height);

            Board.ItemMoved += OnMove;
            
            _generator = new TetrominoGenerator();
        }

        private void OnMove(object sender, ObjectEvent<ISpatial2DThing<char>> e)
        {
            var grid = (UniversalGrid<char>)sender;

            var completedRows = grid.Rows.Where(r => r.All(c => grid.GetObjectsOverlapping(c).Any())).ToList();

            foreach(var row in completedRows)
            {
                var y = row.First().Y;
                var objectsOnRow = row.SelectMany(r => grid.GetObjectsOverlapping(r)).Distinct().ToList();

                foreach(var obj in objectsOnRow)
                {
                    obj.Modify(obj.Positions.Where(p => p.Y != y));
                }
            }
        }

        public UniversalGrid<char> Board { get; private set; }

        public GameState State { get; private set; }
        
        public bool MoveActiveTetromino(Direction direction)
        {
            try
            {
                var cur = ActiveTetromino;

                if (cur == null) return false;

                cur.Move(direction);

                return true;
            }
            catch
            {
            }

            return false;
        }

        public bool RotateActiveTetromino()
        {
            try
            {
                var cur = ActiveTetromino;

                if (cur == null) return false;

                return cur.Rotate();
            }
            catch
            {
            }

            return false;
        }

        public Tetromino ActiveTetromino
        {
            get
            {
                if (State == GameState.Running)
                {
                    return Board.AllObjects.FirstOrDefault(t => t is Tetromino && !((Tetromino)t).IsResting) as Tetromino;
                }

                return null;
            }
        }

        internal Tetromino DropNext()
        {
            if (State == GameState.Finished)
            {
                throw new InvalidOperationException();
            }

            var next = _generator.Generate();

            var centre = Board.Width / 2;

            next.Move(new Point2D() { X = centre });

            Board.AllObjects.ToList().ForEach(t =>
            {
                t.Selected = false;
            });

            try
            {
                Board.SetObject(next);
            }
            catch (InvalidOperationException)
            {
                State = GameState.Finished;
            }

            next.Selected = true;
            next.Begin();

            State = GameState.Running;

            return next;
        }
    }
}
