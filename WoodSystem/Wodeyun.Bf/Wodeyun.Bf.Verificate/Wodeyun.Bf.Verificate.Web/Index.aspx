<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Bf.Verificate.Web.Index" %>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <title>Wodeyun Project Verificate</title>
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
    <form>
      <table width="100%" height="100%">
        <tr>
          <td width="400px" class="Td" style="vertical-align:top">
            <table id="Grid" class="easyui-treegrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:20,idField:'Number',treeField:'LinkName'">
		          <thead>
			          <tr>
				          <th data-options="field:'LinkName'" width="270">链接</th>
                  <th data-options="field:'AccountDescription'" width="100">姓名</th>
			          </tr>
		          </thead>
	          </table>
          </td>
          <td width="400px" style="vertical-align:top">
            <table width="100%">
              <tr>
                <td id="Buttons" class="Td">
                  <img unique="Create" class="Button" style="display:normal" src="/Images/Create.gif" />
                  <img unique="Delete" class="Button" style="display:none" src="/Images/Delete.gif" />
                  <img unique="Save" class="Button" style="display:none" src="/Images/Save.gif" />
                  <img unique="Cancel" class="Button" style="display:none" src="/Images/Cancel.gif" />
                  <img style="display:normal" src="/Images/Blank.gif" />
                </td>
              </tr>
              <tr>
                <td style="display:none;"><input id="Unique" type="text" title="序号" /></td>
              </tr>
            </table>
            <table width="100%">
              <tr>
                <td width="25%" class="Td">链接：</td>
                <td width="75%" class="Td-Combo"><select id="Link" class="easyui-combobox" disabled data-options="valueField:'Unique',textField:'Name',width:284" /></td>
              </tr>
              <tr>
                <td id="Label" class="Td">密码/用户名：</td>
                <td class="Td"><input id="Value" type="text" class="easyui-validatebox" disabled data-options="required:true" style="width:100%" /></td>
              </tr>
              <tr>
                <td class="Td">备注：</td>
                <td class="Td"><textarea id="Remark" disabled style="width:100%;height:100px;"></textarea></td>
              </tr>
            </table>
          </td>
          <td width="*">&nbsp;</td>
        </tr>
      </table>
      
    </form>
  </body>
</html>
