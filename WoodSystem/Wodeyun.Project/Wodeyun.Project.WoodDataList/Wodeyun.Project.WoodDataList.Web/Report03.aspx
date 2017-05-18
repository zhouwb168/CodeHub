<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report03.aspx.cs" Inherits="Wodeyun.Project.WoodDataList.Web.Report03" %>

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
    <script src="/Scripts/Report03.js"></script>
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
                  <td>密码:</td>
                  <td><input id="txtKey" type="text" style="width:60px;" /></td>
                <td style="width:10px">&nbsp;</td>
                  <td>货主:</td>
                  <td><input id="txtSupplier" type="text" style="width:60px;" /></td>
                <td style="width:10px">&nbsp;</td>
                  <td>车牌号:</td>
                  <td><input id="txtLicense" type="text" style="width:80px;" /></td>
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
                  <th data-options="field:'Bang_Time'" width="95px">磅单日期</th>
                  <th data-options="field:'License'" width="75px">车牌号</th>
				  <th data-options="field:'Tree'" width="45px">片 种</th>
                  <th data-options="field:'Key'" width="40px">密 码</th>
                  <th data-options="field:'Water'" width="60px">含水率 %</th>
                  <th data-options="field:'Skin'" width="60px">树皮量 %</th>
                  <th data-options="field:'Scrap'" width="60px">碎料量 %</th>
                  <th data-options="field:'Description'" width="60px">目测人</th>
                  <th data-options="field:'Deduct'" width="150px">扣重原因</th>
                  <th data-options="field:'Remark'" width="150px">备 注</th>
                  <th data-options="field:'Sampler'" width="120px">取样人</th>
                  <th data-options="field:'SampleTime'" width="95px">取样时间</th>
                  <th data-options="field:'UnLoadPalce'" width="55px">卸货地点</th>
                  <th data-options="field:'UnLoadPeople'" width="160px">卸货人</th>
                  <th data-options="field:'Supplier'" width="40px">货 主</th>
                  <th data-options="field:'jWeight'" width="60px">净重（吨）</th>
                  <th data-options="field:'Box'" width="45px">箱 号</th>
			      </tr>
		          </thead>
	          </table>
          </td>
        </tr>
      </table>
    </form>
  </body>
</html>