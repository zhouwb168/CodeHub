<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report03.aspx.cs" Inherits="Wodeyun.Project.WoodReport.Web.Report03" %>

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
    <script src="/Scripts/Report03.js"></script>
</head>
<body>
    <form>
        <table width="100%" height="100%">
            <tr>
                <td height="28px">
                    <table>
                        <tr>
                            <td>过磅时间从</td>
                            <td>
                                <input id="txtStartDate" type="text" class="easyui-datebox" style="width: 100px;" /></td>
                            <td style="width: 10px">到</td>
                            <td>
                                <input id="txtEndDate" type="text" class="easyui-datebox" style="width: 100px;" /></td>
                            <td style="width: 10px">&nbsp;</td>
                            <td>
                                <img id="Query" class="Button" style="display: normal" src="/Images/Query.gif" /></td>
                            <td style="width: 20px">&nbsp;</td>
                            <td>
                                <img id="Excel" class="Button" style="display: normal" src="/Images/Excel.gif" /></td>
                            <td>
                                <a href="#" id="AdvancedSearch" class="easyui-linkbutton" plain="true"><font color="normal">高级查询</font></a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:15,pageList:[15,50,100,Setter.Max]">
                        <thead frozen="true">
                            <tr>
                                <th data-options="field:'License'" width="70px">车 号</th>
                                <th data-options="field:'Tree'" width="50px">品 种</th>
                                <th data-options="field:'jWeight'" width="70px">净重(吨)</th>
                                <th data-options="field:'jVolume'" width="70px">净体积(立方米)</th>
                                <th data-options="field:'Bang_Time'" width="100px">过磅日期</th>
                                <th data-options="field:'BackWeighTime'" width="100px">回皮时间</th>
                                <th data-options="field:'PoundSupplier'" width="50px">卸车员</th>
                                <th data-options="field:'Driver'" width="50px">送货员</th>
                            </tr>
                            <thead>
                                <tr>
                                    <th data-options="field:'Date'" width="100px">报备时间</th>
                                    <th data-options="field:'Ship'" width="100px">发车日期</th>
                                    <th data-options="field:'Origin'" width="80px">来源地</th>
                                    <th data-options="field:'IsAdd'" width="50px">补报</th>
                                    <th data-options="field:'Text'" width="200px">短信内容</th>
                                </tr>
                            </thead>
                    </table>
                </td>
            </tr>
            <tr style="display: none">
                <td>
                    <table id="gridexcel" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:15,pageList:[15,50,100,Setter.Max]">
                        <thead>
                            <tr>
                                <th data-options="field:'License'" width="70px">车 号</th>
                                <th data-options="field:'Tree'" width="50px">品 种</th>
                                <th data-options="field:'jWeight'" width="70px">净重(吨)</th>
                                <th data-options="field:'jVolume'" width="70px">净体积(立方米)</th>
                                <th data-options="field:'Bang_Time'" width="100px">过磅日期</th>
                                <th data-options="field:'BackWeighTime'" width="100px">回皮时间</th>
                                <th data-options="field:'PoundSupplier'" width="50px">卸车员</th>
                                <th data-options="field:'Driver'" width="50px">送货员</th>
                                <th data-options="field:'Date'" width="100px">报备时间</th>
                                <th data-options="field:'Ship'" width="100px">发车日期</th>
                                <th data-options="field:'Origin'" width="80px">来源地</th>
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
