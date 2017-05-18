$(function () {
    var grid = $('#Grid');
    grid.datagrid({ onLoadSuccess: Events.OnLoadSuccess });
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    grid.datagrid("getColumnOption", "Col1").formatter = Events.ToTextForColumn1;
    grid.datagrid("getColumnOption", "Col2").formatter = Events.ToTextForColumn2;
    grid.datagrid("getColumnOption", "Col3").formatter = Events.ToTextForColumn3;
    grid.datagrid("getColumnOption", "Col4").formatter = Events.ToTextForColumn4;
    grid.datagrid("getColumnOption", "Col5").formatter = Events.ToTextForColumn5;
    grid.datagrid("getColumnOption", "Col6").formatter = Events.ToTextForColumn6;
    grid.datagrid("getColumnOption", "Col7").formatter = Events.ToTextForColumn7;
    grid.datagrid("getColumnOption", "Water").formatter = Events.ToFixed;
    grid.datagrid("getColumnOption", "Water").styler = Events.ToBgStyler;
    grid.datagrid("getColumnOption", "Skin").formatter = Events.ToFixed;
    grid.datagrid("getColumnOption", "Skin").styler = Events.ToBgStyler;
    grid.datagrid("getColumnOption", "Scrap").formatter = Events.ToFixed;
    grid.datagrid("getColumnOption", "Scrap").styler = Events.ToBgStyler;
    grid.datagrid("getColumnOption", "User").formatter = Events.ToText;
    grid.datagrid("getColumnOption", "User").styler = Events.ToBgStyler;
    grid.datagrid("getColumnOption", "Col8").formatter = Events.ToTextForColumn8;
    grid.datagrid("getColumnOption", "Col8").styler = Events.ToStyler;
    grid.datagrid("getColumnOption", "Col9").formatter = Events.ToTextForColumn9;
    grid.datagrid("getColumnOption", "Col9").styler = Events.ToStyler;
    grid.datagrid("getColumnOption", "Co21").formatter = Events.ToTextForColumn21;
    grid.datagrid("getColumnOption", "Co22").formatter = Events.ToTextForColumn22;
    grid.datagrid("getColumnOption", "Co23").formatter = Events.ToTextForColumn23;
    grid.datagrid("getColumnOption", "Co24").formatter = Events.ToTextForColumn24;
    grid.datagrid("getColumnOption", "Co25").formatter = Events.ToTextForColumn25;
    grid.datagrid("getColumnOption", "Co26").formatter = Events.ToDuctVolume;

    var itemsOfAlink = $('#tdDate').find('a');
    var itemCount = itemsOfAlink.length;
    for (var i = 0; i < itemCount; i++) {
        itemsOfAlink.eq(i).bind('click', { index: i }, Events.OnClick);
    }
    for (var i = 2; i < itemCount; i++) {
        itemsOfAlink.eq(i).find('font').html(moment().add('days', -i).format('DD') + '日');
    }

    $('#Date').datebox('setValue', moment().add('days', -6).format('YYYY-MM-DD'));

    $('#Query').bind('click', Events.OnQuery);
    $('#aConfirme').bind('click', Events.OnConfirme);
    $('#aBackConfirme').bind('click', Events.OnBackConfirme);//反审核
    $('#imgSave').bind('click', Events.OnSave);
    $('#imgCancel').bind('click', Events.CloseDialog);

    Events.BindBlur();

    Events.ReviewPermissionsbyAccount();

    Events.GetGrid();
});

var Events = {
    Table: 'WoodLaboratoryConfirme',

    ToTextForColumn24: function (value, row, rowIndex) {
        if (row.Less == null) return '';

        return row.Less.toFixed(2) + "%";
    },

    ToTextForColumn23: function (value, row, rowIndex) {
        if (row.Greater == null) return '';

        return row.Greater.toFixed(2) + "%";
    },

    ToTextForColumn22: function (value, row, rowIndex) {
        if (row.Bad == null) return '';

        return row.Bad.toFixed(2) + "%";
    },

    ToTextForColumn21: function (value, row, rowIndex) {
        if (row.IsConfirmed == null) return '否';

        var htmlValue = "";
        if (row.IsConfirmed == true) htmlValue = "<span style=\"color:red;\">是</span>";
        else htmlValue = "否";

        return htmlValue;
    },

    CloseDialog: function () {
        $('#dd').dialog('close');
    },

    EditInfo: function (woodID, Remark, Tree, jweight) {
        Controls.Clear();
        $('#Grid').datagrid('clearSelections');
        /* 把当前要编辑的行的信息填充到窗口表单 */
        var data = {
            service: 'WoodLaboratory',
            method: 'GetLaboratoryDataByWoodID',
            args: [woodID]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Unique != null) {
                Eventer.Set($('#txtWater'), root.Water.toFixed(2));
                Eventer.Set($('#txtRebateWater'), root.RebateWater.toFixed(2));
                Eventer.Set($('#txtSkin'), root.Skin.toFixed(2));
                Eventer.Set($('#txtRebateSkin'), root.RebateSkin.toFixed(2));
                Eventer.Set($('#txtScrap'), root.Scrap.toFixed(2));
                Eventer.Set($('#txtRebateScrap'), root.RebateScrap.toFixed(2));
                if (root.Bad != null) Eventer.Set($('#txtBad'), root.Bad.toFixed(2));
                if (root.Greater != null) Eventer.Set($('#txtGreater'), root.Greater.toFixed(2));
                if (root.RebateGreater != null) Eventer.Set($('#txtRebateGreater'), root.RebateGreater.toFixed(2));
                if (root.Less != null) Eventer.Set($('#txtLess'), root.Less.toFixed(2));
                if (root.DeductVolume != null) Eventer.Set($('#txtDeductVolume'), root.DeductVolume.toFixed(2));
                Eventer.Set($('#txtUnique'), root.Unique);
                Eventer.Set($('#txtRemarkOfWood'), Remark);
                Eventer.Set($('#txtTreeOfWood'), Tree);
                Eventer.Set($('#txtjweight'), jweight);

                var dlg = $('#dd');
                dlg.dialog({
                    title: '&nbsp;修改',
                    iconCls: 'icon-edit'
                });
                dlg.dialog('open');
            }
            else $.messager.alert('提示消息', '加载要编辑的数据失败，请重试', 'error');
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    ToTextForColumn25: function (value, row, rowIndex) {
        return "<span style=\"color:blue; cursor:pointer\" onclick=\"Events.EditInfo(" + row.WoodID + ",'" + row.Remark + "','" + row.Tree + "'," + row.jWeight + ");\">修改</span>";
    },

    ToTextForColumn9: function (value, row, rowIndex) {
        if (row.Remark == null) return '';

        var htmlValue = row.Remark.replace(/＃/g, '<br /><br />');
        htmlValue += "<br /><br />";
        return htmlValue;
    },

    ToStyler: function (value, row, rowIndex) {
        return 'background-color:#FFFF99;';
    },

    ToBgStyler: function (value, row, rowIndex) {
        var bgColor = 'background-color:#FFFFFF;';
        if (row.IsMain == "1") {
            bgColor = 'background-color:#CCFFFF;';
        }
        else if (row.IsLast == "1") {
            bgColor = 'background-color:#FFCC99;';
        }

        return bgColor;
    },

    ToTextForColumn1: function (value, row, rowIndex) {
        return row.WeighTime;
    },

    ToTextForColumn2: function (value, row, rowIndex) {
        return row.License;
    },

    ToTextForColumn3: function (value, row, rowIndex) {
        if (row.Key == null) return '';

        var htmlValue = row.Key.replace(/，/g, '<br />');
        return htmlValue;
    },

    ToTextForColumn4: function (value, row, rowIndex) {
        return row.Tree;
    },

    ToTextForColumn5: function (value, row, rowIndex) {
        return row.jWeight;
    },

    ToTextForColumn6: function (value, row, rowIndex) {
        return row.firstBangUser;
    },

    ToTextForColumn7: function (value, row, rowIndex) {
        if (row.CheckNumber == null) return '';

        return row.CheckNumber;
    },

    ToTextForColumn8: function (value, row, rowIndex) {
        if (row.Sampler == null) return '';

        var htmlValue = row.Sampler.replace(/，/g, '<br />');
        return htmlValue;
    },

    ToFixed: function (value, row, rowIndex) {
        if (value == null) return '';

        return value.toFixed(2) + "%";
    },

    ToDuctVolume: function (value, row, rowIndex) {
        if (row.DeductVolume == null) return '';
        return row.DeductVolume.toFixed(2) + "m³";;
    },

    ToText: function (value, row, rowIndex) {
        if (value == null) return '';

        var htmlValue = $.parseJSON(value).Name;
        if (row.IsMain == "0" && row.IsLast == "0") {
            htmlValue = "<span style=\"color:red;\">" + htmlValue + "</span>";
        }

        return htmlValue;
    },

    OnLoadSuccess: function (data) {
        if (data.rows.length > 0) setTimeout("Events.MergeCellsByField('Grid', 'Col9,Col8,Co25,Co24,Co23,Co22,Co21,Col7,Col6,Col5,Col4,Col3,Col2,Col1,Co20')", 1000);
    },

    MergeCellsByField: function (tableID, colList) {
        /* 根据字段列表合并jquery--easyui--datagrid中的相应列
         参数：tableID - 要操作的table的id；colList - 要合并的列的列表,用逗号分隔（例如："name,id,code"） */
        var ColArray = colList.split(",");
        var tTable = $('#' + tableID);
        var rowItems = tTable.datagrid("getRows");
        var TableRowCnts = rowItems.length;
        var tmpA;
        var tmpB;
        var PerTxt = "";
        var CurTxt = "";
        var alertStr = "";

        for (var j = ColArray.length - 1; j >= 0 ; j--) {
            /* 当循环至某新的列时，变量复位 */
            PerTxt = "";
            tmpA = 1;
            tmpB = 0;

            /* 从第一行（表头为第0行）开始循环，循环至行尾(溢出一位) */
            for (var i = 0; i <= TableRowCnts; i++) {
                if (i == TableRowCnts) CurTxt = "";
                else CurTxt = rowItems[i][ColArray[j]];
                if (PerTxt == CurTxt) tmpA += 1;
                else {
                    tTable.datagrid('mergeCells', {
                        index: i - tmpA,
                        field: ColArray[j],
                        rowspan: tmpA,
                        colspan: null
                    });
                    tmpA = 1;
                }
                PerTxt = CurTxt;
            }
        }
    },

    GetDate: function () {
        var itemsOfAlink = $('#tdDate').find('a');
        var itemCount = itemsOfAlink.length;
        for (var i = 0; i < itemCount; i++) {
            if (itemsOfAlink.eq(i).find('font').attr('color') == '#ff0000') return moment().add('days', -i).format('YYYY-MM-DD');
        }

        return Eventer.Get($('#Date'));
    },

    OnClick: function (event) {
        var date = moment().add('days', -event.data.index).format('YYYY-MM-DD');
        var itemsOfAlink = $('#tdDate').find('a');
        var itemCount = itemsOfAlink.length;
        for (var i = 0; i < itemCount; i++) {
            itemsOfAlink.eq(i).find('font').attr('color', (date == moment().add('days', -i).format('YYYY-MM-DD') ? 'red' : 'normal'));
        }

        Eventer.Set($('#txtKeyForSearch'));
        Eventer.Set($('#txtNumberForSearch'));
        Controls.Clear();
        Events.ResetPager();
        $('#Grid').datagrid('clearSelections'); // 清空已获得的选中项的标识值的列表，这里要调用一下，否则当前页面全选时，重新查询后标题栏的全选按钮还是选择状态

        Events.GetGrid();
    },

    ResetPager: function () {
        var grid = $('#Grid');
        grid.datagrid('options').pageNumber = 1;
        grid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });
    },

    OnQuery: function () {
        var itemsOfAlink = $('#tdDate').find('a');
        var itemCount = itemsOfAlink.length;
        for (var i = 0; i < itemCount; i++) {
            itemsOfAlink.eq(i).find('font').attr('color', 'normal');
        }

        Controls.Clear();
        Events.ResetPager();
        $('#Grid').datagrid('clearSelections');

        Events.GetGrid();
    },

    OnConfirme: function () {
        /* 批量审核 */
        var checkeds = $('#Grid').datagrid('getSelections');
        if (checkeds.length == 0) {
            $.messager.alert('提示消息', '请先选择要审核通过的记录。', 'warning');
            return;
        }

        var collection = [];
        for (var i = 0; i < checkeds.length; i++) {
            if (checkeds[i].LaboratoryID == null || checkeds[i].IsConfirmed) continue;
            var entity = {
                Unique: checkeds[i].LaboratoryID
            };
            collection.push(JSON.stringify(entity));
        }
        if (collection.length == 0) {
            $.messager.alert('提示消息', '您选择的记录无法进行审核。', 'warning');
            return;
        }
        var data = {
            service: Events.Table,
            method: 'BatchConfirme',
            args: [collection, parseInt(Account)]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                $.messager.alert('提示消息', root.Message, 'error');
                return;
            }
            $.messager.alert('提示消息', root.Message, 'info');
            $('#Grid').datagrid('clearSelections'); // 清空已获得的选中项的标识值的列表，这里要调用一下，否则当前页面全选时，批量审核成功后标题栏的全选按钮还是选择状态
            Events.Page();
            Controls.Clear();
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    OnBackConfirme: function () {
        /* 批量反审核 */
        var checkeds = $('#Grid').datagrid('getSelections');
        if (checkeds.length == 0) {
            $.messager.alert('提示消息', '请先选择需要反审核的记录', 'warning');
            return;
        }

        var collection = [];
        for (var i = 0; i < checkeds.length; i++) {
            if (checkeds[i].LaboratoryID == null || !checkeds[i].IsConfirmed) continue;
            var entity = {
                Unique: checkeds[i].LaboratoryID
            };
            collection.push(JSON.stringify(entity));
        }
        if (collection.length == 0) {
            $.messager.alert('提示消息', '您选择的记录无法进行反审核。', 'warning');
            return;
        }
        var data = {
            service: Events.Table,
            method: 'BatchBackConfirme',
            args: [collection, parseInt(Account)]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);
            if (root.Success == false) {
                $.messager.alert('提示消息', root.Message, 'error');
                return;
            }
            $.messager.alert('提示消息', root.Message, 'info');
            $('#Grid').datagrid('clearSelections'); // 清空已获得的选中项的标识值的列表，这里要调用一下，否则当前页面全选时，批量审核成功后标题栏的全选按钮还是选择状态
            Events.Page();
            Controls.Clear();
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    Page: function () {
        Events.GetGrid();
    },

    GetGrid: function () {
        var grid = $('#Grid');
        var gridOptons = grid.datagrid('options');
        var pageSize = gridOptons.pageSize;
        var rowStart = (gridOptons.pageNumber - 1) * pageSize + 1;

        var date = Events.GetDate();
        var key = Eventer.Get($('#txtKeyForSearch')).toUpperCase();
        var number = Eventer.Get($('#txtNumberForSearch'));
        var confirme = parseInt(Eventer.Get($('#sltConfirme'), null));

        Eventer.Grid(grid, Events.Table, 'GetEntitiesBySearchWithPaging', [date, rowStart, pageSize, key, number, confirme], null);
    },

    OnSave: function () {
        if (Eventer.Get($('#txtUnique')) == "0") {
            $.messager.alert('提示消息', '请先选择要修改的记录', 'warning');
            return;
        }

        if (Checker.Valid($('#txtWater'), '原始水份是必须的') == false) return;
        //if (Checker.Valid($('#txtRebateWater'), '折后水份是必须的') == false) return;
        if (Checker.Valid($('#txtSkin'), '原始树皮是必须的') == false) return;
        //if (Checker.Valid($('#txtRebateSkin'), '折后树皮是必须的') == false) return;
        if (Checker.Valid($('#txtScrap'), '原始碎料是必须的') == false) return;
        //if (Checker.Valid($('#txtRebateScrap'), '折后碎料是必须的') == false) return;

        Eventer.Save($('#txtUnique'), Events.Table, Events.Page, Buttons, Controls, Events.CloseDialog);
    },

    OnFilled: function () {
        /* 自动屏蔽无效数据 */
        var objInput = document.all ? window.event.srcElement : event.target;
        var tempValue = objInput.value;
        tempValue = Events.Trim(tempValue);
        if (tempValue == "") return;
        if (!Events.IsDigit(tempValue)) {
            if (!Events.IsFloat(tempValue)) {
                $.messager.alert('提示消息', '必须是有效的数字', 'warning');
                objInput.value = "";
                return;
            }
        }
        if (parseFloat(tempValue) < 0 || parseFloat(tempValue) > 100) {
            $.messager.alert('提示消息', '百分比例必须在0～100之间', 'warning');
            objInput.value = "";
            return;
        }
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
        var arryInput;
        var inputNum;
        arryInput = $('#tblIpuntsGroup').find("input");
        inputNum = arryInput.length;
        for (var i = 0; i < inputNum; i++) {
            arryInput.eq(i).bind("blur", Events.OnFilled);
        }
    },

    ReviewPermissionsbyAccount: function () {
        var data = {
            service: "WoodPrice",
            method: 'ReviewPermissionsbyAccount',
            args: [JSON.stringify({
                Account: Account,
                Role: Eventer.RoleEnum.化验报告反审核
            })]
        };

        var success = function (result) {
            var root = Ajaxer.GetRoot(result);
            if (root > 0) {
                $("#checkTool").show();
            }
            else {
                $("#checkTool").remove();
            }
        };
        Ajaxer.Ajax(Setter.Url, data, success);
    }
};

var Buttons = {
    State: 'None',

    Update: function () {
        Eventer.Show($('#Buttons'), 'Delete');

        this.State = 'Update';
    },

    Create: function () {
        Eventer.Hide($('#Buttons'), 'Delete');

        this.State = 'Create';
    },

    Delete: function () {
        Eventer.Hide($('#Buttons'), 'Delete');

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
        Eventer.Set($('#txtWater'));
        Eventer.Set($('#txtRebateWater'));
        Eventer.Set($('#txtSkin'));
        Eventer.Set($('#txtRebateSkin'));
        Eventer.Set($('#txtScrap'));
        Eventer.Set($('#txtRebateScrap'));
        Eventer.Set($('#txtBad'));
        Eventer.Set($('#txtGreater'));
        Eventer.Set($('#txtRebateGreater'));
        Eventer.Set($('#txtLess'));
        Eventer.Set($('#txtRemarkOfWood'));
        Eventer.Set($('#txtDeductVolume'));
    },

    Get: function () {
        return {
            Unique: Eventer.Get($('#txtUnique')),
            Water: Eventer.Get($('#txtWater')),
            RebateWater: Eventer.Get($('#txtRebateWater')),
            Skin: Eventer.Get($('#txtSkin')),
            RebateSkin: Eventer.Get($('#txtRebateSkin')),
            Scrap: Eventer.Get($('#txtScrap')),
            RebateScrap: Eventer.Get($('#txtRebateScrap')),
            Bad: Eventer.Get($('#txtBad')),
            Greater: Eventer.Get($('#txtGreater')),
            RebateGreater: Eventer.Get($('#txtRebateGreater')),
            Less: Eventer.Get($('#txtLess')),
            RemarkOfWood: Eventer.Get($('#txtRemarkOfWood')),
            TreeOfWood: Eventer.Get($('#txtTreeOfWood')),
            DeductVolume: Eventer.Get($('#txtDeductVolume')),//树皮碎料扣方
            jweight: Eventer.Get($('#txtjweight')),
            Operator: Account
        };
    },

    Set: function () {
    },

    Enabled: function () {
    },

    Disabled: function () {
    }
};
