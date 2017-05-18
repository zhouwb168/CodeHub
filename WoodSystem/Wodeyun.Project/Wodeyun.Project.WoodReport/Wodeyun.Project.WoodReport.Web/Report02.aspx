<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report02.aspx.cs" Inherits="Wodeyun.Project.WoodReport.Web.Report02" %>

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
    <script src="/Scripts/Report02.js"></script>
  </head>
  <body>
    <form>
      <table width="100%" height="100%">
        <tr>
          <td height="28px">
            <table>
              <tr>
                <td id="Buttons">
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="red">本月</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">上月</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Month 2</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Month 3</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Month 4</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Month 5</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Month 6</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Month 7</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Month 8</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Month 9</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Month 10</font></a>
                  <a href="#" class="easyui-linkbutton" plain="true"><font color="normal">Month 11</font></a>
                </td>
                <td><input id="Year" class="easyui-numberspinner" style="width:52px;" /></td>
                <td>年</td>
                <td class="Td-Combo">
                  <select id="Month" class="easyui-combobox" style="width:48px">
                    <option value="1" selected>1月</option>
                    <option value="2">2月</option>
                    <option value="3">3月</option>
                    <option value="4">4月</option>
                    <option value="5">5月</option>
                    <option value="6">6月</option>
                    <option value="7">7月</option>
                    <option value="8">8月</option>
                    <option value="9">9月</option>
                    <option value="10">10月</option>
                    <option value="11">11月</option>
                    <option value="12">12月</option>
                  </select>
                </td>
                <td><img id="Query" class="Button" style="display:normal" src="/Images/Query.gif" /></td>
                <td style="width:10px">&nbsp;</td>
                <td><img id="Excel" class="Button" style="display:normal" src="/Images/Excel.gif" /></td>
              </tr>
            </table>
          </td>
        </tr>
        <tr>
          <td>
            <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:false,pagination:true,pageSize:15,pageList:[15,50,100,Setter.Max]">
		          <thead>
			          <tr>
				          <th data-options="field:'WeighTime'" width="192px">首磅日期</th>
                  <th data-options="field:'AvgWater'" width="200px">平均含水率</th>
                  <th data-options="field:'AvgSkin'" width="200px">平均树皮含量</th>
                  <th data-options="field:'AvgScrap'" width="200px">平均碎料含量</th>
			          </tr>
		          </thead>
	          </table>
          </td>
        </tr>
      </table>
    </form>
  </body>
</html>
