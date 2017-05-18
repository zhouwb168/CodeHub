$(function () {
    var grid = $('#Grid');
    grid.datagrid({ onClickRow: Events.OnClick });
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    grid.datagrid("getColumnOption", "ArriveDate").formatter = Events.ToDateTime;

    var tdOfButtons = $('#Buttons');
    tdOfButtons.find('[unique="Delete"]').bind('click', Events.OnDelete);
    tdOfButtons.find('[unique="Save"]').bind('click', Events.OnBeforeSave);

    $('#txtCard').bind('blur', Events.OnCardBlur);
    $('#aReadCard').bind('click', Events.OnReadCard);

    Events.Page();
});
var Events = {
    Table: 'Wood',

    OnBeforeSave: function () {
        if (Checker.Valid($('#txtCard'), '绿卡号是必须的') == false) return;

        if (Eventer.Get($('#txtCID')) == "0") {
            var cardTextBox = $('#txtCard');
            Events.ShowShortNumberOfCard(cardTextBox, Eventer.Get(cardTextBox), $('#txtCID'), '该绿卡还未进行编号', Events.OnSave);
        }
        else Events.OnSave();
    },

    OnCardBlur: function () {
        var cardTextBox = $(this);
        /* 焦点离开输入框时 */
        if (Eventer.Get(cardTextBox) == "") return;

        Eventer.Disable(cardTextBox);
    },

    ShowShortNumberOfCard: function (cardTextBox, cardNumber, hiddenTexBox, errorMsg, callBack) {
        /* 把电子卡号转变为编号显示 */
        var data = {
            service: 'WoodCard',
            method: 'GetEntityByField',
            args: ['CID', cardNumber, '=']
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.CardNumber != null) {
                Eventer.Set(cardTextBox, root.CardNumber);
                Eventer.Set(hiddenTexBox, root.CardNumber);
                if (callBack != null) callBack();
            }
            else $.messager.alert('提示消息', errorMsg, 'error');
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    ReadCard: function (cardTextBox) {
        /*  读卡 */
        Eventer.Enable(cardTextBox);
        cardTextBox.focus();
    },

    OnReadCard: function () {
        /* 读卡 */
        Controls.Clear();
        Buttons.Create();
        Events.ReadCard($('#txtCard'));
    },

    ToDateTime: function (value) {
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
        Eventer.Delete($('#txtUnique'), Events.Table, Events.Page, Buttons, Controls, Account, Eventer.Get($('#txtCard')));
    },

    OnSave: function () {
        if (Checker.Valid($('#txtCard'), '绿卡号是必须的') == false) return;
        if (Eventer.Get($('#txtCID')) == "0") {
            $.messager.alert('提示消息', '该绿卡号无效，请重新发卡', 'warning');
            return;
        }

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
        Eventer.Show($('#Buttons'), 'Save');

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
        Eventer.Set($('#txtCID'), 0);
        Eventer.Set($('#txtCard'));
    },

    Get: function () {
        return {
            Unique: Eventer.Get($('#txtUnique')),
            CardNumber: Eventer.Get($('#txtCard')),
            Operator: Account
        };
    },

    Set: function () {
        var row = $('#Grid').datagrid('getSelected');

        Eventer.Set($('#txtUnique'), row.Unique);
        Eventer.Set($('#txtCard'), row.CardNumber);
        Eventer.Set($('#txtCID'), row.CardNumber);
    },

    Enabled: function () {
    },

    Disabled: function () {
    }
};
