using PlaylistManager.Models;
using System.Collections.Generic;

namespace PlaylistManager.Strategies.NoiseResolution
{
    [NoiseStrategy(NoiseStrategy.OwnPlaylist)]
    public class OwnPlaylist : INoiseResolutionStrategy
    {
        public List<Vector> ApplyStrategy(List<Vector> trackList)
        {
            return trackList;
        }
    }
}
