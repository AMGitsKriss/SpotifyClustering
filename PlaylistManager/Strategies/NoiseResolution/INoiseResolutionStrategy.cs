using PlaylistManager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlaylistManager.Strategies.NoiseResolution
{
    public interface INoiseResolutionStrategy
    {
        List<Vector> ApplyStrategy(List<Vector> trackList);
    }
}
