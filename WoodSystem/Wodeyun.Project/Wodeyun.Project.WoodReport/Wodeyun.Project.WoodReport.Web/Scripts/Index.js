$(function () {
    var tdOfButtons = $('#Buttons');
    tdOfButtons.find('[unique="Report01"]').bind('click', { unique: 'Report01' }, Events.OnClick);
    tdOfButtons.find('[unique="Report02"]').bind('click', { unique: 'Report02' }, Events.OnClick);
    tdOfButtons.find('[unique="Report03"]').bind('click', { unique: 'Report03' }, Events.OnClick);
    tdOfButtons.find('[unique="Report04"]').bind('click', { unique: 'Report04' }, Events.OnClick);
    tdOfButtons.find('[unique="Report05"]').bind('click', { unique: 'Report05' }, Events.OnClick);
    tdOfButtons.find('[unique="Report06"]').bind('click', { unique: 'Report06' }, Events.OnClick);
    tdOfButtons.find('[unique="WoodAreaInFactory"]').bind('click', { unique: 'WoodAreaInFactory' }, Events.OnClick);
    tdOfButtons.find('[unique="WoodCropInFactory"]').bind('click', { unique: 'WoodCropInFactory' }, Events.OnClick);
    $('#Frame').attr('src', '/Report01.aspx?Token=' + window.location.search.split('=')[1].replace('&Date', '') + '&Date=' + (new Date()).getTime());
});

var Events = {
    OnClick: function (event) {
        $('#Frame').attr('src', '/' + event.data.unique + '.aspx?Token=' + window.location.search.split('=')[1].replace('&Date', '') + '&Date=' + (new Date()).getTime());
        var aLinks = $("#Buttons").find('a');
        var linkCount = aLinks.length;
        for (var i = 0; i < linkCount; i++) {
            aLinks.eq(i).find('font').attr('color', (event.data.unique == aLinks.eq(i).attr('unique') ? 'red' : 'normal'));
        }
    }
};
