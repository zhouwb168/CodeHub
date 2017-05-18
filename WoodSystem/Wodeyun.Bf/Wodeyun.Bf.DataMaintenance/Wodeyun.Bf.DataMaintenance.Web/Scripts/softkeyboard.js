var inputContorl; //输入框控件对象
var capsLockFlag; //字母大小写状态标识
capsLockFlag = true;

window.onload = function () {
    inputContorl = null;
    InitCalc();
}

// 获取元素的坐标
// 参数：oElement - 元素对象
function GetElementPos(oElement) {
    var x2 = 0;
    var y2 = 0;
    var width = oElement.offsetWidth;
    var height = oElement.offsetHeight;
    if (typeof (oElement.offsetParent) != 'undefined') {
        for (var posX = 0, posY = 0; oElement; oElement = oElement.offsetParent) {
            posX += oElement.offsetLeft;
            posY += oElement.offsetTop;
        }
        x2 = posX + width;
        y2 = posY + height;
        return [posX, posY, x2, y2];
    }
    else {
        x2 = oElement.x + width;
        y2 = oElement.y + height;
        return [oElement.x, oElement.y, x2, y2];
    }
}

//生成软键盘父容器
function InitCalc() {
    var objTempDiv = document.createElement("div");
    objTempDiv.innerHTML = "<div align=\"center\" id=\"softkeyboard\" name=\"softkeyboard\" style=\"position:absolute; left:0px; top:0px; width:auto; z-index:180;display:none; border:1px solid #B5ADF1;\"></div>";
    document.body.appendChild(objTempDiv);
}

//绑定软键盘中的各按钮的点击事件，并设置按钮的CSS样式
function BindCalcButtonClick() {
    var arrElemenOfButton = document.getElementById("softkeyboard").getElementsByTagName("input");
    var elemenNum = arrElemenOfButton.length;

    var tempElem;
    var tempButtonValue;

    for (var i = 0; i < elemenNum; i++) {
        tempElem = arrElemenOfButton[i];
        if (tempElem.getAttribute("bgtype") == null) continue; //注意这里：bgtype是按钮的自定义属性
        if (tempElem.getAttribute("bgtype") == "n") {
            tempElem.className = "btn_num";
        }
        else if (tempElem.getAttribute("bgtype") == "l") {
            tempElem.className = "btn_letter";
        }
        else if (tempElem.getAttribute("bgtype") == "z") {
            tempElem.className = "btn_chinese";
        }
        else continue;

        tempButtonValue = tempElem.value;
        tempButtonValue = tempButtonValue.trim();
        if (tempButtonValue != "") {
            if (window.addEventListener) { // Mozilla, Netscape, Firefox
                tempElem.addEventListener('click', AddValue, false);
            } else { // IE
                tempElem.attachEvent('onclick', function () { AddValue(); });
            }
        }
    }
}

//字符串去掉两边空格
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

//把用户点选的值即时显示到输入框控件，这里已根据输入框控件对象的内容最大输入长度做了控制
function AddValue() {
    var srcElm = document.all ? window.event.srcElement : event.target;
    var thisButtonValue = srcElm.value;
    thisButtonValue = thisButtonValue.trim();
    if (inputContorl.value.length < inputContorl.maxLength) {
        inputContorl.value += thisButtonValue;
    }
}

//显软键盘
//参数:objTextBox -输入框控件对象，boardType -软键盘类型（比如纯数字型，纯字母型，或者是汉字型）
function showkeyboard(objTextBox, boardType) {
    inputContorl = objTextBox;
    inputContorl.readOnly = 1;
    inputContorl.value = "";
    inputContorl.blur();
    capsLockFlag = true;

    /* 构造要显示的内容并显示 */
    var strHtml = "";

    /*  构造时注意最内层的table的id=tblCalc
        每个值button按钮带有自定义属性bgtype */
    switch (boardType) {
        case "letter":
            {
                //纯字母
                strHtml = "<table align=\"center\" style=\"border-width:0px;\" cellpadding=\"0\" cellspacing=\"0\"><tr><td class=\"table_title\" style=\"cursor: default;height:30px;vertical-align:middle; background-color:#B5ADF1; text-align:right;\"><span style=\"font-weight:bold; font-size:13px; color:#075BC3;\">软键盘</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"btn_input\" type=\"button\" value=\"使用键盘输入\" onclick=\"closekeyboard(); inputContorl.value = ''; inputContorl.readOnly = 0; inputContorl.focus();\" /></td></tr><tr style=\"text-align:center;\"><td style=\"background-color:#FFFFFF;\"><table cellspacing=\"1\" cellpadding=\"0\"><tr><td><input bgtype=\"l\" type=\"button\" value=\" A \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" B \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" C \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" D \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" E \" /></td><td rowspan=\"2\"><input class=\"btn_letter\" type=\"button\" value=\"退格\" onclick=\"setpassvalue();\" style=\"width:60px; height:42px;\" /></td></tr><tr><td><input bgtype=\"l\" type=\"button\" value=\" F \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" G \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" H \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" I \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" J \" /></td></tr><tr><td><input bgtype=\"l\" type=\"button\" value=\" K \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" L \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" M \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" N \" /></td><td colspan=\"2\" rowspan=\"2\"><input class=\"btn_letter\" type=\"button\" onclick=\"capsLockText();\" value=\"切换大/小写\" style=\"width:100%;height:42px;\" /></td></tr><tr><td><input bgtype=\"l\" type=\"button\" value=\" P \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" Q \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" R \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" S \" /></td></tr><tr><td><input bgtype=\"l\" type=\"button\" value=\" U \" /></td><td> <input bgtype=\"l\" type=\"button\" value=\" V \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" W \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" Y \" /></td><td colspan=\"2\" rowspan=\"2\"><input class=\"btn_letter\" type=\"button\" onclick=\"OverInput();\" value=\"确定\" style=\"width:100%; height:42px;\" /></td></tr><tr><td><input bgtype=\"l\" type=\"button\" value=\" Z \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" O \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" T \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" X \" /></td></tr></table></td></tr></table>";
                break;
            }
        case "number":
            {
                //纯数字
                strHtml = "<table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-width:0px;\"><tr><td class=\"table_title\"  align=\"right\" valign=\"middle\" style=\"cursor: default;height:30px;background-color:#B5ADF1;\"><span style=\"font-weight:bold; font-size:13px; color:#075BC3;\">软键盘</span>&nbsp;<input class=\"btn_input\" type=\"button\" value=\"使用键盘输入\" onclick=\"closekeyboard(); inputContorl.value = ''; inputContorl.readOnly = 0; inputContorl.focus();\" style=\"width:86px;\" /></td></tr><tr align=\"center\"><td align=\"center\" bgcolor=\"#FFFFFF\"><table align=\"center\" border=\"0\" cellspacing=\"1\" cellpadding=\"0\"><tr align=\"left\" valign=\"middle\"><td><input type=\"button\" bgtype=\"n\" value=\" 1 \" /></td><td><input type=\"button\" bgtype=\"n\" value=\" 2 \" /></td><td><input type=\"button\" bgtype=\"n\" value=\" 3 \" /></td><td><input bgtype=\"n\" type=\"button\" value=\" 0 \" /></td><td><input class=\"btn_letter\" type=\"button\" value=\" 退格 \" onclick=\"setpassvalue();\"  style=\"width:60px;\" \></td></tr><tr align=\"left\" valign=\"middle\"><td><input type=\"button\" bgtype=\"n\" value=\" 4 \"></td><td><input type=\"button\" bgtype=\"n\" value=\" 5 \"></td><td><input type=\"button\" bgtype=\"n\" value=\" 6 \" /></td><td><input type=\"button\" bgtype=\"n\" value=\" . \" /></td><td><input class=\"btn_letter\" type=\"button\" disabled=\"disabled\" value=\"A-a\" style=\"width:60px;\" /></td></tr><tr align=\"left\" valign=\"middle\"><td><input type=\"button\" bgtype=\"n\" value=\" 7 \" /></td><td><input type=\"button\" bgtype=\"n\" value=\" 8 \" /></td><td><input type=\"button\" bgtype=\"n\" value=\" 9 \" /></td><td colspan=\"2\"><input class=\"btn_letter\" type=\"button\" onclick=\"OverInput();\" value=\"   确定  \" style=\"width:86px;\" /></td></tr></table></td></tr></table>";
                break;
            }
        case "chinese":
            {
                //纯汉字
                strHtml = "<table align=\"center\" style=\"border-width:0px;\" cellpadding=\"0\" cellspacing=\"0\"><tr><td class=\"table_title\" style=\"cursor: default;height:34px;vertical-align:middle; background-color:#B5ADF1; text-align:right;\"><span style=\"font-weight:bold; font-size:13px; color:#075BC3;\">软键盘</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"btn_input\" type=\"button\" value=\"使用键盘输入\" onclick=\"closekeyboard(); inputContorl.value = ''; inputContorl.readOnly = 0; inputContorl.focus();\" /></td></tr><tr style=\"text-align:center;\"><td style=\"background-color:#FFFFFF;\"><table cellspacing=\"1\" cellpadding=\"0\"><tr><td><input bgtype=\"z\" type=\"button\" value=\" 桂 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 粤 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 滇 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 黔 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 湘 \" /></td><td colspan=\"2\" rowspan=\"2\"><input class=\"btn_letter\" type=\"button\" onclick=\"OverInput();\" value=\"确定\" style=\"width:100%; height:49px;\" /></td></tr><tr><td><input bgtype=\"z\" type=\"button\" value=\" 琼 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 赣 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 鄂 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 港 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 澳 \" /></td></tr><tr><td><input bgtype=\"z\" type=\"button\" value=\" 豫 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 鲁 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 晋 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 皖 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 闽 \" /></td><td colspan=\"2\" rowspan=\"2\"><input class=\"btn_letter\" type=\"button\" value=\"退格\" onclick=\"setpassvalue();\" style=\"width:100%; height:48px;\" /></td></tr><tr><td><input bgtype=\"z\" type=\"button\" value=\" 黑 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 苏 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 津 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 渝 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 川 \" /></td></tr><tr><td><input bgtype=\"z\" type=\"button\" value=\" 京 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 藏 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 浙 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 吉 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 沪 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 冀 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 新 \" /></td></tr><tr><td><input bgtype=\"z\" type=\"button\" value=\" 辽 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 台 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 蒙 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 甘 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 青 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 陕 \" /></td><td><input bgtype=\"z\" type=\"button\" value=\" 宁 \" /></td></tr></table></td></tr></table>";
                break;
            }
        case "numlet":
            {
                //数字和字母
                strHtml = "<div align=\"center\" id=\"softkeyboard\" name=\"softkeyboard\" style=\"position:absolute; left:0px; top:0px; width:auto; z-index:180;display:block; border:1px solid #B5ADF1;\"><table align=\"center\" style=\"border-width:0px;\" cellpadding=\"0\" cellspacing=\"0\"><tr><td class=\"table_title\" style=\"cursor: default;height:30px;vertical-align:middle; background-color:#B5ADF1; text-align:right;\"><span style=\"font-weight:bold; font-size:13px; color:#075BC3;\">软键盘</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"btn_input\" type=\"button\" value=\"使用键盘输入\" onclick=\"closekeyboard(); inputContorl.value = ''; inputContorl.readOnly = 0; inputContorl.focus();\" /></td></tr><tr style=\"text-align:center;\"><td style=\"background-color:#FFFFFF;\"><table cellspacing=\"1\" cellpadding=\"0\"><tr><td><input bgtype=\"n\" type=\"button\" value=\" . \" /></td><td><input bgtype=\"n\" type=\"button\" value=\" 0 \" /></td><td><input bgtype=\"n\" type=\"button\" value=\" 1 \" /></td><td><input bgtype=\"n\" type=\"button\" value=\" 2 \" /></td><td><input bgtype=\"n\" type=\"button\" value=\" 3 \" /></td><td><input bgtype=\"n\" type=\"button\" value=\" 4 \" /></td><td><input bgtype=\"n\" type=\"button\" value=\" 5 \" /></td><td><input bgtype=\"n\" type=\"button\" value=\" 6 \" /></td></tr><tr><td><input bgtype=\"n\" type=\"button\" value=\" 7 \" /></td><td><input bgtype=\"n\" type=\"button\" value=\" 8 \" /></td><td><input bgtype=\"n\" type=\"button\" value=\" 9 \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" A \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" B \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" C \" /></td><td colspan=\"2\" rowspan=\"2\"><input class=\"btn_letter\" type=\"button\" onclick=\"OverInput();\" value=\"确定\" style=\"width:100%; height:42px;\" /></td></tr><tr><td><input bgtype=\"l\" type=\"button\" value=\" D \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" E \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" F \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" G \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" H \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" I \" /></td></tr><tr><td><input bgtype=\"l\" type=\"button\" value=\" J \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" K \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" L \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" M \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" N \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" V \" /></td><td colspan=\"2\" rowspan=\"2\"><input class=\"btn_letter\" type=\"button\" value=\"退格\" onclick=\"setpassvalue();\" style=\"width:100%; height:42px;\" /></td></tr><tr><td><input bgtype=\"l\" type=\"button\" value=\" P \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" Q \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" R \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" S \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" T \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" U \" /></td></tr><tr><td><input bgtype=\"l\" type=\"button\" value=\" O \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" W \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" X \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" Y \" /></td><td><input bgtype=\"l\" type=\"button\" value=\" Z \" /></td><td colspan=\"3\"><input class=\"btn_letter\" type=\"button\" onclick=\"capsLockText();\" value=\"切换大/小写\" style=\"width:100%;\" /></td></tr></table></td></tr></table></div>";
                break;
            }
    }

    var objKeyBoard = document.getElementById("softkeyboard");
    objKeyBoard.innerHTML = strHtml;

    BindCalcButtonClick();

    /* 下面是显示 */
    if (null != hideSelect) hideSelect();
    var pos = GetElementPos(inputContorl);
    objKeyBoard.style.top = pos[1] + 22 + "px";
    objKeyBoard.style.left = pos[0] + "px";
    objKeyBoard.style.display = "block";
}

//强制隐藏页面上的select元素，防止弹出的DIV遮盖不住select
function hideSelect() {
    var arrSelectElem = document.getElementsByTagName("select");
    var selectElemNum = arrSelectElem.length;
    for (var i = 0; i < selectElemNum; i++) {
        arrSelectElem[i].style.visibility = "hidden";
    }
}

//输入完毕
function OverInput() {
    closekeyboard();
    inputContorl.readOnly = 1;
    inputContorl.focus();
}

//关闭软键盘
function closekeyboard() {
    document.getElementById("softkeyboard").style.display = "none";

    if (null != unhideSelect) unhideSelect();
}

//重新显示页面里被隐藏的select元素
function unhideSelect() {
    var arrSelectElem = document.getElementsByTagName("select");
    var selectElemNum = arrSelectElem.length;
    for (var i = 0; i < selectElemNum; i++) {
        arrSelectElem[i].style.visibility = "visible";
    }
}

//字母大小写切换
function capsLockText() {
    var arrElemenOfButton = document.getElementById("softkeyboard").getElementsByTagName("input");
    var elemenNum = arrElemenOfButton.length;

    var tempElem;
    var tempButtonValue;

    for (var i = 0; i < elemenNum; i++) {
        tempElem = arrElemenOfButton[i];
        if (tempElem.getAttribute("bgtype") == null) continue;
        if (tempElem.getAttribute("bgtype") == "l") {
            tempButtonValue = tempElem.value;
            tempButtonValue = tempButtonValue.trim();
            if (capsLockFlag) tempElem.value = " " + String.fromCharCode(tempButtonValue.charCodeAt(0) + 32) + " ";
            else tempElem.value = " " + String.fromCharCode(tempButtonValue.charCodeAt(0) - 32) + " "
        }
    }
    capsLockFlag = !capsLockFlag;
}

//退格
function setpassvalue() {
    var valueLength = inputContorl.value.length;
    var newValue = inputContorl.value.substr(0, valueLength - 1);
    inputContorl.value = newValue;
}