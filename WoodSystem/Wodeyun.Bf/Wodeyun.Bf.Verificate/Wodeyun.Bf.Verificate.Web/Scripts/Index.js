$(function () {
    $('#Grid').treegrid({ onClickRow: Events.OnClick });
    $('#Grid').treegrid({ loadFilter: Events.OnPage });
    $('#Grid').treegrid("getColumnOption", "LinkName").formatter = Events.ToId;
    $('#Grid').treegrid("getColumnOption", "AccountDescription").formatter = Events.ToName;
    $('#Link').combobox({ onChange: Events.OnChange });
    $('#Buttons').find('[unique="Create"]').bind('click', Events.OnCreate);
    $('#Buttons').find('[unique="Delete"]').bind('click', Events.OnDelete);
    $('#Buttons').find('[unique="Save"]').bind('click', Events.OnSave);
    $('#Buttons').find('[unique="Cancel"]').bind('click', Events.OnCancel);

    Eventer.ComboBox($('#Link'), 'Link', 'GetEntities', []);
    Events.Page();
});

var Events = {
    Service: 'Verificate',

    ToId: function (value, row) {
        if (value == null) return row.AccountId;

        return value;
    },

    ToName: function (value) {
        if (value == null) return '';

        return $.parseJSON(value).Name;
    },

    OnChange: function () {
        if (Eventer.Get($('#Link')) == 1) $('#Label').html('密码：');
        else $('#Label').html('用户名：');
    },

    Page: function () {
        var pageNumber = $('#Grid').treegrid('options').pageNumber;
        var pageSize = $('#Grid').treegrid('options').pageSize;

        Events.GetGrid((pageNumber - 1) * pageSize + 1, pageSize);
    },

    GetGrid: function (start, length) {
        Eventer.Grid($('#Grid'), Events.Service, 'GetGridByMethodAndFieldsAndUnique', ['GetEntitiesWithAccountAndLinkNameByStartAndLength', [start, length], ['AccountId'], 'Number']);
    },

    OnClick: function () {
        Eventer.Click($('#Grid'), Buttons, Controls);
    },

    OnPage: function (data) {
        Eventer.Page($('#Grid'), Events.Page, Buttons, Controls);

        return data;
    },

    OnCreate: function () {
        Eventer.Create(Buttons, Controls);
    },

    OnDelete: function () {
        Eventer.Delete($('#Unique'), Events.Service, Events.Page, Buttons, Controls);
    },

    OnSave: function () {
        var message = (Eventer.Get($('#Link')) == 1 ? '请输入密码！' : '请输入用户名！');
        if (Checker.Valid($('#Value'), message) == false) return;

        Eventer.Save($('#Unique'), Events.Service, Events.Page, Buttons, Controls);
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
        Eventer.Set($('#Link'));
        Eventer.Set($('#Value'));
        Eventer.Set($('#Remark'));
    },

    Get: function () {
        var row = $('#Grid').treegrid('getSelected');

        return {
            Unique: Eventer.Get($('#Unique')),
            Account: row.AccountUnique,
            Link: Eventer.Get($('#Link')),
            Value: Eventer.Get($('#Value')),
            Remark: Eventer.Get($('#Remark')),
            Operator: Account
        };
    },

    Set: function () {
        var row = $('#Grid').treegrid('getSelected');

        Eventer.Set($('#Unique'), row.Unique);
        Eventer.Set($('#Link'), row.Link);
        Eventer.Set($('#Value'), row.Value);
        Eventer.Set($('#Remark'), row.Remark);
    },

    Enabled: function () {
        Eventer.Enable($('#Link'));
        Eventer.Enable($('#Value'));
        Eventer.Enable($('#Remark'));
    },

    Disabled: function () {
        Eventer.Disable($('#Link'));
        Eventer.Disable($('#Value'));
        Eventer.Disable($('#Remark'));
    }
};
