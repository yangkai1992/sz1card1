function rnd() {
    rnd.seed = (rnd.seed * 9301 + 49297) % 233280;
    return rnd.seed / (233280.0);

}
rnd.today = new Date();
rnd.seed = rnd.today.getTime();

function rand(number) {
    return Math.ceil(rnd() * number);

}
function ChangeCodeImg(img) {
    var a = document.getElementById(img);
    a.src = "/Common/CodeImg.aspx?" + rand(10000000);
}
