using DTO;
using System;
using System.Collections.Generic;

namespace RepositoryContracts
{
    public interface ISpotifyRepository
    {
        List<Playlist> GetPlaylists(string username);
        List<TrackSummary> GetTrackList(string playlistID);
        List<TrackFeatures> GetTrackFeatures(List<string> trackIDs);
    }
}
