jQuery.support.cors = true;

var Ajaxer = {
    Ajax: function (url, data, success) {
        $.ajax({
            type: 'post',
            contentType: 'application/json; charset=utf-8',
            url: url,
            data: JSON.stringify(data),
            success: success,
            error: this.OnError
        });
    },

    GetRoot: function (result) {
        for (var root in result) {
            return result[root];
        }
    },

    GetItems: function (result) {
        return $.parseJSON(result.Items);
    },

    GetTree: function (result) {
        function GetChildren(value) {
            var results = [];
            var items = $.parseJSON(value.Items);

            for (var i = 0; i < items.length; i++) {
                var item = {
                    id: items[i].id,
                    text: items[i].text,
                    children: GetChildren(items[i].children)
                };

                results.push(item);
            }

            return results;
        }
        
        return GetChildren(result)
    },

    GetGrid: function (result) {
        var grid = {};
        grid.total = result.Total;
        grid.rows = $.parseJSON(result.Items);

        return grid;
    },

    OnError: function (error) {
        alert('系统错误，请与系统管理员联系！\n' + error.statusText);
    }
};

var Checker = {
    Valid: function (control, message) {
        if (control.validatebox('isValid') == false) {
            alert(message);
            control.focus();
            return false;
        }
        if ($.trim(control.val()) == '') {
            alert(message);
            control.select();
            return false;
        }

        return true;
    },

    IsDate: function (control, message) {
        if (moment(control.datetimebox('getValue')).isValid() == false) {
            alert(message);
            return false;
        }

        return true;
    }
};

var Eventer = {
    Grid: function (grid, table, method, args, callback) {
        var data = {
            service: table,
            method: method,
            args: args
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }

            var items = Ajaxer.GetGrid(root);

            if (grid.attr('class').indexOf('easyui-datagrid') != -1) grid.datagrid('loadData', items);
            if (grid.attr('class').indexOf('easyui-treegrid') != -1) grid.treegrid('loadData', items);

            if (callback != null) callback();
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    ComboBox: function (combobox, table, method, args, callback) {
        var data = {
            service: table,
            method: method,
            args: args
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }

            var items = Ajaxer.GetItems(root);

            combobox.combobox('loadData', items);
            if (items.length != 0) combobox.combobox('setValue', items[0].Unique);

            if (callback != null) callback();
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    Click: function (grid, buttons, controls) {
        var row;

        if (grid.attr('class').indexOf('easyui-datagrid') != -1) row = grid.datagrid('getSelected');
        if (grid.attr('class').indexOf('easyui-treegrid') != -1) row = grid.treegrid('getSelected');

        if (row.Unique > Setter.Max) {
            //buttons.Clear();
            //controls.Disabled();
            //controls.Clear();
            Eventer.Create(buttons, controls);
            return;
        }

        //buttons.Update();
        //controls.Enabled();
        controls.Set();
    },

    Page: function (grid, refresh) {
        var options;
        var pager;

        if (grid.attr('class').indexOf('easyui-datagrid') != -1) {
            options = grid.datagrid('options');
            pager = grid.datagrid('getPager');
        }
        if (grid.attr('class').indexOf('easyui-treegrid') != -1) {
            options = grid.treegrid('options');
            pager = grid.treegrid('getPager');
        }

        pager.pagination({
            onSelectPage: function (pageNumber, pageSize) {
                options.pageNumber = pageNumber;
                options.pageSize = pageSize;
                pager.pagination('refresh', {
                    pageNumber: pageNumber,
                    pageSize: pageSize
                });

                refresh();
            }
        });
    },

    Create: function (buttons, controls) {
        buttons.Create();
        controls.Enabled();
        controls.Clear();
    },

    Delete: function (param, table, refresh, buttons, controls) {
        if (confirm("确定要删除吗？") == false) return;

        var data = {
            service: table,
            method: 'DeleteEntityByUnique',
            args: [parseInt(unique.val())]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }

            buttons.Delete();
            controls.Disabled();
            controls.Clear();

            alert(root.Message);

            refresh();
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    Save: function (unique, table, refresh, buttons, controls) {
        var entity = controls.Get();
        var data = {
            service: table,
            method: 'SaveEntityByOperator',
            args: [JSON.stringify(entity)]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }

            alert(root.Message);

            //buttons.Save();
            controls.Clear();
            //Eventer.Create(buttons, controls);
            //unique.val(root.Value);

            refresh();
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    Cancel: function (buttons, controls) {
        buttons.Clear();
        controls.Disabled();
    },

    Get: function (control, method) {
        if (control.attr('class') == null) return control.val();

        if (control.attr('class').indexOf('easyui-combobox') != -1) {
            if (method == null) method = 'getValue';
            return control.combobox(method);
        }
        else if (control.attr('class').indexOf('easyui-combotree') != -1) {
            if (method == null) method = 'getValue';
            return control.combotree(method);
        }
        else if (control.attr('class').indexOf('easyui-datebox') != -1) return control.datebox('getValue');
        else if (control.attr('class').indexOf('easyui-datetimebox') != -1) return control.datetimebox('getValue');
        else if (control.attr('class').indexOf('easyui-timespinner') != -1) return control.timespinner('getValue');
        else return control.val();
    },

    Set: function (control, value, method, formatter) {
        if (control.attr('class') == null) {
            if (value == null) control.val(''); else control.val(value);
            return;
        }

        if (control.attr('class').indexOf('easyui-combobox') != -1) {
            if (value == null) {
                var items = control.combobox('getData');
                if (items.length != 0) control.combobox('select', (items[0].Unique != null ? items[0].Unique : items[0].value));
            } else {
                if (method == null) method = 'select';
                control.combobox(method, value);
            }
        }
        else if (control.attr('class').indexOf('easyui-combotree') != -1) {
            if (value == null) {
                var items = control.combotree('options').data;
                if (items.length != 0) control.combotree('setValue', items[0].id);
            } else {
                if (method == null) method = 'setValue';
                control.combotree(method, value);
            }
        }
        else if (control.attr('class').indexOf('easyui-datebox') != -1) {
            if (value == null) control.val(''); else control.val(formatter(value));
        }
        else if (control.attr('class').indexOf('easyui-datetimebox') != -1) {
            if (value == null) control.val(''); else control.val(formatter(value));
        }
        else if (control.attr('class').indexOf('easyui-timespinner') != -1) {
            if (value == null) control.val(''); else control.val(formatter(value));
        }
        else control.val(value);
    },

    Enable: function (control) {
        if (control.attr('class') == null) {
            control.removeAttr('disabled');
            return;
        }

        if (control.attr('class').indexOf('easyui-combobox') != -1) control.combobox('enable');
        else if (control.attr('class').indexOf('easyui-combotree') != -1) control.combotree('enable');
        else if (control.attr('class').indexOf('easyui-datebox') != -1) control.datebox('enable');
        else if (control.attr('class').indexOf('easyui-datetimebox') != -1) control.datetimebox('enable');
        else if (control.attr('class').indexOf('easyui-timespinner') != -1) control.timespinner('enable');
        else control.removeAttr('disabled');
    },

    Disable: function (control) {
        if (control.attr('class') == null) {
            control.attr('disabled', 'disabled');
            return;
        }

        if (control.attr('class').indexOf('easyui-combobox') != -1) control.combobox('disable');
        else if (control.attr('class').indexOf('easyui-combotree') != -1) control.combotree('disable');
        else if (control.attr('class').indexOf('easyui-datebox') != -1) control.datebox('disable');
        else if (control.attr('class').indexOf('easyui-datetimebox') != -1) control.datetimebox('disable');
        else if (control.attr('class').indexOf('easyui-timespinner') != -1) control.timespinner('disable');
        else control.attr('disabled', 'disabled');
    },

    Show: function (control, unique) {
        control.find('[unique="' + unique + '"]').attr('style', 'display:normal');
    },

    Hide: function (control, unique) {
        control.find('[unique="' + unique + '"]').attr('style', 'display:none');
    }
};