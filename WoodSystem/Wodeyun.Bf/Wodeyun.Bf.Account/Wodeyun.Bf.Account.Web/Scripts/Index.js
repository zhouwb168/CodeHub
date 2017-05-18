$(function () {
    var grid = $('#Grid');
    grid.datagrid({ onClickRow: Events.OnClick });
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    grid.datagrid("getColumnOption", "Unique").formatter = Events.ToName;
    grid.datagrid("getColumnOption", "Description").formatter = Events.ToDepartment;

    $('#Buttons').find('[unique="Create"]').bind('click', Events.OnCreate);
    $('#Buttons').find('[unique="Delete"]').bind('click', Events.OnDelete);
    $('#Buttons').find('[unique="Save"]').bind('click', Events.OnSave);
    $('#Buttons').find('[unique="Cancel"]').bind('click', Events.OnCancel);

    Events.Page();
});

var Events = {
    Service: 'Account',

    ToDepartment: function (value, row, rowIndex) {
        if (row.Description == null) return '';

        return $.parseJSON(row.Description).Department;
    },

    ToName: function (value, row, rowIndex) {
        if (row.Description == null) return '';

        return $.parseJSON(row.Description).Name;
    },

    Page: function () {
        var pageNumber = $('#Grid').datagrid('options').pageNumber;
        var pageSize = $('#Grid').datagrid('options').pageSize;

        Events.GetGrid((pageNumber - 1) * pageSize + 1, pageSize);
    },

    GetGrid: function (start, length) {
        Eventer.Grid($('#Grid'), Events.Service, 'GetEntitiesByStartAndLength', [start, length]);
    },

    OnClick: function () {
        Eventer.Click($('#Grid'), Buttons, Controls);
    },

    OnCreate: function () {
        Eventer.Create(Buttons, Controls);
    },

    OnDelete: function () {
        Eventer.Delete($('#Unique'), Events.Service, Events.Page, Buttons, Controls);
    },

    OnSave: function () {
        if (Checker.Valid($('#Id'), '请输入Id！') == false) return;
        if (Checker.Valid($('#Department'), '请输入单位！') == false) return;
        if (Checker.Valid($('#Name'), '请输入姓名！') == false) return;

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
        Eventer.Set($('#Id'));
        Eventer.Set($('#Department'));
        Eventer.Set($('#Name'));
        Eventer.Set($('#Mobile'));
        Eventer.Set($('#Remark'));
        Eventer.Set($('#Status'));
    },

    Get: function () {
        var description = {
            Department: Eventer.Get($('#Department')),
            Name: Eventer.Get($('#Name')),
            Mobile: Eventer.Get($('#Mobile'))
        };
        return {
            Unique: Eventer.Get($('#Unique')),
            Id: Eventer.Get($('#Id')),
            Description: JSON.stringify(description),
            Remark: Eventer.Get($('#Remark')),
            Status: Eventer.Get($('#Status')),
            Operator: Account
        };
    },

    Set: function () {
        var row = $('#Grid').datagrid('getSelected');
        var description = $.parseJSON(row.Description);

        Eventer.Set($('#Unique'), row.Unique);
        Eventer.Set($('#Id'), row.Id);
        Eventer.Set($('#Department'), description.Department);
        Eventer.Set($('#Name'), description.Name);
        Eventer.Set($('#Mobile'), description.Mobile);
        Eventer.Set($('#Remark'), row.Remark);
        Eventer.Set($('#Status'), row.Status);
    },

    Enabled: function () {
        Eventer.Enable($('#Id'));
        Eventer.Enable($('#Department'));
        Eventer.Enable($('#Name'));
        Eventer.Enable($('#Mobile'));
        Eventer.Enable($('#Remark'));
        Eventer.Enable($('#Status'));
    },

    Disabled: function () {
        Eventer.Disable($('#Id'));
        Eventer.Disable($('#Department'));
        Eventer.Disable($('#Name'));
        Eventer.Disable($('#Mobile'));
        Eventer.Disable($('#Remark'));
        Eventer.Disable($('#Status'));
    }
};
