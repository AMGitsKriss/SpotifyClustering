﻿var Spotify = (function () {

    function bindEvents() {
        $(document).on('click', '#get-playlists', getPlaylists);
        $(document).on('click', '#get-tracks', getTracks);
        $(document).on('click', '#build-playlists', buildPlaylists);
        $(document).on('click', '#save-playlists', savePlaylists);
        $(document).on('change', 'input[type=range]', distanceRangeSlider);
        $(document).on('change', 'input[type=range]', sizeRangeSlider);
        $(document).on('change', '#step-playlists input[type=checkbox]', recordCheckboxPlaylist);
        $(document).on('change', '#step-tracks input[type=checkbox]', recordCheckboxTrack);
    }

    function recordCheckboxPlaylist() {
        var selected = $(this).is(":checked") == true ? 1 : 0;
        gtag('send', 'event', 'GetPlaylists', 'checkbox', 'checked', selected);
    }

    function recordCheckboxTrack() {
        var selected = $(this).is(":checked") == true ? 1 : 0;
        gtag('send', 'event', 'GetTracks', 'checkbox', 'checked', selected);
    }

    function getPlaylists() {
        $.ajax({
            type: "GET",
            url: "Spotify/GetPlaylists",
            success: function (result) {
                $('#step-playlists').html(result);
                gtag('send', 'event', 'GetPlaylists', 'click', 'Success');
            },
            error: function () {
                gtag('send', 'event', 'GetPlaylists', 'click', 'Fail');
                alert('Something broke. Bother Kriss');
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
                gtag('send', 'event', 'GetTracks', 'click', 'Success');
            },
            error: function () {
                gtag('send', 'event', 'GetTracks', 'click', 'Fail');
                alert('Something broke. Bother Kriss');
            }
        });
    }

    function buildPlaylists() {
        var tracks = {
            trackIDs: [],
            minimumSize: $('#minimumSize').val(),
            minimumDistance: $('#minimumDistance').val() / 100,
            noiseStrategy: $('#noiseStrategy').val()
        };

        $('#tracks input:checked').each(function () {
            tracks.trackIDs.push($(this).val());
        });

        $.ajax({
            type: "POST",
            url: "Spotify/BuildPlaylists",
            data: tracks,
            success: function (result) {
                $('#step-build').html(result);
                gtag('send', 'event', 'BuildPlaylists', 'click', 'Success');
            },
            error: function () {
                gtag('send', 'event', 'BuildPlaylists', 'click', 'Fail');
                alert('Something broke. Bother Kriss');
            }
        });
    }

    function savePlaylists() {
        var data = {};
        $('.playlist-name').each(function () {
            var id = $(this).attr('data-id');
            var name = $(this).val();
            data[id] = name;
        });


        $.ajax({
            type: "POST",
            url: "Spotify/SavePlaylists",
            data: data,
            success: function (result) {
                $('#step-save').html(result);
                gtag('send', 'event', 'SavePlaylists', 'click', 'Success');
            },
            error: function () {
                gtag('send', 'event', 'SavePlaylists', 'click', 'Success');
                alert('Something broke. Bother Kriss');
            }
        });
    }

    function initClusteringConfig() {
        distanceRangeSlider();
        sizeRangeSlider();
    }

    function distanceRangeSlider() {
        var value = $('#minimumDistance').val();
        $('span[for=minimumDistance]').html(value / 100);
    }

    function sizeRangeSlider() {
        var value = $('#minimumSize').val();
        $('span[for=minimumSize]').html(value);
    }

    return {
        init: function () {
            bindEvents();
        },
        initClusteringConfig: function () {
            initClusteringConfig();
        }
    };
})();