<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WoodPriceList.aspx.cs" Inherits="Wodeyun.Project.WoodPrice.Web.WoodPriceList" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Wodeyun Project WoodPrice</title>
    <link href="/Scripts/easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="/Scripts/easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/Style.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery/jquery-1.8.0.min.js"></script>
    <script src="/Scripts/easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="/Scripts/easyui/locale/easyui-lang-zh_CN.js" type="text/javascript"></script>
    <script src="/Scripts/json/json2.js" type="text/javascript"></script>
    <script src="/Scripts/moment/moment.min.js" type="text/javascript"></script>
    <script src="/Scripts/Javascript.js"></script>
    <script src="/Scripts/WoodPriceList.js"></script>
</head>
<body>
    <form>
        <table width="100%" height="100%">
            <tr>
                <td height="28px">
                    <table>
                        <tr>
                            <td>过磅日期：</td>
                            <td>
                                <input id="txtStartDate" type="text" class="easyui-datebox" style="width: 95px;" /></td>
                            <td style="width: 2px">到</td>
                            <td>
                                <input id="txtEndDate" type="text" class="easyui-datebox" style="width: 95px;" /></td>
                            <td style="width: 2px">&nbsp;</td>
                            <td>区域：</td>
                            <td class="Td-Combo">
                                <select id="Area" class="easyui-combobox" data-options="valueField:'Unique',textField:'Name'" style="width: 80px"></select>
                            </td>
                            <td>树种：</td>
                            <td class="Td-Combo">
                                <select id="Tree" class="easyui-combobox" data-options="valueField:'Unique',textField:'Name'" style="width: 80px"></select>
                            </td>
                            <td>卸货员：</td>
                            <td>
                                <input id="txtSupplier" type="text" style="width: 50px;" />
                            </td>
                            <td>已生成:</td>
                            <td>
                                <select id="sltCreate" class="easyui-combobox">
                                    <option value="">全部</option>
                                    <option value="已生成">已生成</option>
                                    <option value="未生成">未生成</option>
                                </select>
                            </td>
                            <td>
                                <img id="Query" class="Button" style="display: normal" src="/Images/Query.gif" /></td>
                            <td style="width: 10px">&nbsp;</td>
                            <td>
                                <img id="Excel" class="Button" style="display: normal" src="/Images/Excel.gif" /></td>
                            <td style="width: 10px">&nbsp;</td>
                            <td><a id="btnCreate" class="easyui-linkbutton" href="javascript:;">生成结算单</a></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:false,rownumbers:false,pagination:true,pageSize:15,pageList:[15,50,100,Setter.Max],showFooter:true">
                        <thead>
                            <tr>
                                <th data-options="checkbox:true"></th>
                                <th data-options="field:'Bang_Time'" width="100px">首磅日期</th>
                                <th data-options="field:'License'" width="60px">车号</th>
                                <th data-options="field:'LinkMan'" width="60px">名称</th>
                                <th data-options="field:'Supplier'" width="40px">卸货员</th>
                                <th data-options="field:'Area'" width="60px">区域</th>
                                <th data-options="field:'Tree'" width="60px">树种</th>
                                <th data-options="field:'jWeight'" width="70px">重量(湿吨)</th>
                                <th data-options="field:'GWeight'" width="70px">重量(干吨)</th>
                                <th data-options="field:'Price'" width="60px">单价(干吨)</th>
                                <th data-options="field:'GweightPrice'" width="70px">金额(干吨)</th>
                                <th data-options="field:'FullVolume'" width="90px">首磅体积(立方)</th>
                                <th data-options="field:'JVolume'" width="80px">净体积(立方)</th>
                                <th data-options="field:'CubePrice'" width="60px">单价(立方)</th>
                                <th data-options="field:'VolumePrice'" width="70px">金额(立方)</th>
                                <th data-options="field:'IsCreate'" width="45px">结算单</th>
                            </tr>
                        </thead>
                    </table>
                </td>
            </tr>
        </table>
    </form>
    <!-- dialog主体开始 -->
    <div id="dd" class="easyui-dialog" style="width: 730px; height: 300px" buttons="#dlgButtons" data-options="modal: true,closed: true,title: '&nbsp;结算单',iconCls: 'icon-edit'">
        <table id="gridCost" class="easyui-datagrid" fit="true" data-options="singleSelect:false,rownumbers:false,pagination:false,pageSize:15,pageList:[15,50,100,Setter.Max],idField:'OrderNo',showFooter:true">
            <thead>
                <tr>
                    <th data-options="checkbox:true"></th>
                    <%--<th data-options="field:'IsPrint'" width="45px">已打印</th>--%>
                    <th data-options="field:'Bang_Time'" width="100px">磅单日期</th>
                    <th data-options="field:'License'" width="60px">车牌号</th>
                    <th data-options="field:'Area'" width="60px">区域</th>
                    <th data-options="field:'Tree'" width="60px">品种</th>
                    <th data-options="field:'Supplier'" width="40px">卸货员</th>
                    <th data-options="field:'JWeight'" width="70px">净重(湿吨)</th>
                    <th data-options="field:'GWeight'" width="70px">净重(干吨)</th>
                    <th data-options="field:'GPrice'" width="60px">单价(干吨)</th>
                    <th data-options="field:'Amount'" width="70px">金额(干吨)</th>
                    <th data-options="field:'FullVolume'" width="90px">首磅体积(立方)</th>
                    <th data-options="field:'JVolume'" width="80px">净体积(立方)</th>
                    <th data-options="field:'CubePrice'" width="60px">单价(立方)</th>
                    <th data-options="field:'VolumePrice'" width="70px">金额(立方)</th>
                    <th data-options="field:'OperatorDate'" width="100px">结算日期</th>
                    <%--<th data-options="field:'GroupID'" width="100px">结算编号</th>--%>
                    <%-- <th data-options="field:'IsConfirmed'" width="50px">已审核</th>--%>
                    <%--<th data-options="field:'modify'" width="50px">编辑</th>--%>
                </tr>
            </thead>
        </table>
    </div>
    <!-- dialog主体结束 -->
    <!-- dialog按钮栏开始 -->
    <div id="dlgButtons">
        <table style="width:100%">
            <tr>
                <td>单号：</td>
                <td>
                    <input id="txtGroupID" type="text" style="width: 110px;" disabled /></td>
                <td>车数：</td>
                <td>
                    <input id="txtCarCount" type="text" style="width: 60px;" disabled /></td>
                <td>
                    <img id="Print" class="Button" style="display: normal" src="/Images/Print.png" /></td>
            </tr>
        </table>
    </div>
    <!-- dialog按钮栏结束 -->
    <!-- dialog主体开始 -->
    <div id="Div1" class="easyui-dialog" style="width: 300px; height: 240px" buttons="#Div2" data-options="modal: true,closed: true,title: '&nbsp;修改',iconCls: 'icon-edit'">
        <table id="tblIpuntsGroup" style="margin: 10px">
            <tr>
                <td class="Td">车牌号：</td>
                <td class="Td">
                    <input id="txtLicense1" style="width: 150px;" disabled type="text" class="easyui-validatebox" data-options="required:true" />
                </td>
            </tr>
            <tr>
                <td class="Td">磅单日期：</td>
                <td class="Td">
                    <input id="txtWeightTime" style="width: 150px;" disabled type="text" class="easyui-validatebox" data-options="required:true" />
                </td>
            </tr>
            <tr>
                <td class="Td">当前区域：</td>
                <td class="Td">
                    <input id="txtArea" style="width: 150px;" disabled type="text" class="easyui-validatebox" data-options="required:true" />
                </td>
            </tr>
            <tr>
                <td class="Td">修改区域：</td>
                <td class="Td-Combo">
                    <select id="AreaID" class="easyui-combobox" data-options="valueField:'Unique',textField:'Name'" style="width: 150px"></select></td>
            </tr>
        </table>
    </div>
    <!-- dialog主体结束 -->
    <!-- dialog按钮栏开始 -->
    <div id="Div2">
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
    <input id="hf_OrderNo" type="hidden" />
    <input id="hf_tree" type="hidden" />
</body>
</html>
