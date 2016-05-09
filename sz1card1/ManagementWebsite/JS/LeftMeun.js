//function shrinkmeun(url) {
//    document.getElementById("table3" + url).style.display = (document.getElementById("table3" + url).style.display == "") ? "none" : "";
//}

/*----------------------------------------------------------------------*/

//页面加载默认展开第一个Title下面的二级菜单
function DefaltExpand() {
    var list = document.getElementById("Str").getElementsByTagName("table");
    for (var i = 0; i < list.length; i++) {
        if (list[i].title == "菜单") {
            list[i].style.display = "";
        } else {
            if (list[i].id == "table32") {
                list[i].style.display = "";
            } else if (list[i].title == "") {
                list[i].style.display = "none";
            }
        }
    }
    showDivHeidth(2, 'first')//参数一不能更改的
}

//点击菜单Title展开二级菜单隐藏其它的二级菜单
function shrinkmeun(obj, url) {
    var list = document.getElementById("Str").getElementsByTagName("table");

    for (var i = 0; i < list.length; i++) {
        if (list[i].title == "菜单" && list[i].id == "table3" + url) {
            list[i].style.display = "";
        } else {
            if (obj.id == list[i].id && list[i].title == "") {
                list[i].style.display = (list[i].style.display == "") ? "none" : "";
            } else if (list[i].title == "") {
                list[i].style.display = "none";
            }
        }
    }
    showDivHeidth(url)
}
//自动获取Body与Div的高度间距并赋值给NewDiv对象
function showDivHeidth(url, flag) {
    document.getElementById("newDiv" + url).style.height = 0 + "px";
    var body = document.documentElement.clientHeight;
    var div1 = document.getElementById("Topdiv").offsetHeight;
    var div2 = document.getElementById("downdiv").offsetHeight;
    var newDiv = body - (div1 + div2);
    var OsObject = "";
    if (flag) {
        newDiv = newDiv - 87;
    }
    if (navigator.userAgent.indexOf("MSIE") > 0) {
        if (navigator.userAgent.indexOf("MSIE 6.0") > 0) {
            document.getElementById("newDiv" + url).style.height = 15 + newDiv + "px";
        } else {
            document.getElementById("newDiv" + url).style.height = newDiv + "px";
        }
    } else if (isFirefox = navigator.userAgent.indexOf("Firefox") > 0) {
        document.getElementById("newDiv" + url).style.height = newDiv + "px";
    } else {
        document.getElementById("newDiv" + url).style.height = newDiv + "px";
    }
}
/*------------------------------------------------------------------------------*/

//<--页面导航 Start-->
var pathArr = [];
function AddUrlToPathArr(path, title) {
    for (var i = 0; i < pathArr.length; i++) {
        pathArr[i].action = false;
    }
    var urlObj = new Object();
    urlObj.url = path;
    urlObj.action = true;
    pathArr.push(urlObj);
    var id = path.replace(/\./g, "_").replace(/\//g, "_");
    window.parent.CreateDiv(id, path, title);
}
function ReLoadURL() {
    window.frames["Frameright"].location = window.frames["Frameright"].location;
}
function GoPre() {
    var url;
    for (var i = 0; i < pathArr.length; i++) {
        if (pathArr[i].action == true) {
            pathArr[i].action = false;
            if (i - 1 < 0) {
                url = pathArr[0].url;
                pathArr[0].action = true;
                break;
            }
            url = pathArr[i - 1].url;
            pathArr[i - 1].action = true;
            break;
        }
    }
    if (url != null) {
        window.parent.frames["Frameright"].location = url;
    }
}
function GoNext() {
    var url;
    for (var i = 0; i < pathArr.length; i++) {
        if (pathArr[i].action == true) {
            pathArr[i].action = false;
            if (i + 1 == pathArr.length) {
                url = pathArr[pathArr.length - 1].url;
                pathArr[pathArr.length - 1].action = true;
                break;
            }
            url = pathArr[i + 1].url;
            pathArr[i + 1].action = true;
            break;
        }
    }
    if (url != null) {
        window.parent.frames["Frameright"].location = url;
    }
}
//<--页面导航 End--> 