using Accord.MachineLearning;
using Accord.Math.Distances;
using ConsoleApp.Factories;
using DTO;
using Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ConsoleApp
{
    class Program
    {
        static List<TrackFeatures> features;
        static double[][] observations;
        static string username = "amgitskriss";
        static string token = "BQDO0JAFGoQAXEhnkbTGy53zVjMTNHSfsi_d1-2bBfq_RRxLFCcA7nBfUOfRwGTxX1bZGIgNExjkGWGtdnrutgHC0R3R7Y9EHb6eHnoNKwm1P1q9qgl-39Ek4Fd8WiTibppte4XbZDI-whYDlQ03FXx_pR8wCrQzxvD7T8mW_g-orRCbJYTL3ebsbXRJNHHalZU5dN1QFZXo69yFSrtCUKOHl7_y-NxoHbT4rdY38XgYwxtC1GXLT8A5l4d8doT0AowoYwOcS0wq3HA";
        static void Main(string[] args)
        {
            GetFeatures();
            List<Cluster> clusters = Cluster(3, 10);
            Cluster result = FindOptimumClusterCount(clusters);
            SaveNewPlaylists(result);
        }

        static List<TrackFeatures> GetFeatures()
        {
            Console.WriteLine("Hello World!");
            CollectDataLogic logic = new CollectDataLogic("Bearer " + token);

            List<Playlist> playlists = logic.GetPlaylists(username);
            List<string> filteredPlaylists = playlists.Where(x => x.Name.Contains("iPhone", StringComparison.InvariantCultureIgnoreCase)).Select(x => x.ID).ToList();

            List<TrackSummary> tracks = logic.GetPlaylistTracks(filteredPlaylists).ToList();

            features = logic.GetTrackFeatures(tracks);

            WriteCSV(features, @"C:\Users\Kriss\Desktop\spotify_features.csv");

            return features;
        }

        static List<Cluster> Cluster(int min = 3, int max = 3)
        {
            observations = new double[features.Count()][];
            for (int i = 0; i < features.Count(); i++)
            {
                observations[i] = FeatureFactory.BuildVector(features[i]);
            }

            List<Cluster> myClusters = new List<Cluster>();
            for (int clusterSize = min; clusterSize <= max; clusterSize++)
            {
                Cluster tmpCluster = new Cluster();
                tmpCluster.Size = clusterSize;
                tmpCluster.KMeans = new KMeans(k: clusterSize)
                {
                    Distance = new WeightedSquareEuclidean(FeatureFactory.GenerateWeights(acousticness: 1.2, speechiness: 1.2, instrumentalness: 1.2, tempo: 0.8, key: 0.8, energy: 1.2))
                };
                myClusters.Add(tmpCluster);
            }

            return myClusters;
        }

        private static Cluster FindOptimumClusterCount(List<Cluster> clustersList)
        {
            foreach (var item in clustersList)
            {
                int[] labels = item.KMeans.Learn(observations).Decide(observations);

                item.GroupedTracks = new List<TrackFeatures>[item.Size];
                for (int i = 0; i < labels.Length; i++)
                {
                    if (item.GroupedTracks[labels[i]] == null) item.GroupedTracks[labels[i]] = new List<TrackFeatures>();
                    item.GroupedTracks[labels[i]].Add(features.Single(x => x.ID.Equals(features[i].ID)));
                }
            }
            var err = clustersList.Select(x => x.KMeans.Error).ToList();
            List<double> diffList = new List<double>();
            for (int i = 0; i < err.Count-1; i++)
            {
                double diff = Math.Abs(err[i]- err[i+1]);
                diffList.Add(diff);
            }
            double avg = diffList.Average();
            int resultIndex = 0;
            for (int i = 0; i < diffList.Count; i++)
            {
                if (diffList[i] < avg)
                {
                    resultIndex = i+1;
                    break;
                }
            }
            //Derr 
            // Prev + next - (2 * this)
            return clustersList[resultIndex];
        }

        static void SaveNewPlaylists(Cluster cluster)
        {
            PostDataLogic logic = new PostDataLogic("Bearer " + token);
            for (int i = 0; i < cluster.GroupedTracks.Length; i++)
            {
                Playlist newPlaylist = logic.AddNewPlaylist(username, $"API Cluster Test {i + 1}");
                List<string> trackList = cluster.GroupedTracks[i].Select(x => x.URI).ToList();

                int batchSize = 100;
                for (int j = 0; j < trackList.Count; j += batchSize)
                {
                    List<string> trackBatch = trackList.GetRange(i, Math.Min(batchSize, trackList.Count - i));
                    logic.AddTrack(newPlaylist.ID, trackBatch);
                }
            }
        }

        static void WriteCSV<T>(IEnumerable<T> items, string path)
        {
            Type itemType = typeof(T);
            var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .OrderBy(p => p.Name);

            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine(string.Join(", ", props.Select(p => Escape(p.Name))));

                foreach (var item in items)
                {
                    writer.WriteLine(string.Join(", ", props.Select(p => Escape(p.GetValue(item, null).ToString()))));
                }
            }
        }

        static string Escape(string val)
        {
            if (val == null) return null;
            return val.Replace("\"", "\"\"");
        }
    }
}
