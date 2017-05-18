<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Project.WoodJoin.Web.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Wodeyun Project WoodJoin</title>
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
      <div id="divTab" class="easyui-tabs" data-options="fit:false,border:true">
            <div title="地磅称重数据对接" style="padding:10px;">
                <table>
              <tr>
                <td valign="center">过磅开始日期：<input id="ipDateOfBang" type="text" class="easyui-datebox" style="width:100px;" /></td>
                <td><img id="imgQueryForBang" class="Button" style="display:normal" src="/Images/Query.gif" /></td>
              </tr>
            </table>
                 <div style="margin-top:5px; height: 190px">
                     <table id="GridOfBang" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:4,pageList:[4,10,20]">
		          <thead>
			          <tr>
                  <th data-options="field:'Bang_Time'" width="120px">过磅时间</th>
                  <th data-options="field:'carCID'" width="100px">车号</th>
                  <th data-options="field:'userXHName'" width="100px">卸车员</th>
                  <th data-options="field:'breedName'" width="126px">品种</th>
                  <th data-options="field:'carUser'" width="100px">送货员</th>
                  <th data-options="field:'jWeight'" width="120px">净重（单位：吨）</th>
                  <th data-options="field:'firstBangUser'" width="100px">过磅员</th>
			          </tr>
		          </thead>
	          </table>
                 </div>
                 <table style="border:1px solid #ccc; width: 800px; margin-top:10px">
                     <tr>
                         <td style="border-style: none solid none none; border-width: 1px; border-color: #ccc; vertical-align:top; text-align:left; width:400px">
                             <table>
                     <tr style="display:none">
                         <td>短信报备：</td>
                         <td style="color:red">
                             <input id="Gsm" type="checkbox" /> （勾选后，该车木料将显示在木片报备信息整理报表中）</td>
                         </tr>
                     <tr>
                         <td style="white-space:nowrap">水份打折：</td>
                         <td style="color:red">
                             <input disabled="disabled" id="Rebate" type="checkbox" /> （勾选后，报表中该车木料将显示为水份打折）</td>
                         </tr>
                                 <tr>
                         <td>&nbsp;</td>
                         <td style="color:red" >说明：下面列表中找不到可对接的电子卡记录时，请检查车号中数字0是否被错误写成字母o，或者去掉车号的第一个字，点击 模糊查询；不行再去掉最后一个字，直到找到......</td>
                         </tr>
                                 <tr>
                         <td></td>
                         <td style="padding-top:5px"><a id="aJoin" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">对接</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;车号：<input id="txtLicense" type="text" style="width:100px;" />&nbsp;<a id="aQueryForNoSure" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">模糊查询</a></td>
                         </tr>
                                 </table>
                         </td>
                         <td style="text-align:left">
                              <table>
                     <tr>
                         <td></td>
                         <td>上面列表中品种为木头、甲醛、粗沙等非木片的过磅记录，请进行过滤</td>
                         </tr>
                         <tr>
                         <td></td>
                         <td style="color:red">注意！系统使用定时机制，每隔1小时会从称重系统中获取最新数据来更新上面列表；对于置0或被删除作废的过磅记录，上面列表中会同时出现两条同样的记录，请将其中一条 净重 为0的记录过滤掉</td>
                         </tr>
                                 <tr>
                         <td></td>
                         <td style="padding-top:5px; text-align:center"><a id="aFilte" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">过滤</a></td>
                         </tr>
                                 </table>
                         </td>
                     </tr>
                 </table>
              <div style="margin-top:20px;padding-left:4px;">可对接的电子卡系统数据列表（必选项）：</div>
              <div style="margin-top:4px; height: 200px">
                <table id="GridOfWood" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:4,pageList:[4,10,20]">
		          <thead>
			          <tr>
                  <th data-options="field:'WeighTime'" width="120px">过磅时间</th>
                  <th data-options="field:'License'" width="100px">车号</th>
                  <th data-options="field:'Supplier'" width="100px">卸车员</th>
                  <th data-options="field:'Tree'" width="126px">品种</th>
                  <th data-options="field:'Driver'" width="100px">送货员</th>
                  <th data-options="field:'Area'" width="120px">区域</th>
                  <th data-options="field:'Description'" width="100px">过磅员</th>
                  <th data-options="field:'Place'" width="100px">移动检查站</th>
			          </tr>
		          </thead>
	          </table>
          </div>
    </div>
            <div title="已成功对接的数据" style="padding:10px;">
                 <table style="width:800px">
        <tr>
          <td height="40px" style="text-align:center">
            <table>
              <tr>
                <td valign="center">过磅开始日期：<input id="ipDateOfJoin" type="text" class="easyui-datebox" style="width:100px;" /></td>
                <td style="width:10px">&nbsp;</td>
                  <td>车牌号（不包含省份）:</td>
                  <td><input id="txtLicenseForJoinSearch" type="text" style="width:80px;" /></td>
                <td style="width:10px">&nbsp;</td>
                <td><img id="imgQueryForJoin" class="Button" style="display:normal" src="/Images/Query.gif" /></td>
                <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                <td><a id="aCutOff" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">断开对接</a></td>
              </tr>
            </table>
          </td>
        </tr>
        <tr>
          <td style="height:470px">
            <table id="GridOfJoin" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:20,pageList:[15,20,50,Setter.Max]">
		          <thead>
			          <tr>
                  <th data-options="field:'Bang_Time'" width="120px">过磅时间（地磅）</th>
                  <th data-options="field:'carCID'" width="80px">车号（地磅）</th>
                  <th data-options="field:'userXHName'" width="90px">卸车员（地磅）</th>
                  <th data-options="field:'breedName'" width="120px">品种（地磅）</th>
                  <th data-options="field:'carUser'" width="90px">送货员（地磅）</th>
                  <th data-options="field:'bbbb'" width="20px"></th>
                  <th data-options="field:'WeighTime'" width="120px">过磅时间（电子卡）</th>
                  <th data-options="field:'License'" width="89px">车号（电子卡）</th>
                  <th data-options="field:'Supplier'" width="100px">卸车员（电子卡）</th>
                  <th data-options="field:'Tree'" width="90px">品种（电子卡）</th>
                  <th data-options="field:'Driver'" width="100px">送货员（电子卡）</th>
                  <th data-options="field:'cccccc'" width="20px"></th>
				  <th data-options="field:'JoinTime'" width="110px">对接时间</th>
			          </tr>
		          </thead>
	          </table>
          </td>
        </tr>
      </table>
            </div>
          <div title="已被过滤的数据" style="padding:10px;">
              <table style="width:800px">
        <tr>
          <td height="40px" style="text-align:center">
            <table>
              <tr>
                <td valign="center">过磅开始日期：<input id="ipDateOfFilter" type="text" class="easyui-datebox" style="width:100px;" /></td>
                <td><img id="imgQueryForFilter" class="Button" style="display:normal" src="/Images/Query.gif" /></td>
                <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                <td><a id="aRenew" href="JavaScript:void(0);" class="easyui-linkbutton" plain="false">恢复</a></td>
              </tr>
            </table>
          </td>
        </tr>
        <tr>
          <td style="height:470px">
            <table id="GridOfFilter" class="easyui-datagrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:20,pageList:[15,20,50,Setter.Max]">
		          <thead>
			          <tr>
                  <th data-options="field:'Bang_Time'" width="120px">过磅时间</th>
                  <th data-options="field:'carCID'" width="100px">车号</th>
                  <th data-options="field:'userXHName'" width="100px">卸车员</th>
                  <th data-options="field:'breedName'" width="126px">品种</th>
                  <th data-options="field:'jWeight'" width="112px">净重（单位：吨）</th>
                  <th data-options="field:'firstBangUser'" width="100px">首磅员</th>
                  <th data-options="field:'carUser'" width="100px">送货员</th>
			          </tr>
		          </thead>
	          </table>
          </td>
        </tr>
      </table>
          </div>
      </div>
<div style="display:none">
    <input id="txtUnique" type="text" title="要断开对接的记录编号" value="0" />
    <input id="txtCutOffBangID" type="text" title="要断开对接地磅货重编号" value="0" />
    <input id="txtCutOffBangCID" type="text" title="要断开对接地磅货重编号" value="0" />
    <input id="txtBangID" type="text" title="要对接的地磅货重编号" value="0" />
    <input id="txtBangCID" type="text" title="要对接的地磅货重编号" value="0" />
    <input id="txtBangTime" type="text" title="要查询的地磅首磅时间" value="" />
    <input id="txtWoodID" type="text" title="要对接的木材编号" value="0" />
    <input id="txtGsmID" type="text" title="要对接的短信报备编号" value="0" />
    <input id="txtRenewID" type="text" title="要恢复非过滤状态的记录编号" value="0" />
</div>
    </form>
</body>
</html>
