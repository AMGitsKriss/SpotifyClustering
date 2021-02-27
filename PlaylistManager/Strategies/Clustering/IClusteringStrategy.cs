using PlaylistManager.Models;
using System.Collections.Generic;

namespace PlaylistManager.Strategies.Clustering
{
    interface IClusteringStrategy
    {
        List<Vector> Search(List<Vector> trackList);
        List<Vector> FindNeighbours(List<Vector> trackList, Vector currentTrack);
        double GetDistance(Vector trackA, Vector trackB);
    }
}
