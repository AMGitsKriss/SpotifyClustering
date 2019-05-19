using NUnit.Framework;
using RepositoryContracts;
using Repsoitory;
using System;

namespace RepositoryTests
{
    [TestFixture]
    public class SpotifyRepositoryTests
    {
        [Test]
        public void GetPlaylistsTest()
        {
            string api = "Bearer BQBDPZ9yUU_cCTVS2VohSvtzNaJ396rx6FxTKRCqKSB_7M7hH7jmpzLPqiQ4sdI_blf9gvKJ992uifBGzagOihsq0SndvbBikLPf7Hbkl3uJjdGuS8yZ1lz6bz845IBXY2PS7tinm6vHR76Ms0A";
            ISpotifyRepository context = new SpotifyRepository(api);
            var results = context.GetPlaylists("amgitskriss");
            Assert.IsTrue(results != null);
        }
    }
}
