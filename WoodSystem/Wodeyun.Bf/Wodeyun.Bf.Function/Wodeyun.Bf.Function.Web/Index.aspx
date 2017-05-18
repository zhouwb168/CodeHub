<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Bf.Function.Web.Index" %>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <title>Wodeyun Project Function</title>
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
            <table id="Grid" class="easyui-treegrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:20,idField:'Unique',treeField:'Name'">
		          <thead>
			          <tr>
				          <th data-options="field:'Name'" width="371">名称</th>
			          </tr>
		          </thead>
	          </table>
          </td>
          <td width="400px" style="vertical-align:top">
            <table width="100%">
              <tr>
                <td id="Buttons" class="Td">
                  <img unique="Create" class="Button" src="/Images/Create.gif" />
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
                <td width="25%" class="Td">父功能：</td>
                <td width="75%" class="Td-Combo"><select id="Parent" class="easyui-combotree" disabled data-options="width:284" /></td>
              </tr>
              <tr>
                <td class="Td">类型：</td>
                <td class="Td-Combo">
                  <select id="Type" class="easyui-combobox" disabled style="width:284px">
                    <option value="0" selected>业务管理</option>
                    <option value="1">基础管理</option>
                    <option value="2">系统管理</option>
                  </select>
                </td>
              </tr>
              <tr>
                <td class="Td">顺号：</td>
                <td class="Td"><input id="Order" type="text" class="easyui-numberbox" disabled style="width:100%" data-options="required:true" /></td>
              </tr>
              <tr>
                <td class="Td">编号：</td>
                <td class="Td"><input id="No" type="text" class="easyui-validatebox" disabled style="width:100%" data-options="required:true" /></td>
              </tr>
              <tr>
                <td class="Td">名称：</td>
                <td class="Td"><input id="Name" type="text" class="easyui-validatebox" disabled style="width:100%" data-options="required:true" /></td>
              </tr>
              <tr>
                <td class="Td">网址：</td>
                <td class="Td"><input id="Url" type="text" class="easyui-validatebox" disabled style="width:100%" data-options="required:true" /></td>
              </tr>
              <tr>
                <td class="Td">备注：</td>
                <td class="Td"><textarea id="Remark" disabled style="width:100%;height:100px;"></textarea></td>
              </tr>
            </table>
          </td>
          <td>&nbsp;</td>
        </tr>
      </table>
    </form>
  </body>
</html>
