//添加星星到地图中。
function addMarkerToMap(_color, _type, _size, p) {
    console.log("start marker");
    temp = 0;
    currProgress = (step / (Math.ceil((totalCount - invalidCount) / pageSize))) * 100;
    document.getElementById("divInfo").innerHTML = "正在标注..." + Math.ceil(currProgress) + "%";
    setInterval(
    function showinfoDiv() {
        if (currProgress == lastProgress) {
            hiddenBottomDiv(false);
            document.getElementById("divInfo").innerHTML = "";
        }
    }, 10000);
    lastProgress = currProgress;
    if (currProgress == 100) {
        hiddenBottomDiv(false);
        document.getElementById("divInfo").innerHTML = "";
    }
    if (document.createElement('canvas').getContext) {  // 判断当前浏览器是否支持绘制海量点
        var points = [];  // 添加海量点数据
        for (var i = 0; i < p.length; i++) {
            //points.push(new BMap.Point(data.data[i][0], data.data[i][1]));
            points.push(new BMap.Point(p[i].lng, p[i].lat));
        }
        var options = {
            size: _size,
            shape: _type,
            color: _color
        }
        //var geoc = new BMap.Geocoder();
        var pointCollection = new BMap.PointCollection(points, options);  // 初始化PointCollection
        //为标注添加单击事件。
        //pointCollection.addEventListener('click', function (e) {
        //    geoc.getLocation(e.point, function (rs) {
        //        var addComp = rs.addressComponents;
        //        alert(addComp.province + ", " + addComp.city + ", " + addComp.district + ", " + addComp.street + ", " + addComp.streetNumber);
        //    });
        //});
        map.addOverlay(pointCollection);  // 添加Overlay
        console.log("end marker");
    } else {
        alert('请在chrome、safari、IE8+以上浏览器查看本示例');
    }
    step++;
}
//给标注添加单击事件。
function markerClickHanler(title, content, marker) {
    marker.addEventListener("click", function (e) {
        openInfo(title, content, e);
    });
}
//打开信息框。
function openInfo(_title, content, e) {
    var opts = {
        width: 100,
        height: 50,
        title: _title
    }
    var p = e.target;
    var point = new BMap.Point(p.getPosition().lng, p.getPosition().lat);
    var infoWindow = new BMap.InfoWindow(content, opts);  // 创建信息窗口对象
    map.openInfoWindow(infoWindow, point); //开启信息窗口
}

//根据经纬度在地图上添加标注。
function addMarkerToMapByPoint(longitude, latitude) {
    var point = new BMap.Point(longitude, latitude);
    var marker = new BMap.Marker(point);
    map.addOverlay(marker);
    markerClickHanler("标题", currentLocation, marker);//添加标注。
    return marker;
}

//根据地址在地图上添加标注。
function addMarkerToMapCenter() {
    var p = map.getCenter();
    var marker = new BMap.Marker(p);
    map.removeOverlay(marker);
    map.addOverlay(marker);
    markerClickHanler("标题", currentLocation, marker);//添加标注。
}
//根据地址在地图上添加标注。
function addMarkerToMapByAddress(add) {
    if (index < arr.length - 1)
        setTimeout(window.addAllMaker, 10);
    // 将地址解析结果显示在地图上
    myGeo.getPoint(add, function (point) {
        if (point) {
            var address = new BMap.Point(point.lng, point.lat);
            var marker = new BMap.Marker(address);
            markerClickHanler("title", add, marker);
            map.addOverlay(marker);
        } else {
            alert("您选择地址没有解析到结果【" + index + ":" + add + "】");
        }
    }, "");
}
