$(function () {
    $('#Grid').treegrid({ onClickRow: Events.OnClickMessage });
    $('#Grid').treegrid({ loadFilter: Events.OnPage });
    $('#Grid').treegrid({ rowStyler: Events.ToColor });
    $('#Grid').treegrid("getColumnOption", "Date").formatter = Events.ToDateTime;
    $('#Items').datagrid({ onClickRow: Events.OnClickItem });
    $('#Items').datagrid("getColumnOption", "Make").formatter = Events.ToTime;
    $('#Items').datagrid("getColumnOption", "Ship").formatter = Events.ToTime;
    $('#LinkGrid').datagrid({ loadFilter: Events.OnLinkPage });
    $('#LinkGrid').datagrid("getColumnOption", "Date").formatter = Events.ToDateTime;
    $('#Querys').find('[unique="Query"]').bind('click', Events.OnQuery);
    $('#Buttons').find('[unique="Create"]').bind('click', Events.OnCreate);
    $('#Buttons').find('[unique="Delete"]').bind('click', Events.OnDelete);
    $('#Buttons').find('[unique="Save"]').bind('click', Events.OnSave);
    $('#Buttons').find('[unique="Cancel"]').bind('click', Events.OnCancel);
    $('#Buttons').find('[unique="Link"]').bind('click', Events.OnLink);
    $('#Buttons').find('[unique="Reply"]').bind('click', Events.OnReply);
    $('#Buttons').find('[unique="Rubbish"]').bind('click', Events.OnRubbish);
    $('#Area').combobox({ onSelect: Events.OnSelect });
    $('#LinkButtons').find('[unique="Save"]').bind('click', Events.OnOk);
    $('#LinkButtons').find('[unique="Cancel"]').bind('click', Events.OnClose);
    $('#ReplyButtons').find('[unique="Save"]').bind('click', Events.OnSend);
    $('#ReplyButtons').find('[unique="Cancel"]').bind('click', Events.OnClose);
    $('#ReplyText').bind('keyup', Events.OnKeyup);

    var callback = function () {
        var items = $('#Querys').find('[unique="Supplier"]').combobox('getData');
        items.splice(0, 0, { 'Unique': 'All', 'Name': '(全部)' }, { 'Unique': 'Null', 'Name': '(无)' });

        $('#Querys').find('[unique="Supplier"]').combobox('loadData', items);
        $('#Querys').find('[unique="Supplier"]').combobox('setValue', items[0].Unique);
    };
    Eventer.ComboBox($('#Querys').find('[unique="Supplier"]'), 'GsmSupplier', 'GetEntities', [], callback);
    Eventer.ComboBox($('#Supplier'), 'GsmSupplier', 'GetEntities', []);
    Eventer.ComboBox($('#Tree'), 'GsmTree', 'GetEntities', []);
    Eventer.ComboBox($('#Area'), 'GsmArea', 'GetEntities', []);
    Eventer.ComboBox($('#Line'), 'GsmLine', 'GetEntities', []);
    Events.Page();
});

var Events = {
    Message: 'GsmMessage',
    Item: 'GsmItem',
    Origin: '',

    ToDateTime: function (value) {
        return moment(value).format('YYYY-MM-DD HH:mm');
    },

    ToColor: function (row) {
        if (row.Status == 1) return 'background-color:#eee;';
    },

    ToTime: function (value) {
        return moment(value).format('HH:mm');
    },

    CheckLicense: function (date, license) {
        var data = {
            service: Events.Item,
            method: 'GetEntityByDateAndLicense',
            args: [date, license]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);
            
            if (root.Date != null) $('#Check').html('<font color="red">当天内该车牌号已保存！时间：' + Events.ToTime(root.Date) + '</font>');
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    RefreshItem: function () {
        Eventer.Grid($('#Items'), Events.Item, 'GetEntitiesWithAreaUniqueByMessage', [parseInt(Eventer.Get($('#Message')))]);
    },

    Page: function () {
        var date = Eventer.Get($('#Querys').find('[unique="Date"]'));
        var supplier = Eventer.Get($('#Querys').find('[unique="Supplier"]'), 'getText');
        var mobile = Eventer.Get($('#Querys').find('[unique="Mobile"]'));
        var pageNumber = $('#Grid').treegrid('options').pageNumber;
        var pageSize = $('#Grid').treegrid('options').pageSize;
        
        Events.GetGrid(date, supplier, mobile, (pageNumber - 1) * pageSize + 1, pageSize);
    },

    PageLink: function () {
        var pageNumber = $('#LinkGrid').datagrid('options').pageNumber;
        var pageSize = $('#LinkGrid').datagrid('options').pageSize;

        Events.GetLinkGrid((pageNumber - 1) * pageSize + 1, pageSize);
    },

    GetGrid: function (date, supplier, mobile, start, length) {
        Eventer.Grid($('#Grid'), Events.Message, 'GetGridByMethodAndParent', ['GetEntitiesWithSupplierNameByDateAndSupplierAndMobileAndStartAndLength', [date, supplier, mobile, start, length], 'Parent']);
    },

    GetLinkGrid: function (start, length) {
        var row = $('#Grid').treegrid('getSelected');
        var callback = function () {
            if (row.Parent != null) $('#LinkGrid').datagrid('selectRecord', row.Parent);
        };
        Eventer.Grid($('#LinkGrid'), Events.Message, 'GetEntitiesByFieldAndStartAndLength', ['Mobile', $('#Messages').find('[unique="Mobile"]').val(), '=', start, length], callback);
    },

    OnClickMessage: function () {
        var row = $('#Grid').treegrid('getSelected');
        
        Eventer.Set($('#Message'), row.Unique);
        Eventer.Set($('#Messages').find('[unique="Mobile"]'), row.Mobile);
        Eventer.Set($('#Messages').find('[unique="Date"]'), Events.ToDateTime(row.Date));
        Eventer.Set($('#Messages').find('[unique="Text"]'), row.Text);
        Eventer.Set($('#Messages').find('[unique="Remark"]'), row.Remark);

        Events.RefreshItem();

        Events.Origin = '';
        Buttons.Clear();
        Controls.Disabled();
        Controls.Clear();
    },

    OnClickItem: function () {
        Eventer.Click($('#Items'), Buttons, Controls);
    },

    OnPage: function (data) {
        Eventer.Page($('#Grid'), Events.Page);

        return data;
    },

    OnLinkPage: function (data) {
        Eventer.Page($('#LinkGrid'), Events.PageLink);

        return data;
    },

    OnQuery: function () {
        Events.Page();
    },

    OnCreate: function () {
        Eventer.Create(Buttons, Controls);

        var data = {
            service: Events.Message,
            method: 'GetItemsByText',
            args: [Eventer.Get($('#Messages').find('[unique="Text"]'))]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }

            var items = Ajaxer.GetItems(root);

            var rows = $('#Items').datagrid('getRows');

            for (var i = 0; i < items.length; i++) {
                var found = false;

                for (var j = 0; j < rows.length; j++) {
                    if (items[i].License == rows[j].License) found = true;
                }

                if (found == false) {
                    Eventer.Set($('#Supplier'), items[i].Supplier, 'setText');
                    Eventer.Set($('#Tree'), items[i].Tree, 'setText');
                    Eventer.Set($('#Make'), items[i].Make, null, Events.ToTime);
                    Eventer.Set($('#Area'), items[i].Area);
                    Events.Origin = items[i].Origin;
                    Eventer.Set($('#License'), items[i].License);
                    Eventer.Set($('#Driver'), items[i].Driver);
                    Eventer.Set($('#Ship'), items[i].Ship, null, Events.ToTime);
                    Eventer.Set($('#Line'), items[i].Line, 'setText');

                    Events.CheckLicense(moment(Eventer.Get($('#Messages').find('[unique="Date"]'))).format('YYYY-MM-DD'), items[i].License);

                    break;
                }
            }
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    OnDelete: function () {
        Eventer.Delete($('#Unique'), Events.Item, Events.RefreshItem, Buttons, Controls);
    },

    OnSave: function () {
        if (Checker.Valid($('#License'), '请输入车牌号！') == false) return;
        if (Checker.Valid($('#Driver'), '请输入司机电话！') == false) return;

        Eventer.Save($('#Unique'), Events.Item, Events.RefreshItem, Buttons, Controls);

        Events.Page();
    },

    OnCancel: function () {
        Eventer.Cancel(Buttons, Controls);
    },

    OnLink: function () {
        var row = $('#Grid').treegrid('getSelected');
        Events.GetLinkGrid(1, $('#LinkGrid').datagrid('options').pageSize);
        Eventer.Set($('#LinkRemark'), row.Remark)

        $('#LinkDialog').dialog('open');
    },

    OnReply: function () {
        var row = $('#Grid').treegrid('getSelected');
        Eventer.Set($('#ReplyMobile'), row.Mobile);
        Eventer.Set($('#ReplyText'));

        $('#ReplyDialog').dialog('open');
    },

    OnRubbish: function () {
        if (confirm("是否确定要标记该记录为垃圾短信？") == false) return;

        var row = $('#Grid').treegrid('getSelected');

        var entity = {
            Unique: row.Unique,
            Parent: row.Parent,
            Mobile: row.Mobile,
            Date: moment(row.Date).format('YYYY-MM-DD HH:mm:ss'),
            Text: row.Text,
            Remark: row.Remark,
            Status: 1,
            Operator: Account
        };
        var data = {
            service: Events.Message,
            method: 'SaveEntity',
            args: [JSON.stringify(entity)]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }

            Events.Page();

            alert(root.Message);
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    OnSelect: function (item) {
        var callback = function () {
            Eventer.Set($('#Origin'), Events.Origin, 'setText');
        };

        Eventer.ComboBox($('#Origin'), 'GsmOrigin', 'GetEntitiesWithAreaNameByAreaAndStartAndLength', [item.Unique, 1, Setter.Max], callback);
    },

    OnOk: function () {
        var row = $('#Grid').treegrid('getSelected');
        var link = $('#LinkGrid').datagrid('getSelected');

        var entity = {
            Unique: row.Unique,
            Parent: (link == null ? null : link.Unique),
            Mobile: row.Mobile,
            Date: moment(row.Date).format('YYYY-MM-DD HH:mm:ss'),
            Text: row.Text,
            Remark: Eventer.Get($('#LinkRemark')),
            Status: row.Status,
            Operator: Account
        };
        var data = {
            service: Events.Message,
            method: 'SaveEntity',
            args: [JSON.stringify(entity)]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }

            Events.Page();

            alert(root.Message);

            $('#LinkDialog').dialog('close');
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    OnSend: function () {
        var entity = {
            Command: 'SendMessage',
            Mobile: Eventer.Get($('#ReplyMobile')),
            Text: Eventer.Get($('#ReplyText'))
        };
        var data = {
            service: 'Exchange.Single',
            method: 'Execute',
            args: [Zhhwy, JSON.stringify(entity), 15]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }

            alert(root.Message);

            $('#ReplyDialog').dialog('close');
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    OnClose: function () {
        $('#LinkDialog').dialog('close');
        $('#ReplyDialog').dialog('close');
    },

    OnKeyup: function () {
        var length = Eventer.Get($('#ReplyText')).length;

        if (length <= 70) {
            $('#ReplyRemark').html('<font color="blue">已输入' + length + '个字，还可输入' + (70 - length) + '个字。</font>');
            Eventer.Show($('#ReplyButtons'), 'Save');
        } else {
            $('#ReplyRemark').html('<font color="red">已超过70个字，请删除' + (length - 70) + '个字。</font>');
            Eventer.Hide($('#ReplyButtons'), 'Save');
        }
    }
};

var Buttons = {
    State: 'None',

    Update: function () {
        Eventer.Show($('#Buttons'), 'Link');
        Eventer.Show($('#Buttons'), 'Reply');
        Eventer.Show($('#Buttons'), 'Rubbish');
        Eventer.Hide($('#Buttons'), 'Create');
        Eventer.Show($('#Buttons'), 'Delete');
        Eventer.Show($('#Buttons'), 'Save');
        Eventer.Show($('#Buttons'), 'Cancel');

        this.State = 'Update';
    },

    Create: function () {
        Eventer.Show($('#Buttons'), 'Link');
        Eventer.Show($('#Buttons'), 'Reply');
        Eventer.Show($('#Buttons'), 'Rubbish');
        Eventer.Hide($('#Buttons'), 'Create');
        Eventer.Hide($('#Buttons'), 'Delete');
        Eventer.Show($('#Buttons'), 'Save');
        Eventer.Show($('#Buttons'), 'Cancel');

        this.State = 'Create';
    },

    Delete: function () {
        Eventer.Show($('#Buttons'), 'Link');
        Eventer.Show($('#Buttons'), 'Reply');
        Eventer.Show($('#Buttons'), 'Rubbish');
        Eventer.Show($('#Buttons'), 'Create');
        Eventer.Hide($('#Buttons'), 'Delete');
        Eventer.Hide($('#Buttons'), 'Save');
        Eventer.Hide($('#Buttons'), 'Cancel');

        this.State = 'None';
    },

    Save: function () {
        if (this.State == 'Update' || this.State == 'Create') {
            Eventer.Show($('#Buttons'), 'Link');
            Eventer.Show($('#Buttons'), 'Reply');
            Eventer.Show($('#Buttons'), 'Rubbish');
            Eventer.Show($('#Buttons'), 'Create');
            Eventer.Show($('#Buttons'), 'Delete');
            Eventer.Hide($('#Buttons'), 'Save');
            Eventer.Hide($('#Buttons'), 'Cancel');
        }

        this.State = 'None';
    },

    Cancel: function () {
        if (this.State == 'Update') {
            Eventer.Show($('#Buttons'), 'Link');
            Eventer.Show($('#Buttons'), 'Reply');
            Eventer.Show($('#Buttons'), 'Rubbish');
            Eventer.Show($('#Buttons'), 'Create');
            Eventer.Show($('#Buttons'), 'Delete');
            Eventer.Hide($('#Buttons'), 'Save');
            Eventer.Hide($('#Buttons'), 'Cancel');
        }

        if (this.State == 'Create') {
            Eventer.Show($('#Buttons'), 'Link');
            Eventer.Show($('#Buttons'), 'Reply');
            Eventer.Show($('#Buttons'), 'Rubbish');
            Eventer.Show($('#Buttons'), 'Create');
            Eventer.Hide($('#Buttons'), 'Delete');
            Eventer.Hide($('#Buttons'), 'Save');
            Eventer.Hide($('#Buttons'), 'Cancel');
        }

        this.State = 'None';
    },

    Clear: function () {
        Eventer.Show($('#Buttons'), 'Link');
        Eventer.Show($('#Buttons'), 'Reply');
        Eventer.Show($('#Buttons'), 'Rubbish');
        Eventer.Show($('#Buttons'), 'Create');
        Eventer.Hide($('#Buttons'), 'Delete');
        Eventer.Hide($('#Buttons'), 'Save');
        Eventer.Hide($('#Buttons'), 'Cancel');
    }
};

var Controls = {
    Clear: function () {
        Eventer.Set($('#Unique'));
        Eventer.Set($('#Supplier'));
        Eventer.Set($('#Tree'));
        Eventer.Set($('#Make'));
        Eventer.Set($('#Area'));
        Eventer.Set($('#License'));
        $('#Check').html('');
        Eventer.Set($('#Driver'));
        Eventer.Set($('#Ship'));
        Eventer.Set($('#Line'));
        Eventer.Set($('#Remark'));
    },

    Get: function () {
        return {
            Unique: Eventer.Get($('#Unique')),
            Message: Eventer.Get($('#Message')),
            Supplier: Eventer.Get($('#Supplier'), 'getText'),
            Tree: Eventer.Get($('#Tree'), 'getText'),
            Make: '2000-01-01 ' + Eventer.Get($('#Make')),
            Area: Eventer.Get($('#Area'), 'getText'),
            Origin: Eventer.Get($('#Origin'), 'getText'),
            License: Eventer.Get($('#License')),
            Driver: Eventer.Get($('#Driver')),
            Ship: '2000-01-01 ' + Eventer.Get($('#Ship')),
            Line: Eventer.Get($('#Line'), 'getText'),
            Remark: Eventer.Get($('#Remark')),
            Operator: Account
        };
    },

    Set: function () {
        var row = $('#Items').datagrid('getSelected');

        Eventer.Set($('#Unique'), row.Unique);
        Eventer.Set($('#Supplier'), row.Supplier, 'setText');
        Eventer.Set($('#Tree'), row.Tree, 'setText');
        Eventer.Set($('#Make'), row.Make, null, Events.ToTime);
        Eventer.Set($('#Area'), row.AreaUnique);
        Events.Origin = row.Origin;
        Eventer.Set($('#License'), row.License);
        Eventer.Set($('#Driver'), row.Driver);
        Eventer.Set($('#Ship'), row.Ship, null, Events.ToTime);
        Eventer.Set($('#Line'), row.Line, 'setText');
        Eventer.Set($('#Remark'), row.Remark);
    },

    SetOrigin: function () {
        var row = $('#Items').datagrid('getSelected');
        
        if (row == null) return;

        Eventer.Set($('#Origin'), row.Origin, 'setText');
    },

    Enabled: function () {
        Eventer.Enable($('#Supplier'));
        Eventer.Enable($('#Tree'));
        Eventer.Enable($('#Make'));
        Eventer.Enable($('#Area'));
        Eventer.Enable($('#Origin'));
        Eventer.Enable($('#License'));
        Eventer.Enable($('#Driver'));
        Eventer.Enable($('#Ship'));
        Eventer.Enable($('#Line'));
        Eventer.Enable($('#Remark'));
    },

    Disabled: function () {
        Eventer.Disable($('#Supplier'));
        Eventer.Disable($('#Tree'));
        Eventer.Disable($('#Make'));
        Eventer.Disable($('#Area'));
        Eventer.Disable($('#Origin'));
        Eventer.Disable($('#License'));
        Eventer.Disable($('#Driver'));
        Eventer.Disable($('#Ship'));
        Eventer.Disable($('#Line'));
        Eventer.Disable($('#Remark'));
    }
};
