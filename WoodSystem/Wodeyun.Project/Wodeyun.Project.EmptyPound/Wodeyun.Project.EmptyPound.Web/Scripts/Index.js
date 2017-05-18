/* 以下变量用于控制三维量方 */
var lfObject = null;
var key = null;
var timeHandler = null;

$(function () {
    var grid = $('#Grid');
    grid.datagrid({ onClickRow: Events.OnClick });
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    grid.datagrid("getColumnOption", "BackWeighTime").formatter = Events.ToDateTime;
    grid.datagrid("getColumnOption", "JVolume").formatter = Events.ToJVolume;

    var tdOfButtons = $('#Buttons');
    tdOfButtons.find('[unique="Delete"]').bind('click', Events.OnDelete);
    tdOfButtons.find('[unique="Save"]').bind('click', Events.OnBeforeSave);

    $('#txtStartTime').datebox('setValue', moment().add('years', 10).add('month', 6).add('days', -7).format('YYYY-MM-DD'));
    $('#txtEndTime').datebox('setValue', moment().add('years', 10).add('month', 6).format('YYYY-MM-DD'));

    $('#txtRedCard').bind('blur', Events.OnRedCardBlur);
    $('#aReadCard').bind('click', Events.OnReadCard);
    $('#txtGreenCard').bind('blur', Events.OnGreenCardBlur);
    $('#aChangeCard').bind('click', Events.OnChangeCard);

    $('#aLiangFang').bind('click', Events.OnLiangFang);
    $('#query').bind('click', Events.OnQuery);

    /*  以下是页面初始化 */
    Events.Page();
    //Events.InitLF();
});
var Events = {
    Table: 'EmptyPound',

    ShowDataOfLiangFang: function (volume) {
        window.clearInterval(timeHandler); // 释放定时器
        $('#spanLiangFang').html('');
        /* 显示量方数据 */
        Eventer.Set($('#txtEmptyVolume'), volume);
        var fullVolume = Eventer.Get($('#txtFullVolume'));
        if (fullVolume == "") return;

        var discVolume = parseFloat(fullVolume) - parseFloat(volume);
        Eventer.Set($('#txtDisc'), discVolume);
    },

    OnTiming: function () {
        /* 定时器 */
        Events.GetDataOfLF();

        timeHandler = window.setInterval("Events.GetDataOfLF();", 2000); // 两秒钟后重复执行
    },

    OnQuery: function () {
        var grid = $('#Grid');
        grid.datagrid('options').pageNumber = 1;
        grid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });

        Events.Page();
    },

    OnLiangFang: function () {
        $('#spanLiangFang').html('正在量方...');
        Eventer.Disable($('#aLiangFang'));
        var entity = {
            Command: 'Start',
            License: Eventer.Get($('#txtLicense'))
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
                Eventer.Set($('#txtEmptyVolume'), root.Volume);
                Eventer.Set($('#txtLFUnique'), root.Unique);
                Eventer.Set($('#txtLFDate'), moment(root.Date).format('YYYY-MM-DD HH:mm:ss'));
                //zwb add 
                if (root.Volume != null && Eventer.Get($('#txtLicense')) != "" && root.Volume != "") {
                    Events.getNetVolumeforLed();
                    Events.getAvgloume(root.Volume);
                }
            }
            Eventer.Enable($('#aLiangFang'));
        };

        Ajaxer.Ajax(Setter.Url, data, success);

        //Eventer.Set($('#txtEmptyVolume'));
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

    OnBeforeSave: function () {
        if (Checker.Valid($('#txtGreenCard'), '绿卡号是必须的') == false) return;

        if (Eventer.Get($('#txtGreenCID')) == "0") {
            var cardTextBox = $('#txtGreenCard');
            Events.ShowShortNumberOfCard(cardTextBox, Eventer.Get(cardTextBox), $('#txtGreenCID'), '该绿卡还未进行编号', Events.OnSave);
        }
        else Events.OnSave();
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
        Events.ShowShortNumberOfCard(cardTextBox, Eventer.Get(cardTextBox), $('#txtRedCID'), '该红卡还未进行编号', Events.GetCardInfo);
    },

    OnGreenCardBlur: function () {
        var cardTextBox = $(this);
        /* 焦点离开输入框时 */
        if (Eventer.Get(cardTextBox) == "") return;

        Eventer.Disable(cardTextBox);
    },

    BlurInput: function (cardTextBox, callBack) {
        /* 焦点离开输入框时 */
        if (Eventer.Get(cardTextBox) == "") return;

        Eventer.Disable(cardTextBox);
        if (callBack != null) callBack();
    },

    OnReadCard: function () {
        Controls.Clear();
        Buttons.Create();
        Events.ReadCard($('#txtRedCard')); // 读卡
    },

    OnChangeCard: function () {
        if (Eventer.Get($('#txtUnique')) != "0") {
            $.messager.alert('提示消息', '修改操作时，不允许换卡', 'warning');
            return;
        }

        Eventer.Set($('#txtGreenCard'));
        Eventer.Set($('#txtGreenCID'), 0);
        Events.ReadCard($('#txtGreenCard')); // 换卡

    },

    ReadCard: function (cardTextBox) {
        /*  读卡 */
        Eventer.Enable(cardTextBox);
        cardTextBox.focus();
    },

    GetCardInfo: function () {
        /* 读取电子卡的信息 */
        var data = {
            service: 'Factory',
            method: 'GetEntityByFieldWithOperator',
            args: [Eventer.Get($('#txtRedCard'))]
        };
        var success = function (result) {

            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                $.messager.alert('提示消息', root.Message, 'warning');

                return;
            }

            Eventer.Set($('#txtRecordID'), root.WoodID);
            Eventer.Set($('#txtLicense'), root.License);
            if (root.FullVolume != null) Eventer.Set($('#txtFullVolume'), root.FullVolume);
            Eventer.Set($('#txtSampleTime'), moment(root.SampleTime).format('YYYY-MM-DD HH:mm:ss'));
            if (root.Deduct != null) $('#txtDeduct').html(root.Deduct.replace(/＃/g, '<br />'));
            Eventer.Set($('#txtCreenCardNumber'), root.GreenCard);
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    ToDateTime: function (value) {
        return moment(value).format('YYYY-MM-DD HH:mm');
    },

    ToJVolume: function (value, row, rowindex) {
        if (row.FullVolume != null && row.EmptyVolume != null) {
            var discVolume = Events.getJvolume(row.FullVolume, row.EmptyVolume, row.RebateVolume, row.HandVolume);
            return discVolume;
        }
        return '';
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
    //计算净体积
    getJvolume: function (fullvolume, emptyvolume, rebatevolume, handvolume) {
        var res = 0;
        try {
            fullvolume = (fullvolume == null || fullvolume == "") ? 0 : fullvolume;
            emptyvolume = (emptyvolume == null || emptyvolume == "") ? 0 : emptyvolume;
            handvolume = (handvolume == null || handvolume == "") ? 0 : handvolume;
            rebatevolume = (rebatevolume == null || rebatevolume == "") ? 0 : rebatevolume;
            if (handvolume == 0) {
                res = parseFloat(Events.ToMathFixed(fullvolume, 2)) - parseFloat(Events.ToMathFixed(emptyvolume, 2)) - parseFloat(Events.ToMathFixed(rebatevolume, 2));
            } else {
                res = parseFloat(Events.ToMathFixed(handvolume, 2)) - parseFloat(Events.ToMathFixed(emptyvolume, 2)) - parseFloat(Events.ToMathFixed(rebatevolume, 2));
            }
        } catch (e) {
            alert(e.message.toString());
        }
        return res.toFixed(2);
    },

    ToMathFixed: function (value, fractionDigits) {
        with (Math) {
            return round(value * pow(10, fractionDigits)) / pow(10, fractionDigits);
        }
    },

    OnSave: function () {
        if (Checker.Valid($('#txtRedCard'), '红卡号是必须的') == false) return;
        //if (Checker.Valid($('#txtHandVolume'), '人工量方是必须的！') == false) return
        if (Checker.Valid($('#txtGreenCard'), '绿卡号是必须的') == false) return;
        if (Eventer.Get($('#txtGreenCID')) == "0") {
            $.messager.alert('提示消息', '该绿卡号无效，请重新读卡', 'warning');
            return;
        }
        if (Eventer.Get($('#txtRedCID')) == "0") {
            $.messager.alert('提示消息', '该红卡号无效，请重新读卡', 'warning');
            return;
        }
        if (Eventer.Get($('#txtRecordID')) == "0") {
            $.messager.alert('提示消息', '该红卡登记的数据无效，请重新读卡', 'warning');
            return;
        }
        if (Eventer.Get($('#txtGreenCard')) != Eventer.Get($('#txtCreenCardNumber'))) {
            $.messager.alert('提示消息', '不是这张绿卡，请核对绿卡号码', 'warning');
            return;
        }
        if (Eventer.Get($('#txtEmptyVolume')) == "") {
            if (!confirm("回皮体积还没有，是否继续保存？")) return;
        }

        Eventer.Save($('#txtUnique'), Events.Table, Events.Page, Buttons, Controls, Eventer.Get($('#txtGreenCID')), Eventer.Get($('#txtRedCID')));

        $('#spanLiangFang').html('');
    },
    //获取平均体积
    getAvgloume: function (emptyvolume) {
        var entity = {
            License: Eventer.Get($('#txtLicense'))
        };
        var data = {
            service: Events.Table,
            method: 'getAvgVolume',
            args: [JSON.stringify(entity)]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);
            if (root != null) {
                try {
                    emptyvolume = emptyvolume == null || emptyvolume == "" ? 0 : emptyvolume;
                    if (root.AvgEmptyVolume != null && root.AvgEmptyVolume != "") {
                        if (parseFloat(emptyvolume) - parseFloat(root.AvgEmptyVolume) > 3) {
                            $('#spanLiangFang').html('本次回皮超过平均回皮(' + root.AvgEmptyVolume.toFixed(2) + ')3方，访问客户是否重新测量。');
                        }
                        else if (parseFloat(root.AvgEmptyVolume) - parseFloat(emptyvolume) > 3) {
                            $('#spanLiangFang').html('<span style="color:green;">本次回皮少于平均回皮(' + root.AvgEmptyVolume.toFixed(2) + ')超过3方，请注意是否有做假行为。</span>');
                        }
                        else
                            $('#spanLiangFang').html('');
                    }
                } catch (e) {
                }
            }
        };
        Ajaxer.Ajax(Setter.Url, data, success);
    },

    //获取净体积
    getNetVolumeforLed: function () {
        try {
            var netvolume = Events.getJvolume(Eventer.Get($('#txtFullVolume')), Eventer.Get($('#txtEmptyVolume')), Eventer.Get($('#txtRebateVolume')), Eventer.Get($('#txtHandVolume')));
            Eventer.Set($('#txtDisc'), netvolume);
        } catch (e) {
        }
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
        Eventer.Set($('#txtRedCard'));
        Eventer.Set($('#txtLicense'));
        Eventer.Set($('#txtSampleTime'));
        Eventer.Set($('#txtDeduct'));
        Eventer.Set($('#txtFullVolume'));
        Eventer.Set($('#txtEmptyVolume'));
        Eventer.Set($('#txtDisc'));
        Eventer.Set($('#txtHandVolume'));
        Eventer.Set($('#txtGreenCard'));
        Eventer.Set($('#txtCreenCardNumber'));
        Eventer.Set($('#txtLFUnique'));
        Eventer.Set($('#txtLFDate'));
        Eventer.Set($('#txtRebateVolume'));
        Eventer.Set($('#txtBackWeighTime'));
        Eventer.Set($('#txtBangID'));
        Eventer.Set($('#txtBangCID'));
    },

    Get: function () {
        return {
            Unique: Eventer.Get($('#txtUnique')),
            WoodID: Eventer.Get($('#txtRecordID')),
            EmptyVolume: Eventer.Get($('#txtEmptyVolume')),
            HandVolume: Eventer.Get($('#txtHandVolume')),
            LFUnique: Eventer.Get($('#txtLFUnique')),
            LFDate: Eventer.Get($('#txtLFDate')),
            RebateVolume: Eventer.Get($('#txtRebateVolume')),
            BackWeighTime: Eventer.Get($('#txtBackWeighTime')),
            JVolume: Events.getJvolume(Eventer.Get($('#txtFullVolume')), Eventer.Get($('#txtEmptyVolume')), Eventer.Get($('#txtRebateVolume')), Eventer.Get($('#txtHandVolume'))),
            Operator: Account,
            BangID: Eventer.Get($('#txtBangID')),
            BangCID: Eventer.Get($('#txtBangCID'))
        };
    },

    Set: function () {
        var row = $('#Grid').datagrid('getSelected');

        Eventer.Set($('#txtUnique'), row.Unique);
        Eventer.Set($('#txtRecordID'), row.WoodID);
        Eventer.Set($('#txtRedCard'), row.RedCard);
        Eventer.Set($('#txtRedCID'), row.RedCard);
        if (row.FullVolume != null) Eventer.Set($('#txtFullVolume'), row.FullVolume);
        Eventer.Set($('#txtLicense'), row.License);
        Eventer.Set($('#txtSampleTime'), moment(row.SampleTime).format('YYYY-MM-DD HH:mm:ss'));
        if (row.Deduct != null) $('#txtDeduct').html(row.Deduct.replace(/＃/g, '<br />'));
        if (row.EmptyVolume != null) {
            Eventer.Set($('#txtEmptyVolume'), row.EmptyVolume);
            if (row.FullVolume != null) {
                //var discVolume = parseFloat(row.FullVolume) - parseFloat(row.EmptyVolume) - parseFloat(row.RebateVolume == null ? 0 : row.RebateVolume);
                var discVolume = Events.getJvolume(row.FullVolume, row.EmptyVolume, row.RebateVolume, row.HandVolume);
                Eventer.Set($('#txtDisc'), discVolume);
            }
        }
        if (row.HandVolume != null) Eventer.Set($('#txtHandVolume'), row.HandVolume);
        if (row.RebateVolume != null) Eventer.Set($('#txtRebateVolume'), row.RebateVolume);
        if (row.BackWeighTime != null) Eventer.Set($('#txtBackWeighTime'), moment(row.BackWeighTime).format('YYYY-MM-DD HH:mm:ss'));
        Eventer.Set($('#txtGreenCard'), row.GreenCard);
        Eventer.Set($('#txtGreenCID'), row.GreenCard);
        Eventer.Set($('#txtCreenCardNumber'), row.GreenCard);
        Eventer.Set($('#txtLFUnique'), row.LFUnique);
        Eventer.Set($('#txtLFDate'), moment(row.LFDate).format('YYYY-MM-DD HH:mm:ss'));
        Eventer.Set($('#txtBangID'), row.BangID);
        Eventer.Set($('#txtBangCID'), row.bangCid);
    },

    Enabled: function () {
    },

    Disabled: function () {
    }
};
