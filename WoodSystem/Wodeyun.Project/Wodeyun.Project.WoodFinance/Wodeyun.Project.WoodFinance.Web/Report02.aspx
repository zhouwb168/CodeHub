<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report02.aspx.cs" Inherits="Wodeyun.Project.WoodFinance.Web.Report02" %>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <title>Wodeyun Project WoodFinance</title>
    <link href="/Scripts/easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="/Scripts/easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/Style.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery/jquery-1.8.0.min.js"></script>
    <script src="/Scripts/easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="/Scripts/easyui/locale/easyui-lang-zh_CN.js" type="text/javascript"></script>
    <script src="/Scripts/json/json2.js" type="text/javascript"></script>
    <script src="/Scripts/moment/moment.min.js" type="text/javascript"></script>
    <script src="/Scripts/Javascript.js"></script>
    <script src="/Scripts/Report02.js"></script>
  </head>
  <body>
    <form>
      <table width="100%" height="100%">
        <tr>
          <td height="28px">
            <table>
              <tr>
                <td>首磅时间从</td>
                <td><input id="txtStartDate" type="text" class="easyui-datebox" style="width:100px;" /></td>
                <td style="width:10px">到</td>
                <td><input id="txtEndDate" type="text" class="easyui-datebox" style="width:100px;" /></td>
                <td style="width:10px">&nbsp;</td>
                  <td>卸车员:</td>
                  <td><input id="txtPeople" type="text" style="width:100px;" /></td>
                <td style="width:10px">&nbsp;</td>
                <td><img id="Query" class="Button" style="display:normal" src="/Images/Query.gif" /></td>
                <td style="width:20px">&nbsp;</td>
                <td><img id="Excel" class="Button" style="display:normal" src="/Images/Excel.gif" /></td>
              </tr>
            </table>
          </td>
        </tr>
        <tr>
          <td>
            <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:15,pageList:[15,50,Setter.Max]">
		          <thead>
			          <tr>
				          <th data-options="field:'UnLoadPeople'" width="384px">姓 名</th>
                  <th data-options="field:'TotalWeight'" width="384px">卸木片（吨）</th>
			          </tr>
		          </thead>
	          </table>
          </td>
        </tr>
      </table>
    </form>
  </body>
</html>
