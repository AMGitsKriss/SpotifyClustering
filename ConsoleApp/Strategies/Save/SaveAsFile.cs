using DTO;
using Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using PlaylistManager.Models;

namespace PlaylistManager.Strategies.Save
{
    public class SaveAsFile : ISaveStrategy
    {
        public void Save(IList<TrackFeatures> features, IList<Vector> groupedTracks, string username)
        {
            int clusters = groupedTracks.Max(c => c.Cluster);
            for (int i = -1; i <= clusters; i++)
            {
                IEnumerable<TrackFeatures> featureList = features.Where(f => groupedTracks.Where(t => t.Cluster == i).Select(t => t.ID).Contains(f.ID));

                if(featureList.Any())
                    WriteCSV(featureList, $@"C:\Users\Kriss\Desktop\API DbScan Playlist {i}.csv");
            }
        }

        private void WriteCSV<T>(IEnumerable<T> items, string path)
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

        private string Escape(string val)
        {
            if (val == null) return null;
            return val.Replace("\"", "\"\"");
        }
    }
}
