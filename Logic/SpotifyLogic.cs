using DTO;
using LogicContracts;
using Microsoft.Extensions.Configuration;
using RepositoryContracts;
using Repsoitory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Logic
{
    // TODO - Just call it SpotifyLogic
    public class SpotifyLogic : ISpotifyLogic
    {
        private ISpotifyRepository _repo;

        public SpotifyLogic(ISpotifyRepository repo)
        {
            _repo = repo;
        }

        public LoginSession Login(string code)
        {
            LoginSession result = new LoginSession();

            IConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("appsettings.json");
            IConfiguration config = configBuilder.Build();

            TokenRequest request = new TokenRequest()
            {
                Code = code,
                ClientID = config["Endpoints:Spotify:ClientID"],
                ClientSecret = config["Endpoints:Spotify:SecretKey"],
            };
            result.Tokens = _repo.GetToken(request);
            result.User = _repo.GetCurrentUser();
            return result;
        }

        public List<Playlist> GetPlaylists(string username)
        {
            List<Playlist> results = new List<Playlist>();
            int previousCount = -1;
            int offset = 0;
            while (results.Count != previousCount)
            {
                previousCount = results.Count;
                results.AddRange(_repo.GetPlaylists(username, offset));
                offset += 20;
            }
            return results;
        }

        /// <summary>
        /// Iterate over a list of playlist IDs to fetch a list of tracks contained therein.
        /// </summary>
        /// <param name="playlistIDs"></param>
        /// <returns>A collection of tracks with duplicates removed</returns>
        public List<TrackSummary> GetPlaylistTracks(List<string> playlistIDs)
        {
            List<TrackSummary> trackSet = new List<TrackSummary>();

            foreach (string id in playlistIDs)
            {
                List<TrackSummary> results = new List<TrackSummary>();
                int previousCount = -1;
                int offset = 0;
                while (results.Count != previousCount)
                {
                    previousCount = results.Count;
                    results.AddRange(_repo.GetTrackList(id, offset));
                    offset += 100;
                }
                trackSet.AddRange(results);
            }
            trackSet = trackSet.GroupBy(x => x.ID).Select(y => y.First()).ToList();
            return trackSet;
        }

        public List<TrackFeatures> GetTrackFeatures(List<TrackSummary> trackList)
        {
            List<string> trackIDs = trackList.Select(x => x.ID).ToList();
            List<TrackFeatures> result = _repo.GetTrackFeatures(trackIDs);

            foreach (var item in result)
            {
                TrackSummary itemSummary = trackList.First(x => x.ID.Equals(item.ID));
                item.Name = itemSummary.Name.Replace(",", string.Empty);
                item.Artist = itemSummary.Artist.Replace(",", string.Empty);
            }

            return result;

        }

        public Playlist AddNewPlaylist(string username, string playlistName)
        {
            BasePlaylist request = new BasePlaylist()
            {
                Name = playlistName,
                Public = false
            };
            return _repo.AddNewPlaylist(username, request);
        }

        public bool AddTrack(string playlistID, List<string> uris)
        {
            AddTrackRequest request = new AddTrackRequest()
            {
                uris = uris
            };
            return _repo.AddTrack(playlistID, request);
        }
    }
}
