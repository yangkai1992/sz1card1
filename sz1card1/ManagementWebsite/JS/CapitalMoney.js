function capitalMoney(value) {
    var HundredMillion = 100000000.0;
    var Myriad = 10000;
    var Thousand = 1000;
    var Hundred = 100;
    var Ten = 10;
    var numFloor = 0;
    //元
    var Display = "";
    var DisplayHundredMillion = "";
    var DisplayMyriad = "";
    var DisplayThousand = "";
    var DisplayHundred = "";
    var DisplayTen = "";
    if (value >= HundredMillion) {
        numFloor = Math.floor(value / HundredMillion);
        if (numFloor >= Myriad) {
            DisplayHundredMillion = capitalRecursiveMoney(numFloor);
            //DisplayHundredMillion=DisplayHundredMillion+"&nbsp;"+capitalDigit(Math.floor(numFloor/Myriad))+"万";
            //DisplayHundredMillion=DisplayHundredMillion+"&nbsp;"+capitalDigit(Math.floor(numFloor%Ten))+"万";
            DisplayHundredMillion = DisplayHundredMillion + "亿";
            numFloor = Math.floor(value % HundredMillion);
        }
        else {
            if (numFloor >= Thousand) {
                DisplayHundredMillion = DisplayHundredMillion + "&nbsp;" + capitalDigit(Math.floor(numFloor / Thousand)) + "千";
                numFloor = Math.floor(numFloor % Thousand);
            }
            else {
                if (DisplayHundredMillion != "") DisplayHundredMillion = DisplayHundredMillion + "&nbsp;" + capitalDigit(0) + "千";
            }
            if (numFloor >= Hundred) {
                DisplayHundredMillion = DisplayHundredMillion + "&nbsp;" + capitalDigit(Math.floor(numFloor / Hundred)) + "百";
                numFloor = Math.floor(numFloor % Hundred);
            }
            else {
                if (DisplayHundredMillion != "") DisplayHundredMillion = DisplayHundredMillion + "&nbsp;" + capitalDigit(0) + "百";
            }
            if (numFloor >= Ten) {
                DisplayHundredMillion = DisplayHundredMillion + "&nbsp;" + capitalDigit(Math.floor(numFloor / Ten)) + "十";
                numFloor = Math.floor(numFloor % Ten);
            }
            else {
                if (DisplayHundredMillion != "") DisplayHundredMillion = DisplayHundredMillion + "&nbsp;" + capitalDigit(0) + "十";
            }
            DisplayHundredMillion = DisplayHundredMillion + "&nbsp;" + capitalDigit(numFloor) + "亿";
        }
    }
    if (value >= Myriad) {
        if (value >= HundredMillion) numFloor = Math.floor((value % HundredMillion) / Myriad);
        else numFloor = Math.floor(value / Myriad);
        if (numFloor >= Thousand) {
            DisplayMyriad = DisplayMyriad + "&nbsp;" + capitalDigit(Math.floor(numFloor / Thousand)) + "千";
            numFloor = Math.floor(numFloor % Thousand);
        }
        if (numFloor >= Hundred) {
            DisplayMyriad = DisplayMyriad + "&nbsp;" + capitalDigit(Math.floor(numFloor / Hundred)) + "百";
            numFloor = Math.floor(numFloor % Hundred);
        }
        else {
            if (DisplayMyriad != "") DisplayMyriad = DisplayMyriad + "&nbsp;" + capitalDigit(0) + "百";
        }
        if (numFloor >= Ten) {
            DisplayMyriad = DisplayMyriad + "&nbsp;" + capitalDigit(Math.floor(numFloor / Ten)) + "十";
            numFloor = Math.floor(numFloor % Ten);
        }
        else {
            if (DisplayMyriad != "") DisplayMyriad = DisplayMyriad + "&nbsp;" + capitalDigit(0) + "十";
        }
        DisplayMyriad = DisplayMyriad + "&nbsp;" + capitalDigit(numFloor) + "万";
    }
    else DisplayMyriad = "&nbsp;万";
    numFloor = Math.floor(value % Myriad);
    if (numFloor >= Thousand) {
        DisplayThousand = DisplayThousand + "&nbsp;" + capitalDigit(Math.floor(numFloor / Thousand)) + "千";
        numFloor = Math.floor(numFloor % Thousand);
    }
    else {
        if (value > Thousand) DisplayThousand = "&nbsp;" + capitalDigit(0) + "千";
        else DisplayThousand = "&nbsp;千";
    }
    if (numFloor >= Hundred) {
        DisplayHundred = DisplayHundred + "&nbsp;" + capitalDigit(Math.floor(numFloor / Hundred)) + "百";
        numFloor = Math.floor(numFloor % Hundred);
    }
    else {
        if (value > Hundred) DisplayHundred = "&nbsp;" + capitalDigit(0) + "百";
        else DisplayHundred = "&nbsp;百";
    }
    if (numFloor >= Ten) {
        DisplayTen = DisplayTen + "&nbsp;" + capitalDigit(Math.floor(numFloor / Ten)) + "十";
        numFloor = Math.floor(numFloor % Ten);
    }
    else {
        if (value > Ten) DisplayTen = "&nbsp;" + capitalDigit(0) + "十";
        else DisplayTen = "&nbsp;十";
    }
    Display = DisplayHundredMillion + DisplayMyriad + DisplayThousand + DisplayHundred + DisplayTen + "&nbsp;" + capitalDigit(numFloor) + "元";
    //角、分
    numFloor = Math.floor((value * 100) % Hundred);
    if (numFloor >= Ten) {
        Display = Display + "&nbsp;" + capitalDigit(Math.floor(numFloor / Ten)) + "角";
        numFloor = Math.floor(numFloor % Ten);
    }
    else Display = Display + "&nbsp;" + capitalDigit(0) + "角";
    Display = Display + "&nbsp;" + capitalDigit(numFloor) + "分";
    return Display;
}

function capitalRecursiveMoney(value) {
    var HundredMillion = 100000000.0;
    var Myriad = 10000;
    var Thousand = 1000;
    var Hundred = 100;
    var Ten = 10;
    var numFloor = 0;
    //元
    var Display = "";
    var DisplayHundredMillion = "";
    var DisplayMyriad = "";
    var DisplayThousand = "";
    var DisplayHundred = "";
    var DisplayTen = "";
    if (value >= HundredMillion) {
        numFloor = Math.floor(value / HundredMillion);
        if (numFloor >= Myriad) {
            DisplayHundredMillion = capitalRecursiveMoney(numFloor);
            //DisplayHundredMillion=DisplayHundredMillion+"&nbsp;"+capitalDigit(Math.floor(numFloor/Myriad))+"万";
            //DisplayHundredMillion=DisplayHundredMillion+"&nbsp;"+capitalDigit(Math.floor(numFloor%Ten))+"万";
            numFloor = Math.floor(value % HundredMillion);
            DisplayHundredMillion = DisplayHundredMillion + "亿";
        }
        else {
            if (numFloor >= Thousand) {
                DisplayHundredMillion = DisplayHundredMillion + "&nbsp;" + capitalDigit(Math.floor(numFloor / Thousand)) + "千";
                numFloor = Math.floor(numFloor % Thousand);
            }
            else {
                //if(DisplayHundredMillion!="") DisplayHundredMillion=DisplayHundredMillion+"&nbsp;"+capitalDigit(0)+"千";
            }
            if (numFloor >= Hundred) {
                DisplayHundredMillion = DisplayHundredMillion + "&nbsp;" + capitalDigit(Math.floor(numFloor / Hundred)) + "百";
                numFloor = Math.floor(numFloor % Hundred);
            }
            else {
                //if(DisplayHundredMillion!="") DisplayHundredMillion=DisplayHundredMillion+"&nbsp;"+capitalDigit(0)+"百";
            }
            if (numFloor >= Ten) {
                DisplayHundredMillion = DisplayHundredMillion + "&nbsp;" + capitalDigit(Math.floor(numFloor / Ten)) + "十";
                numFloor = Math.floor(numFloor % Ten);
            }
            else {
                // if(DisplayHundredMillion!="") DisplayHundredMillion=DisplayHundredMillion+"&nbsp;"+capitalDigit(0)+"十";
            }
            DisplayHundredMillion = DisplayHundredMillion + "&nbsp;" + capitalDigit(numFloor) + "亿";
        }
    }
    if (value >= Myriad) {
        if (value >= HundredMillion) numFloor = Math.floor((value % HundredMillion) / Myriad);
        else numFloor = Math.floor(value / Myriad);
        if (numFloor >= Thousand) {
            DisplayMyriad = DisplayMyriad + "&nbsp;" + capitalDigit(Math.floor(numFloor / Thousand)) + "千";
            numFloor = Math.floor(numFloor % Thousand);
        }
        if (numFloor >= Hundred) {
            DisplayMyriad = DisplayMyriad + "&nbsp;" + capitalDigit(Math.floor(numFloor / Hundred)) + "百";
            numFloor = Math.floor(numFloor % Hundred);
        }
        else {
            // if(DisplayMyriad!="") DisplayMyriad=DisplayMyriad+"&nbsp;"+capitalDigit(0)+"百";
        }
        if (numFloor >= Ten) {
            DisplayMyriad = DisplayMyriad + "&nbsp;" + capitalDigit(Math.floor(numFloor / Ten)) + "十";
            numFloor = Math.floor(numFloor % Ten);
        }
        else {
            //if(DisplayMyriad!="") DisplayMyriad=DisplayMyriad+"&nbsp;"+capitalDigit(0)+"十";
        }
        DisplayMyriad = DisplayMyriad + "&nbsp;" + capitalDigit(numFloor) + "万";
    }
    else DisplayMyriad = "&nbsp;万";
    numFloor = Math.floor(value % Myriad);
    if (numFloor >= Thousand) {
        DisplayThousand = DisplayThousand + "&nbsp;" + capitalDigit(Math.floor(numFloor / Thousand)) + "千";
        numFloor = Math.floor(numFloor % Thousand);
    }
    else {

    }
    if (numFloor >= Hundred) {
        DisplayHundred = DisplayHundred + "&nbsp;" + capitalDigit(Math.floor(numFloor / Hundred)) + "百";
        numFloor = Math.floor(numFloor % Hundred);
    }
    else {

    }
    if (numFloor >= Ten) {
        DisplayTen = DisplayTen + "&nbsp;" + capitalDigit(Math.floor(numFloor / Ten)) + "十";
        numFloor = Math.floor(numFloor % Ten);
    }
    else {

    }
    Display = DisplayHundredMillion + DisplayMyriad + DisplayThousand + DisplayHundred + DisplayTen;
    if (numFloor != 0) Display = Display + "&nbsp;" + capitalDigit(numFloor);
    return Display;
}

function capitalDigit(value) {
    value = Math.floor(value);
    switch (value) {
        ////零壹贰叁肆伍陆柒捌玖////
        //Use Unicode to Avoid 乱码
        case 0: return "<font color=blue>\u96F6</font>";
        case 1: return "<font color=blue>\u58F9</font>";
        case 2: return "<font color=blue>\u8D30</font>";
        case 3: return "<font color=blue>\u53C1</font>";
        case 4: return "<font color=blue>\u8086</font>";
        case 5: return "<font color=blue>\u4F0D</font>";
        case 6: return "<font color=blue>\u9646</font>";
        case 7: return "<font color=blue>\u67D2</font>";
        case 8: return "<font color=blue>\u634C</font>";
        case 9: return "<font color=blue>\u7396</font>";
            return "";
    }
}