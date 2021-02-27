﻿using DTO;
using Logic;
using LogicContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlaylistManager;
using PlaylistManager.Strategies.Save;
using System.Collections.Generic;

namespace Website.Controllers
{
    public class SpotifyController : Controller
    {
        private ISpotifyLogic _logic;

        public SpotifyController(ISpotifyLogic logic)
        {
            _logic = logic;
        }
        public IActionResult Index()
        {
            /*
             * TODO - API Keys
             * Check if there's an API key and a User in session that we can use.
             * If we have both, we want to skip ahead to grabbing a list of playlists
             */

            LoginSession user = UserSession();

            return View();
        }

        public IActionResult Login([FromQuery] string code)
        {
            // TODO - Call the token endpoint with the code, save the resulting Tokens to the session, do a GetUser query and save the object, then redirect to Index
            LoginSession user = _logic.Login(code);
            HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
            return RedirectToAction("Index");
        }

        public IActionResult GetPlaylists()
        {
            // TODO - Get and display all the user's playlists.
            var playlists = _logic.GetPlaylists(UserSession().User.Username);
            return PartialView(null);
        }

        public IActionResult GetTracks(List<string> playlistIDs)
        {
            // TODO - Get all the songs and show the options on the screen for 
            var tracks = _logic.GetPlaylistTracks(playlistIDs);
            return PartialView(null);
        }


        public IActionResult CalculatePlaylists(List<string> trackIDs)
        {
            // TODO - Get all the songs and show the options on the screen for 
            var tracks = _logic.GetPlaylistTracks(trackIDs);
            var saveStrategy = new PushPlaylist(_logic);
            // TODO - We need to have the PlaylistManger accept a list of track IDs. Not just a username.
            var manager = new PlaylistManager.PlaylistManager("", UserSession().User.Username, saveStrategy);
            manager.Organise();
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
