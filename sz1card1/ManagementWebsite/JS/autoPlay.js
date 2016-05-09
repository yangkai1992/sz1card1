
var t = n = 0, count;
$(document).ready(function () {
    setBtnPos();
    count = $("#banner_list img").length;
    //如果只有一张图片，则隐藏轮播按钮，不执行轮播代码。
    if (count <= 1) {
        $("#banner li").hide();
        return;
    }
    n = getPicture_Random();
    $("#" + (n + 1) + "").css("color", "#0094ff");
    $("#banner li").click(function () {
        n = $(this).attr("id") - 1;//获取Li元素内ID的值，即1，2，3。
        if (n >= count) return;
        $("#banner_list img").filter(":visible").fadeOut(500).parent().children().eq(n).fadeIn(1000);
        $(this).css({ 'color': '#0094ff' }).siblings().css({ 'color': 'gray' });
        $("#banner_list").css("height", $("#banner_list img").css("height"));
    });

    //开始自动播放。
    t = setInterval("autoPlay()", 3000);

    //鼠标进入按钮时，停止轮播计时,离开后重新计时。
    $("#banner li").hover(function () {
        clearInterval(t)
    }, function () {
        t = setInterval("autoPlay()", 3000);
    });
})

//自动播放。
function autoPlay() {
    n = n >= (count - 1) ? 0 : ++n;
    $("#banner li").eq(n).trigger('click');
}

//随机选择一张图片开始播放。
function getPicture_Random() {
    var ran = Math.floor(Math.random() * count + 0);
    $("#banner_list img").not(":eq(" + ran + ")").hide();
    return ran;
}

//从第一张图片开始播放。
function getPicture_First() {
    $("#banner_list img:not(:first)").hide();
    return 0;
}
//调整按钮位置。
function setBtnPos() {
    var screenWidth = document.body.offsetWidth / 2;
    var ulWidth = $("#banner ul").css("width").replace("px", "") / 2;
    var left = screenWidth - ulWidth;
    var top = $("#banner_list img").css("height").replace("px", "");
    $("#banner ul").css("left", left).css("top", top - 70);
}

window.onresize = setBtnPos;