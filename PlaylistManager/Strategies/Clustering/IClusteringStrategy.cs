using PlaylistManager.Models;
using PlaylistManager.Strategies.NoiseResolution;
using System.Collections.Generic;

namespace PlaylistManager.Strategies.Clustering
{
    interface IClusteringStrategy
    {
        IClusteringStrategy WithConfiguration(int minimumCluserSize, double maximumDistance);
        IClusteringStrategy WithConfiguration(int minimumCluserSize, double maximumDistance, INoiseResolutionStrategy noiseResolutionStrategy);
        void IncrementSearchDistance(double increment);
        List<Vector> Search(List<Vector> trackList);
        List<Vector> FindNeighbours(List<Vector> trackList, Vector currentTrack);
        double GetDistance(Vector trackA, Vector trackB);
    }
}
