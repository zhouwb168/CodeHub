<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Project.Check.Web.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Wodeyun Project Check</title>
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
      <div class="easyui-tabs" data-options="fit:false,border:true">
            <div title="验收电子卡" style="padding:20px;">
                <div style="height:190px;">
                <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:5,pageList:[5,10,20]">
		          <thead>
			          <tr>
                  <th data-options="field:'CardNumber'" width="170px">绿卡号</th>
                  <th data-options="field:'License'" width="100px">车牌号</th>
                  <th data-options="field:'Place'" width="100px">发卡地</th>
                  <th data-options="field:'TimeOfStation'" width="120px">进站时间</th>
                  <th data-options="field:'GPS'" width="130px">GPS距离</th>
                  <th data-options="field:'CheckDate'" width="127px">验收时间</th>
			          </tr>
		          </thead>
	          </table>
          </div>
                <table width="600px;" style="margin-top:34px;">
              <tr>
                <td class="Td">绿卡号：</td>
                <td class="Td"><input id="txtGreenCard" maxlength="10" disabled="disabled" type="text" class="easyui-validatebox" style="width:200px;" data-options="required:true" /> <a id="aCheckCard" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">验卡</a> (隆安县等检查点过来的车辆，需要验卡)</td>
              </tr>
               <tr>
                <td class="Td">车牌号：</td>
                <td class="Td">
                    <input disabled="disabled" id="txtLicense" type="text" class="easyui-validatebox" style="width:200px;" data-options="required:true" />
                </td>
              </tr>
              <tr>
                <td class="Td">发卡地：</td>
                <td class="Td"><input disabled="disabled" id="txtPlace" type="text" class="easyui-validatebox" style="width:200px;" data-options="required:true" /></td>
              </tr>
              <tr>
                <td class="Td">进站时间：</td>
                <td class="Td">
                    <input  disabled="disabled" id="txtTimeOfStation" type="text" class="easyui-validatebox" style="width:200px;" data-options="required:true" />
                    距今已过去了 <input disabled="disabled" id="txtDay" type="text" class="easyui-validatebox" style="width:160px;" data-options="required:true" />
                </td>
              </tr>
              <tr>
                <td class="Td">GPS距离：</td>
                <td class="Td"><input disabled="disabled" id="txtGPS" type="text" class="easyui-validatebox" style="width:200px;" data-options="required:true" /> 米 (5000米内有效)</td>
              </tr>
              <tr>
                <td class="Td">来源地：</td>
                <td class="Td">
                    <input disabled="disabled" id="txtArea" type="text" style="width:200px;" class="easyui-validatebox" data-options="required:true" />
                </td>
              </tr>
              <tr>
                <td class="Td"></td>
                <td class="Td"></td>
              </tr>
              <tr>
                <td class="Td"></td>
                <td id="Buttons" class="Td">
                  <img unique="Save" class="Button"  src="Images/Save.gif" />
                </td>
              </tr>
              <tr>
                <td style="display:none;">
                    <input id="txtUnique" type="text" title="序号" value="0" />
                    <input id="txtRecordID" type="text" title="关联号" value="0" />
                    <input id="txtCID" type="text" title="绿卡编号" value="0" />
                </td>
              </tr>
              <tr>
                <td class="Td">&nbsp;</td>
                <td class="Td">&nbsp;</td>
              </tr>
            </table>
             </div>
        </div>
    </form>
</body>
</html>
