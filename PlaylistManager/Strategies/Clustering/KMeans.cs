using PlaylistManager.Models;
using PlaylistManager.Strategies.NoiseResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using DTO;
using ACCORD = Accord.MachineLearning;

namespace PlaylistManager.Strategies.Clustering
{
    class KMeans : IClusteringStrategy
    {
        private int _minimumClusterSize;
        private double _maximumDistance;
        private INoiseResolutionStrategy _noiseResolutionStrategy;

        public IClusteringStrategy WithConfiguration(int minimumCluserSize, double maximumDistance, INoiseResolutionStrategy noiseResolutionStrategy)
        {
            WithConfiguration(minimumCluserSize, maximumDistance);
            _noiseResolutionStrategy = noiseResolutionStrategy;

            return this;
        }

        public IClusteringStrategy WithConfiguration(int minimumCluserSize, double maximumDistance)
        {
            _minimumClusterSize = minimumCluserSize;
            _maximumDistance = maximumDistance;

            return this;
        }
        public List<Vector> Search(List<Vector> trackList)
        {
            double[][] observations = trackList.Select(t => t.Features).ToArray();
            Dictionary<int, string> TrackIndexDict = new Dictionary<int, string>();

            int clusterSize = 5;

            ACCORD.KMeans kmeans = new ACCORD.KMeans(k: clusterSize);
            ACCORD.KMeansClusterCollection clusters = kmeans.Learn(observations);
            int[] labels = clusters.Decide(observations);

            for (int i = 0; i < trackList.Count(); i++)
            {
                var track = trackList[i];
                var label = labels[i];
                track.Cluster = label;
            }

            if (_noiseResolutionStrategy != null)
                trackList = _noiseResolutionStrategy.ApplyStrategy(trackList);

            return trackList;
        }
    }
}
