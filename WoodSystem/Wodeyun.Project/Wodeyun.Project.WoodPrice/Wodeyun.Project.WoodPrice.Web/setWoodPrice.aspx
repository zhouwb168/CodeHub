<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="setWoodPrice.aspx.cs" Inherits="Wodeyun.Project.WoodPrice.Web.setWoodPrice" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Wodeyun Project WoodPrice</title>
    <link href="/Scripts/easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="/Scripts/easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/Style.css" rel="stylesheet" type="text/css" />
    <link href="Styles/softkey.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery/jquery-1.8.0.min.js"></script>
    <script src="/Scripts/easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="/Scripts/easyui/locale/easyui-lang-zh_CN.js" type="text/javascript"></script>
    <script src="/Scripts/json/json2.js" type="text/javascript"></script>
    <script src="/Scripts/moment/moment.min.js" type="text/javascript"></script>
    <script src="Scripts/softkeyboard.js" type="text/javascript" charset="gb2312"></script>
    <script src="/Scripts/Javascript.js"></script>
    <script src="/Scripts/setWoodPrice.js"></script>


</head>
<body>
    <form>
        <table width="100%" height="100%">
            <tr>
                <td height="28px">
                    <table>
                        <tr>
                            <td style="width: 10px">&nbsp;</td>
                            <td>执行日期从</td>
                            <td>
                                <input id="txtStartDate" type="text" class="easyui-datebox" style="width: 100px;" /></td>
                            <td style="width: 10px">到</td>
                            <td>
                                <input id="txtEndDate" type="text" class="easyui-datebox" style="width: 100px;" /></td>
                            <td style="width: 10px">&nbsp;</td>
                             <td>区域：</td>
                            <td class="Td-Combo">
                                <select id="Area" class="easyui-combobox" data-options="valueField:'Unique',textField:'Name'" style="width: 80px"></select>
                            </td>
                            <td>树种：</td>
                            <td class="Td-Combo">
                                <select id="Tree" class="easyui-combobox" data-options="valueField:'Unique',textField:'Name'" style="width: 80px"></select>
                            </td>
                            <td>
                                <img id="Query" class="Button" style="display: normal" src="/Images/Query.gif" /></td>
                            <td style="width: 10px">&nbsp;</td>
                            <td>
                                <img id="Excel" class="Button" style="display: normal" src="/Images/Excel.gif" /></td>
                            <td style="width: 10px">&nbsp;</td>
                            <td id="checkTool_23"><a id="btnCheck" class="easyui-linkbutton" href="javascript:;">审核</a></td>
                            <td style="width: 10px">&nbsp;</td>
                            <td id="checkTool_24"><a id="btnBackCheck" class="easyui-linkbutton" href="javascript:;">反审</a></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" height="100%">
                        <tr>
                            <td width="700px" class="Td" style="vertical-align: top">
                                <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,checkOnSelect: false, selectOnCheck: false,rownumbers:true,pagination:true,pageSize:20,pageList:[20,50,100,Setter.Max],idField:'Number'">
                                    <thead>
                                        <tr>
                                            <th data-options="checkbox:true"></th>
                                            <th data-options="field:'AreaName'" width="80px">区域</th>
                                            <th data-options="field:'TreeName'" width="80px">树种</th>
                                            <th data-options="field:'Price'" width="90px">价格(干吨)</th>
                                            <th data-options="field:'CubePrice'" width="90px">价格(立方)</th>
                                            <th data-options="field:'WetPrice'" width="90px">价格(湿吨)</th>
                                            <%--<th data-options="field:'Unit'" width="70px">单位</th>--%>
                                            <th data-options="field:'ExeDate'" width="80px">执行日期</th>
                                            <th data-options="field:'Version'" width="40px">版本号</th>
                                            <th data-options="field:'IsConfirmed'" width="40px">已审核</th>
                                        </tr>
                                    </thead>
                                </table>
                            </td>
                            <td width="400px" style="vertical-align: top">
                                <table width="100%">
                                    <tr>
                                        <td id="Buttons" class="Td">
                                            <img unique="Create" class="Button" src="/Images/Create.gif" />
                                            <img unique="Delete" class="Button" style="display: none" src="/Images/Delete.gif" />
                                            <img unique="Save" class="Button" style="display: none" src="/Images/Save.gif" />
                                            <img unique="Cancel" class="Button" style="display: none" src="/Images/Cancel.gif" />
                                            <img style="display: normal" src="/Images/Blank.gif" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="display: none;">
                                            <input id="Unique" type="text" title="序号" /></td>
                                    </tr>
                                </table>
                                <table width="100%">
                                    <tr>
                                        <td width="25%" class="Td">区域：</td>
                                        <td class="Td-Combo" width="75%">
                                            <select id="AreaID" class="easyui-combobox" disabled data-options="valueField:'Unique',textField:'Name'" style="width: 284px"></select></td>
                                    </tr>
                                    <tr>
                                        <td width="25%" class="Td">树种：</td>
                                        <td class="Td-Combo">
                                            <select id="TreeID" class="easyui-combobox" disabled data-options="valueField:'Unique',textField:'Name'" style="width: 284px"></select></td>
                                    </tr>
                                    <tr>
                                        <td class="Td">价格(干吨)：</td>
                                        <td class="Td">
                                            <input id="Price" type="text" class="easyui-numberbox" precision="2" disabled style="width: 100%" /></td>
                                    </tr>

                                    <tr>
                                        <td class="Td">价格(立方)：</td>
                                        <td class="Td">
                                            <input id="CubePrice" type="text" class="easyui-numberbox" precision="2" disabled style="width: 100%" /></td>
                                    </tr>
                                    <tr>
                                        <td class="Td">价格(湿吨)：</td>
                                        <td class="Td">
                                            <input id="WetPrice" type="text" class="easyui-numberbox" precision="2" disabled style="width: 100%" /></td>
                                    </tr>
                                    <tr style="display: none">
                                        <td class="Td">单位：</td>
                                        <td class="Td-Combo">
                                            <select id="Unit" class="easyui-combobox" disabled style="width: 284px">
                                                <option value="元/干吨">元/干吨</option>
                                                <option value="元/吨">元/吨</option>
                                                <option value="元/方">元/方</option>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Td">执行日期：</td>
                                        <td class="Td">
                                            <input id="ExeDate" type="text" class="easyui-datebox" style="width: 100%" data-options="required:true" /></td>
                                    </tr>
                                    <tr>
                                        <td class="Td">备注：</td>
                                        <td class="Td">
                                            <textarea id="Remark" disabled style="width: 100%; height: 100px;"></textarea></td>
                                    </tr>
                                </table>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <input id="hf_Version" type="hidden" value="0" />
    </form>
</body>
</html>
