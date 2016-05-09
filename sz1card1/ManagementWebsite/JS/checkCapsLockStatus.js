$(document).ready(function () {
    if (navigator.userAgent.indexOf("MSIE") != -1)
        return;
    var txtPassword = $("#txtUserPassword");
    function show(id) {
        var ele = $("#" + id + "");
        $(ele).css("display", "block");
        $("#txtCheck").parent().attr("style", "margin-top:15px");
    }
    function hide(id) {
        var ele = $("#" + id + "");
        $(ele).css("display", "none");
        $("#txtCheck").parent().attr("style", "");
    }
    var isCapslockOn = undefined;
    function checkCapsLock_keyPress() {
        var e = event || window.event;
        var keyCode = e.keyCode || e.which;
        var isShift = e.shiftKey || (keyCode == 16) || false; //shift键是否按住。
        if (
        ((keyCode >= 65 && keyCode <= 90) && !isShift) //CapsLock 打开，且没有按住shift键 。
        || ((keyCode >= 97 && keyCode <= 122) && isShift))//CapsLock 打开，且按住shift键。
            isCapslockOn = true;
        else
            isCapslockOn = false;
    }
    function checkCapsLock_keydown(e) {
        var keynum = window.event ? e.keyCode : e.which;
        if (keynum == 20 && isCapslockOn == true)
            isCapslockOn = false;
        else if (keynum == 20 && isCapslockOn == false)
            isCapslockOn = true;
    }
    function tip() {
        if (isCapslockOn)
            show("capsLockTip");
        else
            hide("capsLockTip");
    }

    $(document).keypress(checkCapsLock_keyPress);
    $(document).keydown(checkCapsLock_keydown);
    txtPassword.keyup(tip).hover(
        function () {
            if (isCapslockOn) show("capsLockTip");
        }, function () {
            hide("capsLockTip");
        }).blur(function () {
            hide("capsLockTip");
        })
});