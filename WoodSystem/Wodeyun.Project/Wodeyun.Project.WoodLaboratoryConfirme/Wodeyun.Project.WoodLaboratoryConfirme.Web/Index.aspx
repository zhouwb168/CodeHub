<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Project.WoodLaboratoryConfirme.Web.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Wodeyun Project WoodLaboratoryConfirme</title>
    <link href="/Scripts/easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="/Scripts/easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/Style.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery/jquery-1.8.0.min.js"></script>
    <script src="/Scripts/easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="/Scripts/easyui/locale/easyui-lang-zh_CN.js" type="text/javascript"></script>
    <script src="/Scripts/json/json2.js" type="text/javascript"></script>
    <script src="/Scripts/moment/moment.min.js" type="text/javascript"></script>
    <script src="/Scripts/Javascript.js"></script>
    <script src="/Scripts/Index.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <table width="100%" height="100%">
            <tr>
                <td height="28px">
                    <table style="height: 28px">
                        <tr>
                            <td id="tdDate">
                                <a href="#" class="easyui-linkbutton" plain="true"><font color="red">今天</font></a>
                                <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">昨天</font></a>
                                <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Day 2</font></a>
                                <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Day 3</font></a>
                                <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Day 4</font></a>
                                <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Day 5</font></a>
                            </td>
                            <td valign="center">
                                <input id="Date" type="text" class="easyui-datebox" style="width: 95px;" /></td>
                            <td style="width: 5px">&nbsp;</td>
                            <td>密码:</td>
                            <td>
                                <input id="txtKeyForSearch" type="text" style="width: 40px;" /></td>
                            <td style="width: 5px">&nbsp;</td>
                            <td>检验号:</td>
                            <td>
                                <input id="txtNumberForSearch" type="text" style="width: 40px;" /></td>
                            <td style="width: 5px">&nbsp;</td>
                            <td>已审核:</td>
                            <td>
                                <select id="sltConfirme" class="easyui-combobox">
                                    <option value="-1">全部</option>
                                    <option value="0">否</option>
                                    <option value="1">是</option>
                                </select>
                            </td>
                            <td style="width: 5px">&nbsp;</td>
                            <td>
                                <img id="Query" class="Button" style="display: normal" src="/Images/Query.gif" /></td>
                            <td style="width: 5px">&nbsp;</td>
                            <td><a id="aConfirme" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">批量审核通过</a></td>
                            <td style="width: 5px">&nbsp;</td>
                            <td id="checkTool"><a id="aBackConfirme" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">反审核</a></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:false,rownumbers:false,pagination:true,pageSize:4,pageList:[5,100,Setter.Max],idField:'Unique'">
                        <thead>
                            <tr>
                                <th data-options="checkbox:true,field:'Co20'" rowspan="2">全选</th>
                                <th data-options="field:'Col1'" width="55px" rowspan="2">日期</th>
                                <th data-options="field:'Col2'" width="64px" rowspan="2">车号</th>
                                <th data-options="field:'Col3'" width="35px" rowspan="2">密码</th>
                                <th data-options="field:'Col4'" width="44px" rowspan="2">木种</th>
                                <th data-options="field:'Col5'" width="35px" rowspan="2">净重</th>
                                <th data-options="field:'Col6'" width="47px" rowspan="2">过磅员</th>
                                <th data-options="field:'Col7'" width="43px" rowspan="2">检验号</th>
                                <th colspan="4">目测</th>
                                <th colspan="2">对比分析</th>
                                <th data-options="field:'Co26'" width="75px" rowspan="2">树皮碎料扣方</th>
                                <th data-options="field:'Co21'" width="45px" rowspan="2">已审核</th>
                                <th data-options="field:'Co22'" width="49px" rowspan="2">腐 朽</th>
                                <th data-options="field:'Co23'" width="55px" rowspan="2">过大木片</th>
                                <th data-options="field:'Co24'" width="55px" rowspan="2">过小木片</th>
                                <th data-options="field:'Co25'" width="32px" rowspan="2">编辑</th>
                            </tr>
                            <tr>
                                <th data-options="field:'Water'" width="49px">含水率</th>
                                <th data-options="field:'Skin'" width="55px">树皮含量</th>
                                <th data-options="field:'Scrap'" width="55px">碎料含量</th>
                                <th data-options="field:'User'" width="55px">目测人</th>
                                <th data-options="field:'Col8'" width="55px" align="center">取样人</th>
                                <th data-options="field:'Col9'" width="147px" align="center">备注</th>
                            </tr>
                        </thead>
                    </table>
                </td>
            </tr>
        </table>
    </form>
    <!-- datagrid工具条结束 -->
    <!-- dialog主体开始 -->
    <div id="dd" class="easyui-dialog" style="width: 600px; height: 320px" buttons="#dlgButtons" data-options="modal: true,closed: true,title: '&nbsp;修改',iconCls: 'icon-edit'">
        <table id="tblIpuntsGroup" style="margin: 10px">
            <tr>
                <td class="Td">原始水份：</td>
                <td class="Td">
                    <input id="txtWater" style="width: 150px;" type="text" class="easyui-validatebox" data-options="required:true" />
                    %</td>
                <td class="Td">折后水份：</td>
                <td class="Td">
                    <input disabled="disabled" id="txtRebateWater" style="width: 150px;" type="text" />
                    %</td>
            </tr>
            <tr>
                <td class="Td">原始树皮：</td>
                <td class="Td">
                    <input id="txtSkin" type="text" class="easyui-validatebox" style="width: 150px;" data-options="required:true" />
                    %</td>
                <td class="Td">折后树皮：</td>
                <td class="Td">
                    <input disabled="disabled" id="txtRebateSkin" style="width: 150px;" type="text" class="easyui-validatebox" data-options="required:true" />
                    %</td>
            </tr>
            <tr>
                <td class="Td">原始碎料：</td>
                <td class="Td">
                    <input id="txtScrap" type="text" class="easyui-validatebox" style="width: 150px;" data-options="required:true" />
                    %</td>
                <td class="Td">折后碎料：</td>
                <td class="Td">
                    <input disabled="disabled" id="txtRebateScrap" style="width: 150px;" type="text" class="easyui-validatebox" data-options="required:true" />
                    %</td>
            </tr>
            <tr>
                <td class="Td">过大木片：</td>
                <td class="Td">
                    <input id="txtGreater" type="text" style="width: 150px;" />
                    %</td>
                <td class="Td">拆后过大木片：</td>
                <td class="Td">
                    <input disabled="disabled" id="txtRebateGreater" type="text" style="width: 150px;" />
                    %</td>
            </tr>
            <tr>
                <td class="Td">腐&nbsp;&nbsp;&nbsp;&nbsp;朽：</td>
                <td class="Td">
                    <input id="txtBad" style="width: 150px;" type="text" />
                    %</td>
                <td class="Td">树皮碎料扣除：</td>
                <td class="Td"><input disabled="disabled" id="txtDeductVolume" type="text" style="width:150px;" /> m³</td>
            </tr>
            <tr>
                <td class="Td">过小木片：</td>
                <td class="Td">
                    <input id="txtLess" type="text" style="width: 150px;" />
                    %</td>
                <td class="Td">&nbsp;</td>
                <td class="Td">
                    <input id="txtUnique" type="hidden" title="序号" value="0" /></td>
            </tr>
            <tr>
                <td class="Td" colspan="4"><b style="color: red">提示：折后数据系统会自动计算，只需输入原始数据保存即可.</b></td>

            </tr>
        </table>
    </div>
    <!-- dialog主体结束 -->
    <!-- dialog按钮栏开始 -->
    <div id="dlgButtons">
        <table>
            <tr>
                <td>
                    <img id="imgSave" class="Button" src="Images/Save.gif" alt="" /></td>
                <td style="width: 10px">&nbsp;</td>
                <td>
                    <img id="imgCancel" class="Button" src="Images/Cancel.gif" alt="" /></td>
                <td style="width: 5px">&nbsp;</td>
            </tr>
        </table>
    </div>
    <!-- dialog按钮栏结束 -->
    <input id="txtRemarkOfWood" type="hidden" value="" />
    <input id="txtTreeOfWood" type="hidden" value="" />
    <input id="txtjweight" type="hidden" value="" />
</body>
</html>
