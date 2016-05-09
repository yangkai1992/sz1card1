var isCheckAccount = false;
function getData() {
    return {
        Action: "Register",
        BusinessAccount: $("#businessAccount").val(),
        Company: $("#companyName").val(),
        Service: $("#primaryService").val(),
        Contact: $("#contact").val(),
        QQ: $("#QQ").val(),
        Mobile: $("#mobile").val(),
        MobileCheckCode: $("#MobileCheckCode").val(),
        Saler: $getParam["Saler"],
        SecretKey: $("#hidSecretKey").val()
    };
}
$("#btnAgentRegister").click(function () {
    var txt = $(".txt");
    if (!isCheckAccount)
        return false;
    for (var i = 1; i < txt.length; i++) {
        if (!validate($(txt[i]), false)) {
            return false;
        }
    }
    if ($("#businessAccount").val().length > 12) {
        alert("帐号长度不能超过12个字符！");
        return false;
    }
    var btn = $("#btnAgentRegister");
    btn.val("正在注册...");
    btn.attr("disabled", true).addClass("disabled")
    var data = getData();
    ajaxRequest("AgentHandler.ashx", data, "POST", function (data) {
        if (data.success) {
            document.getElementById("step1").style.display = "none";
            document.getElementById("step2").removeAttribute("style");
            document.getElementById("spanAccount").innerHTML = "登录账号：" + data.account;
            document.getElementById("spanPassword").innerHTML = "登录密码：" + data.password;
            document.getElementById("editionInfo").innerHTML = data.editInfo;
        }
        else
            alert(data.message);
        btn.val("立即注册");
        btn.removeAttr("disabled").removeClass("disabled");
        ChangeCodeImg("authenticode");
    });
});

//ajax请求
function ajaxRequest(url, data, method, fn) {
    var request = new XMLHttpRequest();
    request.open(method, url, true);
    request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded;charset=utf-8");
    request.onreadystatechange = function () {
        if (request.readyState == 4 && request.status == 200)
            fn(eval('(' + request.responseText + ')'));
        else if (request.readyState == 4 && request.status == 500) {
            console.log(request.responseText);
            alert("未知错误");
            return false;
        }
    }
    var str = "";
    if (typeof data == 'object') {
        for (var d in data) {
            str += d + "=" + data[d] + "&";
        }
        str = str.substr(0, str.length - 1);
    }
    else
        str = data;
    request.send(str);
}
//获取焦点事件
$("#step1 .txt").focus(function () {
    $(this).next().children(".tips").css("display", "inline-block").siblings().css("display", "none");
});
//离开焦点事件
$("#step1 .txt").blur(function () {
    validate($(this), true);
});
//验证表单数据的合法性
var validate = function (obj, ispostback) {
    var id = obj.attr("id");
    //去掉前后空格
    var txt = obj.val().replace(/(^[\s\xA0]+|[\s\xA0]+$)/g, '');
    //全角转为半角
    //txt = dbc2sbcm(txt, true);
    obj.val(txt);
    var error = obj.siblings("p").children(".error");
    var minlength = obj.attr("minlength") == undefined ? 0 : obj.attr("minlength");
    var maxlength = obj.attr("maxlength");
    if (txt.length == 0) {
        error.children("span").html("不能为空");
        error.css("display", "inline-block").siblings().css("display", "none");
        return false;
    }
    if (txt.length < minlength) {
        error.children("span").html(minlength + "-" + maxlength + "个字符");
        error.css("display", "inline-block").siblings().css("display", "none");
        return false;
    }
    switch (id) {
        case "mobile":
            return checkMobile(obj);
        case "businessAccount":
            return checkAccount(obj);
        default:
            if (!$("#" + id).val() == "")
                obj.siblings("p").children(".success").css("display", "inline-block").siblings().css("display", "none");
            break;
    }
    return true;
}
function checkAccount(obj) {
    var error = obj.siblings("p").children(".error");
    var account = obj.val();
    if (!isDigitOrLetter(account)) {
        error.children("span").html("账号必须是数字或者字母");
        error.css("display", "inline-block").siblings().css("display", "none");
        return false;
    }
    if (account.length < 4) {
        error.children("span").html("4~12个字符");
        error.css("display", "inline-block").siblings().css("display", "none");
        return false;
    }
    if (account.length > 12) {
        error.children("span").html("4~12个字符");
        error.css("display", "inline-block").siblings().css("display", "none");
        return false;
    }
    var data = {
        Action: "CheckAccount",
        Account: account,
        SecretKey: $("#hidSecretKey").val()
    };
    ajaxRequest("AgentHandler.ashx", data, "POST", function (data) {
        if (data.success) {
            obj.siblings("p").children(".success").css("display", "inline-block").siblings().css("display", "none");
            isCheckAccount = true;
            return true;
        }
        isCheckAccount = false;
        error.children("span").html("此账号已被使用");
        error.css("display", "inline-block").siblings().css("display", "none");
    });
}
//检测手机号
var checkMobile = function (obj) {
    var value = obj.val();
    var btnCheckcode = $("#btnCheckcode");
    if (!isMobile(value)) {
        var error = obj.siblings("p").children(".error");
        error.children("span").html("手机号格式不正确");
        error.css("display", "inline-block").siblings().css("display", "none");
        btnCheckcode.attr("disabled", true).addClass("disabled");
        return false;
    }
    obj.siblings("p").children(".success").css("display", "inline-block").siblings().css("display", "none");
    return true;
}
//是否为手机号
var isMobile = function (str) {
    var reg = /^1\d{10}$/;
    if (reg.test(str))
        return true;
    else
        return false;
}
//是否为数字或字母
var isDigitOrLetter = function (str) {
    var reg = /^[A-Za-z0-9]+$/;
    if (reg.test(str)) {
        return true;
    }
    else {
        return false;
    }
}
//验证QQ。
var isQQNumber = function () {
    var qq = $("#QQ");
    var reg = /^[0-9]+$/
    if (reg.test(qq.val()))
        return true;
    var error = qq.siblings("p").children(".error");
    error.children("span").html("QQ号码只能输入数字");
    error.css("display", "inline-block").siblings().css("display", "none");
    return false;
}
var $getParam = (function () {
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
var showPrivacy = function () {
    window.open('../../company/privacy.aspx', 'newwindow', 'height=600,width=600,top=0,left=0,toolbar=no,menubar=no,scrollbars=no, resizable=no,location=no, status=no');
    return false;
}

function timeDescend(obj) {
    var time = 60;
    obj.attr("disabled", true).addClass("disabled");
    var intervalID = setInterval(function () {
        obj.val(time + "秒后重发");
        if (time == 0) {
            clearInterval(intervalID);
            if (flag)
                obj.removeAttr("disabled").removeClass("disabled");
            obj.val("获取验证码");
        }
        time--;
    }, 1000);
}