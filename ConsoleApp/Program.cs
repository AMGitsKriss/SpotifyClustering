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
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            CollectDataLogic logic = new CollectDataLogic("Bearer BQA53bUzFyFs6Je8tXGxuQMc5Z-7SGWFgBUe80eP_kRkm0f4nMoT5YRCoXTozHx-G4Dt0S51hJNfWE-Vx5hWVmNfLqsZPzOxpmy7-FXeT1CjQEwL0b3MbJIrJOHdosULOVnMRRWWsCyV94Q8hAY");
            string username = "amgitskriss";

            List<Playlist> playlists = logic.GetPlaylists(username);
            List<string> filteredPlaylists = playlists.Where(x => x.Name.Contains("iPhone", StringComparison.InvariantCultureIgnoreCase)).Select(x => x.ID).ToList();

            List<TrackSummary> tracks = logic.GetPlaylistTracks(filteredPlaylists).ToList();

            List<TrackFeatures> features = logic.GetTrackFeatures(tracks);

            WriteCSV(features, @"C:\Users\Kriss\Desktop\spotify_features.csv");
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
                    writer.WriteLine(string.Join(", ", props.Select(p => Escape(p.GetValue(item, null).ToString()) )));
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
