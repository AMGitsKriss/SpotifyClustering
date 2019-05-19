using DTO;
using System;
using System.Collections.Generic;

namespace LogicContracts
{
    public interface ICollectDataLogic
    {
        List<Playlist> GetPlaylists(string username);
        HashSet<TrackSummary> GetPlaylistTracks(List<string> playlistIDs);
    }
}
