<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Project.GsmJoin.Web.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Wodeyun Project GsmJoin</title>
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
</head>
<body>
    <form id="form1" runat="server">
        <div id="divTab" class="easyui-tabs" data-options="fit:false,border:true">
            <div title="报备信息对接" style="padding: 10px;">
                <table>
                    <tr>
                        <td valign="center">首磅日期：从<input id="ipDateOfBang" type="text" class="easyui-datebox" style="width: 100px;" /></td>
                        <td valign="center">到
                            <input id="ipDateOfBangend" type="text" class="easyui-datebox" style="width: 100px;" /></td>
                        <td valign="center">车号：<input id="txtcarid" type="text" style="width: 100px;" /></td>
                        <td valign="center">区域：<input id="txtarea" type="text" style="width: 100px;" /></td>
                        <td>
                            <img id="imgQueryForBang" class="Button" style="display: normal" src="/Images/Query.gif" /></td>
                    </tr>
                </table>
                <div style="margin-top: 5px; height: 190px">
                    <table id="GridOfBang" class="easyui-datagrid" fit="true" data-options="singleSelect:true,checkOnSelect: false, selectOnCheck: false,rownumbers:true,pagination:true,pageSize:5,pageList:[5,10,20]">
                        <thead>
                            <tr>
                                <%--                                <th data-options="field:'Bang_Time'" width="120px">过磅时间（地磅）</th>
                                <th data-options="field:'carCID'" width="80px">车号（地磅）</th>
                                <th data-options="field:'userXHName'" width="90px">卸车员（地磅）</th>
                                <th data-options="field:'breedName'" width="120px">品种（地磅）</th>
                                <th data-options="field:'carUser'" width="90px">送货员（地磅）</th>--%>
                                <th data-options="checkbox:true"></th>
                                <th data-options="field:'Bang_Time'" width="120px">过磅时间</th>
                                <th data-options="field:'License'" width="89px">车号</th>
                                <th data-options="field:'Supplier'" width="100px">卸车员</th>
                                <th data-options="field:'Tree'" width="90px">品种</th>
                                <th data-options="field:'Driver'" width="100px">送货员</th>
                                <th data-options="field:'Area'" width="100px">区域</th>
                                <th data-options="field:'JWeight'" width="100px">净重(吨)</th>
                                <th data-options="field:'FullVolume'" width="100px">首磅体质(立方米)</th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <table style="border: 1px solid #ccc; width: 95%; margin-top: 10px">
                    <tr>
                        <td style="border-style: none solid none none; border-width: 1px; border-color: #ccc; vertical-align: top; text-align: left; width: 400px">
                            <table>
                                <tr>
                                    <td colspan="7"><b>请选择短信报备信息查询条件</b></td>
                                </tr>
                                <tr>
                                    <td style="color: red">
                                        <input id="chklicense" type="checkbox" checked disabled /></td>
                                    <td style="white-space: nowrap">车号 </td>
                                    <td style="color: red">
                                        <input id="chkdriver" type="checkbox" /></td>
                                    <td style="white-space: nowrap">送货员 </td>
                                    <td style="color: red">
                                        <input id="chksupplier" type="checkbox" /></td>
                                    <td style="white-space: nowrap">卸货员 </td>
                                    <td style="color: red">
                                        <input id="IsAdd" type="checkbox" /></td>
                                    <td style="white-space: nowrap">补报 </td>
                                    <td style="text-align: right"><a id="aJoin" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">对接</a></td>
                                </tr>
                            </table>
                        </td>
                        <td style="border-style: none solid none none; border-width: 1px; border-color: #ccc; vertical-align: top; text-align: left; width: 300px">
                            <table>
                                <tr>
                                    <td colspan="3"><b>无报备信息对接(<span style="color:red">请选择正确的区域</span>)</b></td>
                                </tr>
                                <tr>
                                    <td>区域：</td>
                                    <td class="Td-Combo">
                                        <select id="Area" class="easyui-combobox" data-options="valueField:'Unique',textField:'Name'" style="width: 80px"></select>
                                    </td>
                                    <td style="text-align: left"><a id="aNoGsmJoin" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">对接</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="border-style: none solid none none; border-width: 1px; border-color: #ccc; vertical-align: top; text-align: left; width: 300px">
                            <table>
                                <tr>
                                    <td><b>手动查询</b></td>
                                </tr>
                                <tr>
                                    <td style="text-align: center">车号：<input id="txtLicense" type="text" style="width: 100px;" />&nbsp;<a id="aQueryForNoSure" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">模糊查询</a>
                                    </td>
                                </tr>
                            </table>
                        </td>

                        <td style="text-align: left; width: 350px">
                            <table>
                                <tr>
                                    <td><b>过滤上面列表没有报备的记录</b></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left"><a id="aFilte" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">过滤</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <div style="margin-top: 20px; padding-left: 4px;">可对接的短信报备系统数据列表（必选项）：</div>
                <div style="margin-top: 4px; height: 200px">
                    <table id="GridOfGsm" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:5,pageList:[5,10,20]">
                        <thead>
                            <tr>
                                <th data-options="field:'License'" width="100px">车号</th>
                                <th data-options="field:'Date'" width="130px">报备时间</th>
                                <th data-options="field:'Ship'" width="130px">发车日期</th>
                                <th data-options="field:'Supplier'" width="100px">卸车员</th>
                                <th data-options="field:'Tree'" width="126px">品种</th>
                                <th data-options="field:'Driver'" width="100px">送货员</th>
                                <th data-options="field:'Area'" width="120px">来源地</th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
            <div title="已成功对接的数据" style="padding: 10px;">
                <table style="width: 95%">
                    <tr>
                        <td height="40px" style="text-align: center">
                            <table>
                                <tr>
                                    <td valign="center">首磅开始日期：<input id="ipDateOfJoin" type="text" class="easyui-datebox" style="width: 100px;" /></td>
                                    <td style="width: 10px">&nbsp;</td>
                                    <td>车牌号（不包含省份）:</td>
                                    <td>
                                        <input id="txtLicenseForJoinSearch" type="text" style="width: 80px;" /></td>
                                    <td style="width: 10px">&nbsp;</td>
                                    <td>
                                        <img id="imgQueryForJoin" class="Button" style="display: normal" src="/Images/Query.gif" /></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td><a id="aCutOff" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">断开对接</a></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 470px">
                            <table id="GridOfJoin" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:20,pageList:[15,20,50,Setter.Max]">
                                <thead>
                                    <tr>
                                        <th data-options="field:'Bang_Time'" width="120px">过磅时间（地磅）</th>
                                        <th data-options="field:'License'" width="80px">车号（地磅）</th>
                                        <th data-options="field:'Supplier'" width="90px">卸车员（地磅）</th>
                                        <th data-options="field:'Tree'" width="100px">品种（地磅）</th>
                                        <th data-options="field:'Driver'" width="90px">送货员（地磅）</th>
                                        <%--<th data-options="field:'Area'" width="120px">区域（地磅）</th>--%>
                                        <th data-options="field:'jWeight'" width="100px">净重(吨)</th>
                                        <th data-options="field:'FullVolume'" width="100px">首磅体质(立方米)</th>
                                        <th data-options="field:'bbbb'" width="20px"></th>
                                        <%--<th data-options="field:'GILicense'" width="100px">车号（短信）</th>--%>
                                        <th data-options="field:'Date'" width="130px">报备时间（短信）</th>
                                        <th data-options="field:'Ship'" width="130px">发车日期（短信）</th>
                                        <th data-options="field:'GISupplier'" width="100px">卸车员（短信）</th>
                                        <%--<th data-options="field:'GITree'" width="100px">品种（短信）</th>--%>
                                        <th data-options="field:'GIArea'" width="100px">区域（短信）</th>
                                        <th data-options="field:'JoinTime'" width="110px">对接时间</th>
                                    </tr>
                                </thead>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div title="已被过滤的数据" style="padding: 10px;">
                <table style="width: 800px">
                    <tr>
                        <td height="40px" style="text-align: center">
                            <table>
                                <tr>
                                    <td valign="center">过磅开始日期：<input id="ipDateOfFilter" type="text" class="easyui-datebox" style="width: 100px;" /></td>
                                    <td>
                                        <img id="imgQueryForFilter" class="Button" style="display: normal" src="/Images/Query.gif" /></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td><a id="aRenew" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">恢复</a></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 470px">
                            <table id="GridOfFilter" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:20,pageList:[15,20,50,Setter.Max]">
                                <thead>
                                    <tr>
                                        <th data-options="field:'Bang_Time'" width="120px">过磅时间</th>
                                        <th data-options="field:'carCID'" width="100px">车号</th>
                                        <th data-options="field:'userXHName'" width="100px">卸车员</th>
                                        <th data-options="field:'breedName'" width="126px">品种</th>
                                        <th data-options="field:'jWeight'" width="112px">净重（单位：吨）</th>
                                        <th data-options="field:'firstBangUser'" width="100px">首磅员</th>
                                        <th data-options="field:'carUser'" width="100px">送货员</th>
                                    </tr>
                                </thead>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div style="display: none">
            <input id="txtUnique" type="text" title="要断开对接的记录编号" value="0" />
            <input id="txtCutOffBangID" type="text" title="地磅货重编号" value="0" />
            <input id="txtBangID" type="text" title="要对接的地磅货重编号" value="0" />
            <input id="txtBangTime" type="text" title="要查询的地磅首磅时间" value="" />
            <input id="txtWoodID" type="text" title="要对接的木材编号" value="0" />
            <input id="txtGsmID" type="text" title="要对接的短信报备编号" value="0" />
            <input id="txtRenewID" type="text" title="要恢复非过滤状态的记录编号" value="0" />
            <input id="txtDriver" type="text" title="要对接的送货员" value="" />
            <input id="txtSupplier" type="text" title="要对接的卸货员" value="" />
            <input id="txtJoinType" type="text" value="" />
        </div>
    </form>
</body>
</html>
