var Site = (function () {

    var selector = {
        Section: {
            Top: '.panel-container'
        },
        Endpoint: {
            Edit: '/PointOfInterest/EditPostSection/',
            View: '/PointOfInterest/GetPostSection/',
            Save: '/PointOfInterest/SavePostSection/'
        }
    }

    function bindEvents() {
        // TODO - Get edit window on edit
        $(document).on('click', '.panel-heading-btn #edit', { Endpoint: selector.Endpoint.Edit }, getPanel);
        $(document).on('click', '#discard', { Endpoint: selector.Endpoint.View }, getPanel);
        $(document).on('click', '#save', { Endpoint: selector.Endpoint.Save }, postPanel);
        // TODO - Get panel window on eddismissit
        // TODO - Get save and get panel window on save
        // TODO - Delete panel (prompt to confirm)
    }

    function getPanel(e) {
        var panel = $(this).closest(selector.Section.Top)
        var id = panel.attr('data-section-id');
        $.ajax({
            method: 'GET',
            url: e.data.Endpoint + id,
            success: function (result) {
                panel.replaceWith(result);
            },
            error: function () {
                alert('There was an error getting the editor');
            }
        });
    }

    function postPanel(e) {
        var panel = $(this).closest(selector.Section.Top)
        var id = panel.attr('data-section-id');
        var data = { id, content: $('[data-section-id=' + id + '] [name=content]').val(), heading: $('[data-section-id=' + id + '] [name=heading]').val() };
        $.ajax({
            method: 'POST',
            data: $(panel).serialize(),
            url: e.data.Endpoint + id,
            success: function (result) {
                panel.replaceWith(result);
            },
            error: function () {
                alert('There was an error getting the editor');
            }
        });
    }


    return {
        init: function () {
            bindEvents();
        }
    }
})();