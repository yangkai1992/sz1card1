var isGecko = navigator.userAgent.toLowerCase().indexOf("gecko") != -1;
function $(ele) {
    if (typeof (ele) == 'string') {
        ele = document.getElementById(ele)
        if (!ele) {
            return null;
        }
    }
    return ele;
}
function stopEvent(event) {//阻止一切事件执行,包括浏览器默认的事件
    event = window.event || event;
    if (!event) {
        return;
    }
    if (isGecko) {
        event.preventDefault();
        event.stopPropagation();
    }
    event.cancelBubble = true
    event.returnValue = false;
}
var Drag = {
    "obj": null,
    "init": function(handle, dragBody, e) {
        if (e == null) {
            handle.onmousedown = Drag.start;
        }
        handle.root = dragBody;

        if (isNaN(parseInt(handle.root.style.left))) handle.root.style.left = "0px";
        if (isNaN(parseInt(handle.root.style.top))) handle.root.style.top = "0px";
        handle.root.onDragStart = new Function();
        handle.root.onDragEnd = new Function();
        handle.root.onDrag = new Function();
        if (e != null) {
            var handle = Drag.obj = handle;
            e = Drag.fixe(e);
            var top = parseInt(handle.root.style.top);
            var left = parseInt(handle.root.style.left);
            handle.root.onDragStart(left, top, e.pageX, e.pageY);
            handle.lastMouseX = e.pageX;
            handle.lastMouseY = e.pageY;
            document.onmousemove = Drag.drag;
            document.onmouseup = Drag.end;
        }
    },
    "start": function(e) {
        var handle = Drag.obj = this;
        e = Drag.fixEvent(e);
        var top = parseInt(handle.root.style.top);
        var left = parseInt(handle.root.style.left);
        handle.root.onDragStart(left, top, e.pageX, e.pageY);
        handle.lastMouseX = e.pageX;
        handle.lastMouseY = e.pageY;
        document.onmousemove = Drag.drag;
        document.onmouseup = Drag.end;
        return false;
    },
    "drag": function(e) {
        e = Drag.fixEvent(e);
        var handle = Drag.obj;
        var mouseY = e.pageY;
        var mouseX = e.pageX;
        var top = parseInt(handle.root.style.top);
        var left = parseInt(handle.root.style.left);

        var currentLeft, currentTop;
        currentLeft = left + mouseX - handle.lastMouseX;
        currentTop = top + (mouseY - handle.lastMouseY);
        handle.root.style.left = currentLeft + "px";
        handle.root.style.top = currentTop + "px";
        handle.lastMouseX = mouseX;
        handle.lastMouseY = mouseY;
        handle.root.onDrag(currentLeft, currentTop, e.pageX, e.pageY);
        return false;
    },
    "end": function() {
        document.onmousemove = null;
        document.onmouseup = null;
        Drag.obj.root.onDragEnd(parseInt(Drag.obj.root.style.left), parseInt(Drag.obj.root.style.top));
        Drag.obj = null;
    },
    "fixEvent": function(e) {//格式化事件参数对象
        var sl = Math.max(document.documentElement.scrollLeft, document.body.scrollLeft);
        var st = Math.max(document.documentElement.scrollTop, document.body.scrollTop);
        if (typeof e == "undefined") e = window.event;
        if (typeof e.layerX == "undefined") e.layerX = e.offsetX;
        if (typeof e.layerY == "undefined") e.layerY = e.offsetY;
        if (typeof e.pageX == "undefined") e.pageX = e.clientX + sl - document.body.clientLeft;
        if (typeof e.pageY == "undefined") e.pageY = e.clientY + st - document.body.clientTop;
        return e;
    }
};
var PopupDiv = {};
PopupDiv.show = function(e) {
    var bgDiv = $(e + 'bg');
    if (bgDiv) {
        bgDiv.style.display = 'block';
    }
    var div = $(e);
    if (div) {
        var doc = window.document;
        var cw = doc.compatMode == "BackCompat" ? doc.body.clientWidth : doc.documentElement.clientWidth;
        var ch = doc.compatMode == "BackCompat" ? doc.body.clientHeight : doc.documentElement.clientHeight;
        var width = parseInt(div.style.width.replace(/px/, ""));
        var height = parseInt(div.style.height.replace(/px/, ""));
        div.style.top = (ch - height) / 2 + "px";
        div.style.left = (cw - width) / 2 + "px";
        div.style.display = 'block';
    }
};
PopupDiv.showDialog = function(e, url) {
    var divContent = $("_" + e + "__content");
    var height = $(e).style.height;
    divContent.innerHTML = '<iframe height="' + height + '" width="100%" frameborder="0" style="border:0px;" src="' + url + '"></iframe>';
    PopupDiv.show(e);
};
PopupDiv.close = function(e) {
    var bgDiv = $(e + 'bg');
    if (bgDiv) {
        bgDiv.style.display = 'none';
    }
    var div = $(e);
    if (div) {
        div.style.display = 'none';
    }
}