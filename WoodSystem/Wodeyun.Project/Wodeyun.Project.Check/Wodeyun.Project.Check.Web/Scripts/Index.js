$(function () {
    var grid = $('#Grid');
    grid.datagrid({ loadFilter: Events.OnPage });
    grid.datagrid("getColumnOption", "GPS").formatter = Events.ToMeter;
    grid.datagrid("getColumnOption", "TimeOfStation").formatter = Events.ToDateTime;
    grid.datagrid("getColumnOption", "CheckDate").formatter = Events.ToDateTime;

    $('#Buttons').find('[unique="Save"]').bind('click', Events.OnSave);

    $('#txtGreenCard').bind('blur', Events.OnGreenCardBlur);
    $('#aCheckCard').bind('click', Events.OnCheckCard);

    Events.Page();
});

var Events = {
    Table: 'Check',

    OnGreenCardBlur: function () {
        var cardTextBox = $(this);
        /* 焦点离开输入框时 */
        if (Eventer.Get(cardTextBox) == "") return;

        Eventer.Disable(cardTextBox);
        Events.ShowShortNumberOfCard(cardTextBox, Eventer.Get(cardTextBox), $('#txtCID'), '该绿卡还未进行编号', Events.GetCardInfo);
    },

    ReadCard: function (cardTextBox) {
        /*  读卡 */
        Eventer.Enable(cardTextBox);
        cardTextBox.focus();
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

    OnCheckCard: function () {
            /* 验卡 */
        Controls.Clear();
        Events.ReadCard($('#txtGreenCard'));
        },

    GetCardInfo: function () {
        /* 读取电子卡的信息 */
        var data = {
            service: 'Barrier',
            method: 'GetEntityByFieldWithOperator',
            args: [Eventer.Get($('#txtGreenCard'))]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                $.messager.alert('提示消息', root.Message, 'warning');

                return;
            }

            Eventer.Set($('#txtRecordID'), root.Unique);
            Eventer.Set($('#txtLicense'), root.License);
            Eventer.Set($('#txtPlace'), root.Place);
            Eventer.Set($('#txtTimeOfStation'), moment(root.TimeOfStation).format('YYYY-MM-DD HH:mm:ss'));
            Eventer.Set($('#txtGPS'), root.GPS);
            Eventer.Set($('#txtArea'), root.Area);

            /* 计算并显示相差的天数 */
            var timeOfStation = moment(root.TimeOfStation).format('YYYY-MM-DD HH:mm:ss');
            var startTime = new Date(Date.parse(timeOfStation.replace(/-/g, "/"))).getTime();
            var endTime = (new Date()).getTime();
            var diffTime;
            var d = 0;
            var h;
            var m;
            var s;
            var strResult = "";
            if (startTime > endTime) return;
            diffTime = endTime - startTime;
            d = Math.floor(diffTime / (1000 * 60 * 60 * 24));
            h = Math.floor(diffTime / (1000 * 60 * 60)) - d * 24;
            m = Math.floor(diffTime / (1000 * 60)) - d * 24 * 60 - h * 60;
            s = Math.floor(diffTime / 1000) - d * 24 * 60 * 60 - h * 60 * 60 - m * 60;
            strResult = d + "天" + h + "小时" + m + "分钟" + s + "秒";
            Eventer.Set($('#txtDay'), strResult);
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    ToMeter: function (value) {
        var str = "";
        if (parseFloat(value) > 5000) str = "<span style=\"color:red;\">" + value + "米</span>";
        else str = value + "米";
        return str;
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

    OnPage: function (data) {
        Eventer.Page($('#Grid'), Events.Page);

        return data;
    },

    OnSave: function () {
        if (Checker.Valid($('#txtLicense'), '车牌号是必须的') == false) return;
        if (Checker.Valid($('#txtArea'), '来源地是必须的') == false) return;
        if (Checker.Valid($('#txtGreenCard'), '绿卡号是必须的') == false) return;
        if (Eventer.Get($('#txtCID')) == "0") {
            $.messager.alert('提示消息', '该绿卡号无效，请重新验卡', 'warning');
            return;
        }
        if (Eventer.Get($('#txtRecordID')) == "0") {
            $.messager.alert('提示消息', '该绿卡登记的数据无效，请重新验卡', 'warning');
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
        Eventer.Set($('#txtRecordID'), 0);
        Eventer.Set($('#txtLicense'));
        Eventer.Set($('#txtPlace'));
        Eventer.Set($('#txtTimeOfStation'));
        Eventer.Set($('#txtDay'));
        Eventer.Set($('#txtGPS'));
        Eventer.Set($('#txtArea'));
        Eventer.Set($('#txtGreenCard'));
    },

    Get: function () {
        return {
            Unique: Eventer.Get($('#txtUnique')),
            BarrierID: Eventer.Get($('#txtRecordID')),
            Operator: Account,
            CardNumber: Eventer.Get($('#txtGreenCard'))
        };
    },

    Set: function () {
        var row = $('#Grid').datagrid('getSelected');

        Eventer.Set($('#txtUnique'), row.Unique);
        Eventer.Set($('#txtCard'), row.CardNumber);
        var license = row.License; // 车牌号
        var chinese = license.substr(0, 1);
        var letter = license.substr(1, license.length - 1);
        Eventer.Set($('#txtCartChinese'), chinese);
        Eventer.Set($('#txtCartNumber'), letter);
        Eventer.Set($('#txtGPS'), row.GPS);
    },

    Enabled: function () {
    },

    Disabled: function () {
    }
};
