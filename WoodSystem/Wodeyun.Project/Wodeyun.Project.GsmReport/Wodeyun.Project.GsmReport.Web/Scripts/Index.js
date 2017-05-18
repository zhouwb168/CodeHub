$(function () {
    $('#Buttons').find('[unique="Report01"]').bind('click', { unique: 'Report01' }, Events.OnClick);
    $('#Buttons').find('[unique="Report02"]').bind('click', { unique: 'Report02' }, Events.OnClick);
    $('#Buttons').find('[unique="Report03"]').bind('click', { unique: 'Report03' }, Events.OnClick);
    $('#Buttons').find('[unique="Report04"]').bind('click', { unique: 'Report04' }, Events.OnClick);
    $('#Buttons').find('[unique="Report05"]').bind('click', { unique: 'Report05' }, Events.OnClick);
    $('#Buttons').find('[unique="Report06"]').bind('click', { unique: 'Report06' }, Events.OnClick);
    $('#Buttons').find('[unique="Report07"]').bind('click', { unique: 'Report07' }, Events.OnClick);

    $('#Frame').attr('src', '/Report01.aspx?Token=' + window.location.search.split('=')[1].replace('&Date', '') + '&Date=' + (new Date()).getTime());
});

var Events = {
    OnClick: function (event) {
        $('#Frame').attr('src', '/' + event.data.unique + '.aspx?Token=' + window.location.search.split('=')[1].replace('&Date', '') + '&Date=' + (new Date()).getTime());

        for (var i = 0; i < $("#Buttons").find('a').length; i++) {
            $("#Buttons").find('a').eq(i).find('font').attr('color', (event.data.unique == $("#Buttons").find('a').eq(i).attr('unique') ? 'red' : 'normal'));
        }
    }
};
