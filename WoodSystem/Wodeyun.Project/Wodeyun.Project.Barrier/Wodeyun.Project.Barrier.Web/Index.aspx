<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Project.Barrier.Web.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Wodeyun Project Barrier</title>
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
<script type="text/javascript" src="http://api.map.baidu.com/api?v=1.5&ak=817f1eb9a7d33947558fc70aedcdef4a"></script>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%">
        <tr>
          <td width="310px" class="Td" style="vertical-align:top; height:200px;">
            <table width="100%">
              <tr>
                <td class="Td">发卡地：</td>
                <td class="Td">
                    <input disabled="disabled" id="txtPlace" type="text" style="width:100px;" class="easyui-validatebox" data-options="required:true" />
                </td>
              </tr>
              <tr>
                <td width="64px" class="Td">绿卡号：</td>
                <td class="Td"><input id="txtCard" maxlength="10" disabled="disabled" type="text" class="easyui-validatebox" style="width:150px;" data-options="required:true" /> <a id="aReadCard" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">发卡</a></td>
              </tr>
              <tr>
                <td class="Td">车牌号：</td>
                <td class="Td">
                    <input id="txtCartChinese" type="text" style="width:30px" class="easyui-validatebox" data-options="required:true" maxlength="1" onclick="showkeyboard(this, 'chinese');" />
                    <input id="txtCartNumber" type="text" style="width:117px" class="easyui-validatebox" data-options="required:true" maxlength="7" onclick="showkeyboard(this, 'numlet');" />
                </td>
              </tr>
              <tr>
                <td class="Td">来源地：</td>
                <td class="Td">
                    <input id="txtArea" type="text" style="width:150px;" class="easyui-validatebox" data-options="required:true" />
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
                    <input id="txtRecordID" type="text" title="关联号" value="0" />
                    <input id="txtImageFileName" type="text" title="相片文件名称" value="" />
                </td>
              </tr>
            </table>
          </td>
          <td style="vertical-align:top;">
            <table id="Grid" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:false,pagination:true,pageSize:5,pageList:[5,10,20]">
		          <thead>
			          <tr>
				          <th data-options="field:'CardNumber'" width="50px">绿卡号</th>
                  <th data-options="field:'TimeOfStation'" width="110px">进站时间</th>
                  <th data-options="field:'License'" width="73px">车牌号</th>
                  <th data-options="field:'PhotoNumber'" width="60px">已拍照片</th>
                  <th data-options="field:'GPS'" width="70px">GPS距离</th>
                  <th data-options="field:'Area'" width="120px">来源地</th>
			          </tr>
		          </thead>
	          </table>
          </td>
        </tr>
      </table>
    <div style="width:100%; margin-top:20px; padding-left:10px;">
         <!-- layout主体开始 -->
<div class="easyui-layout" style="width:816px; height:330px;">
    <!-- layout north开始 -->
    <div data-options="region:'north',split:false,iconCls:'icon-search'" title=" " style="height:80px;padding:10px">
      <span style="margin-left:110px"><a id="aPhoto" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">拍照</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a id="aSaveImage" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">发送照片</a></span>
    </div>
    <!-- layout north结束 -->
    <!-- layout westh开始 -->
    <div region="west" split="false" title="" style="width:400px; text-align: center">
         <img id="imgPhoto" src="Images/msn.gif" alt="" style="width:300px; height:200px; margin-top: 26px; margin-left:auto; margin-right:auto" />
    </div>
    <!-- layout westh结束 -->
    <!-- layout center开始 -->  
    <div region="center" title="" style="text-align:center">
        <div style="margin-top:10px">当前距离指定点的GPS距离：<input disabled="disabled" id="txtGPS" type="text" style="width:100px" class="easyui-validatebox" data-options="required:true" /> 米</div>
        <div id="divBaiDuMap" style="width:300px; height:180px; margin-top:20px; margin-left:auto; margin-right:auto"></div>
    </div>
    <!-- layout center结束 -->     
</div> 
 <!-- layout主体结束 -->
   </div>
    </form>
</body>
</html>
