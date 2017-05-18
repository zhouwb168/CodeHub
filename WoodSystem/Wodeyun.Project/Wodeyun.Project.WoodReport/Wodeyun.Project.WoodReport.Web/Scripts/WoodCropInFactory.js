$(function () {
    var grid = $('#Grid');
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    grid.datagrid("getColumnOption", "Bang_Time").formatter = Events.ToDateTime;
    grid.datagrid("getColumnOption", "jWeight").formatter = Events.ToFixed;

    var aLinkS = $('#Buttons').find('a');
    var linkCount = aLinkS.length;
    for (var i = 0; i < linkCount; i++) {
        aLinkS.eq(i).bind('click', { index: i }, Events.OnClick);
    }
    for (var i = 2; i < linkCount; i++) {
        aLinkS.eq(i).find('font').html(moment().add('months', -i).format('MM') + '月');
    }

    $('#Year').numberspinner('setValue', moment().add('years', 0).format('YYYY'));
    $('#Month').combobox('setValue', moment().format('M'));

    $('#Query').bind('click', Events.OnQuery);
    $('#Excel').bind('click', Events.OnExcel);

    Events.GetGrid(Events.mergeCellsByField);
});

var Events = {
    Service: 'WoodReport',

    ResetPager: function () {
        var grid = $('#Grid');
        grid.datagrid('options').pageNumber = 1;
        grid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });
    },

    ToFixed: function (value) {
        if (value == null) return '';
        return value.toFixed(2);
    },

    Toscale: function (value) {
        if (value == null) return '';
        return (value * 100);
    },

    ToDateTime: function (value) {
        if (value == null) return '';
        var arryDate = value.split('-');
        return parseInt(arryDate[1], 10) + "月" + arryDate[2] + "日";
    },

    GetMonth: function () {
        var aLinkS = $('#Buttons').find('a');
        var linkCount = aLinkS.length;
        for (var i = 0; i < linkCount; i++) {
            if (aLinkS.eq(i).find('font').attr('color') == '#ff0000') return moment().add('months', -i).format('YYYY-MM');
        }
        return Eventer.Get($('#Year')) + '-' + Eventer.Get($('#Month'));
    },

    GetGrid: function (callback) {
        var grid = $('#Grid');
        var gridOptons = grid.datagrid('options');
        var pageSize = gridOptons.pageSize;
        var rowStart = (gridOptons.pageNumber - 1) * pageSize + 1;

        var month = Events.GetMonth();
        Eventer.Grid(grid, Events.Service, 'WoodAreaInFactoryReport', [month, rowStart, pageSize], callback);
    },

    Page: function () {
        Events.GetGrid(Events.mergeCellsByField);
    },

    OnClick: function (event) {
        var month = moment().add('months', -event.data.index).format('YYYY-MM');
        var itemsOfAlink = $('#Buttons').find('a');
        var itemCount = itemsOfAlink.length;
        for (var i = 0; i < itemCount; i++) {
            itemsOfAlink.eq(i).find('font').attr('color', (month == moment().add('months', -i).format('YYYY-MM') ? 'red' : 'normal'));
        }

        Events.ResetPager();

        Events.GetGrid(Events.mergeCellsByField);
    },
    /**
    * EasyUI DataGrid根据字段动态合并单元格
    * 参数 tableID 要合并table的id
    * 参数 colList 要合并的列,用逗号分隔(例如："name,department,office");
    */
    mergeCellsByField: function () {
        var tableID = "Grid", colList = "Area,SumjWeight,Totalscale";
        var ColArray = colList.split(",");
        var tTable = $("#" + tableID);
        var TableRowCnts = tTable.datagrid("getRows").length;
        var tmpA;
        var tmpB;
        var PerTxt = "";
        var CurTxt = "";
        var alertStr = "";
        for (j = ColArray.length - 1; j >= 0; j--) {
            PerTxt = "";
            tmpA = 1;
            tmpB = 0;
            for (i = 0; i <= TableRowCnts; i++) {
                if (i == TableRowCnts) {
                    CurTxt = "";
                }
                else {
                    CurTxt = tTable.datagrid("getRows")[i][ColArray[j]];
                }
                if (PerTxt == CurTxt) {
                    tmpA += 1;
                }
                else {
                    tmpB += tmpA;

                    tTable.datagrid("mergeCells", {
                        index: i - tmpA,
                        field: ColArray[j],　　//合并字段
                        rowspan: tmpA,
                        colspan: null
                    });
                    tTable.datagrid("mergeCells", { //根据ColArray[j]进行合并
                        index: i - tmpA,
                        field: "Ideparture",
                        rowspan: tmpA,
                        colspan: null
                    });

                    tmpA = 1;
                }
                PerTxt = CurTxt;
            }
        }
        if (tTable.datagrid("getRows").length > 0) {
            tTable.datagrid('appendRow', { Area: '<b style=color:red>总计</b>', SumjWeight: tTable.datagrid("getRows")[0].TotaljWeight, Totalscale: 100 });
        }
    },

    OnQuery: function () {
        var itemsOfAlink = $('#Buttons').find('a');
        var itemCount = itemsOfAlink.length;
        for (var i = 0; i < itemCount; i++) {
            itemsOfAlink.eq(i).find('font').attr('color', 'normal');
        }

        Events.ResetPager();

        Events.GetGrid(Events.mergeCellsByField);
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
            var SetValueAndStyle = function (cell, row, column) {
                var result = row[column];
                switch (column) {
                    case "Bang_Time":
                        result = Events.ToDateTime(result)
                        break;
                    case "jWeight":
                        result = Events.ToFixed(result)
                        break;
                    case "SumjWeight":
                        result = Events.ToFixed(result)
                        break;
                    default:
                        break;
                }
                cell.Value = (result == null ? '' : $('<div>' + result + '</div>').text());
            };

            var GetFilter = function () {
                return '月份：' + Events.GetMonth();
            };

            var excel;
            try {
                excel = new ActiveXObject('Excel.Application');
            }
            catch (exception) {
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

            sheet.Cells(1, 1) = '各区域品种进柴统计';
            sheet.Cells(2, 1) = GetFilter();

            var grid = $('#Grid');
            var columns = grid.datagrid('getColumnFields');
            var columnCount = columns.length;
            for (var i = 0; i < columnCount; i++) {
                sheet.Cells(4, i + 1).Value = grid.datagrid('getColumnOption', columns[i]).title;
            }

            sheet.Range('A1:' + 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(columnCount - 1, columnCount) + '1').MergeCells = true;
            sheet.Range('A2:' + 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(columnCount - 1, columnCount) + '2').MergeCells = true;
            sheet.Cells(1, 1).HorizontalAlignment = 3;
            sheet.Cells(2, 1).HorizontalAlignment = 3;
            sheet.Rows(1).Font.Bold = true;
            for (var i = 0; i < columnCount; i++) {
                var column = grid.datagrid('getColumnOption', columns[i]);

                sheet.Columns(i + 1).ColumnWidth = column.width / 16;
                sheet.Columns(i + 1).Font.Size = 9;
                sheet.Columns(i + 1).WrapText = true;
            }
            sheet.Rows(4).Font.Bold = true;

            var rows = grid.datagrid('getRows');
            var rowCount = rows.length;
            if (rowCount > 0) {
                grid.datagrid('appendRow', { Area: '<b style=color:red>总计</b>', SumjWeight: grid.datagrid("getRows")[0].TotaljWeight, Totalscale: 100 });
            }
            var currentRowIndex = 4; // 当前数据行的索引
            for (var i = 0; i < rowCount; i++) {
                currentRowIndex += 1;
                for (var j = 0; j < columnCount; j++) {
                    SetValueAndStyle(sheet.Cells(4 + i + 1, j + 1), rows[i], columns[j]);
                }
            }
            SetValueAndStyle(sheet.Cells(4 + rowCount + 1, 1), rows[rowCount], columns[0]);
            SetValueAndStyle(sheet.Cells(4 + rowCount + 1, 4), rows[rowCount], columns[3]);
            sheet.Range('A4:' + 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(columnCount - 1, columnCount) + currentRowIndex).Borders.Weight = 2; // 设置单元格边框
        };

        Events.GetGrid(callback);
    }
};
