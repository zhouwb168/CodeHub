<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Project.WoodLaboratory.Web.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Wodeyun Project WoodLaboratory</title>
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
        <table style="height:28px">
              <tr>
                <td id="tdDate">
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="red">今天</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">昨天</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Day 2</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Day 3</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Day 4</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Day 5</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Day 6</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Day 7</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Day 8</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Day 9</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Day 10</font></a>
                </td>
                <td valign="center"><input id="Date" type="text" class="easyui-datebox" style="width:100px;" /></td>
                  <td style="width:5px">&nbsp;</td>
                  <td>密码:</td>
                  <td><input id="txtKeyForSearch" type="text" style="width:50px;" /></td>
                <td style="width:5px">&nbsp;</td>
                  <td>检验号:</td>
                  <td><input id="txtNumberForSearch" type="text" style="width:50px;" /></td>
                <td style="width:5px">&nbsp;</td>
                <td><img id="Query" class="Button" style="display:normal" src="/Images/Query.gif" /></td>
              </tr>
            </table>
      <table width="100%">
        <tr>
          <td width="300px" class="Td" style="vertical-align:top; height:560px;">
            <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:25,pageList:[25,50,100]">
		          <thead>
			          <tr>
				   <th data-options="field:'WeighTime'" width="73px">过磅日期</th>
                  <th data-options="field:'Key'" width="45px">密码</th>
                  <th data-options="field:'CheckTime'" width="100px">化验时间</th>
                  <th data-options="field:'Number'" width="50px">检验号</th>
			          </tr>
		          </thead>
	          </table>
          </td>
          <td style="vertical-align:top">
            <table width="100%">
              <tr>
                <td>
                    <table style="width:100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="64px" class="Td">密码：</td>
                <td class="Td"><input disabled="disabled" id="txtKey" type="text" class="easyui-validatebox" style="width:150px;" data-options="required:true" /></td>
                <td  width="64px" class="Td">检验号：</td>
                <td class="Td">
                    <input id="txtNumber" disabled="disabled" type="text" class="easyui-validatebox" style="width:150px;" data-options="required:true" />
                </td>
                        </tr>
                    </table>
                </td>
              </tr>
              <tr style="background-color: #FFF8D3">
                <td class="Td" style="text-align:center; height:30px">参考一，以下数据来源于电子卡系统：</td>
              </tr>
              <tr>
                <td>
                    <table style="width:100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width:64px" class="Td">品&nbsp;&nbsp;&nbsp;&nbsp;种：</td>
                            <td class="Td"><input id="txtTreeOfWood" style="width:150px;" type="text" disabled="disabled" /></td>
                            <td style="width:64px" class="Td">过磅时间：</td>
                            <td class="Td"><input id="txtWeighTimeOfWood" style="width:150px;" type="text" disabled="disabled" /></td>
                        </tr>
                        <tr>
                            <td style="width:64px" class="Td">取样备注：</td>
                            <td class="Td"><textarea id="txtRemarkOfWood" style="width:150px;height:64px;"></textarea></td>
                            <td style="width:64px;vertical-align:top" class="Td">净重(湿吨)</td>
                            <td class="Td" style="vertical-align:top"><input id="txtjweight" style="width:150px;" type="text" disabled="disabled" /></td>
                        </tr>
                    </table>
                </td>
              </tr>
              <tr style="background-color: #E0FCCE">
                <td class="Td" style="text-align:center; height:30px">参考二，以下数据来源于人工整理：</td>
              </tr>
              <tr>
                <td>
                    <table style="width:100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width:64px" class="Td">品&nbsp;&nbsp;&nbsp;&nbsp;种：</td>
                            <td class="Td"><input id="txtTreeOfGsm" style="width:150px;" type="text" disabled="disabled" /></td>
                            <td style="width:64px" class="Td">过磅时间：</td>
                            <td class="Td"><input id="txtBang_TimeOfGsm" style="width:150px;" type="text" disabled="disabled" /></td>
                        </tr>
                        <tr>
                            <td style="width:64px" class="Td">水份打折：</td>
                            <td class="Td"><input id="txtRebateOfGsm" style="width:150px;" type="text" disabled="disabled" /></td>
                            <td style="width:64px" class="Td">&nbsp;</td>
                            <td class="Td">&nbsp;</td>
                        </tr>
                    </table>
                </td>
              </tr>
              <tr style="background-color: #EAF9FC">
                <td class="Td" style="text-align:center; height:30px">请填写下面的化验报告并提交保存：</td>
              </tr>
              <tr>
                <td>
                    <table id="tblIpuntsGroup" style="width:100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="Td">原始水份：</td>
                            <td class="Td"><input id="txtWater" style="width:150px;" type="text" onclick="showkeyboard(this, 'number');" class="easyui-validatebox" data-options="required:true" /> %</td>
                            <td class="Td">折后水份：</td>
                            <td class="Td"><input disabled="disabled" id="txtRebateWater" style="width:150px;" type="text" onclick="showkeyboard(this, 'number');" /> %</td>
                        </tr>
                        <tr>
                            <td class="Td">原始树皮：</td>
                            <td class="Td"><input onclick="showkeyboard(this, 'number');" id="txtSkin" type="text" class="easyui-validatebox" style="width:150px;" data-options="required:true" /> %</td>
                            <td class="Td">折后树皮：</td>
                            <td class="Td"><input disabled="disabled" id="txtRebateSkin" style="width:150px;" type="text" onclick="showkeyboard(this, 'number');" class="easyui-validatebox" data-options="required:true" /> %</td>
                        </tr>
                        <tr>
                            <td class="Td">原始碎料：</td>
                            <td class="Td"><input onclick="showkeyboard(this, 'number');" id="txtScrap" type="text" class="easyui-validatebox" style="width:150px;" data-options="required:true" /> %</td>
                            <td class="Td">折后碎料：</td>
                            <td class="Td"><input disabled="disabled" id="txtRebateScrap" style="width:150px;" type="text" onclick="showkeyboard(this, 'number');" class="easyui-validatebox" data-options="required:true" /> %</td>
                        </tr>
                        <tr>
                            <td class="Td">过大木片：</td>
                            <td class="Td"><input onclick="showkeyboard(this, 'number');" id="txtGreater" type="text" style="width:150px;" /> %</td>
                            <td class="Td">拆后过大木片：</td>
                            <td class="Td"><input disabled="disabled" onclick="showkeyboard(this, 'number');" id="txtRebateGreater" type="text" style="width:150px;" /> %</td>
                        </tr>
                                                <tr>
                            <td class="Td">腐&nbsp;&nbsp;&nbsp;&nbsp;朽：</td>
                            <td class="Td"><input id="txtBad" style="width:150px;" type="text" onclick="showkeyboard(this, 'number');" /> %</td>
                            <td class="Td" colspan="2"><%--<b style="color:red">提示：折后数据系统会自动计算，只需输入原始数据保存即可.</b>--%></td>
                        </tr>
                        <tr>
                            <td class="Td">过小木片：</td>
                            <td class="Td"><input onclick="showkeyboard(this, 'number');" id="txtLess" type="text" style="width:150px;" /> %</td>
                            <td class="Td" colspan="2"><b style="color:red">树皮碎料扣除和折后数据系统会自动计算.</b></td>
                        </tr>
                        <tr>
                            <td class="Td">树皮碎料扣除：</td>
                            <td class="Td"><input disabled="disabled" id="txtDeductVolume" type="text" style="width:150px;" /> m³</td>
                            <td class="Td" colspan="2"><b style="color:red">只需输入原始数据保存即可.</b></td>
                        </tr>
                    </table>
                </td>
              </tr>
              <tr>
                <td class="Td">&nbsp;</td>
              </tr>
              <tr>
                <td id="Buttons" class="Td" style="padding-left:100px">
                  <img unique="Create" style="display:none;" class="Button" src="/Images/Create.gif" />
                  <img unique="Save" style="display:none;" class="Button" src="/Images/Save.gif" />
                  &nbsp;&nbsp;&nbsp;&nbsp;<img unique="Delete" class="Button" style="display:none" src="/Images/Delete.gif" />
                  <img unique="Cancel" class="Button" style="display:none" src="/Images/Cancel.gif" />
                  <img style="display:normal" src="/Images/Blank.gif" />
                </td>
              </tr>
                <tr style="display:none;">
                <td class="Td">
                    <input id="txtUnique" type="text" title="序号" value="0" />
                    <input id="txtRecordID" type="text" title="关联号" value="0" />
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </form>
</body>
</html>
