$(function () {
    $('#Button').bind('click', OnClick);

    Eventer.ComboBox($('#Link'), 'Link', 'GetEntities', []);
});

function OnClick() {
    if (Checker.Valid($('#Username'), '请输入用户名！') == false) return;
    if (Checker.Valid($('#Password'), '请输入密码！') == false) return;
    
    var data = {
        service: 'Verificate',
        method: 'GetEntityByLinkAndUsernameAndPassword',
        args: [parseInt(Eventer.Get($('#Link'))), Eventer.Get($('#Username')).toLowerCase(), Eventer.Get($('#Password'))]
    };
    var success = function (result) {
        var root = Ajaxer.GetRoot(result);
        
        if (root.Success == false) {
            alert(root.Message);
            
            if (root.Value == 'Id' || root.Value == 'Link') {
                Eventer.Set($('#Username'));
                Eventer.Set($('#Password'));
                $('#Username').focus();
            } else {
                Eventer.Set($('#Password'));
                $('#Password').focus();
            }

            return;
        }
        
        window.location.href = '/Index.aspx?Token=' + root.Value.Token + '&Date=' + (new Date()).getTime();
    };

    Ajaxer.Ajax(Setter.Url, data, success);
}

var Events = {
    GetRootPath: function () {
        var strFullPath = window.document.location.href;
        var strPath = window.document.location.pathname;
        var pos = strFullPath.indexOf(strPath);
        var prePath = strFullPath.substring(0, pos);
        var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);

        return (prePath + postPath);
    }
};
