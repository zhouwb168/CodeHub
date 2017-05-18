$(function () {
    var grid = $('#Grid');
    grid.datagrid({ onClickRow: Events.OnClick });
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    grid.datagrid("getColumnOption", "Unique").formatter = Events.ToName;
    grid.datagrid("getColumnOption", "Description").formatter = Events.ToDepartment;

    $("#Buttons").find("[unique='Save']").bind('click', Events.OnSave);
    $("#Buttons").find("[unique='Cancel']").bind('click', Events.OnCancel);

    Events.Page();
    Events.GetRole();
});

var Events = {
    Service: 'Act',

    ToDepartment: function (value, row, rowIndex) {
        if (row.Description == null) return '';

        return $.parseJSON(row.Description).Department;
    },

    ToName: function (value, row, rowIndex) {
        if (row.Description == null) return '';

        return $.parseJSON(row.Description).Name;
    },

    GetRole: function () {
        Eventer.Grid($('#Role'), 'Role', 'GetEntities', []);
    },

    Page: function () {
        var pageNumber = $('#Grid').datagrid('options').pageNumber;
        var pageSize = $('#Grid').datagrid('options').pageSize;

        Events.GetGrid((pageNumber - 1) * pageSize + 1, pageSize);
    },

    GetGrid: function (start, length) {
        Eventer.Grid($('#Grid'), 'Account', 'GetEntitiesByStartAndLength', [start, length]);
    },

    OnClick: function () {
        Buttons.Update();
        $('#Role').datagrid('unselectAll');

        var row = $('#Grid').datagrid('getSelected');
        var data = {
            service: Events.Service,
            method: 'GetEntitiesByField',
            args: ['Account', row.Unique, '=']
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }

            var items = Ajaxer.GetItems(root);

            for (var i = 0; i < items.length; i++) {
                $('#Role').datagrid('selectRecord', items[i].Role);
            }
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    OnSave: function () {
        var row = $('#Grid').datagrid('getSelected');
        var checkeds = $('#Role').datagrid('getSelections');

        var collection = [];
        collection[0] = JSON.stringify({
            Account: row.Unique
        });
        for (var i = 0; i < checkeds.length; i++) {
            var entity = {
                Unique: '',
                Account: row.Unique,
                Role: checkeds[i].Unique,
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
