$(function () {
    var grid = $('#Grid');
    grid.datagrid({ onClickRow: Events.OnGridClick });
    Eventer.Page(grid, Events.Page); // 注意！分页事件不能在onClickRow事件之前进行初始化
    grid.datagrid("getColumnOption", "WeighTime").formatter = Events.ToDate;
    grid.datagrid("getColumnOption", "UnPackTime").formatter = Events.ToDateTime;

    var tdOfButtons = $('#Buttons');
    tdOfButtons.find('[unique="Delete"]').bind('click', Events.OnDelete);
    tdOfButtons.find('[unique="Save"]').bind('click', Events.OnSave);
    
    var aLinks = $('#tdDate').find('a');
    var linkCount = aLinks.length;
    for (var i = 0; i < linkCount; i++) {
        aLinks.eq(i).bind('click', { index: i }, Events.OnClick);
    }
    for (var i = 2; i < linkCount; i++) {
        aLinks.eq(i).find('font').html(moment().add('days', -i).format('DD') + '日');
    }

    $('#Query').bind('click', Events.OnQuery);

    $('#Date').datebox('setValue', moment().add('days', -12).format('YYYY-MM-DD'));

    Events.GetGrid(moment().format('YYYY-MM-DD'), 1, $('#Grid').datagrid('options').pageSize, null);
});
var Events = {
    Table: 'WoodUnPackBox',

    ResetPager: function () {
        var grid = $('#Grid');
        grid.datagrid('options').pageNumber = 1;
        grid.datagrid('getPager').pagination('refresh', {
            pageNumber: 1
        });
    },

    Trim: function (s) {
        return s.replace(/(^\s*)|(\s*$)/g, "");
    },

    ToDate: function (value) {
        if (value == null) return "";

        return moment(value).format('YYYY-MM-DD');
    },

    OnQuery: function () {
        var itemsOfAlink = $('#tdDate').find('a');
        var itemCount = itemsOfAlink.length;
        for (var i = 0; i < itemCount; i++) {
            itemsOfAlink.eq(i).find('font').attr('color', 'normal');
        }

        Events.ResetPager();

        Events.GetGrid(null);
    },

    GetDate: function () {
        var itemsOfAlink = $('#tdDate').find('a');
        var itemCount = itemsOfAlink.length;
        for (var i = 0; i < itemCount; i++) {
            if (itemsOfAlink.eq(i).find('font').attr('color') == '#ff0000') return moment().add('days', -i).format('YYYY-MM-DD');
        }

        return Eventer.Get($('#Date'));
    },

    ToDateTime: function (value) {
        if (value == null) return "";

        return moment(value).format('YYYY-MM-DD HH:mm');
    },

    Page: function () {
        Events.GetGrid(null);
    },

    GetGrid: function (date, start, length, callback) {
        var grid = $('#Grid');
        var gridOptons = grid.datagrid('options');
        var pageSize = gridOptons.pageSize;
        var rowStart = (gridOptons.pageNumber - 1) * pageSize + 1;

        var date = Events.GetDate();

        Eventer.Grid(grid, Events.Table, 'GetEntitiesByStartAndLengthWithOperator', [date, rowStart, pageSize, Account], callback);
    },

    OnClick: function (event) {
        var date = moment().add('days', -event.data.index).format('YYYY-MM-DD');
        var itemsOfAlink = $('#tdDate').find('a');
        var itemCount = itemsOfAlink.length;
        for (var i = 0; i < itemCount; i++) {
            itemsOfAlink.eq(i).find('font').attr('color', (date == moment().add('days', -i).format('YYYY-MM-DD') ? 'red' : 'normal'));
        }

        Events.ResetPager();

        Events.GetGrid(null);
    },

    OnGridClick: function () {
        Eventer.Click($('#Grid'), Buttons, Controls);
    },

    OnDelete: function () {
        Eventer.Delete($('#txtUnique'), Events.Table, Events.Page, Buttons, Controls, $('#txtRecordID'), Account);
    },

    OnSave: function () {
        if (Checker.Valid($('#txtNumber'), '检验号是必须的') == false) return;

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
        Eventer.Hide($('#Buttons'), 'Delete');
        Eventer.Hide($('#Buttons'), 'Save');

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
        Eventer.Set($('#txtPackTime'));
        Eventer.Set($('#txtNumber'));
        Eventer.Set($('#txtKey'));
        Eventer.Set($('#txtBox'));
    },

    Get: function () {
        return {
            Unique: Eventer.Get($('#txtUnique')),
            WoodID: Eventer.Get($('#txtRecordID')),
            Number: Events.Trim(Eventer.Get($('#txtNumber'))),
            Operator: Account
        };
    },

    Set: function () {
        var row = $('#Grid').datagrid('getSelected');

        if (row.Unique != null) Eventer.Set($('#txtUnique'), row.Unique);
        Eventer.Set($('#txtRecordID'), row.WoodID);
        Eventer.Set($('#txtWeighTime'), moment(row.WeighTime).format('YYYY-MM-DD'));
        Eventer.Set($('#txtBox'), row.Box);
        Eventer.Set($('#txtKey'), row.Key);
        if (row.Number != null) Eventer.Set($('#txtNumber'), row.Number);
    },

    Enabled: function () {
    },

    Disabled: function () {
    }
};
