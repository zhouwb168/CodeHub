<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report04.aspx.cs" Inherits="Wodeyun.Project.WoodDataList.Web.Report04" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Wodeyun Project WoodDataList</title>
    <link href="/Scripts/easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="/Scripts/easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/Style.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery/jquery-1.8.0.min.js"></script>
    <script src="/Scripts/easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="/Scripts/easyui/locale/easyui-lang-zh_CN.js" type="text/javascript"></script>
    <script src="/Scripts/json/json2.js" type="text/javascript"></script>
    <script src="/Scripts/moment/moment.min.js" type="text/javascript"></script>
    <script src="/Scripts/Javascript.js"></script>
    <script src="/Scripts/Report04.js"></script>
</head>
<body>
    <form>
        <table width="100%" height="100%">
            <tr>
                <td height="28px">
                    <table>
                        <tr>
                            <td>首磅时间从</td>
                            <td>
                                <input id="txtStartDate" type="text" class="easyui-datebox" style="width: 100px;" /></td>
                            <td style="width: 10px">到</td>
                            <td>
                                <input id="txtEndDate" type="text" class="easyui-datebox" style="width: 100px;" /></td>
                            <td style="width: 10px">&nbsp;</td>
                            <td>密码:</td>
                            <td>
                                <input id="txtKey" type="text" style="width: 60px;" /></td>
                            <td style="width: 10px">&nbsp;</td>
                            <td>货主:</td>
                            <td>
                                <input id="txtSupplier" type="text" style="width: 60px;" /></td>
                            <td style="width: 10px">&nbsp;</td>
                            <td>车牌号:</td>
                            <td>
                                <input id="txtLicense" type="text" style="width: 80px;" /></td>
                            <td style="width: 10px">&nbsp;</td>
                            <td>
                                <img id="Query" class="Button" style="display: normal" src="/Images/Query.gif" /></td>
                            <td style="width: 20px">&nbsp;</td>
                            <td>
                                <img id="Excel" class="Button" style="display: normal" src="/Images/Excel.gif" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:15,pageList:[15,50,100,Setter.Max]">
                        <thead>
                            <tr>
                                <%--<th data-options="field:'WeighTime'" width="95px">首磅时间</th>--%>
                                <th data-options="field:'Bang_Time'" width="95px">磅单时间</th>
                                <th data-options="field:'License'" width="75px">车牌号</th>
                                <th data-options="field:'Tree'" width="45px">片 种</th>
                                <th data-options="field:'Key'" width="40px">密 码</th>
                                <th data-options="field:'Water'" width="69px">原始水份 %</th>
                                <th data-options="field:'RebateWater'" width="69px">折后水份 %</th>
                                <th data-options="field:'Skin'" width="69px">原始树皮 %</th>
                                <th data-options="field:'RebateSkin'" width="69px">折后树皮 %</th>
                                <th data-options="field:'Scrap'" width="69px">原始碎料 %</th>
                                <th data-options="field:'RebateScrap'" width="69px">折后碎料 %</th>
                                <th data-options="field:'Bad'" width="44px">腐朽 %</th>
                                <th data-options="field:'Greater'" width="69px">过大木片 %</th>
                                <th data-options="field:'Less'" width="69px">过小木片 %</th>
                                <th data-options="field:'DeductVolume'" width="95px">树皮碎料扣除m³</th>

                                <th data-options="field:'Deduct'" width="150px">扣重原因</th>
                                <th data-options="field:'Remark'" width="150px">备 注</th>

                                <th data-options="field:'Sampler'" width="69px">取样人</th>
                                <th data-options="field:'CheckNumber'" width="45px">检验号</th>
                                <th data-options="field:'CheckTime'" width="95px">化验时间</th>
                                <th data-options="field:'LaboratoryPeople'" width="60px">报告人</th>
                                <th data-options="field:'IsConfirmed'" width="45px">已审核</th>
                                <th data-options="field:'ConfirmePeople'" width="60px">审核人</th>
                                <th data-options="field:'Supplier'" width="40px">货 主</th>
                                <th data-options="field:'jWeight'" width="60px">净重（吨）</th>
                            </tr>
                        </thead>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
