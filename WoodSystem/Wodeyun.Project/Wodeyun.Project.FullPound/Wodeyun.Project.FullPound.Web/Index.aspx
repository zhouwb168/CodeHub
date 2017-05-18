<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Project.FullPound.Web.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Wodeyun Project FullPound</title>
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
            <div title="车辆首磅" style="padding: 10px;">
                 <table>
                        <tr>
                            <td style="display:none;">首磅日期从</td>
                            <td style="display:none;"><input id="txtStartTime" type="text" class="easyui-datebox" style="width: 95px;" /></td>
                            <td style="width: 2px;display:none;">到</td>
                            <td style="display:none;"><input id="txtEndTime" type="text" class="easyui-datebox" style="width: 95px;" /></td>
                            <td style="width: 2px;display:none;">&nbsp;</td>
                            <td>车牌号:</td>
                            <td><input id="txtCarID" type="text" style="width: 70px;" /></td>
                            <td style="width: 2px">&nbsp;</td>
                            <td><img id="queryFullPound" class="Button" style="display: normal" src="/Images/Query.gif" /></td>
                        </tr>
                    </table>
                <div style="height: 190px;">
                    <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:5,pageList:[5,10,20]">
                        <thead>
                            <tr>
                                <th data-options="field:'CreenCardNumber'" width="80px">绿卡号</th>
                                <th data-options="field:'License'" width="80px">车牌号</th>
                                <th data-options="field:'WeighTime'" width="105px">首磅时间</th>
                                <th data-options="field:'FullVolume'" width="110px">首磅体积(立方米)</th>
                                <th data-options="field:'Tree'" width="60px">树种</th>
                                <th data-options="field:'Driver'" width="60px">送货员</th>
                                <th data-options="field:'Supplier'" width="60px">卸车员</th>
                                <th data-options="field:'Area'" width="110px">来源地</th>
                                <th data-options="field:'CardNumber'" width="80px">对应红卡号</th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <table width="800px;" style="margin-top: 20px;">
                    <tr>
                        <td class="Td">绿色卡号：</td>
                        <td class="Td">
                            <input id="txtGreenCard" maxlength="10" disabled="disabled" type="text" class="easyui-validatebox" style="width: 200px;" data-options="required:true" />
                            <a id="aReadCard" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">读卡</a> (当前为车辆首磅)
                        </td>
                    </tr>
                    <tr>
                        <td class="Td">进厂时间：</td>
                        <td class="Td">
                            <input disabled="disabled" id="txtArriveDate" type="text" class="easyui-validatebox" style="width: 200px;" data-options="required:true" /></td>
                    </tr>
                    
                    <tr>
                        <td class="Td" style="width: 64px">首磅体积：</td>
                        <td class="Td">
                            <table cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="width: 370px">
                                        <input disabled="disabled" id="txtFullVolume" type="text" style="width: 200px" />
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
                        <td class="Td">车牌号：</td>
                        <td class="Td">
                            <input id="txtCartChinese" type="text" style="width: 40px" class="easyui-validatebox" data-options="required:true" maxlength="1" onclick="showkeyboard(this, 'chinese');" />
                            <input id="txtCartNumber" type="text" style="width: 157px" class="easyui-validatebox" data-options="required:true" maxlength="7" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Td">来源地：</td>
                        <td class="Td">
                            <input id="txtArea" type="text" style="width: 200px;" class="easyui-validatebox" data-options="required:true" />
                            (如：右江区，需要填写或修正)
                        </td>
                    </tr>
                    <tr>
                        <td class="Td">树种：</td>
                        <td class="Td">
                            <select id="sltTree" class="easyui-combobox" style="width: 80px;" data-options="valueField:'Unique',textField:'Name'"></select>
                            <span style="color: red">(请不要忘记要在这里选择正确的品种)</span></td>
                    </tr>
                    <tr>
                        <td class="Td">送货员：</td>
                        <td class="Td">
                            <input id="txtDriver" type="text" style="width: 200px;" class="easyui-validatebox" data-options="required:true" />
                            (需要填写或修正)</td>
                    </tr>
                    <tr>
                        <td class="Td">卸车员：</td>
                        <td class="Td">
                            <input id="txtSupplier" type="text" class="easyui-validatebox" style="width: 200px;" data-options="required:true" />
                            (如：A2，需要填写或修正)</td>
                    </tr>
                    <tr>
                        <td class="Td">红卡号：</td>
                        <td class="Td">
                            <input disabled="disabled" maxlength="10" id="txtRedCard" type="text" class="easyui-validatebox" style="width: 200px;" data-options="required:true" />
                            <a id="aChangeCard" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">换卡</a></td>
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
                            <input id="txtWeightTime" type="text" title="首磅时间" value="0" />
                        </td>
                        <td class="Td">&nbsp;</td>
                    </tr>
                </table>
            </div>
            <div title="木材收购磅单" style="padding: 10px;">
                <div style="height: 400px;">
                    <table width="100%" height="100%">
                        <tr>
                            <td height="28px">
                                <table>
                                    <tr>
                                        <td>首磅日期从</td>
                                        <td>
                                            <input id="txtStartDate" type="text" class="easyui-datebox" style="width: 95px;" /></td>
                                        <td style="width: 2px">到</td>
                                        <td>
                                            <input id="txtEndDate" type="text" class="easyui-datebox" style="width: 95px;" /></td>
                                        <td style="width: 2px">&nbsp;</td>
                                        <td>车牌号:</td>
                                        <td>
                                            <input id="txtLicense1" type="text" style="width: 70px;" /></td>
                                        <td style="width: 2px">&nbsp;</td>
                                        <td>
                                            <img id="Query" class="Button" style="display: normal" src="/Images/Query.gif" /></td>
                                        <td style="width: 2px">&nbsp;</td>
                                        <td>
                                            <img id="Print" class="Button" style="display: normal" src="/Images/Print.png" /></td>
                                        <td style="width: 2px">&nbsp;</td>
                                        <td><a id="arePrint" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">补打单</a></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="bangrid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:5,pageList:[25,50,100],idField:'Bang_Time'">
                                    <thead>
                                        <tr>
                                            <th data-options="checkbox:true"></th>
                                            <th data-options="field:'WeighTime'" width="105px">首磅时间</th>
                                            <th data-options="field:'License'" width="80px">车牌号</th>
                                            <th data-options="field:'Tree'" width="60px">树种</th>
                                            <th data-options="field:'FullVolume'" width="110px">首磅体积(立方米)</th>
                                            <th data-options="field:'EmptyVolume'" width="110px">回皮体积(立方米)</th>
                                            <th data-options="field:'jVolume'" width="110px">净体积(立方米)</th>
                                            <th data-options="field:'RebateVolume'" width="110px">扣方(立方米)</th>
                                            <%--<th data-options="field:'firstBangUser'" width="60px">过磅员</th>--%>
                                            <th data-options="field:'Supplier'" width="60px">卸车员</th>
                                            <th data-options="field:'Driver'" width="60px">送货员</th>
                                        </tr>
                                    </thead>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
