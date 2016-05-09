//自定义标注尺寸。
var overlaySize = {
    //超小。
    SS_TINY: "5",
    //很小。
    SS_SMALLER: "10",
    //小。
    SS_SMALL: "15",
    //正常。
    SS_NORMAL: "20",
    //大。
    SS_BIG: "25",
    //很大。
    SS_BIGGER: "30",
    //超大。
    SS_HUGE: "35"
}
var overlayColor = [
        "Cyan",
        "Green",
        "Gray",
        "Red",
        "Yellow",
        "LightGreen",
        "Blue",
        "Orange",
        "Purple",
        "DeepPink",
        "Salmon",
        "GreenYellow",
        "IndianRed",
        "White"
];
// 复杂的自定义覆盖物
function CustomOverlay(point, text, color, size) {
    this._point = point;
    this._text = text;
    this._color = color;
    this._size = size;
    this._fn;
    this._event;
}
CustomOverlay.prototype = new BMap.Overlay();
CustomOverlay.prototype.initialize = function (map) {
    this._map = map;
    var div = this._div = document.createElement("div");
    div.style.position = "absolute";
    div.style.zIndex = BMap.Overlay.getZIndex(this._point.lat);

    var that = this;
    div.style.cssText = "border-radius: 25px;";
    div.style.background = this._color;
    div.style.position = "absolute";
    div.style.width = this._size + "px";
    div.style.height = this._size + "px";
    div.style.top = "22px";
    div.style.left = "10px";
    div.style.overflow = "hidden";
    div.style.cursor = "pointer";
    div[this._event] = this._fn;
    div.onclick = function (obj) {
        alert(that._text)
    }
    map.getPanes().labelPane.appendChild(div);
    return div;
}
//实现绘制方法。
CustomOverlay.prototype.draw = function () {
    var map = this._map;
    var pixel = map.pointToOverlayPixel(this._point);
    this._div.style.left = pixel.x - this._size + "px";
    this._div.style.top = pixel.y - this._size + "px";
}
//为自定义标注添加事件。
CustomOverlay.prototype.addEventLisner = function (event, fn) {
    this._fn = fn(this);
    this._event = 'on' + event;
};