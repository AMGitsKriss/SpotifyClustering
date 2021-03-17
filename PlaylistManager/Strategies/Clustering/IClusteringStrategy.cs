using PlaylistManager.Models;
using PlaylistManager.Strategies.NoiseResolution;
using System.Collections.Generic;

namespace PlaylistManager.Strategies.Clustering
{
    interface IClusteringStrategy
    {
        IClusteringStrategy WithConfiguration(int minimumCluserSize, double maximumDistance);
        IClusteringStrategy WithConfiguration(int minimumCluserSize, double maximumDistance, INoiseResolutionStrategy noiseResolutionStrategy);
        List<Vector> Search(List<Vector> trackList);
    }
}
