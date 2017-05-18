$(function () {
    var grid = $('#Grid');
    grid.datagrid({ onLoadSuccess: Events.OnLoadSuccess });
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    grid.datagrid("getColumnOption", "Col1").formatter = Events.ToTextForColumn1;
    grid.datagrid("getColumnOption", "Col2").formatter = Events.ToTextForColumn2;
    grid.datagrid("getColumnOption", "Col3").formatter = Events.ToTextForColumn3;
    grid.datagrid("getColumnOption", "Col4").formatter = Events.ToTextForColumn4;
    grid.datagrid("getColumnOption", "Col5").formatter = Events.ToTextForColumn5;
    grid.datagrid("getColumnOption", "Col6").formatter = Events.ToTextForColumn6;
    grid.datagrid("getColumnOption", "Col7").formatter = Events.ToTextForColumn7;
    grid.datagrid("getColumnOption", "Water").formatter = Events.ToFixed;
    grid.datagrid("getColumnOption", "Water").styler = Events.ToBgStyler;
    grid.datagrid("getColumnOption", "Skin").formatter = Events.ToFixed;
    grid.datagrid("getColumnOption", "Skin").styler = Events.ToBgStyler;
    grid.datagrid("getColumnOption", "Scrap").formatter = Events.ToFixed;
    grid.datagrid("getColumnOption", "Scrap").styler = Events.ToBgStyler;
    grid.datagrid("getColumnOption", "User").formatter = Events.ToText;
    grid.datagrid("getColumnOption", "User").styler = Events.ToBgStyler;
    grid.datagrid("getColumnOption", "Col8").formatter = Events.ToTextForColumn8;
    grid.datagrid("getColumnOption", "Col8").styler = Events.ToStyler;
    grid.datagrid("getColumnOption", "Col9").formatter = Events.ToTextForColumn9;
    grid.datagrid("getColumnOption", "Col9").styler = Events.ToStyler;
    grid.datagrid("getColumnOption", "Co20").formatter = Events.ToTextForColumn20;
    grid.datagrid("getColumnOption", "Co21").formatter = Events.ToTextForColumn21;
    grid.datagrid("getColumnOption", "Co22").formatter = Events.ToTextForColumn22;

    var aLinkS = $('#Buttons').find('a');
    var linkCount = aLinkS.length;
    for (var i = 0; i < linkCount; i++) {
        aLinkS.eq(i).bind('click', { index: i }, Events.OnClick);
    }
    for (var i = 2; i < linkCount; i++) {
        aLinkS.eq(i).find('font').html(moment().add('days', -i).format('DD') + '日');
    }

    $('#Query').bind('click', Events.OnQuery);
    $('#Excel').bind('click', Events.OnExcel);

    $('#Date').datebox('setValue', moment().add('days', -12).format('YYYY-MM-DD'));

    Events.GetGrid(null);
});

var Events = {
    Service: 'WoodReport',

    ToTextForColumn22: function (value, row, rowIndex) {
        if (row.Less == null) return '';

        return row.Less.toFixed(2) + "%";
    },

    ToTextForColumn21: function (value, row, rowIndex) {
        if (row.Greater == null) return '';

        return row.Greater.toFixed(2) + "%";
    },

    ToTextForColumn20: function (value, row, rowIndex) {
        if (row.Bad == null) return '';

        return row.Bad.toFixed(2) + "%";
    },

    ResetPager: function () {
        var grid = $('#Grid');
        grid.datagrid('options').pageNumber = 1;
        grid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });
    },

    ToTextForColumn9: function (value, row, rowIndex) {
        if (row.Remark == null) return '';

        var htmlValue = row.Remark.replace(/＃/g, '<br /><br />');
        htmlValue += "<br /><br />";
        return htmlValue;
    },

    ToStyler: function (value, row, rowIndex) {
        return 'background-color:#FFFF99;';
    },

    ToBgStyler: function (value, row, rowIndex) {
        var bgColor = 'background-color:#FFFFFF;';
        if (row.IsMain == "1") {
            bgColor = 'background-color:#CCFFFF;';
        }
        else if (row.IsLast == "1") {
            bgColor = 'background-color:#FFCC99;';
        }

        return bgColor;
    },

    ToTextForColumn1: function (value, row, rowIndex) {
        return row.WeighTime;
    },

    ToTextForColumn2: function (value, row, rowIndex) {
        return row.License;
    },

    ToTextForColumn3: function (value, row, rowIndex) {
        if (row.Key == null) return '';

        var htmlValue = row.Key.replace(/，/g, '<br />');
        return htmlValue;
    },

    ToTextForColumn4: function (value, row, rowIndex) {
        return row.Tree;
    },

    ToTextForColumn5: function (value, row, rowIndex) {
        return row.jWeight;
    },

    ToTextForColumn6: function (value, row, rowIndex) {
        return row.firstBangUser;
    },

    ToTextForColumn7: function (value, row, rowIndex) {
        if (row.Bang_Time != null) return moment(row.Bang_Time).format('HH:mm');

        return row.Bang_Time;
    },

    ToTextForColumn8: function (value, row, rowIndex) {
        if (row.Sampler == null) return '';

        var htmlValue = row.Sampler.replace(/，/g, '<br />');
        return htmlValue;
    },

    ToFixed: function (value, row, rowIndex) {
        if (value == null) return '';

        return value.toFixed(2) + "%";
    },

    ToText: function (value, row, rowIndex) {
        if (value == null) return '';

        var htmlValue = $.parseJSON(value).Name;
        if (row.IsMain == "0" && row.IsLast == "0") {
            htmlValue = "<span style=\"color:red;\">" + htmlValue + "</span>";
        }

        return htmlValue;
    },

    OnLoadSuccess: function (data) {
        if (data.rows.length > 0) setTimeout("Events.MergeCellsByField('Grid', 'Col9,Col8,Co22,Co21,Co20,Col7,Col6,Col5,Col4,Col3,Col2,Col1')", 1000);
    },

    MergeCellsByField: function (tableID, colList) {
        /* 根据字段列表合并jquery--easyui--datagrid中的相应列
         参数：tableID - 要操作的table的id；colList - 要合并的列的列表,用逗号分隔（例如："name,id,code"） */
        var ColArray = colList.split(",");
        var tTable = $('#' + tableID);
        var rowItems = tTable.datagrid("getRows");
        var TableRowCnts = rowItems.length;
        var tmpA;
        var tmpB;
        var PerTxt = "";
        var CurTxt = "";
        var alertStr = "";

        for (var j = ColArray.length - 1; j >= 0 ; j--) {
            /* 当循环至某新的列时，变量复位 */
            PerTxt = "";
            tmpA = 1;
            tmpB = 0;

            /* 从第一行（表头为第0行）开始循环，循环至行尾(溢出一位) */
            for (var i = 0; i <= TableRowCnts; i++) {
                if (i == TableRowCnts) CurTxt = "";
                else CurTxt = rowItems[i][ColArray[j]];
                if (PerTxt == CurTxt) tmpA += 1;
                else {
                        tTable.datagrid('mergeCells',{ 
                            index:i - tmpA,
                            field:ColArray[j],
                            rowspan:tmpA, 
                            colspan: null
                        });
                        tmpA = 1;
                }
                PerTxt = CurTxt;
            }
        }
    },

    GetDate: function () {
        var aLinkS = $('#Buttons').find('a');
        var linkCount = aLinkS.length;
        for (var i = 0; i < linkCount; i++) {
            if (aLinkS.eq(i).find('font').attr('color') == '#ff0000') return moment().add('days', -i).format('YYYY-MM-DD');
        }

        return Eventer.Get($('#Date'));
    },

    GetGrid: function (callback) {
        var grid = $('#Grid');
        var gridOptons = grid.datagrid('options');
        var pageSize = gridOptons.pageSize;
        var rowStart = (gridOptons.pageNumber - 1) * pageSize + 1;
        var date = Events.GetDate();

        Eventer.Grid(grid, Events.Service, 'GetReport04ByDateAndStartAndLength', [date, rowStart, pageSize], callback);
    },

    Page: function () {
        Events.GetGrid(null);
    },

    OnClick: function (event) {
        var date = moment().add('days', -event.data.index).format('YYYY-MM-DD');
        var itemsOfAlink = $('#Buttons').find('a');
        var itemCount = itemsOfAlink.length;
        for (var i = 0; i < itemCount; i++) {
            itemsOfAlink.eq(i).find('font').attr('color', (date == moment().add('days', -i).format('YYYY-MM-DD') ? 'red' : 'normal'));
        }

        Events.ResetPager();

        Events.GetGrid(null);
    },

    OnQuery: function () {
        var itemsOfAlink = $('#Buttons').find('a');
        var itemCount = itemsOfAlink.length;
        for (var i = 0; i < itemCount; i++) {
            itemsOfAlink.eq(i).find('font').attr('color', 'normal');
        }

        Events.ResetPager();

        Events.GetGrid(null);
    },

    OnExcel: function () {
        if (confirm("该操作将花费较多的时间，数据量较大时甚至可能会超过10分钟，如果您的电脑性能较差，则不建议操作。是否继续？\n\n请注意，在等待的过程中，请不要随意点击，否则可能会终止操作。") == false) return;

        var grid = $('#Grid');
        var gridOptons = grid.datagrid('options');
        gridOptons.pageNumber = 1;
        gridOptons.pageSize = Setter.Max;
        grid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1,
            pageSize: Setter.Max
        });

        var callback = function () {
            var SetValueAndStyle = function (cell, row, column, isSame) {
                var result = row[column];
                var strValue = "";

                /* 不同一组时，才填充所有字段 */
                if (!isSame) {
                    switch (column) {
                        case "Col1": {
                            strValue = row.WeighTime;
                            result = strValue;
                            break;
                        }
                        case "Col2": {
                            strValue = row.License;
                            result = strValue;
                            break;
                        }
                        case "Col3": {
                            strValue = row.Key == null ? '' : row.Key;
                            result = strValue;
                            break;
                        }
                        case "Col4": {
                            strValue = row.Tree;
                            result = strValue;
                            break;
                        }
                        case "Col5": {
                            strValue = row.jWeight == null ? '' : row.jWeight;
                            result = strValue;
                            break;
                        }
                        case "Col7": {
                            strValue = row.Bang_Time == null ? '' : moment(row.Bang_Time).format('HH:mm');
                            result = strValue;
                            break;
                        }
                        case "Col8": {
                            cell.Interior.ColorIndex = 36; // 设置底色
                            strValue = row.Sampler == null ? '' : row.Sampler;
                            result = strValue;
                            break; 
                        }
                        case "Col9": {
                            cell.Interior.ColorIndex = 36; // 设置底色
                            if (row.Remark != null) {
                                strValue = row.Remark.replace(/＃/g, '\n');
                                strValue += "\n";
                                result = strValue;
                            }
                            else result = '';
                            break; 
                        }
                        case "Col6": {
                            strValue = row.firstBangUser;
                            result = strValue;
                            break;
                        }
                        case "Co20": {
                            strValue = row.Bad == null ? '' : row.Bad.toFixed(2) + "%";
                            result = strValue;
                            break;
                        }
                        case "Co21": {
                            strValue = row.Greater == null ? '' : row.Greater.toFixed(2) + "%";
                            result = strValue;
                            break;
                        }
                        case "Co22": {
                            strValue = row.Less == null ? '' : row.Less.toFixed(2) + "%";
                            result = strValue;
                            break;
                        }
                        default: {
                            break;
                        }
                    }
                }
                else {
                    result = ''; // 否则，留空，不填充
                }

                switch (column) {
                    case "Water": {
                        if (row.IsMain == "1") {
                            cell.Interior.ColorIndex = 20; // 设置底色
                        }
                        else if (row.IsLast == "1") {
                            cell.Interior.ColorIndex = 40; // 设置底色
                        }
                        strValue = row.Water == null ? '' : row.Water.toFixed(2) + "%";
                        result = strValue;
                        break;
                    }
                    case "Skin": {
                        if (row.IsMain == "1") {
                            cell.Interior.ColorIndex = 20; // 设置底色
                        }
                        else if (row.IsLast == "1") {
                            cell.Interior.ColorIndex = 40; // 设置底色
                        }
                        strValue = row.Skin == null ? '' : row.Skin.toFixed(2) + "%";
                        result = strValue;
                        break;
                    }
                    case "Scrap": {
                        if (row.IsMain == "1") {
                            cell.Interior.ColorIndex = 20; // 设置底色
                        }
                        else if (row.IsLast == "1") {
                            cell.Interior.ColorIndex = 40; // 设置底色
                        }
                        strValue = row.Scrap == null ? '' : row.Scrap.toFixed(2) + "%";
                        result = strValue;
                        break;
                    }
                    case "User": {
                        if (row.IsMain == "1") {
                            cell.Interior.ColorIndex = 20; // 设置底色
                        }
                        else if (row.IsLast == "1") {
                            cell.Interior.ColorIndex = 40; // 设置底色
                        }
                        else {
                            cell.Font.ColorIndex = 3; // 设置字体颜色
                        }
                        strValue = row.User == null ? '' : $.parseJSON(row.User).Name;
                        result = strValue;
                        break;
                    }
                    default: {
                        break;
                    }
                }

                cell.Value = (result == null ? '' : result);
            };

            var GetFilter = function () {
                return '日期：' + Events.GetDate();
            };

            var excel;
            try
            {
                excel = new ActiveXObject('Excel.Application');
            }
            catch (exception)
            {
                alert('您的浏览器不允许自动启动 Excel。\n'
                    + '请确认您已经安装有 Excel，并请按以下操作方式设置浏览器：\n'
                    + '1、在浏览器中点击“工具”菜单。\n'
                    + '2、在弹出的菜单中点击“Internet 选项”。\n'
                    + '3、在弹出的窗口中点击“安全”页签。\n'
                    + '4、在打开的页签中点击“自定义级别”按钮。\n'
                    + '5、在弹出的窗口中找到“ActiveX 控件和插件”下面的“对未标记为可安全执行脚本的 ActiveX 控件初始化并执行脚本”。\n'
                    + '6、点击上述项目下的“提示”单选框。\n'
                    + '7、点击“确定”按钮关闭所有弹出的窗口。');
                return;
            }
            var workbook = excel.Workbooks.Add();
            var sheet = workbook.ActiveSheet;

            excel.Visible = true;
            excel.UserControl = true;

            sheet.Cells(1, 1) = '木片水份检测日分析报表'; // 主标题
            sheet.Cells(2, 1) = GetFilter(); // 副标题

            sheet.Cells(4, 8).Value = "目测";
            sheet.Cells(4, 8).HorizontalAlignment = 3;
            sheet.Range('H4:K4').MergeCells = true;

            sheet.Cells(4, 12).Value = "对比分析";
            sheet.Cells(4, 12).HorizontalAlignment = 3;
            sheet.Cells(4, 12).Interior.ColorIndex = 20; // 设置底色 
            sheet.Range('L4:M4').MergeCells = true;

            /* 填充标题 */
            var grid = $('#Grid');
            var columns = grid.datagrid('getColumnFields');
            var columnCount = columns.length;
            for (var i = 0; i < columnCount; i++) {
                sheet.Cells(5, i + 1).Value = grid.datagrid('getColumnOption', columns[i]).title;
            }

            sheet.Cells(5, 12).Interior.ColorIndex = 20; // 设置底色
            sheet.Cells(5, 13).Interior.ColorIndex = 20; // 设置底色 

            /* 合并主标题行、副标题行，并设置居中对齐显示 */
            sheet.Range('A1:' + 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(columnCount - 1, columnCount) + '1').MergeCells = true;
            sheet.Range('A2:' + 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(columnCount - 1, columnCount) + '2').MergeCells = true;
            sheet.Cells(1, 1).HorizontalAlignment = 3;
            sheet.Cells(2, 1).HorizontalAlignment = 3;

            sheet.Rows(1).Font.Bold = true;
            sheet.Rows(4).Font.Bold = true;
            sheet.Rows(5).Font.Bold = true;

            /* 设置各列的宽度、字体大小和允许换行显示 */
            for (var i = 0; i < columnCount; i++) {
                var column = grid.datagrid('getColumnOption', columns[i]);

                sheet.Columns(i + 1).ColumnWidth = column.width / 8;
                sheet.Columns(i + 1).Font.Size = 9;
                sheet.Columns(i + 1).WrapText = true;
            }

            /* 合并标题行 */
            for (var i = 0; i < 7; i++) {
                sheet.Range('ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(i, i + 1) + '4:' + 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(i, i + 1) + '5').MergeCells = true;
            }

            var rows = grid.datagrid('getRows');
            var currentGroup = ""; // 当前组标识
            var prevGroup = ""; // 前一组标识
            var startRowIndexForMerge = 6; // 要合并行的开始索引号
            var endRowIndexForMerge = 0; // 要合并行的结束索引号
            var currentRowIndex = 5; // 当前数据行的索引
            var carNumber = 0; // 车的数量
            var totalWater = 0.00; // 总的实际含水率
            var totalSkin = 0.00; // 总的实际树皮含量
            var totalScrap = 0.00; // 总的实际碎屑含量
            var avgNumber = 0; // 实际结果记录数
            var totalRebateWater = 0.00; // 总的折后含水率
            var numberOfRebateWater = 0; // 折后含水率记录数
            var totalRebateSkin = 0.00; // 总的折后树皮含量
            var numberOfRebateSkin = 0; // 折后树皮含量记录数
            var totalRebateScrap = 0.00; // 总的折后碎屑含量
            var numberOfRebateScrap = 0; // 折后碎屑含量记录数
            var rowCount = rows.length;
            for (var i = 0; i < rowCount; i++) {
                if (parseInt(rows[i].IsMain) == 1) {
                    /* 为实际结果时，车数加1，实际结果记录数加1，并且累加实际结果的含水率、树皮含量、碎屑含量 */
                    ++carNumber;
                    if (rows[i].Water != null) {
                        totalWater += parseFloat(rows[i].Water);
                        ++avgNumber;
                    }
                    if (rows[i].Skin != null) totalSkin += parseFloat(rows[i].Skin);
                    if (rows[i].Scrap != null) totalScrap += parseFloat(rows[i].Scrap);
                }
                else if (parseInt(rows[i].IsLast) == 1) {
                    /* 为调整后时，累加调整后的含水率、树皮含量、碎屑含量 */
                    if (rows[i].Water != null) {
                        totalRebateWater += parseFloat(rows[i].Water);
                        ++numberOfRebateWater;
                    }
                    if (rows[i].Skin != null) {
                        totalRebateSkin += parseFloat(rows[i].Skin);
                        ++numberOfRebateSkin;
                    }
                    if (rows[i].Scrap != null) {
                        totalRebateScrap += parseFloat(rows[i].Scrap);
                        ++numberOfRebateScrap;
                    }
                }

                currentRowIndex += 1;
                currentGroup = rows[i].WoodID; // 获取当前记录的组标识

                /* 如果不属于当前组，则表示上一组已完成，换另一组 */
                if (currentGroup != prevGroup) {

                    /* 如果不是第一条记录，则合并行向单元格 */
                    if (i != 0) {
                        for (var j = 0; j < 7; j++) {
                            sheet.Range('ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(j, j + 1) + startRowIndexForMerge + ':' + 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(j, j + 1) + (currentRowIndex - 1)).MergeCells = true;
                        }

                        /* 最后两列的行也要合并 */
                        for (var j = 11; j < 13; j++) {
                            sheet.Range('ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(j, j + 1) + startRowIndexForMerge + ':' + 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(j, j + 1) + (currentRowIndex - 1)).MergeCells = true;
                        }

                        startRowIndexForMerge = currentRowIndex; // 合并完一组后，要重新把合并行的开始索引号指向当前数据行的索引
                    }
                }

                /* 向单元格中填写内容 */
                for (var j = 0; j < columnCount; j++) {
                    SetValueAndStyle(sheet.Cells(currentRowIndex, j + 1), rows[i], columns[j], prevGroup == currentGroup);
                }

                prevGroup = currentGroup;

                /* 最后一条记录时，行向单元格也要合并 */
                if (i == rowCount - 1) {
                    for (var j = 0; j < 7; j++) {
                        sheet.Range('ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(j, j + 1) + startRowIndexForMerge + ':' + 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(j, j + 1) + currentRowIndex).MergeCells = true;
                    }

                    /* 最后两列的行向单元格也要合并 */
                    for (var j = 11; j < 13; j++) {
                        sheet.Range('ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(j, j + 1) + startRowIndexForMerge + ':' + 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(j, j + 1) + currentRowIndex).MergeCells = true;
                    }
                }
            }

            sheet.Range('A4:' + 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(columnCount - 1, columnCount) + currentRowIndex).Borders.Weight = 2; // 设置单元格边框

            sheet.Cells(currentRowIndex + 2, 1).Value = carNumber + "车";

            sheet.Cells(currentRowIndex + 2, 8).Font.ColorIndex = 3; // 设置字体颜色
            sheet.Cells(currentRowIndex + 2, 8).Value = totalWater > 0 ? (totalWater / avgNumber).toFixed(2) : "0.00";
            sheet.Cells(currentRowIndex + 2, 9).Font.ColorIndex = 3; // 设置字体颜色
            sheet.Cells(currentRowIndex + 2, 9).Value = totalSkin > 0 ? (totalSkin / avgNumber).toFixed(2) : "0.00";
            sheet.Cells(currentRowIndex + 2, 10).Font.ColorIndex = 3; // 设置字体颜色
            sheet.Cells(currentRowIndex + 2, 10).Value = totalScrap > 0 ? (totalScrap / avgNumber).toFixed(2) : "0.00";

            sheet.Cells(currentRowIndex + 3, 8).Font.ColorIndex = 3; // 设置字体颜色
            sheet.Cells(currentRowIndex + 3, 8).Value = totalRebateWater > 0 ? (totalRebateWater / numberOfRebateWater).toFixed(2) : "0.00";
            sheet.Cells(currentRowIndex + 3, 9).Font.ColorIndex = 3; // 设置字体颜色
            sheet.Cells(currentRowIndex + 3, 9).Value = totalRebateSkin > 0 ? (totalRebateSkin / numberOfRebateSkin).toFixed(2) : "0.00";
            sheet.Cells(currentRowIndex + 3, 10).Font.ColorIndex = 3; // 设置字体颜色
            sheet.Cells(currentRowIndex + 3, 10).Value = totalRebateScrap > 0 ? (totalRebateScrap / numberOfRebateScrap).toFixed(2) : "0.00";
        };

        Events.GetGrid(callback);
    }
};
