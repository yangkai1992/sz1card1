//var CONTEXTPATH = 'http://www.5-studio.com/wp-content/uploads/2009/06/';//图片路径配置
var CONTEXTPATH = '../images/Dialog/'; //图片路径配置
var isIE = navigator.userAgent.toLowerCase().indexOf("msie") != -1;
var isIE6 = navigator.userAgent.toLowerCase().indexOf("msie 6.0") != -1;
var isGecko = navigator.userAgent.toLowerCase().indexOf("gecko") != -1;

function $(ele) {
    if (typeof (ele) == 'string') {
        ele = document.getElementById(ele)
        if (!ele) {
            return null;
        }
    }
    if (ele) {
        Core.attachMethod(ele);
    }
    return ele;
}
function $T(tagName, ele) {
    ele = $(ele);
    ele = ele || document;
    var ts = ele.getElementsByTagName(tagName); //此处返回的不是数组
    var arr = [];
    var len = ts.length;
    for (var i = 0; i < len; i++) {
        arr.push($(ts[i]));
    }
    return arr;
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

Array.prototype.remove = function(s) {
    for (var i = 0; i < this.length; i++) {
        if (s == this[i]) {
            this.splice(i, 1);
        }
    }
}
if (window.HTMLElement) {//给FF添加IE专有的属性和方法
    HTMLElement.prototype.__defineGetter__("parentElement", function() {
        if (this.parentNode == this.ownerDocument) return null;
        return this.parentNode;
    });
    HTMLElement.prototype.__defineSetter__("outerHTML", function(sHTML) {
        var r = this.ownerDocument.createRange();
        r.setStartBefore(this);
        var df = r.createContextualFragment(sHTML);
        this.parentNode.replaceChild(df, this);
        return sHTML;
    });
    HTMLElement.prototype.__defineGetter__("outerHTML", function() {
        var attr;
        var attrs = this.attributes;
        var str = "<" + this.tagName;
        for (var i = 0; i < attrs.length; i++) {
            attr = attrs[i];
            if (attr.specified)
                str += " " + attr.name + '="' + attr.value + '"';
        }
        if (!this.canHaveChildren)
            return str + ">";
        return str + ">" + this.innerHTML + "</" + this.tagName + ">";
    });
    HTMLElement.prototype.__defineSetter__("innerText", function(sText) {
        var parsedText = document.createTextNode(sText);
        this.innerHTML = parsedText;
        return parsedText;
    });
    HTMLElement.prototype.__defineGetter__("innerText", function() {
        var r = this.ownerDocument.createRange();
        r.selectNodeContents(this);
        return r.toString();
    });
}

var $E = {};
$E.getTopLevelWindow = function() {
    var pw = window;
    while (pw != pw.parent) {
        pw = pw.parent;
    }
    return pw;
}
$E.hide = function(ele) {
    ele = ele || this;
    ele = $(ele);
    ele.style.display = 'none';
}
$E.show = function(ele) {
    ele = ele || this;
    ele = $(ele);
    ele.style.display = '';
}
var Core = {};
Core.attachMethod = function(ele) {
    if (!ele || ele["$A"]) {
        return;
    }
    if (ele.nodeType == 9) {
        return;
    }
    var win;
    try {
        if (isGecko) {
            win = ele.ownerDocument.defaultView;
        } else {
            win = ele.ownerDocument.parentWindow;
        }
        for (var prop in $E) {
            ele[prop] = win.$E[prop];
        }
    } catch (ex) {
        //alert("Core.attachMethod:"+ele)//有些对象不能附加属性，如flash
    }
}

function Dialog(strID) {
    if (!strID) {
        alert("错误的Dialog ID！");
        return;
    }
    this.ID = strID;
    this.isModal = true;
    this.Width = 400;
    this.Height = 300;
    this.Top = 0;
    this.Left = 0;
    this.ParentWindow = null;
    this.onLoad = null;
    this.Window = null;

    this.Title = "";
    this.URL = null;
    this.DialogArguments = {};
    this.WindowFlag = false;
    this.Message = null;
    this.ShowMessageRow = false;
    //this.ShowButtonRow = true;
    this.Icon = null;
    this.bgdivID = null;
}

Dialog._Array = [];

Dialog.prototype.showWindow = function() {
    if (isIE) {
        this.ParentWindow.showModalessDialog(this.URL, this.DialogArguments, "dialogWidth:" + this.Width + ";dialogHeight:" + this.Height + ";help:no;scroll:no;status:no");
    }
    if (isGecko) {
        var sOption = "location=no,menubar=no,status=no;toolbar=no,dependent=yes,dialog=yes,minimizable=no,modal=yes,alwaysRaised=yes,resizable=no";
        this.Window = this.ParentWindow.open('', this.URL, sOption, true);
        var w = this.Window;
        if (!w) {
            alert("发现弹出窗口被阻止，请更改浏览器设置，以便正常使用本功能!");
            return;
        }
        w.moveTo(this.Left, this.Top);
        w.resizeTo(this.Width, this.Height + 30);
        w.focus();
        w.location.href = this.URL;
        w.Parent = this.ParentWindow;
        w.dialogArguments = this.DialogArguments;
    }
}

Dialog.prototype.show = function() {
    var pw = $E.getTopLevelWindow();
    var doc = pw.document;
    var cw = doc.compatMode == "BackCompat" ? doc.body.clientWidth : doc.documentElement.clientWidth;
    var ch = doc.compatMode == "BackCompat" ? doc.body.clientHeight : doc.documentElement.clientHeight; //必须考虑文本框处于页面边缘处，控件显示不全的问题
    var sl = Math.max(doc.documentElement.scrollLeft, doc.body.scrollLeft);
    var st = Math.max(doc.documentElement.scrollTop, doc.body.scrollTop); //考虑滚动的情况
    var sw = Math.max(doc.documentElement.scrollWidth, doc.body.scrollWidth);
    var sh = Math.max(doc.documentElement.scrollHeight, doc.body.scrollHeight); //考虑滚动的情况
    sw = Math.max(sw, cw);
    sh = Math.max(sh, ch);
    //	alert("\n"+cw+"\n"+ch+"\n"+sw+"\n"+sh)

    if (!this.ParentWindow) {
        this.ParentWindow = window;
    }
    this.DialogArguments._DialogInstance = this;
    this.DialogArguments.ID = this.ID;

    if (!this.Height) {
        this.Height = this.Width / 2;
    }

    if (this.Top == 0) {
        this.Top = (ch - this.Height - 30) / 2 + st - 8;
    }
    if (this.Left == 0) {
        this.Left = (cw - this.Width - 12) / 2 + sl;
    }
    if (this.WindowFlag) {
        this.showWindow();
        return;
    }
    var arr = [];
    arr.push("<table style='-moz-user-select:none;' oncontextmenu='stopEvent(event);' onselectstart='stopEvent(event);' border='0' cellpadding='0' cellspacing='0' width='" + (this.Width + 26) + "'>");
    arr.push("  <tr style='cursor:move;' id='_draghandle_" + this.ID + "'>");
    arr.push("    <td width='13' height='33' style=\"background-image:url(" + CONTEXTPATH + "dialog_lt.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + CONTEXTPATH + "dialog_lt.png', sizingMethod='crop');\"><div style='width:13px;'></div></td>");
    arr.push("    <td height='33' style=\"background-image:url(" + CONTEXTPATH + "dialog_ct.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + CONTEXTPATH + "dialog_ct.png', sizingMethod='crop');\"><div style=\"float:left;font-weight:bold; color:#FFFFFF; padding:9px 0 0 4px;\"><img src=\"" + CONTEXTPATH + "icon_dialog.gif\" align=\"absmiddle\">&nbsp;" + this.Title + "</div>");
    arr.push("      <div style=\"position: relative;cursor:pointer; float:right; margin:5px 0 0; _margin:4px 0 0;height:17px; width:28px; background-image:url(" + CONTEXTPATH + "dialog_closebtn.gif)\" onMouseOver=\"this.style.backgroundImage='url(" + CONTEXTPATH + "dialog_closebtn_over.gif)'\" onMouseOut=\"this.style.backgroundImage='url(" + CONTEXTPATH + "dialog_closebtn.gif)'\" drag='false' onClick=\"Dialog.getInstance('" + this.ID + "').close();\"></div></td>");
    arr.push("    <td width='13' height='33' style=\"background-image:url(" + CONTEXTPATH + "dialog_rt.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + CONTEXTPATH + "dialog_rt.png', sizingMethod='crop');\"><div style=\"width:13px;\"></div></td>");
    arr.push("  </tr>");
    arr.push("  <tr drag='false'><td width='13' style=\"background-image:url(" + CONTEXTPATH + "dialog_mlm.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + CONTEXTPATH + "dialog_mlm.png', sizingMethod='crop');\"></td>");
    arr.push("    <td align='center' valign='top'>");
    arr.push("    <table width='100%' border='0' cellpadding='0' cellspacing='0' bgcolor='#FFFFFF'>");
    arr.push("        <tr id='_MessageRow_" + this.ID + "' style='display:none'>");
    arr.push("          <td height='30' valign='top'><table id='_MessageTable_" + this.ID + "' width='100%' border='0' cellspacing='0' cellpadding='4' style=\" background:#EAECE9 url(" + CONTEXTPATH + "dialog_bg.jpg) no-repeat right top;\">");
    arr.push("              <tr><td width='25' height='30' align='right'><img id='_MessageIcon_" + this.ID + "' src='" + CONTEXTPATH + "window.gif' width='16' height='16'></td>");
    arr.push("                <td align='left' style='line-height:14px;'>");
    arr.push("                <div id='_Message_" + this.ID + "'>&nbsp;</div></td>");
    arr.push("              </tr></table></td></tr>");
    arr.push("        <tr><td align='center' valign='top'>");
    arr.push("          <iframe src='");
    if (this.URL.indexOf(":") == -1) {
        arr.push(this.URL);
    } else {
        arr.push(this.URL);
    }
    arr.push("' id='_DialogFrame_" + this.ID + "' allowTransparency='true'  width='" + this.Width + "' height='" + this.Height + "' frameborder='0' style=\"background-color: #transparent; border:none;\"></iframe></td></tr>");
    arr.push("      </table></td>");
    arr.push("    <td width='13' style=\"background-image:url(" + CONTEXTPATH + "dialog_mrm.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + CONTEXTPATH + "dialog_mrm.png', sizingMethod='crop');\"></td></tr>");
    arr.push("  <tr><td width='13' height='13' style=\"background-image:url(" + CONTEXTPATH + "dialog_lb.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + CONTEXTPATH + "dialog_lb.png', sizingMethod='crop');\"></td>");
    arr.push("    <td style=\"background-image:url(" + CONTEXTPATH + "dialog_cb.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + CONTEXTPATH + "dialog_cb.png', sizingMethod='crop');\"></td>");
    arr.push("    <td width='13' height='13' style=\"background-image:url(" + CONTEXTPATH + "dialog_rb.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + CONTEXTPATH + "dialog_rb.png', sizingMethod='crop');\"></td>");
    arr.push("  </tr></table>");
    this.TopWindow = pw;

    var bgdiv = pw.$("_DialogBGDiv");
    if (!bgdiv) {
        bgdiv = pw.document.createElement("div");
        bgdiv.id = "_DialogBGDiv";
        $E.hide(bgdiv);
        pw.$T("body")[0].appendChild(bgdiv);
        if (isIE6) {
            var bgIframeBox = document.createElement('<div style="position:relative;width:100%;height:100%;"></div>');
            var bgIframe = document.createElement('<iframe src="about:blank" style="filter:alpha(opacity=1);" width="100%" height="100%"></iframe>');
            var bgIframeMask = document.createElement('<div src="about:blank" style="position:absolute;background-color:#333;filter:alpha(opacity=1);width:100%;height:100%;"></div>');
            bgIframeBox.appendChild(bgIframeMask);
            bgIframeBox.appendChild(bgIframe);
            bgdiv.appendChild(bgIframeBox);
            var bgIframeDoc = bgIframe.contentWindow.document;
            bgIframeDoc.open();
            bgIframeDoc.write("<body style='background-color:#333' oncontextmenu='return false;'></body>");
            bgIframeDoc.close();
        }
    }
    var div = pw.$("_DialogDiv_" + this.ID);
    if (!div) {
        div = pw.document.createElement("div");
        $E.hide(div);
        div.id = "_DialogDiv_" + this.ID;
        div.className = "dialogdiv";
        div.setAttribute("dragStart", "Dialog.dragStart");
        pw.$T("body")[0].appendChild(div);
    }

    this.DialogDiv = div;
    div.innerHTML = arr.join('\n');

    pw.$("_DialogFrame_" + this.ID).DialogInstance = this;

    pw.Drag.init(pw.$("_draghandle_" + this.ID), pw.$("_DialogDiv_" + this.ID)); //注册拖拽方法

    //显示标题图片
    if (this.ShowMessageRow) {
        $E.show(pw.$("_MessageRow_" + this.ID));
        if (this.Message) {
            pw.$("_Message_" + this.ID).innerHTML = this.Message;
        }
    }
    if (!this.AlertFlag) {
        $E.show(bgdiv);
        this.bgdivID = "_DialogBGDiv";
    } else {
        bgdiv = pw.$("_AlertBGDiv");
        if (!bgdiv) {
            bgdiv = pw.document.createElement("div");
            bgdiv.id = "_AlertBGDiv";
            $E.hide(bgdiv);
            pw.$T("body")[0].appendChild(bgdiv);
            if (isIE6) {
                var bgIframeBox = document.createElement('<div style="position:relative;width:100%;height:100%;"></div>');
                var bgIframe = document.createElement('<iframe src="about:blank" style="filter:alpha(opacity=1);" width="100%" height="100%"></iframe>');
                var bgIframeMask = document.createElement('<div src="about:blank" style="position:absolute;background-color:#333;filter:alpha(opacity=1);width:100%;height:100%;"></div>');
                bgIframeBox.appendChild(bgIframeMask);
                bgIframeBox.appendChild(bgIframe);
                bgdiv.appendChild(bgIframeBox);
                var bgIframeDoc = bgIframe.contentWindow.document;
                bgIframeDoc.open();
                bgIframeDoc.write("<body style='background-color:#333' oncontextmenu='return false;'></body>");
                bgIframeDoc.close();
            }
            bgdiv.style.cssText = "background-color:#333;position:absolute;left:0px;top:0px;opacity:0.4;filter:alpha(opacity=40);width:100%;height:" + sh + "px;z-index:991";
        }
        $E.show(bgdiv);
        this.bgdivID = "_AlertBGDiv";
    }
    this.DialogDiv.style.cssText = "position:absolute; display:block;z-index:" + (this.AlertFlag ? 992 : 990) + ";left:" + this.Left + "px;top:" + this.Top + "px";

    //判断当前窗口是否是对话框，如果是，则将其置在bgdiv之后
    if (!this.AlertFlag) {
        var win = window;
        var flag = false;
        while (win != win.parent) {//需要考虑父窗口是弹出窗口中的一个iframe的情况
            if (win._DialogInstance) {
                win._DialogInstance.DialogDiv.style.zIndex = 959;
                flag = true;
                break;
            }
            win = win.parent;
        }
        if (!flag) {
            bgdiv.style.cssText = "background-color:#333;position:absolute;left:0px;top:0px;opacity:0.4;filter:alpha(opacity=40);width:100%;height:" + sh + "px;z-index:960";
        }
    }
    //this.OKButton.focus();
    //放入队列中，以便于ESC时正确关闭
    pw.Dialog._Array.push(this.ID);
}

Dialog.prototype.addParam = function(paramName, paramValue) {
    this.DialogArguments[paramName] = paramValue;
}

Dialog.prototype.close = function() {
    if (this.WindowFlag) {
        this.ParentWindow.$D = null;
        this.ParentWindow.$DW = null;
        this.Window.opener = null;
        this.Window.close();
        this.Window = null;
    } else {
        //如果上级窗口是对话框，则将其置于bgdiv前
        var pw = $E.getTopLevelWindow();
        var win = window;
        var flag = false;
        while (win != win.parent) {
            if (win._DialogInstance) {
                flag = true;
                win._DialogInstance.DialogDiv.style.zIndex = 960;
                break;
            }
            win = win.parent;
        }
        if (this.AlertFlag) {
            $E.hide(pw.$("_AlertBGDiv"));
        }
        if (!flag && !this.AlertFlag) {//此处是为处理弹出窗口被关闭后iframe立即被重定向时背景层不消失的问题
            pw.eval('window._OpacityFunc = function(){var w = $E.getTopLevelWindow();$E.hide(w.$("_DialogBGDiv"));}');
            pw._OpacityFunc();
        }
        this.DialogDiv.outerHTML = "";
        pw.Dialog._Array.remove(this.ID);
    }
}

Dialog.prototype.addButton = function(id, txt, func) {
    var html = "<input id='_Button_" + this.ID + "_" + id + "' type='button' value='" + txt + "'> ";
    var pw = $E.getTopLevelWindow();
    pw.$("_DialogButtons_" + this.ID).$T("input")[0].getParent("a").insertAdjacentHTML("beforeBegin", html);
    pw.$("_Button_" + this.ID + "_" + id).onclick = func;
}

Dialog.close = function(evt) {
    window.Args._DialogInstance.close();
}

Dialog.getInstance = function(id) {
    var pw = $E.getTopLevelWindow()
    var f = pw.$("_DialogFrame_" + id);
    if (!f) {
        return null;
    }
    return f.DialogInstance;
}

Dialog.popup = function(div, title, w, h) {
    if (!$(div))
        return;
    var pw = $E.getTopLevelWindow();
    var diag = new Dialog("_DialogPopup" + div);
    diag.ParentWindow = pw;
    diag.Width = w ? w : 300;
    diag.Height = h ? h : 240;
    diag.Title = title ? title : '详细信息';
    diag.URL = "javascript:void(0);";
    diag.show();
    
}

Dialog.AlertNo = 0;
Dialog.alert = function(msg, func, w, h) {
    var pw = $E.getTopLevelWindow()
    var diag = new Dialog("_DialogAlert" + Dialog.AlertNo++);
    diag.ParentWindow = pw;
    diag.Width = w ? w : 300;
    diag.Height = h ? h : 120;
    diag.Title = "系统提示";
    diag.URL = "javascript:void(0);";
    diag.AlertFlag = true;
    diag.show();
    pw.$("_AlertBGDiv").style.display = "";
    var win = pw.$("_DialogFrame_" + diag.ID).contentWindow;
    var doc = win.document;
    doc.open();
    doc.write("<body oncontextmenu='return false;'></body>");
    var arr = [];
    arr.push("<table height='100%' border='0' align='center' cellpadding='10' cellspacing='0'>");
    arr.push("<tr><td align='right'><img id='Icon' src='" + CONTEXTPATH + "icon_alert.gif' width='34' height='34' align='absmiddle'></td>");
    arr.push("<td align='left' id='Message' style='font-size:9pt'>" + msg + "</td></tr>");
    arr.push("</table>");
    var div = doc.createElement("div");
    div.innerHTML = arr.join('');
    doc.body.appendChild(div);
    doc.close();
    var h = Math.max(doc.documentElement.scrollHeight, doc.body.scrollHeight);
    var w = Math.max(doc.documentElement.scrollWidth, doc.body.scrollWidth);
    if (w > 300) {
        win.frameElement.width = w;
    }
    if (h > 120) {
        win.frameElement.height = h;
    }
}

var _DialogInstance = window.frameElement ? window.frameElement.DialogInstance : null;
var Page = {};
Page.onDialogLoad = function() {
    if (_DialogInstance) {
        if (_DialogInstance.Title) {
            document.title = _DialogInstance.Title;
        }
        window.Args = _DialogInstance.DialogArguments;
        _DialogInstance.Window = window;
        window.Parent = _DialogInstance.ParentWindow;
    }
}

Page.onDialogLoad();

PageOnLoad = function() {
    var d = _DialogInstance;
    if (d) {
        try {
            d.ParentWindow.$D = d;
            d.ParentWindow.$DW = d.Window;
            var flag = false;
            if (!this.AlertFlag) {
                var win = d.ParentWindow;
                while (win != win.parent) {
                    if (win._DialogInstance) {
                        flag = true;
                        break;
                    }
                    win = win.parent;
                }
                if (!flag) {
                    $E.getTopLevelWindow().$("_DialogBGDiv").style.opacity = "0";
                    $E.getTopLevelWindow().$("_DialogBGDiv").style.filter = "alpha(opacity=0)";
                }
            }
            if (d.AlertFlag) {
                $E.show($E.getTopLevelWindow().$("_AlertBGDiv"));
            }
            if (d.onLoad) {
                d.onLoad();
            }
        } catch (ex) { alert("DialogOnLoad:" + ex.message + "\t(" + ex.fileName + " " + ex.lineNumber + ")"); }
    }
}

Dialog.onKeyUp = function(event) {
    if (event.keyCode == 9) {
        var pw = $E.getTopLevelWindow();
        if (pw.Dialog._Array.length > 0) {
            stopEvent(event);
        }
    }
    if (event.keyCode == 27) {
        var pw = $E.getTopLevelWindow();
        if (pw.Dialog._Array.length > 0) {
            var diag = pw.Dialog.getInstance(pw.Dialog._Array[pw.Dialog._Array.length - 1]);
            diag.close();
        }
    }
}

Dialog.dragStart = function(evt) {
    //DragManager.doDrag(evt,this.getParent("div"));//拖拽处理
}
Dialog.setPosition = function() {
    if (window.parent != window) return;
    var pw = $E.getTopLevelWindow();
    var DialogArr = pw.Dialog._Array;
    if (DialogArr == null || DialogArr.length == 0) return;

    for (i = 0; i < DialogArr.length; i++) {
        pw.$("_DialogFrame_" + DialogArr[i]).DialogInstance.setPosition();
    }
}
Dialog.prototype.setPosition = function() {
    var pw = $E.getTopLevelWindow();
    var doc = pw.document;
    var cw = doc.compatMode == "BackCompat" ? doc.body.clientWidth : doc.documentElement.clientWidth;
    var ch = doc.compatMode == "BackCompat" ? doc.body.clientHeight : doc.documentElement.clientHeight; //必须考虑文本框处于页面边缘处，控件显示不全的问题
    var sl = Math.max(doc.documentElement.scrollLeft, doc.body.scrollLeft);
    var st = Math.max(doc.documentElement.scrollTop, doc.body.scrollTop); //考虑滚动的情况
    var sw = Math.max(doc.documentElement.scrollWidth, doc.body.scrollWidth);
    var sh = Math.max(doc.documentElement.scrollHeight, doc.body.scrollHeight);
    sw = Math.max(sw, cw);
    sh = Math.max(sh, ch);
    this.Top = (ch - this.Height - 30) / 2 + st - 8; //有8像素的透明背景
    this.Left = (cw - this.Width - 12) / 2 + sl;
    this.DialogDiv.style.top = this.Top + "px";
    this.DialogDiv.style.left = this.Left + "px";
    pw.$(this.bgdivID).style.height = sh + "px";
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

if (isIE) {
    document.attachEvent("onkeydown", Dialog.onKeyUp);
    window.attachEvent("onload", PageOnLoad);
    window.attachEvent('onresize', Dialog.setPosition);
} else {
    document.addEventListener("keydown", Dialog.onKeyUp, false);
    window.addEventListener("load", PageOnLoad, false);
    window.addEventListener('resize', Dialog.setPosition, false);
}