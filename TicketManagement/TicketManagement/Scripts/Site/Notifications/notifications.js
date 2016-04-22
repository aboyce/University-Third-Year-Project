(function () {

    function toggleAllSocialMedia(social_media_toggle_all) {
        if ($(social_media_toggle_all).prop('checked') === true) {
            $('#cb_tickets_today').bootstrapToggle('on');
            $('#cb_tickets_week').bootstrapToggle('on');
            $('#cb_tickets_month').bootstrapToggle('on');
            $('#cb_tickets_total').bootstrapToggle('on');
            $('#cb_time_today').bootstrapToggle('on');
            $('#cb_time_week').bootstrapToggle('on');
            $('#cb_time_month').bootstrapToggle('on');
            $('#cb_time_total').bootstrapToggle('on');
        } else {
            $('#cb_tickets_today').bootstrapToggle('off');
            $('#cb_tickets_week').bootstrapToggle('off');
            $('#cb_tickets_month').bootstrapToggle('off');
            $('#cb_tickets_total').bootstrapToggle('off');
            $('#cb_time_today').bootstrapToggle('off');
            $('#cb_time_week').bootstrapToggle('off');
            $('#cb_time_month').bootstrapToggle('off');
            $('#cb_time_total').bootstrapToggle('off');
        }
    }

    function toggleAllSocialMediaManagement(social_media_toggle_all) {
        if ($(social_media_toggle_all).prop('checked') === true) {
            $('#cb_user_tickets_day').bootstrapToggle('on');
            $('#cb_user_tickets_week').bootstrapToggle('on');
            $('#cb_user_tickets_month').bootstrapToggle('on');
            $('#cb_user_tickets_total').bootstrapToggle('on');

            $('#cb_user_replies_day').bootstrapToggle('on');
            $('#cb_user_replies_week').bootstrapToggle('on');
            $('#cb_user_replies_month').bootstrapToggle('on');
            $('#cb_user_replies_total').bootstrapToggle('on');

        } else {
            $('#cb_user_tickets_day').bootstrapToggle('off');
            $('#cb_user_tickets_week').bootstrapToggle('off');
            $('#cb_user_tickets_month').bootstrapToggle('off');
            $('#cb_user_tickets_total').bootstrapToggle('off');

            $('#cb_user_replies_day').bootstrapToggle('off');
            $('#cb_user_replies_week').bootstrapToggle('off');
            $('#cb_user_replies_month').bootstrapToggle('off');
            $('#cb_user_replies_total').bootstrapToggle('off');
        }
    }

    $(document).ready(function () {

        // When the user toggles the Social Media section to toggle all.
        $('#cb_social_media_toggle_all').change(function () {
            toggleAllSocialMedia(this);
        });
        $('#cb_social_media_management_toggle_all').change(function () {
            toggleAllSocialMediaManagement(this);
        });
    });

})();