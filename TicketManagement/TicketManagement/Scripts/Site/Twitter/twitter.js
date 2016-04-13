(function () {
    $('#twitter_profile_summary_form').submit();
    $('#twitter_user_timeline_form').submit();

    $('#modal_twitter_reply').on('show.bs.modal', function(e) {
        var model = $(this), tweetId = e.relatedTarget.id;
        model.find('.edit-content').html(tweetId);
    });

    $(document).ready(function () {

        // When the user toggles the message to be internal or external, change the UI as required.
        var checkbox = $('#cb_load_home_timeline');
        checkbox.change(function () {
            if (checkbox.prop('checked') === true) {
                $('#twitter_home_timeline_form').submit();
            } else {
                $('#twitter_home_timeline').empty();
            }
        });
    });

})();