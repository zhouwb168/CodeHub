<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Project.WoodDataList.Web.Index" %>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <title>Wodeyun Project WoodDataList</title>
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
      <table width="100%" height="100%" cellpadding="0" cellspacing="0">
        <tr>
          <td id="Buttons" height="28px" style="background-color:#ddd;">
            <a href="#" unique="Report01" class="easyui-linkbutton" plain="true"><font color="red">移动检查站数据清单</font></a>
            <a href="#" unique="Report02" class="easyui-linkbutton" plain="true"><font color="normal">地磅数据清单</font></a>
            <a href="#" unique="Report03" class="easyui-linkbutton" plain="true"><font color="normal">料厂数据清单</font></a>
            <a href="#" unique="Report04" class="easyui-linkbutton" plain="true"><font color="normal">化验室数据清单</font></a>
          </td>
        </tr>
        <tr>
          <td><iframe id="Frame" width="100%" height="100%" frameborder="no" border="0" /></td>
        </tr>
      </table>
    </form>
  </body>
</html>
