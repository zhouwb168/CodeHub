<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Project.Wood.Web.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Wodeyun Project Wood</title>
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
            <div title="发放电子卡" style="padding:20px;">
                <div style="height:190px;">
                <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:5,pageList:[5,10,20]">
		          <thead>
			          <tr>
                  <th data-options="field:'CardNumber'" width="100px">绿卡号</th>
                  <th data-options="field:'ArriveDate'" width="651px">进厂时间</th>
			          </tr>
		          </thead>
	          </table>
          </div>
                <table style="margin-top:70px;">
              <tr>
                <td class="Td">绿 卡 号：</td>
                <td class="Td"><input id="txtCard" maxlength="10" disabled="disabled" type="text" class="easyui-validatebox" style="width:200px;" data-options="required:true" /> <a id="aReadCard" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">发卡</a> (右江区本地车辆，需要发给一张进门的电子卡)</td>
              </tr>
              <tr>
                <td class="Td"></td>
                <td class="Td"></td>
              </tr>
              <tr>
                <td class="Td">&nbsp;</td>
                <td class="Td">&nbsp;</td>
              </tr>
              <tr>
                <td class="Td"></td>
                <td id="Buttons" class="Td">
                  <img unique="Create" style="display:none;" class="Button" src="/Images/Create.gif" />
                  <img unique="Save" class="Button" src="/Images/Save.gif" />
                  &nbsp;&nbsp;&nbsp;&nbsp;<img unique="Delete" class="Button" style="display:none" src="/Images/Delete.gif" />
                  <img unique="Cancel" class="Button" style="display:none" src="/Images/Cancel.gif" />
                  <img style="display:normal" src="/Images/Blank.gif" />
                </td>
              </tr>
                <tr>
                <td style="display:none;">
                    <input id="txtUnique" type="text" title="序号" value="0" />
                    <input id="txtCID" type="text" title="绿卡编号" value="0" />
                </td>
              </tr>
              <tr>
                <td class="Td">&nbsp;</td>
                <td class="Td">&nbsp;</td>
              </tr>
              <tr>
                <td class="Td">&nbsp;</td>
                <td class="Td">&nbsp;</td>
              </tr>
              <tr>
                <td class="Td">&nbsp;</td>
                <td class="Td">&nbsp;</td>
              </tr>
              <tr>
                <td class="Td">&nbsp;</td>
                <td class="Td">&nbsp;</td>
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
