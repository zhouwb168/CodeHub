$(function () {
    $('#Grid').treegrid({ onClickRow: Events.OnClick });

    Events.Refresh();
});

var Events = {
    Table: 'Act',

    Refresh: function () {
        Eventer.Grid($('#Grid'), Events.Table, 'GetGridByMethodAndParent', ['GetEntitiesWithFunctionByAccount', [Account], 'Parent']);
    },

    OnClick: function () {
        var row = $('#Grid').datagrid('getSelected');
        if (row.Unique > Setter.Max) return;
        $('#Frame').attr('src', row.Url.replace('{0}', Address) + '?Token=' + window.location.search.split('=')[1].replace('&Date', '') + '&Date=' + (new Date()).getTime());
    }
};
