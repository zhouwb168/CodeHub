$(function () {
    var bangGrid = $('#GridOfBang');
    bangGrid.datagrid({ onClickRow: Events.OnClick });
    Eventer.Page(bangGrid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    bangGrid.datagrid("getColumnOption", "Bang_Time").formatter = Events.ToDateTime;

    var woodGrid = $('#GridOfWood');
    woodGrid.datagrid({ onClickRow: Events.OnWoodClick });
    Eventer.Page(woodGrid, Events.PageForWood); // 注意！分页事件不能在onClickRow事件之前进行初始化
    woodGrid.datagrid("getColumnOption", "WeighTime").formatter = Events.ToDateTime;
    woodGrid.datagrid("getColumnOption", "Description").formatter = Events.ToText;

    var joinGrid = $('#GridOfJoin');
    joinGrid.datagrid({ onClickRow: Events.OnJoinClick });
    Eventer.Page(joinGrid, Events.PageForJoin); // 注意！分页事件不能在onClickRow事件之前进行初始化
    joinGrid.datagrid("getColumnOption", "JoinTime").formatter = Events.ToDateTime;
    joinGrid.datagrid("getColumnOption", "Bang_Time").formatter = Events.ToDateTime;
    joinGrid.datagrid("getColumnOption", "WeighTime").formatter = Events.ToDateTime;

    var filterGrid = $('#GridOfFilter');
    filterGrid.datagrid({ onClickRow: Events.OnFilterClick });
    Eventer.Page(filterGrid, Events.PageForFilter); // 注意！分页事件不能在onClickRow事件之前进行初始化
    filterGrid.datagrid("getColumnOption", "Bang_Time").formatter = Events.ToDateTime;

    $('#imgQueryForBang').bind('click', Events.OnQuery);
    $('#aJoin').bind('click', Events.OnSave);
    $('#aQueryForNoSure').bind('click', Events.OnQueryForNoSure);
    $('#aFilte').bind('click', Events.OnFilte);

    $('#imgQueryForJoin').bind('click', Events.OnQueryForJoin);
    $('#aCutOff').bind('click', Events.OnDelete);

    $('#imgQueryForFilter').bind('click', Events.OnQueryFilter);
    $('#aRenew').bind('click', Events.OnRenew);

    $('#ipDateOfBang').datebox('setValue', moment().add('days', -30).format('YYYY-MM-DD'));
    $('#ipDateOfJoin').datebox('setValue', moment().add('days', -30).format('YYYY-MM-DD'));
    $('#ipDateOfFilter').datebox('setValue', moment().add('days', -30).format('YYYY-MM-DD'));
    
    Events.Page();

    $("#divTab").tabs({ onSelect: Events.SelectTab });
});

var Events = {
    Service: 'WoodJoin',

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

        Eventer.Set($('#txtWoodID'), 0);
        $("#Rebate").removeAttr("checked");

        var data = {
            "total": "0",
            "rows": []
        }

        var woodGrid = $('#GridOfWood');
        woodGrid.datagrid('options').pageNumber = 1;
        woodGrid.datagrid('getPager').pagination('refresh', {
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
        var isFiltered = 1;

        Events.GetGrid(grid, date, (pageNumber - 1) * pageSize + 1, pageSize, isFiltered, null);
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
        var isFiltered = 1;
        var params = [parseInt(recordID), isFiltered]

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
        else if (title == "地磅数据对接") Events.Page();
        else if (title == "已被过滤的数据") Events.PageForFilter();
    },

    OnJoinClick: function () {
        var row = $('#GridOfJoin').datagrid('getSelected');

        Eventer.Set($('#txtUnique'), row.Unique);
        Eventer.Set($('#txtCutOffBangID'), row.BangID);
        Eventer.Set($('#txtCutOffBangCID'), row.bangCid);
    },

    PageForJoin: function () {
        var grid = $('#GridOfJoin');
        var gridOptions = grid.datagrid('options');
        var pageNumber = gridOptions.pageNumber;
        var pageSize = gridOptions.pageSize;
        var date = Eventer.Get($('#ipDateOfJoin'));
        var license = Eventer.Get($('#txtLicenseForJoinSearch'));

        Eventer.Grid(grid, Events.Service, 'GetDataOfJoinByDateAndStartAndLength', [date, (pageNumber - 1) * pageSize + 1, pageSize, license], null);
    },

    PageToGirdJoin: function () {
        var grid = $('#GridOfJoin');
        var pageSize = grid.datagrid('options').pageSize;
        var date = Eventer.Get($('#ipDateOfJoin'));
        var license = Eventer.Get($('#txtLicenseForJoinSearch'));

        Eventer.Grid(grid, Events.Service, 'GetDataOfJoinByDateAndStartAndLength', [date, 1, pageSize, license], null);
    },

    OnWoodClick: function () {
        var row = $('#GridOfWood').datagrid('getSelected');
        //if (row.Place != null) $('#Rebate').attr("checked", true);
        Eventer.Set($('#txtWoodID'), row.WoodID);
    },

    PageForWood: function () {
        var grid = $('#GridOfWood');
        var gridOptions = grid.datagrid('options');
        var pageNumber = gridOptions.pageNumber;
        var pageSize = gridOptions.pageSize;

        var license = Eventer.Get($('#txtLicense'));
        var date = Eventer.Get($('#txtBangTime'));

        Eventer.Grid(grid, Events.Service, 'GetDataOfWoodByDateAndStartAndLength', [license, date, (pageNumber - 1) * pageSize + 1, pageSize], null);
    },

    ToText: function (value, row, rowIndex) {
        if (value == null) return value;
        return $.parseJSON(value).Name;
    },

    AutoSelectForWood: function () {
        var grid = $('#GridOfWood');
        var items = grid.datagrid('getRows');

        for (var i = 0; i < items.length; i++) {
            var rowData = items[i];
            Eventer.Set($('#txtWoodID'), rowData.WoodID);
            // if (rowData.Place != null) $('#Rebate').attr("checked", true);
            var rowIndex = grid.datagrid('getRowIndex', rowData);
            grid.datagrid('selectRow', rowIndex);
            break;
        }
    },

    RefreshWood: function () {
        var grid = $('#GridOfWood');
        var pageSize = grid.datagrid('options').pageSize;
        var license = Eventer.Get($('#txtLicense'));
        var date = Eventer.Get($('#txtBangTime'));

        Eventer.Grid(grid, Events.Service, 'GetDataOfWoodByDateAndStartAndLength', [license, date, 1, pageSize], Events.AutoSelectForWood);
    },

    OnQuery: function () {
        Controls.Clear();

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
        var isFiltered = 0;

        Events.GetGrid(grid, date, (pageNumber - 1) * pageSize + 1, pageSize, isFiltered, null);
    },

    GetGrid: function (grid, date, start, length, isFiltered, callback) {
        Eventer.Grid(grid, Events.Service, 'GetDataOfBangByDateAndStartAndLength', [date, start, length, isFiltered], callback);
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
            BangCID: Eventer.Get($('#txtCutOffBangCID')),
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
        if (Eventer.Get($('#txtBangID')) == "0" || Eventer.Get($('#txtBangID')) == "") {
            $.messager.alert('提示消息', '请从上面列表中选择要进行对接的地磅数据记录', 'warning');
            return;
        }
        if (Eventer.Get($('#txtWoodID')) == "0" || Eventer.Get($('#txtWoodID')) == "") {
            $.messager.alert('提示消息', '请从下面列表中选择可对接的电子卡系统数据记录', 'warning');
            return;
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
        Eventer.Set($('#txtBangCID'), 0);
        Eventer.Set($('#txtCutOffBangID'), 0);
        Eventer.Set($('#txtCutOffBangCID'), 0);
        Eventer.Set($('#txtBangTime'));
        Eventer.Set($('#txtLicense'));
        Eventer.Set($('#txtWoodID'), 0);
        Eventer.Set($('#txtRenewID'), 0);
        $("#Rebate").removeAttr("checked");

        var data = {
            "total": "0",
            "rows":[]
        }

        var woodGrid = $('#GridOfWood');
        woodGrid.datagrid('options').pageNumber = 1;
        woodGrid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });
        woodGrid.datagrid('loadData', data);
    },

    Get: function () {
        var rebate = $('#Rebate').is(':checked') ? 1 : 0;
        var gsm = $('#Gsm').is(':checked') ? 1 : 0;
        return {
            Unique: Eventer.Get($('#txtUnique')),
            BangID: Eventer.Get($('#txtBangID')),
            BangCID: Eventer.Get($('#txtBangCID')),
            GsmID: Eventer.Get($('#txtGsmID')),
            WoodID: Eventer.Get($('#txtWoodID')),
            IsRebate: rebate,
            IsGsm: gsm,
            Operator: Account
        };
    },

    Set: function () {
        Controls.Clear();

        var row = $('#GridOfBang').datagrid('getSelected');
        Eventer.Set($('#txtBangID'), row.bangid);
        Eventer.Set($('#txtBangCID'), row.bangCid);
        Eventer.Set($('#txtBangTime'), moment(row.Bang_Time).format('YYYY-MM-DD HH:mm:ss'));
        Eventer.Set($('#txtLicense'), row.carCID);

        Events.RefreshWood();
    },

    Enabled: function () {
    },

    Disabled: function () {
    }
};
