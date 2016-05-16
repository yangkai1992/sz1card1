/* Version: 1.2.0.0
 * [summary] Babino.Common javascript class.
 */

Type.registerNamespace('Babino');

Babino.Common = function() {}

Babino.Common.GetRelativePos = function(element) {
    var scrollLeft = 0, scrollTop = 0;

    if (element.scrollLeft) scrollLeft = element.scrollLeft;
    if (element.scrollTop) scrollTop = element.scrollTop;
    
    var pos = { x: element.offsetLeft - scrollLeft, y: element.offsetTop - scrollTop };
    
    if (element.offsetParent && Babino.Common.GetStyle(element.offsetParent, "position") != "relative" &&
        Babino.Common.GetStyle(element.offsetParent, "position") != "absolute" &&
        Babino.Common.GetStyle(element.offsetParent, "position") != "fixed")
    {
        var tmp = Babino.Common.GetRelativePos(element.offsetParent);
        pos.x += tmp.x;
        pos.y += tmp.y;
    }
    
    return pos;
}

Babino.Common.GetStyle = function(element, name) {
    if(element.currentStyle)
        return element.currentStyle[name];
    else
    {
        // Convert style name to W3C format. (ex. "textAlign" means "text-Align")
        name = name.replace(/([A-Z])/g,"-$1");
        name = name.toLowerCase();
        
        return document.defaultView.getComputedStyle(element, null).getPropertyValue("position");
    }
}

Babino.Common.GetInnerText = function(element) {
    if(Babino.Common.GetBrowserType() == Babino.Common.BrowserType.Firefox)
        return Babino.Common.Trim(element.textContent);
    else
        return Babino.Common.Trim(element.innerText);
}

Babino.Common.Trim = function(str) {
    return str.replace(/(^\s*)|(\s*$)/g, "");
}

Babino.Common.GetBrowserType = function() {
    var userAgent = navigator.userAgent.toLowerCase();

    if(userAgent.indexOf(Babino.Common.BrowserType.MSIE) != -1)
        return Babino.Common.BrowserType.MSIE;
    if(userAgent.indexOf(Babino.Common.BrowserType.Firefox) != -1)
        return Babino.Common.BrowserType.Firefox;
    if(userAgent.indexOf(Babino.Common.BrowserType.Safari) != -1)
        return Babino.Common.BrowserType.Safari;

    return Babino.Common.BrowserType.MSIE;
}

Babino.Common.BrowserType = {'MSIE' : 'msie', 'Firefox' : 'firefox', 'Safari' : 'safari'};

Babino.Common.TryFireEvent = function(element, eventName, properties) {
    try {
        if (document.createEventObject) {
            var e = document.createEventObject();
            Babino.Common.ApplyProperties(e, properties || {});
            element.fireEvent("on" + eventName, e);
            return true;
        } else if (document.createEvent) {
            var def = Babino.Common.__DOMEvents[eventName];
            if (def) {
                var e = document.createEvent(def.eventGroup);
                def.init(e, properties || {});
                element.dispatchEvent(e);
                return true;
            }
        }
    } catch (e) {
    }
    return false;
}

Babino.Common.ApplyProperties = function(target, properties) {
    for (var p in properties) {
        var pv = properties[p];
        if (pv != null && Object.getType(pv)===Object) {
            var tv = target[p];
            Babino.Common.ApplyProperties(tv, pv);
        } else {
            target[p] = pv;
        }
    }
}

Babino.Common.__DOMEvents = {
    focusin : { eventGroup : "UIEvents", init : function(e, p) { e.initUIEvent("focusin", true, false, window, 1); } },
    focusout : { eventGroup : "UIEvents", init : function(e, p) { e.initUIEvent("focusout", true, false, window, 1); } },
    activate : { eventGroup : "UIEvents", init : function(e, p) { e.initUIEvent("activate", true, true, window, 1); } },
    focus : { eventGroup : "UIEvents", init : function(e, p) { e.initUIEvent("focus", false, false, window, 1); } },
    blur : { eventGroup : "UIEvents", init : function(e, p) { e.initUIEvent("blur", false, false, window, 1); } },
    click : { eventGroup : "MouseEvents", init : function(e, p) { e.initMouseEvent("click", true, true, window, 1, p.screenX || 0, p.screenY || 0, p.clientX || 0, p.clientY || 0, p.ctrlKey || false, p.altKey || false, p.shiftKey || false, p.metaKey || false, p.button || 0, p.relatedTarget || null); } },
    dblclick : { eventGroup : "MouseEvents", init : function(e, p) { e.initMouseEvent("click", true, true, window, 2, p.screenX || 0, p.screenY || 0, p.clientX || 0, p.clientY || 0, p.ctrlKey || false, p.altKey || false, p.shiftKey || false, p.metaKey || false, p.button || 0, p.relatedTarget || null); } },
    mousedown : { eventGroup : "MouseEvents", init : function(e, p) { e.initMouseEvent("mousedown", true, true, window, 1, p.screenX || 0, p.screenY || 0, p.clientX || 0, p.clientY || 0, p.ctrlKey || false, p.altKey || false, p.shiftKey || false, p.metaKey || false, p.button || 0, p.relatedTarget || null); } },
    mouseup : { eventGroup : "MouseEvents", init : function(e, p) { e.initMouseEvent("mouseup", true, true, window, 1, p.screenX || 0, p.screenY || 0, p.clientX || 0, p.clientY || 0, p.ctrlKey || false, p.altKey || false, p.shiftKey || false, p.metaKey || false, p.button || 0, p.relatedTarget || null); } },
    mouseover : { eventGroup : "MouseEvents", init : function(e, p) { e.initMouseEvent("mouseover", true, true, window, 1, p.screenX || 0, p.screenY || 0, p.clientX || 0, p.clientY || 0, p.ctrlKey || false, p.altKey || false, p.shiftKey || false, p.metaKey || false, p.button || 0, p.relatedTarget || null); } },
    mousemove : { eventGroup : "MouseEvents", init : function(e, p) { e.initMouseEvent("mousemove", true, true, window, 1, p.screenX || 0, p.screenY || 0, p.clientX || 0, p.clientY || 0, p.ctrlKey || false, p.altKey || false, p.shiftKey || false, p.metaKey || false, p.button || 0, p.relatedTarget || null); } },
    mouseout : { eventGroup : "MouseEvents", init : function(e, p) { e.initMouseEvent("mousemove", true, true, window, 1, p.screenX || 0, p.screenY || 0, p.clientX || 0, p.clientY || 0, p.ctrlKey || false, p.altKey || false, p.shiftKey || false, p.metaKey || false, p.button || 0, p.relatedTarget || null); } },
    load : { eventGroup : "HTMLEvents", init : function(e, p) { e.initEvent("load", false, false); } },
    unload : { eventGroup : "HTMLEvents", init : function(e, p) { e.initEvent("unload", false, false); } },
    select : { eventGroup : "HTMLEvents", init : function(e, p) { e.initEvent("select", true, false); } },
    change : { eventGroup : "HTMLEvents", init : function(e, p) { e.initEvent("change", true, false); } },
    submit : { eventGroup : "HTMLEvents", init : function(e, p) { e.initEvent("submit", true, true); } },
    reset : { eventGroup : "HTMLEvents", init : function(e, p) { e.initEvent("reset", true, false); } },
    resize : { eventGroup : "HTMLEvents", init : function(e, p) { e.initEvent("resize", true, false); } },
    scroll : { eventGroup : "HTMLEvents", init : function(e, p) { e.initEvent("scroll", true, false); } }
}
