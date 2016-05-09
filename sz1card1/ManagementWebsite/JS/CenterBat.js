var status = 1;
function switchSysBar() {
    if (1 == window.status) {
        window.status = 0;
        switchPoint.innerHTML = '<img src="../images/menu_on.gif"style="cursor:hand">';
        document.all("FrameTitle").style.display = "none"
    }
    else {
        window.status = 1;
        switchPoint.innerHTML = '<img src="../images/menu_off.gif"style="cursor:hand">';
        document.all("FrameTitle").style.display = ""
    }
}
window.onbeforeunload = function() {
    var isIE = document.all ? true : false;
    if (isIE) {
        var n = window.event.screenX - window.screenLeft;
        var b = n > document.documentElement.scrollWidth - 20;
        if (b && window.event.clientY < 0 || window.event.altKey) {
            window.event.returnValue = "非正常退出本页面，可能会引起数据丢失!";
        }
    } else {
        //        if (document.documentElement.scrollWidth != 0) {
        //            return "非正常退出本页面，可能会引起数据丢失!";
        //        }
    }
}
function showWindow(windowName) {
    var url = windowName, sw, sh, w = h = 650, pra;
    sw = Math.floor((window.screen.width / 2 - w / 2));
    sh = Math.floor((window.screen.height / 2 - h / 2));
    pra = "height=" + h + ", width=" + w + ", top=" + sh + ", left=" + sw + "menubar=no, scrollbars=yes, resizable=no,location=no, status=no";
    window.open(url, "", pra, true);
}


