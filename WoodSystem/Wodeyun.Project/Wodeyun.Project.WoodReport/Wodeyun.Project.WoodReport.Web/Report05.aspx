<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report05.aspx.cs" Inherits="Wodeyun.Project.WoodReport.Web.Report05" %>

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
    <script src="/Scripts/Report05.js"></script>
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
                                <input id="txtStartDate" type="text" class="easyui-datebox" style="width: 95px;" /></td>
                            <td style="width: 2px">到</td>
                            <td>
                                <input id="txtEndDate" type="text" class="easyui-datebox" style="width: 95px;" /></td>
                            <td style="width: 2px">&nbsp;</td>
                            <td>密码:</td>
                            <td>
                                <input id="txtKey" type="text" style="width: 40px;" /></td>
                            <td style="width: 2px">&nbsp;</td>
                            <td>货主:</td>
                            <td>
                                <input id="txtSupplier" type="text" style="width: 40px;" /></td>
                            <td style="width: 2px">&nbsp;</td>
                            <td>车牌号:</td>
                            <td>
                                <input id="txtLicense" type="text" style="width: 70px;" /></td>
                            <td style="width: 2px">&nbsp;</td>
                            <td>已打印:</td>
                            <td>
                                <select id="sltPrint" class="easyui-combobox">
                                    <option value="-1">全部</option>
                                    <option value="0">否</option>
                                    <option value="1">是</option>
                                </select>
                            </td>
                            <td style="width: 2px">&nbsp;</td>
                            <td>
                                <img id="Query" class="Button" style="display: normal" src="/Images/Query.gif" /></td>
                            <td style="width: 2px">&nbsp;</td>
                            <td>
                                <img id="Print" class="Button" style="display: normal" src="/Images/Print.png" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:false,rownumbers:true,pagination:true,pageSize:15,pageList:[15,50,100,Setter.Max],idField:'Unique'">
                        <thead>
                            <tr>
                                <th data-options="checkbox:true"></th>
                                <th data-options="field:'Printed'" width="45px">已打印</th>
                                <th data-options="field:'Bang_Time'" width="105px">过磅时间（磅单）</th>
                                <th data-options="field:'License'" width="75px">车牌号</th>
                                <th data-options="field:'Supplier'" width="45px">卸车员</th>
                                <th data-options="field:'Tree'" width="48px">树种</th>
                                <th data-options="field:'Driver'" width="45px">送货员</th>
                                <th data-options="field:'jWeight'" width="60px">净重（吨）</th>
                                <th data-options="field:'Key'" width="55px">料厂密码</th>
                                <th data-options="field:'Sampler'" width="60px">采样人</th>
                                <th data-options="field:'Water'" width="69px">原始水份 %</th>
                                <th data-options="field:'RebateWater'" width="69px">折后水份 %</th>
                                <th data-options="field:'Skin'" width="70px">原始树皮 %</th>
                                <th data-options="field:'RebateSkin'" width="70px">折后树皮 %</th>
                                <th data-options="field:'Scrap'" width="70px">原始碎料 %</th>
                                <th data-options="field:'RebateScrap'" width="70px">折后碎料 %</th>
                                <th data-options="field:'Bad'" width="50px">腐朽 %</th>
                                <th data-options="field:'Greater'" width="70px">过大木片 %</th>
                                <th data-options="field:'RebateGreater'" width="70px">折后过大木片 %</th>
                                <th data-options="field:'Less'" width="70px">过小木片 %</th>
                                <th data-options="field:'DeductVolume'" width="95px">树皮碎料扣除m³</th>
                                <th data-options="field:'WeighTime'" width="100px">过磅时间（刷卡）</th>
                                <th data-options="field:'carCID'" width="55px">车号</th>
                                <th data-options="field:'userXHName'" width="45px">卸车员</th>
                                <th data-options="field:'breedName'" width="120px">品种</th>
                                <th data-options="field:'carUser'" width="45px">送货员</th>
                                <th data-options="field:'firstBangUser'" width="55px">首磅员</th>
                            </tr>
                        </thead>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
