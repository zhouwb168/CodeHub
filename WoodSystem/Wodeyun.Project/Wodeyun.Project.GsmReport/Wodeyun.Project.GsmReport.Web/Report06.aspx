<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report06.aspx.cs" Inherits="Wodeyun.Project.GsmReport.Web.Report06" %>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <title>Wodeyun Project GsmReport</title>
    <link href="/Scripts/easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="/Scripts/easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/Style.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery/jquery-1.8.0.min.js"></script>
    <script src="/Scripts/easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="/Scripts/easyui/locale/easyui-lang-zh_CN.js" type="text/javascript"></script>
    <script src="/Scripts/json/json2.js" type="text/javascript"></script>
    <script src="/Scripts/moment/moment.min.js" type="text/javascript"></script>
    <script src="/Scripts/Javascript.js"></script>
    <script src="/Scripts/Report06.js"></script>
  </head>
  <body>
    <form>
      <table width="100%" height="100%">
        <tr>
          <td height="28px">
            <table>
              <tr>
                <td id="Buttons">
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="red">今年</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">去年</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Month 2</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Month 3</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Month 4</font></a>
                </td>
                <td><input id="Year" class="easyui-numberspinner" style="width:52px;" /></td>
                <td>年</td>
                <td><img id="Query" class="Button" style="display:normal" src="/Images/Query.gif" /></td>
                <td><img id="Excel" class="Button" style="display:normal" src="/Images/Excel.gif" /></td>
              </tr>
            </table>
          </td>
        </tr>
        <tr>
          <td>
            <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:20,pageList:[15,20,50,Setter.Max],showFooter:true">
		          <thead>
			          <tr>
				          <th data-options="field:'Number'" width="35px">排名</th>
                  <th data-options="field:'Supplier'" width="60px">货主代码</th>
                  <th data-options="field:'Trees'" width="135px">树种</th>
                  <th data-options="field:'Count'" width="35px">车数</th>
                  <th data-options="field:'Scale'" width="60px">比例</th>
			          </tr>
		          </thead>
	          </table>
          </td>
        </tr>
      </table>
    </form>
  </body>
</html>
