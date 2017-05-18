/* 以下变量用于控制三维量方 */
var lfObject = null;
var key = null;
var timeHandler = null;

$(function () {
    var grid = $('#Grid');
    grid.datagrid({ onClickRow: Events.OnClick });
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    grid.datagrid("getColumnOption", "WeighTime").formatter = Events.ToDateTime;

    var tdOfButtons = $('#Buttons');
    tdOfButtons.find('[unique="Delete"]').bind('click', Events.OnDelete);
    tdOfButtons.find('[unique="Save"]').bind('click', Events.OnBeforeSave);

    $('#txtGreenCard').bind('blur', Events.OnGreenCardBlur);
    $('#aReadCard').bind('click', Events.OnReadCard);
    $('#txtRedCard').bind('blur', Events.OnRedCardBlur);
    $('#aChangeCard').bind('click', Events.OnChangeCard);
    $('#txtCartChinese').bind('blur', Events.GetMatchInfo);
    $('#txtCartNumber').bind('blur', Events.GetMatchInfo);
    $('#queryFullPound').bind('click', Events.OnQueryFullPound);
    $('#aLiangFang').bind('click', Events.OnLiangFang);

    $('#txtStartTime').datebox('setValue', moment().add('years', 10).add('month', 6).add('days', -7).format('YYYY-MM-DD'));
    $('#txtEndTime').datebox('setValue', moment().add('years', 10).add('month', 6).format('YYYY-MM-DD'));
    /*  以下是页面初始化 */
    Eventer.ComboBox($('#sltTree'), 'GsmTree', 'GetEntities', []);
    Events.Page();
    //Events.InitLF();

    //绑定木材收购磅单
    var bangrid = $('#bangrid');
    bangrid.datagrid({ onClickRow: Events.bangOnClick });
    Eventer.Page(bangrid, Events.bangPage); // 注意！分页事件不能在onClickRow事件之前进行初始化
    bangrid.datagrid("getColumnOption", "WeighTime").formatter = Events.ToDateTime;
    bangrid.datagrid("getColumnOption", "FullVolume").formatter = Events.ToFixed;
    bangrid.datagrid("getColumnOption", "EmptyVolume").formatter = Events.ToFixed;
    bangrid.datagrid("getColumnOption", "jVolume").formatter = Events.ToJVolume;
    bangrid.datagrid("getColumnOption", "RebateVolume").formatter = Events.ToFixed;

    //地磅那边的电脑时间跟正常时间相差10年零六个月
    $('#txtStartDate').datebox('setValue', moment().add('years', 10).add('month', 6).format('YYYY-MM-DD'));
    $('#txtEndDate').datebox('setValue', moment().add('years', 10).add('month', 6).format('YYYY-MM-DD'));
    $('#Query').bind('click', Events.OnQuery);
    $('#Print').bind('click', Events.OnPrint);
    $('#arePrint').bind('click', Events.OnPrint);

    Events.bangPage();
});
var Events = {
    Table: 'FullPound',

    ShowDataOfLiangFang: function (volume) {
        window.clearInterval(timeHandler); // 释放定时器
        $('#spanLiangFang').html('');
        /* 显示量方数据 */
        Eventer.Set($('#txtFullVolume'), volume);
    },

    OnTiming: function () {
        /* 定时器 */
        Events.GetDataOfLF();

        timeHandler = window.setInterval("Events.GetDataOfLF();", 2000); // 两秒钟后重复执行
    },

    OnLiangFang: function () {
        $('#spanLiangFang').html('正在量方...');
        Eventer.Disable($('#aLiangFang'));
        var entity = {
            Command: 'Start',
            License: Events.Trim(Eventer.Get($('#txtCartChinese'))) + Events.Trim(Eventer.Get($('#txtCartNumber'))).toUpperCase()
        };
        var data = {
            service: 'Exchange.Single',
            method: 'Execute',
            args: ['Wodekeji.Device.TruckVolume.1.0', JSON.stringify(entity), 90]
        };
        var success = function (result) {
            $('#spanLiangFang').html('');
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
            }
            else {
                Eventer.Set($('#txtFullVolume'), root.Volume);
                Eventer.Set($('#txtLFUnique'), root.Unique);
                Eventer.Set($('#txtLFDate'), moment(root.Date).format('YYYY-MM-DD HH:mm:ss'));
            }
            Eventer.Enable($('#aLiangFang'));
        };

        Ajaxer.Ajax(Setter.Url, data, success);

        //Eventer.Set($('#txtFullVolume'));
        //key = new Date().toLocaleTimeString(); // 生成唯一键
        //Events.GetDataOfLF();
        //timeHandler = window.setTimeout("Events.OnTiming();", 60000); // 一分钟后执行
    },

    GetDataOfLF: function () {
        /* 获取量方数据 */
        var resultValue = lfObject.StartLF(key, false);
        if (isNaN(resultValue)) {
            if (resultValue.charAt(0) == 'E') {
                window.clearInterval(timeHandler); // 释放定时器
                $('#spanLiangFang').html('链接已断开，请点击“量方”按钮重新量方。' + resultValue);
            }
            else $('#spanLiangFang').html(resultValue);
        }
        else Events.ShowDataOfLiangFang(resultValue);
    },

    InitLF: function () {
        lfObject = new ActiveXObject("COMLF.LF");
        lfObject.init("172.16.145.179", 6178);
    },

    Trim: function (s) {
        return s.replace(/(^\s*)|(\s*$)/g, "");
    },

    ToJVolume: function (value, row, rowindex) {
        var jvolume = Events.getJVolume(row.FullVolume, row.EmptyVolume, row.RebateVolume);
        return jvolume;
    },

    //获取净体积
    getJVolume: function (FullVolume, EmptyVolume, RebateVolume) {
        FullVolume = (FullVolume == null || FullVolume == '') ? 0 : FullVolume;
        EmptyVolume = (EmptyVolume == null || EmptyVolume == '') ? 0 : EmptyVolume;
        RebateVolume = (RebateVolume == null || RebateVolume == '') ? 0 : RebateVolume;
        return (parseFloat(FullVolume).toFixed(2) - parseFloat(EmptyVolume).toFixed(2) - parseFloat(RebateVolume).toFixed(2)).toFixed(2);
    },

    OnBeforeSave: function () {
        if (Checker.Valid($('#txtRedCard'), '红卡号是必须的') == false) return;

        if (Eventer.Get($('#txtRedCID')) == "0") {
            var cardTextBox = $('#txtRedCard');
            Events.ShowShortNumberOfCard(cardTextBox, Eventer.Get(cardTextBox), $('#txtRedCID'), '该红卡还未进行编号', Events.OnSave);
        }
        else Events.OnSave();
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

        Eventer.Disable(cardTextBox);
    },

    OnGreenCardBlur: function () {
        var cardTextBox = $(this);
        /* 焦点离开输入框时 */
        if (Eventer.Get(cardTextBox) == "") return;

        Eventer.Disable(cardTextBox);
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

    //木材收购磅单
    bangPage: function () {
        var gridOptions = $('#bangrid').datagrid('options');
        var pageSize = gridOptions.pageSize;
        Events.bangGetGrid((gridOptions.pageNumber - 1) * pageSize + 1, pageSize);
    },

    bangGetGrid: function (start, length) {
        var startDate = Eventer.Get($('#txtStartDate'));
        var endDate = Eventer.Get($('#txtEndDate'));
        var License = Eventer.Get($('#txtLicense1'));
        Eventer.Grid($('#bangrid'), Events.Table, 'GetWoodBangPrintInfo', [start, length, Account, startDate, endDate, License]);
    },

    bangOnClick: function () {
        //Eventer.Click($('#bangrid'), Buttons, Controls);
    },

    OnQuery: function () {
        var bangrid = $('#bangrid');
        bangrid.datagrid('options').pageNumber = 1;
        bangrid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });

        Events.bangPage();
    },

    OnQueryFullPound: function () {
        var grid = $('#Grid');
        grid.datagrid('options').pageNumber = 1;
        grid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });
        Events.Page();
    },

    OnSave: function () {
        if (Checker.Valid($('#txtGreenCard'), '绿卡号是必须的') == false) return;
        if (Checker.Valid($('#txtCartChinese'), '请选择车牌省份') == false) return;
        if (Checker.Valid($('#txtCartNumber'), '请选择车牌号') == false) return;
        if (Checker.Valid($('#txtArea'), '木材来源地是必须的') == false) return;
        if (Checker.Valid($('#txtSupplier'), '卸车员是必须的') == false) return
        if (Checker.Valid($('#txtDriver'), '送货员是必须的') == false) return;
        if (Eventer.Get($('#sltTree')) == "") {
            $.messager.alert('提示消息', '树种是必须的', 'warning');
            return;
        }
        if (Eventer.Get($('#txtRecordID')) == "0") {
            $.messager.alert('提示消息', '该绿卡登记的数据无效，请重新读卡', 'warning');
            return;
        }
        if (Eventer.Get($('#txtGreenCID')) == "0") {
            $.messager.alert('提示消息', '该绿卡号无效，请重新读卡', 'warning');
            return;
        }
        if (Eventer.Get($('#txtRedCID')) == "0") {
            $.messager.alert('提示消息', '该红卡号无效，请重新读卡', 'warning');
            return;
        }
        if (Eventer.Get($('#txtFullVolume')) == "") {
            if (!confirm("首磅体积还没有，是否继续保存？")) return;
        }
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

    OnQuery: function () {
        var bangrid = $('#bangrid');
        bangrid.datagrid('options').pageNumber = 1;
        bangrid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });

        Events.bangPage();
    },

    OnPrint: function () {
        /* 批量打印 */
        var checkeds = $('#bangrid').datagrid('getSelections');
        var rowCounts = checkeds.length;
        if (rowCounts < 1) {
            $.messager.alert('提示消息', '请先选择要打印的结算单', 'warning');
            return;
        }
        var WeighTime = "", license = "",
            tree = "", fullvolume = 0, emptyvolume = 0, firstbanguser = "", supplier = "",
            driver = "", jVolume = 0, rebatevolume = 0;
        for (var i = 0; i < rowCounts; i++) {
            if (i == 0) {
                WeighTime = Events.ToDateTime(checkeds[i].WeighTime);
                license = checkeds[i].License;
                tree = checkeds[i].Tree;
                fullvolume = Events.ToFixed(checkeds[i].FullVolume);
                emptyvolume = Events.ToFixed(checkeds[i].EmptyVolume);
                //firstbanguser = checkeds[i].firstBangUser;
                supplier = checkeds[i].Supplier;
                driver = checkeds[i].Driver;
                jVolume = Events.getJVolume(checkeds[i].FullVolume, checkeds[i].EmptyVolume, checkeds[i].RebateVolume);
                rebatevolume = Events.ToFixed(checkeds[i].RebateVolume);
            } else {
                Bang_Time += "_" + Events.ToDateTime(checkeds[i].Bang_Time);
                license += "_" + checkeds[i].License;
                tree += "_" + checkeds[i].Tree;
                fullvolume += "_" + Events.ToFixed(checkeds[i].FullVolume);
                emptyvolume += "_" + Events.ToFixed(checkeds[i].EmptyVolume);
                //firstbanguser = "_" + checkeds[i].firstBangUser;
                supplier += "_" + checkeds[i].Supplier;
                driver += "_" + checkeds[i].Driver;
                jVolume += "_" + Events.getJVolume(checkeds[i].FullVolume, checkeds[i].EmptyVolume, checkeds[i].RebateVolume);
                rebatevolume += "_" + Events.ToFixed(checkeds[i].RebateVolume);
            }
        }
        var printForm = document.createElement("form");
        document.body.appendChild(printForm);
        printForm.method = "get";
        printForm.action = "backPrint.html";
        printForm.target = "_blank";
        var txtBox = document.createElement("input");
        txtBox.id = "params";
        txtBox.name = "params";
        txtBox.type = "text";
        txtBox.value = escape(WeighTime + "|" + license + "|" + tree + "|" + fullvolume + "|" + emptyvolume + "|" + supplier + "|" + driver + "|" + jVolume + "|" + this.id + "|" + rebatevolume);
        printForm.appendChild(txtBox);
        printForm.submit();
        document.body.removeChild(printForm);
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
            WeightTime: Eventer.Get($('#txtWeightTime')),
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
        Eventer.Set($('#txtArriveDate'), moment(row.ArriveDate).format('YYYY-MM-DD HH:mm:ss'));
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
        Eventer.Set($('#txtLFDate'), moment(row.LFDate).format('YYYY-MM-DD HH:mm:ss'));
        Eventer.Set($('#txtWeightTime'), moment(row.WeighTime).format('YYYY-MM-DD HH:mm:ss'));
    },

    Enabled: function () {
    },

    Disabled: function () {
    }
};
