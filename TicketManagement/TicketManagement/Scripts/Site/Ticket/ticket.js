(function () {
    //https://github.com/eternicode/bootstrap-datepicker

    $('.datepicker').datepicker({
        format: "dd/mm/yyyy",
        clearBtn: true,
        autoclose: true,
        todayHighlight: true,
        weekStart: 1,
        daysOfWeekDisabled: "0,6",
        daysOfWeekHighlighted: "1,2,3,4,5"
    });
})();