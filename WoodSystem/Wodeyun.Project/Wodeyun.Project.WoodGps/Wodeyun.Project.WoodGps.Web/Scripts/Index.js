var serviceUniques = ""; // 单返机的服务器唯一识别码组合

$(function () {
    var grid = $('#Grid');
    grid.datagrid({ onClickRow: Events.OnClick });
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化

    var tdOfButtons = $('#Buttons');
    tdOfButtons.find('[unique="Create"]').bind('click', Events.OnCreate);
    tdOfButtons.find('[unique="Delete"]').bind('click', Events.OnDelete);
    tdOfButtons.find('[unique="Save"]').bind('click', Events.OnSave);
    tdOfButtons.find('[unique="Cancel"]').bind('click', Events.OnCancel);

    $('#aGetPos').bind('click', Events.GetGps);

    Events.Page();
    Events.GetUniqueOfSevice();
});

var Events = {
    Service: 'WoodGps',

    GetGps: function () {
        if (serviceUniques == "") {
            $.messager.alert('提示消息', '您还没有获得该设备的读卡器的使用权限', 'warning');
            return;
        }

        /* 获取GPS坐标 */
        var uniques = serviceUniques;
        var entity = {
            Command: 'GPS'
        };
        var second = 60;
        var params = [uniques, JSON.stringify(entity), second]

        var data = {
            service: "Exchange.Single",
            method: "Execute",
            args: params
        };

        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }

            Eventer.Set($('#txtLat'), root.Latitude);
            Eventer.Set($('#txtLng'), root.Longitude);
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    GetUniqueOfSevice: function () {
        var data = {
            service: 'WoodPowerOfReadCard',
            method: 'GetEntityByAccount',
            args: [Account]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }

            if (root.Unique == null) Eventer.Disable($('#aGetPos'));
            else serviceUniques = root.MachineNumber;
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    Page: function () {
        var gridOptions = $('#Grid').datagrid('options');
        var pageSize = gridOptions.pageSize;

        Events.GetGrid((gridOptions.pageNumber - 1) * pageSize + 1, pageSize);
    },

    GetGrid: function (start, length) {
        Eventer.Grid($('#Grid'), Events.Service, 'GetEntitiesByStartAndLength', [start, length]);
    },

    OnClick: function () {
        Eventer.Click($('#Grid'), Buttons, Controls);
    },

    OnCreate: function () {
        Eventer.Create(Buttons, Controls);
    },

    OnDelete: function () {
        Eventer.Delete($('#Unique'), Events.Service, Events.Page, Buttons, Controls);
    },

    OnSave: function () {
        if (Checker.Valid($('#txtStationName'), '检查站名称是必须的！') == false) return;
        if (Checker.Valid($('#txtLat'), 'GPS纬度是必须的！') == false) return;
        if (Checker.Valid($('#txtLng'), 'GPS经度是必须的！') == false) return;

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
        Eventer.Set($('#txtStationName'));
        Eventer.Set($('#txtLat'));
        Eventer.Set($('#txtLng'));
    },

    Get: function () {
        return {
            Unique: Eventer.Get($('#Unique')),
            StationName: Eventer.Get($('#txtStationName')),
            Lat: Eventer.Get($('#txtLat')),
            Lng: Eventer.Get($('#txtLng')),
            Operator: Account
        };
    },

    Set: function () {
        var row = $('#Grid').datagrid('getSelected');

        Eventer.Set($('#Unique'), row.Unique);
        Eventer.Set($('#txtStationName'), row.StationName);
        Eventer.Set($('#txtLat'), row.Lat);
        Eventer.Set($('#txtLng'), row.Lng);
    },

    Enabled: function () {
        Eventer.Enable($('#txtStationName'));
        Eventer.Enable($('#txtLat'));
        Eventer.Enable($('#txtLng'));
    },

    Disabled: function () {
        Eventer.Disable($('#txtStationName'));
        Eventer.Disable($('#txtLat'));
        Eventer.Disable($('#txtLng'));
    }
};
