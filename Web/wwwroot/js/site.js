$(document).on('click', '#fetchPlaylists', function () {
    // TODO (Kriss) - AJAX query to a stap 2 action.
    // Put the response into '#step-two'
    // remove the hidden class

    $.ajax({
        method: "GET",
        url: 'home/playlists?username=' + $('#username').val(),
        async: true,
        success: function (data) {
            $('#step-two').html(data);
            $('#step-two').removeClass('hidden');
        },
        error: function (e) {
            $('#step-two').html(e.responseText);
            $('#step-two').removeClass('hidden');
            alert("Something went horribly wrong...");
        }
    });
});