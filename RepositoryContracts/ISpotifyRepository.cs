﻿using DTO;
using System;
using System.Collections.Generic;

namespace RepositoryContracts
{
    public interface ISpotifyRepository
    {
        List<Playlist> GetPlaylists(string username, int offset);
        List<TrackSummary> GetTrackList(string playlistID, int offset);
        List<TrackFeatures> GetTrackFeatures(List<string> trackIDs);
        Playlist AddNewPlaylist(string username, BasePlaylist request);
        bool AddTrack(string playlistID, AddTrackRequest request);
    }
}
