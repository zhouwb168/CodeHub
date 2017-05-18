$(function () {
    $('#Grid').datagrid({ onClickRow: Events.OnClick });
    $('#Grid').datagrid({ loadFilter: Events.OnPage });
    $("#Buttons").find("[unique='Save']").bind('click', Events.OnSave);
    $("#Buttons").find("[unique='Cancel']").bind('click', Events.OnCancel);

    Events.Page();
    Events.GetFunction();
});

var Events = {
    Service: 'Grant',

    GetFunction: function () {
        Eventer.Grid($('#Function'), 'Function', 'GetGridByMethodAndParent', ['GetEntities', [], 'Parent']);
    },

    Page: function () {
        var pageNumber = $('#Grid').datagrid('options').pageNumber;
        var pageSize = $('#Grid').datagrid('options').pageSize;

        Events.GetGrid((pageNumber - 1) * pageSize + 1, pageSize);
    },

    GetGrid: function (start, length) {
        Eventer.Grid($('#Grid'), 'Role', 'GetEntitiesByStartAndLength', [start, length]);
    },

    OnClick: function () {
        Buttons.Update();

        var row = $('#Grid').datagrid('getSelected');
        var items = $('#Function').treegrid('getChildren');

        for (var i = 0; i < items.length; i++) {
            $('#Function').treegrid('unselect', items[i].Unique)
        }

        var data = {
            service: Events.Service,
            method: 'GetEntitiesByField',
            args: ['Role', row.Unique, '=']
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }

            var items = Ajaxer.GetItems(root);

            for (var i = 0; i < items.length; i++) {
                $('#Function').treegrid('select', items[i].Function);
            }
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    OnPage: function (data) {
        Eventer.Page($('#Grid'), Events.Page);

        return data;
    },

    OnSave: function () {
        var row = $('#Grid').datagrid('getSelected');
        var checkeds = $('#Function').treegrid('getSelections');

        var collection = [];
        collection[0] = JSON.stringify({
            Role: row.Unique
        });
        for (var i = 0; i < checkeds.length; i++) {
            var entity = {
                Unique: '',
                Role: row.Unique,
                Function: checkeds[i].Unique,
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
