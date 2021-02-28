﻿var Spotify = (function () {

    function bindEvents() {
        $(document).on('click', '#get-playlists', getPlaylists);
        $(document).on('click', '#get-tracks', getTracks);
        $(document).on('click', '#build-playlists', buildPlaylists);
        $(document).on('click', '#save-playlists', savePlaylists);
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

    function buildPlaylists() {
        var tracks = { trackIDs: [] };

        $('#tracks input:checked').each(function () {
            tracks.trackIDs.push($(this).val());
        });

        $.ajax({
            type: "POST",
            url: "Spotify/BuildPlaylists",
            data: tracks,
            success: function (result) {
                $('#step-build').html(result);
            }
        });
    }

    function savePlaylists() {
        $.ajax({
            type: "POST",
            url: "Spotify/SavePlaylists",
            success: function (result) {
                $('#step-save').html(result);
            }
        });
    }

    return {
        init: function () {
            bindEvents();
        }
    }
})();