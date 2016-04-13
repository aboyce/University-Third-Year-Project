function hideSuggestion(suggestionId) {
    $('#' + suggestionId).hide();
}

(function () {
    
    $(document).ready(function() {
        $('#modal_social_media_suggestions').modal('show');
    });

})();