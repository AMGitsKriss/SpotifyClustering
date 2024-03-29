﻿using DTO;
using Logic.Factories;
using Logic.Save;
using LogicContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PlaylistManager;
using PlaylistManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Website.Models.Spotify;

namespace Website.Controllers
{
    public class SpotifyController : Controller
    {
        private ISpotifyLogic _logic;
        private ISavePlaylistLogic _save;
        private ClusteringManager _playlistManager;
        private ILogger<SpotifyController> _logger;

        public SpotifyController(ISpotifyLogic logic, ISavePlaylistLogic save, ClusteringManager playlistManager, ILogger<SpotifyController> logger)
        {
            _logic = logic;
            _save = save;
            _playlistManager = playlistManager;
            _logger = logger;
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
            try
            {
                var user = UserSession();
                _logic.SetUser(user);
                GetPlaylistsViewModel model = new GetPlaylistsViewModel()
                {
                    Playlists = _logic.GetPlaylists(user.User.Username)
                };

                return PartialView("_GetPlaylists", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Caught in GetPlaylists");
                return BadRequest();
            }
        }

        public IActionResult GetTracks(List<string> playlistIDs)
        {
            try
            {
                var user = UserSession();
                _logic.SetUser(user);
                var tracks = _logic.GetPlaylistTracks(playlistIDs);
                HttpContext.Session.SetString("tracks", JsonConvert.SerializeObject(tracks));
                GetTracksViewModel model = new GetTracksViewModel()
                {
                    Tracks = tracks
                };
                return PartialView("_GetTracks", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Caught in GetTracks");
                return BadRequest();
            }
        }


        public IActionResult BuildPlaylists(BuildPlaylistRequest request)
        {
            try
            {
                var user = UserSession();
                _logic.SetUser(user);

                var tracks = TrackSession().Where(t => request.TrackIDs.Contains(t.ID)).ToList();
                var trackFeatures = _logic.GetTrackFeatures(tracks.ToList());
                var featureVectors = trackFeatures.Select(f => new Vector() { ID = f.ID, Features = Builders.FeatureBuilder.BuildVector(f) }).ToList();

                _playlistManager.SetDataPool(featureVectors);
                _playlistManager.SetConfigs(request.MinimumSize, request.MinimumDistance);
                _playlistManager.SetNoisResolutionMethod(NoiseStrategyAttribute.GetInstance(request.NoiseStrategy));
                var result = _playlistManager.FindClusters();

                BuildPlaylistsViewModel model = new BuildPlaylistsViewModel()
                {
                    Playlists = new List<PlaylistViewModel>()
                };
                foreach (var cluster in result)
                {
                    if (cluster.Features.Any())
                    {
                        var clusterIDs = cluster.Features.Select(f => f.ID);
                        var clusterTracks = tracks.Where(t => clusterIDs.Contains(t.ID)).ToList();
                        var filteredFeatures = trackFeatures.Where(f => clusterTracks.Select(t => t.ID).Contains(f.ID));
                        PlaylistViewModel pl = new PlaylistViewModel()
                        {
                            Playlist = new Playlist()
                            {
                                Name = Guid.NewGuid().ToString()
                            },
                            ClusterID = cluster.ClusterID,
                            TrackFeatures = filteredFeatures.ToList(),
                            FeatureVectors = filteredFeatures.Select(f => Builders.FeatureBuilder.BuildCSVRow(f)).ToList()
                        };
                        model.Playlists.Add(pl);
                    }
                }

                HttpContext.Session.SetString("playlists", JsonConvert.SerializeObject(model));

                return PartialView("_BuildPlaylists", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Caught in BuildPlaylists");
                return BadRequest();
            }
        }

        public IActionResult SavePlaylists(Dictionary<int, string> playListNames)
        {
            try
            {
                var user = UserSession();
                _save.SetUser(user);

                var playlists = PlaylistsSession();
                foreach (var item in playlists.Playlists)
                {
                    _save.Save(item.TrackFeatures, playListNames[item.ClusterID]);
                }
                return PartialView("_SavePlaylists", playListNames);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Caught in SavePlaylists");
                return BadRequest();
            }
        }

        private LoginSession UserSession()
        {
            return JsonConvert.DeserializeObject<LoginSession>(HttpContext.Session.GetString("user") ?? string.Empty);
        }

        private List<TrackSummary> TrackSession()
        {
            return JsonConvert.DeserializeObject<List<TrackSummary>>(HttpContext.Session.GetString("tracks") ?? string.Empty);
        }

        private BuildPlaylistsViewModel PlaylistsSession()
        {
            return JsonConvert.DeserializeObject<BuildPlaylistsViewModel>(HttpContext.Session.GetString("playlists") ?? string.Empty);
        }
    }
}
