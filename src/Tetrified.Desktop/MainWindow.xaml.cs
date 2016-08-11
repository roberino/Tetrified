using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UniversalGrid;

namespace Tetrified.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const double unitSize = 20;

        private bool _closing;

        private readonly Engine.Engine _gameEngine;

        public MainWindow()
        {
            InitializeComponent();

            _gameEngine = new Engine.Engine();

            _gameEngine.Game.Board.ItemMoved += RenderCanvas;

            foreach(var cell in _gameEngine.Game.Board.Rows.SelectMany(r => r))
            {
                main.Children.Add(new Border()
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    Width = cell.X * unitSize,
                    Height = cell.Y * unitSize
                });
            }

            base.KeyDown += OnKey;

            _gameEngine.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _closing = true;
            base.OnClosing(e);
        }

        private void OnKey(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    _gameEngine.Game.MoveActiveTetromino(UniversalGrid.Geometry.Direction.Down);
                    break;
                case Key.Up:
                    _gameEngine.Game.RotateActiveTetromino();
                    break;
                case Key.Left:
                    _gameEngine.Game.MoveActiveTetromino(UniversalGrid.Geometry.Direction.Left);
                    break;
                case Key.Right:
                    _gameEngine.Game.MoveActiveTetromino(UniversalGrid.Geometry.Direction.Right);
                    break;
            }
        }

        private void RenderCanvas(object sender, UniversalGrid.Events.ObjectEvent<UniversalGrid.Geometry.ISpatial2DThing<char>> e)
        {
            if (_closing) return;

            Dispatcher.Invoke(() =>
            {
                if (_closing) return;

                if (main.Children.Count > 0)
                {
                    main.Children.Clear();
                }
            }, DispatcherPriority.DataBind);

            Dispatcher.Invoke(() =>
            {
                if (_closing) return;

                var grid = (UniversalGrid<char>)sender;

                grid.Render((p, o) =>
                {
                    foreach(var item in o)
                    {
                        var sq = new Rectangle()
                        {
                            Fill = new SolidColorBrush(Color.FromArgb(item.Colour.A, item.Colour.R, item.Colour.G, item.Colour.B)),
                            Width = unitSize,
                            Height = unitSize
                        };

                        Canvas.SetLeft(sq, p.X * unitSize);
                        Canvas.SetTop(sq, p.Y * unitSize);

                        main.Children.Add(sq);
                    }
                });
            }, DispatcherPriority.Render);
        }
    }
}
