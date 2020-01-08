using Accord.MachineLearning;
using DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public class Cluster
    {
        public KMeans KMeans { get; set; }
        public int Size { get; set; }
        public List<TrackFeatures>[] GroupedTracks { get; set; }
    }
}
