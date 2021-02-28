using PlaylistManager.Factories;
using PlaylistManager.Models;
using PlaylistManager.Strategies.Clustering;
using DTO;
using System.Collections.Generic;
using System.Linq;

namespace PlaylistManager
{
    // TODO - PlaylistManager should become Clustering
    public class ClusteringManager
    {
        private List<Vector> _featureVectors;

        // TODO - Object: ISortingConfig, minimum cluster size and maximum range should come from a config.

        public void SetDataPool(List<TrackFeatures> features)
        {
            _featureVectors = new List<Vector>();
            _featureVectors.AddRange(features.Select(f => new Vector() { ID = f.ID, Features = FeatureFactory.BuildVector(f) }));
        }

        public List<Cluster> FindClusters()
        {

            IClusteringStrategy algorithm = new DbScan(10, 0.55);
            algorithm.Search(_featureVectors);

            int clusters = _featureVectors.Max(m => m.Cluster);

            Dictionary<int, int> clusterCount = new Dictionary<int, int>();
            for (int i = -1; i <= clusters; i++)
            {
                clusterCount.Add(i, _featureVectors.Count(t => t.Cluster == i));
            }

            List<Cluster> result = new List<Cluster>();
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
