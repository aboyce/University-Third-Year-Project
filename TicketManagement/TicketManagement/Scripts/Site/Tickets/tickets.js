(function () {

    $(document).ready(function () {
        $('[data-toggle="popover"]').hover(
            function () {
                $(this).popover({
                    container: 'body',
                    html: true,
                    content: function () {
                        var content = $(this).data('content-target');
                        try { content = $(content).html() } catch (e) {/* Ignore */ }
                        return content || $(this).data('content');
                    }
                }).popover('show');
            }, function () {
                $(this).popover('hide');
            });
    });
})();
