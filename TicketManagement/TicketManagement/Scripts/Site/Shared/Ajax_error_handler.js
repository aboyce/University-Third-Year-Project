(function () {

    function handleError(ajaxContext, updateTargetId) {
        if (ajaxContext != null && ajaxContext.responseText != null) {
            $("#" + updateTargetId).html(ajaxContext.responseType);
        }
    }

})();