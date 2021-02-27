var Spotify = (function () {

    function bindEvents() {
        $(document).on('click', '#get-playlists', getPlaylists);
        $(document).on('click', '#get-tracks', getTracks);
    }

    function getPlaylists() {
        $.ajax({
            type: "GET",
            url: "Spotify/GetPlaylists",
            success: function (result) {
                $('#step-playlists').html(result);
            }
        });
    }

    function getTracks() {
        var playlists = { playlistIDs: [] };

        $('#playlists input:checked').each(function () {
            playlists.playlistIDs.push($(this).val());
        });

        $.ajax({
            type: "POST",
            url: "Spotify/GetTracks",
            data: playlists,
            success: function (result) {
                $('#step-tracks').html(result);
            }
        });
    }

    function test() {
        alert();
    }

    return {
        init: function () {
            bindEvents();
        }
    }
})();