$(function () {
    var grid = $('#Grid');
    grid.datagrid({ onClickRow: Events.OnClick });
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    grid.datagrid("getColumnOption", "WeighTime").formatter = Events.ToDateTime;
    grid.datagrid("getColumnOption", "BackWeighTime").formatter = Events.ToDateTime;

    var tdOfButtons = $('#Buttons');
    tdOfButtons.find('[unique="Delete"]').bind('click', Events.OnDelete);
    tdOfButtons.find('[unique="Save"]').bind('click', Events.OnBeforeSave);
    tdOfButtons.find('[unique="Create"]').bind('click', Events.OnCreate);

    //$('#txtGreenCard').bind('blur', Events.OnGreenCardBlur);
    //$('#aReadCard').bind('click', Events.OnReadCard);
    //$('#txtRedCard').bind('blur', Events.OnRedCardBlur);
    //$('#aChangeCard').bind('click', Events.OnChangeCard);
    $('#txtCartChinese').bind('blur', Events.GetMatchInfo);
    $('#txtCartNumber').bind('blur', Events.GetMatchInfo);
    $('#queryFullPound').bind('click', Events.OnQueryFullPound);
    $('#SyncFullVolumeData').bind('click', Events.SyncFullVolumeData);

    $('#txtStartTime').datebox('setValue', moment().add('days', -15).format('YYYY-MM-DD'));
    $('#txtEndTime').datebox('setValue', moment().format('YYYY-MM-DD'));
    /*  以下是页面初始化 */
    Eventer.ComboBox($('#sltTree'), 'GsmTree', 'GetEntities', []);
    Events.Page();
});
var Events = {
    Table: 'DataMaintenance',

    Trim: function (s) {
        return s.replace(/(^\s*)|(\s*$)/g, "");
    },

    OnCreate: function () {
        Controls.Clear();
        Eventer.Hide($('#Buttons'), 'Delete');
        var grid = $('#Grid');
        grid.datagrid('clearSelections');
    },

    OnBeforeSave: function () {
        //if (Checker.Valid($('#txtRedCard'), '红卡号是必须的') == false) return;

        //if (Eventer.Get($('#txtRedCID')) == "0") {
        //    var cardTextBox = $('#txtRedCard');
        //    Events.ShowShortNumberOfCard(cardTextBox, Eventer.Get(cardTextBox), $('#txtRedCID'), '该红卡还未进行编号', Events.OnSave);
        //}
        //else
        Events.OnSave();
    },

    GetMatchInfo: function () {
        /*  根据车牌号匹配查询出最近一次匹配的记录 */
        var chinese = Eventer.Get($('#txtCartChinese'));
        var letter = Eventer.Get($('#txtCartNumber'));
        if (chinese == "" || letter == "") return;

        var license = chinese + letter;
        Events.GetWoodWhereComeFrom(license);
        Events.GetWhoSupplier(license);
    },

    GetWoodWhereComeFrom: function (license) {
        /*  根据车牌号到短信报备系统或木材收购系统获取相应的木材来源地 */
        var data = {
            service: Events.Table,
            method: 'GetEntityByFieldForMatch',
            args: [license]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Area != null) Eventer.Set($('#txtArea'), root.Area);
            else Eventer.Set($('#txtArea'));
        };

        Ajaxer.Ajax(Setter.Url, data, success);
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

    OnRedCardBlur: function () {
        var cardTextBox = $(this);
        /* 焦点离开输入框时 */
        if (Eventer.Get(cardTextBox) == "") return;

        //Eventer.Disable(cardTextBox);
    },

    OnGreenCardBlur: function () {
        var cardTextBox = $(this);
        /* 焦点离开输入框时 */
        if (Eventer.Get(cardTextBox) == "") return;

        //Eventer.Disable(cardTextBox);
        Events.ShowShortNumberOfCard(cardTextBox, Eventer.Get(cardTextBox), $('#txtGreenCID'), '该绿卡还未进行编号', Events.GetCardInfo);
    },

    OnReadCard: function () {
        Controls.Clear();
        Buttons.Create();
        Events.ReadCard($('#txtGreenCard')); // 读卡
    },

    OnChangeCard: function () {
        if (Eventer.Get($('#txtUnique')) != "0") {
            $.messager.alert('提示消息', '修改操作时，不允许换卡', 'warning');
            return;
        }

        Eventer.Set($('#txtRedCard'));
        Eventer.Set($('#txtRedCID'), 0);
        Events.ReadCard($('#txtRedCard')); // 换卡
    },

    ReadCard: function (cardTextBox) {
        /*  读卡 */
        Eventer.Enable(cardTextBox);
        cardTextBox.focus();
    },

    GetWhoSupplier: function (license) {
        /*  根据车牌号匹配查询出最近一次记录的木材来提供商的代码和送货员 */
        var data = {
            service: Events.Table,
            method: 'GetEntityByFieldForMatch',
            args: [license]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Driver != null) Eventer.Set($('#txtDriver'), root.Driver);
            else Eventer.Set($('#txtDriver'));
            if (root.Supplier != null) Eventer.Set($('#txtSupplier'), root.Supplier);
            else Eventer.Set($('#txtSupplier'));
            if (root.Tree != null) Eventer.Set($('#sltTree'), root.Tree, 'setText');
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    GetCardInfo: function () {
        /* 读取电子卡的信息 */
        var data = {
            service: 'Wood',
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
            Eventer.Set($('#txtArriveDate'), moment(root.ArriveDate).format('YYYY-MM-DD HH:mm:ss'));
            if (root.Area != null) Eventer.Set($('#txtArea'), root.Area);
            if (root.License != null) {
                var license = root.License; // 车牌号
                var chinese = license.substr(0, 1);
                var letter = license.substr(1, license.length - 1);
                Eventer.Set($('#txtCartChinese'), chinese);
                Eventer.Set($('#txtCartNumber'), letter);

                Events.GetWhoSupplier(license);
            }
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    ToDateTime: function (value) {
        if (value == null || value == '') return '';
        return moment(value).format('YYYY-MM-DD HH:mm:ss');
    },

    ToFixed: function (value) {
        if (value == null) return '';
        return value.toFixed(2);
    },

    Page: function () {
        var gridOptions = $('#Grid').datagrid('options');
        var pageSize = gridOptions.pageSize;

        Events.GetGrid((gridOptions.pageNumber - 1) * pageSize + 1, pageSize);
    },

    GetGrid: function (start, length) {
        var StartTime = Eventer.Get($('#txtStartTime'));
        var EndTime = Eventer.Get($('#txtEndTime'));
        var CarID = Eventer.Get($('#txtCarID'));
        Eventer.Grid($('#Grid'), Events.Table, 'GetEntitiesByStartAndLengthWithOperator', [start, length, Account, StartTime, EndTime, CarID]);
    },

    OnClick: function () {
        Eventer.Click($('#Grid'), Buttons, Controls);
    },

    OnDelete: function () {
        Eventer.Delete($('#txtUnique'), Events.Table, Events.Page, Buttons, Controls, $('#txtRecordID'), Account, Eventer.Get($('#txtGreenCID')), Eventer.Get($('#txtRedCID')));
    },

    OnQueryFullPound: function () {
        var grid = $('#Grid');
        grid.datagrid('options').pageNumber = 1;
        grid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });
        Events.Page();
        Controls.Clear();
    },

    OnSave: function () {
        //if (Checker.Valid($('#txtGreenCard'), '绿卡号是必须的') == false) return;
        if (Checker.Valid($('#txtCartChinese'), '请选择车牌省份') == false) return;
        if (Checker.Valid($('#txtCartNumber'), '请选择车牌号') == false) return;
        if (Checker.Valid($('#txtArea'), '木材来源地是必须的') == false) return;
        if (Checker.Valid($('#txtSupplier'), '卸车员是必须的') == false) return
        if (Checker.Valid($('#txtDriver'), '送货员是必须的') == false) return;
        if (Eventer.Get($('#sltTree')) == "") {
            $.messager.alert('提示消息', '树种是必须的', 'warning');
            return;
        }
        //if (Eventer.Get($('#txtRecordID')) == "0") {
        //    $.messager.alert('提示消息', '该绿卡登记的数据无效，请重新读卡', 'warning');
        //    return;
        //}
        //if (Eventer.Get($('#txtGreenCID')) == "0") {
        //    $.messager.alert('提示消息', '该绿卡号无效，请重新读卡', 'warning');
        //    return;
        //}
        //if (Eventer.Get($('#txtRedCID')) == "0") {
        //    $.messager.alert('提示消息', '该红卡号无效，请重新读卡', 'warning');
        //    return;
        //}
        //if (Eventer.Get($('#txtFullVolume')) == "") {
        //    if (!confirm("首磅体积还没有，是否继续保存？")) return;
        //}
        //zwb add 20141210
        var Supplier = Eventer.Get($('#txtSupplier')).toUpperCase();
        var CartNumber = Events.Trim(Eventer.Get($('#txtCartNumber'))).toUpperCase();

        if (CartNumber != "A62319" && CartNumber != "P02502" && CartNumber != "L12900") {
            if ((Supplier == "C2" || Supplier == "H1" || Supplier == "C5")
                && Eventer.Get($('#txtArea')) != "右江区") {
                $.messager.alert('提示消息', '卸车员：' + Eventer.Get($('#txtSupplier')).toUpperCase() + ' 来源地为"右江区",请修正。', 'warning');
                return;
            }
        }

        if ((CartNumber == "A62319" || CartNumber == "P02502" || CartNumber == "L12900")
            && Eventer.Get($('#txtArea')) != "田阳县") {
            $.messager.alert('提示消息', '车号：' + CartNumber + ' 来源地为"田阳县",请修正。', 'warning');
            return;
        }

        Eventer.Save($('#txtUnique'), Events.Table, Events.Page, Buttons, Controls, Eventer.Get($('#txtGreenCID')));
    },
    //计算净体积
    getJvolume: function (fullvolume, emptyvolume, rebatevolume, handvolume) {
        var res = 0;
        fullvolume = (fullvolume == null || fullvolume == "") ? 0 : fullvolume;
        emptyvolume = (emptyvolume == null || emptyvolume == "") ? 0 : emptyvolume;
        handvolume = (handvolume == null || handvolume == "") ? 0 : handvolume;
        rebatevolume = (rebatevolume == null || rebatevolume == "") ? 0 : rebatevolume;
        if (handvolume == 0) {
            res = parseFloat(fullvolume) - parseFloat(emptyvolume) - parseFloat(rebatevolume);
        } else {
            res = parseFloat(handvolume) - parseFloat(emptyvolume) - parseFloat(rebatevolume);
        }
        return res.toFixed(6);
    },
    //量方数据同步到工业互联网
    SyncFullVolumeData: function () {
        var StartTime = Eventer.Get($('#txtStartTime'));
        var EndTime = Eventer.Get($('#txtEndTime'));
        var CarID = Eventer.Get($('#txtCarID'));
        Eventer.OnSyncFullVolumeData(Events.Table, StartTime, EndTime, CarID);
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
        Eventer.Set($('#txtGreenCID'), 0);
        Eventer.Set($('#txtRedCID'), 0);
        Eventer.Set($('#txtGreenCard'));
        Eventer.Set($('#txtArriveDate'));
        Eventer.Set($('#txtCartChinese'));
        Eventer.Set($('#txtCartNumber'));
        Eventer.Set($('#txtArea'));
        Eventer.Set($('#txtSupplier'));
        Eventer.Set($('#txtDriver'));
        Eventer.Set($('#txtRedCard'));
        Eventer.Set($('#txtFullVolume'));
        Eventer.Set($('#txtLFUnique'));
        Eventer.Set($('#txtLFDate'));
        Eventer.Set($('#txtWeightTime'));
        Eventer.Set($('#txtWeighTime'));
        //回皮
        Eventer.Set($('#txtUniqueEmpty'), 0)
        Eventer.Set($('#txtBackWeighTime'));
        Eventer.Set($('#txtEmptyVolume'));
        Eventer.Set($('#txtDeduct'));
        Eventer.Set($('#txtDisc'));
        Eventer.Set($('#txtHandVolume'));
        Eventer.Set($('#txtRebateVolume'));
        Eventer.Set($('#txtLFUniqueEmpty'));
        Eventer.Set($('#txtLFDateEmpty'));
    },

    Get: function () {
        return {
            Unique: Eventer.Get($('#txtUnique')),
            WoodID: Eventer.Get($('#txtRecordID')),
            FullVolume: Eventer.Get($('#txtFullVolume')),
            License: Events.Trim(Eventer.Get($('#txtCartChinese'))) + Events.Trim(Eventer.Get($('#txtCartNumber'))).toUpperCase(),
            Area: Eventer.Get($('#txtArea')) == "田阳" ? "田阳县" : Eventer.Get($('#txtArea')),
            Tree: Eventer.Get($('#sltTree'), 'getText'),
            Driver: Eventer.Get($('#txtDriver')),
            Supplier: Eventer.Get($('#txtSupplier')),
            CardNumber: Eventer.Get($('#txtRedCard')),
            LFUnique: Eventer.Get($('#txtLFUnique')),
            LFDate: Eventer.Get($('#txtLFDate')),
            WeighTime: Eventer.Get($('#txtWeighTime')),
            WeightTime: Eventer.Get($('#txtWeightTime')),
            //回皮
            UniqueEmpty: Eventer.Get($('#txtUniqueEmpty')),
            BackWeighTime: Eventer.Get($('#txtBackWeighTime')),
            EmptyVolume: Eventer.Get($('#txtEmptyVolume')),
            HandVolume: Eventer.Get($('#txtHandVolume')),
            RebateVolume: Eventer.Get($('#txtRebateVolume')),
            LFUniqueEmpty: Eventer.Get($('#txtLFUniqueEmpty')),
            LFDateEmpty: Eventer.Get($('#txtLFDateEmpty')),
            Operator: Account
        };
    },

    Set: function () {
        var row = $('#Grid').datagrid('getSelected');
        Eventer.Set($('#txtUnique'), row.Unique);
        Eventer.Set($('#txtRecordID'), row.WoodID);
        Eventer.Set($('#txtGreenCID'), row.CreenCardNumber);
        Eventer.Set($('#txtRedCID'), row.CardNumber);
        Eventer.Set($('#txtGreenCard'), row.CreenCardNumber);
        Eventer.Set($('#txtLicense'), row.License);
        if (row.ArriveDate != null) {
            Eventer.Set($('#txtArriveDate'), Events.ToDateTime(row.ArriveDate));
        }
        if (row.FullVolume != null) Eventer.Set($('#txtFullVolume'), row.FullVolume);
        var license = row.License; // 车牌号
        var chinese = license.substr(0, 1);
        var letter = license.substr(1, license.length - 1);
        Eventer.Set($('#txtCartChinese'), chinese);
        Eventer.Set($('#txtCartNumber'), letter);
        Eventer.Set($('#txtArea'), row.Area);
        Eventer.Set($('#sltTree'), row.Tree, 'setText');
        Eventer.Set($('#txtDriver'), row.Driver);
        Eventer.Set($('#txtSupplier'), row.Supplier);
        Eventer.Set($('#txtRedCard'), row.CardNumber);
        Eventer.Set($('#txtLFUnique'), row.LFUnique);
        Eventer.Set($('#txtLFDate'), Events.ToDateTime(row.LFDate));
        Eventer.Set($('#txtWeighTime'), Events.ToDateTime(row.WeighTime));
        Eventer.Set($('#txtWeightTime'), Events.ToDateTime(row.WeighTime));
        //回皮
        Eventer.Set($('#txtBackWeighTime'), Events.ToDateTime(row.BackWeighTime));
        if (row.HandVolume != null) Eventer.Set($('#txtHandVolume'), row.HandVolume);
        if (row.RebateVolume != null) Eventer.Set($('#txtRebateVolume'), row.RebateVolume);
        if (row.EmptyVolume != null) Eventer.Set($('#txtEmptyVolume'), row.EmptyVolume);
        var discVolume = Events.getJvolume(row.FullVolume, row.EmptyVolume, row.RebateVolume, row.HandVolume);
        Eventer.Set($('#txtDisc'), discVolume);
        Eventer.Set($('#txtLFUniqueEmpty'), row.LFUniqueEmpty);
        Eventer.Set($('#txtLFDateEmpty'), Events.ToDateTime(row.LFDateEmpty));
        Eventer.Set($('#txtUniqueEmpty'), row.UniqueEmpty);
    },

    Enabled: function () {
    },

    Disabled: function () {
    }
};
