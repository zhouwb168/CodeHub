$(function () {
    $('#Grid').datagrid({ onClickRow: Events.OnClick });
    $('#Grid').datagrid({ loadFilter: Events.OnPage });
    $('#Buttons').find('[unique="Create"]').bind('click', Events.OnCreate);
    $('#Buttons').find('[unique="Delete"]').bind('click', Events.OnDelete);
    $('#Buttons').find('[unique="Save"]').bind('click', Events.OnSave);
    $('#Buttons').find('[unique="Cancel"]').bind('click', Events.OnCancel);
    
    Events.Page();
});

var Events = {
    Table: 'GsmTree',

    Page: function () {
        var pageNumber = $('#Grid').datagrid('options').pageNumber;
        var pageSize = $('#Grid').datagrid('options').pageSize;

        Events.GetGrid((pageNumber - 1) * pageSize + 1, pageSize);
    },

    GetGrid: function (start, length) {
        Eventer.Grid($('#Grid'), Events.Table, 'GetEntitiesByStartAndLength', [start, length]);
    },

    OnClick: function () {
        Eventer.Click($('#Grid'), Buttons, Controls);
    },

    OnPage: function (data) {
        Eventer.Page($('#Grid'), Events.Page);

        return data;
    },

    OnCreate: function () {
        Eventer.Create(Buttons, Controls);
    },

    OnDelete: function () {
        Eventer.Delete($('#Unique'), Events.Table, Events.Page, Buttons, Controls);
    },

    OnSave: function () {
        if (Checker.Valid($('#Name'), '请输入名称！') == false) return;

        Eventer.Save($('#Unique'), Events.Table, Events.Page, Buttons, Controls);
    },

    OnCancel: function () {
        Eventer.Cancel(Buttons, Controls);
    }
};

var Buttons = {
    State: 'None',

    Update: function () {
        Eventer.Hide($('#Buttons'), 'Create');
        Eventer.Show($('#Buttons'), 'Delete');
        Eventer.Show($('#Buttons'), 'Save');
        Eventer.Show($('#Buttons'), 'Cancel');

        this.State = 'Update';
    },

    Create: function () {
        Eventer.Hide($('#Buttons'), 'Create');
        Eventer.Hide($('#Buttons'), 'Delete');
        Eventer.Show($('#Buttons'), 'Save');
        Eventer.Show($('#Buttons'), 'Cancel');

        this.State = 'Create';
    },

    Delete: function () {
        Eventer.Show($('#Buttons'), 'Create');
        Eventer.Hide($('#Buttons'), 'Delete');
        Eventer.Hide($('#Buttons'), 'Save');
        Eventer.Hide($('#Buttons'), 'Cancel');

        this.State = 'None';
    },

    Save: function () {
        if (this.State == 'Update' || this.State == 'Create') {
            Eventer.Show($('#Buttons'), 'Create');
            Eventer.Show($('#Buttons'), 'Delete');
            Eventer.Hide($('#Buttons'), 'Save');
            Eventer.Hide($('#Buttons'), 'Cancel');
        }

        this.State = 'None';
    },

    Cancel: function () {
        if (this.State == 'Update') {
            Eventer.Show($('#Buttons'), 'Create');
            Eventer.Show($('#Buttons'), 'Delete');
            Eventer.Hide($('#Buttons'), 'Save');
            Eventer.Hide($('#Buttons'), 'Cancel');
        }

        if (this.State == 'Create') {
            Eventer.Show($('#Buttons'), 'Create');
            Eventer.Hide($('#Buttons'), 'Delete');
            Eventer.Hide($('#Buttons'), 'Save');
            Eventer.Hide($('#Buttons'), 'Cancel');
        }

        this.State = 'None';
    },

    Clear: function () {
        Eventer.Show($('#Buttons'), 'Create');
        Eventer.Hide($('#Buttons'), 'Delete');
        Eventer.Hide($('#Buttons'), 'Save');
        Eventer.Hide($('#Buttons'), 'Cancel');
    }
};

var Controls = {
    Clear: function () {
        Eventer.Set($('#Unique'));
        Eventer.Set($('#Name'));
        Eventer.Set($('#Alias'));
        Eventer.Set($('#Remark'));
    },

    Get: function () {
        return {
            Unique: Eventer.Get($('#Unique')),
            Name: Eventer.Get($('#Name')),
            Alias: Eventer.Get($('#Alias')),
            Remark: Eventer.Get($('#Remark')),
            Operator: Account
        };
    },

    Set: function () {
        var row = $('#Grid').datagrid('getSelected');

        Eventer.Set($('#Unique'), row.Unique);
        Eventer.Set($('#Name'), row.Name);
        Eventer.Set($('#Alias'), row.Alias);
        Eventer.Set($('#Remark'), row.Remark);
    },

    Enabled: function () {
        Eventer.Enable($('#Name'));
        Eventer.Enable($('#Alias'));
        Eventer.Enable($('#Remark'));
    },

    Disabled: function () {
        Eventer.Disable($('#Name'));
        Eventer.Disable($('#Alias'));
        Eventer.Disable($('#Remark'));
    }
};
