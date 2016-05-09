
var MD5Code;
var UKBussinessAccount;
var UKUserAccount;
var separator = '\x1d';
var keyenable = false;
var clientDatetime = new Date();

window.onload = function () {

    //解决ie不兼容getElementsByClassName
    if (!document.getElementsByClassName) {
        document.getElementsByClassName = function (className, element) {
            var children = (element || document).getElementsByTagName('*');
            var elements = new Array();
            for (var i = 0; i < children.length; i++) {
                var child = children[i];
                var classNames = child.className.split(' ');
                for (var j = 0; j < classNames.length; j++) {
                    if (classNames[j] == className) {
                        elements.push(child);
                        break;
                    }
                }
            }
            return elements;
        };
    }

    var ul = document.getElementsByClassName("tab")[0];
    var li = ul.getElementsByTagName("li");

    for (var i = 0, len = li.length; i < len; i++) {

        li[i].onclick = function () {
            for (var j = 0, lens = li.length; j < lens; j++) {
                li[j].removeAttribute("class");
                if (this == li[j]) {
                    this.setAttribute("class", "current");
                    checkLogin(j);
                    changeIndex(j);
                }
            }
        }
    };
    LoadCookies();
    Update(document.getElementById("hidUpdateScript").value, "为提高客户端网络连接的稳定性，最新发布一升级补丁，是否现在升级？");
    Update(document.getElementById("hidInstallerScript").value, "发现新版本，立即下载并手动安装。");
};

//切换登录模式
var checkLogin = function (_index) {
    var form = document.getElementsByClassName("loginform");
    for (var i = 0; i < form.length; i++) {
        form[i].setAttribute("class", "loginform dn");
        if (_index == i) {
            form[i].setAttribute("class", "loginform");
        };
    };
};

//在父窗口打开
if (self.parent.frames.length != 0) {
    self.parent.location = document.location.href;
}

//登录成功后写cookies
function SaveCookies(flag) {
    var _domain = window.location.host;
    var options = { expires: 31, path: '/', domain: _domain, secure: false, raw: false };

    if (!flag) { //登录失败时清除cookie
        $.cookie('BusinessAccount', null, options);
        $.cookie('UserAccount', null, options);
        $.cookie('RemeberPassword', null, options);
        $.cookie('UserPassword', null, options);
        $.cookie('UKPassword', null, options);
        return;
    }

    var RemeberPassword = $("#ckbRemeberPassword").attr("checked");

    $.cookie('BusinessAccount', $("#txtBusinessAccount").val(), options);
    $.cookie('UserAccount', $("#txtUserAccount").val(), options);
    $.cookie('RemeberPassword', RemeberPassword, options);
    if (RemeberPassword) {
        $.cookie('UserPassword', $("#txtUserPassword").val(), options);
        $.cookie('UKPassword', $("#txtUKPassword").val(), options);
    }
    else {
        $.cookie('UserPassword', null, options);
        $.cookie('UKPassword', null, options);
    }
}

//打开页面时加载cookies
function LoadCookies() {
    var timeout = 200;
    //普通登录
    if ($.cookie('BusinessAccount')) {
        $("#txtBusinessAccount").val($.cookie('BusinessAccount')).css("color", "#383838");
        if ($.cookie('UserAccount')) {
            $("#txtUserAccount").val($.cookie('UserAccount')).css("color", "#383838");
            if ($.cookie('RemeberPassword') && $.cookie('RemeberPassword') == "checked") {
                if ($.cookie('UserPassword')) {
                    $("#txtUserPassword").val($.cookie('UserPassword')).css("color", "#383838");
                    $("#txtCheck").focus();
                }
                else {
                    $("#txtUserPassword").focus();
                    setTimeout(function () { $("#txtUserPassword").val(""); }, timeout);
                }
                $("#ckbRemeberPassword").attr("checked", "checked")
            }
            else {
                $("#ckbRemeberPassword").removeAttr("checked")
            }
        }
        else {
            $("#txtUserAccount").focus();
            setTimeout(function () { $("#txtUserAccount").val(""); }, timeout);
        }
    }
    else {
        $("#txtBusinessAccount").focus();
        setTimeout(function () { $("#txtBusinessAccount").val(""); }, timeout);
    }
    //安全登录
    if ($.cookie('RemeberPassword') && $.cookie('RemeberPassword') == "checked") {
        if ($.cookie('UKPassword')) {
            $("#txtUKPassword").val($.cookie('UKPassword')).css("color", "#383838");
        }
        else {
            $("#txtUKPassword").focus();
            setTimeout(function () { $("#txtUKPassword").val(""); }, timeout);
        }
        $("#ckbRemeberPassword").attr("checked", "checked")
    }
    else {
        $("#ckbRemeberPassword").removeAttr("checked")
    }
}

//在表单时，回车提交
function NoSubmit(ev) {
    if (ev.keyCode == 13) {
        LcallServer();
        return false;
    }
    return true;
}


///登录
function LcallServer() {
    var speed = $("#hidSpeed").val();
    var serverDatetime = Date.parse(document.getElementById("hidServerTime").value);
    var difference = Math.abs(serverDatetime - clientDatetime);
    if (difference > 30 * 60 * 1000) {//相差半小时
        if (!confirm("您电脑显示时间不准确可能会影响到软件的操作，\n建议您校正您电脑时间，是否要继续登录？")) {
            return false;
        }
    }
    var isChrome = window.navigator.userAgent.indexOf("Chrome") != -1
    if (!isChrome) {
        if (!confirm("为了提高使用速度，推荐您先下载客户端再登录使用。\n确认继续使用当前浏览器登录？")) {
            return false;
        }
    }
    var para;
    var obj;
    var tabIndex = 0;
    var form = document.getElementsByClassName("current");
    if (form[0].textContent == "安全登录") {
        tabIndex = 1;
    }
    if (obj == "usb") {
        para = "USB" + separator + usbKeyPn;
        WebForm_DoCallback('__page', para, callResult, null, null, false);
    }
    else {
        if (tabIndex == "0") {
            document.getElementById("txtBusinessAccount").blur();
            document.getElementById("txtUserAccount").blur();
            document.getElementById("txtUserPassword").blur();
            document.getElementById("txtCheck").blur();
            var businessAccount = document.getElementById("txtBusinessAccount").value;
            var userAccount = document.getElementById("txtUserAccount").value;;
            var userPassword = document.getElementById("txtUserPassword").value;
            var TextCode = document.getElementById("txtCheck").value;
            if (document.getElementById("txtBusinessAccount").value.length == 0) {
                alert("账号不能为空！");
                document.getElementById("txtBusinessAccount").focus();
                return;
            }
            if (document.getElementById("txtUserAccount").value.length == 0) {
                alert("工号不能为空！");
                document.getElementById("txtUserAccount").focus();
                return;
            }
            if (document.getElementById("txtUserPassword").value.length == 0) {
                alert("密码不能为空！");
                document.getElementById("txtUserPassword").focus();
                return;
            }
            if (document.getElementById("txtCheck").value.length == 0) {
                alert("验证码不能为空！");
                document.getElementById("txtCheck").focus();
                return;
            }
            var rempwd = false;
            if (document.getElementById("ckbRemeberPassword").checked) {
                rempwd = true;
            }
            var hei = $(".login").eq(1).height();
            $("#loading").height(hei);
            document.getElementById("loading").style.display = "block";
            para = "LOGIN" + separator + "0" + separator + businessAccount + separator + userAccount + separator + hex_md5(userPassword) + separator
                     + TextCode + separator + rempwd + separator + speed;
            WebForm_DoCallback('__page', para, callResult, null, null, false);
        }
        else {
            if (document.getElementById('plugin').GetUsbKeySN == "0") {
                document.getElementById("txtUKUserAccount").value = "";
                document.getElementById("txtUKPassword").value = "";
                document.getElementById("txtUKBusinessAccount").value = "未检测到U-key";
                document.getElementById("rtMsg").innerHTML = "帐号、工号存储在U-key中，必须插入电脑才能登陆，大大提高安全性！";
                return;
            }
            if (document.getElementById("txtUKBusinessAccount").value != "未检测到U-key") {
                var usbPwd = document.getElementById("txtUKPassword").value;
                if (usbPwd.length == 0) {
                    alert("密码不能为空！");
                    return;
                }
                var rempwd = false;
                if (document.getElementById("ckbRemeberPassword").checked) {
                    rempwd = true;
                }
                para = "LOGIN" + separator + "1" + separator + UKBussinessAccount + separator + UKUserAccount + separator + hex_md5(usbPwd) + separator
                        + document.getElementById('plugin').GetHmacMd5Code(MD5Code) + separator + rempwd + separator + speed;
                document.getElementById("rtMsg").innerHTML = "正在验证USB-Key及密码...";
                WebForm_DoCallback('__page', para, callResult, null, null, false);
            }
        }
    }
}

//切换登录模式
function changeIndex(n) {
    if (n == "1") {
        document.getElementById("pluginObjDiv").innerHTML = "<object id=\"plugin\" type=\"application/x-sz1card1plugin\" width=\"0\" height=\"0\"></object>";
        var plugin = document.getElementById('plugin');
        if (plugin.GetUsbKeySN == undefined || plugin.GetUsbKeySN == "0") {
            document.getElementById("txtUKUserAccount").value = "";
            document.getElementById("txtUKPassword").value = "";
            document.getElementById("txtUKBusinessAccount").value = "未检测到U-key";
            document.getElementById("txtUKBusinessAccount").style.color = "#f00";
            document.getElementById("rtMsg").innerHTML = "帐号、工号存储在U-key中，必须插入电脑才能登陆，大大提高安全性！";
        }
        else {
            var para;
            var usbKeyPn = plugin.GetUsbKeySN;
            para = "USB" + separator + usbKeyPn;
            document.getElementById("rtMsg").innerHTML = "正在验证设备序列号...";
            WebForm_DoCallback('__page', para, callResult, null, null, false);
        }
    }
}


//全角转半角函数
function dbc2sbcm(str, flag) {
    var result = '';
    var str1 = document.getElementById(str).value.toString();
    str1 = str1.replace(/。/g, "．");
    for (var i = 0; i < str1.length; i++) {
        code = str1.charCodeAt(i);
        if (flag) {
            if (code >= 65281 && code <= 65373) result += String.fromCharCode(str1.charCodeAt(i) - 65248);
            else if (code == 12288) result += String.fromCharCode(str1.charCodeAt(i) - 12288 + 32);
            else result += str1.charAt(i);
        }
        else {
            if (code >= 33 && code <= 126) result += String.fromCharCode(str1.charCodeAt(i) + 65248);
            else if (code == 32) result += String.fromCharCode(str1.charCodeAt(i) - 32 + 12288);
            else result += str1.charAt(i);
        }
    }
    document.getElementById(str).value = result;
}

///检查是否升级
function Update(updateUrl, text) {
    if (updateUrl != "" || updateUrl == undefined) {
        alert(text);
        window.open(updateUrl);
    }
}

///登录验证后的回调
function callResult(arg) {
    document.getElementById("loading").style.display = "none";
    var args = arg.split(separator);
    if (args[0] == "") {
        alert("USB-Key验证失败!");
        document.getElementById("rtMsg").innerHTML = "";
        return;
    }
    switch (args[0]) {
        case "USB": //传入随机号到控件；
            document.getElementById("rtMsg").innerHTML = "";
            UKBussinessAccount = document.getElementById("txtUKBusinessAccount").value = args[1];
            UKUserAccount = document.getElementById("txtUKUserAccount").value = args[2];
            document.getElementById("txtUKPassword").focus();
            MD5Code = args[3];
            break;
        case "redirect":
            SaveCookies(true);
            document.getElementById("rtMsg").innerHTML = "";
            window.location.href = args[1];
            break;
        default:
            SaveCookies(false);
            document.getElementById("rtMsg").innerHTML = "";
            var msg = args[1].toString();
            if (msg.indexOf("验证码") > -1) {
                ChangeCodeImg('ImageCheckAdmin');
                document.getElementById("txtCheck").value = "";
                document.getElementById("txtCheck").focus();
            } else if (msg.indexOf("帐号") > -1) {
                document.getElementById("txtBusinessAccount").value = "";
                document.getElementById("txtBusinessAccount").focus();
            } else if (msg.indexOf("工号") > -1) {
                document.getElementById("txtUserAccount").value = "";
                document.getElementById("txtUserAccount").focus();
            } else if (msg.indexOf("密码") > -1) {
                document.getElementById("txtUserPassword").value = "";
                document.getElementById("txtUserPassword").focus();
                ChangeCodeImg('ImageCheckAdmin');
                document.getElementById("txtCheck").value = "";
            }
            alert(msg);
            break;
    }
}

///点击升级通知
$(".upgradetitle").click(function (e) {
    opareteUpgradePanel(e);
})

//关闭升级通知
$("#close").click(function (e) {
    opareteUpgradePanel(e);
})

//更多升级通知
$("#btnmore").click(function () {
    changeUpgradeNote();

})

//查看某条升级通知
$(".upgradeTitleList").live('click', function () {
    changeUpgradeNote();
    loadUpgradeDetail(this.id);
})

function changepictrue(n) {
    var img = document.getElementsByClassName("btn");
    if (n == 1) {
        img[0].src = "Images/login_down.png";
    }
    else {
        img[0].src = "Images/login.png";
    }
}

function showclose(img, n) {
    if (n == 1) {
        img.src = "Images/close_over.png";
    }
    else {
        img.src = "Images/close.png";
    }
}

$(function () {
    //兼容ie时的placeholder
    (function setTxt() {

        if ($.browser.chrome) {
            return false;
        };
        var txt = $(".txt"),
        len = txt.length;

        for (var i = 0; i < len; i++) {
            txt[i].value = txt[i].placeholder;
            txt[i].style.color = "#ccc";
        }
    }());

    //兼容ie时的placeholder
    $(".txt").focus(function () {
        if ($(this).val() == $(this).attr("placeholder")) {
            $(this).val('').css("color", "#ccc");
        }
        else {
            $(this).css("color", "##383838");
        }
    }).blur(function () {
        var that = $(this),
            val = that.val()
        placeholder = that.attr("placeholder");

        if (val.length < 1) {
            if (!$.browser.chrome) {
                that.val(placeholder).css("color", "#ccc");
            }
        } else {
            that.css('color', '#383838');
        }
    }).keyup(function () {
        $(this).css('color', '#383838'); ('color', '#383838')
    });
})

//升级通知明细的回调
function callResult1(arg) {
    unmask($("#upgradepanel"));
    var result = JSON.parse(arg);
    if (result.success) {
        $("#NoteTitle").html(result.title);
        $("#UpgradeTime").html(result.upgradetime);
        $("#NoteDetail").html(result.detail);
        $("#PublishTime").html(result.publishtime);
    }
    else {
        $("#NoteTitle").html("暂无升级通知");
        $("#UpgradeTime").html("");
        $("#NoteDetail").html("");
        $("#PublishTime").html("");
        $("#btnmore").css("display", "none");
    }
}

var times = 61;
//获取短信验证码回调
function callResult2(arg) {
    if (arg.indexOf("true") >= 0) {
        times = times - 1;
        if (times > 0) {
            if (times == 60) {
                document.getElementById("error1").innerText = "提示：验证码发送成功";
            }
            document.getElementById("txtAccount").readOnly = true;
            document.getElementById("txtPhone").readOnly = true;
            document.getElementById("GetCheckCode").value = times + "秒后重试";
            document.getElementById("GetCheckCode").disabled = true;
            setTimeout("callResult2('true')", 1000);
        }
        else {
            document.getElementById("txtAccount").readOnly = false;
            document.getElementById("txtPhone").readOnly = false;
            document.getElementById("GetCheckCode").value = "获取验证码";
            document.getElementById("GetCheckCode").disabled = false;

            document.getElementById("error2").innerText = "";
            $("#ImgCode").css("visibility", "hidden");

            times = 61;
        }
    }
    else {
        if (arg.indexOf("帐号不存在") < 0) {
            document.getElementById("error2").innerText = arg;
            document.getElementById("error1").innerText = "";
        }
        else {
            document.getElementById("error1").innerText = arg;
            document.getElementById("error2").innerText = "";
        }
        document.getElementById("GetCheckCode").disabled = false;
        document.getElementById("GetCheckCode").value = "获取验证码";
    }
}

//点下一步回调
function callResult4(arg) {
    document.getElementById("btnNext").disabled = false;
    document.getElementById("btnNext").value = "下 一 步";
    if (arg.indexOf("true") < 0) {
        document.getElementById("error2").innerText = arg;
        $("#ImgCode").css("visibility", "hidden");
    }
    else {
        $("#newPwd").show();
        $("#divCheckPhone").slideUp();
        document.getElementById("error2").innerText = "";
        $("#ImgCode").css("visibility", "visible")
    }
}

//点确定提交密码回调
function callResult5(arg) {
    $("#btnOK").val("确 认");
    document.getElementById("error3").innerText = arg;
}

//获取升级通知列表回调
function callResult6(arg) {
    unmask($("#upgradepanel"));
    var result = JSON.parse(arg);
    if (result.success) {
        var list = result.data;
        var innerhtml = "";
        if (list.length == 0) {
            innerhtml = "<div>暂无升级通知<div>";
        }
        else {
            for (var i = 0; i < list.length; i++) {
                var html = "<li>\
				               <h3><a id='{0}' class='upgradeTitleList'>{1}</a></h3>\
                               <p>升级日期：{2}</p>\
                               <p>升级内容：</p>\
                               <div>{3}</div>\
			                </li>";
                html = html.replace("{0}", list[i].Guid);
                html = html.replace("{1}", list[i].Title);
                html = html.replace("{2}", list[i].UpgradeTime);
                list[i].Detail = list[i].Detail.replace(/&amp;ldquo;/g, "\“").replace(/&amp;rdquo;/g, "\”").replace(/&amp;lsquo;/g, "\‘").replace(/&amp;rsquo;/g, "\’");
                list[i].Detail = list[i].Detail.length > 100 ? list[i].Detail.substring(0, 100) + "..." : list[i].Detail
                html = html.replace("{3}", list[i].Detail);
                innerhtml += html;
            }
        }
    }
    else {
        innerhtml = "<div>获取升级通知列表失败<div>";
    }
    $("#upgradelist").html(innerhtml);
}

$("#txtAccount").blur(function () {
    txtAccount_blur();
});

//打开找回密码框
$("#ReSetPassword").click(function (e) {
    var userAccount = $("#txtUserAccount");
    if (userAccount.val() != "10000") {
        alert("请联系系统管理员重置密码。");
        return;
    }
    var account = $("#txtBusinessAccount").val();
    var txtPhone = $("#txtPhone");
    txtPhone.val("");
    txtPhone.attr("readonly", false);
    txtPhone.css("color", "black");

    $.get("Login.aspx", { _action: "getTelNum", _account: account }, function (data) {
        if (data != "" && data != undefined && /^[0-9]*$/.test(data)) {
            txtPhone.val(data.substr(0, 3) + "****" + data.substr(7));
            txtPhone.attr("readonly", true);
            txtPhone.css("color", "gray");
            $("#telePhoneNumber").val(data);
        }
    });

    $("#upgradepanel").animate(
    {
        top: "-150%"
    }, "slow");
    $("#upgradepanel,#upgradelist").hide(1);

    var bodywidth = $("body").width() || document.body.clientWidth;
    var setpwdright = bodywidth - $("#loginid").offset().left + 30;
    $("#SetPwd").css({ "top": "-20%", "right": "35%", "width": "300px", "height": "310px" });
    $("#SetPwd").show(1);

    $("#SetPwd").animate(
    {
        top: "20%",
    }, "slow");

    var businessaccount = $("#txtBusinessAccount").val();
    if (businessaccount != "" && businessaccount != "请帐号") {
        $("#txtAccount").val(businessaccount).css('color', '#383838');
    }
    $("#txtNo").val("10000").css('color', '#383838');

    $("#checkphone").val("");
    $("#ImgCode").css("visibility", "hidden")
})

//关闭找回密码框
$("#close1").click(function () {
    var bodywidth = $("body").width() || document.body.clientWidth;
    var x = $("#ReSetPassword").offset().top
    var y = bodywidth - $("#ReSetPassword").offset().left - 30;
    $("#SetPwd").animate(
    {
        top: -150 + "%",
    }, "slow");
    $("#SetPwd,#newPwd").hide(1);
    $("#divCheckPhone").show();
    $("#error1").text("");
    $("#error2").text("");
    $("#error3").text("");
})

var txtAccount_blur = function () {
    var accont = $("#txtAccount").val().trim();
    if (accont.length == 0 && $("#SetPwd").css("display") != "none") {
        $("#error1").text("提示：请输入账号");
    }
    else { $("#error1").text(""); }
}
var txtPhone_blur = function () {
    dbc2sbcm("txtPhone", true);
    var reg = /(^1\d{10}$)/;
    var phone = $("#txtPhone").val().trim();
    if (phone.length > 0 && (phone.length != 11 || !reg.test(phone))) {
        $("#error2").text("提示：手机号码格式不正确！");
    }
    else if (phone.length == 0 && $("#SetPwd").css("display") != "none") {
        $("#error2").text("提示：请输入绑定的手机号码！");
    }
    else {
        if ($("#GetCheckCode").attr("disabled") != "disabled") {
            $("#error2").text("");
        }
    }
    if ($("#txtPhone").attr("readonly") == "readonly")
        $("#error2").text("");
}

//下一步
$("#btnNext").click(function () {
    txtAccount_blur();
    txtPhone_blur();
    if ($("#checkphone").val().trim().length != 0) {
        $("#btnNext").attr('disabled', true).val("正在提交...");
        callServer4($("#checkphone").val().trim());
    }
    else {
        if ($("#error2").text() == "") {
            $("#error2").text("提示：请输入验证码！");
        }
    }
})

$("#txtPhone").blur(function () { txtPhone_blur() });

///获取短信验证码 
$("#GetCheckCode").click(function () {
    txtAccount_blur();
    txtPhone_blur();
    //if ($("#txtAccount").val().trim().length > 0 && $("#txtPhone").val().trim().length > 0) {
    //    if ($("#error1").text().trim().length == 0 && $("#error2").text().trim().length == 0) {
    //        $("#GetCheckCode").attr('disabled', true).val("正在获取...");
    //        callServer2($("#txtAccount").val().trim(), $("#txtNo").val().trim(), $("#txtPhone").val().trim());
    //    }
    //}
    var phoneNumber = $("#txtPhone").val().trim();
    if (phoneNumber.indexOf("****") > 0) {
        phoneNumber = $("#telePhoneNumber").val().trim();
    }
    if ($("#txtAccount").val().trim().length > 0 && phoneNumber.length > 0) {
        if ($("#error1").text().trim().length == 0 && $("#error2").text().trim().length == 0) {
            $("#GetCheckCode").attr('disabled', true).val("正在获取...");
            callServer2($("#txtAccount").val().trim(), $("#txtNo").val().trim(), phoneNumber);
        }
    }
})

$("#txtNewPwd1").blur(function () {
    var newpwd = $("#txtNewPwd").val().trim();
    var newpwd1 = $("#txtNewPwd1").val().trim();
    var reg = /^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,20}$/;
    if (newpwd.length < 6) {
        $("#error3").text("提示：密码长度不能少于6位");
    }
    else {
        if (isAllDigit(newpwd)) {
            $("#error3").text("提示：密码不能全为数字");
        }
        else if (isAllLetter(newpwd)) {
            $("#error3").text("提示：密码不能全为字母");
        }
        else {
            if (newpwd == newpwd1) {
                $("#btnOK").attr('disabled', false);
                $("#error3").text("");
            }
            else {
                $("#error3").text("提示：密码不一致！");
            }
        }
    }
    O_color = "#ccc";
    L_color = "#f00";
    M_color = "#f90";
    S_color = "#3c0";
    if (newpwd == null || newpwd == '') {
        Lcolor = Mcolor = Scolor = O_color;
    }
    var slevel = checkStrong(newpwd);
    switch (slevel) {
        case 0:
            Lcolor = Mcolor = Scolor = O_color;
            break;
        case 1:
            Lcolor = L_color;
            Mcolor = Scolor = O_color;
            break;
        case 2:
            Lcolor = Mcolor = M_color;
            Scolor = O_color;
            break;
        case 3:
            Lcolor = Mcolor = Scolor = S_color;
            break;
        default:
            break;
    }
    $("#weak").css("background", Lcolor);
    $("#middle").css("background", Mcolor);
    $("#strong").css("background", Scolor);
})

//测试某个字符是属于哪一类. 
function CharMode(iN) {
    if (iN >= 48 && iN <= 57) //数字 
        return 1;
    if (iN >= 65 && iN <= 90) //大写字母 
        return 2;
    if (iN >= 97 && iN <= 122) //小写 
        return 4;
    else
        return 8; //特殊字符
}
//计算出当前密码当中一共有多少种模式 
function bitTotal(num) {
    modes = 0;
    for (i = 0; i < 4; i++) {
        if (num & 1) modes++;
        num >>>= 1;
    }
    return modes;
}
//返回密码的强度级别 
function checkStrong(sPW) {
    if (sPW.length <= 5)
        return 0; //密码太短 
    Modes = 0;
    for (i = 0; i < sPW.length; i++) {
        //测试每一个字符的类别并统计一共有多少种模式. 
        Modes |= CharMode(sPW.charCodeAt(i));
    }
    return bitTotal(Modes);
}


//弹出密码小键盘
$(".key").click(function () {
    var that;
    switch (this.id) {
        case "pwdkey1":
            that = $("#txtNewPwd");
            break;
        case "pwdkey2":
            that = $("#txtNewPwd1");
            break;
        case "pwdkey3":
            that = $("#txtUserPassword");
            break;
        case "pwdkey4":
            that = $("#txtUKPassword");
            break;
        default:
            break;
    }
    var id = that.attr("id"),
            x = that.offset().left,
            y = that.offset().top + 38;
    if (!keyenable) {
        keyenable = true;
        $(this).addClass("key1");
    }
    else {
        keyenable = false;
        $(this).removeClass("key1");
    }
    var maxwidth = $("body").width();
    if (maxwidth < x + 335) {
        $("#softkey").css("right", "0px");
    }
    else {
        $("#softkey").css("left", x + "px");
    }
    $("#softkey").css("top", y + "px");
    VirtualKeyboard.toggle(id, 'softkey');
    $("#kb_langselector,#kb_mappingselector,#copyrights").css("display", "none");
})

//隐藏密码小键盘
$("#txtUserPassword,#txtUKPassword,#txtNewPwd,#txtNewPwd1").on('blur', function () {
    if (keyenable) {
        VirtualKeyboard.toggle(this.id, 'softkey');
        $(".key").removeClass("key1");
        keyenable = false;
    }
    dbc2sbcm($(this).attr("id"), true);
})


///确定修改密码
$("#btnOK").click(function () {
    if ($("#error3").text().trim().length == 0 && $("#txtNewPwd").val().trim().length >= 6 && $("#txtNewPwd1").val().trim().length >= 6) {
        $("#btnOK").attr('disabled', true).val("正在提交...");
        callServer5($("#txtAccount").val().trim(), $("#txtNo").val().trim(), hex_md5($("#txtNewPwd").val().trim()), hex_md5($("#txtNewPwd1").val().trim()), $("#txtPhone").val());
    }
})


//全部输入框全角转半角
$("input:text").on('blur', function () {
    dbc2sbcm($(this).attr("id"), true);
})

var mask = function (obj, text) {
    if (obj == undefined || obj == null) {
        return;
    }
    if (text == undefined || text == "") {
        text = "正在加载...";
    }
    var maskDiv = document.createElement("div");
    var id = "mask_" + obj.id;
    maskDiv.id = id;
    maskDiv.className = "ui-mask";
    maskDiv.innerHTML = "<img src='Images/icons/loading.gif'style='vertical-align: middle' /> <span></span>";
    maskDiv.style.zIndex = 20000;
    maskDiv.style.display = "none";
    document.body.appendChild(maskDiv);
    $("#" + id).css("top", obj.offset().top);
    $("#" + id).css("left", obj.offset().left);
    $("#" + id).css("width", obj.outerWidth());
    $("#" + id).css("height", obj.outerHeight());
    $("#" + id).css("line-height", obj.outerHeight() + "px");
    $("#" + id + ">span").html(text);
    $("#" + id).show();
}

var unmask = function (obj) {
    if (obj == undefined || obj == null) {
        return;
    }
    $("#mask_" + obj.id).remove();
}

//获取升级通知列表
var getUpgradeList = function () {
    mask($("#upgradepanel"), "正在加载...");
    callServer6();
}

//打开、关闭升级通知对话框
var opareteUpgradePanel = function (e) {
    var isshow = $("#upgradepanel").css("display");
    if (isshow == "none") {
        $("#upgradepanel").css({ "top": -20 + "%", "left": 25 + "%", "width": "50%", "height": "66%" });
        $("#upgradepanel").show();
        $("#upgradepanel").animate(
        {
            top: "20%"
        }, "slow", function () {
            loadUpgradeDetail(e.currentTarget.id);
        });
    }
    else {
        $("#upgradepanel").animate(
        {
            top: -150 + "%",
        }, "slow", function () {
            $("#upgradepanel").hide(1);
        });
    }
}

//加载升级通知明细
var loadUpgradeDetail = function (guid) {
    mask($("#upgradepanel"), "正在加载...");
    callServer1(guid);
}

//升级通知明细和列表间的切换
var changeUpgradeNote = function () {
    if ($("#upgradedetail").css("display") != "none") {
        $("#upgradelist").show();
        $("#upgradedetail").hide();
        $("#btnmore").html("返回");
        getUpgradeList();
    }
    else {
        $("#upgradelist").hide();
        $("#upgradedetail").show();
        $("#btnmore").html("更多");
    }
}


//是否全为数字
var isAllDigit = function (str) {
    var reg = /^[0-9]*$/;
    if (reg.test(str)) {
        return true;
    }
    else {
        return false;
    }
}

//是否全为字母
var isAllLetter = function (str) {
    var reg = /^[A-Za-z]+$/;
    if (reg.test(str)) {
        return true;
    }
    else {
        return false;
    }
}

$("#imgBg").load(function () {
    var filesize = 86.8;    //measured in KB   
    var et = new Date();
    var speed = Math.round(filesize * 1000) / (et - clientDatetime);
    $("#hidSpeed").val(speed);
})