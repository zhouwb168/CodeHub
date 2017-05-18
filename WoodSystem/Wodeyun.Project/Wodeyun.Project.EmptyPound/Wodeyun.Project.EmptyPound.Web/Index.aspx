<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Project.EmptyPound.Web.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Wodeyun Project EmptyPound</title>
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

    <link href="Styles/softkey.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/softkeyboard.js" type="text/javascript" charset="gb2312"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="easyui-tabs" data-options="fit:false,border:true">
            <div title="车辆回皮" style="padding: 10px;">
                <table>
                    <tr>
                        <td style="display:none;">回皮日期从</td>
                        <td style="display:none;">
                            <input id="txtStartTime" type="text" class="easyui-datebox" style="width: 95px;" /></td>
                        <td style="width: 2px;display:none;">到</td>
                        <td style="display:none;">
                            <input id="txtEndTime" type="text" class="easyui-datebox" style="width: 95px;" /></td>
                        <td style="width: 2px;display:none;">&nbsp;</td>
                        <td>车牌号:</td>
                        <td>
                            <input id="txtCarID" type="text" style="width: 70px;" /></td>
                        <td style="width: 2px">&nbsp;</td>
                        <td>
                            <img id="query" class="Button" style="display: normal" src="/Images/Query.gif" /></td>
                    </tr>
                </table>
                <div style="height: 190px;">
                    <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:5,pageList:[5,10,20]">
                        <thead>
                            <tr>
                                <th data-options="field:'BackWeighTime'" width="110px">回皮时间</th>
                                <th data-options="field:'RedCard'" width="110px">红卡号</th>
                                <th data-options="field:'License'" width="110px">车牌号</th>
                                <th data-options="field:'FullVolume'" width="110px">首磅体积(立方米)</th>
                                <th data-options="field:'EmptyVolume'" width="110px">回皮体积(立方米)</th>
                                <th data-options="field:'HandVolume'" width="110px">人工量方(立方米)</th>
                                <th data-options="field:'RebateVolume'" width="90px">扣方(立方米)</th>
                                <th data-options="field:'JVolume'" width="110px">净体积(立方米)</th>
                                <th data-options="field:'GreenCard'" width="110px">对应绿卡号</th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <table width="800px;" style="margin-top: 20px;">
                    <tr>
                        <td width="84px" class="Td">红卡号：</td>
                        <td class="Td">
                            <input id="txtRedCard" maxlength="10" disabled="disabled" type="text" class="easyui-validatebox" style="width: 200px;" data-options="required:true" />
                            <a id="aReadCard" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">读卡</a> (当前为车辆回皮)
                        </td>
                    </tr>
                    <tr>
                        <td  class="Td">车牌号：</td>
                        <td class="Td">
                            <input disabled="disabled" id="txtLicense" type="text" style="width: 200px;" /></td>
                    </tr>
                    <tr>
                        <td  class="Td">卸货时间：</td>
                        <td class="Td">
                            <input disabled="disabled" id="txtSampleTime" type="text" style="width: 200px;" /></td>
                    </tr>
                    <tr>
                        <td  class="Td">扣重(方)原因：</td>
                        <td class="Td">
                            <textarea disabled="disabled" id="txtDeduct" style="width: 200px; height: 64px;"></textarea></td>
                    </tr>
                    <tr>
                        <td class="Td">&nbsp;</td>
                        <td class="Td">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="Td">首磅体积：</td>
                        <td class="Td">
                            <input disabled="disabled" id="txtFullVolume" type="text" style="width: 200px;" />
                            (单位：立方米)
                        </td>
                    </tr>
                    <tr>
                        <td  class="Td">人工量方：</td>
                        <td class="Td">
                            <input id="txtHandVolume" onclick="showkeyboard(this, 'number');" type="text" style="width: 200px;" maxlength="18" />
                            (单位：立方米)</td>
                    </tr>
                    <tr>
                        <td  class="Td">扣方：</td>
                        <td class="Td">
                            <input id="txtRebateVolume" onclick="showkeyboard(this, 'number');" type="text" style="width: 200px;" maxlength="18" />
                            (<b style="color:green">若有扣方请先输入扣方数据再量方</b>，单位：立方米)</td>
                    </tr>
                    <tr>
                        <td class="Td">回皮体积：</td>
                        <td class="Td">
                            <table cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="width: 370px">
                                        <input disabled="disabled" id="txtEmptyVolume" type="text" style="width: 200px" />
                                        (单位：立方米) <a id="aLiangFang" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">量方</a></td>
                                    <td style="width: 350px">&nbsp;<span id="spanLiangFang" style="color: red; float: left"></span>
                                        <input style="display: none" id="txtLFUnique" type="text" style="width: 200px" />
                                        <input style="display: none" id="txtLFDate" type="text" style="width: 200px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td  class="Td">净体积：</td>
                        <td class="Td">
                            <input disabled="disabled" id="txtDisc" type="text" style="width: 200px;" />
                            (净体积 = 首磅体积 － 回皮体积 － 扣方 ，单位：立方米)</td>
                    </tr>
                    <tr>
                        <td class="Td">&nbsp;</td>
                        <td class="Td">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="Td">对应绿卡：</td>
                        <td class="Td">
                            <input disabled="disabled" id="txtCreenCardNumber" type="text" style="width: 200px;" class="easyui-validatebox" data-options="required:true" />
                            (需要换回的绿卡号码)</td>
                    </tr>
                    <tr>
                        <td class="Td">绿卡号：</td>
                        <td class="Td">
                            <input id="txtGreenCard" maxlength="10" disabled="disabled" type="text" class="easyui-validatebox" style="width: 200px;" data-options="required:true" />
                            <a id="aChangeCard" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">换卡</a>
                        </td>
                    </tr>
                    <tr>
                        <td class="Td"></td>
                        <td class="Td"></td>
                    </tr>
                    <tr>
                        <td class="Td"></td>
                        <td id="Buttons" class="Td">
                            <img unique="Create" style="display: none;" class="Button" src="/Images/Create.gif" />
                            <img unique="Save" class="Button" src="/Images/Save.gif" />
                            &nbsp;&nbsp;&nbsp;&nbsp;<img unique="Delete" class="Button" style="display: none" src="/Images/Delete.gif" />
                            <img unique="Cancel" class="Button" style="display: none" src="/Images/Cancel.gif" />
                            <img style="display: normal" src="/Images/Blank.gif" />
                        </td>
                    </tr>
                    <tr>
                        <td style="display: none;">
                            <input id="txtUnique" type="text" title="序号" value="0" />
                            <input id="txtRecordID" type="text" title="关联号" value="0" />
                            <input id="txtGreenCID" type="text" title="绿卡编号" value="0" />
                            <input id="txtRedCID" type="text" title="红卡编号" value="0" />
                            <input id="txtBackWeighTime" type="text" title="回皮时间" value="" />
                            <input id="txtBangID" type="text" title="要对接的地磅编号" value="0" />
                            <input id="txtBangCID" type="text" title="要对接的磅单编号" value="0" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
