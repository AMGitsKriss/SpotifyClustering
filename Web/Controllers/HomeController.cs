using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DTO;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Repsoitory;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string username, string[] playlists)
        {
            if (username != null && playlists.Any()) // Step 3
            {
                ViewData["username"] = username;
                ViewData["playlists"] = true;
            }
            else if (username != null) // Step 2
            {
                ViewData["username"] = username;
            }
            else // Step 1
            {
                // Atub
            }
            return View();
        }

        public IActionResult Playlists(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new Exception();
            }

            string key = "Bearer BQDEy5p6ILxzPwqifRlm5h-UmDlNQT-kL0dKKfntB7LCN3p0xwrsnJNDaPqXTW3j7pEZTrDD8DDcv5CtkPaHKpfx8b2kce_jFENhOzl_1qT93UxBf5HvVVX3qjpS5tF3RVNNvouzCVA-YZXT3A4";
            ISpotifyRepository repo = new SpotifyRepository(key);
            List<Playlist> result = repo.GetPlaylists("AMGitsKriss");

            @ViewData["username"] = username;
            return PartialView("_Step2", result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
