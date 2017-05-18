$(function () {
    Events.FillPrintData();
});

var Events = {
    Table: 'WoodPrice',

    PrintReport: function () {
        Events.PageSetup_Default();
        //Events.PageSetup();
        /*  打印结算单 */
        var bdhtml = window.document.body.innerHTML;
        var sprnstr = "<!--startprint-->";
        var eprnstr = "<!--endprint-->";
        var prnhtml = bdhtml.substr(bdhtml.indexOf(sprnstr) + 17);
        prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
        window.document.body.innerHTML = prnhtml;
        window.print();
    },

    FillPrintData: function () {
        /*  获取木片检验结果通知单的打印信息，并调用打印机 */
        if (Events.GetQuery("params") == null) {
            alert("参数无效。");
            return;
        }
        var params = Events.GetQuery("params").split("|");
        var orderno = params[0];
        var account = params[1];
        var ismegre = params[2];
        var data = {
            service: Events.Table,
            method: 'GetEntitysForCostPrint',
            args: [JSON.stringify({
                OrderNo: orderno,
                Account: account,
                Ismegre: ismegre
            })]
        };

        var success = function (result) {
            var root = Ajaxer.GetRoot(result);
            var Weights = Ajaxer.GetItems(root.Weights);
            var Volumes = Ajaxer.GetItems(root.Volumes);
            var CarCounts = Ajaxer.GetItems(root.CarCounts);
            if (ismegre == 0) {
                //打印重量结算单
                Events.OnPrintWeight(Weights, CarCounts);
            } else {
                //打印体积结算单
                Events.OnPrintVolume(Weights, Volumes, CarCounts);
            }
            Events.PrintReport(); // 下发打印指令
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    GetQuery: function (name) {
        /* 获取URL参数的值 */
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = document.location.search.substr(1).match(reg);
        if (r != null) return decodeURIComponent(r[2]); return null;
    },

    ToDateTime: function (value) {
        if (value == null) return "";
        return moment(value).format('YYYY-MM-DD');
    },

    //打印体积结算单
    OnPrintVolume: function (Weights, Volumes, CarCounts) {
        var table = "";
        table += "<table style=\"width: 100%\">";
        var panel = ""; // 每一个小通知单
        var noneValue = "&nbsp;";
        var amount = 0.00, jweight = 0.00;
        var len = Volumes.length;
        for (var j = 0; j < Volumes.length; j++) {
            amount += Volumes[j].Amount;
            jweight += Volumes[j].JWeight;
        }

        panel = "";
        panel += "<div style=\"width:600px; letter-spacing:1px; margin-left:auto; margin-right:auto\">";
        panel += "<div style=\"color: #FF0000; width:100%; text-align:center; margin-top:30px; font-weight:bold\">广西百色丰林人造板有限公司</div>";
        //panel += "<div style=\"border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #FF0000; font-size: 14px; font-weight:bolder; color: #FF0000; margin-top: 10px; padding-bottom: 2px; width:162px; margin-left:auto; margin-right:auto\">木质原料收购结算单</div>";
        panel += "<table width='600px' style=\" margin-top:10px\">";
        panel += "<tr>";
        panel += "<td style='text-align:right;width:367px;font-size: 14px; font-weight:bolder;color: #FF0000'>木质原料收购结算单</td>";
        panel += "<td colspan=3 align=right>" + moment().format('YYYY') + "";
        panel += "年";
        panel += "" + moment().add('years', 10).format('MM') + "";
        panel += "月";
        panel += "" + moment().format('DD') + "";
        panel += "日";
        panel += "</td></tr>";
        panel += "</table>";
        panel += "<table cellpadding=\"5\" style=\"width:600px;text-align:center; margin-left:auto; margin-right:auto;margin-top:5px;color: #FF0000\" class=\"print\">";
        panel += "<tr>";
        panel += "</tr>";
        panel += "<tr>";
        panel += "<td style=\"width: 140px\">名称</td>";
        panel += "<td style=\"width: 130px\">净体积(立方)</td>";
        panel += "<td style=\"width: 130px\">单价(元/方)</td>";
        panel += "<td>金额(元)</td>";
        panel += "</tr>";

        for (var k = 0; k < len; k++) {
            panel += "<td style=\"color: #000000\">" + Volumes[k].Tree + "</td>";
            panel += "<td style=\"color: #000000\">" + Volumes[k].JVolume.toFixed(2) + "</td>";
            panel += "<td style=\"color: #000000\">" + Volumes[k].CubePrice.toFixed(2) + "</td>";
            panel += "<td style=\"color: #000000\">" + Volumes[k].Amount.toFixed(2) + "</td>";
            panel += "</tr>";
            panel += "<tr>";
        }
        if (len < 3) {
            for (var n = 0; n < (3 - len) ; n++) {
                panel += "<tr>";
                panel += "<td>&nbsp;</td>";
                panel += "<td></td>";
                panel += "<td></td>";
                panel += "<td></td>";
                panel += "</tr>";
            }
        }

        panel += "<tr>";
        panel += "<td>重量(湿吨)</td>";
        panel += "<td style=\"color: #000000\">" + jweight.toFixed(2) + "</td>";
        panel += "<td>合计</td>";
        panel += "<td style=\"color: #000000\">" + amount.toFixed(2) + "</td>";
        panel += "</tr>";

        panel += "<tr>";
        panel += "<td>结算磅单日期</td>";
        panel += "<td colspan=\"2\" style=\"color: #000000;\">" + ((Events.ToDateTime(CarCounts[0].MINBang_Time) == Events.ToDateTime(CarCounts[0].MAXBang_Time)) ? Events.ToDateTime(CarCounts[0].MINBang_Time) : (Events.ToDateTime(CarCounts[0].MINBang_Time) + " - " + Events.ToDateTime(CarCounts[0].MAXBang_Time))) + "</td>";
        panel += "<td style=\"color: #000000\">车数：" + CarCounts[0].CarCount + "</td>";
        panel += "</tr>";

        panel += "<tr>";
        panel += "<td style=\"text-align:left;\" colspan=\"2\">审核人：</td>";
        panel += "<td style=\"text-align:left;\" colspan=\"2\">制单人：</td>";
        panel += "</tr>";
        panel += "</table>";
        panel += "</div>";
        table += "<tr>";
        table += "<td style=\"width:50%\">";
        table += panel;
        table += "</td>";
        table += "</tr>";
        table += "</table>"; // 总表格结束
        $('#divPrint').html(table);
    },
    //打印重量结算单
    OnPrintWeight: function (Weights, CarCounts) {
        var table = "";
        table += "<table style=\"width: 100%\">";
        var panel = ""; // 每一个小通知单
        var noneValue = "&nbsp;";
        var amount = 0.00, jweight = 0.00;
        var len = Weights.length;
        if (len == 0) {
            alert("没有数据");
            return;
        }
        for (var i = 0; i < len; i++) {
            amount += Weights[i].Amount;
            jweight += Weights[i].JWeight;
        }

        panel = "";
        panel += "<div style=\"width:600px; letter-spacing:1px; margin-left:auto; margin-right:auto\">";
        panel += "<div style=\"color: #FF0000; width:100%; text-align:center; margin-top:30px; font-weight:bold\">广西百色丰林人造板有限公司</div>";
        //panel += "<div style=\"border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #FF0000; font-size: 14px; font-weight:bolder; color: #FF0000; margin-top: 10px; padding-bottom: 2px; width:162px; margin-left:auto; margin-right:auto\">木质原料收购结算单</div>";
        panel += "<table width='600px' style=\" margin-top:10px\">";
        panel += "<tr>";
        panel += "<td style='text-align:right;width:367px;font-size: 14px; font-weight:bolder;color: #FF0000'>木质原料收购结算单</td>";
        panel += "<td colspan=3 align=right>" + moment().format('YYYY') + "";
        panel += "年";
        panel += "" + moment().add('years', 10).format('MM') + "";
        panel += "月";
        panel += "" + moment().format('DD') + "";
        panel += "日";
        panel += "</td></tr>";
        panel += "</table>";
        panel += "<table cellpadding=\"5\" style=\"width:600px;text-align:center; margin-left:auto; margin-right:auto;margin-top:5px;color: #FF0000\" class=\"print\">";
        panel += "<tr>";
        panel += "</tr>";
        panel += "<tr>";
        panel += "<td style=\"width: 140px\">名称</td>";
        panel += "<td style=\"width: 130px\">重量(干吨)</td>";
        panel += "<td style=\"width: 130px\">单价(元/干吨)</td>";
        panel += "<td>金额(元)</td>";
        panel += "</tr>";
        panel += "<tr>";
        for (var j = 0; j < len; j++) {
            panel += "<td style=\"color: #000000\">" + Weights[j].Tree + "</td>";
            panel += "<td style=\"color: #000000\">" + Weights[j].GWeight.toFixed(2) + "</td>";
            panel += "<td style=\"color: #000000\">" + Weights[j].GPrice.toFixed(2) + "</td>";
            panel += "<td style=\"color: #000000\">" + Weights[j].Amount.toFixed(2) + "</td>";
            panel += "</tr>";
            panel += "<tr>";
        }
        if (len < 3) {
            for (var k = 0; k < (3 - len) ; k++) {
                panel += "<tr>";
                panel += "<td>&nbsp;</td>";
                panel += "<td></td>";
                panel += "<td></td>";
                panel += "<td></td>";
                panel += "</tr>";
            }
        }

        panel += "<td>重量(湿吨)</td>";
        panel += "<td style=\"color: #000000\">" + jweight.toFixed(2) + "</td>";
        panel += "<td>合计</td>";
        panel += "<td style=\"color: #000000\">" + amount.toFixed(2) + "</td>";
        panel += "</tr>";

        panel += "<tr>";
        panel += "<td>结算磅单日期</td>";
        panel += "<td colspan=\"2\" style=\"color: #000000;\">" + ((Events.ToDateTime(CarCounts[0].MINBang_Time) == Events.ToDateTime(CarCounts[0].MAXBang_Time)) ? Events.ToDateTime(CarCounts[0].MINBang_Time) : (Events.ToDateTime(CarCounts[0].MINBang_Time) + " - " + Events.ToDateTime(CarCounts[0].MAXBang_Time))) + "</td>";
        panel += "<td style=\"color: #000000\">车数：" + CarCounts[0].CarCount + "</td>";
        panel += "</tr>";

        panel += "<tr>";
        panel += "<td style=\"text-align:left;\" colspan=\"2\">审核人：</td>";
        panel += "<td style=\"text-align:left;\" colspan=\"2\">制单人：</td>";
        panel += "</tr>";
        panel += "</table>";
        panel += "</div>";
        table += "<tr>";
        table += "<td style=\"width:50%\">";
        table += panel;
        table += "</td>";
        table += "</tr>";
        table += "</table>"; // 总表格结束
        $('#divPrint').html(table);
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
            Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, "1");
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
    }
};