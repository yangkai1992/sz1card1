//-----------------------------------------------------------------------

// <copyright file="sz1card1.Vallidator.cs" company="sz1card1">

//     Copyright (c) sz1card1 All rights reserved.

// </copyright>

// <author>hjw</author>

// <date>2014-08-26</date>

//-----------------------------------------------------------------------

//去除左侧空格
function LTrim(str) {
    return str.replace(/^\s*/g, "");
}

//去右空格
function RTrim(str) {
    return str.replace(/\s*$/g, "");
}

//去掉字符串两端的空格
function trim(str) {
    return str.replace(/(^\s*)|(\s*$)/g, "");
}

//去除字符串中间空格
function CTim(str) {
    return str.replace(/\s/g, '');
}

//是否为由数字组成的字符串(包括正负小数)
function isNumber(str) {
    var reg = /^(-|\+)?\b[0-9]*\.?[0-9]+\b$/;//匹配整数
    return reg.test(str);
}

//是否为由数字组成的字符串
function is_digitals(str) {
    var reg = /^[0-9]*$/;//匹配整数
    return reg.test(str);
}

//验证是否为整数，包括正负数；
function Is_Int(str) {
    var reg = /^(-|\+)?\d+$/;
    return reg.test(str);
}

//是大于0的整数
function Is_positive_num(str) {
    var reg = /^\d+$/;
    return reg.test(str);
}

//负整数的验证
function Is_minus(str) {
    var reg = /^-\d+$/;
    return reg.test(str);
}

//验证是否为浮点数（正数）
function IsPositiveFloat(str) {
    var check_float = new RegExp("^[1-9][0-9]*\.[0-9]+$");//匹配浮点数
    return check_float.exec(str);
}

//验证是否为浮点数，包括正负数；
function IsFloat(str) {
    var check_float = new RegExp("^(-|\+)?[1-9][0-9]*\.[0-9]+$");//匹配浮点数
    return check_float.exec(str);
}

//是否为固定电话，区号3到4位，号码7到8位,区号和号码用"－"分割开，转接号码为1到6位，用小括号括起来紧跟在号码后面
function IsTelphone(str) {
    var reg = /^[0-9]{3,4}\-\d{7,8}(\(\d{1,6}\))?$/;

    if (reg.test(str))
        return true;
    else
        return false;
}

//手机号码验证，验证13系列和158，159几种号码，长度11位
function IsMobel(str) {
    var reg0 = /^13\d{9}$/;
    var reg1 = /^158\d{8}$/;
    var reg2 = /^159\d{8}$/;

    return (reg0.test(str) || reg1.test(str) || reg2.test(str))
}

//验证是否为中文
function IsChinese(str) {
    var reg = /^[\u0391-\uFFE5]+$/;
    return reg.test(str);
}

//验证是否为qq号码，长度为5－10位
function IsQq(str) {
    var reg = /^[1-9]\d{4,9}$/;
    return reg.test(str);
}

//验证邮编
function IsPostId(str) {
    var reg = /^\d{6}$/;
    return reg.test(str);
}

//验证是否未email
function IsEmail(str) {
    var reg = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
    return reg.test(str);
}

//验证IP地址
function IsIp(str) {
    var check = function (v) {
        try {
            return (v <= 255 && v >= 0)
        } catch (x) {
            return false;
        }
    }
    var re = str.split(".")
    return (re.length == 4) ? (check(re[0]) && check(re[1]) && check(re[2]) && check(re[3])) : false
}

//身份证验证
function IsIdnum(str) {
    var City = {
        11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江 ",
        31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北 ",
        43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏 ",
        61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外 "
    }
    var iSum = 0
    var info = ""
    if (!/^\d{17}(\d|x)$/i.test(str))
        return false;
    str = str.replace(/x$/i, "a");
    if (City[parseInt(str.substr(0, 2))] == null) {
        alert("Error:非法地区");
        return false;
    }
    sBirthday = str.substr(6, 4) + "-" + Number(str.substr(10, 2)) + "-" + Number(str.substr(12, 2));
    var d = new Date(sBirthday.replace(/-/g, "/"))
    if (sBirthday != (d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate())) {
        alert("Error:非法生日");
        return false;
    }
    for (var i = 17; i >= 0; i--)
        iSum += (Math.pow(2, i) % 11) * parseInt(str.charAt(17 - i), 11)
    if (iSum % 11 != 1) {
        alert("Error:非法证号");
        return false;
    }
    return City[parseInt(str.substr(0, 2))] + "," + sBirthday + "," + (str.substr(16, 1) % 2 ? "男" : "女")
}

//判断是否短时间，形如 (13:04:06)
function IsTime(str) {
    var a = str.match(/^(\d{1,2})(:)?(\d{1,2})\2(\d{1,2})$/);
    if (a == null) {
        alert('输入的参数不是时间格式'); return false;
    }
    if (a[1] > 24 || a[3] > 60 || a[4] > 60) {
        alert("时间格式不对");
        return false
    }
    return true;
}

//短日期，形如 (2003-12-05)
function IsDate(str) {
    var r = str.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
    if (r == null)
        return false;
    var d = new Date(r[1], r[3] - 1, r[4]);
    return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4]);
}

// 长时间，形如 (2003-12-05 13:04:06)
function IsDateTime(str) {
    var reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/;
    var r = str.match(reg);
    if (r == null)
        return false;
    var d = new Date(r[1], r[3] - 1, r[4], r[5], r[6], r[7]);
    return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4] && d.getHours() == r[5] && d.getMinutes() == r[6] && d.getSeconds() == r[7]);
}

// 判断字符全部由a-Z或者是A-Z的字字母组成
function Is_Letters(str) {
    var reg = /[^a-zA-Z]/g;
    return reg.test(str);
}

// 判断字符由字母和数字组成。
function Is_letter_num(str) {
    var reg = /[^0-9a-zA-Z]/g;
    return reg.test(str);
}

//判断字符由字母和数字，下划线,点号组成.且开头的只能是下划线和字母
function IsUserName(str) {
    var reg = /^([a-zA-z_]{1})([\w]*)$/g;
    return reg.test(str);
}

// 判断浏览器的类型
function GetBrowseType() {
    alert(window.navigator.appName);
}

//判断ie的版本
function Get_Eidition() {
    alert(window.navigator.appVersion);
}

//判断客户端的分辨率
function GetResolution() {
    alert(window.screen.height);
    alert(window.screen.width);
}

// 判断用户名是否为数字字母下滑线 
function notchinese(str) {
    var reg = /[^A-Za-z0-9_]/g
    if (reg.test(str)) {
        return (false);
    }
    else {
        return (true);
    }
}

//验证url
function IsUrl(str) {
    var reg = /^(http\:\/\/)?([a-z0-9][a-z0-9\-]+\.)?[a-z0-9][a-z0-9\-]+[a-z0-9](\.[a-z]{2,4})+(\/[a-z0-9\.\,\-\_\%\?\=\&]?)?$/i;
    return reg.test(str);
}

//判断是否含有汉字       
function ContentWord(str) {
    if (escape(str).indexOf("%u") != -1)
        return true;
    else
        return false;
}