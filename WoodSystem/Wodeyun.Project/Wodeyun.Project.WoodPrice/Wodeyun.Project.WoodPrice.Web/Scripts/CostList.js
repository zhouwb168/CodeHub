$(function () {

    Events.ReviewPermissionsbyAccount(Eventer.RoleEnum.成本结算审核);
    Events.ReviewPermissionsbyAccount(Eventer.RoleEnum.成本结算反审);

    var grid = $('#Grid');
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    grid.datagrid("getColumnOption", "IsPrint").formatter = Events.ToPrintedText;
    grid.datagrid("getColumnOption", "OperatorDate").formatter = Events.ToDateTime;
    grid.datagrid("getColumnOption", "Bang_Time").formatter = Events.ToDateTime;
    grid.datagrid("getColumnOption", "modify").formatter = Events.ToTextFormodify;
    grid.datagrid("getColumnOption", "IsConfirmed").formatter = Events.ToPrintedText;
    grid.datagrid("getColumnOption", "VolumePrice").formatter = Events.ToFixed;
    grid.datagrid("getColumnOption", "CubePrice").formatter = Events.ToFixed;
    grid.datagrid("getColumnOption", "GPrice").formatter = Events.ToFixed;
    grid.datagrid("getColumnOption", "JVolume").formatter = Events.ToFixed;


    $('#txtStartDate').datebox('setValue', moment().add('days', -1).format('YYYY-MM-DD'));
    $('#txtEndDate').datebox('setValue', moment().format('YYYY-MM-DD'));
    //$('#txtBangStartDate').datebox('setValue', moment().add('days', -1).format('YYYY-MM-DD'));
    //$('#txtBangEndDate').datebox('setValue', moment().format('YYYY-MM-DD'));

    //绑定区域
    Eventer.ComboBox($('#AreaID'), 'GsmArea', 'GetEntities', []);

    $('#Query').bind('click', Events.OnQuery);
    $('#Print').bind('click', Events.OnPrint);
    $('#delete').bind('click', Events.OnDelete);
    $('#imgSave').bind('click', Events.OnChangeArea);
    $('#imgCancel').bind('click', Events.CloseDialog);
    $('#btnCheck').bind('click', Events.OnCostCheck);
    $('#btnBackCheck').bind('click', Events.OnCostCheck);
    $('#Excel').bind('click', Events.OnExcel);

    Events.GetGrid(null);
});

var Events = {
    Service: 'WoodPrice',

    ToPrintedText: function (value) {
        if (value == null) return '';
        var htmlValue = "";
        if (value == 1) htmlValue = "<span style=\"color:red;\">是</span>";
        else htmlValue = "否";
        return htmlValue;
    },

    ToDateTime: function (value) {
        if (value == null) return "";
        return moment(value).format('YYYY-MM-DD HH:mm');
    },

    ToTextFormodify: function (value, row, rowIndex) {
        if (row.License.toString().indexOf('<b>') != -1) return;
        return "<span style=\"color:blue; cursor:pointer\" onclick=\"Events.EditInfo('" + row.OrderNo + "','" + row.License + "','" + row.Area + "','" + Events.ToDateTime(row.Bang_Time) + "','" + row.Tree + "'," + row.IsConfirmed + ",'" + row.Supplier + "'," + row.Unique + ");\">修改</span>";
    },

    //立方金额
    ToJVolumePrice: function (value, row, rowindex) {
        var cubePrice = row.CubePrice == null ? 0 : row.CubePrice;
        var jvolume = row.JVolume == null ? 0 : row.JVolume;
        return (cubePrice * jvolume).toFixed(2);
    },

    ToFixed: function (value) {
        if (value == null) return '';
        if (value.toString().indexOf('<b>') != -1) return value;
        return value.toFixed(2);
    },

    GetGrid: function (callback) {
        var startDate = Eventer.Get($('#txtStartDate'));
        var endDate = Eventer.Get($('#txtEndDate'));
        var bangstartDate = Eventer.Get($('#txtBangStartDate'));
        var bangendDate = Eventer.Get($('#txtBangEndDate'));

        var grid = $('#Grid');
        var gridOptons = grid.datagrid('options');
        var pageSize = gridOptons.pageSize;
        var rowStart = (gridOptons.pageNumber - 1) * pageSize + 1;

        var license = Eventer.Get($('#txtLicense')).toUpperCase();
        var groupid = Eventer.Get($('#txtGroupID'));
        var printed = parseInt(Eventer.Get($('#sltPrint'), null));
        var Supplier = Eventer.Get($('#txtSupplier')).toUpperCase();

        var footer = function () {
            var items = $('#Grid').datagrid('getRows');
            var jVolume = 0.00, gweight = 0.00, weightprice = 0.00, jvolumeprice = 0.00;

            for (var i = 0; i < items.length; i++) {
                jVolume += items[i].JVolume;
                gweight += items[i].GWeight;
                weightprice += items[i].Amount;
                jvolumeprice += items[i].VolumePrice;
            }

            grid.datagrid('reloadFooter', [{
                License: '<font color="red"><b>合计</b></font>'
                , GWeight: '<font color="red"><b>' + gweight.toFixed(2) + '</b></font>'
                , JVolume: '<font color="red"><b>' + jVolume.toFixed(2) + '</b></font>'
                , Amount: '<font color="red"><b>' + weightprice.toFixed(2) + '</b></font>'
                , VolumePrice: '<font color="red"><b>' + jvolumeprice.toFixed(2) + '</b></font>'
            }]);
            if (callback != null) callback();
        };

        Eventer.Grid(grid, Events.Service, 'getCostDataList', [startDate, endDate, bangstartDate, bangendDate, rowStart, pageSize, license, printed, groupid, Supplier], footer);
    },

    Page: function () {
        Events.GetGrid(null);
    },

    OnQuery: function () {
        var grid = $('#Grid');
        grid.datagrid('clearSelections');
        grid.datagrid('options').pageNumber = 1;
        grid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });

        Events.GetGrid(null);
    },

    OnPrint: function () {
        /* 批量打印 */
        var checkeds = $('#Grid').datagrid('getSelections');
        var rowCounts = checkeds.length;
        if (rowCounts < 1) {
            $.messager.alert('提示消息', '请先选择要打印的结算单', 'warning');
            return;
        }
        var ismegre = 1;
        if (confirm("确定打印重量结算单吗？")) ismegre = 0;
        var orderno = [];
        for (var i = 0; i < rowCounts; i++) {
            if (checkeds[i].IsConfirmed == 1) continue;
            orderno.push(checkeds[i].OrderNo);
        }
        if (orderno.length == 0) {
            $.messager.alert('提示消息', '已审核的结算单不能再打印。', 'warning');
            return;
        }
        var printForm = document.createElement("form");
        document.body.appendChild(printForm);
        printForm.method = "get";
        printForm.action = "Print.html";
        printForm.target = "_blank";
        var txtBox = document.createElement("input");
        txtBox.id = "params";
        txtBox.name = "params";
        txtBox.type = "text";
        txtBox.value = orderno.join(",") + "|" + Account + "|" + ismegre;
        printForm.appendChild(txtBox);
        printForm.submit();
        document.body.removeChild(printForm);

        Events.OnQuery();
    },
    EditInfo: function (orderno, License, area, bangTime, tree, IsConfirmed, Supplier, Unique) {
        //清空选择
        $('#Grid').datagrid('clearSelections');
        if (IsConfirmed == 1) {
            $.messager.alert('提示消息', '不能修改已审核的结算单。', 'warning');
            return;
        }

        $('#AreaID').combobox("setText", area);
        Eventer.Set($("#hf_OrderNo"), orderno);
        Eventer.Set($("#hf_tree"), tree);
        Eventer.Set($("#txtLicense1"), License);
        Eventer.Set($("#txtArea"), area);
        Eventer.Set($("#txtWeightTime"), bangTime);
        Eventer.Set($("#hf_Unique"), Unique);
        Eventer.Set($("#txt_Supplier"), Supplier);

        var dlg = $('#dd');
        dlg.dialog({
            title: '&nbsp;修改区域或卸货代码',
            iconCls: 'icon-edit'
        });
        dlg.dialog('open');
    },

    CloseDialog: function () {
        $('#dd').dialog('close');
    },

    //修改区域
    OnChangeArea: function () {
        var orderno = Eventer.Get($('#hf_OrderNo'));
        var tree = Eventer.Get($('#hf_tree'));
        var oldArea = Eventer.Get($('#txtArea'));
        var newArea = $('#AreaID').combobox("getText");
        var Bang_Time = Eventer.Get($('#txtWeightTime'));
        var unique = Eventer.Get($('#hf_Unique'));
        var supplier = Eventer.Get($('#txt_Supplier'));
        //if (oldArea == newArea) {
        //    $.messager.alert('提示消息', '区域没有改变。', 'warning');
        //    return;
        //}
        if (supplier == "") {
            $.messager.alert('提示消息', '卸货代码不能为空。', 'warning');
            return;
        }
        if (confirm("确定要修改吗？") == false) return;
        var entity = {
            OrderNo: orderno,
            Area: newArea,
            Bang_Time: Bang_Time,
            Tree: tree,
            Unique: unique,
            Supplier: supplier,
            Account: Account
        };
        var data = {
            service: 'WoodPrice',
            method: 'OnChangeArea',
            args: [JSON.stringify(entity)]
        };

        var success = function (result) {
            var root = Ajaxer.GetRoot(result);
            switch (root) {
                case "SUCCESS":
                    $.messager.alert('提示消息', "修改成功。", 'info');
                    break;
                case "FAIL":
                    $.messager.alert('提示消息', "修改失败。", 'error');
                    break;
                default:
                    $.messager.alert('提示消息', "修改异常。", 'error');
                    break;
            }
            $('#Grid').datagrid('clearSelections');
            Events.Page();
            Events.CloseDialog();
        };
        Ajaxer.Ajax(Setter.Url, data, success);
    },
    OnCostCheck: function () {
        /* 批量审核结算单 */
        var checktype = this.id == "btnCheck" ? 0 : 1;
        var alterString = checktype == 0 ? "审核" : "反审";
        var checkeds = $('#Grid').datagrid('getSelections');
        var rowCounts = checkeds.length;
        if (rowCounts < 1) {
            $.messager.alert('提示消息', '请先选择要' + alterString + '的记录', 'warning');
            return;
        }
        if (confirm("确定要" + alterString + "结算单吗？") == false) return;
        var orderno = [];
        for (var i = 0; i < rowCounts; i++) {
            if (checktype == 0) {
                if (checkeds[i].IsConfirmed == 1) continue;
            } else {
                if (checkeds[i].IsConfirmed == 0) continue;
            }
            orderno.push(checkeds[i].OrderNo);
        }
        if (orderno.length == 0) {
            $.messager.alert('提示消息', '请选择' + (checktype == 0 ? "未审核" : "已审核") + '的记录', 'warning');
            return;
        }

        var data = {
            service: Events.Service,
            method: 'BatchCheckCostState',
            args: [JSON.stringify({
                OrderNo: orderno.join(","),
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
            $('#Grid').datagrid('clearSelections');
            Events.Page();
        };
        Ajaxer.Ajax(Setter.Url, data, success);
    },

    OnDelete: function () {
        /* 批量删除结算单 */
        var checkeds = $('#Grid').datagrid('getSelections');
        var rowCounts = checkeds.length;
        if (rowCounts < 1) {
            $.messager.alert('提示消息', '请先选择要删除的记录', 'warning');
            return;
        }
        if (confirm("确定删除结算单吗？") == false) return;
        var orderno = [];
        for (var i = 0; i < rowCounts; i++) {
            if (checkeds[i].IsConfirmed == 1) continue;
            orderno.push(checkeds[i].OrderNo);
        }
        if (orderno.length == 0) {
            $.messager.alert('提示消息', '已审核的记录无法删除。', 'warning');
            return;
        }
        var data = {
            service: Events.Service,
            method: 'BatchDeleteCostState',
            args: [JSON.stringify({
                OrderNo: orderno.join(","),
                Account: Account
            })]
        };

        var success = function (result) {
            var root = Ajaxer.GetRoot(result);
            switch (root) {
                case "SUCCESS":
                    $.messager.alert('提示消息', "删除成功。", 'info');
                    break;
                case "FAIL":
                    $.messager.alert('提示消息', "删除失败。", 'error');
                    break;
                default:
                    $.messager.alert('提示消息', "删除异常。", 'error');
                    break;
            }
            $('#Grid').datagrid('clearSelections');
            Events.Page();
        };
        Ajaxer.Ajax(Setter.Url, data, success);
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
                        result = Events.ToDateTime(result);
                        break;
                    case "IsPrint":
                        result = Events.ToPrintedText(result);
                        break;
                    case "OperatorDate":
                        result = Events.ToDateTime(result);
                        break;
                    case "IsConfirmed":
                        result = Events.ToPrintedText(result);
                        break;
                    case "CubePrice":
                    case "VolumePrice":
                    case "JVolume":
                        result = Events.ToFixed(result);
                        break;
                }
                cell.Value = (result == null ? '' : $('<div>' + result + '</div>').text());
            };

            var GetFilter = function () {
                return '结算日期：从 ' + Eventer.Get($('#txtStartDate')) + " 到 " + Eventer.Get($('#txtEndDate'));
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

            sheet.Cells(1, 1) = '木材收购结算单';
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

            sheet.Range('A4:' + 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(columnCount - 1, columnCount) + currentRowIndex).Borders.Weight = 2; // 设置单元格边框
        };

        Events.GetGrid(callback);
    }
};
