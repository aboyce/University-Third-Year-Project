(function () {

    function getUrlParameter(parameter) {
        var pageUrl = window.location.search.substring(1);
        var urlVariables = pageUrl.split('&');

        for (var i = 0; i < urlVariables.length; i++) {
            var parameterName = urlVariables[i].split('=');

            if (parameterName[0] == parameter) {
                return parameterName[1];
            }
        }

        return null;
    }

    function makeCurrentSortTypeTabActive() {

        var sortType = getUrlParameter("sort");

        if (sortType == null) // If null there is no sorting so it defaults to 'All'
            $('#All').parent('li').addClass("active");

        else // If not null there is a sorting parameter
        {
            $('#ticketSortTabs').each(function () // Go through each of the Li items
            {
                $(this).find('li').each(function () // Go through each of the a items
                {
                    var current = $(this);

                    if (current.find('a').attr('id') == sortType)
                        current.addClass("active");
                });
            });
        }
    }

    function toggleSmsButton(show) {
        if (show) {
            $("label:contains('SMS')").parent().parent().show();
        } else {
            //$('#cb-send-sms').bootstrapToggle('off');
            $('#cb-send-sms').bootstrapToggle('off');
            $("label:contains('SMS')").parent().parent().hide();
        }
    }

    function toggleSendButton() {
        if ($('#message').val() === "")
            $('#btn-message-send').attr('disabled', 'disabled');
        else
            $('#btn-message-send').attr("disabled", false);
    }

    function updateNewResponsePanel(cbInternalExternal) {
        if ($(cbInternalExternal).prop('checked') === true) {
            $('#new-response-panel-heading').text("New Response");
            $('#new-response-panel').removeClass('panel-default');
            $('#new-response-panel').addClass('panel-success');
            toggleSmsButton(true);
        }
        else {
            $('#new-response-panel-heading').text("New Internal Response");
            $('#new-response-panel').removeClass('panel-success');
            $('#new-response-panel').addClass('panel-default');
            toggleSmsButton(false);
        }
}

    $(document).ready(function () {

        makeCurrentSortTypeTabActive();

        toggleSendButton();
        toggleSmsButton(false);

        // When one from the list of tickets is clicked on, take it to the correct page.
        $('.clickable-row').click(function () {
            window.document.location = $(this).data("url");
        });

        // Check that there is text entered in the 'TicketLog - Message' input box, to enable/disable the 'Send' button.
        $('#message').keyup(function () {
            toggleSendButton();
        });

        // When the user toggles the message to be internal or external, change the UI as required.
        $('#cb-internal-external').change(function () {
            updateNewResponsePanel(this);
        });

    });

})();