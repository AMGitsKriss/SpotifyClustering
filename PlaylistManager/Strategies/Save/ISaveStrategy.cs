using DTO;
using System.Collections.Generic;
using PlaylistManager.Models;

namespace PlaylistManager.Strategies.Save
{
    public interface ISaveStrategy
    {
        void Save(IList<TrackFeatures> features, IList<Vector> groupedTracks, string username);
    }
}
