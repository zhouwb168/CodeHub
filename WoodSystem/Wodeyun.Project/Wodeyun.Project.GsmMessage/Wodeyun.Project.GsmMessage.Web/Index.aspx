<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wodeyun.Project.GsmMessage.Web.Index" %>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <title>Wodeyun Project GsmMessage</title>
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
    <form>
      <table height="100%">
        <tr>
          <td width="316px" class="Td" style="vertical-align:top">
            <table width="100%" height="100%" cellpadding="0" cellspacing="0">
              <tr>
                <td style="border:1px solid #ccc;">
                  <table width="100%" id="Querys" cellpadding="0" cellspacing="0">
                    <tr>
                      <td width="30%" class="Td">发送日期：</td>
                      <td width="50%" class="Td"><input unique="Date" type="text" class="easyui-datebox" style="width:145px;" /></td>
                      <td width="20%"></td>
                    </tr>
                    <tr>
                      <td class="Td">货主代码：</td>
                      <td class="Td-Combo"><select unique="Supplier" class="easyui-combobox" data-options="valueField:'Unique',textField:'Name'" style="width:145px"></select></td>
                      <td></td>
                    </tr>
                    <tr>
                      <td class="Td">发件人手机：</td>
                      <td class="Td"><input unique="Mobile" type="text" style="width:100%" /></td>
                      <td class="Td"><img unique="Query" class="Button" src="/Images/Query.gif" /></td>
                    </tr>
                  </table>
                </td>
              </tr>
              <tr>
                <td height="6px"></td>
              </tr>
              <tr>
                <td>
                  <table id="Grid" class="easyui-treegrid" fit="true" data-options="singleSelect:true,rownumbers:true,pagination:true,pageSize:20,idField:'Unique',treeField:'Date'">
		                <thead>
			                <tr>
				                <th data-options="field:'Date'" width="145px">发送时间</th>
                        <th data-options="field:'SupplierName'" width="60px">货主代码</th>
                        <th data-options="field:'Mobile'" width="80px">发件人手机</th>
			                </tr>
		                </thead>
	                </table>
                </td>
              </tr>
            </table>
          </td>
          <td width="485px" style="vertical-align:top">
            <table width="100%" id="Messages">
              <tr>
                <td width="20%" class="Td">发送时间：</td>
                <td width="80%" class="Td"><input unique="Date" type="text" disabled style="width:100%" /></td>
              </tr>
              <tr>
                <td class="Td">发件人手机：</td>
                <td class="Td"><input unique="Mobile" type="text" disabled style="width:100%" /></td>
              </tr>
              <tr>
                <td class="Td">内容：</td>
                <td class="Td"><textarea unique="Text" style="width:100%;height:100px;table-layout:fixed;word-break:break-all;"></textarea></td>
              </tr>
              <tr>
                <td class="Td">备注：</td>
                <td class="Td"><input unique="Remark" type="text" disabled style="width:100%;" /></td>
              </tr>
            </table>
            <table width="100%" height="105px">
              <tr>
                <td class="Td">
                  <table id="Items" class="easyui-datagrid" fit="true" data-options="singleSelect:true">
		                <thead>
			                <tr>
				                <th data-options="field:'Tree'" width="45px">树种</th>
                        <th data-options="field:'Make'" width="60px">打片时间</th>
                        <th data-options="field:'Origin'" width="120px">产地</th>
                        <th data-options="field:'License'" width="60px">车牌号</th>
                        <th data-options="field:'Driver'" width="80px">司机电话</th>
                        <th data-options="field:'Ship'" width="60px">起运时间</th>
                        <th data-options="field:'Line'" width="120px">行驶路线</th>
			                </tr>
		                </thead>
	                </table>
                </td>
              </tr>
            </table>
            <table width="100%">
              <tr>
                <td id="Buttons" class="Td">
                  <img unique="Link" class="Button" style="display:none" src="/Images/Link.gif" />
                  <img unique="Reply" class="Button" style="display:none" src="/Images/Reply.gif" />
                  <img unique="Rubbish" class="Button" style="display:none" src="/Images/Rubbish.png" />
                  &nbsp;&nbsp;&nbsp;&nbsp;
                  <img unique="Create" class="Button" style="display:none" src="/Images/Create.gif" />
                  <img unique="Delete" class="Button" style="display:none" src="/Images/Delete.gif" />
                  <img unique="Save" class="Button" style="display:none" src="/Images/Save.gif" />
                  <img unique="Cancel" class="Button" style="display:none" src="/Images/Cancel.gif" />
                  <img style="display:normal" src="/Images/Blank.gif" />
                </td>
              </tr>
              <tr>
                <td style="display:none;"><input id="Message" type="text" title="序号" /></td>
              </tr>
              <tr>
                <td style="display:none;"><input id="Unique" type="text" title="序号" /></td>
              </tr>
            </table>
            <table width="100%">
              <tr>
                <td width="20%" class="Td">货主代码：</td>
                <td width="80%" class="Td-Combo"><select id="Supplier" class="easyui-combobox" data-options="valueField:'Unique',textField:'Name'" style="width:371px"></select></td>
              </tr>
              <tr>
                <td class="Td">树种：</td>
                <td class="Td-Combo"><select id="Tree" class="easyui-combobox" disabled data-options="valueField:'Unique',textField:'Name'" style="width:371px"></select></td>
              </tr>
              <tr>
                <td class="Td">打片时间：</td>
                <td class="Td"><input id="Make" type="text" class="easyui-timespinner" disabled data-options="required:true" style="width:371px" /></td>
              </tr>
              <tr>
                <td class="Td">属地及产地：</td>
                <td class="Td-Combo">
                  <select id="Area" class="easyui-combobox" disabled data-options="valueField:'Unique',textField:'Name'" style="width:150px"></select>
                  <select id="Origin" class="easyui-combobox" disabled data-options="valueField:'Unique',textField:'Name'" style="width:218px"></select>
                </td>
              </tr>
              <tr>
                <td class="Td">车牌号：</td>
                <td class="Td">
                  <input id="License" type="text" class="easyui-validatebox" disabled style="width:150px" data-options="required:true" />
                  &nbsp;&nbsp;
                  <div id="Check" style="display:inline;"></div>
                </td>
              </tr>
              <tr>
                <td class="Td">司机电话：</td>
                <td class="Td"><input id="Driver" type="text" class="easyui-validatebox" disabled style="width:100%" data-options="required:true" /></td>
              </tr>
              <tr>
                <td class="Td">起运时间：</td>
                <td class="Td"><input id="Ship" type="text" class="easyui-timespinner" disabled data-options="required:true" style="width:371px" /></td>
              </tr>
              <tr>
                <td class="Td">行驶路线：</td>
                <td class="Td-Combo"><select id="Line" class="easyui-combobox" disabled data-options="valueField:'Unique',textField:'Name'" style="width:371px"></select></td>
              </tr>
              <tr>
                <td class="Td">备注：</td>
                <td class="Td"><textarea id="Remark" disabled style="width:100%;height:100px;"></textarea></td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
      <div id="LinkDialog" class="easyui-dialog" buttons="#LinkButtons" closed="true" modal="true" title="请选择备注的原始记录" style="width:700px;height:402px">
        <div style="margin:10px;">
          <table>
            <tr>
              <td>
                <table id="LinkGrid" class="easyui-datagrid" data-options="singleSelect:true,rownumbers:true,pagination:true,idField:'Unique',pageSize:8,pageList:[8,20,50]" style="width:660px;height:258px">
		              <thead>
			              <tr>
				              <th data-options="checkbox:true"></th>
				              <th data-options="field:'Date'" width="100px">发送时间</th>
                      <th data-options="field:'Text'" width="502px">货主代码</th>
			              </tr>
		              </thead>
	              </table>
              </td>
            </tr>
            <tr>
              <td class="Td">备注：</td>
            </tr>
            <tr>
              <td width="660px"><input id="LinkRemark" type="text" style="width:100%" /></td>
            </tr>
          </table>
        </div>
      </div>
      <div id="LinkButtons">
        <img unique="Save" class="Button" src="/Images/Save.gif" />
        <img unique="Cancel" class="Button" src="/Images/Cancel.gif" />
      </div>
      <div id="ReplyDialog" class="easyui-dialog" buttons="#ReplyButtons" closed="true" modal="true" title="请输入回复的短信内容" style="width:700px;height:402px">
        <div style="margin:10px;">
          <table>
            <tr>
              <td class="Td">手机：</td>
            </tr>
            <tr>
              <td width="660px"><input id="ReplyMobile" type="text" disabled style="width:100%" /></td>
            </tr>
            <tr>
              <td class="Td">文本：</td>
            </tr>
            <tr>
              <td width="660px"><textarea id="ReplyText" style="width:100%;height:100px;"></textarea></td>
            </tr>
            <tr>
              <td width="660px" id="ReplyRemark"></td>
            </tr>
          </table>
        </div>
      </div>
      <div id="ReplyButtons">
        <img unique="Save" class="Button" src="/Images/Reply.gif" />
        <img unique="Cancel" class="Button" src="/Images/Cancel.gif" />
      </div>
    </form>
  </body>
</html>
