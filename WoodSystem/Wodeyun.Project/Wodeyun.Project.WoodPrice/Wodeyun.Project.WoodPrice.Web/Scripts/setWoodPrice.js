$(function () {

    Events.ReviewPermissionsbyAccount(Eventer.RoleEnum.价格体系审核);
    Events.ReviewPermissionsbyAccount(Eventer.RoleEnum.价格体系反审);

    var grid = $('#Grid');
    grid.datagrid({ onClickRow: Events.OnClick });
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    grid.datagrid("getColumnOption", "ExeDate").formatter = Events.ToDateTime;
    grid.datagrid("getColumnOption", "Price").formatter = Events.ToPrice;
    grid.datagrid("getColumnOption", "CubePrice").formatter = Events.ToPrice;
    grid.datagrid("getColumnOption", "WetPrice").formatter = Events.ToPrice;
    grid.datagrid("getColumnOption", "IsConfirmed").formatter = Events.ToIsConfirmed;

    $('#Buttons').find('[unique="Create"]').bind('click', Events.OnCreate);
    $('#Buttons').find('[unique="Delete"]').bind('click', Events.OnDelete);
    $('#Buttons').find('[unique="Save"]').bind('click', Events.OnSave);
    $('#Buttons').find('[unique="Cancel"]').bind('click', Events.OnCancel);

    $('#ExeDate').datebox('setValue', moment().format('YYYY-MM-DD'));
    $('#txtStartDate').datebox('setValue', moment().add('days', -30).format('YYYY-MM-DD'));
    $('#txtEndDate').datebox('setValue', moment().format('YYYY-MM-DD'));
    //绑定区域
    Eventer.ComboBox($('#AreaID'), 'GsmArea', 'GetEntities', []);
    //绑定树种
    Eventer.ComboBox($('#TreeID'), 'GsmTree', 'GetEntities', []);

    //绑定区域
    Eventer.ComboBox($('#Area'), 'GsmArea', 'GetEntities', [], function () {
        $('#Area').combobox('setValue', "");
    });

    //绑定树种
    Eventer.ComboBox($('#Tree'), 'GsmTree', 'GetEntities', [], function () {
        $('#Tree').combobox('setValue', "");
    });

    Events.Page();

    $('#Query').bind('click', Events.OnQuery);
    $('#Excel').bind('click', Events.OnExcel);
    $('#btnCheck').bind('click', Events.OnPriceCheck);
    $('#btnBackCheck').bind('click', Events.OnPriceCheck);
});

var Events = {
    Service: 'WoodPrice',

    ToPrice: function (value) {
        if (value == null) return '';
        return value.toFixed(2);
    },

    ToName: function (value, row, rowIndex) {
        if (row.Description == null) return '';

        return $.parseJSON(row.Description).Name;
    },

    ToDateTime: function (value, row, rowIndex) {
        if (value == null) return '';

        return moment(value).format('YYYY/MM/DD');
    },

    ToIsConfirmed: function (value) {
        if (value == null) return '';
        var htmlValue = "";
        if (value == 1) htmlValue = "<span style=\"color:red;\">是</span>";
        else htmlValue = "否";
        return htmlValue;
    },

    Page: function () {
        Events.GetGrid(null);
    },
    GetGrid: function (callback) {
        var startDate = Eventer.Get($('#txtStartDate'));
        var endDate = Eventer.Get($('#txtEndDate'));
        var area = $('#Area').combobox("getText");
        var tree = $('#Tree').combobox("getText");
        var grid = $('#Grid');
        grid.datagrid('clearSelections');
        var gridOptons = grid.datagrid('options');
        var pageSize = gridOptons.pageSize;
        var rowStart = (gridOptons.pageNumber - 1) * pageSize + 1;
        Eventer.Grid($('#Grid'), Events.Service, 'GetWoodPriceData', [startDate, endDate, rowStart, pageSize, area, tree], callback);
    },
    OnQuery: function () {
        var grid = $('#Grid');
        grid.datagrid('options').pageNumber = 1;
        grid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });

        Events.GetGrid(null);
    },
    OnClick: function () {
        Eventer.Click($('#Grid'), Buttons, Controls);
    },

    OnCreate: function () {
        Eventer.Create(Buttons, Controls);
    },

    OnDelete: function () {
        Eventer.Delete($('#Unique'), Events.Service, Events.Page, Buttons, Controls);
    },

    OnSave: function () {
        if (Checker.Valid($('#Price'), '请输入干吨价格，没有就输“0”！') == false) return;
        if (Checker.Valid($('#WetPrice'), '请输湿吨入价格，没有就输“0”！') == false) return;
        if (Checker.Valid($('#CubePrice'), '请输入立方价格，没有就输“0”！') == false) return;
        Eventer.Save($('#Unique'), Events.Service, Events.Page, Buttons, Controls);
    },

    OnCancel: function () {
        Eventer.Cancel(Buttons, Controls);
    },

    OnPriceCheck: function () {
        /* 批量审核价格体系 */
        var checktype = this.id == "btnCheck" ? 0 : 1;
        var alterString = checktype == 0 ? "审核" : "反审";
        var checkeds = $('#Grid').datagrid('getChecked');
        var rowCounts = checkeds.length;
        if (rowCounts < 1) {
            $.messager.alert('提示消息', '请先选择要' + alterString + '的记录', 'warning');
            return;
        }
        if (confirm("确定要" + alterString + "价格体系单吗？") == false) return;
        var uniques = [], versions = [];
        for (var i = 0; i < rowCounts; i++) {
            if (checktype == 0) {
                if (checkeds[i].IsConfirmed == 1) continue;
            } else {
                if (checkeds[i].IsConfirmed == 0) continue;
            }
            uniques.push(checkeds[i].Unique);
            versions.push(checkeds[i].Version);
        }
        if (uniques.length == 0) {
            $.messager.alert('提示消息', '请选择' + (checktype == 0 ? "未审核" : "已审核") + '的记录', 'warning');
            return;
        }

        var data = {
            service: Events.Service,
            method: 'BatchCheckWoodPrice',
            args: [JSON.stringify({
                Unique: uniques.join(","),
                Version: versions.join(","),
                CheckType: checktype,//审核
                Account: Account
            })]
        };

        var success = function (result) {
            var root = Ajaxer.GetRoot(result);
            switch (root) {
                case "SUCCESS":
                    $.messager.alert('提示消息', alterString + "成功。", 'info');
                    break;
                case "FAIL":
                    $.messager.alert('提示消息', alterString + "失败。", 'error');
                    break;
                default:
                    $.messager.alert('提示消息', alterString + "异常。", 'error');
                    break;
            }
            Buttons.Delete();
            Controls.Disabled();
            Controls.Clear();
            $('#Grid').datagrid('clearChecked');
            $('#Grid').datagrid('clearSelections');
            Events.Page();
        };
        Ajaxer.Ajax(Setter.Url, data, success);
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
                    case "ExeDate":
                        result = Events.ToDateTime(result);
                        break;
                    case "Price":
                    case "WetPrice":
                    case "CubePrice":
                        result = Events.ToPrice(result);
                        break;
                    case "IsConfirmed":
                        result = Events.ToIsConfirmed(result);
                        break;
                    default:
                        break;
                }
                cell.Value = (result == null ? '' : $('<div>' + result + '</div>').text());
            };

            var GetFilter = function () {
                return '执行日期：从 ' + Eventer.Get($('#txtStartDate')) + " 到 " + Eventer.Get($('#txtEndDate'));
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

            sheet.Cells(1, 1) = '百色木材价格体系';
            sheet.Cells(2, 1) = GetFilter();

            var grid = $('#Grid');
            var columns = grid.datagrid('getColumnFields');
            var columnCount = columns.length;
            for (var i = 0; i < columnCount; i++) {
                sheet.Cells(4, i + 1).Value = grid.datagrid('getColumnOption', columns[i]).title;
                sheet.Cells(4, i + 1).Interior.ColorIndex = 36; // 设置底色
            }

            sheet.Range('A1:' + 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(columnCount - 1, columnCount) + '1').MergeCells = true;
            sheet.Range('A2:' + 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(columnCount - 1, columnCount) + '2').MergeCells = true;
            sheet.Cells(1, 1).HorizontalAlignment = 3;
            sheet.Cells(2, 1).HorizontalAlignment = 3;
            sheet.Rows(1).Font.Bold = true;
            for (var i = 0; i < columnCount; i++) {
                var column = grid.datagrid('getColumnOption', columns[i]);

                sheet.Columns(i + 1).ColumnWidth = column.width / 8;
                sheet.Columns(i + 1).Font.Size = 9;
                sheet.Columns(i + 1).WrapText = true;
            }
            sheet.Rows(4).Font.Bold = true;

            var rows = grid.datagrid('getRows');
            var rowCount = rows.length;
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
    },
    ReviewPermissionsbyAccount: function (roleid) {
        var data = {
            service: Events.Service,
            method: 'ReviewPermissionsbyAccount',
            args: [JSON.stringify({
                Account: Account,
                Role: roleid
            })]
        };

        var success = function (result) {
            var root = Ajaxer.GetRoot(result);
            if (root > 0) {
                $("#checkTool_" + roleid).show();
            }
            else {
                $("#checkTool_" + roleid).remove();
            }
        };
        Ajaxer.Ajax(Setter.Url, data, success);
    }
};

var Buttons = {
    State: 'None',

    Update: function () {
        Eventer.Hide($('#Buttons'), 'Create');
        Eventer.Show($('#Buttons'), 'Delete');
        Eventer.Show($('#Buttons'), 'Save');
        Eventer.Show($('#Buttons'), 'Cancel');

        this.State = 'Update';
    },

    Create: function () {
        Eventer.Hide($('#Buttons'), 'Create');
        Eventer.Hide($('#Buttons'), 'Delete');
        Eventer.Show($('#Buttons'), 'Save');
        Eventer.Show($('#Buttons'), 'Cancel');

        this.State = 'Create';
    },

    Delete: function () {
        Eventer.Show($('#Buttons'), 'Create');
        Eventer.Hide($('#Buttons'), 'Delete');
        Eventer.Hide($('#Buttons'), 'Save');
        Eventer.Hide($('#Buttons'), 'Cancel');

        this.State = 'None';
    },

    Save: function () {
        if (this.State == 'Update' || this.State == 'Create') {
            Eventer.Show($('#Buttons'), 'Create');
            Eventer.Show($('#Buttons'), 'Delete');
            Eventer.Hide($('#Buttons'), 'Save');
            Eventer.Hide($('#Buttons'), 'Cancel');
        }

        this.State = 'None';
    },

    Cancel: function () {
        if (this.State == 'Update') {
            Eventer.Show($('#Buttons'), 'Create');
            Eventer.Show($('#Buttons'), 'Delete');
            Eventer.Hide($('#Buttons'), 'Save');
            Eventer.Hide($('#Buttons'), 'Cancel');
        }

        if (this.State == 'Create') {
            Eventer.Show($('#Buttons'), 'Create');
            Eventer.Hide($('#Buttons'), 'Delete');
            Eventer.Hide($('#Buttons'), 'Save');
            Eventer.Hide($('#Buttons'), 'Cancel');
        }

        this.State = 'None';
    },

    Clear: function () {
        Eventer.Show($('#Buttons'), 'Create');
        Eventer.Hide($('#Buttons'), 'Delete');
        Eventer.Hide($('#Buttons'), 'Save');
        Eventer.Hide($('#Buttons'), 'Cancel');
    }
};

var Controls = {
    Clear: function () {
        Eventer.Set($('#Unique'));
        Eventer.Set($('#AreaID'));
        Eventer.Set($('#TreeID'));
        Eventer.Set($('#Price'));
        Eventer.Set($('#WetPrice'));
        Eventer.Set($('#CubePrice'));
        Eventer.Set($('#Unit'));
        Eventer.Set($('#ExeDate'));
        Eventer.Set($('#Remark'));
        Eventer.Set($('#hf_Version'));
    },

    Get: function () {
        return {
            Unique: Eventer.Get($('#Unique')),
            AreaID: Eventer.Get($('#AreaID')),
            TreeID: Eventer.Get($('#TreeID')),
            Price: Eventer.Get($('#Price')) == "" ? 0 : Eventer.Get($('#Price')),
            WetPrice: Eventer.Get($('#WetPrice')) == "" ? 0 : Eventer.Get($('#WetPrice')),
            CubePrice: Eventer.Get($('#CubePrice')) == "" ? 0 : Eventer.Get($('#CubePrice')),
            Unit: Eventer.Get($('#Unit')),
            ExeDate: Eventer.Get($('#ExeDate')),
            Remark: Eventer.Get($('#Remark')),
            IsConfirmed: 0,
            Version1: Eventer.Get($('#hf_Version')),
            Operator: Account
        };
    },

    Set: function () {
        var row = $('#Grid').datagrid('getSelected');
        if (row.IsConfirmed == 1) {
            Buttons.Clear();
            Controls.Clear();
            Controls.Disabled();
            return;
        }
        Eventer.Set($('#Unique'), row.Unique);
        Eventer.Set($('#AreaID'), row.AreaID);
        Eventer.Set($('#TreeID'), row.TreeID);
        Eventer.Set($('#Price'), Events.ToPrice(row.Price));
        Eventer.Set($('#WetPrice'), Events.ToPrice(row.WetPrice));
        Eventer.Set($('#CubePrice'), Events.ToPrice(row.CubePrice));
        Eventer.Set($('#Unit'), row.Unit);
        $('#ExeDate').datebox('setValue', moment(row.ExeDate).format('YYYY-MM-DD'));
        Eventer.Set($('#Remark'), row.Remark);
        Eventer.Set($('#hf_Version'), row.Version);
    },

    Enabled: function () {
        Eventer.Enable($('#AreaID'));
        Eventer.Enable($('#TreeID'));
        Eventer.Enable($('#Price'));
        Eventer.Enable($('#WetPrice'));
        Eventer.Enable($('#CubePrice'));
        Eventer.Enable($('#Unit'));
        Eventer.Enable($('#ExeDate'));
        Eventer.Enable($('#Remark'));
    },

    Disabled: function () {
        Eventer.Disable($('#AreaID'));
        Eventer.Disable($('#TreeID'));
        Eventer.Disable($('#Price'));
        Eventer.Disable($('#WetPrice'));
        Eventer.Disable($('#CubePrice'));
        Eventer.Disable($('#Unit'));
        Eventer.Disable($('#ExeDate'));
        Eventer.Disable($('#Remark'));
    }
};
