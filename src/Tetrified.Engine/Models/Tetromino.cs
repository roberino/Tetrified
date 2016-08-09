using System;
using System.Collections.Generic;
using UniversalGrid.Drawing;
using UniversalGrid.Geometry;

namespace Tetrified.Engine.Models
{
    public class Tetromino : Spatial2DThing<char>
    {
        private readonly ITimer _timer;

        public Tetromino(char shape, ITimer timer) : base(GetCoords(shape))
        {
            _timer = timer;

            Colour = GetColour(shape);
            RotationalCentre = GetCentre(shape);
            Data = shape;
        }

        public void Begin()
        {
            _timer.RunUntil(Fall);
        }

        public event EventHandler Resting;

        public bool IsResting { get; private set; }

        private void OnResting()
        {
            IsResting = true;

            var ev = Resting;

            if(ev != null)
            {
                ev.Invoke(this, EventArgs.Empty);
            }
        }

        private bool Fall()
        {
            if (IsResting) return true;

            try
            {
                if (Move(new Point2D() { Y = 1 }))
                {
                    return false;
                }
            }
            catch (ObjectOutOfBoundsException) { }
            catch (InvalidOperationException) { }

            OnResting();

            return true;
        }

        public static IEnumerable<char> Shapes
        {
            get
            {
                yield return 'I';
                yield return 'O';
                yield return 'T';
                yield return 'S';
                yield return 'Z';
                yield return 'J';
                yield return 'L';
            }
        }

        private static Colour GetColour(char shape)
        {
            switch (shape)
            {
                case 'I':
                    return Col(0, 255, 255);
                case 'O':
                    return Col(255, 216, 0);
                case 'T':
                    return Col(178, 45, 255);
                case 'S':
                    return Col(80, 255, 96);
                case 'Z':
                    return Col(255, 0, 0);
                case 'J':
                    return Col(0, 38, 255);
                case 'L':
                    return Col(255, 195, 0);
            }

            return new Colour();
        }

        private static Point2D GetCentre(char shape)
        {
            switch (shape)
            {
                case 'L':
                    return Coord(0, 1);
                case 'O':
                    return Coord(0, 1);
                case 'S':
                    return Coord(2, 0);
                case 'I':
                    return Coord(0, 1);
                case 'Z':
                    return Coord(0, 1);
                case 'T':
                    return Coord(0, 1);
                case 'J':
                    return Coord(1, 1);
            }

            return new Point2D();
        }

        private static IEnumerable<Point2D> GetCoords(char shape)
        {
            switch (shape)
            {
                case 'L':
                    yield return Coord(0, 0);
                    yield return Coord(0, 1);
                    yield return Coord(0, 2);
                    yield return Coord(1, 2);
                    break;
                case 'O':
                    yield return Coord(0, 0);
                    yield return Coord(0, 1);
                    yield return Coord(1, 0);
                    yield return Coord(1, 1);
                    break;
                case 'S':
                    yield return Coord(1, 0);
                    yield return Coord(2, 0);
                    yield return Coord(0, 1);
                    yield return Coord(1, 1);
                    break;
                case 'I':
                    yield return Coord(0, 0);
                    yield return Coord(0, 1);
                    yield return Coord(0, 2);
                    yield return Coord(0, 3);
                    break;
                case 'Z':
                    yield return Coord(0, 0);
                    yield return Coord(0, 1);
                    yield return Coord(1, 1);
                    yield return Coord(1, 2);
                    break;
                case 'T':
                    yield return Coord(0, 0);
                    yield return Coord(0, 1);
                    yield return Coord(0, 2);
                    yield return Coord(1, 1);
                    break;
                case 'J':
                    yield return Coord(1, 0);
                    yield return Coord(1, 1);
                    yield return Coord(1, 2);
                    yield return Coord(1, 3);
                    break;
            }
        }

        private static Colour Col(byte r, byte g, byte b)
        {
            return new Colour()
            {
                R = r,
                G = g,
                B = b,
                A = 255
            };
        }

        private static Point2D Coord(int x, int y)
        {
            return new Point2D()
            {
                X = x,
                Y = y
            };
        }
    }
}
