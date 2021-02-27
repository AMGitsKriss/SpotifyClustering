using PlaylistManager.Factories;
using PlaylistManager.Models;
using PlaylistManager.Strategies.Clustering;
using PlaylistManager.Strategies.Save;
using DTO;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using LogicContracts;

namespace PlaylistManager
{
    public class Manager
    {
        private ISpotifyLogic _logic;
        private ISaveStrategy saveStrategy;

        public Manager(ISpotifyLogic logic, ISaveStrategy strategy)
        {
            _logic = logic;
            saveStrategy = strategy;
        }

        public void Organise()
        {
            List<TrackFeatures> features = GetFeatures();
            List<Vector> groupedTracks = Cluster(features);

            saveStrategy.Save(features, groupedTracks, "username");
        }

        private List<TrackFeatures> GetFeatures()
        {

            List<Playlist> playlists = _logic.GetPlaylists("username");
            List<string> filteredPlaylists = playlists.Where(x => "trackIDs".Contains(x.ID)).Select(x => x.ID).ToList();

            List<TrackSummary> tracks = _logic.GetPlaylistTracks(filteredPlaylists).ToList();

            List<TrackFeatures> result = _logic.GetTrackFeatures(tracks);

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
