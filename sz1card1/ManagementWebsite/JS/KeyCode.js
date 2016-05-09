//定义当前是否大写的状态
var CapsLockValue = 0;
var check;
function setVariables() {
    tablewidth = 630; // logo width, in pixels
    tableheight = 20; // logo height, in pixels
    if (navigator.appName == "Netscape") {
        horz = ".left";
        vert = ".top";
        docStyle = "document.";
        styleDoc = "";
        innerW = "window.innerWidth";
        innerH = "window.innerHeight";
        offsetX = "window.pageXOffset";
        offsetY = "window.pageYOffset";
    }
    else {
        horz = ".pixelLeft";
        vert = ".pixelTop";
        docStyle = "";
        styleDoc = ".style";
        innerW = "document.body.clientWidth";
        innerH = "document.body.clientHeight";
        offsetX = "document.body.scrollLeft";
        offsetY = "document.body.scrollTop";
    }
}
function checkLocation() {
    if (check) {
        objectXY = "softkeyboard";
        var availableX = eval(innerW);
        var availableY = eval(innerH);
        var currentX = eval(offsetX);
        var currentY = eval(offsetY);
        x = availableX - tablewidth + currentX;
        //y=availableY-tableheight+currentY;
        y = currentY;
        evalMove();
    }
    setTimeout("checkLocation()", 0);
}
function evalMove() {
    //eval(docStyle + objectXY + styleDoc + horz + "=" + x);
    eval(docStyle + objectXY + styleDoc + vert + "=" + y);
}
self.onError = null;
currentX = currentY = 0;
whichIt = null;
lastScrollX = 0; lastScrollY = 0;
NS = (document.layers) ? 1 : 0;
IE = (document.all) ? 1 : 0;
function heartBeat() {
    if (IE) { diffY = document.body.scrollTop; diffX = document.body.scrollLeft; }
    if (NS) { diffY = self.pageYOffset; diffX = self.pageXOffset; }
    if (diffY != lastScrollY) {
        percent = .1 * (diffY - lastScrollY);
        if (percent > 0) percent = Math.ceil(percent);
        else percent = Math.floor(percent);
        if (IE) document.all.softkeyboard.style.pixelTop += percent;
        if (NS) document.softkeyboard.top += percent;
        lastScrollY = lastScrollY + percent;
    }
    if (diffX != lastScrollX) {
        percent = .1 * (diffX - lastScrollX);
        if (percent > 0) percent = Math.ceil(percent);
        else percent = Math.floor(percent);
        if (IE) document.all.softkeyboard.style.pixelLeft += percent;
        if (NS) document.softkeyboard.left += percent;
        lastScrollX = lastScrollX + percent;
    }
}
function checkFocus(x, y) {
    stalkerx = document.softkeyboard.pageX;
    stalkery = document.softkeyboard.pageY;
    stalkerwidth = document.softkeyboard.clip.width;
    stalkerheight = document.softkeyboard.clip.height;
    if ((x > stalkerx && x < (stalkerx + stalkerwidth)) && (y > stalkery && y < (stalkery + stalkerheight))) return true;
    else return false;
}
function grabIt(e) {
    check = false;
    if (IE) {
        whichIt = event.srcElement;
        while (whichIt.id.indexOf("softkeyboard") == -1) {
            whichIt = whichIt.parentElement;
            if (whichIt == null) { return true; }
        }
        whichIt.style.pixelLeft = whichIt.offsetLeft;
        whichIt.style.pixelTop = whichIt.offsetTop;
        currentX = (event.clientX + document.body.scrollLeft);
        currentY = (event.clientY + document.body.scrollTop);
    } else {
        window.captureEvents(Event.MOUSEMOVE);
        if (checkFocus(e.pageX, e.pageY)) {
            whichIt = document.softkeyboard;
            StalkerTouchedX = e.pageX - document.softkeyboard.pageX;
            StalkerTouchedY = e.pageY - document.softkeyboard.pageY;
        }
    }
    return true;
}
function moveIt(e) {
    if (whichIt == null) { return false; }
    if (IE) {
        newX = (event.clientX + document.body.scrollLeft);
        newY = (event.clientY + document.body.scrollTop);
        distanceX = (newX - currentX); distanceY = (newY - currentY);
        currentX = newX; currentY = newY;
        whichIt.style.pixelLeft += distanceX;
        whichIt.style.pixelTop += distanceY;
        if (whichIt.style.pixelTop < document.body.scrollTop) whichIt.style.pixelTop = document.body.scrollTop;
        if (whichIt.style.pixelLeft < document.body.scrollLeft) whichIt.style.pixelLeft = document.body.scrollLeft;
        if (whichIt.style.pixelLeft > document.body.offsetWidth - document.body.scrollLeft - whichIt.style.pixelWidth - 20) whichIt.style.pixelLeft = document.body.offsetWidth - whichIt.style.pixelWidth - 20;
        if (whichIt.style.pixelTop > document.body.offsetHeight + document.body.scrollTop - whichIt.style.pixelHeight - 5) whichIt.style.pixelTop = document.body.offsetHeight + document.body.scrollTop - whichIt.style.pixelHeight - 5;
        event.returnValue = false;
    } else {
        whichIt.moveTo(e.pageX - StalkerTouchedX, e.pageY - StalkerTouchedY);
        if (whichIt.left < 0 + self.pageXOffset) whichIt.left = 0 + self.pageXOffset;
        if (whichIt.top < 0 + self.pageYOffset) whichIt.top = 0 + self.pageYOffset;
        if ((whichIt.left + whichIt.clip.width) >= (window.innerWidth + self.pageXOffset - 17)) whichIt.left = ((window.innerWidth + self.pageXOffset) - whichIt.clip.width) - 17;
        if ((whichIt.top + whichIt.clip.height) >= (window.innerHeight + self.pageYOffset - 17)) whichIt.top = ((window.innerHeight + self.pageYOffset) - whichIt.clip.height) - 17;
        return false;
    }
    return false;
}
function dropIt() {
    whichIt = null;
    if (NS) window.releaseEvents(Event.MOUSEMOVE);
    return true;
}
if (NS) {
    window.captureEvents(Event.MOUSEUP | Event.MOUSEDOWN);
    window.onmousedown = grabIt;
    window.onmousemove = moveIt;
    window.onmouseup = dropIt;
}
if (IE) {
    document.onmousedown = grabIt;
    document.onmousemove = moveIt;
    document.onmouseup = dropIt;
}
if (NS || IE) action = window.setInterval("heartBeat()", 1);
document.write(' <DIV align=center id=\"softkeyboard\" name=\"softkeyboard\" style=\"position:absolute;  width:400px; z-index:180;display:none\">');
document.write(' <table width=\"300\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"#07405E\">');
document.write(' <FORM name=Calc action=\"\" method=post autocomplete=\"off\">');
document.write(' <INPUT type=hidden value=ok name=action2>');
document.write(' <tr> ');
document.write(' <td align=\"right\" bgcolor=\"#4094C1\"> ');
//document.write(' <INPUT class=button type=button value=输入完毕 name=\"Submit3\" onclick=\"OverInput(curEditName);\"> <INPUT class=button type=reset value=输错重来 name=\"Submit23\"> ');
document.write(' <input class=\"btnclose\" type=button value=\"关闭\" name=\"Submit222\" onclick=\"closekeyboard(curEditName);\"> </td>');
document.write(' </tr>');
document.write(' <tr> ');
document.write(' <td align=\"center\" bgcolor=\"#4094C1\" align=\"center\"><table align=\"center\" width=\"98%\" height=\"70px\" border=\"0\" cellspacing=\"1\" cellpadding=\"1\">');
document.write(' <tr align=\"left\" valign=\"middle\"> ');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button  class=\"btn\" onclick=\"addValue(\'1\');\" value=\" 1 \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\" > ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'2\');\" value=\" 2 \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\" > ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'3\');\" value=\" 3 \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'4\');\" value=\" 4 \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'5\');\" value=\" 5 \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'6\');\" value=\" 6 \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td  nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'7\');\" value=\" 7 \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'8\');\" value=\" 8 \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'9\');\" value=\" 9 \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'0\');\" value=\" 0 \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'-\');\" value=\" - \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\'  ></td>');
document.write(' <td nowrap class=\"a1\"><input name=\"button10\" class=\"btn\" type=button value=\" <----\" style=\"width:70px;\" onclick=\"setpassvalue();\"  onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' > ');
document.write(' </td>');
document.write(' <tr align=\"left\" valign=\"middle\"> ');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'q\');\" value=\" q \"  onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'w\');\" value=\" w \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'e\');\" value=\" e \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'r\');\" value=\" r \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'t\');\" value=\" t \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'y\');\" value=\" y \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'u\');\" value=\" u \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'i\');\" value=\" i \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" class=\"button\" onclick=\"addValue(\'o\');\" value=\" o \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'p\');\" value=\" p \"  onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input name=\"button6\" class=\"btn\" type=button onClick=\"addValue(\':\');\" value=\" : \"  onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\" colspan=\"2\"><input name=\"button12\" class=\"btn\" type=button onclick=\"OverInput(curEditName);\" style=\"width:70px;\" value=\" Enter \"  onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' > ');
document.write(' </td>');
//document.write(' <td nowrap class=\"a1\"> ');
//document.write(' </td>');
document.write(' </tr>');
document.write(' <tr align=\"left\" valign=\"middle\"> ');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'a\');\" value=\" a \"  onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'s\');\" value=\" s \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'d\');\" value=\" d \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'f\');\" value=\" f \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'g\');\" value=\" g \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'h\');\" value=\" h \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'j\');\" value=\" j \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'k\');\" value=\" k \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'l\');\" value=\" l \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input name=\"button8\" class=\"btn\" type=button onClick=\"addValue(\'[\');\" value=\" [ \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input name=\"button9\" class=\"btn\"  class=\"btn\" type=button onClick=\"addValue(\']\');\" value=\" ] \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td  nowrap class=\"a1\" colspan=\"2\"><input name=\"button9\" class=\"btn\" type=button onClick=\"setCapsLock();\" style=\"width:70px;\" value=\"切换大/小写\" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' </tr>');
document.write(' <tr align=\"left\" valign=\"middle\"> ');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'z\');\" value=\" z \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'x\');\" value=\" x \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'c\');\" value=\" c \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'v\');\" value=\" v \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'b\');\" value=\" b \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'n\');\" value=\" n \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'m\');\" value=\" m \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input name=\"button3\" class=\"btn\" type=button onClick=\"addValue(\'<\');\" value=\" < \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input name=\"button4\" class=\"btn\" type=button onClick=\"addValue(\'>\');\" value=\" > \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\'></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input name=\"button5\" class=\"btn\" type=button onClick=\"addValue(\'(\');\" value=\" ( \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input name=\"button7\" class=\"btn\" type=button onClick=\"addValue(\')\');\" value=\" ) \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td  nowrap class=\"a1\" colspan=\"2\"> ');
document.write(' <input name=\"showCapsLockValue\" class=\"btn\" type=reset style=\"width:70px;\" value=\"当前是小写 \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' </tr>');
document.write(' <tr align=\"left\" valign=\"middle\"> ');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input name=\"button2\" class=\"btn\" type=button onClick=\"addValue(\',\');\" value=\" , \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'~\');\" value=\" ~ \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'!\');\" value=\" ! \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'@\');\" value=\" @ \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'#\');\" value=\" # \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'$\');\" value=\" $ \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\"  onclick=\"addValue(\'%\');\" value=\" % \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'^\');\" value=\" ^ \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'*\');\" value=\" * \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'|\');\" value=\" | \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td nowrap class=\"a1\"> ');
document.write(' <input type=button class=\"btn\" onclick=\"addValue(\'?\');\" value=\" ? \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' <td  nowrap class=\"a1\" colspan=\"2\"><input name=\"btn\" class=\"btn\" type=button onClick=\"addValue(\'=\');\" style=\"width:70px;\" value=\" = \" onmouseover=\'this.className=\"btn1_mouseover\"\' onmouseout=\'this.className=\"btn\"\' ></td>');
document.write(' </tr>');
document.write(' </table></td>');
document.write(' </tr>');
document.write(' </FORM>');
document.write(' </table>');
document.write('</DIV>');
//给输入的密码框添加新值
function addValue(newValue) {
    if (CapsLockValue == 0) {
        if (document.getElementById("tabIndex").value == "0") {
            document.forms["form1"].tbUserpassword.value += newValue;
        }
        else {
            document.forms["form1"].tbUKPassword.value += newValue;
        }
    }
    else {
        if (document.getElementById("tabIndex").value == "0") {
            document.forms["form1"].tbUserpassword.value += newValue.toUpperCase();
        }
        else {
            document.forms["form1"].tbUKPassword.value += newValue.toUpperCase();
        }
    }
}
//实现BackSpace键的功能
function setpassvalue() {
    if (document.getElementById("tabIndex").value == "0") {
        var longnum = document.forms["form1"].tbUserpassword.value.length;
        var num;
        num = document.forms["form1"].tbUserpassword.value.substr(0, longnum - 1);
        document.forms["form1"].tbUserpassword.value = num;
    }
    else {
        var longnum = document.forms["form1"].tbUKPassword.value.length;
        var num;
        num = document.forms["form1"].tbUKPassword.value.substr(0, longnum - 1);
        document.forms["form1"].tbUKPassword.value = num;
    }
    
}
//输入完毕
function OverInput(theForm) {
    document.getElementById("softkeyboard").style.display = "none";
    document.getElementById("TextCode").focus();
}
//关闭软键盘
function closekeyboard(theForm) {
    document.getElementById("softkeyboard").style.display = "none";
}
//显示软键盘
function showkeyboard(event, o) {
    var key = document.getElementById("softkeyboard");
    if (key.style.display == "none") {
        key.style.left = event.clientX - 210 + "px";
        key.style.top = event.clientY + 8 + "px";
        key.style.display = "block";
    } else {
        key.style.display = "none";
    }
}
//设置是否大写的值
function setCapsLock() {
    if (CapsLockValue == 0) {
        CapsLockValue = 1
        document.forms["Calc"].showCapsLockValue.value = "当前是大写 ";
    }
    else {
        CapsLockValue = 0
        document.forms["Calc"].showCapsLockValue.value = "当前是小写 ";
    }
}