using DTO;
using Newtonsoft.Json;
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
            List<Playlist> result = new List<Playlist>();
            while (!result.Any() || result.Count % 20 == 0)
            {
                string query = $"users/{username}/playlists?limit=20&offset={result.Count}";
                JObject playlists = (JObject)Query(requestType.GET, query);
                result.AddRange(playlists["items"].ToObject<List<Playlist>>());
            }
            return result;
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
        public List<TrackFeatures> GetTrackFeatures(List<string> trackIDs)
        {
            List<TrackFeatures> tracks = new List<TrackFeatures>();
            int batchSize = 100;

            for (int i = 0; i < trackIDs.Count; i += batchSize)
            {
                List<string> trackBatch = trackIDs.GetRange(i, Math.Min(batchSize, trackIDs.Count - i));
                string query = $"audio-features?ids={string.Join(",", trackBatch)}";

                JObject tempTracks = (JObject)Query(requestType.GET, query);
                tracks.AddRange(tempTracks["audio_features"].ToObject<List<TrackFeatures>>());
            }

            return tracks;
        }

        /// <summary>
        /// Takes a track ID and returns the track features. (danceability, energy, tempo, etc)
        /// </summary>
        public Playlist AddNewPlaylist(string username, BasePlaylist request)
        {
            string query = $"users/{username}/playlists";
            JObject playlists = (JObject)Query(requestType.POST, query, JsonConvert.SerializeObject(request));
            Playlist result = playlists.ToObject<Playlist>();
            return result;
        }

        /// <summary>
        /// Takes a track ID and returns the track features. (danceability, energy, tempo, etc)
        /// </summary>
        public bool AddTrack(string playlistID, AddTrackRequest request)
        {
            string query = $"playlists/{playlistID}/tracks";
            JObject playlists = (JObject)Query(requestType.POST, query, JsonConvert.SerializeObject(request));
            bool result = playlists.HasValues;
            return result;
        }
    }
}
