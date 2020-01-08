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
        static List<TrackFeatures>[] groupedTracks;
        static double[][] observations;
        static string username = "amgitskriss";
        static string token = "BQCoYqLwMlcwG6AnAtLGRCrS7Q3ws7YO9x9usxHfh7YuuW8gLf9r1hx29CqbO94k593hAEHK6PE6pCtjFoSR1XzwyeoSONYFklvZo1E0RPazMr5liJOhnFpehW08JvRnj9UpTyx-xslD_fO4XM0ykAocMPieNwBvFor1oYx142XyoQzhvpIg0BmgsLdgRQgzOOZ2BvoTAukmdVErvCrKt-HU";
        static void Main(string[] args)
        {
            GetFeatures();
            List<KMeans> clusters = Cluster();
            groupedTracks = FindOptimumClusterCount(clusters);
            SaveNewPlaylists();
        }

        static List<TrackFeatures> GetFeatures()
        {
            Console.WriteLine("Hello World!");
            CollectDataLogic logic = new CollectDataLogic("Bearer " + token);

            List<Playlist> playlists = logic.GetPlaylists(username);
            List<string> filteredPlaylists = playlists.Where(x => x.Name.Contains("iPhone", StringComparison.InvariantCultureIgnoreCase)).Select(x => x.ID).ToList();

            List<TrackSummary> tracks = logic.GetPlaylistTracks(filteredPlaylists).ToList();

            features = logic.GetTrackFeatures(tracks);

            //WriteCSV(features, @"C:\Users\Kriss\Desktop\spotify_features.csv");

            return features;
        }

        static List<KMeans> Cluster(int min = 3, int max = 3)
        {
            observations = new double[features.Count()][];
            for (int i = 0; i < features.Count(); i++)
            {
                observations[i] = FeatureFactory.BuildVector(features[i]);
            }

            List<KMeans> myList = new List<KMeans>();
            for (int clusterSize = min; clusterSize <= max; clusterSize++)
            {
                KMeans kmeans = new KMeans(k: clusterSize)
                {
                    Distance = new WeightedSquareEuclidean(FeatureFactory.GenerateWeights(acousticness: 1.1))
                };
                myList.Add(kmeans);
            }

            return myList;
        }

        private static List<TrackFeatures>[] FindOptimumClusterCount(List<KMeans> clustersList)
        {
            var kmeans = clustersList.First();
            int[] labels = kmeans.Learn(observations).Decide(observations);

            groupedTracks = new List<TrackFeatures>[kmeans.Clusters.Count];
            for (int i = 0; i < labels.Length; i++)
            {
                if (groupedTracks[labels[i]] == null) groupedTracks[labels[i]] = new List<TrackFeatures>();
                groupedTracks[labels[i]].Add(features.Single(x => x.ID.Equals(features[i].ID)));
            }

            return groupedTracks;
        }

        static void SaveNewPlaylists()
        {
            PostDataLogic logic = new PostDataLogic("Bearer " + token);
            for (int i = 0; i < groupedTracks.Length; i++)
            {
                Playlist newPlaylist = logic.AddNewPlaylist(username, $"API Test Playlist {i + 1}");
                List<string> trackList = groupedTracks[i].Select(x => x.URI).ToList();

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
