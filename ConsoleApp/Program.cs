using Accord.MachineLearning;
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
        static string token = "BQD6YL60sU0nj7mE1PGEkcOTXobNNRgkshwnkVWhVqnST-RQ601TpXsiGqXbgqBRyjTgyD1W_DMak3O7_3kGkgooEl54OQvmRpRgRSNDn5QMQCNLViYH2DSCLnKvPBsvxUZm7_7pdLD_1M7ivoA";
        static void Main(string[] args)
        {
            GetFeatures();
            ClusterFromCSV();
        }

        static List<TrackFeatures> GetFeatures()
        {
            Console.WriteLine("Hello World!");
            CollectDataLogic logic = new CollectDataLogic("Bearer " + token);
            string username = "amgitskriss";

            List<Playlist> playlists = logic.GetPlaylists(username);
            List<string> filteredPlaylists = playlists.Where(x => x.Name.Contains("iPhone", StringComparison.InvariantCultureIgnoreCase)).Select(x => x.ID).ToList();

            List<TrackSummary> tracks = logic.GetPlaylistTracks(filteredPlaylists).ToList();

            features = logic.GetTrackFeatures(tracks);

            //WriteCSV(features, @"C:\Users\Kriss\Desktop\spotify_features.csv");

            return features;
        }

        static void ClusterFromCSV()
        {
            double[][] observations = new double[features.Count()][];
            Dictionary<int, string> TrackIndexDict = new Dictionary<int, string>();
            for (int i = 0; i < features.Count(); i++)
            {
                try
                {
                    TrackIndexDict.Add(i, features[i].ID);
                    observations[i] = FeatureFactory.BuildVector(features[i]);
                }
                catch (Exception ex)
                {
                    // We really need to make sure we're rid of duplicates
                    throw ex;
                }

            }

            int clusterSize = 5;

            KMeans kmeans = new KMeans(k: clusterSize);
            KMeansClusterCollection clusters = kmeans.Learn(observations);
            int[] labels = clusters.Decide(observations);

            List<TrackFeatures>[] groupedTracks = new List<TrackFeatures>[clusterSize];
            for (int i = 0; i < labels.Length; i++)
            {
                if (groupedTracks[labels[i]] == null) groupedTracks[labels[i]] = new List<TrackFeatures>();
                groupedTracks[labels[i]].Add(features.Single(x => x.ID.Equals(TrackIndexDict[i])));
            }

            for (int i = 0; i < groupedTracks.Length; i++)
            {
                WriteCSV(groupedTracks[i], @"C:\Users\Kriss\Desktop\spotify_group_"+i+".csv");
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
