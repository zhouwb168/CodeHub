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

        controls.Clear();
        buttons.Update();
        controls.Enabled();
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

    Delete: function (unique, table, refresh, buttons, controls, record, operator, greenCardNumber, redCardNumber) {
        if (confirm("是否确定要删除该记录？") == false) return;

        var data = {
            service: table,
            method: 'DeleteEntityByUniqueWithOperator',
            args: [parseInt(unique.val()), parseInt(record.val()), parseInt(operator), greenCardNumber, redCardNumber]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }

            //buttons.Delete();
            //controls.Disabled();
            controls.Clear();
            buttons.Create();

            alert(root.Message);

            refresh();
        };

        Ajaxer.Ajax(Setter.Url, data, success);
    },

    Save: function (unique, table, refresh, buttons, controls, cardNumber) {
        var entity = controls.Get();
        var data = {
            service: table,
            method: 'SaveEntityByOperator',
            args: [JSON.stringify(entity), cardNumber]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);

            if (root.Success == false) {
                alert(root.Message);
                return;
            }
            alert(root.Message);
            //打印磅单
            //Eventer.OnNowPrint(entity, weight_time);
            //buttons.Save();
            //controls.Disabled();
            //unique.val(root.Value);
            controls.Clear();
            buttons.Create();

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
    },
    //保存即打印
    OnNowPrint: function (entity, weight_time) {
        if (!confirm("是否打印磅单？")) return;
        var WeightTime = "", license = "", tree = "", fullvolume = 0, supplier = "", driver = "";
        WeightTime = weight_time == "" ? Eventer.ToDateTime(entity.WeightTime) : weight_time;
        license = entity.License;
        tree = entity.Tree;
        fullvolume = entity.FullVolume;
        supplier = entity.Supplier;
        driver = entity.Driver;

        var printForm = document.createElement("form");
        document.body.appendChild(printForm);
        printForm.method = "get";
        printForm.action = "Print.html";
        printForm.target = "_blank";
        var txtBox = document.createElement("input");
        txtBox.id = "params";
        txtBox.name = "params";
        txtBox.type = "text";
        txtBox.value = escape(WeightTime + "|" + license + "|" + tree + "|" + fullvolume + "|" + supplier);
        printForm.appendChild(txtBox);
        printForm.submit();
        document.body.removeChild(printForm);
    },
    //量方数据同步到工业互联网
    OnSyncFullVolumeData: function (table, StartTime, EndTime, CarID) {
        if (confirm("确定要将量方数据同步到工业互联网？") == false) return;
        $("#SyncFullVolumeData").attr('disabled', 'disabled');
        var data = {
            service: table,
            method: 'BatchSyncFullVolumeData',
            args: [StartTime, EndTime, CarID]
        };
        var success = function (result) {
            var root = Ajaxer.GetRoot(result);
            $("#SyncFullVolumeData").removeAttr('disabled');
            if (root.Success == false) {
                alert(root.Message);
                return;
            }
            alert(root.Message);
        };
        Ajaxer.Ajax(Setter.Url, data, success);
    },
    ToDateTime: function (value) {
        return moment(value).format('YYYY-MM-DD HH:mm');
    }
};