<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report04.aspx.cs" Inherits="Wodeyun.Project.WoodReport.Web.Report04" %>

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
    <script src="/Scripts/Report04.js"></script>
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
                <td style="width:10px">&nbsp;</td>
                <td><img id="Excel" class="Button" style="display:normal" src="/Images/Excel.gif" /></td>
              </tr>
            </table>
          </td>
        </tr>
        <tr>
          <td>
            <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:false,pagination:true,pageSize:4,pageList:[4,100,Setter.Max]">
		          <thead>
			          <tr>
				          <th data-options="field:'Col1'" width="55px" rowspan="2">日期</th>
                          <th data-options="field:'Col2'" width="64px" rowspan="2">车号</th>
                          <th data-options="field:'Col3'" width="40px" rowspan="2">代号</th>
                          <th data-options="field:'Col4'" width="46px" rowspan="2">木种</th>
                          <th data-options="field:'Col5'" width="40px" rowspan="2">净重</th>
                          <th data-options="field:'Col6'" width="56px" rowspan="2">过磅员</th>
                          <th data-options="field:'Col7'" width="55px" rowspan="2">毛重时间</th>
                          <th colspan="4">目测</th>
                          <th colspan="2">对比分析</th>
				          <th data-options="field:'Co20'" width="49px" rowspan="2">腐 朽</th>
                          <th data-options="field:'Co21'" width="55px" rowspan="2">过大木片</th>
                          <th data-options="field:'Co22'" width="55px" rowspan="2">过小木片</th>
			          </tr>
                      <tr>
				          <th data-options="field:'Water'" width="50px">含水率</th>
                          <th data-options="field:'Skin'" width="55px">树皮含量</th>
                          <th data-options="field:'Scrap'" width="55px">碎料含量</th>
                          <th data-options="field:'User'" width="60px">目测人</th>
                          <th data-options="field:'Col8'" width="60px" align="center">取样人</th>
                          <th data-options="field:'Col9'" width="147px" align="center">备注</th>
			          </tr>
		          </thead>
	          </table>
          </td>
        </tr>
      </table>
    </form>
  </body>
</html>
