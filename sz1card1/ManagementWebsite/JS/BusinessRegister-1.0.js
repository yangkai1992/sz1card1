$(function () {
    (function ($) {
        $.getUrlParam = function (name) {
            var reg = new RegExp("(^|&)" + name.toLocaleLowerCase() + "=([^&]*)(&|$)");
            var r = window.location.search.toLocaleLowerCase().substr(1).match(reg);
            if (r != null)
                return unescape(r[2]);
            return null;
        }
    })(jQuery);

    SetBodyHeight();
    //获取焦点事件
    $("#step1 .txt").focus(function () {
        $(this).siblings("p").children(".tips").css("display", "inline-block").siblings().css("display", "none");
    });
    //离开焦点事件
    $("#step1 .txt").blur(function () {
        validate($(this), true);
    });

    //离开焦点事件
    $("#step2 .txt").blur(function () {
        var txt = $(this).val().replace(/(^[\s\xA0]+|[\s\xA0]+$)/g, '');
        //全角转为半角
        $(this).val(dbc2sbcm(txt, true));
    });

    //手机号文本框按键事件
    $("#mobile").keyup(function () {
        var mobile = $("#mobile").val();
        var btnCheckcode = $("#btnCheckcode");
        if (isMobile(mobile)) {
            debugger
            var val = btnCheckcode.html().replace(/(^[\s\xA0]+|[\s\xA0]+$)/g, '')
            var textCode = $("#textcode");
            checkTextCode(textCode)
            //if (val == "获取验证码" && checkTextCode(textCode)) //检查图形校验码是否正确
            //    btnCheckcode.removeAttr("disabled").removeClass("disabled");                       
        }
        else {
            btnCheckcode.attr("disabled", true).addClass("disabled");
        }
    });
    //图形验证码文本框按键事件
    $("#textcode").keyup(function () {
        var textCode = $("#textcode");
        var btnCheckcode = $("#btnCheckcode");
        if (!checkTextCode(textCode)) {
            btnCheckcode.attr("disabled", true).addClass("disabled");
        }
        //if (checkTextCode(textCode)) { //检查图形校验码是否正确 
        //    var mobile = $("#mobile").val();            
        //    if (isMobile(mobile)) {
        //        var val = btnCheckcode.html().replace(/(^[\s\xA0]+|[\s\xA0]+$)/g, '')
        //        if (val == "获取验证码")                   
        //            btnCheckcode.removeAttr("disabled").removeClass("disabled");
        //    }
        //}
        //else {
        //    btnCheckcode.attr("disabled", true).addClass("disabled");
        //}           
    });

    //获取验证码单击事件
    $("#btnCheckcode").click(function () {
        var txt = $(".txt");
        for (var i = 0; i < txt.length; i++) {
            if ($(txt[i]).attr("id") != "checkcode" && !validate($(txt[i]), true)) {
                return false;
            }
        }

        var btnCheckcode = $("#btnCheckcode");
        var mobile = $("#mobile").val();
        btnCheckcode.html("正在获取...");
        btnCheckcode.attr("disabled", true).addClass("disabled");
        AjaxControlRequest({
            id: "GetCheckCode",
            request: function () {
                return {
                    Mobile: mobile,
                    SecretKey: $("#hidSecretKey").val()
                }
            },
            callback: function (result) {
                if (result.success) {
                    $("#mobile").attr("readonly", true).addClass("readonly");
                    $("#account").attr("readonly", true).addClass("readonly");
                    RegisterBusiness(false);
                    timeDescend(btnCheckcode);
                }
                else {
                    btnCheckcode.html("获取验证码");
                    btnCheckcode.removeAttr("disabled").removeClass("disabled");
                    alert(result.message);
                }
            }
        });
    });

    //商家注册单击事件
    $("#btnRegister").click(function () {
        var txt = $(".txt");
        for (var i = 0; i < txt.length; i++) {
            if (!validate($(txt[i]), false)) {
                return false;
            }
        }
        if ($("#account").val().length > 12) {
            alert("帐号长度不能超过12个字符！");
            return false;
        }

        var btnRegister = $("#btnRegister");
        btnRegister.html("正在注册...");
        btnRegister.attr("disabled", true).addClass("disabled");
        AjaxControlRequest({
            id: "Register",
            request: function () {
                return {
                    BusinessName: $("#name").val(),
                    BusinessAccount: $("#account").val(),
                    BusinessPwd: $("#password").val(),
                    BusinessContact: $("#contact").val(),
                    ContactSex: $('input:radio[name="sex"]:checked').val(),
                    Mobile: $("#mobile").val(),
                    CheckCode: $("#checkcode").val(),
                    SecretKey: $("#hidSecretKey").val()
                }
            },
            callback: function (result) {
                if (result.success) {
                    var url = "BusinessRegister.aspx?SID=" + result.sid + "&step=2";
                    if ($.getUrlParam('Agent') != null && $.getUrlParam('Agent') != "") {
                        url = url + "&Agent=" + $.getUrlParam('Agent');
                    }
                    if ($.getUrlParam('AgentSaler') != null && $.getUrlParam('AgentSaler') != "") {
                        url = url + "&AgentSaler=" + $.getUrlParam('AgentSaler');
                    }
                    if ($.getUrlParam('SourceId') != null && $.getUrlParam('SourceId') != "") {
                        url = url + "&SourceId=" + $.getUrlParam('SourceId');
                    }
                    if ($.getUrlParam('Saler') != null && $.getUrlParam('Saler') != "") {
                        url = url + "&Saler=" + $.getUrlParam('Saler');
                    }
                    if ($.getUrlParam('ref') != null && $.getUrlParam('ref') != "") {
                        url = url + "&ref=" + $.getUrlParam('ref');
                    }
                    window.location = url;
                }
                else {
                    alert(result.message);
                }
                btnRegister.html("立即注册");
                btnRegister.removeAttr("disabled").removeClass("disabled");
            }
        });
    });

    //选择注册版本事件
    $("#ddlEdition").change(function () {
        getEditionDesc();
    });

    if ($("#hidFirstLoad").val() == "false") {
        $("#btnCheckcode").removeAttr("disabled").removeClass("disabled");
        var txt = $(".txt");
        for (var i = 0; i < txt.length; i++) {
            if (!validate($(txt[i]), true)) {
                return false;
            }
        }
    }
})

var RegisterBusiness = function (ischeckcode) {
    var txt = $(".txt");
    for (var i = 0; i < txt.length; i++) {
        //不验证验证码
        if ((ischeckcode || $(txt[i]).attr("id") != "checkcode") && !validate($(txt[i]), true)) {
            return false;
        }
    }
    AjaxControlRequest({
        id: "RegBusiness",
        request: function () {
            return {
                BusinessName: $("#name").val(),
                BusinessAccount: $("#account").val(),
                BusinessPwd: $("#password").val(),
                BusinessContact: $("#contact").val(),
                ContactSex: $('input:radio[name="sex"]:checked').val(),
                Mobile: $("#mobile").val(),
                SecretKey: $("#hidSecretKey").val()
            }
        },
        callback: function (result) {
            if (result.success) {
                timeDescend(btnCheckcode);
            }
            else {
                btnCheckcode.html("获取验证码");
                btnCheckcode.removeAttr("disabled").removeClass("disabled");
                alert(result.message);
            }
        }
    });
}

var timeDescend = function (obj) {
    var time = 60;
    obj.attr("disabled", true).addClass("disabled");
    var intervalID = setInterval(function () {
        obj.html(time + "秒后重发");
        if (time == 0) {
            clearInterval(intervalID);
            obj.removeAttr("disabled").removeClass("disabled");
            obj.html("获取验证码");
        }
        time--;
    }, 1000);
}

//验证表单数据的合法性
var validate = function (obj, ispostback) {
    var id = obj.attr("id");
    //去掉前后空格
    var txt = obj.val().replace(/(^[\s\xA0]+|[\s\xA0]+$)/g, '');
    //全角转为半角
    txt = dbc2sbcm(txt, true);

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
        case "name":
            return checkBusinessName(obj);
        case "account":
            return checkAccount(obj, ispostback);
        case "password":
            return checkPassword(obj);
        case "confirmpwd":
            return confirmpassword(obj);
        case "mobile":
            return checkMobile(obj);
        case "textcode":
            return checkTextCode(obj);
        default:
            obj.siblings("p").children(".success").css("display", "inline-block").siblings().css("display", "none");
            break;
    }
    return true;
}


//验证图形验证码
var checkTextCode = function (obj) {
    var value = obj.val();
    var btnCheckcode = $("#btnCheckcode");
    var error = obj.siblings("p").children(".error");
    var reg = /^\d{4}$/;
    if (!reg.test(value)) {
        error.children("span").html("图片验证码输入错误");
        error.css("display", "inline-block").siblings().css("display", "none");
        return false;
    }
    AjaxControlRequest({
        id: "CheckTextCode",
        request: function () {
            return {
                TextCode: value
            }
        },
        callback: function (result) {
            if (result.success) {
                // error.css("display", "inline-block").siblings().css("display", "none");
                obj.siblings("p").children(".success").css("display", "inline-block").siblings().css("display", "none");

                var mobile = $("#mobile").val();
                if (isMobile(mobile)) {     //图形验证成功后验证手机号是否正确                          
                    var val = btnCheckcode.html().replace(/(^[\s\xA0]+|[\s\xA0]+$)/g, '');
                    if (val == "获取验证码")
                        btnCheckcode.removeAttr("disabled").removeClass("disabled");
                }
                else {
                    btnCheckcode.attr("disabled", true).addClass("disabled");
                };
            }
            else {
                error.children("span").html(result.message);
                error.css("display", "inline-block").siblings().css("display", "none");
                btnCheckcode.attr("disabled", true).addClass("disabled");
            }
        }
    });
    return true;
}
//验证商家名称
var checkBusinessName = function (obj) {
    var value = obj.val();
    var error = obj.siblings("p").children(".error");
    var re = /^[\u4E00-\u9FA5a-zA-Z0-9_]{1,20}$/;
    if (!re.test(value)) {
        error.children("span").html("商家名称只允许输入汉字、字母、数字和下划线");
        error.css("display", "inline-block").siblings().css("display", "none");
        return false;
    }
    obj.siblings("p").children(".success").css("display", "inline-block").siblings().css("display", "none");
    return true;
}

//检测商家账号
var checkAccount = function (obj, ispostback) {
    var value = obj.val();
    var error = obj.siblings("p").children(".error");
    if (!isDigitOrLetter(value)) {
        error.children("span").html("登录账号不合法，只能包含字母和数字");
        error.css("display", "inline-block").siblings().css("display", "none");
        return false;
    }
    var acountattr = $("#account").attr("readonly");
    if (ispostback && !acountattr) {
        var loading = obj.siblings("p").children(".loading");
        loading.css("display", "inline-block").siblings().css("display", "none");
        AjaxControlRequest({
            id: "CheckBusinessAccount",
            request: function () {
                return {
                    Account: value,
                    SecretKey: $("#hidSecretKey").val()
                }
            },
            callback: function (result) {
                if (result.success) {
                    obj.siblings("p").children(".success").css("display", "inline-block").siblings().css("display", "none");
                }
                else {
                    var error = obj.siblings("p").children(".error");
                    error.children("span").html(result.message);
                    error.css("display", "inline-block").siblings().css("display", "none");
                }
            }
        });
    }
    else {
        obj.siblings("p").children(".success").css("display", "inline-block").siblings().css("display", "none");
    }
    return true;
}

//验证确认密码
var confirmpassword = function (obj) {
    var txt = obj.val();
    var pwd = $("#password").val();
    if (txt != pwd) {
        var error = obj.siblings("p").children(".error");
        error.children("span").html("两次密码输入不一致");
        error.css("display", "inline-block").siblings().css("display", "none");
        return false;
    }
    obj.siblings("p").children(".success").css("display", "inline-block").siblings().css("display", "none");
    return true;
}

//检测密码格式
var checkPassword = function (obj) {
    var value = obj.val();
    if (isAllDigit(value)) {
        var error = obj.siblings("p").children(".error");
        error.children("span").html("不能全为数字");
        error.css("display", "inline-block").siblings().css("display", "none");
        return false;
    }
    if (isAllLetter(value)) {
        var error = obj.siblings("p").children(".error");
        error.children("span").html("不能全为字母");
        error.css("display", "inline-block").siblings().css("display", "none");
        return false;
    }
    obj.siblings("p").children(".success").css("display", "inline-block").siblings().css("display", "none");
    return true;
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
    else {
        obj.siblings("p").children(".success").css("display", "inline-block").siblings().css("display", "none");
        var textCode = $("#textcode");
        if (!checkTextCode(textCode)) //验证图形校验码
        {
            return false;
        }
    }
    var val = btnCheckcode.html().replace(/(^[\s\xA0]+|[\s\xA0]+$)/g, '')
    if (val == "获取验证码")
        btnCheckcode.removeAttr("disabled").removeClass("disabled");
    return true;
}

//获取版本描述
var getEditionDesc = function () {
    var keyName = $("#ddlEdition").val();
    var editionDesc = $("#editionDesc");
    if (keyName == "") {
        editionDesc.html("请选择注册版本");
        return;
    }
    editionDesc.html("正在加载版本信息..");
    AjaxControlRequest({
        id: "GetEditionDesc",
        request: function () {
            return { KeyName: keyName }
        },
        callback: function (result) {
            if (result.success) {
                editionDesc.html(result.desc);
            }
        }
    });
}

//全角转半角函数
function dbc2sbcm(str, flag) {
    var result = '';
    str = str.replace(/。/g, "．");
    for (var i = 0; i < str.length; i++) {
        code = str.charCodeAt(i);
        if (flag) {
            if (code >= 65281 && code <= 65373) result += String.fromCharCode(str.charCodeAt(i) - 65248);
            else if (code == 12288) result += String.fromCharCode(str.charCodeAt(i) - 12288 + 32);
            else result += str.charAt(i);
        }
        else {
            if (code >= 33 && code <= 126) result += String.fromCharCode(str.charCodeAt(i) + 65248);
            else if (code == 32) result += String.fromCharCode(str.charCodeAt(i) - 32 + 12288);
            else result += str.charAt(i);
        }
    }
    return result;
}

//是否为中文的判断
var isContainChn = function (str) {
    var reg = /^[\u4E00-\u9FA5]+$/;
    for (var i = 0; i < str.length; i++) {
        var s = str.charAt(i);
        if (reg.test(s)) {
            return true;
        }
    }
    return false;
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

//是否为手机号
var isMobile = function (str) {
    var reg = /^1\d{10}$/;
    if (reg.test(str)) {
        return true;
    }
    else {
        return false;
    }
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

//必须同时包含数字和字母
var isDigitAndLetter = function (str) {
    var reg = /^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,20}$/;
    if (reg.test(str)) {
        return true;
    }
    else {
        return false;
    }
}

var showPrivacy = function () {
    window.open('../../company/privacy.aspx', 'newwindow', 'height=600,width=600,top=0,left=0,toolbar=no,menubar=no,scrollbars=no, resizable=no,location=no, status=no');
    return false;
}

//浏览器大小
var i = 0;
$(window).resize(function () {
    i++;
    SetBodyHeight();
    if (i > 10) {
        i = 0;
        return;
    }
});

var SetBodyHeight = function () {
    var height = $(window).height();
    height = height - 77 - 54 - 40;
    $(".content").css("height", height + "px");
}
function checkQQ() {
    var reg = /^[0-9]+$/
    if (reg.test($("#linkqq").val())) 
        return true;
    alert("QQ号码只能由数字组成");
    return false;
}