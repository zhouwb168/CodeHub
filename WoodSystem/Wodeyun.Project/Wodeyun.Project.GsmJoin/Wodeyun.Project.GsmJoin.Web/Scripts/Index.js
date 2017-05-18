$(function () {
    var bangGrid = $('#GridOfBang');
    bangGrid.datagrid({ onClickRow: Events.OnClick });
    Eventer.Page(bangGrid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    bangGrid.datagrid("getColumnOption", "Bang_Time").formatter = Events.ToDateTime;
    //bangGrid.datagrid("getColumnOption", "WeighTime").formatter = Events.ToDateTime;
    bangGrid.datagrid("getColumnOption", "FullVolume").formatter = Events.ToFixed;

    var gsmGrid = $('#GridOfGsm');
    gsmGrid.datagrid({ onClickRow: Events.OnGsmClick });
    Eventer.Page(gsmGrid, Events.PageForGsm); // 注意！分页事件不能在onClickRow事件之前进行初始化
    gsmGrid.datagrid("getColumnOption", "Date").formatter = Events.ToDateTime;
    gsmGrid.datagrid("getColumnOption", "Ship").formatter = Events.ToDateTime;

    var joinGrid = $('#GridOfJoin');
    joinGrid.datagrid({
        rowStyler: function (index, row) {
            if (row.IsAdd == 1) {
                return 'background-color:pink;color:red;font-weight:bold;';
            }
        }
    });
    joinGrid.datagrid({ onClickRow: Events.OnJoinClick });
    Eventer.Page(joinGrid, Events.PageForJoin); // 注意！分页事件不能在onClickRow事件之前进行初始化
    joinGrid.datagrid("getColumnOption", "JoinTime").formatter = Events.ToDateTime;
    joinGrid.datagrid("getColumnOption", "Bang_Time").formatter = Events.ToDateTime;
    //joinGrid.datagrid("getColumnOption", "WeighTime").formatter = Events.ToDateTime;
    joinGrid.datagrid("getColumnOption", "Date").formatter = Events.ToDateTime;
    joinGrid.datagrid("getColumnOption", "Ship").formatter = Events.ToDateTime;
    joinGrid.datagrid("getColumnOption", "FullVolume").formatter = Events.ToFixed;

    var filterGrid = $('#GridOfFilter');
    filterGrid.datagrid({ onClickRow: Events.OnFilterClick });
    Eventer.Page(filterGrid, Events.PageForFilter); // 注意！分页事件不能在onClickRow事件之前进行初始化
    filterGrid.datagrid("getColumnOption", "Bang_Time").formatter = Events.ToDateTime;

    $('#imgQueryForBang').bind('click', Events.OnQuery);
    $('#aJoin').bind('click', Events.OnSave);
    $('#aNoGsmJoin').bind('click', Events.OnBatchJoin);
    $('#aQueryForNoSure').bind('click', Events.OnQueryForNoSure);
    $('#aFilte').bind('click', Events.OnFilte);

    $('#imgQueryForJoin').bind('click', Events.OnQueryForJoin);
    $('#aCutOff').bind('click', Events.OnDelete);

    $('#imgQueryForFilter').bind('click', Events.OnQueryFilter);
    $('#aRenew').bind('click', Events.OnRenew);

    $('#ipDateOfBang').datebox('setValue', moment().add('days', -30).format('YYYY-MM-DD'));
    $('#ipDateOfBangend').datebox('setValue', moment().format('YYYY-MM-DD'));
    $('#ipDateOfJoin').datebox('setValue', moment().add('days', -30).format('YYYY-MM-DD'));
    $('#ipDateOfFilter').datebox('setValue', moment().add('days', -30).format('YYYY-MM-DD'));

    //绑定区域
    Eventer.ComboBox($('#Area'), 'GsmArea', 'GetEntities', []);

    Events.Page();

    $("#divTab").tabs({ onSelect: Events.SelectTab });
});

var Events = {
    Service: 'GsmJoin',

    OnQueryForNoSure: function () {
        var date = Eventer.Get($('#txtBangTime'));
        if (date == "") {
            $.messager.alert('提示消息', '请从上面列表中选择要进行对接的地磅数据记录', 'warning');
            return;
        }
        var license = Eventer.Get($('#txtLicense'));
        if (license == "") {
            $.messager.alert('提示消息', '请输入要模糊查询的车号', 'warning');
            return;
        }

        Eventer.Set($('#txtGsmID'), 0);

        var data = {
            "total": "0",
            "rows": []
        }

        var gsmGrid = $('#GridOfGsm');
        gsmGrid.datagrid('options').pageNumber = 1;
        gsmGrid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });

        Events.RefreshWood();
    },

    ToDateTimeWithTip: function (value, row, rowIndex) {
        if (value == null) return '';

        var str = moment(value).format('YYYY-MM-DD HH:mm:ss');
        if (row.Text != null) str = '<span title="' + row.Text + '">' + moment(value).format('YYYY-MM-DD HH:mm:ss') + '</span>';

        return str;
    },

    ToTip: function (value, row, rowIndex) {
        if (value == null) return '';

        return '<span title="' + value + '">' + value + '</span>';
    },

    ToFixed: function (value) {
        if (value == null) return '';
        return value.toFixed(2);
    },

    ToTimeWithTip: function (value, row, rowIndex) {
        if (value == null) return '';

        var str = '';
        if (row.Text != null) str = '<span title="' + row.Text + '">' + moment(value).format('HH:mm') + '</span>';
        else str = '<span title="' + moment(value).format('YYYY-MM-DD HH:mm') + '">' + moment(value).format('HH:mm') + '</span>';

        return str;
    },

    OnRenew: function () {
        if (Eventer.Get($('#txtRenewID')) == "0" || Eventer.Get($('#txtRenewID')) == "") {
            $.messager.alert('提示消息', '请从下面列表中选择要恢复的数据记录', 'warning');
            return;
        }

        var recordID = Eventer.Get($('#txtRenewID'));
        var isFiltered = 0;
        var params = [parseInt(recordID), isFiltered]

        var data = {
            service: Events.Service,
            method: "UpdateFilter",
            args: params
        };

        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }

            alert(root.Message);
            Controls.Clear();
            Events.PageForFilter();
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    OnQueryFilter: function () {
        Controls.Clear();

        var filterGrid = $('#GridOfFilter');
        filterGrid.datagrid('options').pageNumber = 1;
        filterGrid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });

        Events.PageForFilter();
    },

    PageForFilter: function () {
        var grid = $('#GridOfFilter');
        var gridOptions = grid.datagrid('options');
        var pageNumber = gridOptions.pageNumber;
        var pageSize = gridOptions.pageSize;
        var date = Eventer.Get($('#ipDateOfFilter'));
        var IsFilterGsmed = 1;

        Events.GetGridFilter(grid, date, (pageNumber - 1) * pageSize + 1, pageSize, IsFilterGsmed, null);
    },

    GetGridFilter: function (grid, date, start, length, IsFilterGsmed, callback) {
        Eventer.Grid(grid, Events.Service, 'GetDataOfBangByDateAndStartAndLength', [date, start, length, IsFilterGsmed], callback);
    },

    OnFilterClick: function () {
        var row = $('#GridOfFilter').datagrid('getSelected');

        Eventer.Set($('#txtRenewID'), row.bangid);
    },

    OnFilte: function () {
        if (Eventer.Get($('#txtBangID')) == "0" || Eventer.Get($('#txtBangID')) == "") {
            $.messager.alert('提示消息', '请从上面列表中选择要过滤的地磅数据记录', 'warning');
            return;
        }

        var recordID = Eventer.Get($('#txtBangID'));
        var IsFilterGsmed = 1;
        var params = [parseInt(recordID), IsFilterGsmed]

        var data = {
            service: Events.Service,
            method: "UpdateFilter",
            args: params
        };

        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                $.messager.alert('提示消息', root.Message, 'error');
                return;
            }

            $.messager.alert('提示消息', root.Message, 'info');
            Controls.Clear();
            Events.Page();
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    OnQueryForJoin: function () {
        Controls.Clear();

        var joinGrid = $('#GridOfJoin');
        joinGrid.datagrid('options').pageNumber = 1;
        joinGrid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });

        Events.PageToGirdJoin();
    },

    SelectTab: function (title) {
        Controls.Clear();
        if (title == "已成功对接的数据") Events.PageToGirdJoin();
        else if (title == "报备信息对接") Events.Page();
        else if (title == "已被过滤的数据") Events.PageForFilter();
    },

    OnJoinClick: function () {
        var row = $('#GridOfJoin').datagrid('getSelected');

        Eventer.Set($('#txtUnique'), row.Unique);
        Eventer.Set($('#txtCutOffBangID'), row.BangID);
    },

    //批量对接区域
    OnBatchJoin: function () {
        var checkeds = $('#GridOfBang').datagrid('getChecked');
        var rowCounts = checkeds.length;
        if (rowCounts < 1) {
            $.messager.alert('提示消息', '请选择要对接的过磅信息', 'warning');
            return;
        }
        if (confirm("确定要对接吗？") == false) return;
        var BangIDs = [], WoodIDs = [];
        for (var i = 0; i < rowCounts; i++) {
            if (checkeds[i].BangID == 0) continue;
            BangIDs.push(checkeds[i].BangID);
            WoodIDs.push(checkeds[i].WoodID);
        }
        var areaid = $('#Area').combobox("getValue");
        var data = {
            service: Events.Service,
            method: 'BatchJoinData',
            args: [JSON.stringify({
                BangIDs: BangIDs.join(","),
                WoodIDs: WoodIDs.join(","),
                AreaID: areaid,
                Account: Account
            })]
        };

        var success = function (result) {
            var root = Ajaxer.GetRoot(result);
            switch (root) {
                case "SUCCESS":
                    $.messager.alert('提示消息', "对接成功。", 'info');
                    break;
                case "FAIL":
                    $.messager.alert('提示消息', "对接失败。", 'error');
                    break;
                default:
                    $.messager.alert('提示消息', "对接异常。", 'error');
                    break;
            }
            $('#GridOfBang').datagrid('clearChecked');
            Events.Page();
        };
        Ajaxer.Ajax(Setter.Url, data, success);

    },

    PageForJoin: function () {
        var grid = $('#GridOfJoin');
        var gridOptions = grid.datagrid('options');
        var pageNumber = gridOptions.pageNumber;
        var pageSize = gridOptions.pageSize;
        var date = Eventer.Get($('#ipDateOfJoin'));
        var license = Eventer.Get($('#txtLicenseForJoinSearch'));

        Eventer.Grid(grid, Events.Service, 'GetDataOfJoinGsmByDateAndStartAndLength', [date, (pageNumber - 1) * pageSize + 1, pageSize, license], null);
    },

    PageToGirdJoin: function () {
        var grid = $('#GridOfJoin');
        var pageSize = grid.datagrid('options').pageSize;
        var date = Eventer.Get($('#ipDateOfJoin'));
        var license = Eventer.Get($('#txtLicenseForJoinSearch'));

        Eventer.Grid(grid, Events.Service, 'GetDataOfJoinGsmByDateAndStartAndLength', [date, 1, pageSize, license], null);
    },

    OnGsmClick: function () {
        var row = $('#GridOfGsm').datagrid('getSelected');
        Eventer.Set($('#txtGsmID'), row.Unique);
    },

    PageForGsm: function () {
        var grid = $('#GridOfGsm');
        var gridOptions = grid.datagrid('options');
        var pageNumber = gridOptions.pageNumber;
        var pageSize = gridOptions.pageSize;

        var date = Eventer.Get($('#txtBangTime'));
        var license = $('#chklicense').is(':checked') ? Eventer.Get($('#txtLicense')) : "";
        var driver = $('#chkdriver').is(':checked') ? Eventer.Get($('#txtDriver')) : "";
        var supplier = $('#chksupplier').is(':checked') ? Eventer.Get($('#txtSupplier')) : "";
        var area = "";
        Eventer.Grid(grid, Events.Service, 'GetDataOfGsmByDateAndStartAndLength', [license, driver, supplier, area, date, (pageNumber - 1) * pageSize + 1, pageSize], null);
    },

    ToText: function (value, row, rowIndex) {
        if (value == null) return value;
        return $.parseJSON(value).Name;
    },

    AutoSelectForWood: function () {
        var grid = $('#GridOfGsm');
        var items = grid.datagrid('getRows');

        for (var i = 0; i < items.length; i++) {
            var rowData = items[i];
            Eventer.Set($('#txtGsmID'), rowData.Unique);
            var rowIndex = grid.datagrid('getRowIndex', rowData);
            grid.datagrid('selectRow', rowIndex);
            break;
        }
    },

    RefreshWood: function () {
        var grid = $('#GridOfGsm');
        var pageSize = grid.datagrid('options').pageSize;
        var date = Eventer.Get($('#txtBangTime'));

        var license = $('#chklicense').is(':checked') ? Eventer.Get($('#txtLicense')) : "";
        var driver = $('#chkdriver').is(':checked') ? Eventer.Get($('#txtDriver')) : "";
        var supplier = $('#chksupplier').is(':checked') ? Eventer.Get($('#txtSupplier')) : "";
        var area = "";

        Eventer.Grid(grid, Events.Service, 'GetDataOfGsmByDateAndStartAndLength', [license, driver, supplier, area, date, 1, pageSize], Events.AutoSelectForWood);
    },

    OnQuery: function () {
        Controls.Clear();
        $('#GridOfBang').datagrid('clearChecked');
        var bangGrid = $('#GridOfBang');
        bangGrid.datagrid('options').pageNumber = 1;
        bangGrid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });

        Events.Page();
    },

    GetDate: function (dateTextBox) {
        return Eventer.Get(dateTextBox);
    },

    ToDateTime: function (value) {
        if (value == null) return '';

        return moment(value).format('YYYY-MM-DD HH:mm:ss');
    },

    Page: function () {
        var grid = $('#GridOfBang');
        var gridOptions = grid.datagrid('options');
        var pageNumber = gridOptions.pageNumber;
        var pageSize = gridOptions.pageSize;
        var date = Eventer.Get($('#ipDateOfBang'));
        var enddate = Eventer.Get($('#ipDateOfBangend'));
        var license = Eventer.Get($('#txtcarid'));
        var area = Eventer.Get($('#txtarea'));

        Events.GetGrid(grid, date, enddate, (pageNumber - 1) * pageSize + 1, pageSize, license, area, null);
    },

    GetGrid: function (grid, date, enddate, start, length, license, area, callback) {
        Eventer.Grid(grid, Events.Service, 'GetDataOfJoinByDateAndStartAndLength', [date, enddate, start, length, license, area], callback);
    },

    OnClick: function () {
        Eventer.Click($('#GridOfBang'), Buttons, Controls);
    },

    OnDelete: function () {
        if (Eventer.Get($('#txtUnique')) == "0" || Eventer.Get($('#txtUnique')) == "") {
            $.messager.alert('提示消息', '请从下面列表中选择要断开对接的数据记录', 'info');
            return;
        }
        if (confirm("确定要断开这些数据的对接吗？断开后，可以重新进行对接") == false) return;

        var entity = {
            Unique: Eventer.Get($('#txtUnique')),
            BangID: Eventer.Get($('#txtCutOffBangID')),
            Operator: Account
        };
        var params = [JSON.stringify(entity)];

        var data = {
            service: Events.Service,
            method: "CutOffJoin",
            args: params
        };

        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                $.messager.alert('提示消息', root.Message, 'error');
                return;
            }

            Controls.Clear();
            $.messager.alert('提示消息', root.Message, 'info');
            Events.PageToGirdJoin();
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    OnSave: function () {
        Eventer.Set($('#txtJoinType'), this.id);
        if (Eventer.Get($('#txtBangID')) == "0" || Eventer.Get($('#txtBangID')) == "") {
            $.messager.alert('提示消息', '请从上面列表中选择要进行对接的地磅数据记录', 'warning');
            return;
        }
        if (this.id == "aJoin") {
            if (Eventer.Get($('#txtGsmID')) == "0" || Eventer.Get($('#txtGsmID')) == "") {
                $.messager.alert('提示消息', '请从下面列表中选择可对接的短信报备数据记录', 'warning');
                return;
            }
        }
        Eventer.Save($('#txtUnique'), Events.Service, Events.Page, Buttons, Controls);
    },

    OnCancel: function () {
        Eventer.Cancel(Buttons, Controls);
    }

};

var Buttons = {
    State: 'None',

    Update: function () {
        Eventer.Show($('#Buttons'), 'Delete');
        Eventer.Show($('#Buttons'), 'Save');

        this.State = 'Update';
    },

    Create: function () {
        this.State = 'Create';
    },

    Delete: function () {
        Eventer.Hide($('#Buttons'), 'Delete');
        Eventer.Hide($('#Buttons'), 'Save');

        this.State = 'None';
    },

    Save: function () {
        if (this.State == 'Update' || this.State == 'Create') {
        }

        this.State = 'None';
    },

    Cancel: function () {
        if (this.State == 'Update') {
            Eventer.Show($('#Buttons'), 'Delete');
            Eventer.Hide($('#Buttons'), 'Save');
        }

        if (this.State == 'Create') {
            Eventer.Hide($('#Buttons'), 'Delete')
        }

        this.State = 'None';
    },

    Clear: function () {
    }
};

var Controls = {
    Clear: function () {
        Eventer.Set($('#txtUnique'), 0);
        Eventer.Set($('#txtBangID'), 0);
        Eventer.Set($('#txtWoodID'), 0);
        Eventer.Set($('#txtCutOffBangID'), 0);
        Eventer.Set($('#txtBangTime'));
        Eventer.Set($('#txtLicense'));
        Eventer.Set($('#txtDriver'));
        Eventer.Set($('#txtSuppler'));
        Eventer.Set($('#txtGsmID'), 0);
        Eventer.Set($('#txtRenewID'), 0);
        Eventer.Set($('#txtJoinType'));
        Eventer.Set($('#Area'));
        $("#IsAdd").removeAttr("checked");

        var data = {
            "total": "0",
            "rows": []
        }

        //$('#GridOfBang').datagrid('clearChecked');
        var gsmGrid = $('#GridOfGsm');
        gsmGrid.datagrid('options').pageNumber = 1;
        gsmGrid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });
        gsmGrid.datagrid('loadData', data);
    },

    Get: function () {
        var isadd = $('#IsAdd').is(':checked') ? 1 : 0;
        var jointype = Eventer.Get($('#txtJoinType'));
        var areaid = jointype == "aNoGsmJoin" ? $('#Area').combobox("getValue") : 0;
        return {
            Unique: Eventer.Get($('#txtUnique')),
            BangID: Eventer.Get($('#txtBangID')),
            GsmID: jointype == "aNoGsmJoin" ? 0 : Eventer.Get($('#txtGsmID')),
            WoodID: Eventer.Get($('#txtWoodID')),
            IsAdd: isadd,//补报
            IsGsm: 1,
            AreaID: areaid,
            Operator: Account
        };
    },

    Set: function () {
        Controls.Clear();
        var row = $('#GridOfBang').datagrid('getSelected');
        Eventer.Set($('#txtBangID'), row.BangID);
        Eventer.Set($('#txtWoodID'), row.WoodID);
        Eventer.Set($('#txtBangTime'), moment(row.Bang_Time).format('YYYY-MM-DD HH:mm:ss'));
        Eventer.Set($('#txtLicense'), row.carCID);
        Eventer.Set($('#txtDriver'), row.Driver);
        Eventer.Set($('#txtSupplier'), row.Supplier);
        //Eventer.Set($('#Area'), row.Area, 'setText');
        Events.RefreshWood();
    },

    Enabled: function () {
    },

    Disabled: function () {
    }
};
