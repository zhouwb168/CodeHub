$(function () {
    var grid = $('#Grid');
    grid.datagrid({ onClickRow: Events.OnClick });
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    grid.datagrid("getColumnOption", "Unique").formatter = Events.ToName;
    grid.datagrid("getColumnOption", "Description").formatter = Events.ToDepartment;

    var tdOfButtons = $('#Buttons');
    tdOfButtons.find("[unique='Save']").bind('click', Events.OnSave);
    tdOfButtons.find("[unique='Cancel']").bind('click', Events.OnCancel);

    Events.Page();
    Events.GetPower();
});

var Events = {
    Service: 'WoodPowerOfGps',

    ToDepartment: function (value, row, rowIndex) {
        if (row.Description == null) return '';

        return $.parseJSON(row.Description).Department;
    },

    ToName: function (value, row, rowIndex) {
        if (row.Description == null) return '';

        return $.parseJSON(row.Description).Name;
    },

    GetPower: function () {
        Eventer.Grid($('#Power'), 'WoodGps', 'GetEntities', []);
    },

    Page: function () {
        var gridOptions = $('#Grid').datagrid('options');
        var pageSize = gridOptions.pageSize;

        Events.GetGrid((gridOptions.pageNumber - 1) * pageSize + 1, pageSize);
    },

    GetGrid: function (start, length) {
        Eventer.Grid($('#Grid'), 'Account', 'GetEntitiesByStartAndLength', [start, length]);
    },

    OnClick: function () {
        Buttons.Update();
        $('#Power').datagrid('unselectAll');

        var row = $('#Grid').datagrid('getSelected');
        var data = {
            service: Events.Service,
            method: 'GetEntitiesByField',
            args: ['AccountID', row.Unique, '=']
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }

            var items = Ajaxer.GetItems(root);
            var powerGrid = $('#Power');
            var itemCount = items.length;
            for (var i = 0; i < itemCount; i++) {
                powerGrid.datagrid('selectRecord', items[i].GpsID);
            }
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    OnSave: function () {
        var row = $('#Grid').datagrid('getSelected');
        var checkeds = $('#Power').datagrid('getSelections');

        var collection = [];
        collection[0] = JSON.stringify({
            AccountID: row.Unique
        });
        for (var i = 0; i < checkeds.length; i++) {
            var entity = {
                Unique: '',
                AccountID: row.Unique,
                GpsID: checkeds[i].Unique,
                Operator: Account
            };
            collection[i + 1] = JSON.stringify(entity);
        }

        var data = {
            service: Events.Service,
            method: 'SaveEntities',
            args: [collection]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }

            alert(root.Message);

            Buttons.Save();
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    OnCancel: function () {
        Buttons.Cancel();
    }
};

var Buttons = {
    State: 'None',

    Update: function () {
        Eventer.Show($('#Buttons'), 'Save');
        Eventer.Show($('#Buttons'), 'Cancel');

        this.State = 'Update';
    },

    Save: function () {
        if (this.State == 'Update') {
            Eventer.Hide($('#Buttons'), 'Save');
            Eventer.Hide($('#Buttons'), 'Cancel');
        }

        this.State = 'None';
    },

    Cancel: function () {
        if (this.State == 'Update') {
            Eventer.Hide($('#Buttons'), 'Save');
            Eventer.Hide($('#Buttons'), 'Cancel');
        }

        this.State = 'None';
    }
};
