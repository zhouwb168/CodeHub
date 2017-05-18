<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdvancedSearch.aspx.cs" Inherits="Wodeyun.Project.WoodReport.Web.AdvancedSearch" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Wodeyun Project WoodReport</title>
    <link href="/Scripts/easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="/Scripts/easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/Style.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery/jquery-1.8.0.min.js"></script>
    <script src="/Scripts/easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="/Scripts/easyui/locale/easyui-lang-zh_CN.js" type="text/javascript"></script>
    <script src="/Scripts/json/json2.js" type="text/javascript"></script>
    <script src="/Scripts/moment/moment.min.js" type="text/javascript"></script>
    <script src="/Scripts/Javascript.js"></script>
    <script src="/Scripts/AdvancedSearch.js"></script>
</head>
<body>
    <form>
        <table width="100%" height="100%">
            <tr>
                <td width="100%" width="900px" style="vertical-align: top">
                    <table  width="900px">
                        <tr>
                            <td>过磅时间：从</td>
                            <td colspan="2"><input id="txtStartDate" type="text" class="easyui-datebox" style="width: 100px;" /> 到 <input id="txtEndDate" type="text" class="easyui-datebox" style="width: 100px;" /></td>
                            <td colspan="3"><b style="color:red;">说明：多个车号、送货员、卸车员、来源地以英文的分号(;)分开.</b></td>
                        </tr>
                        <tr>
                            <td>车号：</td>
                            <td colspan="2"><input id="txtLicense" type="text" style="width: 250px;" title="多个车号以英文的分号(;)分开." /><input id="chkLicense" type="checkbox" />统计</td>
                            <td style="width: 10px">&nbsp;</td>
                            <td>卸车员：</td>
                            <td><input id="txtPoundSupplier" type="text" style="width: 250px;" title="多个卸车员以英文的分号(;)分开." /><input id="chkPoundSupplier" type="checkbox" />统计</td>
                        </tr>
                        <tr>
                            <td>送货员：</td>
                            <td colspan="2"><input id="txtDriver" type="text" style="width: 250px;" title="多个送货员以英文的分号(;)分开." /><input id="chkDriver" type="checkbox" />统计</td>
                            <td style="width: 10px">&nbsp;</td>
                            <td>来源地：</td>
                            <td ><input id="txtOrigin" type="text" style="width: 250px;" title="多个来源地以英文的分号(;)分开." /><input id="chkOrigin" type="checkbox" />统计</td>
                            <td colspan="2" align="left">
                                <img id="Query" class="Button" style="display: normal" src="/Images/Query.gif" />&nbsp;&nbsp;
                                <img id="Excel" class="Button" style="display:normal" src="/Images/Excel.gif" />
                            </td>
                        </tr>
                    </table>
                </td>
                </tr><tr>
                <td height="400px" class="Td" style="vertical-align: top">
                    <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:15,pageList:[15,50,100,Setter.Max],showFooter:true">
                        <thead>
                            <tr>
                                <th data-options="field:'Number'" width="35px">排名</th>
                                <th data-options="field:'License'" width="70px">车 号</th>
                                <th data-options="field:'Tree'" width="50px">品 种</th>
                                <th data-options="field:'jWeight'" width="70px" sum="true" avg="true" min="true" max="true">净重(吨)</th>
                                <th data-options="field:'jVolume'">净体积(立方米)</th>
                                <th data-options="field:'Driver'" width="50px">送货员</th>
                                <th data-options="field:'Bang_Time'" width="100px">过磅时间</th>
                                <th data-options="field:'BackWeighTime'" width="100px">回皮时间</th>
                                <th data-options="field:'PoundSupplier'" width="50px">卸车员</th>
                                <th data-options="field:'Date'" width="100px">报备时间</th>
                                <th data-options="field:'Ship'" width="100px">发车日期</th>
                                <th data-options="field:'Origin'" width="70px">来源地</th>
                                <th data-options="field:'IsAdd'" width="50px">补报</th>
                                <th data-options="field:'Text'" width="200px">短信内容</th>
                            </tr>
                        </thead>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
