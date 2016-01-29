(function () {

    $('#fb_profile_summary_form').submit();
    $('#fb_admin_page_form').submit();
    $('#fb_admin_page_posts_form').submit();

    var allowLoadMore = true;
    var displayedPostCount = 0;

    $(document).ready(function () {

        $('#fb_posts').scroll(function () {

            var scrollingDiv = $('#fb_posts');
            buffer = 60;

            if (scrollingDiv.prop('scrollHeight') - scrollingDiv.scrollTop() <= scrollingDiv.height() + buffer) {
                var nextPageUri = $('#fb_posts_next_page_link').val();

                if (nextPageUri != null && nextPageUri != "") {
                    getMoreResults(nextPageUri);
                } else {
                    allowLoadMore = false;
                }
            }
        });
    });

    function getMoreResults(nextPageUri) {

        if (allowLoadMore) {
            allowLoadMore = false;
            $('#fb_posts_loading_search').show();

            $.ajax({
                type: "POST",
                url: "Facebook/GetMorePagePosts",
                data: { nextPageUri: nextPageUri },
                cache: false,
                success: function (result) {
                    if (result != null && result != "") {
                        $('#fb_posts_loading_search').hide();
                        $('#fb_posts_next_page_link').remove();
                        $('#fb_posts_current_data_count').remove();
                        $('#fb_posts_load_more').append(result);
                        completeLoadingMore();
                    }
                    allowLoadMore = true;
                },
                error: function (result) {
                    allowLoadMore = false;
                }
            });
        }
    }

    function completeLoadingMore() {
        allowLoadMore = true;
        displayedPostCount = displayedPostCount + parseInt($('#fb_posts_current_data_count').val());
        $('#fb_posts_count').html("Showing " + displayedPostCount + " items");
    }

})();