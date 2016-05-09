
function html_Replace(str) {
    var s = "";
    if (str.length == 0) return "";
    s = str.replace(/&/g, "&amp;");
    s = s.replace(/</g, "&lt;");
    s = s.replace(/>/g, "&gt;");
    s = s.replace(/ /g, "+");
    s = s.replace(/\'/g, "&#39;");
    s = s.replace(/\"/g, "&quot;");
    return s;
}

function ajaxRequest(url, method, data, successed, failed) {
    var ajax = new XMLHttpRequest();
    ajax.open(method, url, true);
    ajax.setRequestHeader("Content-Type", "application/x-www-form-urlencoded;charset=utf-8");
    ajax.onreadystatechange = function () {
        if (ajax.readyState == 4 && ajax.status == 200) {
            if (successed != undefined)
                successed(ajax);
        }
        else if (ajax.readyState == 4 && ajax.status == 500) {
            if (failed != undefined)
                failed(ajax);
        }
    }
    ajax.send(data);
}
var $_GET = (function () {
    var url = window.document.location.href.toString();
    var u = url.split("?");
    if (typeof (u[1]) == "string") {
        u = u[1].split("&");
        var get = {};
        for (var i in u) {
            var j = u[i].split("=");
            get[j[0]] = j[1];
        }
        return get;
    } else {
        return {};
    }
})();