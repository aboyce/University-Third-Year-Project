(function () {

    $(document).ready(function() {

        $('#cb-enable-populate-data').change(function () {
            if ($(this).prop('checked') === true)
                $('#btn-populate-data').attr('disabled', false);
            else
                $('#btn-populate-data').attr('disabled', 'disabled');
        });
    });

})();