using System;
using System.Collections.Generic;
using System.Linq;
using PlaylistManager.Models;

namespace PlaylistManager.Strategies.Clustering
{
    class DbScan : IClusteringStrategy
    {
        public int MinimumClusterSize { get; }
        public double RequiredRange { get; }

        public DbScan(int minimumCluserSize, double requiredRange)
        {
            MinimumClusterSize = minimumCluserSize;
            RequiredRange = requiredRange;
        }

        public List<Vector> Search(List<Vector> trackList)
        {
            int clusters = 0;
            foreach (Vector track in trackList)
            {
                if (track.Cluster != 0)
                    continue;

                List<Vector> neighbours = FindNeighbours(trackList, track);
                if (neighbours.Count(c => c.Cluster <= 0) < MinimumClusterSize)
                {
                    track.Cluster = -1;
                    continue;
                }
                clusters++;
                track.Cluster = clusters;
                List<Vector> seedTracks = neighbours.Where(n => n.ID != track.ID).ToList();
                foreach (Vector neighbour in seedTracks.ToList())
                {
                    if (neighbour.Cluster <= 0)
                    {
                        neighbour.Cluster = clusters;
                        List<Vector> newNeighbours = FindNeighbours(trackList, neighbour);
                        if (neighbours.Count(c => c.Cluster <= 0) >= MinimumClusterSize)
                            seedTracks.AddRange(newNeighbours);
                    }
                }
            }

            return trackList;
        }

        public List<Vector> FindNeighbours(List<Vector> trackList, Vector currentTrack)
        {
            List<Vector> neighbours = new List<Vector>();
            foreach (var compareTrack in trackList)
            {
                if (GetDistance(currentTrack, compareTrack) <= RequiredRange)
                {
                    neighbours.Add(compareTrack);
                }
            }
            return neighbours;
        }

        public double GetDistance(Vector trackA, Vector trackB)
        {
            if (trackA.Features.Length != trackB.Features.Length)
                throw new ArgumentOutOfRangeException("Vectors must have the same number of features.");

            double distance = trackA.Features[0] - trackB.Features[0];
            for (int i = 1; i < trackA.Features.Length - 1; i++)
            {
                double dimensionADistance = Math.Abs(distance);
                double dimensionBDistance = Math.Abs(trackA.Features[i] - trackB.Features[i]);
                distance = GetHypotenuse(dimensionADistance, dimensionBDistance);
            }
            return distance;
        }

        public double GetHypotenuse(double a, double b)
        {
            // TODO - Make into a Math.Hypotenuse extension method
            return Math.Sqrt((a * a) + (b * b));
        }
    }
}
