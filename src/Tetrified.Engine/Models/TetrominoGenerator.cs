using System;
using System.Linq;

namespace Tetrified.Engine.Models
{
    public class TetrominoGenerator
    {
        private readonly Random _rnd;
        private readonly char[] _shapes;
        private readonly ITimer _timer;

        public TetrominoGenerator(ITimer timer = null)
        {
            _rnd = new Random(DateTime.Now.Millisecond);
            _shapes = Tetromino.Shapes.ToArray();
            _timer = timer ?? new DefaultTimer(TimeSpan.FromSeconds(1));
        }

        public Tetromino Generate()
        {
            var x = _rnd.Next(_shapes.Length);

            return new Tetromino(_shapes[x], _timer);
        }
    }
}