$(function () {
    $('#Grid').treegrid({ onClickRow: Events.OnClick });
    $('#Grid').treegrid({ loadFilter: Events.OnPage });
    $("#Buttons").find("[unique='Create']").bind('click', Events.OnCreate);
    $("#Buttons").find("[unique='Delete']").bind('click', Events.OnDelete);
    $("#Buttons").find("[unique='Save']").bind('click', Events.OnSave);
    $("#Buttons").find("[unique='Cancel']").bind('click', Events.OnCancel);

    Events.Page();
    Events.GetParent();
});

var Events = {
    Service: 'Function',

    ToName: function (value, row) {
        if (row.Id == null) return row.RoleName;

        return value;
    },

    GetParent: function () {
        var data = {
            service: 'Function',
            method: 'GetTreeByMethodAndParentAndUniqueAndName',
            args: ['GetEntities', [], 'Parent', 'Unique', 'Name']
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }
            
            var items = Ajaxer.GetTree(root);
            var item = { 'id': '', 'text': '(无)' };
            items.splice(0, 0, item);

            $('#Parent').combotree('loadData', items);
            $('#Parent').combotree('setValue', items[0].id);
        }

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    Page: function () {
        var pageNumber = $('#Grid').treegrid('options').pageNumber;
        var pageSize = $('#Grid').treegrid('options').pageSize;

        Events.GetGrid((pageNumber - 1) * pageSize + 1, pageSize);
    },

    GetGrid: function (start, length) {
        Eventer.Grid($('#Grid'), Events.Service, 'GetGridByMethodAndParent', ['GetEntities', [], 'Parent']);
    },

    OnClick: function () {
        Eventer.Click($('#Grid'), Buttons, Controls);
    },

    OnPage: function (data) {
        Eventer.Page($('#Grid'), Events.Page);

        return data;
    },

    OnCreate: function () {
        Eventer.Create(Buttons, Controls);
    },

    OnDelete: function () {
        var row = $('#Grid').treegrid('getSelected');
        if (row.children.length > 0) {
            alert("该记录存在子级记录，不能删除！");
            return;
        }

        Eventer.Delete($('#Unique'), Events.Service, Events.Page, Buttons, Controls);
    },

    OnSave: function () {
        if (Checker.Valid($('#Order'), '请输入顺号！') == false) return;
        if (Checker.Valid($('#No'), '请输入编号！') == false) return;
        if (Checker.Valid($('#Name'), '请输入名称！') == false) return;
        if (Checker.Valid($('#Url'), '请输入网址！') == false) return;
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
        Eventer.Set($('#Parent'));
        Eventer.Set($('#Type'));
        Eventer.Set($('#Order'));
        Eventer.Set($('#No'));
        Eventer.Set($('#Name'));
        Eventer.Set($('#Url'));
        Eventer.Set($('#Remark'));
    },

    Get: function () {
        return {
            Unique: Eventer.Get($('#Unique')),
            Parent: Eventer.Get($('#Parent')),
            Type: Eventer.Get($('#Type')),
            Order: Eventer.Get($('#Order')),
            No: Eventer.Get($('#No')),
            Name: Eventer.Get($('#Name')),
            Url: Eventer.Get($('#Url')),
            Remark: Eventer.Get($('#Remark')),
            Operator: Account
        };
    },

    Set: function () {
        var row = $('#Grid').treegrid('getSelected');
        
        Eventer.Set($('#Unique'), row.Unique);
        Eventer.Set($('#Parent'), row.Parent);
        Eventer.Set($('#Type'), row.Type);
        Eventer.Set($('#Order'), row.Order);
        Eventer.Set($('#No'), row.No);
        Eventer.Set($('#Name'), row.Name);
        Eventer.Set($('#Url'), row.Url);
        Eventer.Set($('#Remark'), row.Remark);
    },

    Enabled: function () {
        Eventer.Enable($('#Parent'));
        Eventer.Enable($('#Type'));
        Eventer.Enable($('#Order'));
        Eventer.Enable($('#No'));
        Eventer.Enable($('#Name'));
        Eventer.Enable($('#Url'));
        Eventer.Enable($('#Remark'));
    },

    Disabled: function () {
        Eventer.Disable($('#Parent'));
        Eventer.Disable($('#Type'));
        Eventer.Disable($('#Order'));
        Eventer.Disable($('#No'));
        Eventer.Disable($('#Name'));
        Eventer.Disable($('#Url'));
        Eventer.Disable($('#Remark'));
    }
};
