<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report01.aspx.cs" Inherits="Wodeyun.Project.GsmReport.Web.Report01" %>

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
    <script src="/Scripts/Report01.js"></script>
  </head>
  <body>
    <form>
      <table width="100%" height="100%">
        <tr>
          <td height="28px">
            <table>
              <tr>
                <td id="Buttons">
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
                <td><img id="Excel" class="Button" style="display:normal" src="/Images/Excel.gif" /></td>
              </tr>
            </table>
          </td>
        </tr>
        <tr>
          <td>
            <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:20,pageList:[15,20,50,Setter.Max]">
		          <thead>
			          <tr>
				          <th data-options="field:'Date'" width="100px">发送时间</th>
                  <th data-options="field:'SupplierName'" width="60px">货主代码</th>
                  <th data-options="field:'Mobile'" width="80px">发件人手机</th>
                  <th data-options="field:'Text'" width="403px">内容</th>
                  <th data-options="field:'Remark'" width="100px">备注</th>
			          </tr>
		          </thead>
	          </table>
          </td>
        </tr>
      </table>
    </form>
  </body>
</html>
