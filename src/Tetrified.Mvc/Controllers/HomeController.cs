using System.Web.Mvc;

namespace Tetrified.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private static Engine.Engine _engine;

        static HomeController()
        {
            _engine = new Engine.Engine();
        }

        public ActionResult Index(bool restart = false, string move = null)
        {
            if (!_engine.IsRunning || restart)
            {
                _engine.Start();
            }

            switch (move)
            {
                case "left":
                    _engine.Game.MoveActiveTetromino(UniversalGrid.Geometry.Direction.Left);
                    break;
                case "right":
                    _engine.Game.MoveActiveTetromino(UniversalGrid.Geometry.Direction.Right);
                    break;
            }

            if (restart || !string.IsNullOrEmpty(move))
            {
                return new RedirectResult("/");
            }

            return View(_engine.Game.Board);
        }
    }
}