var serviceUniques = ""; // 单返机的服务器唯一识别码组合

var latitude = 0.00; // GPS纬度
var longitude = 0.00; // GPS经度

var map = null; // 百度地图

$(function () {
    var grid = $('#Grid');
    grid.datagrid({ onClickRow: Events.OnClick });
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    grid.datagrid("getColumnOption", "TimeOfStation").formatter = Events.ToDateTime;
    grid.datagrid("getColumnOption", "PhotoNumber").formatter = Events.ToNumber;
    grid.datagrid("getColumnOption", "GPS").formatter = Events.ToMeter;

    var tdOfButtons = $('#Buttons');
    tdOfButtons.find('[unique="Delete"]').bind('click', Events.OnDelete);
    tdOfButtons.find('[unique="Save"]').bind('click', Events.OnSave);

    $('#txtCard').bind('blur', Events.OnBlur);
    $('#aReadCard').bind('click', Events.OnReadCard);
    $('#aPhoto').bind('click', Events.Photo);
    $('#aSaveImage').bind('click', Events.SavePhoto);
    $('#txtCartChinese').bind('blur', Events.GetWoodWhereComeFrom);
    $('#txtCartNumber').bind('blur', Events.GetWoodWhereComeFrom);

    Events.GetUniqueOfSevice();
    Events.GetNameAndGpsForStation();
    Events.Page();
    Controls.ForbidPhoto();
});

var Events = {
    Table: 'Barrier',

    ToNumber: function (value) {
        if (value == null) return '';

        var htmlValue = "";
        if (parseInt(value) == 0) htmlValue = "<span style=\"color:red;\">0</span> 张";
        else htmlValue = value + " 张";

        return htmlValue;
    },

    Trim: function (s) {
        return s.replace(/(^\s*)|(\s*$)/g, "");
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

    OnBlur: function () {
        var cardTextBox = $(this);
        /* 焦点离开输入框时 */
        if (Eventer.Get(cardTextBox) == "") return;

        Eventer.Disable(cardTextBox);
        Events.ShowShortNumberOfCard(cardTextBox, Eventer.Get(cardTextBox), $('#txtCID'), '该电子卡还未进行编号', Events.GetGps);
    },

    ReadCard: function (cardTextBox) {
        /* 读卡 */
        Eventer.Enable(cardTextBox);
        cardTextBox.focus();
    },

    OnReadCard: function () {
        Controls.Clear();
        Events.ReadCard($('txtCard')); // 读卡
    },

    SavePhoto: function () {
        if (Checker.Valid($('#txtGPS'), 'GPS距离是必须的，请关闭系统重新登录') == false) return;
        if (Eventer.Get($('#txtRecordID')) == "0") {
            $.messager.alert('提示消息', '请先发放电子卡，并保存成功', 'warning');
            return;
        }
        if (Eventer.Get($('#txtImageFileName')) == "") {
            $.messager.alert('提示消息', '请先拍照', 'warning');
            return;
        }

        
        /* 保存照片 */
        var entity = {
            BarrierID: Eventer.Get($('#txtRecordID')),
            GPS: Eventer.Get($('#txtGPS')),
            ImageFileName: Eventer.Get($('#txtImageFileName')),
            Operator: Account
        };
        var params = [JSON.stringify(entity)];

        var data = {
            service: Events.Table,
            method: "SavePhoto",
            args: params
        };

        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                $.messager.alert('提示消息', root.Message, 'error');
                return;
            }

            $.messager.alert('提示消息', root.Message, 'info');

            Events.Page();
            document.getElementById("imgPhoto").src = "Images/msn.gif";
            Eventer.Set($('#txtImageFileName'), "");
            Eventer.Set($('#txtGPS'));

            /* 显示百度地图 */
            if (map == null) return;

            map.clearOverlays(); // 清空旧的标注

            /*  重新添加标注 */
            var pointFixed = new BMap.Point(longitude, latitude);  // 创建指定点坐标

            /* 动态改变地图的放大倍数 */
            map.setZoom(13);

            /* 显示标注点 */
            var markerFixed = new BMap.Marker(pointFixed);  // 创建指定点的标注
            map.addOverlay(markerFixed);              // 将标注添加到地图中
            var label = new BMap.Label("指定点", { offset: new BMap.Size(20, -10) }); // 文字标签
            markerFixed.setLabel(label);
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    ShowGpsExtent: function (lat, lng) {
        /* 显示百度地图 */
        if (map == null) return;

        map.clearOverlays(); // 清空旧的标注

        /*  重新添加标注 */
        var pointCurrent = new BMap.Point(lng, lat);  // 创建当前所在点坐标
        var pointFixed = new BMap.Point(longitude, latitude);  // 创建指定点坐标

        var gpslength = parseFloat(map.getDistance(pointCurrent, pointFixed).toFixed(2)); // 获取两点间的距离，单位：米

        /* 动态改变地图的放大倍数 */
        var zoomNumber = 17; // 值范围：3 - 19 ，值越大，则显示越清晰
        if (gpslength > 100000) zoomNumber = 8;
        else if (gpslength > 10000) zoomNumber = 10;
        else if (gpslength > 5000) zoomNumber = 11;
        else if (gpslength > 1000) zoomNumber = 13;
        else if (gpslength > 500) zoomNumber = 14;
        else if (gpslength > 100) zoomNumber = 15;
        else if (gpslength > 50) zoomNumber = 16;
        map.setZoom(zoomNumber);

        /* 显示标注点 */
        var markerCurrent = new BMap.Marker(pointCurrent);  // 创建当前所在标注
        map.addOverlay(markerCurrent);              // 将标注添加到地图中
        var markerFixed = new BMap.Marker(pointFixed);  // 创建指定点的标注
        map.addOverlay(markerFixed);              // 将标注添加到地图中
        var label = new BMap.Label("指定点", { offset: new BMap.Size(20, -10) }); // 文字标签
        markerFixed.setLabel(label);

        /* 显示路径到百度地图 */
        var polyline = new BMap.Polyline([
                pointCurrent,
                pointFixed
                ], { strokeColor: "blue", strokeWeight: 2, strokeOpacity: 0.5 });
        map.addOverlay(polyline);

        Eventer.Set($('#txtGPS'), gpslength); // 显示两点间的距离
    },

    GetGps: function () {
        if (serviceUniques == "") {
            $.messager.alert('提示消息', '您还没有获得该平板电脑的使用权限，请关闭系统重新登录', 'warning');
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
                $.messager.alert('提示消息', root.Message, 'error');
                return;
            }
            
            Events.ShowGpsExtent(parseFloat(root.Latitude), parseFloat(root.Longitude));
        };

        Ajaxer.Ajax(Setter.Url, data, success);

    },

    ShowImage: function (imgFileName) {
        /* 显示照片 */
        var requetUrl = "Handers/ShowImage.ashx?FN=" + imgFileName;
        document.getElementById("imgPhoto").src = requetUrl;
    },

    Photo: function () {
        if (serviceUniques == "") {
            $.messager.alert('提示消息', '您还没有获得该平板电脑的使用权限，请关闭系统重新登录', 'warning');
            return;
        }
        if (Eventer.Get($('#txtRecordID')) == "0") {
            $.messager.alert('提示消息', '请先发放电子卡，并保存成功', 'warning');
            return;
        }

        /* 下发拍照指令 */
        var uniques = serviceUniques;
        var entity = {
            Command: 'Photo'
        };
        var second = 300;
        var params = [uniques, JSON.stringify(entity), second]

        var data = {
            service: "Exchange.Single",
            method: "Execute",
            args: params
        };

        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                $.messager.alert('提示消息', root.Message, 'error');
                return;
            }

            Eventer.Set($('#txtImageFileName'), root.Photo);
            Events.ShowGpsExtent(parseFloat(root.Latitude), parseFloat(root.Longitude));
            Events.ShowImage(root.Photo);
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    GetNameAndGpsForStation: function () {
        var data = {
            service: 'WoodPowerOfGps',
            method: 'GetEntityByAccount',
            args: [Account]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }

            if (root.Unique == null) {
                Eventer.Disable($('#aPhoto'));
                Eventer.Disable($('#aUpload'));
            }
            else {
                Eventer.Set($('#txtPlace'), root.StationName);
                latitude = parseFloat(root.Lat);
                longitude = parseFloat(root.Lng);

                /* 显示百度地图 */
                map = new BMap.Map("divBaiDuMap");
                map.centerAndZoom(new BMap.Point(longitude, latitude), 13);

                var marker = new BMap.Marker(new BMap.Point(longitude, latitude));  // 创建指定点的标注
                map.addOverlay(marker);              // 将标注添加到地图中

                var label = new BMap.Label("指定点", { offset: new BMap.Size(20, -10) }); // 文字标签
                marker.setLabel(label);

            }
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
                $.messager.alert('提示消息', root.Message, 'error');
                return;
            }

            if (root.Unique == null) Eventer.Disable($('#aReadCard'));
            else serviceUniques = root.MachineNumber;
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    ToMeter: function (value) {
        var str = "";
        if (parseFloat(value) > 5000) str = "<span style=\"color:red;\">" + value + "</span>米";
        else str = value + "米";

        return str;
    },

    ToDateTime: function (value) {
        return moment(value).format('YYYY-MM-DD HH:mm');
    },

    Page: function () {
        var gridOptions =$('#Grid').datagrid('options');
        var pageSize = gridOptions.pageSize;

        Events.GetGrid((gridOptions.pageNumber - 1) * pageSize + 1, pageSize);
    },

    GetGrid: function (start, length) {
        Eventer.Grid($('#Grid'), Events.Table, 'GetEntitiesByStartAndLengthWithOperator', [start, length, Account]);
    },

    OnClick: function () {
        Eventer.Click($('#Grid'), Buttons, Controls);
    },

    OnDelete: function () {
        Eventer.Delete($('#txtUnique'), Events.Table, Events.Page, Buttons, Controls, Account, Eventer.Get($('#txtCID')));
    },

    OnSave: function () {
        if (Checker.Valid($('#txtPlace'), '您还没有获得该检查站的GPS定位拍照权限，请关闭系统重新登录') == false) return;
        if (Checker.Valid($('#txtCard'), '绿卡号是必须的') == false) return;
        if (Checker.Valid($('#txtCartChinese'), '请选择车牌省份') == false) return;
        if (Checker.Valid($('#txtCartNumber'), '请选择车牌号') == false) return;
        if (Checker.Valid($('#txtArea'), '木材来源地是必须的') == false) return;
        if (Checker.Valid($('#txtGPS'), 'GPS距离是必须的，请关闭系统重新登录') == false) return;
        if (Eventer.Get($('#txtCID')) == "0") {
            $.messager.alert('提示消息', '该绿卡号无效，请重新发卡', 'warning');
            return;
        }

        Eventer.Save($('#txtUnique'), Events.Table, Events.Page, Buttons, Controls);
    },

    OnReadCard: function () {
        Controls.Clear();
        Events.ReadCard($('#txtCard')); // 读卡
    },

    GetWoodWhereComeFrom: function () {
        /*  根据车牌号到短信报备系统或木材收购系统获取相应的木材来源地 */
        var chinese = Eventer.Get($('#txtCartChinese'));
        var letter = Eventer.Get($('#txtCartNumber'));
        if (chinese == "" || letter == "") return;

        var license = chinese + letter;

        var data = {
            service: Events.Table,
            method: 'GetEntityByFieldForWoodWhereComeFrom',
            args: ['License', license, '=']
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Area != null) Eventer.Set($('#txtArea'), root.Area);
            else Eventer.Set($('#txtArea'));
        };

        Ajaxer.Ajax(Setter.Url, data, success);
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
        Eventer.Set($('#txtCID'), 0);
        Eventer.Set($('#txtCard'));
        Eventer.Set($('#txtCartChinese'));
        Eventer.Set($('#txtCartNumber'));
        Eventer.Set($('#txtArea'));
        Eventer.Set($('#txtGPS'));
    },

    Get: function () {
        return {
            Unique: Eventer.Get($('#txtUnique')),
            Place: Eventer.Get($('#txtPlace')),
            CardNumber: Eventer.Get($('#txtCard')),
            License: Events.Trim(Eventer.Get($('#txtCartChinese'))) + Events.Trim(Eventer.Get($('#txtCartNumber'))).toUpperCase(),
            Area: Eventer.Get($('#txtArea')),
            GPS: Eventer.Get($('#txtGPS')),
            Operator: Account
        };
    },

    Set: function () {
        var row = $('#Grid').datagrid('getSelected');

        Eventer.Set($('#txtUnique'), row.Unique);
        Eventer.Set($('#txtCard'), row.CardNumber);
        Eventer.Set($('#txtCID'), row.CardNumber);
        var license = row.License; // 车牌号
        var chinese = license.substr(0, 1);
        var letter = license.substr(1, license.length - 1);
        Eventer.Set($('#txtCartChinese'), chinese);
        Eventer.Set($('#txtCartNumber'), letter);
        Eventer.Set($('#txtPlace'), row.Place);
        Eventer.Set($('#txtGPS'), row.GPS);
        Eventer.Set($('#txtArea'), row.Area);
    },

    Enabled: function () {
    },

    Disabled: function () {
    },

    ForbidPhoto: function () {
        Eventer.Disable($('#aPhoto'));
        Eventer.Disable($('#aSaveImage'));
        Eventer.Set($('#txtImageFileName'), "");
        Eventer.Set($('#txtRecordID'), 0);
    },

    PermitPhoto: function (rcordID) {
        if (Eventer.Get($('#txtUnique')) == "0") {
            /* 保存好记录关联号，允许拍照 */
            Eventer.Set($('#txtRecordID'), rcordID);
            Eventer.Enable($('#aPhoto'));
            Eventer.Enable($('#aSaveImage'));

            /* 显示百度地图 */
            if (map == null) return;

            map.clearOverlays(); // 清空旧的标注

            /*  重新添加标注 */
            var pointFixed = new BMap.Point(longitude, latitude);  // 创建指定点坐标

            /* 动态改变地图的放大倍数 */
            map.setZoom(13);

            /* 显示标注点 */
            var markerFixed = new BMap.Marker(pointFixed);  // 创建指定点的标注
            map.addOverlay(markerFixed);              // 将标注添加到地图中
            var label = new BMap.Label("指定点", { offset: new BMap.Size(20, -10) }); // 文字标签
            markerFixed.setLabel(label);
        }
    }
};
