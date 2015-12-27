(function () {

    function getUrlParameter(parameter)
    {
        var pageUrl = window.location.search.substring(1);
        var urlVariables = pageUrl.split('&');

        for (var i = 0; i < urlVariables.length; i++)
        {
            var parameterName = urlVariables[i].split('=');

            if (parameterName[0] == parameter)
            {
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

    function toggleSendButton() {
        if ($('#data').val() === "")
            $('#btn-message-send').attr('disabled', 'disabled');
        else
            $('#btn-message-send').attr("disabled", false);
    }

    function toggleUploadButton() {
        if ($('#file').val() === "")
            $('#btn-message-upload').attr('disabled', 'disabled');
        else
            $('#btn-message-upload').attr("disabled", false);
    }

    $(document).ready(function() {

        makeCurrentSortTypeTabActive();
        toggleSendButton();
        toggleUploadButton();

        // When one from the list of tickets is clicked on, take it to the correct page.
        $('.clickable-row').click(function() {
            window.document.location = $(this).data("url");
        });

        // Check that there is text entered in the 'TicketLog - Message' input box, to enable/disable the 'Send' button.
        $('#data').keyup(function () {
            toggleSendButton();
        });

        // Check that there is a file to be uploaded in the 'TicketLog - File' input, to enable/disable the 'Upload' button.
        $('#upload').change(function () {
            toggleUploadButton();
        });

    });

})();