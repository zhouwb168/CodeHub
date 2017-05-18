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
    grid.datagrid("getColumnOption", "Bang_Time").formatter = Events.ToDateTime;
    grid.datagrid("getColumnOption", "GweightPrice").formatter = Events.ToFixed;
    grid.datagrid("getColumnOption", "VolumePrice").formatter = Events.ToFixed;
    grid.datagrid("getColumnOption", "Price").formatter = Events.ToFixed;
    grid.datagrid("getColumnOption", "CubePrice").formatter = Events.ToFixed;
    grid.datagrid("getColumnOption", "IsCreate").formatter = Events.ToCreatedText;
    grid.datagrid("getColumnOption", "JVolume").formatter = Events.ToFixed;
    grid.datagrid("getColumnOption", "FullVolume").formatter = Events.ToFixed;

    var gridCost = $('#gridCost');
    //gridCost.datagrid("getColumnOption", "IsPrint").formatter = Events.ToPrintedText;
    gridCost.datagrid("getColumnOption", "OperatorDate").formatter = Events.ToDateTime;
    gridCost.datagrid("getColumnOption", "Bang_Time").formatter = Events.ToDateTime;
    //gridCost.datagrid("getColumnOption", "modify").formatter = Events.ToTextFormodify;
    //gridCost.datagrid("getColumnOption", "IsConfirmed").formatter = Events.ToPrintedText;
    gridCost.datagrid("getColumnOption", "VolumePrice").formatter = Events.ToFixed;
    gridCost.datagrid("getColumnOption", "CubePrice").formatter = Events.ToFixed;
    gridCost.datagrid("getColumnOption", "GPrice").formatter = Events.ToFixed;
    gridCost.datagrid("getColumnOption", "JVolume").formatter = Events.ToFixed;
    gridCost.datagrid("getColumnOption", "FullVolume").formatter = Events.ToFixed;

    //$('#Year').numberspinner('setValue', moment().add('years', 0).format('YYYY'));
    //$('#Month').combobox('setValue', moment().format('M'));
    $('#txtStartDate').datebox('setValue', moment().add('days', -1).format('YYYY-MM-DD'));
    $('#txtEndDate').datebox('setValue', moment().format('YYYY-MM-DD'));

    $('#Query').bind('click', Events.OnQuery);
    $('#Excel').bind('click', Events.OnExcel);
    $('#btnCreate').bind('click', Events.OnCostStatement);
    $('#Print').bind('click', Events.OnPrint);
    $('#imgSave').bind('click', Events.OnChangeArea);
    $('#imgCancel').bind('click', Events.CloseDialog);

    //绑定区域
    Eventer.ComboBox($('#Area'), 'GsmArea', 'GetEntities', [], function () {
        $('#Area').combobox('setValue', "");
    });

    //绑定树种
    Eventer.ComboBox($('#Tree'), 'GsmTree', 'GetEntities', [], function () {
        $('#Tree').combobox('setValue', "");
    });
    Events.GetGrid(null);
});

var Events = {
    Service: 'WoodPrice',

    ResetPager: function () {
        var grid = $('#Grid');
        grid.datagrid('options').pageNumber = 1;
        grid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });
    },
    ToCreatedText: function (value) {
        if (value == null) return '';
        var htmlValue = "";
        if (value == "已生成") htmlValue = "<span style=\"color:red;\">已生成</span>";
        else htmlValue = "未生成";

        return htmlValue;
    },
    //是否补报
    ToIsAdd: function (value) {
        if (value == null) return '';
        var htmlValue = value == 1 ? "<span style=\"color:red;\">是</span>" : "否";
        return htmlValue;
    },
    ToFixed: function (value) {
        if (value == null) return '';
        if (value.toString().indexOf('<b>') != -1) return value;
        return value.toFixed(2);
    },
    //干吨金额
    ToGWeightPrice: function (value, row, rowindex) {
        var price = row.Price == null ? 0 : row.Price;
        var gweight = row.GWeight == null ? 0 : row.GWeight;
        return (price * gweight).toFixed(2);
    },
    //立方金额
    ToJVolumePrice: function (value, row, rowindex) {
        var cubePrice = row.CubePrice == null ? 0 : row.CubePrice;
        var jvolume = row.JVolume == null ? 0 : row.JVolume;
        return (cubePrice * jvolume).toFixed(2);
    },
    ToDateTime: function (value) {
        if (value == null) return '';
        return moment(value).format('YYYY/MM/DD HH:mm');
    },
    ToAmount: function (gweight, price) {
        price = price == null ? 0 : price;
        gweight = gweight == null ? 0 : gweight;
        return (price * gweight).toFixed(2);
    },
    GetMonth: function () {
        return Eventer.Get($('#Year')) + '-' + Eventer.Get($('#Month'));
    },

    ToTextFormodify: function (value, row, rowIndex) {
        return "<span style=\"color:blue; cursor:pointer\" onclick=\"Events.EditInfo('" + row.OrderNo + "','" + row.License + "','" + row.Area + "','" + Events.ToDateTime(row.Bang_Time) + "','" + row.Tree + "'," + row.IsConfirmed + ");\">修改</span>";
    },

    GetGrid: function (callback) {
        var grid = $('#Grid');
        var gridOptons = grid.datagrid('options');
        var pageSize = gridOptons.pageSize;
        var rowStart = (gridOptons.pageNumber - 1) * pageSize + 1;
        var area = $('#Area').combobox("getText");
        var tree = $('#Tree').combobox("getText");
        var IsCreate = $('#sltCreate').combobox("getValue");
        var startDate = Eventer.Get($('#txtStartDate'));
        var endDate = Eventer.Get($('#txtEndDate'));
        var Supplier = Eventer.Get($('#txtSupplier')).toUpperCase();

        var footer = function () {
            var items = $('#Grid').datagrid('getRows');
            var jWeight = 0.00
                , jVolume = 0.00
                , fullvolume = 0.00
                , gweight = 0.00, weightprice = 0.00, jvolumeprice = 0.00;

            for (var i = 0; i < items.length; i++) {
                jWeight += items[i].jWeight;
                jVolume += items[i].JVolume;
                fullvolume += items[i].FullVolume;
                gweight += items[i].GWeight;
                weightprice += items[i].GweightPrice;
                jvolumeprice += items[i].VolumePrice;
            }

            $('#Grid').datagrid('reloadFooter', [{
                License: '<font color="red"><b>合计</b></font>'
                , jWeight: '<font color="red"><b>' + jWeight.toFixed(2) + '</b></font>'
                , GWeight: '<font color="red"><b>' + gweight.toFixed(2) + '</b></font>'
                , JVolume: '<font color="red"><b>' + jVolume.toFixed(2) + '</b></font>'
                , FullVolume: '<font color="red"><b>' + fullvolume.toFixed(2) + '</b></font>'
                , GweightPrice: '<font color="red"><b>' + weightprice.toFixed(2) + '</b></font>'
                , VolumePrice: '<font color="red"><b>' + jvolumeprice.toFixed(2) + '</b></font>'
            }]);
            if (callback != null) callback();
        };

        Eventer.Grid(grid, Events.Service, 'getPriceDataList', [startDate, endDate, rowStart, pageSize, area, tree, IsCreate, Supplier], footer);
    },

    Page: function () {
        Events.GetGrid(null);
    },

    OnClick: function (event) {
        var month = moment().add('months', -event.data.index).format('YYYY-MM');
        var itemsOfAlink = $('#Buttons').find('a');
        var itemCount = itemsOfAlink.length;
        for (var i = 0; i < itemCount; i++) {
            itemsOfAlink.eq(i).find('font').attr('color', (month == moment().add('months', -i).format('YYYY-MM') ? 'red' : 'normal'));
        }

        Events.ResetPager();
        $('#Grid').datagrid('clearSelections');
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
    OnCostStatement: function () {
        /* 批量生成结算单 */
        var checkeds = $('#Grid').datagrid('getSelections');
        var rowCounts = checkeds.length;
        if (rowCounts < 1) {
            $.messager.alert('提示消息', '请先选择要生成的记录', 'warning');
            return;
        }
        if (confirm("确定要生成结算单吗？") == false) return;
        var intRes = 0, intErr = 0;
        var collection = [];
        for (var i = 0; i < rowCounts; i++) {
            if (checkeds[i].IsCreate == "已生成") continue;
            var entity = {
                Unique: checkeds[i].Unique,
                License: checkeds[i].License,
                LinkMan: checkeds[i].LinkMan,
                Bang_Time: Events.ToDateTime(checkeds[i].Bang_Time),
                JWeight: checkeds[i].jWeight,
                Tree: checkeds[i].Tree,
                Area: checkeds[i].Area,
                GWeight: checkeds[i].GWeight,
                GPrice: (checkeds[i].Price == null ? 0 : checkeds[i].Price),
                Amount: Events.ToAmount(checkeds[i].GWeight, checkeds[i].Price),
                LinkMan: checkeds[i].LinkMan,
                Account: Account,
                JVolume: checkeds[i].JVolume,
                FullVolume: checkeds[i].FullVolume,
                CubePrice: (checkeds[i].CubePrice == null ? 0 : checkeds[i].CubePrice)
            };
            collection.push(JSON.stringify(entity));
        }
        var data = {
            service: 'WoodPrice',
            method: 'InsertCostStatement',
            args: [collection]
        };

        var success = function (result) {
            var root = Ajaxer.GetRoot(result);
            if (root.Success == false) {
                alert(root.Message);
                return;
            }

            var items = Ajaxer.GetGrid(root);
            var gridCost = $("#gridCost");
            gridCost.datagrid('clearSelections');
            if (gridCost.attr('class').indexOf('easyui-datagrid') != -1) gridCost.datagrid('loadData', items);
            if (gridCost.attr('class').indexOf('easyui-treegrid') != -1) gridCost.treegrid('loadData', items);

            var dlg = $('#dd');
            dlg.dialog({
                title: '&nbsp;结算单',
                iconCls: 'icon-edit'
            });
            dlg.dialog('open');

            var len = items.rows.length;
            var wetWeight = 0, GWeight = 0, WeightPrice = 0, jVolume = 0, VolumePrice = 0, fullvolume = 0;
            if (len > 0) {
                Eventer.Set($("#txtGroupID"), items.rows[0].GroupID);
                for (var i = 0; i < len; i++) {
                    wetWeight += items.rows[i].JWeight;
                    GWeight += items.rows[i].GWeight;
                    WeightPrice += items.rows[i].Amount;
                    jVolume += items.rows[i].JVolume;
                    fullvolume += items.rows[i].FullVolume;
                    VolumePrice += items.rows[i].VolumePrice;
                }
            }
            gridCost.datagrid('reloadFooter', [{
                License: '<font color="red"><b>合计</b></font>'
                , JWeight: '<font color="red"><b>' + wetWeight.toFixed(2) + '</b></font>'
                , GWeight: '<font color="red"><b>' + GWeight.toFixed(2) + '</b></font>'
                , Amount: '<font color="red"><b>' + WeightPrice.toFixed(2) + '</b></font>'
                , JVolume: '<font color="red"><b>' + jVolume.toFixed(2) + '</b></font>'
                , FullVolume: '<font color="red"><b>' + fullvolume.toFixed(2) + '</b></font>'
                , VolumePrice: '<font color="red"><b>' + VolumePrice.toFixed(2) + '</b></font>'
            }]);
            Eventer.Set($("#txtwetWeight"), wetWeight.toFixed(2));
            Eventer.Set($("#txtGWeight"), GWeight.toFixed(2));
            Eventer.Set($("#txtWeightPrice"), WeightPrice.toFixed(2));
            Eventer.Set($("#txtjVolume"), jVolume.toFixed(2));
            Eventer.Set($("#txtVolumePrice"), VolumePrice.toFixed(2));
            Eventer.Set($("#txtCarCount"), len);

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
                    case "Bang_Time":
                        result = Events.ToDateTime(result);
                        break;
                    case "GweightPrice":
                    case "VolumePrice":
                    case "Price":
                    case "CubePrice":
                    case "FullVolume":
                    case "JVolume":
                        result = Events.ToFixed(result);
                        break;
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

            sheet.Cells(1, 1) = '木材收购统计明细表';
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
    },

    CloseDialog: function () {
        $('#dd').dialog('close');
    },

    CloseDialog1: function () {
        $('#Div1').dialog('close');
    },

    OnPrint: function () {

        /* 批量打印 */
        var checkeds = $('#gridCost').datagrid('getSelections');
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

        //Events.OnQuery();
    },

    EditInfo: function (orderno, License, area, bangTime, tree, IsConfirmed) {
        $('#gridCost').datagrid('clearSelections');
        if (IsConfirmed == 1) {
            $.messager.alert('提示消息', '不能修改已审核的结算单。', 'warning');
            return;
        }
        //清空选择
        Eventer.Set($("#hf_OrderNo"), orderno);
        Eventer.Set($("#hf_tree"), tree);
        Eventer.Set($("#txtLicense1"), License);
        Eventer.Set($("#txtArea"), area);
        Eventer.Set($("#txtWeightTime"), bangTime);
        //绑定区域
        Eventer.ComboBox($('#AreaID'), 'GsmArea', 'GetEntities', []);
        var dlg = $('#Div1');
        dlg.dialog({
            title: '&nbsp;修改区域',
            iconCls: 'icon-edit'
        });
        dlg.dialog('open');
    },

    //修改区域
    OnChangeArea: function () {
        var orderno = Eventer.Get($('#hf_OrderNo'));
        var tree = Eventer.Get($('#hf_tree'));
        var oldArea = Eventer.Get($('#txtArea'));
        var newArea = $('#AreaID').combobox("getText");
        var Bang_Time = Eventer.Get($('#txtWeightTime'));
        if (oldArea == newArea) {
            $.messager.alert('提示消息', '区域没有改变。', 'warning');
            return;
        }
        if (confirm("确定要修改区域吗？") == false) return;
        var entity = {
            OrderNo: orderno,
            Area: newArea,
            Bang_Time: Bang_Time,
            Tree: tree,
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
            $('#gridCost').datagrid('clearSelections');
            Events.CloseDialog1();
        };
        Ajaxer.Ajax(Setter.Url, data, success);
    }
};
