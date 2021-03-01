using DTO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Repsoitory
{
    public class SpotifyRepository : BaseRepository, ISpotifyRepository
    {
        private string _baseUri = "https://api.spotify.com/v1";
        private IConfiguration _config;

        public SpotifyRepository(IConfiguration config) : base (config)
        {
            _config = config;
        }

        public void SetAuthorization(string apiKey)
        {
            _apiKey = apiKey;
        }

        /// <summary>
        /// It's an authorisation code and returns access and refresh tokens to the application.
        /// </summary>
        public TokenResponse GetToken(TokenRequest request)
        {
            // TODO - This setting on the content type is messy. Apply it as a strategy.
            // TODO - We use different authorization methods at different stages. Make a request factory.
            string query = $"https://accounts.spotify.com/api/token";
            string body = $"grant_type=authorization_code&code={request.Code}&redirect_uri={HttpUtility.UrlEncode(_config["Endpoints:Spotify:Callback"])}&client_id={request.ClientID}&client_secret={request.ClientSecret}";
            _contentType = _form;
            _apiKey = $"Basic {EncodeTo64($"{_config["Endpoints:Spotify:ClientID"]}:{_config["Endpoints:Spotify:SecretKey"]}")}";
            JObject tokens = (JObject)MakeRequest(RequestType.POST, query, body);
            _contentType = _json;
            TokenResponse result = tokens.ToObject<TokenResponse>(); 
            _apiKey = $"Bearer {result.AccessToken}";
            return result;
        }

        /// <summary>
        /// It's an authorisation code and returns access and refresh tokens to the application.
        /// </summary>
        public TokenResponse RefreshToken(TokenRequest request)
        {
            // TODO - This setting on the content type is messy. Apply it as a strategy.
            string query = $"https://accounts.spotify.com/api/token";
            string body = $"grant_type=authorization_code&grant_type={request.GrantType}&refresh_token={request.RefreshToken}&client_id={request.ClientID}&client_secret={request.ClientSecret}";
            _contentType = _form;
            JObject tokens = (JObject)MakeRequest(RequestType.POST, query, body);
            _contentType = _json;
            TokenResponse result = tokens.ToObject<TokenResponse>(); 
            _apiKey = result.AccessToken;
            return result;
        }

        /// <summary>
        /// Takes a Spotify username and fetches the user's playlists.
        /// </summary>
        public User GetCurrentUser()
        {
            string query = $"{_baseUri}/me";
            JObject playlists = (JObject)MakeRequest(RequestType.GET, query);
            User result = playlists.ToObject<User>();
            return result;
        }

        /// <summary>
        /// Takes a Spotify username and fetches the user's playlists.
        /// </summary>
        public List<Playlist> GetPlaylists(string username)
        {
            List<Playlist> result = new List<Playlist>();
            while (!result.Any() || result.Count % 20 == 0)
            {
                string query = $"{_baseUri}/users/{username}/playlists?limit=20&offset={result.Count}";
                JObject playlists = (JObject)MakeRequest(RequestType.GET, query);
                result.AddRange(playlists["items"].ToObject<List<Playlist>>());
            }
            return result;
        }

        /// <summary>
        /// Accepts a playlist ID and returns a list of tracks.
        /// </summary>
        public List<TrackSummary> GetTrackList(string playlistID, int offset)
        {
            string query = $"{_baseUri}/playlists/{playlistID}/tracks?limit=100&offset={offset}";
            JObject playlists = (JObject)MakeRequest(RequestType.GET, query);

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
                List<string> trackBatch = trackIDs.GetRange(i, Math.Min(batchSize, trackIDs.Count-i));
                string query = $"{_baseUri}/audio-features?ids={string.Join(",", trackBatch)}";

                JObject tempTracks = (JObject)MakeRequest(RequestType.GET, query);
                tracks.AddRange(tempTracks["audio_features"].ToObject<List<TrackFeatures>>());
            }

            return tracks;
        }

        /// <summary>
        /// Takes a track ID and returns the track features. (danceability, energy, tempo, etc)
        /// </summary>
        public Playlist AddNewPlaylist(string username, BasePlaylist request)
        {
            string query = $"{_baseUri}/users/{username}/playlists";
            JObject playlists = (JObject)MakeRequest(RequestType.POST, query, JsonConvert.SerializeObject(request));
            Playlist result = playlists.ToObject<Playlist>();
            return result;
        }

        /// <summary>
        /// Takes a track ID and returns the track features. (danceability, energy, tempo, etc)
        /// </summary>
        public bool AddTrack(string playlistID, AddTrackRequest request)
        {
            string query = $"{_baseUri}/playlists/{playlistID}/tracks";
            JObject playlists = (JObject)MakeRequest(RequestType.POST, query, JsonConvert.SerializeObject(request));
            bool result = playlists.HasValues;
            return result;
        }
    }
}
