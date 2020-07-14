using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Website.Helpers;
using Website.Models;
using Website.Models.Home;
using Website.Models.Shared;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        private ModelHandler _handler;
        public HomeController(ModelHandler handler)
        {
            _handler = handler;
        }

        public IActionResult Index()
        {
            HomeViewModel model = (HomeViewModel)_handler.Create<HomeViewModel>();
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
