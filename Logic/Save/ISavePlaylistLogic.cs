using DTO;
using System.Collections.Generic;

namespace Logic.Save
{
    public interface ISavePlaylistLogic
    {
        void SetUser(LoginSession user);
        void Save(IList<TrackFeatures> features, string playlistName);
        Playlist AddNewPlaylist(string username, string playlistName);
        bool AddTrack(string playlistID, List<string> uris);
    }
}
