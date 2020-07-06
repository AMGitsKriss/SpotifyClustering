using DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DummyRepository
{
    public class DumbSpotifyRepository 
    {
        private JObject LoadFromFile(string filename)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"json\" + filename);
            using (StreamReader file = File.OpenText(path))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                return (JObject)JToken.ReadFrom(reader);
            }
        }

        public List<Playlist> GetPlaylists(string username)
        {
            JObject playlists = LoadFromFile("playlists.json");
            return playlists["items"].ToObject<List<Playlist>>();
        }

        public TrackFeatures GetTrackFeatures(Guid trackID)
        {
            JObject features = LoadFromFile("track-features.json");
            return features.ToObject<TrackFeatures>();
        }

        public List<TrackSummary> GetTrackList(string playlist)
        {
            JObject trackList = LoadFromFile("track-features.json");
            return trackList["items"].ToObject<List<TrackSummary>>();
        }
    }
}
