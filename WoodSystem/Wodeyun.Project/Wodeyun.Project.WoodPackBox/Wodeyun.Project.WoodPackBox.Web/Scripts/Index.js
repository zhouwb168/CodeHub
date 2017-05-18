$(function () {
    var grid = $('#Grid');
    grid.datagrid({ onClickRow: Events.OnClick });
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    grid.datagrid("getColumnOption", "SampleTime").formatter = Events.ToDateTime;
    grid.datagrid("getColumnOption", "PackTime").formatter = Events.ToDateTime;

    var tdOfButtons = $('#Buttons');
    tdOfButtons.find('[unique="Delete"]').bind('click', Events.OnDelete);
    tdOfButtons.find('[unique="Save"]').bind('click', Events.OnSave);
    
    Events.Page();
});
var Events = {
    Table: 'WoodPackBox',

    Trim: function (s) {
        return s.replace(/(^\s*)|(\s*$)/g, "");
    },

    ToDateTime: function (value) {
        if (value == null) return "";
        return moment(value).format('YYYY-MM-DD HH:mm');
    },

    Page: function () {
        var gridOptions = $('#Grid').datagrid('options');
        var pageSize = gridOptions.pageSize;

        Events.GetGrid((gridOptions.pageNumber - 1) * pageSize + 1, pageSize);
    },

    GetGrid: function (start, length) {
        Eventer.Grid($('#Grid'), Events.Table, 'GetEntitiesByStartAndLengthWithOperator', [start, length, Account]);
    },

    OnClick: function () {
        Eventer.Click($('#Grid'), Buttons, Controls);
    },

    OnDelete: function () {
        Eventer.Delete($('#txtUnique'), Events.Table, Events.Page, Buttons, Controls, $('#txtRecordID'), Account);
    },

    OnSave: function () {
        if (Checker.Valid($('#txtBox'), '送样箱号是必须的') == false) return;

        Eventer.Save($('#txtUnique'), Events.Table, Events.Page, Buttons, Controls);
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
        Eventer.Hide($('#Buttons'), 'Delete');
        Eventer.Hide($('#Buttons'), 'Save');

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
            Eventer.Hide($('#Buttons'), 'Delete');
        }

        this.State = 'None';
    },

    Clear: function () {
    }
};

var Controls = {
    Clear: function () {
        Eventer.Set($('#txtUnique'), 0);
        Eventer.Set($('#txtRecordID'), 0);
        Eventer.Set($('#txtSampleTime'));
        Eventer.Set($('#txtUnLoadPalce'));
        Eventer.Set($('#txtTree'));
        Eventer.Set($('#txtSampler'));
        Eventer.Set($('#txtKey'));
        Eventer.Set($('#txtBox'));
    },

    Get: function () {
        return {
            Unique: Eventer.Get($('#txtUnique')),
            WoodID: Eventer.Get($('#txtRecordID')),
            Box: Events.Trim(Eventer.Get($('#txtBox'))),
            Operator: Account
        };
    },

    Set: function () {
        var row = $('#Grid').datagrid('getSelected');

        if (row.Unique != null) Eventer.Set($('#txtUnique'), row.Unique);
        Eventer.Set($('#txtRecordID'), row.WoodID);
        Eventer.Set($('#txtSampleTime'), moment(row.SampleTime).format('YYYY-MM-DD HH:mm:ss'));
        Eventer.Set($('#txtUnLoadPalce'), row.UnLoadPalce);
        if (row.Tree != null) Eventer.Set($('#txtTree'), row.Tree);
        Eventer.Set($('#txtSampler'), row.Sampler);
        Eventer.Set($('#txtKey'), row.Key);
        if (row.Box != null) Eventer.Set($('#txtBox'), row.Box);
    },

    Enabled: function () {
    },

    Disabled: function () {
    }
};
