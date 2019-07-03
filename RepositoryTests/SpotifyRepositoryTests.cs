using DTO;
using NUnit.Framework;
using RepositoryContracts;
using Repsoitory;
using System;
using System.Collections.Generic;

namespace RepositoryTests
{
    [TestFixture]
    public class SpotifyRepositoryTests
    {
        string api = "Bearer BQCoYqLwMlcwG6AnAtLGRCrS7Q3ws7YO9x9usxHfh7YuuW8gLf9r1hx29CqbO94k593hAEHK6PE6pCtjFoSR1XzwyeoSONYFklvZo1E0RPazMr5liJOhnFpehW08JvRnj9UpTyx-xslD_fO4XM0ykAocMPieNwBvFor1oYx142XyoQzhvpIg0BmgsLdgRQgzOOZ2BvoTAukmdVErvCrKt-HU";
        [Test]
        public void GetPlaylistsTest()
        {
            ISpotifyRepository context = new SpotifyRepository(api);
            var results = context.GetPlaylists("amgitskriss");
            Assert.IsTrue(results != null);
        }

        [Test]
        public void AddPlaylistTest()
        {
            ISpotifyRepository context = new SpotifyRepository(api);
            BasePlaylist request = new BasePlaylist()
            {
                Name = "API PlayList Test"
            };
            var results = context.AddNewPlaylist("amgitskriss", request);
            Assert.IsTrue(results != null);
        }

        [Test]
        public void AddTrackTest()
        {
            AddTrackRequest request = new AddTrackRequest() { uris = new List<string>() { "spotify:track:6VnLn1tg0c8H8QpwPADBaz" } };
            string playlistID = "3Ugg7L6E5FdoLXRqTaGWHA";
            ISpotifyRepository context = new SpotifyRepository(api);

            var results = context.AddTrack(playlistID, request);
            Assert.IsTrue(results != null);
        }
    }
}
