using DTO;
using System;
using System.Collections.Generic;

namespace LogicContracts
{
    public interface ICollectDataLogic
    {
        List<Playlist> GetPlaylists(string username);
        List<TrackSummary> GetPlaylistTracks(List<string> playlistIDs);
        List<TrackFeatures> GetTrackFeatures(List<TrackSummary> trackList);
    }
}
