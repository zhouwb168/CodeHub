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
        var WeighTime = params[0].split("_");
        var license = params[1].split("_");
        var tree = params[2].split("_");
        var fullvolume = params[3].split("_");
        var supplier = params[4].split("_");

        Events.SinglePrint(WeighTime[0], license[0], tree[0], fullvolume[0], supplier[0]);
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
    SinglePrint: function (WeighTime, license, tree, fullvolume, supplier) {
        var table = ""; // 总的表格
        table += "<table style=\"width: 100%;align:right\">"; // 总表格开始
        var panel = ""; // 每一个小通知单
        var noneValue = "&nbsp;";
        panel = "";
        panel += "<div style=\"width:600px; letter-spacing:1px; margin-left:auto; margin-right:auto\">";
        panel += "<div style=\"color: #FF0000; width:100%; text-align:center; margin-top:30px; font-weight:bold\">广西百色丰林人造板有限公司</div>";
        panel += "<div style=\"border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #FF0000; font-size: 16px; font-weight:bolder; color: #FF0000; margin-top: 10px; padding-bottom: 2px; width:162px; margin-left:auto; margin-right:auto\">木质原料收购磅单</div>";
        panel += "<table width='600px' style=\" margin-top:10px\">";
        panel += "<tr>";
        panel += "<td colspan=3 align=right>" + moment(WeighTime).format('YYYY') + "";
        panel += "年";
        panel += "" + moment(WeighTime).format('MM') + "";
        panel += "月";
        panel += "" + moment(WeighTime).format('DD') + "";
        panel += "日";
        panel += "</td></tr>";
        panel += "</table>";
        panel += "<table cellpadding=\"5\" style=\"text-align:center;margin-top:5px;color: #FF0000\" class=\"print\">";
        panel += "<tr>";
        panel += "<td style=\"width: 140px\">首磅时间</td>";
        panel += "<td style=\"width: 80px;\">车号</td>";
        panel += "<td style=\"width: 70px\">名称</td>";
        panel += "<td style=\"width: 90px;\">首磅体积</td>";
        panel += "<td style=\"width: 60px;\">扣方</td>";
        panel += "<td style=\"width: 90px;\">回皮体积</td>";
        panel += "<td style=\"width: 90px;\">净体积</td>";
        panel += "</tr>";
        panel += "<tr>";
        panel += "<td style=\"color: #000000\">" + WeighTime + "</td>";
        panel += "<td style=\"color: #000000\">" + license + "</td>";
        panel += "<td style=\"color: #000000\">" + tree + "</td>";
        panel += "<td style=\"color: #000000\">" + Events.ToFixed(fullvolume, 2) + "</td>";
        panel += "<td style=\"color: #000000\"></td>";
        panel += "<td style=\"color: #000000\"></td>";
        panel += "<td style=\"color: #000000\"></td>";
        panel += "</tr>";
        panel += "<tr>";
        panel += "<td>过磅员</td>";
        panel += "<td></td>";
        panel += "<td>货场</td>";
        panel += "<td colspan=2></td>";
        panel += "<td>卸货员</td>";
        panel += "<td style=\"color: #000000\">" + supplier + "</td>";
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
    }
};