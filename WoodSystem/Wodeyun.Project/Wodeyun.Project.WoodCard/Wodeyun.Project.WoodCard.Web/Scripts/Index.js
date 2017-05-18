$(function () {
    var grid = $('#Grid');
    grid.datagrid({ onClickRow: Events.OnClick });
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化

    $('#Buttons').find('[unique="Create"]').bind('click', Events.OnCreate);
    $('#Buttons').find('[unique="Delete"]').bind('click', Events.OnDelete);
    $('#Buttons').find('[unique="Save"]').bind('click', Events.OnSave);
    $('#Buttons').find('[unique="Cancel"]').bind('click', Events.OnCancel);

    $('#txtCID').bind('blur', Events.OnBlur);
    $('#aReadCard').bind('click', Events.OnReadCard);

    Events.Page();
});

var Events = {
    Service: 'WoodCard',

    BlurInput: function (cardTextBox, callBack) {
        /* 焦点离开输入框时 */
        if (Eventer.Get(cardTextBox) == "") return;

        Eventer.Disable(cardTextBox);
        if (callBack != null) callBack();
    },

    OnBlur: function () {
        Events.BlurInput($(this), null); // 焦点离开
    },

    ReadCard: function (cardTextBox) {
        /*  读卡 */
        var textBox = document.getElementById("txtCID");
        textBox.disabled = false;
        textBox.focus();
    },

    OnReadCard: function () {
        Eventer.Set($('#txtCID'));
        Events.ReadCard($('txtCID')); // 读卡
    },

    Page: function () {
        var gridOptions = $('#Grid').datagrid('options');
        var pageSize = gridOptions.pageSize;

        Events.GetGrid((gridOptions.pageNumber - 1) * pageSize + 1, pageSize);
    },

    GetGrid: function (start, length) {
        Eventer.Grid($('#Grid'), Events.Service, 'GetEntitiesByStartAndLength', [start, length]);
    },

    OnClick: function () {
        Controls.Clear();
        Eventer.Click($('#Grid'), Buttons, Controls);
    },

    OnCreate: function () {
        Eventer.Create(Buttons, Controls);
    },

    OnDelete: function () {
        Eventer.Delete($('#Unique'), Events.Service, Events.Page, Buttons, Controls);
    },

    OnSave: function () {
        if (Checker.Valid($('#txtCID'), '电子卡是必须的') == false) return;
        if (Checker.Valid($('#txtCardNumber'), '新编号是必须的') == false) return;

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
        Eventer.Set($('#txtCID'));
        Eventer.Set($('#txtNumber'));
    },

    Get: function () {
        return {
            Unique: Eventer.Get($('#Unique')),
            CID: Eventer.Get($('#txtCID')),
            CardNumber: Eventer.Get($('#txtCardNumber')),
            Operator: Account
        };
    },

    Set: function () {
        var row = $('#Grid').datagrid('getSelected');

        Eventer.Set($('#Unique'), row.Unique);
        Eventer.Set($('#txtCID'), row.CID);
        Eventer.Set($('#txtCardNumber'), row.CardNumber);
    },

    Enabled: function () {
        Eventer.Enable($('#txtCardNumber'));
    },

    Disabled: function () {
        Eventer.Disable($('#txtCID'));
        Eventer.Disable($('#txtCardNumber'));
    }
};
