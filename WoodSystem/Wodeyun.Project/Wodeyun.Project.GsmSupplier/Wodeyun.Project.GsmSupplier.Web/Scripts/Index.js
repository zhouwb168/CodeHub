$(function () {
    $('#Grid').treegrid({ onClickRow: Events.OnClick });
    $('#Grid').treegrid({ loadFilter: Events.OnPage });
    $('#Grid').datagrid('getColumnOption', 'Name').formatter = Events.ToName;
    $('#Buttons').find('[unique="Create"]').bind('click', Events.OnCreate);
    $('#Buttons').find('[unique="Delete"]').bind('click', Events.OnDelete);
    $('#Buttons').find('[unique="Save"]').bind('click', Events.OnSave);
    $('#Buttons').find('[unique="Cancel"]').bind('click', Events.OnCancel);
    
    Events.Page();
});

var Events = {
    Table: 'GsmSupplier',

    ToName: function (value, row) {
        if (row.Type == 0 && row.Name == null) return '供应商';
        if (row.Type == 1 && row.Name == null) return '散户';

        return value;
    },

    Page: function () {
        var pageNumber = $('#Grid').treegrid('options').pageNumber;
        var pageSize = $('#Grid').treegrid('options').pageSize;

        Events.GetGrid((pageNumber - 1) * pageSize + 1, pageSize);
    },

    GetGrid: function (start, length) {
        Eventer.Grid($('#Grid'), Events.Table, 'GetGridByMethodAndFieldsAndUnique', ['GetEntitiesByStartAndLength', [start, length], ['Type'], 'Unique']);
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
        if (Checker.Valid($('#Name'), '请输入供应商代码！') == false) return;

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
        Eventer.Set($('#Type'), 0);
        Eventer.Set($('#Name'));
        Eventer.Set($('#Remark'));
        Eventer.Set($('#LinkMan'));
        Eventer.Set($('#LinkPhone'));
    },

    Get: function () {
        return {
            Unique: Eventer.Get($('#Unique')),
            Type: Eventer.Get($('#Type')),
            Name: Eventer.Get($('#Name')),
            Remark: Eventer.Get($('#Remark')),
            LinkMan: Eventer.Get($('#LinkMan')),
            LinkPhone: Eventer.Get($('#LinkPhone')),
            Operator: Account
        };
    },

    Set: function () {
        var row = $('#Grid').treegrid('getSelected');

        Eventer.Set($('#Unique'), row.Unique);
        Eventer.Set($('#Type'), row.Type);
        Eventer.Set($('#Name'), row.Name);
        Eventer.Set($('#Remark'), row.Remark);
        Eventer.Set($('#LinkMan'), row.LinkMan);
        Eventer.Set($('#LinkPhone'), row.LinkPhone);
    },

    Enabled: function () {
        Eventer.Enable($('#Type'));
        Eventer.Enable($('#Name'));
        Eventer.Enable($('#Remark'));
        Eventer.Enable($('#LinkMan'));
        Eventer.Enable($('#LinkPhone'));
    },

    Disabled: function () {
        Eventer.Disable($('#Type'));
        Eventer.Disable($('#Name'));
        Eventer.Disable($('#Remark'));
        Eventer.Disable($('#LinkMan'));
        Eventer.Disable($('#LinkPhone'));
    }
};
