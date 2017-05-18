$(function () {
    $('#aLangFang').bind('click', Events.LangFang);
});

var Events = {
    Compled: function () {
        /* 发送量方指令 */
        var uniques = "liangfang";
        var entity = {
            Command: 'Compled'
        };
        var second = 180;
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
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    LangFang: function () {
        Eventer.Set($('#txtFullVolume'));

        /* 发送量方指令 */
        var uniques = "liangfang";
        var entity = {
            Command: 'Start'
        };
        var second = 180;
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

            if (root.Volume != null) Eventer.Set($('#txtFullVolume'), root.Volume);
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    }
};