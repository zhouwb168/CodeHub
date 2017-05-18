$(function () {
    Events.FillPrintData();
});

var Events = {
    Table: 'WoodReport',

    PrintReport: function () {
        /*  打印木片检验结果通知单 */
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
        if (Events.GetQuery("ArrID") == null) {
            alert("参数无效。");
            return;
        }
        var params = Events.GetQuery("ArrID").split("|");
        var ArrID = params[0];
        var iswater = params[1];
        var data = {
            service: Events.Table,
            method: 'GetEntitysForReportPrint',
            args: [ArrID]
        };

        var success = function (result) {
            var root = Ajaxer.GetRoot(result);
            var grid = Ajaxer.GetGrid(root);
            if (iswater == 0) {
                //打印重量化验结果单
                Events.PrintWeight(grid);
            } else {
                //打印体化验结果单
                Events.PrintVolume(grid);
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

    //打印重量结果单
    PrintWeight: function (grid) {
        /* 下面开始拼凑html */
        var table = ""; // 总的表格
        /* 水份通知单一式两份，并排放在同一行，也就是总表格table，每行tr里面分两列td，每个td里面放一个通知单子 */
        table += "<table style=\"width: 100%\">"; // 总表格开始

        var panel = ""; // 每一个小通知单
        var row; // 行记录对象
        var noneValue = "&nbsp;";

        for (var i = 0; i < grid.total; i++) {
            row = grid.rows[i];

            /* 开始拼凑td里的通知单的html */
            panel = ""; // 因为这个panel变量要重复使用，所以每条不同的记录要清空一次，也就是复位
            panel += "<div style=\"width:303px; height:590px; letter-spacing:1px; margin-left:auto; margin-right:auto\">";
            panel += "<div style=\"color: #FF0000; width:100%; text-align:center; margin-top:30px; font-weight:bold\">广西百色丰林人造板有限公司</div>";
            panel += "<div style=\"border-bottom-style: solid; border-bottom-width: 1px; text-align:center;border-bottom-color: #FF0000; font-size: 16px; font-weight:bolder; color: #FF0000; margin-top: 10px; padding-bottom: 2px; margin-left:auto; margin-right:auto\">木质原料检验结果通知单</div>";
            panel += "<div style=\"width:100%; color: #FF0000; margin-top:10px\">";
            panel += "<table style=\"float:right; margin-right:20px\">";
            panel += "<tr>";
            panel += "<td style=\"color: #000000\">" + (row.Bang_Time == null ? noneValue : moment(row.Bang_Time).format('YYYY')) + "</td>";
            panel += "<td>年</td>";
            panel += "<td style=\"color: #000000\">" + (row.Bang_Time == null ? noneValue : moment(row.Bang_Time).format('MM')) + "</td>";
            panel += "<td>月</td>";
            panel += "<td style=\"color: #000000\">" + (row.Bang_Time == null ? noneValue : moment(row.Bang_Time).format('DD')) + "</td>";
            panel += "<td>日</td>";
            panel += "</tr>";
            panel += "</table>";
            panel += "</div>";
            panel += "<table cellpadding=\"5\" style=\"text-align:center; margin-left:auto; margin-right:auto;margin-top:5px;color: #FF0000\" class=\"print\">";
            panel += "<tr>";
            panel += "<td style=\"width: 60px\">进厂时间</td>";
            panel += "<td style=\"color: #000000; width: 65px\">" + (row.Bang_Time == null ? noneValue : moment(row.Bang_Time).format('HH:mm')) + "</td>";
            panel += "<td style=\"width: 50px\">车号</td>";
            panel += "<td style=\"color: #000000; width: 80px\">" + (row.carCID == null ? row.License : row.carCID) + "</td>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td>木片种类</td>";
            panel += "<td style=\"color: #000000\">" + row.Tree + "</td>";
            panel += "<td>采样人</td>";
            panel += "<td style=\"color: #000000\">FL-CZ</td>";
            //panel += "<td style=\"color: #000000\">" + (row.Sampler == null ? noneValue : row.Sampler.replace(/，/g, '<br />')) + "</td>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td>分析项目</td>";
            panel += "<td>指标(%)</td>";
            panel += "<td colspan=\"2\">结果(%)</td>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td style=\"line-height:26px\">木&nbsp;&nbsp;片<br />含水率</td>";
            panel += "<td style=\"color: #000000\">&nbsp;</td>";
            panel += "<td style=\"color: #000000\" colspan=\"2\">" + (row.RebateWater == null ? noneValue : row.RebateWater.toFixed(2) + "%") + "</td>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td>树皮含量</td>";
            panel += "<td>≤&nbsp;10%</td>";
            panel += "<td style=\"color: #000000\" colspan=\"2\">" + (row.RebateSkin == null ? noneValue : row.RebateSkin.toFixed(2) + "%") + "</td>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td>碎料含量</td>";
            panel += "<td>≤&nbsp;5%</td>";
            panel += "<td style=\"color: #000000\" colspan=\"2\">" + (row.RebateScrap == null ? noneValue : row.RebateScrap.toFixed(2) + "%") + "</td>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td>腐&nbsp;&nbsp;&nbsp;&nbsp;朽</td>";
            panel += "<td>≤&nbsp;4%</td>";
            panel += "<td style=\"color: #000000\" colspan=\"2\">" + (row.Bad == null ? noneValue : row.Bad.toFixed(2) + "%") + "</td>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td>过大木片</td>";
            panel += "<td>≤&nbsp;5%</td>";
            panel += "<td style=\"color: #000000\" colspan=\"2\">" + (row.Greater == null ? noneValue : row.Greater.toFixed(2) + "%") + "</td>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td>过小木片</td>";
            panel += "<td>≤&nbsp;20%</td>";
            panel += "<td style=\"color: #000000\" colspan=\"2\">" + (row.Less == null ? noneValue : row.Less.toFixed(2) + "%") + "</td>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td style=\"color: #000000\">" + (row.userXHName == null ? row.Supplier : row.userXHName) + "</td>";
            panel += "<td style=\"color: #000000\">&nbsp;</td>";
            panel += "<td style=\"color: #000000\" colspan=\"2\">" + (row.jWeight == null ? noneValue : row.jWeight + " 吨") + "</td>";
            panel += "</tr>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td style=\"color: #000000\">干吨</td>";
            panel += "<td style=\"color: #000000\">&nbsp;</td>";
            panel += "<td style=\"color: #000000\" colspan=\"2\">" + row.GWeight.toFixed(2) + " 吨</td>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td>扣减合计</td>";
            panel += "<td style=\"color: #000000\" colspan=\"3\">&nbsp;</td>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td style=\"text-align:left; height:90px\" colspan=\"4\">审核员：<span style=\"color: #000000\">" + (row.Description == null ? noneValue : $.parseJSON(row.Description).Name) + "</span></td>";
            panel += "</tr>";
            panel += "</table>";
            panel += "<div style=\"width:100%; text-align:center; margin-top:20px;color: #FF0000\">检验员：FL-JY</div>";
            panel += "</div>";
            // 新增加一行
            if ((i + 1) % 2 == 0) {
                table += "<td style=\"width:50%;border-bottom: 1px dashed #ddd;\">";
                table += panel;
                table += "</td>";
                table += "</tr>";
            } else {
                table += "<tr>";
                table += "<td style=\"width:50%;border-right: 1px dashed #ddd;border-bottom: 1px dashed #ddd;\">";
                table += panel;
                table += "</td>";
            }
        }
        table += "</table>"; // 总表格结束
        $('#divPrint').html(table);
    }
    ,
    //打印体积结果单
    PrintVolume: function (grid) {
        /* 下面开始拼凑html */
        var table = ""; // 总的表格
        /* 水份通知单一式两份，并排放在同一行，也就是总表格table，每行tr里面分两列td，每个td里面放一个通知单子 */
        table += "<table style=\"width: 100%\">"; // 总表格开始

        var panel = ""; // 每一个小通知单
        var row; // 行记录对象
        var noneValue = "&nbsp;";

        for (var i = 0; i < grid.total; i++) {
            row = grid.rows[i];

            /* 开始拼凑td里的通知单的html */
            panel = ""; // 因为这个panel变量要重复使用，所以每条不同的记录要清空一次，也就是复位
            panel += "<div style=\"width:303px; height:590px; letter-spacing:1px; margin-left:auto; margin-right:auto\">";
            panel += "<div style=\"color: #FF0000; width:100%; text-align:center; margin-top:30px; font-weight:bold\">广西百色丰林人造板有限公司</div>";
            panel += "<div style=\"border-bottom-style: solid; border-bottom-width: 1px; text-align:center;border-bottom-color: #FF0000; font-size: 16px; font-weight:bolder; color: #FF0000; margin-top: 10px; padding-bottom: 2px; margin-left:auto; margin-right:auto\">木质原料检验结果通知单</div>";
            panel += "<div style=\"width:100%; color: #FF0000; margin-top:10px\">";
            panel += "<table style=\"float:right; margin-right:20px\">";
            panel += "<tr>";
            panel += "<td style=\"color: #000000\">" + (row.Bang_Time == null ? noneValue : moment(row.Bang_Time).format('YYYY')) + "</td>";
            panel += "<td>年</td>";
            panel += "<td style=\"color: #000000\">" + (row.Bang_Time == null ? noneValue : moment(row.Bang_Time).format('MM')) + "</td>";
            panel += "<td>月</td>";
            panel += "<td style=\"color: #000000\">" + (row.Bang_Time == null ? noneValue : moment(row.Bang_Time).format('DD')) + "</td>";
            panel += "<td>日</td>";
            panel += "</tr>";
            panel += "</table>";
            panel += "</div>";
            panel += "<table cellpadding=\"5\" style=\"text-align:center; margin-left:auto; margin-right:auto;margin-top:5px;color: #FF0000\" class=\"print\">";
            panel += "<tr>";
            panel += "<td style=\"width: 60px\">进厂时间</td>";
            panel += "<td style=\"color: #000000; width: 65px\">" + (row.Bang_Time == null ? noneValue : moment(row.Bang_Time).format('HH:mm')) + "</td>";
            panel += "<td style=\"width: 50px\">车号</td>";
            panel += "<td style=\"color: #000000; width: 80px\">" + (row.carCID == null ? row.License : row.carCID) + "</td>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td>木片种类</td>";
            panel += "<td style=\"color: #000000\">" + row.Tree + "</td>";
            panel += "<td>采样人</td>";
            panel += "<td style=\"color: #000000\">FL-CZ</td>";
            //panel += "<td style=\"color: #000000\">" + (row.Sampler == null ? noneValue : row.Sampler.replace(/，/g, '<br />')) + "</td>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td>分析项目</td>";
            panel += "<td>指标(%)</td>";
            panel += "<td colspan=\"2\">结果(%)</td>";
            panel += "</tr>";
            if (row.Tree == "碎单板") {
                panel += "<tr>";
                panel += "<td style=\"line-height:26px\">树皮碎料<br />扣除</td>";
                panel += "<td style=\"color: #000000\">&nbsp;</td>";
                panel += "<td style=\"color: #000000\" colspan=\"2\"></td>";
                panel += "</tr>";
            } else {
                panel += "<tr>";
                panel += "<td style=\"line-height:26px\">树皮碎料<br />扣除</td>";
                panel += "<td style=\"color: #000000\">&nbsp;</td>";
                panel += "<td style=\"color: #000000\" colspan=\"2\">" + (row.DeductVolume == null ? noneValue : row.DeductVolume.toFixed(2) + "(方)") + "</td>";
                panel += "</tr>";
            }
            panel += "<tr>";
            panel += "<td>树皮含量</td>";
            panel += "<td>≤&nbsp;20%</td>";
            panel += "<td style=\"color: #000000\" colspan=\"2\">" + (row.Skin == null ? noneValue : row.Skin.toFixed(2) + "%") + "</td>";
            panel += "</tr>";

            panel += "<tr>";
            panel += "<td>碎料含量</td>";
            panel += "<td>≤&nbsp;5%</td>";
            panel += "<td style=\"color: #000000\" colspan=\"2\">" + (row.Scrap == null ? noneValue : row.Scrap.toFixed(2) + "%") + "</td>";
            panel += "</tr>";

            panel += "<tr>";
            panel += "<td>腐&nbsp;&nbsp;&nbsp;&nbsp;朽</td>";
            panel += "<td>≤&nbsp;4%</td>";
            panel += "<td style=\"color: #000000\" colspan=\"2\">" + (row.Bad == null ? noneValue : row.Bad.toFixed(2) + "%") + "</td>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td>过大木片</td>";
            panel += "<td>≤&nbsp;5%</td>";
            panel += "<td style=\"color: #000000\" colspan=\"2\">" + (row.Greater == null ? noneValue : row.Greater.toFixed(2) + "%") + "</td>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td>过小木片</td>";
            panel += "<td>≤&nbsp;20%</td>";
            panel += "<td style=\"color: #000000\" colspan=\"2\">" + (row.Less == null ? noneValue : row.Less.toFixed(2) + "%") + "</td>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td style=\"color: #000000\">" + (row.userXHName == null ? row.Supplier : row.userXHName) + "</td>";
            panel += "<td style=\"color: #000000\">&nbsp;</td>";
            panel += "<td style=\"color: #000000\" colspan=\"2\">" + (row.jWeight == null ? noneValue : row.jWeight + " 吨") + "</td>";
            panel += "</tr>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td style=\"color: #000000\">净体积</td>";
            panel += "<td style=\"color: #000000\">&nbsp;</td>";
            panel += "<td style=\"color: #000000\" colspan=\"2\">" + row.JVolume.toFixed(2) + " (方)</td>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td>扣减合计</td>";
            panel += "<td style=\"color: #000000\" colspan=\"3\">&nbsp;</td>";
            panel += "</tr>";
            panel += "<tr>";
            panel += "<td style=\"text-align:left; height:90px\" colspan=\"4\">审核员：<span style=\"color: #000000\">" + (row.Description == null ? noneValue : $.parseJSON(row.Description).Name) + "</span></td>";
            panel += "</tr>";
            panel += "</table>";
            panel += "<div style=\"width:100%; text-align:center; margin-top:20px;color: #FF0000\">检验员：FL-JY</div>";
            panel += "</div>";
            // 新增加一行
            if ((i + 1) % 2 == 0) {
                table += "<td style=\"width:50%;border-bottom: 1px dashed #ddd;\">";
                table += panel;
                table += "</td>";
                table += "</tr>";
            } else {
                table += "<tr>";
                table += "<td style=\"width:50%;border-right: 1px dashed #ddd;border-bottom: 1px dashed #ddd;\">";
                table += panel;
                table += "</td>";
            }
        }
        table += "</table>"; // 总表格结束
        $('#divPrint').html(table);
    }
};