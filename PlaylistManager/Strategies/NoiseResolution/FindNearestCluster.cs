using PlaylistManager.Models;
using PlaylistManager.Strategies.Clustering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlaylistManager.Strategies.NoiseResolution
{
    [NoiseStrategy(NoiseStrategy.FindNearestCluster)]
    public class FindNearestCluster : INoiseResolutionStrategy
    {
        public List<Vector> ApplyStrategy(List<Vector> trackList)
        {
            var clusterCenters = CalculageAllCenters(trackList);

            trackList = AssignClosestCluster(trackList, clusterCenters);

            return trackList;
        }

        private List<double[]> CalculageAllCenters(List<Vector> trackList)
        {
            var clusterCenters = new List<double[]>();
            var featureCount = trackList[0].Features.Length;

            for (int clusterNo = 0; clusterNo < trackList.Max(v => v.Cluster); clusterNo++)
            {
                clusterCenters.Add(CalculageSingleCenter(trackList.Where(t => t.Cluster == clusterNo), featureCount));
            }
            return clusterCenters;
        }

        private double[] CalculageSingleCenter(IEnumerable<Vector> currentCluster, int featureCount)
        {
            double[] currentCenter = new double[featureCount];
            for (int i = 0; i < currentCenter.Length; i++)
            {
                currentCenter[i] = currentCluster.Count() / currentCluster.Select(c => c.Features).Sum(f => f[i]);
            }
            return currentCenter;
        }

        private List<Vector> AssignClosestCluster(List<Vector> trackList, List<double[]> clusterCenters)
        {
            foreach (var track in trackList.Where(t => t.Cluster == 0))
            {
                int existingClosest = 0;
                double existingClosestDistance = double.MaxValue;

                for (int i = 1; i < clusterCenters.Count(); i++)
                {
                    var cluster = clusterCenters[i];
                    double currentDistance = GetDistance(track.Features, cluster);

                    if (currentDistance < existingClosestDistance)
                    {
                        existingClosest = i;
                        existingClosestDistance = currentDistance;
                    }
                }
                track.Cluster = existingClosest;
            }
            return trackList;
        }

        public double GetDistance(double[] track, double[] cluster)
        {
            if (cluster.Length != track.Length)
                throw new ArgumentOutOfRangeException("Vectors must have the same number of features.");

            double distance = cluster[0] - track[0];
            for (int i = 1; i < cluster.Length - 1; i++)
            {
                double dimensionADistance = Math.Abs(distance);
                double dimensionBDistance = Math.Abs(cluster[i] - track[i]);
                distance = CalculateHypotenuse(dimensionADistance, dimensionBDistance);
            }
            return distance;
        }

        public double CalculateHypotenuse(double a, double b)
        {
            return Math.Sqrt((a * a) + (b * b));
        }
    }
}
