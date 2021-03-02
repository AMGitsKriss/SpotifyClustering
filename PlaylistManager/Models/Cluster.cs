using System.Collections.Generic;

namespace PlaylistManager.Models
{
    public class Cluster
    {
        public int ClusterID { get; set; }
        /// <summary>
        /// A collection of features belonging to a single group
        /// </summary>
        public List<Vector> Features { get; set; } = new List<Vector>();
    }
}