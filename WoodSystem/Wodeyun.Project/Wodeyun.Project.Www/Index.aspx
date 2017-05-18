<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Project.Www.Index" %>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <title>木材采购管理系统</title>
    <link href="/Scripts/easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="/Scripts/easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/Style.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery/jquery-1.8.0.min.js"></script>
    <script src="/Scripts/easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="/Scripts/easyui/locale/easyui-lang-zh_CN.js" type="text/javascript"></script>
    <script src="/Scripts/json/json2.js" type="text/javascript"></script>
    <script src="/Scripts/moment/moment.min.js" type="text/javascript"></script>
    <script src="/Scripts/Javascript.js" type="text/javascript"></script>
    <script src="/Scripts/Index.js" type="text/javascript"></script>
  </head>
  <body>
    <form runat="server">
      <table width="100%" height="100%" cellpadding="0" cellspacing="0">
        <tr>
          <td height="40px">
            <table width="100%" height="100%" style="background-image:url('/Images/Black.gif');">
              <tr>
                <td width="40px" align="center"><img src="/Images/Small.png" /></td>
                <td style="color:#999;font-size:20px;font-family:'Microsoft YaHei'">木材采购管理系统</td>
                <td align="right" style="color:#999;font-size:16px;font-family:'Microsoft YaHei'">欢迎您，</td>
                <td id="Username" width="48px" style="color:#999;font-size:16px;font-family:'Microsoft YaHei'" runat="server">用户</td>
                <td width="6px">&nbsp;</td>
              </tr>
            </table>
          </td>
        </tr>
        <tr>
          <td>
            <table width="100%" height="100%" cellpadding="0" cellspacing="0">
              <tr>
                <td width="153px" style="padding:6px 6px 6px 6px;background-image:url('/Images/Black.gif');">
                  <table id="Grid" class="easyui-treegrid" fit="true" data-options="idField:'Unique',treeField:'Name'">
		                <thead>
			                <tr>
				                <th data-options="field:'Name'" width="133px">名称</th>
			                </tr>
		                </thead>
	                </table>
                </td>
                <td><iframe id="Frame" width="100%" height="100%" frameborder="no" border="0" /></td>
               </tr>
            </table>
          </td>
        </tr>
      </table>
    </form>
  </body>
</html>
