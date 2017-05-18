$(function () {
    var td0fButtons = $('#Buttons');
    td0fButtons.find('[unique="Report01"]').bind('click', { unique: 'Report01' }, Events.OnClick);
    td0fButtons.find('[unique="Report02"]').bind('click', { unique: 'Report02' }, Events.OnClick);

    $('#Frame').attr('src', '/Report01.aspx?Token=' + window.location.search.split('=')[1].replace('&Date', '') + '&Date=' + (new Date()).getTime());
});

var Events = {
    OnClick: function (event) {
        $('#Frame').attr('src', '/' + event.data.unique + '.aspx?Token=' + window.location.search.split('=')[1].replace('&Date', '') + '&Date=' + (new Date()).getTime());

        var aLinks = $("#Buttons").find('a');
        var aLinkCount = aLinks.length;
        for (var i = 0; i < aLinkCount; i++) {
            aLinks.eq(i).find('font').attr('color', (event.data.unique == aLinks.eq(i).attr('unique') ? 'red' : 'normal'));
        }
    }
};
