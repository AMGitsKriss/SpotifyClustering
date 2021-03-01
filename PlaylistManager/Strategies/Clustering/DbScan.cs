﻿using System;
using System.Collections.Generic;
using System.Linq;
using PlaylistManager.Models;

namespace PlaylistManager.Strategies.Clustering
{
    class DbScan : IClusteringStrategy
    {
        private readonly int _minimumClusterSize;
        private readonly double _maximumDistance;

        public DbScan(int minimumCluserSize, double maximumDistance)
        {
            _minimumClusterSize = minimumCluserSize;
            _maximumDistance = maximumDistance;
        }

        public List<Vector> Search(List<Vector> trackList)
        {
            int clusters = 0;
            foreach (Vector track in trackList)
            {
                if (track.Cluster != 0)
                    continue;

                List<Vector> neighbours = FindNeighbours(trackList, track);
                if (neighbours.Count(c => c.Cluster <= 0) < _minimumClusterSize)
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
                        if (neighbours.Count(c => c.Cluster <= 0) >= _minimumClusterSize)
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
                if (GetDistance(currentTrack, compareTrack) <= _maximumDistance)
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
