<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Project.WoodCarPhoto.Web.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Wodeyun Project WoodCarPhoto</title>
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
    <form id="form1" runat="server">
      <table width="100%" height="100%">
          <tr>
          <td id="Td1" height="28px" style="background-color:#ddd; text-align:center">
            <span style="font-weight:bold">操作说明：</span>第一个列表框为检查站发放电子卡的记录，点选列表中的记录，将会在第二个列表框中显示关联的车辆电子照片
          </td>
        </tr>
        <tr>
          <td height="28px">
            <table>
              <tr>
                <td>进站时间从</td>
                <td><input id="txtStartDate" type="text" class="easyui-datebox" style="width:100px;" /></td>
                <td style="width:10px">到</td>
                <td><input id="txtEndDate" type="text" class="easyui-datebox" style="width:100px;" /></td>
                <td style="width:10px">&nbsp;</td>
                  <td>站点:</td>
                  <td><input id="txtPlace" type="text" style="width:60px;" /></td>
                <td style="width:10px">&nbsp;</td>
                  <td>货主:</td>
                  <td><input id="txtSupplier" type="text" style="width:60px;" /></td>
                <td style="width:10px">&nbsp;</td>
                  <td>车牌号:</td>
                  <td><input id="txtLicense" type="text" style="width:80px;" /></td>
                <td style="width:10px">&nbsp;</td>
                <td><img id="Query" class="Button" style="display:normal" src="/Images/Query.gif" /></td>
              </tr>
            </table>
          </td>
        </tr>
        <tr>
          <td style="height:190px">
            <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:2,pageList:[5,50,100]">
		          <thead>
			          <tr>
				          <th data-options="field:'Place'" width="55px">站 点</th>
                  <th data-options="field:'TimeOfStation'" width="105px">进站时间</th>
                  <th data-options="field:'License'" width="75px">车牌号</th>
                  <th data-options="field:'Area'" width="120px">木材来源</th>
                  <th data-options="field:'GPS'" width="90px">GPS（发卡点）</th>
                  <th data-options="field:'PhotoNumber'" width="60px">共有照片</th>
                  <th data-options="field:'Description'" width="70px">拍照人</th>
                  <th data-options="field:'Tree'" width="52px">品 种</th>
                  <th data-options="field:'Supplier'" width="45px">货 主</th>
                  <th data-options="field:'CheckDate'" width="105px">到厂时间</th>
			      </tr>
		          </thead>
	          </table>
          </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
          <td style="height:470px;">
            <table id="GridOfPhoto" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:false,pagination:false,pageSize:50,pageList:[50,100,1000]">
		          <thead>
			          <tr>
				          <th data-options="field:'PhotoTime'" width="100px">拍照时间</th>
                  <th data-options="field:'GPS'" width="100px">GPS（拍照点）</th>
                  <th data-options="field:'Unique'" width="584px" align="center">电子照片</th>
			      </tr>
		          </thead>
	          </table>
          </td>
        </tr>
    </table>
    </form>
</body>
</html>
