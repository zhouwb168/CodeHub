<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Bf.WoodCard.Web.Index" %>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <title>Wodeyun Project WoodCard</title>
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
            <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:20">
		          <thead>
			          <tr>
				          <th data-options="field:'CID'" width="200px">电子卡</th>
				          <th data-options="field:'CardNumber'" width="170px">新编号</th>
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
                <td width="25%" class="Td">电子卡：</td>
                <td width="75%" class="Td"><input id="txtCID" maxlength="10" type="text" class="easyui-validatebox" disabled="disabled" style="width:180px" data-options="required:true" maxlength="50" />  <a id="aReadCard" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">读卡</a></td>
              </tr>
              <tr>
                <td class="Td">新编号：</td>
                <td class="Td"><input id="txtCardNumber" type="text" class="easyui-validatebox" disabled="disabled" style="width:100%" data-options="required:true" maxlength="50" /></td>
              </tr>
              <tr>
                <td class="Td">&nbsp;</td>
                <td class="Td-Combo">
                  &nbsp;
                </td>
              </tr>
              <tr>
                <td class="Td">&nbsp;</td>
                <td class="Td-Combo">
                  &nbsp;
                </td>
              </tr>
              <tr>
                <td class="Td" style="vertical-align:top">说明：</td>
                <td class="Td-Combo">
                  请为每一张电子卡重新编制更容易识别的编号。
                </td>
              </tr>
            </table>
          </td>
          <td>&nbsp;</td>
        </tr>
      </table>
    </form>
  </body>
</html>
