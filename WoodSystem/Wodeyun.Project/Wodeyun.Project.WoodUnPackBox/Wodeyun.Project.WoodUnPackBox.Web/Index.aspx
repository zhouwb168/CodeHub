<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Project.WoodUnPackBox.Web.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Wodeyun Project WoodUnPackBox</title>
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
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Day 11</font></a>
                </td>
                <td valign="center"><input id="Date" type="text" class="easyui-datebox" style="width:100px;" /></td>
                <td><img id="Query" class="Button" style="display:normal" src="/Images/Query.gif" /></td>
              </tr>
            </table>
      <table width="100%">
        <tr>
          <td width="500px" class="Td" style="vertical-align:top; height:560px;">
            <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:25,pageList:[25,50,100]">
		          <thead>
			          <tr>
				   <th data-options="field:'WeighTime'" width="100px">过磅日期</th>
                  <th data-options="field:'Box'" width="80px">箱号</th>
                  <th data-options="field:'Key'" width="87px">密码</th>
                  <th data-options="field:'UnPackTime'" width="100px">拆箱时间</th>
                  <th data-options="field:'Number'" width="100px">检验号</th>
			          </tr>
		          </thead>
	          </table>
          </td>
          <td style="vertical-align:top;">
            <table width="100%">
              <tr>
                <td class="Td"></td>
                <td class="Td"></td>
              </tr>
              <tr>
                <td class="Td">过磅日期：</td>
                <td class="Td">
                    <input disabled="disabled" id="txtWeighTime" type="text" style="width:200px;" />
                </td>
              </tr>
              <tr>
                <td width="64px" class="Td">箱号：</td>
                <td class="Td"><input disabled="disabled" id="txtBox" type="text" style="width:200px;" /></td>
              </tr>
              <tr>
                <td class="Td">密码：</td>
                <td class="Td">
                    <input id="txtKey" disabled="disabled" type="text" style="width:200px;" />
                </td>
              </tr>
              <tr>
                <td class="Td">检验号：</td>
                <td class="Td">
                    <input id="txtNumber" type="text" onclick="showkeyboard(this, 'number');" maxlength="4" style="width:200px;" class="easyui-validatebox" data-options="required:true" />
                </td>
              </tr>
              <tr>
                <td class="Td"></td>
                <td class="Td"></td>
              </tr>
              <tr>
                <td class="Td"></td>
                <td id="Buttons" class="Td">
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
                <td class="Td"></td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </form>
</body>
</html>
