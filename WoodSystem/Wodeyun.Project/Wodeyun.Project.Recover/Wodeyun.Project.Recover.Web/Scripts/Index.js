$(function () {
    var grid = $('#Grid');
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    grid.datagrid("getColumnOption", "RecoverTime").formatter = Events.ToDateTime;

    $('#Buttons').find('[unique="Save"]').bind('click', Events.OnSave);

    $('#txtCardNumber').bind('blur', Events.OnCardBlur);
    $('#aRecoverCard').bind('click', Events.OnRecoverCard);

    Events.Page();
});
var Events = {
    Table: 'Recover',

    ReadCard: function (cardTextBox) {
        /*  读卡 */
        Eventer.Enable(cardTextBox);
        cardTextBox.focus();
    },

    OnCardBlur: function () {
        var cardTextBox = $(this);
        /* 焦点离开输入框时 */
        if (Eventer.Get(cardTextBox) == "") return;

        Eventer.Disable(cardTextBox);
        Events.ShowShortNumberOfCard(cardTextBox, Eventer.Get(cardTextBox), $('#txtCID'), '该绿卡还未进行编号', Events.GetCardInfo);
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

    OnRecoverCard: function () {
        /* 收卡 */
        Controls.Clear();
        Events.ReadCard($('#txtCardNumber'));
    },

    GetCardInfo: function () {
        /* 读取电子卡的信息 */
        var data = {
            service: 'EmptyPound',
            method: 'GetEntityByFieldWithOperator',
            args: [Eventer.Get($('#txtCardNumber'))]
        };
        var success = function (result) {

            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                $.messager.alert('提示消息', root.Message, 'warning');

                return;
            }

            Eventer.Set($('#txtRecordID'), root.WoodID);
            Eventer.Set($('#txtLicense'), root.License);
            Eventer.Set($('#txtBackWeighTime'), moment(root.BackWeighTime).format('YYYY-MM-DD HH:mm:ss'));
        };

        Ajaxer.Ajax(Setter.Url, data, success);
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

    OnSave: function () {
        if (Checker.Valid($('#txtCardNumber'), '绿卡号是必须的') == false) return;
        if (Eventer.Get($('#txtCID')) == "0") {
            $.messager.alert('提示消息', '该绿卡号无效，请重新收卡', 'warning');
            return;
        }
        if (Eventer.Get($('#txtRecordID')) == "0") {
            $.messager.alert('提示消息', '该绿卡登记的数据无效，请重新收卡', 'warning');
            return;
        }

        Eventer.Save($('#txtUnique'), Events.Table, Events.Page, Buttons, Controls, Eventer.Get($('#txtCID')));
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
        Eventer.Set($('#txtRecordID'), 0);
        Eventer.Set($('#txtCID'), 0);
        Eventer.Set($('#txtCardNumber'));
        Eventer.Set($('#txtLicense'));
        Eventer.Set($('#txtBackWeighTime'));
    },

    Get: function () {
        return {
            Unique: Eventer.Get($('#txtUnique')),
            WoodID: Eventer.Get($('#txtRecordID')),
            Operator: Account
        };
    },

    Set: function () {
        var row = $('#Grid').datagrid('getSelected');

        Eventer.Set($('#txtUnique'), row.Unique);
        Eventer.Set($('#txtCard'), row.CardNumber);
        var license = row.License; //车牌号
        var chinese = license.substr(0, 1);
        var letter = license.substr(1, license.length - 1);
        Eventer.Set($('#txtCartChinese'), chinese);
        Eventer.Set($('#txtCartNumber'), letter);
        Eventer.Set($('#txtOrigin'), row.Origin);
    },

    Enabled: function () {
    },

    Disabled: function () {
    }
};
