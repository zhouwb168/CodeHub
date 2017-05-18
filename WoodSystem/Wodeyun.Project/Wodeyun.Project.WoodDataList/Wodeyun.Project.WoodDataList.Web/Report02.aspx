<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report02.aspx.cs" Inherits="Wodeyun.Project.WoodDataList.Web.Report02" %>

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
    <script src="/Scripts/Report02.js"></script>
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
                            <td>货主:</td>
                            <td>
                                <input id="txtSupplier" type="text" style="width: 60px;" /></td>
                            <td>车牌号:</td>
                            <td>
                                <input id="txtLicense" type="text" style="width: 80px;" /></td>
                            <td>
                                <img id="Query" class="Button" style="display: normal" src="/Images/Query.gif" /></td>
                            <td>
                                <img id="Excel" class="Button" style="display: normal" src="/Images/Excel.gif" /></td>
                            <td id="datahtml"></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:15,pageList:[15,50,100,Setter.Max],showFooter:true"">
                        <thead>
                            <tr>
                                <th data-options="field:'Bang_Time'" width="100px">磅单日期</th>
                                <th data-options="field:'License'" width="70px">车牌号</th>
                                <th data-options="field:'Tree'" width="60px">品 种</th>
                                <th data-options="field:'WeighTime'" width="100px">首磅日期</th>
                                <th data-options="field:'FullVolume'" width="80px">首磅体积(方)</th>
                                <th data-options="field:'EmptyVolume'" width="80px">回皮体积(方)</th>
                                <th data-options="field:'DiscVolume'" width="80px">净体积(方)</th>
                                <th data-options="field:'HandVolume'" width="80px">人工量方(方)</th>
                                <th data-options="field:'MoreTanVolume'" width="80px">体积相差(方)</th>
                                <th data-options="field:'RebateVolume'" width="60px">扣方(方)</th>
                                <th data-options="field:'jWeight'" width="60px">净重（吨）</th>
                                <th data-options="field:'Driver'" width="45px">送货员</th>
                                <th data-options="field:'Supplier'" width="45px">卸车员</th>
                                <th data-options="field:'Area'" width="80px">木材来源</th>
                                <th data-options="field:'PeopleOfFullPound'" width="60px">首磅员</th>
                                <th data-options="field:'BackWeighTime'" width="100px">回皮时间</th>
                                <th data-options="field:'PeopleOfEmptyPound'" width="60px">回皮员</th>
                            </tr>
                        </thead>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
