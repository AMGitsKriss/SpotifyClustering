using DTO;
using LogicContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlaylistManager;
using System.Collections.Generic;
using Website.Models.Spotify;

namespace Website.Controllers
{
    public class SpotifyController : Controller
    {
        private ISpotifyLogic _logic;
        private Manager _playlistManager;

        public SpotifyController(ISpotifyLogic logic, Manager playlistManager)
        {
            _logic = logic;
            _playlistManager = playlistManager;
        }
        public IActionResult Index()
        {
            LoginSession user = UserSession();

            return View(user);
        }

        public IActionResult Login([FromQuery] string code)
        {
            LoginSession user = _logic.Login(code);
            HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
            return RedirectToAction("Index");
        }

        public IActionResult GetPlaylists()
        {
            var user = UserSession();
            _logic.SetUser(user);
            GetPlaylistsViewModel model = new GetPlaylistsViewModel()
            {
                Playlists = _logic.GetPlaylists(user.User.Username)
            };
            return PartialView("_GetPlaylists", model);
        }

        public IActionResult GetTracks(List<string> playlistIDs)
        {
            var user = UserSession();
            _logic.SetUser(user);
            GetTracksViewModel model = new GetTracksViewModel()
            {
                Tracks = _logic.GetPlaylistTracks(playlistIDs)
            };
            return PartialView("_GetTracks", model);
        }


        public IActionResult CalculatePlaylists(List<string> trackIDs)
        {
            // TODO - Get all the songs and show the options on the screen for 
            var tracks = _logic.GetPlaylistTracks(trackIDs);
            _playlistManager.Organise();
            return PartialView(null);
        }

        public IActionResult SavePlaylists()
        {
            return PartialView(null);
        }

        private LoginSession UserSession()
        {
            return JsonConvert.DeserializeObject<LoginSession>(HttpContext.Session.GetString("user") ?? string.Empty);
        }
    }
}
