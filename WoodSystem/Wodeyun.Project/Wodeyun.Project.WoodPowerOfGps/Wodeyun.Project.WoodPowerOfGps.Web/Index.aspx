<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Bf.WoodPowerOfGps.Web.Index" %>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <title>Wodeyun Bf WoodPowerOfGps</title>
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
				          <th data-options="field:'Id'" width="80px">Id</th>
				          <th data-options="field:'Unique'" width="90px">姓名</th>
				          <th data-options="field:'Description'" width="182px">所属部门</th>
			          </tr>
		          </thead>
	          </table>
          </td>
          <td width="400px" style="vertical-align:top">
            <table width="100%" height="100%">
              <tr>
                <td id="Buttons" height="18px" class="Td">
                  <img unique="Save" class="Button" style="display:none" src="/Images/Save.gif" />
                  <img unique="Cancel" class="Button" style="display:none" src="/Images/Cancel.gif" />
                  <img style="display:normal" src="/Images/Blank.gif" />
                </td>
              </tr>
              <tr>
                <td class="Td" height="100%">
                  <table id="Power" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,idField:'Unique'">
		                <thead>
			                <tr>
				                <th data-options="checkbox:true"></th>
                        <th data-options="field:'StationName'" width="326">检查站名称</th>
			                </tr>
		                </thead>
	                </table>
                </td>
              </tr>
            </table>
          </td>
          <td >&nbsp;</td>
        </tr>
      </table>
    </form>
  </body>
</html>
