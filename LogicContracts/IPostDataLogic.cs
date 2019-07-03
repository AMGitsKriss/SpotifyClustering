using DTO;
using System;
using System.Collections.Generic;

namespace LogicContracts
{
    public interface IPostDataLogic
    {
        Playlist AddNewPlaylist(string username, string playlistName);
        bool AddTrack(string playlistID, List<string> uris);
    }
}
