using DTO;
using System.Collections.Generic;

namespace Logic.Save
{
    // TODO - Pull all the save functionality out of PlaylistManager. This belongs with the spotify logic
    public interface ISaveStrategy
    {
        void SetUser(LoginSession user);
        void Save(IList<TrackFeatures> features, string playlistName);
        Playlist AddNewPlaylist(string username, string playlistName);
        bool AddTrack(string playlistID, List<string> uris);
    }
}
