<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report01.aspx.cs" Inherits="Wodeyun.Project.WoodReport.Web.Report01" %>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <title>Wodeyun Project WoodReport</title>
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
                <td>首磅时间从</td>
                <td><input id="txtStartDate" type="text" class="easyui-datebox" style="width:100px;" /></td>
                <td style="width:10px">到</td>
                <td><input id="txtEndDate" type="text" class="easyui-datebox" style="width:100px;" /></td>
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
            <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:15,pageList:[15,50,100,Setter.Max]">
		          <thead>
			          <tr>
				          <th data-options="field:'WeighTime'" width="82px">首磅日期</th>
                  <th data-options="field:'Key'" width="90px">代 码</th>
                  <th data-options="field:'Tree'" width="60px">种 类</th>
                  <th data-options="field:'Water'" width="80px">含水率 %</th>
                  <th data-options="field:'Skin'" width="90px">树皮含量 %</th>
                  <th data-options="field:'Scrap'" width="90px">碎料含量 %</th>
                  <th data-options="field:'Greater'" width="100px">过大木片 %</th>
                  <th data-options="field:'Sampler'" width="170px">取样人</th>
			          </tr>
		          </thead>
	          </table>
          </td>
        </tr>
      </table>
    </form>
  </body>
</html>
