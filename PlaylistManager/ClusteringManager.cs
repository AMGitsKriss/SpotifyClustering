using PlaylistManager.Models;
using PlaylistManager.Strategies.Clustering;
using PlaylistManager.Strategies.NoiseResolution;
using System.Collections.Generic;
using System.Linq;

namespace PlaylistManager
{
    public class ClusteringManager
    {
        private List<Vector> _featureVectors;
        private int _minimumClusterSize;
        private double _minimumDistance;

        public void SetDataPool(List<Vector> features)
        {
            _featureVectors = features;
        }

        public void SetConfigs(int minimumClusterSize, double minimumDistance)
        {
            _minimumClusterSize = minimumClusterSize;
            _minimumDistance = minimumDistance;
        }

        public List<Cluster> FindClusters()
        {
            // TODO - Object: ISortingConfig, minimum cluster size and maximum range should come from a config.
            IClusteringStrategy algorithm = new DbScan();
            INoiseResolutionStrategy noiseResolution = new NearestCluster();

            algorithm.WithConfiguration(_minimumClusterSize, _minimumDistance, noiseResolution);
            algorithm.Search(_featureVectors);

            int clusters = _featureVectors.Max(m => m.Cluster);

            Dictionary<int, int> clusterCount = new Dictionary<int, int>();
            for (int i = 0; i <= clusters; i++)
            {
                clusterCount.Add(i, _featureVectors.Count(t => t.Cluster == i));
            }

            List<Cluster> result = new List<Cluster>();

            for (int i = 0; i < clusters; i++)
            {
                var currentCluster = new Cluster();
                currentCluster.Features.AddRange(_featureVectors.Where(f => f.Cluster == i).ToList());
                result.Add(currentCluster);
            }
            return result;
        }
    }
}
