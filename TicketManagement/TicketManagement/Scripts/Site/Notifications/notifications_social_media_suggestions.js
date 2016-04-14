function hideSuggestion(suggestionId) {
    $('#' + suggestionId).hide();
}

function postSuggestionToFacebook(id, message_to_post) {
    $.ajax({
        type: "POST",
        url: "/Facebook/PostSuggestion",
        data: { messageToPost: message_to_post },
        datatype: "html",
        complete: function(xhr, textStatus) {
            successfulPostToSocialMedia(xhr, message_to_post);
        }
    });

    $('#social_media_suggestion_facebook_' + id).hide();
}

function postSuggestionToTwitter(id, message_to_post) {

    $.ajax({
        type: "POST",
        url: "/Twitter/PostSuggestion",
        data: { messageToPost: message_to_post },
        datatype: "html",
        complete: function (xhr, textStatus) {
            successfulPostToSocialMedia(xhr, message_to_post);
        }
    });

    $('#social_media_suggestion_twitter_' + id).hide();
}

function successfulPostToSocialMedia(xhr, messagePosted) {
    if (xhr.status === 200)
        alert('Posted to the Social Media Site: ' + messagePosted);
    else if (xhr.status === 401)
        alert('Problem posting, please check your \'External Site\' credentials.');
}

(function () {
    
    $(document).ready(function() {
        $('#modal_social_media_suggestions').modal('show');
    });

})();