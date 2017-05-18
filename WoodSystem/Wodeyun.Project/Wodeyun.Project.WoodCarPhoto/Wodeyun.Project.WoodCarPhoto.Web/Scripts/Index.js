$(function () {
    $('#Grid').datagrid({ onClickRow: Events.OnClick });
    Eventer.Page($('#Grid'), Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    $('#Grid').datagrid("getColumnOption", "TimeOfStation").formatter = Events.ToDateTime;
    $('#Grid').datagrid("getColumnOption", "GPS").formatter = Events.ToMeter;
    $('#Grid').datagrid("getColumnOption", "PhotoNumber").formatter = Events.ToNumber;
    $('#Grid').datagrid("getColumnOption", "Description").formatter = Events.ToText;
    $('#Grid').datagrid("getColumnOption", "CheckDate").formatter = Events.ToDateTime;

    $('#GridOfPhoto').datagrid("getColumnOption", "PhotoTime").formatter = Events.ToDateTime;
    $('#GridOfPhoto').datagrid("getColumnOption", "GPS").formatter = Events.ToMeter;
    $('#GridOfPhoto').datagrid("getColumnOption", "Unique").formatter = Events.ToShowPoto;

    $('#txtStartDate').datebox('setValue', moment().format('YYYY-MM-DD'));
    $('#txtEndDate').datebox('setValue', moment().format('YYYY-MM-DD'));

    $('#Query').bind('click', Events.OnQuery);

    Events.GetGrid(null);
});

var Events = {
    Service: 'WoodCarPhoto',

    Trim: function (s) {
        return s.replace(/(^\s*)|(\s*$)/g, "");
    },

    ToShowPoto: function (value) {
        var url = "Handers/ShowImage.ashx?PID=" + value;
        var html = "<img style=\"width:580px\" alt=\"车辆电子照片\" src=\"" + url + "\" />";

        return html;
    },

    ToNumber: function (value) {
        if (value == null) return '';

        var htmlValue = "";
        if (parseInt(value) == 0) htmlValue = "<span style=\"color:red;\">0</span> 张";
        else htmlValue = value + " 张";

        return htmlValue;
    },

    ToMeter: function (value) {
        if (value == null) return '';

        var str = "";
        if (parseFloat(value) > 5000) str = "<span style=\"color:red;\">" + value + "</span> 米";
        else str = value + " 米";

        return str;
    },

    ToText: function (value, row, rowIndex) {
        if (value == null) return '';

        return $.parseJSON(value).Name;
    },

    ToDateTime: function (value) {
        if (value == null) return '';

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
        var palce = Eventer.Get($('#txtPlace'));

        Eventer.Grid(grid, Events.Service, 'GetCarPhotoReportBySearchWithPaging', [startDate, endDate, rowStart, pageSize, supplier, license, palce], callback);
    },

    Page: function () {
        Events.GetGrid(null);
    },

    OnClick: function (event) {
        Controls.Clear();

        var row = $('#Grid').datagrid('getSelected');
        var recordID = parseInt(row.Unique);
        var grid = $('#GridOfPhoto');
        var pageSize = grid.datagrid('options').pageSize;

        Eventer.Grid(grid, Events.Service, 'GetCarPhotoListByRecordID', [recordID, 1, pageSize], null);
    },

    OnQuery: function () {
        var grid = $('#Grid');
        grid.datagrid('options').pageNumber = 1;
        grid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });

        Events.GetGrid(null);
    }
};

var Controls = {
    Clear: function () {
        var data = {
            "total": "0",
            "rows": []
        }

        var woodGrid = $('#GridOfPhoto');
        woodGrid.datagrid('options').pageNumber = 1;
        woodGrid.datagrid('loadData', data);
    }

};


