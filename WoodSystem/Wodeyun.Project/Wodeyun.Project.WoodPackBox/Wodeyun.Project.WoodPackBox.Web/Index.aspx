<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Project.WoodPackBox.Web.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Wodeyun Project WoodPackBox</title>
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

    <link href="Styles/softkey.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/softkeyboard.js" type="text/javascript" charset="gb2312"></script>
</head>
<body>
    <form id="form1" runat="server">
      <table width="100%">
        <tr>
          <td class="Td" style="vertical-align:top; height:560px; width:515px">
            <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:25,pageList:[25,30,50]">
		          <thead>
			          <tr>
				   <th data-options="field:'SampleTime'" width="100px">取样时间</th>
                  <th data-options="field:'Sampler'" width="50px">取样人</th>
                  <th data-options="field:'UnLoadPalce'" width="60px">卸货地点</th>
                  <th data-options="field:'Tree'" width="60px">树种</th>
                  <th data-options="field:'Key'" width="50px">密码</th>
				   <th data-options="field:'PackTime'" width="100px">装箱时间</th>
                  <th data-options="field:'Box'" width="60px">送样箱号</th>
			          </tr>
		          </thead>
	          </table>
          </td>
          <td style="vertical-align:top; padding-right:10px;">
            <table width="100%">
              <tr>
                <td class="Td"></td>
                <td class="Td"></td>
              </tr>
              <tr>
                <td class="Td">取样时间：</td>
                <td class="Td">
                    <input disabled="disabled" id="txtSampleTime" type="text" style="width:150px;" />
                </td>
              </tr>
              <tr>
                <td class="Td">取样人：</td>
                <td class="Td">
                    <input disabled="disabled" id="txtSampler" type="text" style="width:150px;" />
                </td>
              </tr>
              <tr>
                <td width="64px" class="Td">卸货地点：</td>
                <td class="Td"><input disabled="disabled" id="txtUnLoadPalce" type="text" style="width:150px;" /></td>
              </tr>
              <tr>
                <td class="Td">树种：</td>
                <td class="Td">
                    <input disabled="disabled" id="txtTree" type="text" style="width:150px;" />
                </td>
              </tr>
              <tr>
                <td class="Td">密码：</td>
                <td class="Td">
                    <input disabled="disabled" id="txtKey" type="text" style="width:150px;" />
                </td>
              </tr>
              <tr>
                <td class="Td">送样箱号：</td>
                <td class="Td">
                    <input class="easyui-validatebox" data-options="required:true" id="txtBox" type="text" onclick="showkeyboard(this, 'number');" style="width:150px;" maxlength="3" />
                </td>
              </tr>
              <tr>
                <td class="Td"></td>
                <td class="Td"></td>
              </tr>
              <tr>
                <td class="Td"></td>
               <td id="Buttons" class="Td">
                  <img unique="Create" style="display:none;" class="Button" src="/Images/Create.gif" />
                  <img unique="Save" style="display:none;" class="Button" src="/Images/Save.gif" />
                  &nbsp;&nbsp;&nbsp;&nbsp;<img unique="Delete" class="Button" style="display:none" src="/Images/Delete.gif" />
                  <img unique="Cancel" class="Button" style="display:none" src="/Images/Cancel.gif" />
                  <img style="display:normal" src="/Images/Blank.gif" />
                </td>
              </tr>
                <tr style="display:none;">
                <td class="Td">
                    <input id="txtUnique" type="text" title="序号" value="0" />
                    <input id="txtRecordID" type="text" title="关联号" value="0" />
                </td>
                <td class="Td"></td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </form>
</body>
</html>
