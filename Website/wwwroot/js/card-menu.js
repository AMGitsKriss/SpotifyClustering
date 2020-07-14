var CardMenu = (function () {

    function bindEvents() {
        $(document).on('click', '.widget-list-item', loadCardContent);
    }

    function loadCardContent() {
        var selector = this;
        var id = $(this).attr('data-post-id');
        var url = $(this).attr('data-post-action') + id;
        // Ajax query for the partial
        $.ajax({
            method: "GET",
            url: url,
            success: function (result) {
                $('#card-menu-content').html(result);
                $('.widget-list-item').removeClass('active');
                $(selector).addClass('active');
                window.history.pushState({}, "TODO", "/PointOfInterest/?id="+id);
            },
            error: function () {
                alert('Nope.');
            }
        });
    }

    return {
        init: function () {
            bindEvents();
        }
    }
})();