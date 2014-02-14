/*
* BookItems aircraft booking system
* By Craig Miller
* 2010
*/

var _userId;
var _bookedNodes;
var _hasStartedBooking;
var _isReserve

// Booking start date/time
var _bookingStartYear;
var _bookingStartMonth;
var _bookingStartDate;
var _bookingStartHour;

// Booking end date/time
var _bookingEndYear;
var _bookingEndMonth;
var _bookingEndDate;
var _bookingEndHour;

// Delete dates
var _bookingDeleteYear;
var _bookingDeleteMonth;
var _bookingDeleteDate;
var _bookingDeleteReserve;

function setup() {
    _bookedNodes = new Array();
    _hasStartedBooking = false;
}

function focusBookableSegment(segment) {
    segment.style.outline = "Yellow solid 3px";
}

function blurBookableSegment(segment) {
    segment.style.outline = "#abace1 solid 0px";
}

function clickBookableSegment(segment, year, month, date, hour) {
    _isReserve = segment.id.endsWith(".2");
    
    if (!_hasStartedBooking) {
        _bookingStartYear = year;
        _bookingStartMonth = month;
        _bookingStartDate = date;
        _bookingStartHour = hour;

        segment.style.backgroundColor = "#33af4c";

        //segment.innerHTML = _userId;
    } else {
        _bookingEndYear = year;
        _bookingEndMonth = month;
        _bookingEndDate = date;
        _bookingEndHour = hour;

        //segment.innerHTML = _userId;

        if (_bookingEndHour < _bookingStartHour) {
            var temp = _bookingStartHour;
            _bookingStartHour = _bookingEndHour;
            _bookingEndHour = temp;
        }

        for (var i = _bookingStartHour; i <= _bookingEndHour; i++) {
            var seg = document.getElementById("" + _bookingStartYear + padleft(_bookingStartMonth, "0", 2) + padleft(_bookingStartDate, "0", 2) + padleft(i, "0", 2) + (_isReserve ? ".2" : ""));
            seg.style.backgroundColor = "#b5cefc";
        }

        $("#choosePilotDilogButtonOK").removeAttr("disabled");
        $("#choosePilotDilogButtonCancel").removeAttr("disabled");
        jQuery('#choosePilotDialog').dialog('open');
    }

    _hasStartedBooking = !_hasStartedBooking;
}

function confirmOK() {
    var select = document.getElementById("userChoice");

    $("#choosePilotDilogButtonOK").attr("disabled", "disabled");
    $("#choosePilotDilogButtonCancel").attr("disabled", "disabled");

    saveBooking(select.value);
}

function confirmCancel() {
    jQuery('#choosePilotDialog').dialog('close');

    $.ajax({
        type: "POST",
        url: "BookingHandler.ashx",
        data: "type=" + (_isReserve ? "2" : "1") + "&year=" + _bookingStartYear + "&month=" + _bookingStartMonth + "&date=" + _bookingStartDate + "&startHour=" + _bookingStartHour + "&endHour=" + _bookingEndHour,
        success: function (msg) {
            var bookingDay = document.getElementById("bookingday" + _bookingStartYear + padleft(_bookingStartMonth, "0", 2) + padleft(_bookingStartDate, "0", 2));

            var result = eval(msg);

            bookingDay.innerHTML = result.html;
        }
    });
}

function saveBooking(userId) {
    $.ajax({
        type: "POST",
        url: "BookingHandler.ashx",
        data: "type=" + (_isReserve ? "2" : "1") + "&userId=" + userId + "&year=" + _bookingStartYear + "&month=" + _bookingStartMonth + "&date=" + _bookingStartDate + "&startHour=" + _bookingStartHour + "&endHour=" + _bookingEndHour,
        success: function (msg) {
            jQuery('#choosePilotDialog').dialog('close');

            var bookingDay = document.getElementById("bookingday" + _bookingStartYear + padleft(_bookingStartMonth, "0", 2) + padleft(_bookingStartDate, "0", 2));

            var result = eval(msg);

            bookingDay.innerHTML = result.html;

            if (result.errorMessage != "")
                alert(result.errorMessage);
        }
    });
}

function showDeleteBookingDialog(isReserve, year, month, date) {
    _bookingDeleteYear = year;
    _bookingDeleteMonth = month;
    _bookingDeleteDate = date;
    _bookingDeleteReserve = isReserve;
    
    jQuery('#deleteBookingDialog').dialog('open');
}

function deleteBookingDialogConfirmCancel() {
    jQuery('#deleteBookingDialog').dialog('close');
}

function deleteBookingForUser(userIdToDelete) {
    $.ajax({
        type: "POST",
        url: "BookingHandler.ashx",
        data: "deletemode=usertoday&type=" + (_bookingDeleteReserve ? "2" : "1") + "&userId=" + userIdToDelete + "&year=" + _bookingDeleteYear + "&month=" + _bookingDeleteMonth + "&date=" + _bookingDeleteDate,
        success: function (msg) {
            jQuery('#deleteBookingDialog').dialog('close');

            var bookingDay = document.getElementById("bookingday" + _bookingDeleteYear + padleft(_bookingDeleteMonth, "0", 2) + padleft(_bookingDeleteDate, "0", 2));

            var result = eval(msg);

            bookingDay.innerHTML = result.html;

            if (result.errorMessage != "")
                alert(result.errorMessage);
        }
    });
}

function enterDeleteMode() {
    $.ajax({
        type: "POST",
        url: "BookingHandler.ashx",
        data: "deletemode=true&type=" + (_bookingDeleteReserve ? "2" : "1") + "&year=" + _bookingDeleteYear + "&month=" + _bookingDeleteMonth + "&date=" + _bookingDeleteDate,
        success: function (msg) {
            jQuery('#deleteBookingDialog').dialog('close');

            var bookingDay = document.getElementById("bookingday" + _bookingDeleteYear + padleft(_bookingDeleteMonth, "0", 2) + padleft(_bookingDeleteDate, "0", 2));

            var result = eval(msg);

            bookingDay.innerHTML = result.html;

            if (result.errorMessage != "")
                alert(result.errorMessage);
        }
    });
}

function deleteBookingHour(year, month, date, hour, isReserve, fromEnd) {
    
}

/*************** Date changing *****************/

function selectBookingPeriod(dateText, inst) {
    var date = $('#datepicker').datepicker("getDate");

    $.ajax({
        type: "POST",
        url: "BookingPeriodHandler.ashx",
        data: "year=" + date.getFullYear() + "&month=" + (date.getMonth() + 1) + "&date=" + date.getDate(),
        success: function (msg) {
            var bookingArea = document.getElementById("bookingarea");

            var result = eval(msg);

            bookingArea.innerHTML = result.html;

            if (result.errorMessage != "")
                alert(result.errorMessage);
        }
    });
}

/***********************************************/

function padleft(val, ch, num) {
    var re = new RegExp(".{" + num + "}$");
    var pad = "";
    if (!ch) ch = " ";
    do {
        pad += ch;
    } while (pad.length < num);
    return re.exec(pad + val)[0];
}

String.prototype.endsWith = function (str) {
    var lastIndex = this.lastIndexOf(str);
    return (lastIndex != -1) && (lastIndex + str.length == this.length);
}
