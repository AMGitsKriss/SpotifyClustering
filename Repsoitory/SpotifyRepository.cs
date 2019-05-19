using DTO;
using Newtonsoft.Json.Linq;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repsoitory
{
    public class SpotifyRepository : BaseRepository, ISpotifyRepository
    {

        public SpotifyRepository(string apiKey)
        {
            _apiKey = apiKey;
        }

        /// <summary>
        /// Takes a Spotify username and fetches the user's playlists.
        /// </summary>
        public List<Playlist> GetPlaylists(string username)
        {
            string query = $"users/{username}/playlists";
            JObject playlists = (JObject)Query(requestType.GET, query);
            return playlists["items"].ToObject<List<Playlist>>();
        }

        /// <summary>
        /// Accepts a playlist ID and returns a list of tracks.
        /// </summary>
        public List<TrackSummary> GetTrackList(string playlistID)
        {
            string query = $"playlists/{playlistID}/tracks";
            JObject playlists = (JObject)Query(requestType.GET, query);

            // TODO - Use a dictionary instead.
            Dictionary<string, TrackSummary> results = new Dictionary<string, TrackSummary>();
            foreach (JToken item in playlists["items"])
            {
                TrackSummary tmp = item["track"].ToObject<TrackSummary>();
                tmp.Artist = item["track"]["artists"][0]["name"].ToString();
                results.Add(tmp.ID, tmp);
            }

            return results.Values.ToList();
        }

        /// <summary>
        /// Takes a track ID and returns the track features. (danceability, energy, tempo, etc)
        /// </summary>
        public TrackFeatures GetTrackFeatures(Guid trackID)
        {
            // TODO - Get trackfeatures for a 
            throw new NotImplementedException();
        }
    }
}
