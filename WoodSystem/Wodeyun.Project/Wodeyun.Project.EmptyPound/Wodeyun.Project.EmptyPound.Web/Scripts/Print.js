$(function () {
    Events.FillPrintData();
});

var Events = {
    Table: 'WoodPrice',

    PrintReport: function () {
        Events.PageSetup_Default();
        Events.PageSetup();
        /*  打印结算单 */
        var bdhtml = window.document.body.innerHTML;
        var sprnstr = "<!--startprint-->";
        var eprnstr = "<!--endprint-->";
        var prnhtml = bdhtml.substr(bdhtml.indexOf(sprnstr) + 17);
        prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
        window.document.body.innerHTML = prnhtml;
        window.print();
        Events.PageSetup_Default();
    },
    //设置网页打印的页眉页脚和页边距为空
    PageSetup: function () {
        var HKEY_Root, HKEY_Path, HKEY_Key;
        HKEY_Root = "HKEY_CURRENT_USER";
        HKEY_Path = "\\Software\\Microsoft\\Internet Explorer\\PageSetup\\";
        var head, foot, top, bottom, left, right;

        try {
            var Wsh = new ActiveXObject("WScript.Shell");
            //HKEY_Key = "header";
            //设置页眉（为空）  
            //Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, "");
            //HKEY_Key = "footer";
            //设置页脚（为空）  
            //Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, "");
            //HKEY_Key = "margin_bottom";
            //设置下页边距（0）  
            //Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, "0");
            HKEY_Key = "margin_left";
            //设置左页边距（0）  
            Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, "0.05");
            HKEY_Key = "margin_right";
            //设置右页边距（0）  
            Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, "1");
            //HKEY_Key = "margin_top";
            //设置上页边距（8）  
            //Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, "8");
        } catch (e) {
            alert("不允许ActiveX控件");
        }
    },

    //设置网页打印的页眉页脚和页边距为默认值  
    PageSetup_Default: function () {
        var HKEY_Root, HKEY_Path, HKEY_Key;
        HKEY_Root = "HKEY_CURRENT_USER";
        HKEY_Path = "\\Software\\Microsoft\\Internet Explorer\\PageSetup\\";
        var head, foot, top, bottom, left, right;
        try {
            var Wsh = new ActiveXObject("WScript.Shell");
            HKEY_Key = "header";
            HKEY_Key = "header";
            //还原页眉  
            Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, head);
            HKEY_Key = "footer";
            //还原页脚  
            Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, foot);
            HKEY_Key = "margin_bottom";
            //还原下页边距  
            Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, bottom);
            HKEY_Key = "margin_left";
            //还原左页边距  
            Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, left);
            HKEY_Key = "margin_right";
            //还原右页边距  
            Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, right);
            HKEY_Key = "margin_top";
            //还原上页边距  
            Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, top);
        } catch (e) {
            alert("不允许ActiveX控件");
        }
    },

    FillPrintData: function () {
        /*  获取木片检验结果通知单的打印信息，并调用打印机 */
        if (Events.GetQuery("params") == null) {
            alert("参数无效。");
            return;
        }
        var params = unescape(Events.GetQuery("params")).split("|");
        var emptyvolume = params[0].split("_");
        var jvolume = params[1].split("_");
        var rebatevolume = params[2].split("_");

        Events.SinglePrint(emptyvolume[0], jvolume[0], rebatevolume[0]);
        Events.PrintReport(); // 下发打印指令
    },

    ToFixed: function (value, fractionDigits) {
        with (Math) {
            return round(value * pow(10, fractionDigits)) / pow(10, fractionDigits);
        }
    },

    GetQuery: function (name) {
        /* 获取URL参数的值 */
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = document.location.search.substr(1).match(reg);
        if (r != null) return decodeURIComponent(r[2]); return null;
    },
    //单个打印
    SinglePrint: function (emptyvolume, jvolume, rebatevolume) {
        var table = ""; // 总的表格
        //补打单
        table += "<table style=\"width: 100%;align:right\">"; // 总表格开始
        var panel = ""; // 每一个小通知单
        var noneValue = "&nbsp;";

        panel = "";
        panel += "<div style=\"width:600px; letter-spacing:1px; margin-left:auto; margin-right:auto\">";
        panel += "<div style=\"color: #FF0000; width:100%; text-align:center; margin-top:30px; font-weight:bold\">&nbsp;</div>";
        panel += "<div style=\"border-bottom-style: solid; border-bottom-width: 0px; border-bottom-color: #FF0000; font-size: 16px; font-weight:bolder; color: #FF0000; margin-top: 10px; padding-bottom: 2px; width:162px; margin-left:auto; margin-right:auto\">&nbsp;</div>";
        //panel += "<div style=\"width:560px; color: #FF0000; margin-top:10px\">";
        panel += "<table width='600px' style=\" margin-top:10px\">";
        panel += "<tr>";
        panel += "<td colspan=3 align=right>&nbsp;";
        //panel += "年";
        //panel += "" + moment().add('years', 10).add('month', 6).format('MM') + "";
        //panel += "月";
        //panel += "" + moment().format('DD') + "";
        //panel += "日";
        panel += "</td></tr>";
        panel += "</table>";
        //panel += "</div>";
        panel += "<table cellpadding=\"5\" style=\"text-align:center;margin-top:5px;color: #FF0000\" class=\"reprint\">";
        panel += "<tr>";
        panel += "<td style=\"width: 140px\">&nbsp;</td>";
        panel += "<td style=\"width: 80px;\">&nbsp;</td>";
        panel += "<td style=\"width: 70px\">&nbsp;</td>";
        panel += "<td style=\"width: 90px;\">&nbsp;</td>";
        panel += "<td style=\"width: 60px;\">&nbsp;</td>";
        panel += "<td style=\"width: 90px;\">&nbsp;</td>";
        panel += "<td style=\"width: 90px;\">&nbsp;</td>";
        panel += "</tr>";
        panel += "<tr>";
        panel += "<td style='height:30px'></td>";
        panel += "<td></td>";
        panel += "<td></td>";
        panel += "<td></td>";
        panel += "<td></td>";
        panel += "<td></td>";
        panel += "<td></td>";
        panel += "</tr>";
        panel += "<tr>";
        panel += "<td style=\"color: #000000\">&nbsp;</td>";
        panel += "<td style=\"color: #000000\">&nbsp;</td>";
        panel += "<td style=\"color: #000000\">&nbsp;</td>";
        panel += "<td style=\"color: #000000\">&nbsp;</td>";
        panel += "<td style=\"color: #000000\">" + Events.ToFixed(rebatevolume, 2) + "</td>";
        panel += "<td style=\"color: #000000\">" + Events.ToFixed(emptyvolume, 2) + "</td>";
        panel += "<td style=\"color: #000000\">" + Events.ToFixed(jvolume, 2) + "</td>";
        panel += "</tr>";
        panel += "<tr>";
        panel += "<td>&nbsp;</td>";
        panel += "<td>&nbsp;</td>";
        panel += "<td>&nbsp;</td>";
        panel += "<td>&nbsp;</td>";
        panel += "<td>&nbsp;</td>";
        panel += "<td>&nbsp;</td>";
        panel += "<td>&nbsp;</td>";
        panel += "</tr>";
        panel += "<tr>";
        panel += "<td colspan=7 style='text-align:right;height:80px'>净体积：" + Events.DX(Events.ToFixed(jvolume, 2)).replace("元", "点").replace("角", "").replace("分", "") + "(立方)</td>";
        panel += "</tr>";
        panel += "</table>";
        panel += "</div>";
        table += "<tr>"; // 新增加一行
        table += "<td style=\"width:50%\">";
        table += panel;
        table += "</td>";
        table += "</tr>"; // 该行结束
        table += "</table>"; // 总表格结束
        $('#divPrint').html(table);
    },

    DX: function (num) {
        var strOutput = "";
        var strUnit = '仟佰拾亿仟佰拾万仟佰拾元角分';
        num += "00";
        var intPos = num.indexOf('.');
        if (intPos >= 0)
            num = num.substring(0, intPos) + num.substr(intPos + 1, 2);
        strUnit = strUnit.substr(strUnit.length - num.length);
        for (var i = 0; i < num.length; i++)
            strOutput += '零壹贰叁肆伍陆柒捌玖'.substr(num.substr(i, 1), 1) + strUnit.substr(i, 1);
        return strOutput.replace(/零角零分$/, '整').replace(/零[仟佰拾]/g, '零').replace(/零{2,}/g, '零').replace(/零([亿|万])/g, '$1').replace(/零+元/, '元').replace(/亿零{0,3}万/, '亿').replace(/^元/, "零元");
    }
};