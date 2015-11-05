(function () {
    $(window).load(function () {

        var _window = $(window),
            $bg = $("#bg_image"),
            aspectRatio = $bg.width() / $bg.height();

        function resize_bg_image() {

            if ((_window.width() / _window.height()) < aspectRatio) {

                $bg.removeClass().addClass('bg_image_height');

            } else {

                $bg.removeClass().addClass('bg_image_width');
            }

        }

        _window.resize(resize_bg_image).trigger("resize");

    });
})();



