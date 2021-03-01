using PlaylistManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace PlaylistManager.Strategies.NoiseResolution
{
    [NoiseStrategy(NoiseStrategy.Discard)]
    public class Discard : INoiseResolutionStrategy
    {
        public List<Vector> ApplyStrategy(List<Vector> trackList)
        {
            trackList = trackList.Where(t => t.Cluster != 0).ToList();

            return trackList;
        }
    }
}
