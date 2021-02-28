using System;
using System.Collections.Generic;
using System.Text;

namespace PlaylistManager.Models
{
    public class Cluster
    {
        /// <summary>
        /// A collection of features belonging to a single group
        /// </summary>
        public List<Vector> Features { get; set; } = new List<Vector>();
    }
}