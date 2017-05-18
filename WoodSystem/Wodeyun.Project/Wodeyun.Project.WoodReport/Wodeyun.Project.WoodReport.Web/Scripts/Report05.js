$(function () {
    var grid = $('#Grid');
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    grid.datagrid("getColumnOption", "Printed").formatter = Events.ToPrintedText;
    grid.datagrid("getColumnOption", "WeighTime").formatter = Events.ToDateTime;
    grid.datagrid("getColumnOption", "Bang_Time").formatter = Events.ToDateTime;

    $('#txtStartDate').datebox('setValue', moment().format('YYYY-MM-DD'));
    $('#txtEndDate').datebox('setValue', moment().format('YYYY-MM-DD'));

    $('#Query').bind('click', Events.OnQuery);
    $('#Print').bind('click', Events.OnPrint);

    Events.GetGrid(null);
});

var Events = {
    Service: 'WoodReport',

    ToPrintedText: function (value) {
        if (value == null) return '';

        var htmlValue = "";
        if (value == true) htmlValue = "<span style=\"color:red;\">是</span>";
        else htmlValue = "否";

        return htmlValue;
    },

    ToDateTime: function (value) {
        if (value == null) return "";

        return moment(value).format('YYYY-MM-DD HH:mm');
    },

    GetGrid: function (callback) {
        var startDate = Eventer.Get($('#txtStartDate'));
        var endDate = Eventer.Get($('#txtEndDate'));

        var grid = $('#Grid');
        var gridOptons = grid.datagrid('options');
        var pageSize = gridOptons.pageSize;
        var rowStart = (gridOptons.pageNumber - 1) * pageSize + 1;

        var supplier = Eventer.Get($('#txtSupplier')).toUpperCase();
        var license = Eventer.Get($('#txtLicense')).toUpperCase();
        var key = Eventer.Get($('#txtKey')).toUpperCase();
        var printed = parseInt(Eventer.Get($('#sltPrint'), null));

        Eventer.Grid(grid, Events.Service, 'GetReport05BySearchWithPaging', [startDate, endDate, rowStart, pageSize, supplier, license, key, printed], callback);
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
            $.messager.alert('提示消息', '请先选择要打印的检查结果', 'warning');
            return;
        }
        var iswater = 1;
        if (confirm("选“确定”打印重量检查结果单，“取消”打印量方检查结果单。")) iswater = 0;

        var arrID = "0";
        for (var i = 0; i < rowCounts; i++) arrID += "," + checkeds[i].WoodID;
        var printForm = document.createElement("form");
        document.body.appendChild(printForm);
        printForm.method = "get";
        printForm.action = "Print.html";
        printForm.target = "_blank";
        var txtBox = document.createElement("input");
        txtBox.id = "ArrID";
        txtBox.name = "ArrID";
        txtBox.type = "text";
        txtBox.value = arrID + "|" + iswater;
        printForm.appendChild(txtBox);
        printForm.submit();
        document.body.removeChild(printForm);

        Events.OnQuery();
    }
};
