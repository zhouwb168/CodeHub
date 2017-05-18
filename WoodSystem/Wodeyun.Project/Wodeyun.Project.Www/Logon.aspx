<%@ Page Language="C#" AutoEventWireup="true" %>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <title>木材采购管理系统</title>
      <script type="text/javascript">
          var Setter = {
              Url: 'http://<%= this.Request.Url.Host%>:10000/ExecuteService.svc/Execute',
              Max: 1073741823
          };
      </script>
    <link href="/Scripts/easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="/Scripts/easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/Style.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery/jquery-1.8.0.min.js"></script>
    <script src="/Scripts/easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="/Scripts/easyui/locale/easyui-lang-zh_CN.js" type="text/javascript"></script>
    <script src="/Scripts/json/json2.js" type="text/javascript"></script>
    <script src="/Scripts/moment/moment.min.js" type="text/javascript"></script>
    <script src="/Scripts/Javascript.js" type="text/javascript"></script>
    <script src="/Scripts/Logon.js" type="text/javascript"></script>
  </head>
  <body>
    <form>
      <table width="100%" height="100%">
        <tr>
          <td align="center" valign="middle">
            <table width="1000px" height="500px">
              <tr>
                <td height="100px">
                  <table>
                    <tr>
                      <td><img src="/Images/Logo.gif" /></td>
                      <td>&nbsp;&nbsp;</td>
                      <td style="color:#333;font-size:30px;font-family:'Microsoft YaHei'">木材采购管理系统</td>
                    </tr>
                  </table>
                </td>
              </tr>
              <tr>
                <td align="center" valign="middle" style="background-image:url('/Images/Background.gif')">
                  <table width="900px" height="350px">
                    <tr>
                      <td width="600px">&nbsp;</td>
                      <td align="center" style="background-color:white;">
                        <table width="90%" height="100%">
                          <tr>
                            <td colspan="2" height="120px" style="color:#333;font-size:25px;font-family:'Microsoft YaHei'">登录系统</td>
                          </tr>
                          <tr>
                            <td width="64px" class="Td">登录方式：</td>
                            <td class="Td-Combo"><select id="Link" class="easyui-combobox" data-options="valueField:'Unique',textField:'Name'" style="width:166px"></select></td>
                          </tr>
                          <tr>
                            <td class="Td">用&nbsp;&nbsp;户&nbsp;&nbsp;名：</td>
                            <td class="Td"><input id="Username" type="text" class="easyui-validatebox" style="width:100%" data-options="required:true" /></td>
                          </tr>
                          <tr>
                            <td class="Td">密&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;码：</td>
                            <td class="Td"><input id="Password" type="password" class="easyui-validatebox" style="width:100%" data-options="required:true" /></td>
                          </tr>
                          <tr>
                            <td colspan="2" class="Td"><input id="Button" type="button" value="登录系统" class="Logon" /></td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </form>
  </body>
</html>
