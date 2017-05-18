$(function () {
    $('#Grid').treegrid({ loadFilter: Events.OnGrid });
    $('#Grid').treegrid('getColumnOption', 'No').formatter = Events.ToNo;
    $('#Grid').treegrid('getColumnOption', 'Count').formatter = Events.ToCount;
    $('#Grid').treegrid('getColumnOption', 'Origin').formatter = Events.ToOrigin;

    Events.GetSuppliers();

    for (var i = 0; i < $('#Buttons').find('a').length; i++) {
        $('#Buttons').find('a').eq(i).bind('click', { index: i }, Events.OnClick);
    }
    $('#Query').bind('click', Events.OnQuery);
    $('#Excel').bind('click', Events.OnExcel);

    for (var i = 2; i < $('#Buttons').find('a').length; i++) {
        $('#Buttons').find('a').eq(i).find('font').html(moment().add('months', -i).format('MM') + '月');
    }
    $('#Year').numberspinner('setValue', moment().add('years', -1).format('YYYY'));
    $('#Month').combobox('setValue', moment().format('M'));
});

var Events = {
    Service: 'GsmReport',

    ToNo: function (value, row) {
        if (value == null) return '<font color="red">小计</font>';

        return value;
    },

    ToCount: function (value, row) {
        if (row.No == null) {
            var items = $('#Grid').treegrid('getChildren', row.Number);
            var count = 0;
            
            for (var i = 0; i < items.length; i++) {
                count = count + items[i].Count;
            }

            return '<font color="red"><b>' + count + '</b></font>';
        }

        return value;
    },

    ToOrigin: function (value, row) {
        if (row.No == null) return '';

        return value;
    },

    GetSuppliers: function () {
        var data = {
            service: 'GsmSupplier',
            method: 'GetEntities',
            args: []
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }

            var items = Ajaxer.GetItems(root);

            var text = '';
            for (var i = 0; i < items.length; i++) {
                text = text + '<a href="#" unique="' + items[i].Name + '" class="easyui-linkbutton" plain="true"><font color="' + (i == 0 ? 'red' : 'normal') + '">' + items[i].Name + '</font></a>';
            }

            $('#Suppliers').html(text)
            $.parser.parse('#Suppliers');

            for (var i = 0; i < $("#Suppliers").find('a').length; i++) {
                $("#Suppliers").find('a').eq(i).bind('click', { index: i }, Events.OnSupplier);
            }

            Events.GetGrid(moment().format('YYYY-MM'), Events.GetSupplier(), 1, $('#Grid').treegrid('options').pageSize);
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    GetSupplier: function () {
        for (var i = 0; i < $("#Suppliers").find('a').length; i++) {
            if ($("#Suppliers").find('a').eq(i).find('font').attr('color') == '#ff0000') return $("#Suppliers").find('a').eq(i).find('font').html();
        }
    },

    GetMonth: function () {
        for (var i = 0; i < $('#Buttons').find('a').length; i++) {
            if ($('#Buttons').find('a').eq(i).find('font').attr('color') == '#ff0000') return moment().add('months', -i).format('YYYY-MM');
        }

        return Eventer.Get($('#Year')) + '-' + Eventer.Get($('#Month'));
    },

    GetGrid: function (month, supplier, start, length, callback) {
        var footer = function () {
            var items = $('#Grid').treegrid('getChildren');
            var count = 0;

            for (var i = 0; i < items.length; i++) {
                count = count + items[i].Count;
            }

            $('#Grid').treegrid('reloadFooter', [{ No: '<font color="red">合计</font>', Count: '<font color="red"><b>' + count + '</b></font>' }]);

            if (callback != null) callback();
        };
        
        Eventer.Grid($('#Grid'), Events.Service, 'GetGridByMethodAndFieldsAndUnique', ['GetReport03ByMonthAndSupplierAndStartAndLength', [month, supplier, start, length], ['Origin'], 'Number'], footer);

        for (var i = 0; i < $('#Buttons').find('a').length; i++) {
            $('#Buttons').find('a').eq(i).find('font').attr('color', (month == moment().add('months', -i).format('YYYY-MM') ? 'red' : 'normal'));
        }

        for (var i = 0; i < $("#Suppliers").find('a').length; i++) {
            $("#Suppliers").find('a').eq(i).find('font').attr('color', (supplier == $("#Suppliers").find('a').eq(i).find('font').html() ? 'red' : 'normal'));
        }
    },

    Page: function () {
        var pageNumber = $('#Grid').treegrid('options').pageNumber;
        var pageSize = $('#Grid').treegrid('options').pageSize;

        var month = Eventer.Get($('#Year')) + '-' + Eventer.Get($('#Month'));
        for (var i = 0; i < $('#Buttons').find('a').length; i++) {
            if ($('#Buttons').find('a').eq(i).find('font').attr('color') == '#ff0000') month = moment().add('months', -i).format('YYYY-MM');
        }

        Events.GetGrid(month, Events.GetSupplier(), (pageNumber - 1) * pageSize + 1, pageSize);
    },

    OnClick: function (event) {
        Events.GetGrid(moment().add('months', -event.data.index).format('YYYY-MM'), Events.GetSupplier(), 1, $('#Grid').treegrid('options').pageSize);
    },

    OnSupplier: function (event) {
        Events.GetGrid(Events.GetMonth(), $("#Suppliers").find('a').eq(event.data.index).find('font').html(), 1, $('#Grid').treegrid('options').pageSize);
    },

    OnQuery: function () {
        Events.GetGrid(Eventer.Get($('#Year')) + '-' + Eventer.Get($('#Month')), Events.GetSupplier(), 1, $('#Grid').treegrid('options').pageSize);
    },

    OnExcel: function () {
        $('#Grid').treegrid('options').pageSize = Setter.Max;
        $('#Grid').treegrid('getPager').pagination('refresh', { pageSize: Setter.Max });

        var callback = function () {
            var SetValueAndStyle = function (cell, row, column) {
                var result = row[column];

                if (column == 'No') result = Events.ToNo(row[column], row);
                if (column == 'Count') result = Events.ToCount(row[column], row);
                if (column == 'Origin') result = Events.ToOrigin(row[column], row);
                if (column == 'Drivers') {
                    if (result != null && result.length > 11) result = '\'' + result;
                }

                cell.Value = (result == null ? '' : $('<div>' + result + '</div>').text());

                if (column == 'No' || column == 'Count') {
                    if ((result + '').indexOf('<font color="red">') != -1) cell.Font.ColorIndex = 3;
                    if ((result + '').indexOf('<b>') != -1) cell.Font.Bold = true;
                }
            };

            var GetFilter = function () {
                return '日期：' + Events.GetMonth() + '    ' + '供应商代码：' + Events.GetSupplier();
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

            sheet.Cells(1, 1) = '月供应商明细表';
            sheet.Cells(2, 1) = GetFilter();

            var columns = $('#Grid').treegrid('getColumnFields');
            for (var i = 0; i < columns.length; i++) {
                sheet.Cells(4, i + 1).Value = $('#Grid').treegrid('getColumnOption', columns[i]).title;
            }

            sheet.Range('A1:' + 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(columns.length - 1, columns.length) + '1').MergeCells = true;
            sheet.Range('A2:' + 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.substring(columns.length - 1, columns.length) + '2').MergeCells = true;
            sheet.Cells(1, 1).HorizontalAlignment = 3;
            sheet.Cells(2, 1).HorizontalAlignment = 3;
            sheet.Rows(1).Font.Bold = true;
            for (var i = 0; i < columns.length; i++) {
                var column = $('#Grid').treegrid('getColumnOption', columns[i]);

                sheet.Columns(i + 1).ColumnWidth = column.width / 8;
                sheet.Columns(i + 1).Font.Size = 9;
                sheet.Columns(i + 1).WrapText = true;
            }
            sheet.Rows(4).Font.Bold = true;

            $('#Grid').treegrid('selectAll');
            var rows = $('#Grid').treegrid('getSelections');
            for (var i = 0; i < rows.length; i++) {
                sheet.Rows(4 + i + 1).Select;
                for (var j = 0; j < columns.length; j++) {
                    SetValueAndStyle(sheet.Cells(4 + i + 1, j + 1), rows[i], columns[j]);
                }
            }
            $('#Grid').treegrid('unselectAll');

            var footers = $('#Grid').treegrid('getFooterRows');
            for (var i = 0; i < footers.length; i++) {
                sheet.Rows(4 + rows.length + i + 1).Select;
                for (var j = 0; j < columns.length; j++) {
                    SetValueAndStyle(sheet.Cells(4 + rows.length + i + 1, j + 1), footers[i], columns[j]);
                }
            }
        };

        Events.GetGrid(Events.GetMonth(), Events.GetSupplier(), 1, $('#Grid').treegrid('options').pageSize, callback);
    },

    OnGrid: function (data) {
        Eventer.Page($('#Grid'), Events.Page);

        return data;
    }
};
