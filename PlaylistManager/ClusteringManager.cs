using PlaylistManager.Factories;
using PlaylistManager.Models;
using PlaylistManager.Strategies.Clustering;
using DTO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PlaylistManager
{
    public class ClusteringManager
    {
        private List<Vector> _featureVectors;
        private int _minimumClusterSize;
        private double _minimumDistance;

        public void SetDataPool(List<TrackFeatures> features)
        {
            _featureVectors = new List<Vector>();
            _featureVectors.AddRange(features.Select(f => new Vector() { ID = f.ID, Features = FeatureFactory.BuildVector(f) }));
        }

        public void SetConfigs(int minimumClusterSize, double minimumDistance)
        {
            _minimumClusterSize = minimumClusterSize;
            _minimumDistance = minimumDistance;
        }

        public List<Cluster> FindClusters()
        {
            // TODO - Object: ISortingConfig, minimum cluster size and maximum range should come from a config.
            IClusteringStrategy algorithm = new DbScan(_minimumClusterSize, _minimumDistance);
            algorithm.Search(_featureVectors);

            int clusters = _featureVectors.Max(m => m.Cluster);

            Dictionary<int, int> clusterCount = new Dictionary<int, int>();
            for (int i = -1; i <= clusters; i++)
            {
                clusterCount.Add(i, _featureVectors.Count(t => t.Cluster == i));
            }

            List<Cluster> result = new List<Cluster>();
            result.Add(new Cluster() { 
                Features = _featureVectors.Where(f => f.Cluster == -1).ToList()
            });

            for (int i = 0; i < clusters; i++)
            {
                var currentCluster = new Cluster();
                currentCluster.Features.AddRange(_featureVectors.Where(f => f.Cluster == i + 1).ToList());
                result.Add(currentCluster);
            }
            return result;
        }
    }
}
