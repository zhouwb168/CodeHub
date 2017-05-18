$(function () {
    $('#Grid').treegrid({ onClickRow: Events.OnClick });
    $('#Grid').treegrid({ loadFilter: Events.OnPage });
    $('#Grid').datagrid('getColumnOption', 'Name').formatter = Events.ToName;
    $('#Buttons').find('[unique="Create"]').bind('click', Events.OnCreate);
    $('#Buttons').find('[unique="Delete"]').bind('click', Events.OnDelete);
    $('#Buttons').find('[unique="Save"]').bind('click', Events.OnSave);
    $('#Buttons').find('[unique="Cancel"]').bind('click', Events.OnCancel);
    
    Eventer.ComboBox($('#Area'), 'GsmArea', 'GetEntities', []);
    Events.Page();
});

var Events = {
    Table: 'GsmOrigin',

    ToName: function (value, row) {
        if (row.Name == null) return row.AreaName;

        return value;
    },

    Page: function () {
        var pageNumber = $('#Grid').treegrid('options').pageNumber;
        var pageSize = $('#Grid').treegrid('options').pageSize;

        Events.GetGrid((pageNumber - 1) * pageSize + 1, pageSize);
    },

    GetGrid: function (start, length) {
        Eventer.Grid($('#Grid'), Events.Table, 'GetGridByMethodAndFieldsAndUnique', ['GetEntitiesWithAreaNameByStartAndLength', [start, length], ['AreaName'], 'Unique']);
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
        Eventer.Set($('#Area'));
        Eventer.Set($('#Name'));
        Eventer.Set($('#Alias'));
        Eventer.Set($('#Except'));
        Eventer.Set($('#Remark'));
    },

    Get: function () {
        return {
            Unique: Eventer.Get($('#Unique')),
            Area: Eventer.Get($('#Area')),
            Name: Eventer.Get($('#Name')),
            Alias: Eventer.Get($('#Alias')),
            Except: Eventer.Get($('#Except')),
            Remark: Eventer.Get($('#Remark')),
            Operator: Account
        };
    },

    Set: function () {
        var row = $('#Grid').treegrid('getSelected');

        Eventer.Set($('#Unique'), row.Unique);
        Eventer.Set($('#Area'), row.Area);
        Eventer.Set($('#Name'), row.Name);
        Eventer.Set($('#Alias'), row.Alias);
        Eventer.Set($('#Except'), row.Except);
        Eventer.Set($('#Remark'), row.Remark);
    },

    Enabled: function () {
        Eventer.Enable($('#Area'));
        Eventer.Enable($('#Name'));
        Eventer.Enable($('#Alias'));
        Eventer.Enable($('#Except'));
        Eventer.Enable($('#Remark'));
    },

    Disabled: function () {
        Eventer.Disable($('#Area'));
        Eventer.Disable($('#Name'));
        Eventer.Disable($('#Alias'));
        Eventer.Disable($('#Except'));
        Eventer.Disable($('#Remark'));
    }
};
