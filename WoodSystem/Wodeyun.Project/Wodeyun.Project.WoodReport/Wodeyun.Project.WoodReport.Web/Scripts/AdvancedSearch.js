$(function () {
    var grid = $('#Grid');
    grid.datagrid({
        rowStyler: function (index, row) {
            if (row.IsAdd == 1) {
                return 'background-color:pink;color:red;font-weight:bold;';
            }
        }
    });
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    grid.datagrid("getColumnOption", "Date").formatter = Events.ToDateTime;
    grid.datagrid("getColumnOption", "Ship").formatter = Events.ToDateTime;
    grid.datagrid("getColumnOption", "Bang_Time").formatter = Events.ToDateTime;
    grid.datagrid("getColumnOption", "BackWeighTime").formatter = Events.ToDateTime;
    grid.datagrid("getColumnOption", "Text").formatter = Events.ToTip;
    grid.datagrid("getColumnOption", "IsAdd").formatter = Events.ToIsAdd;
    grid.datagrid("getColumnOption", "jVolume").formatter = Events.ToFixed;

    $('#txtStartDate').datebox('setValue', moment().format('YYYY-MM-DD'));
    $('#txtEndDate').datebox('setValue', moment().format('YYYY-MM-DD'));

    $('#Query').bind('click', Events.OnQuery);
    $('#Excel').bind('click', Events.OnExcel);

    Events.GetGrid(Events.compute);
});

var Events = {
    Service: 'WoodReport',

    ToTip: function (value, row, rowIndex) {
        if (value == null) return '';
        return '<span title="' + value + '">' + value + '</span>';
    },

    ToWetherRebate: function (value) {
        if (value == null) return '';

        var htmlValue = value ? "<span style=\"color:red;font-weight:bold\">是</span>" : "<span style=\"font-weight:bold\">否</span>";

        return htmlValue;
    },

    //是否补报
    ToIsAdd: function (value) {
        if (value == null) return '';
        var htmlValue = value == 1 ? "<span style=\"color:red;\">是</span>" : "否";
        return htmlValue;
    },

    ToRedFixed: function (value, row, rowIndex) {
        if (value == null) return '';

        if (!row.IsRebate) return "";

        var htmlValue = value.toFixed(2) + "%";
        htmlValue = "<span style=\"color:red;\">" + htmlValue + "</span>";

        return htmlValue;
    },

    ToFontRed: function (value, row, rowIndex) {
        if (value == null) return '';

        if (!row.IsRebate) return "";

        return "<span style=\"color:red;font-weight:bold\">" + value + "</span>";
    },

    ToFontBold: function (value, row, rowIndex) {
        if (value == null) return '';

        if (!row.IsRebate) return "";

        return "<span style=\"font-weight:bold\">" + value + "</span>";
    },

    ToDateTimeForWeight: function (value) {
        if (value == null) return '';

        return moment(value).format('HH:mm');
    },

    ToTimeForShip: function (value, row, rowIndex) {
        if (value == null) return '';

        //var htmlValue = moment(value).format('HH:mm');
        //var shipDate = moment(value).format('YYYY/MM/DD');
        //var shipDateTime = new Date(Date.parse(shipDate + " 23:59:59")).getTime();
        //var gsmDate = moment(row.Date).format('YYYY-MM-DD HH:mm:ss');
        //var gsmDateTime = new Date(Date.parse(gsmDate.replace(/-/g, "/"))).getTime();
        //if (shipDateTime < gsmDateTime) {
        //    htmlValue = "<span style=\"color:red;font-weight:bold\">" + htmlValue + "</span>";
        //}

        //return htmlValue;

        return moment(value).format('HH:mm');
    },

    ToDateTimeForShip: function (value, row, rowIndex) {
        if (value == null) return '';

        //var htmlValue = moment(value).format('YYYY/MM/DD');
        //var shipDateTime = new Date(Date.parse(htmlValue + " 23:59:59")).getTime();
        //var gsmDate = moment(row.Date).format('YYYY-MM-DD HH:mm:ss');
        //var gsmDateTime = new Date(Date.parse(gsmDate.replace(/-/g, "/"))).getTime();
        //if (shipDateTime < gsmDateTime) {
        //    htmlValue = "<span style=\"color:red;font-weight:bold\">" + htmlValue + "</span>";
        //}

        //return htmlValue;

        return moment(value).format('YYYY/MM/DD HH:mm');
    },

    ToTime: function (value, row, rowIndex) {
        if (value == null) return '';

        return moment(value).format('HH:mm');
    },

    ToFixed: function (value, row, rowIndex) {
        if (value == null) return '';
        if (value.toString().indexOf('red') != -1) return value;
        var htmlValue = value.toFixed(2);
        return htmlValue;
    },

    ToDateTime: function (value, row, rowIndex) {
        if (value == null) return '';
        var date = moment(value).format('YYYY/MM/DD HH:mm');
        if (date == '1900/01/01 00:00') return '';
        return date;
    },
    GetGrid: function (callback) {
        var startDate = Eventer.Get($('#txtStartDate'));
        var endDate = Eventer.Get($('#txtEndDate'));
        var License = Eventer.Get($('#txtLicense'));
        var chkLicense = document.getElementById("chkLicense").checked;
        var Driver = Eventer.Get($('#txtDriver'));
        var chkDriver = document.getElementById("chkDriver").checked;
        var PoundSupplier = Eventer.Get($('#txtPoundSupplier'));
        var chkPoundSupplier = document.getElementById("chkPoundSupplier").checked;
        var Origin = Eventer.Get($('#txtOrigin'));
        var chkOrigin = document.getElementById("chkOrigin").checked;
        var arrCountFiled = [];

        var grid = $('#Grid');
        var gridOptons = grid.datagrid('options');
        var pageSize = gridOptons.pageSize;
        var rowStart = (gridOptons.pageNumber - 1) * pageSize + 1;

        if (chkLicense) arrCountFiled.push("License");
        if (chkDriver) arrCountFiled.push("Driver");
        if (chkPoundSupplier) arrCountFiled.push("PoundSupplier");
        if (chkOrigin) arrCountFiled.push("Origin");
        if (arrCountFiled.length > 0) {
            //统计查询
            Events.ShowOrhideColun("hideColumn", grid);
            for (var j = 0; j < arrCountFiled.length; j++) {
                grid.datagrid("showColumn", arrCountFiled[j]);
            }
        }
        else {
            //明细查询
            callback = callback.toString().indexOf('SetValueAndStyle') == -1 ? null : callback;
            Events.ShowOrhideColun("showColumn", grid);
        }
        Eventer.Grid(grid, Events.Service, 'AdvancedSearchReport', [startDate, endDate, rowStart, pageSize, License, Driver, PoundSupplier, Origin, arrCountFiled.join(",")], callback);

    },

    Page: function () {
        Events.GetGrid(Events.compute);
    },
    //显示/隐藏列
    ShowOrhideColun: function (cType, gridobj) {
        var arrcol = ["License", "Tree", "Driver", "Bang_Time",
        "Origin", "BackWeighTime", "PoundSupplier", "Date", "Ship", "Text", "IsAdd"];
        for (var i = 0; i < arrcol.length; i++) {
            gridobj.datagrid(cType, arrcol[i]);
        }
    },
    compute: function (grid) {//计算函数
        var rows = $('#Grid').datagrid('getRows')//获取当前的数据行
        var jweight = 0//计算净重总和
        var jvolume = 0;//计算净体积的总和
        for (var i = 0; i < rows.length; i++) {
            jweight += rows[i]['jWeight'] == "" ? 0 : rows[i]['jWeight'];
            jvolume += rows[i]['jVolume'] == "" ? 0 : rows[i]['jVolume'];
        }
        //新增一行显示统计信息
        $('#Grid').datagrid('appendRow', { Number: '<b style=color:red>合计</b>', jWeight: "<b style=color:red>" + jweight.toFixed(2) + "</b>", jVolume: "<b style=color:red>" + jvolume.toFixed(2) + "</b>" });
    }
    ,
    OnQuery: function () {
        var grid = $('#Grid');
        grid.datagrid('options').pageNumber = 1;
        grid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });

        Events.GetGrid(Events.compute);
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
                    case "Date": {
                        result = Events.ToDateTime(result);
                        break;
                    }
                    case "GsmTime": {
                        result = Events.ToTime(result);
                        break;
                    }
                    case "IsAdd": {
                        result = Events.ToIsAdd(result);
                        break;
                    }
                    case "jVolume": {
                        result = Events.ToFixed(result);
                        break;
                    }
                    case "Ship": {
                        //if (result != null) {
                        //    result = moment(result).format('YYYY/MM/DD');
                        //    var shipDateTime = new Date(Date.parse(result + " 23:59:59")).getTime();
                        //    var gsmDate = moment(row.Date).format('YYYY-MM-DD HH:mm:ss');
                        //    var gsmDateTime = new Date(Date.parse(gsmDate.replace(/-/g, "/"))).getTime();
                        //    if (shipDateTime < gsmDateTime) {
                        //        cell.Font.ColorIndex = 3; // 设置字体颜色
                        //        cell.Font.Bold = true; // 设置字体加粗
                        //    }
                        //}

                        result = Events.ToDateTime(result);
                        break;
                    }
                    case "ShipTime": {
                        //if (result != null) {
                        //    var shipDate = moment(result).format('YYYY/MM/DD');
                        //    var shipDateTime = new Date(Date.parse(shipDate + " 23:59:59")).getTime();
                        //    var gsmDate = moment(row.Date).format('YYYY-MM-DD HH:mm:ss');
                        //    var gsmDateTime = new Date(Date.parse(gsmDate.replace(/-/g, "/"))).getTime();
                        //    if (shipDateTime < gsmDateTime) {
                        //        cell.Font.ColorIndex = 3; // 设置字体颜色
                        //        cell.Font.Bold = true; // 设置字体加粗
                        //    }
                        //    result = moment(result).format('HH:mm');
                        //}

                        result = result == null ? "" : moment(result).format('HH:mm');
                        break;
                    }
                    case "Bang_Time": {
                        result = Events.ToDateTime(result);
                        break;
                    }
                    case "WeightTime": {
                        if (result != null) result = moment(result).format('HH:mm');
                        break;
                    }
                    case "Key": {
                        if (result != null) {
                            if (row.IsRebate) cell.Font.Bold = true; // 设置字体加粗
                            else result = "";
                        }
                        break;
                    }
                    case "Place": {
                        if (result != null) {
                            if (row.IsRebate) {
                                cell.Font.ColorIndex = 3; // 设置字体颜色
                                cell.Font.Bold = true; // 设置字体加粗
                            }
                            else result = "";
                        }
                        break;
                    }
                    case "Water": {
                        if (result != null) {
                            if (row.IsRebate) result = result.toFixed(2) + "%";
                            else result = "";
                        }
                        break;
                    }
                    case "RebateWater": {
                        if (result != null) {
                            if (row.IsRebate) {
                                result = result.toFixed(2) + "%";
                                cell.Font.ColorIndex = 3; // 设置字体颜色
                            }
                            else result = "";
                        }
                        break;
                    }
                    case "IsRebate": {
                        if (result != null) {
                            if (result) {
                                result = "是";
                                cell.Font.ColorIndex = 3; // 设置字体颜色
                            }
                            else result = "否";
                            cell.Font.Bold = true; // 设置字体加粗
                        }
                        break;
                    }
                    case "BackWeighTime": {
                        result = Events.ToDateTime(result);
                        break;
                    }
                    default: {
                        break;
                    }
                }

                cell.Value = (result == null ? '' : $('<div>' + result + '</div>').text());
            };

            var GetFilter = function () {
                return '日期：从 ' + Eventer.Get($('#txtStartDate')) + " 到 " + Eventer.Get($('#txtEndDate'));
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

            sheet.Cells(1, 1) = '木质原料来源地总表';
            sheet.Cells(2, 1) = GetFilter();

            var columns = grid.datagrid('getColumnFields');
            var columnCount = columns.length;

            var k = 0;
            for (var i = 0; i < columnCount; i++) {
                if (grid.datagrid('getColumnOption', columns[i]).hidden) continue;
                sheet.Cells(4, k + 1).Value = grid.datagrid('getColumnOption', columns[i]).title;
                sheet.Cells(4, k + 1).Interior.ColorIndex = 36; // 设置底色
                k++;
            }

            sheet.Range('A1:' + 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(k - 1, k) + '1').MergeCells = true;
            sheet.Range('A2:' + 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(k - 1, k) + '2').MergeCells = true;
            sheet.Cells(1, 1).HorizontalAlignment = 3;
            sheet.Cells(2, 1).HorizontalAlignment = 3;
            sheet.Rows(1).Font.Bold = true;

            k = 0;
            for (var i = 0; i < columnCount; i++) {
                if (grid.datagrid('getColumnOption', columns[i]).hidden) continue;
                var column = grid.datagrid('getColumnOption', columns[i]);
                sheet.Columns(k + 1).ColumnWidth = column.width / 8;
                sheet.Columns(k + 1).Font.Size = 9;
                sheet.Columns(k + 1).WrapText = true;
                k++;
            }
            sheet.Rows(4).Font.Bold = true;

            var rows = grid.datagrid('getRows');
            var rowCount = rows.length;
            var blfla = false;
            var chkLicense = document.getElementById("chkLicense").checked;
            var chkDriver = document.getElementById("chkDriver").checked;
            var chkPoundSupplier = document.getElementById("chkPoundSupplier").checked;
            var chkOrigin = document.getElementById("chkOrigin").checked;
            if (rowCount > 0) {
                if (chkLicense || chkDriver || chkPoundSupplier || chkOrigin) {
                    blfla = true;
                    Events.compute(grid);
                }
            }
            var currentRowIndex = 4; // 当前数据行的索引
            for (var i = 0; i < rowCount; i++) {
                currentRowIndex += 1;
                k = 0;
                for (var j = 0; j < columnCount; j++) {
                    if (grid.datagrid('getColumnOption', columns[j]).hidden) continue;
                    SetValueAndStyle(sheet.Cells(4 + i + 1, k + 1), rows[i], columns[j]);
                    k++;
                }
            }
            if (blfla) {
                SetValueAndStyle(sheet.Cells(4 + rowCount + 1, 1), rows[rowCount], columns[0]);
                SetValueAndStyle(sheet.Cells(4 + rowCount + 1, 3), rows[rowCount], columns[3]);
                SetValueAndStyle(sheet.Cells(4 + rowCount + 1, 4), rows[rowCount], columns[4]);
            }
            sheet.Range('A4:' + 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(k - 1, k) + (currentRowIndex + 1)).Borders.Weight = 2; // 设置单元格边框
        };

        Events.GetGrid(callback);
    }
};
