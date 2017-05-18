$(function () {
    var grid = $('#Grid');
    grid.datagrid({ onClickRow: Events.OnClick });
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    grid.datagrid("getColumnOption", "WeighTime").formatter = Events.ToDateTime;
    grid.datagrid("getColumnOption", "PackTime").formatter = Events.ToDateTime;

    var tdOfButtons = $('#Buttons');
    tdOfButtons.find('[unique="Delete"]').bind('click', Events.OnDelete);
    tdOfButtons.find('[unique="Save"]').bind('click', Events.OnSave);

    $('#txtCard').bind('blur', Events.OnBlur);
    $('#aReadCard').bind('click', Events.OnReadCard);
    Events.BindBlur();

    Events.Page();
});

var Events = {
    Table: 'Factory',

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
            else $.messager.alert('提示消息', errorMsg, 'warning');
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    OnBlur: function () {
        var cardTextBox = $(this);
        /* 焦点离开输入框时 */
        if (Eventer.Get(cardTextBox) == "") return;

        Eventer.Disable(cardTextBox);
        Events.ShowShortNumberOfCard(cardTextBox, Eventer.Get(cardTextBox), $('#txtCID'), '该红卡还未进行编号', Events.GetCardInfo);
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
        Eventer.Enable($('#txtBox'));
        Events.ReadCard($('#txtCard')); // 读卡
    },

    ReadCard: function (cardTextBox) {
        /*  读卡 */
        Eventer.Enable(cardTextBox);
        cardTextBox.focus();
    },

    GetCardInfo: function () {
        /* 读取电子卡的信息 */
        var data = {
            service: 'FullPound',
            method: 'GetEntityByFieldWithOperator',
            args: [Eventer.Get($('#txtCard'))]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                $.messager.alert('提示消息', root.Message, 'warning');

                return;
            }

            Eventer.Set($('#txtRecordID'), root.WoodID);
            Eventer.Set($('#txtLicense'), root.License);
            Eventer.Set($('#txtWeighTime'), moment(root.WeighTime).format('YYYY-MM-DD HH:mm:ss'));
            Eventer.Set($('#txtTree'), root.Tree);
            Eventer.Set($('#txtDriver'), root.Driver);
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    ToDateTime: function (value) {
        if (null == value || '' == value) return '';
        return moment(value).format('YYYY-MM-DD HH:mm');
    },

    Page: function () {
        var gridOptions = $('#Grid').datagrid('options');
        var pageSize = gridOptions.pageSize;
        Eventer.Enable($('#txtBox'));
        Events.GetGrid((gridOptions.pageNumber - 1) * pageSize + 1, pageSize);
    },

    GetGrid: function (start, length) {
        Eventer.Grid($('#Grid'), Events.Table, 'GetEntitiesByStartAndLengthWithOperator', [start, length, Account]);
    },

    OnClick: function () {
        Controls.Clear();
        Eventer.Click($('#Grid'), Buttons, Controls);
    },

    OnDelete: function () {
        Eventer.Delete($('#txtUnique'), Events.Table, Events.Page, Buttons, Controls, $('#txtRecordID'), Account, Eventer.Get($('#txtCID')));
    },

    OnSave: function () {
        if (Checker.Valid($('#txtCard'), '红卡号是必须的') == false) return;
        if (Checker.Valid($('#txtUnLoadPalce'), '卸货地点是必须的') == false) return
        if (Checker.Valid($('#txtUnLoadPeople'), '卸货人是必须的') == false) return;
        if (Checker.Valid($('#txtKey'), '密码是必须的') == false) return
        if (Checker.Valid($('#txtSampler'), '取样人是必须的') == false) return;
        //if (Checker.Valid($('#txtBox'), '送样箱号是必须的') == false) return;
        if (Eventer.Get($('#txtRecordID')) == "0") {
            $.messager.alert('提示消息', '该红卡登记的数据无效，请重新读卡', 'warning');
            return;
        }
        if (Eventer.Get($('#txtCID')) == "0") {
            $.messager.alert('提示消息', '该红卡号无效，请重新读卡', 'warning');
            return;
        }

        /* 检查评估值是否已填写 */
        var water = 0;
        var skin = 0;
        var scrap = 0;

        var arryInput = $('#tblAssess').find("input");
        var inputNum = arryInput.length;

        var tempValue = "";
        var tempGroup = "";

        for (var i = 0; i < inputNum; i++) {
            tempValue = arryInput.eq(i).val();
            tempValue = Events.Trim(tempValue);
            if (tempValue == "") continue;
            tempGroup = arryInput.eq(i).attr('group');
            switch (tempGroup) {
                case "Water":
                    {
                        water = parseFloat(tempValue);
                        break;
                    }
                case "Skin":
                    {
                        skin = parseFloat(tempValue);
                        break;
                    }
                case "Scrap":
                    {
                        scrap = parseFloat(tempValue);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        if (water <= 0) {
            $.messager.alert('提示消息', '请对木片含水率进行评估', 'warning');
            return;
        }
        if (skin <= 0) {
            $.messager.alert('提示消息', '请对树皮含量进行评估', 'warning');
            return;
        }
        if (scrap <= 0) {
            $.messager.alert('提示消息', '请对碎料含量进行评估', 'warning');
            return;
        }

        Eventer.Save($('#txtUnique'), Events.Table, Events.Page, Buttons, Controls, Eventer.Get($('#txtCID')));
    },

    OnFilled: function () {
        /* 目测平估数据填充后，自动屏蔽旧数据 */
        var objInput = document.all ? window.event.srcElement : event.target;
        var tempValue = objInput.value;
        tempValue = Events.Trim(tempValue);
        if (tempValue == "") return;

        if (!Events.IsDigit(tempValue)) {
            if (!Events.IsFloat(tempValue)) {
                $.messager.alert('提示消息', '提示，评估值必须是有效的数字', 'warning');
                objInput.value = "";
                return;
            }
        }

        if (parseFloat(tempValue) < 0 || parseFloat(tempValue) > 100) {
            $.messager.alert('提示消息', '提示，评估值必须在0～100之间', 'warning');
            objInput.value = "";
            return;
        }

        var parentObj = objInput.parentElement.parentElement;
        var arryInput = parentObj.getElementsByTagName("input");
        var inputNum = arryInput.length;
        for (var i = 0; i < inputNum; i++) {
            arryInput[i].value = "";
        }
        objInput.value = tempValue;
    },

    IsDigit: function (str) {
        /*  判断是否为整数 */
        var patrn = /^([0-9][0-9]*)$/;
        if (!patrn.exec(str)) return false
        return true
    },

    IsFloat: function (str) {
        /*  判断是否为2位小数的正浮点数 */
        var patrnl = /^[0-9][0-9]*(?:\.\d{0,2})?$/;
        if (!patrnl.exec(str)) return false

        return true
    },

    Trim: function (str) {
        /* 去掉两边空格  */
        return str.replace(/(^\s*)|(\s*$)/g, "");
    },

    BindBlur: function () {
        /*  绑定焦点离开事件  */
        var arryInput = $('#tblAssess').find("input");
        var inputNum = arryInput.length;
        for (var i = 0; i < inputNum; i++) {
            arryInput.eq(i).bind("blur", Events.OnFilled);
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
        Eventer.Set($('#txtCID'), 0);
        Eventer.Set($('#txtCard'));
        Eventer.Set($('#txtLicense'));
        Eventer.Set($('#txtWeighTime'));
        Eventer.Set($('#txtTree'));
        Eventer.Set($('#txtDriver'));
        Eventer.Set($('#txtUnLoadPalce'));
        Eventer.Set($('#txtUnLoadPeople'));
        Eventer.Set($('#txtKey'));
        Eventer.Set($('#txtOldKey'));
        Eventer.Set($('#txtSampler'));
        Eventer.Set($('#txtDeduc'));
        Eventer.Set($('#txtRemarkt'));
        Eventer.Set($('#txtBox'));

        var arryInput = $('#tblAssess').find("input");
        var inputNum = arryInput.length;
        for (var i = 0; i < inputNum; i++) {
            arryInput.eq(i).val('');
        }
    },

    Get: function () {
        /* 获取目测评估值 */
        var water = 0;
        var skin = 0;
        var scrap = 0;

        var arryInput = $('#tblAssess').find("input");
        var inputNum = arryInput.length;

        var tempValue = "";
        var tempGroup = "";

        for (var i = 0; i < inputNum; i++) {
            tempValue = arryInput.eq(i).val();
            tempValue = Events.Trim(tempValue);
            if (tempValue == "") continue;
            tempGroup = arryInput.eq(i).attr('group');
            switch (tempGroup) {
                case "Water":
                    {
                        water = parseFloat(tempValue);
                        break;
                    }
                case "Skin":
                    {
                        skin = parseFloat(tempValue);
                        break;
                    }
                case "Scrap":
                    {
                        scrap = parseFloat(tempValue);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        return {
            Unique: Eventer.Get($('#txtUnique')),
            WoodID: Eventer.Get($('#txtRecordID')),
            UnLoadPalce: Events.Trim(Eventer.Get($('#txtUnLoadPalce'))),
            UnLoadPeople: Events.Trim(Eventer.Get($('#txtUnLoadPeople'))),
            Key: Events.Trim(Eventer.Get($('#txtKey'))).toUpperCase(),
            OldKey: Eventer.Get($('#txtOldKey')) == "" ? "" : Events.Trim(Eventer.Get($('#txtOldKey'))).toUpperCase(),
            Sampler: Events.Trim(Eventer.Get($('#txtSampler'))),
            Water: water,
            Skin: skin,
            Scrap: scrap,
            Deduct: Eventer.Get($('#txtDeduc')),
            Remark: Eventer.Get($('#txtRemarkt')),
            WeighTime: Eventer.Get($('#txtWeighTime')),
            Box: Eventer.Get($('#txtBox')),
            Operator: Account
        };
    },

    Set: function () {
        var row = $('#Grid').datagrid('getSelected');

        Eventer.Set($('#txtUnique'), row.Unique);
        Eventer.Set($('#txtRecordID'), row.WoodID);
        Eventer.Set($('#txtCard'), row.RedCardNumber);
        Eventer.Set($('#txtCID'), row.RedCardNumber);
        Eventer.Set($('#txtLicense'), row.License);
        if (row.WeighTime != null) Eventer.Set($('#txtWeighTime'), moment(row.WeighTime).format('YYYY-MM-DD HH:mm:ss'));
        Eventer.Set($('#txtTree'), row.Tree);
        Eventer.Set($('#txtDriver'), row.Driver);
        Eventer.Set($('#txtUnLoadPalce'), row.UnLoadPalce);
        Eventer.Set($('#txtUnLoadPeople'), row.UnLoadPeople);
        Eventer.Set($('#txtKey'), row.Key);
        Eventer.Set($('#txtOldKey'), row.Key);//旧密码
        Eventer.Set($('#txtSampler'), row.Sampler);
        Eventer.Set($('#txtBox'), row.Box);//箱号
        Eventer.Disable($('#txtBox'));
        if (row.Deduct != null) Eventer.Set($('#txtDeduc'), row.Deduct);
        if (row.Remark != null) Eventer.Set($('#txtRemarkt'), row.Remark);

        /* 填充目测评估值 */
        var water = parseFloat(row.Water);
        var skin = parseFloat(row.Skin);
        var scrap = parseFloat(row.Scrap);

        var arryInput = $('#tblAssess').find("input");
        var inputNum = arryInput.length;

        var currentGroup = ""; // 当前组标识
        var nextGroup = ""; // 下一组标识
        var tempGroup = "";

        for (var i = 0; i < inputNum; i++) {
            tempGroup = arryInput.eq(i).attr('group');
            if (tempGroup != currentGroup) {
                /* 如果不属于当前组，则表示上一组已完成，换另一组 */
                currentGroup = nextGroup = tempGroup;
            }

            if (currentGroup != nextGroup) continue; // 如果当前组和下一组不同，说明当前组已填充好数据，只等待遍历完该组剩余的控件后，再对下一组填充

            switch (tempGroup) {
                case "Water":
                    {
                        if (water >= parseFloat(arryInput.eq(i).attr('min'))) {
                            /* 数据库表里的值大于等于该输入框的下限时，填充数据，并且把下一组标识变量的值变更 */
                            arryInput.eq(i).val(water);
                            nextGroup = "";
                        }
                        break;
                    }
                case "Skin":
                    {
                        if (skin >= parseFloat(arryInput.eq(i).attr('min'))) {
                            /* 数据库表里的值大于等于该输入框的下限时，填充数据，并且把下一组标识变量的值变更 */
                            arryInput.eq(i).val(skin);
                            nextGroup = "";
                        }
                        break;
                    }
                case "Scrap":
                    {
                        if (scrap >= parseFloat(arryInput.eq(i).attr('min'))) {
                            /* 数据库表里的值大于等于该输入框的下限时，填充数据，并且把下一组标识变量的值变更 */
                            arryInput.eq(i).val(scrap);
                            nextGroup = "";
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    },

    Enabled: function () {
        
    },

    Disabled: function () {
    }
};
