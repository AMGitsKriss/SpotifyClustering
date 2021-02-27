using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models.Spotify
{
    public class GetTracksViewModel
    {
        public List<TrackSummary> Tracks { get; internal set; }
    }
}
