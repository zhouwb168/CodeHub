<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Project.Factory.Web.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Wodeyun Project Factory</title>
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
            <div title="卸货取样" style="padding: 5px; padding-top: 10px">
                <div style="height: 190px;">
                    <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:false,pagination:true,pageSize:5,pageList:[5,10,20]">
                        <thead>
                            <tr>
                                <th data-options="field:'WeighTime'" width="110px">过磅时间</th>
                                <th data-options="field:'License'" width="70px">车牌号</th>
                                <th data-options="field:'RedCardNumber'" width="45px">红卡号</th>
                                <th data-options="field:'Key'" width="40px">密码</th>
                                <th data-options="field:'Tree'" width="45px">树种</th>
                                <th data-options="field:'UnLoadPalce'" width="55px">卸货地点</th>
                                <th data-options="field:'UnLoadPeople'" width="60px">卸货人</th>
                                <th data-options="field:'Water'" width="55px">含水率 %</th>
                                <th data-options="field:'Skin'" width="55px">树皮量 %</th>
                                <th data-options="field:'Scrap'" width="55px">碎料量 %</th>
                                <th data-options="field:'Deduct'" width="80px">扣重原因</th>
                                <th data-options="field:'Remark'" width="80px">备注</th>
                                <th data-options="field:'Sampler'" width="60px">取样人</th>
                                <th data-options="field:'Box'" width="60px">箱号</th>
                                <th data-options="field:'PackTime'" width="110px">装箱时间</th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <table width="800px;" style="margin-top: 40px;">
                    <tr>
                        <td width="64px" class="Td">红卡号：</td>
                        <td class="Td">
                            <input id="txtCard" maxlength="10" type="text" disabled="disabled" class="easyui-validatebox" style="width: 200px;" data-options="required:true" />
                            <a id="aReadCard" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">读卡</a>（请点击该按钮，并刷读电子卡）
                        </td>
                    </tr>
                    <tr>
                        <td width="64px" class="Td">车牌号：</td>
                        <td class="Td">
                            <input disabled="disabled" id="txtLicense" type="text" style="width: 200px;" />
                        </td>
                    </tr>
                    <tr>
                        <td width="64px" class="Td">首磅时间：</td>
                        <td class="Td">
                            <input disabled="disabled" id="txtWeighTime" type="text" style="width: 200px;" /></td>
                    </tr>
                    <tr>
                        <td width="64px" class="Td">树种：</td>
                        <td class="Td">
                            <input disabled="disabled" id="txtTree" type="text" style="width: 200px;" /></td>
                    </tr>
                    <tr>
                        <td width="64px" class="Td">送货员：</td>
                        <td class="Td">
                            <input disabled="disabled" id="txtDriver" type="text" style="width: 200px;" /></td>
                    </tr>
                    <tr>
                        <td class="Td">&nbsp;</td>
                        <td class="Td">&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="64px" class="Td">卸货地点：</td>
                        <td class="Td">
                            <input id="txtUnLoadPalce" type="text" class="easyui-validatebox" style="width: 80px;" data-options="required:true" onclick="showkeyboard(this, 'letter');" maxlength="3" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;卸货人：<input id="txtUnLoadPeople" class="easyui-validatebox" data-options="required:true" type="text" style="width: 170px" />
                        </td>
                    </tr>
                    <tr>
                        <td width="64px" class="Td">密码：</td>
                        <td class="Td">
                            <input id="txtKey" class="easyui-validatebox" data-options="required:true" type="text" style="width: 80px" onclick="showkeyboard(this, 'numlet');" maxlength="6" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;取样人：<input id="txtSampler" class="easyui-validatebox" data-options="required:true" type="text" style="width: 80px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Td">&nbsp;</td>
                        <td class="Td">&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="64px" class="Td" style="vertical-align: top;">目测评估：</td>
                        <td class="Td" style="padding-left: 0px;">
                            <table id="tblAssess" style="width: 100%;">
                                <tr>
                                    <td style="padding-left: 0px;">
                                        <table cellpadding="0" cellspacing="8">
                                            <tr>
                                                <td>很湿</td>
                                                <td>湿</td>
                                                <td>微湿</td>
                                                <td>微干</td>
                                                <td>干</td>
                                                <td>很干</td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: 10px;">(70～99) %</td>
                                                <td style="font-size: 10px;">(50～70) %</td>
                                                <td style="font-size: 10px;">(40～50) %</td>
                                                <td style="font-size: 10px;">(20～40) %</td>
                                                <td style="font-size: 10px;">(10～20) %</td>
                                                <td style="font-size: 10px;">(0～10) %</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input group="Water" min="70" id="Text2" style="width: 40px;" type="text" onclick="showkeyboard(this, 'number');" /></td>
                                                <td>
                                                    <input group="Water" min="50" id="Text13" style="width: 40px;" type="text" onclick="showkeyboard(this, 'number');" /></td>
                                                <td>
                                                    <input group="Water" min="40" id="Text14" style="width: 40px;" type="text" onclick="showkeyboard(this, 'number');" /></td>
                                                <td>
                                                    <input group="Water" min="20" id="Text18" style="width: 40px;" type="text" onclick="showkeyboard(this, 'number');" /></td>
                                                <td>
                                                    <input group="Water" min="10" id="Text20" style="width: 40px;" type="text" onclick="showkeyboard(this, 'number');" /></td>
                                                <td>
                                                    <input group="Water" min="0" id="Text21" style="width: 40px;" type="text" onclick="showkeyboard(this, 'number');" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="8">
                                            <tr>
                                                <td>树皮多</td>
                                                <td>树皮中</td>
                                                <td>树皮少</td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: 10px;">(15～50) %</td>
                                                <td style="font-size: 10px;">(5～15) %</td>
                                                <td style="font-size: 10px;">(0～5) %</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input group="Skin" min="15" id="Text3" style="width: 40px;" type="text" onclick="showkeyboard(this, 'number');" /></td>
                                                <td>
                                                    <input group="Skin" min="5" id="Text5" style="width: 40px;" type="text" onclick="showkeyboard(this, 'number');" /></td>
                                                <td>
                                                    <input group="Skin" min="0" id="Text6" style="width: 40px;" type="text" onclick="showkeyboard(this, 'number');" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="8">
                                            <tr>
                                                <td>碎屑多</td>
                                                <td>碎屑中</td>
                                                <td>碎屑少</td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: 10px;">(10～20) %</td>
                                                <td style="font-size: 10px;">(5～10) %</td>
                                                <td style="font-size: 10px;">(0～5) %</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input group="Scrap" min="10" id="Text7" style="width: 40px;" type="text" onclick="showkeyboard(this, 'number');" /></td>
                                                <td>
                                                    <input group="Scrap" min="5" id="Text8" style="width: 40px;" type="text" onclick="showkeyboard(this, 'number');" /></td>
                                                <td>
                                                    <input group="Scrap" min="0" id="Text9" style="width: 40px;" type="text" onclick="showkeyboard(this, 'number');" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="Td"></td>
                        <td class="Td"></td>
                    </tr>
                    <tr>
                        <td width="64px" class="Td">扣重原因：</td>
                        <td class="Td">
                            <textarea id="txtDeduc" style="width: 320px; height: 40px;"></textarea>（显示给地磅看）
                        </td>
                    </tr>
                    <tr>
                        <td width="64px" class="Td">备注：</td>
                        <td class="Td">
                            <textarea id="txtRemarkt" style="width: 320px; height: 40px;"></textarea>（显示给化验室看）
                        </td>
                    </tr>
                    <tr>
                        <td class="Td">送样箱号：</td>
                        <td class="Td">
                            <input class="easyui-validatebox" data-options="required:true" id="txtBox" type="text" onclick="showkeyboard(this, 'number');" style="width: 150px;" maxlength="3" />（修改箱号需到“样品装箱”模块操作）
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
                            <input id="txtCID" type="text" title="绿卡编号" value="0" />
                            <input id="txtOldKey" type="text" title="旧密码" value="" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
