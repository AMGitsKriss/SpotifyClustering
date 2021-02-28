using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models.Spotify
{
    public class BuildPlaylistsViewModel
    {
        public List<PlaylistViewModel> Playlists { get; set; }
    }

    public class PlaylistViewModel
    {
        public Playlist Playlist { get; set; }
        public List<TrackFeatures> TrackFeatures { get; set; }
        public List<string[]> FeatureVectors { get; set; }
    }
}
