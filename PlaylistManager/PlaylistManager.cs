using PlaylistManager.Factories;
using PlaylistManager.Models;
using PlaylistManager.Strategies.Clustering;
using PlaylistManager.Strategies.Save;
using DTO;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlaylistManager
{
    public class PlaylistManager
    {
        string username;
        string apiToken;

        ISaveStrategy saveStrategy;

        public PlaylistManager(string apiToken, string username, ISaveStrategy strategy)
        {
            this.apiToken = apiToken;
            this.username = username;
            saveStrategy = strategy;
        }

        public void Organise()
        {
            List<TrackFeatures> features = GetFeatures();
            List<Vector> groupedTracks = Cluster(features);

            saveStrategy.Save(features, groupedTracks, username);
        }

        private List<TrackFeatures> GetFeatures()
        {
            CollectDataLogic logic = new CollectDataLogic("Bearer " + apiToken);

            List<Playlist> playlists = logic.GetPlaylists(username);
            List<string> filteredPlaylists = playlists.Where(x => x.Name.Contains("iPhone", StringComparison.InvariantCultureIgnoreCase)).Select(x => x.ID).ToList();

            List<TrackSummary> tracks = logic.GetPlaylistTracks(filteredPlaylists).ToList();

            List<TrackFeatures> result = logic.GetTrackFeatures(tracks);

            return result;
        }

        private List<Vector> Cluster(List<TrackFeatures> features)
        {
            List<Vector> featureVectors = new List<Vector>();

            featureVectors.AddRange(features.Select(f => new Vector() { ID = f.ID, Features = FeatureFactory.BuildVector(f) }));

            IClusteringStrategy algorithm = new DbScan(10, 0.6);
            algorithm.Search(featureVectors);

            int clusters = featureVectors.Max(m => m.Cluster);

            Dictionary<int, int> clusterCount = new Dictionary<int, int>();
            for (int i = -1; i <= clusters; i++)
            {
                clusterCount.Add(i, featureVectors.Count(t => t.Cluster == i));
            }
            return featureVectors;
        }
    }
}
