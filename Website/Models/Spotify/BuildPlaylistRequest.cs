using PlaylistManager.Strategies;
using System.Collections.Generic;

namespace Website.Models.Spotify
{
    public class BuildPlaylistRequest
    {
        public List<string> TrackIDs { get; set; }
        public int MinimumSize { get; set; }
        public double MinimumDistance { get; set; }
        public NoiseStrategy NoiseStrategy { get; set; }
    }
}
