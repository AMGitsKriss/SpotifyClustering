using DTO;
using System;
using System.Collections.Generic;

namespace LogicContracts
{
    public interface ISpotifyLogic
    {
        LoginSession Login(string code);
        List<Playlist> GetPlaylists(string username);
        List<TrackSummary> GetPlaylistTracks(List<string> playlistIDs);
        List<TrackFeatures> GetTrackFeatures(List<TrackSummary> trackList);
        Playlist AddNewPlaylist(string username, string playlistName);
        bool AddTrack(string playlistID, List<string> uris);
    }
}
