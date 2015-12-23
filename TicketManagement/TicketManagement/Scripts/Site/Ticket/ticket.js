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

    $(document).ready(function () {
        makeCurrentSortTypeTabActive();
    });

})();