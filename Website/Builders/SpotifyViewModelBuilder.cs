using Logic;
using Website.Models.Spotify;

namespace Website.Builders
{
    public static class SpotifyViewModelBuilder
    {
        public static SpotifyViewModel Build()
        {
            SpotifyViewModel result = new SpotifyViewModel();

            //SpotifyLogic logic = new SpotifyLogic();
            //result.Playlists = logic.GetPlaylists("");

            return result;
        }
    }
}
