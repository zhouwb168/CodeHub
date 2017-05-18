$(function () {
    var td0fButtons = $('#Buttons');
    td0fButtons.find('[unique="WoodPriceList"]').bind('click', { unique: 'WoodPriceList' }, Events.OnClick);
    td0fButtons.find('[unique="CostList"]').bind('click', { unique: 'CostList' }, Events.OnClick);
    td0fButtons.find('[unique="setWoodPrice"]').bind('click', { unique: 'setWoodPrice' }, Events.OnClick);

    $('#Frame').attr('src', '/WoodPriceList.aspx?Token=' + window.location.search.split('=')[1].replace('&Date', '') + '&Date=' + (new Date()).getTime());
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
