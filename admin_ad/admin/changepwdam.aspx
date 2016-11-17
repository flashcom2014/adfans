<%@ Page ResponseEncoding="UTF-8" Language="C#" AutoEventWireup="true" CodeFile="changepwdam.aspx.cs" Inherits="changepwdam" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <script src="/js/jquery-2.2.0.min.js"></script>
  
    <link href="../style.css" rel="stylesheet" type="text/css" />
     <link href="/css/zepto.alert.css" rel="stylesheet" />
      <script src="/js/zepto.alert.js"></script>
      
    <title>
    
</title>
<style type="text/css" >
.persontable
{
    border:1px solid #B1B4D1;
	background:#fff;
    text-align: left;
}
.persontable td{background-color:#EAEAF3;padding-right: 2px;padding-left:5px;
  
    padding-top: 4px;
    padding-bottom: 4px;
}
	
.persontable th
{
    background-color: #B1B4D1;
    color: white;
    font-size: 12px;
    font-weight:bold;
    height: 20px;
    text-align:center;
}

    </style></head>
<body>
    <form runat="server" id="form1"  >


        <table width="98%" border="0" cellpadding="3" cellspacing="1" class="persontable" align="Center" style="margin-top: 7px;">
            <tr>
                <th colspan="2">
                    修改密码</th>
            </tr>
              
            <tr>
                <td class="forumRow" align="right" width="12%" style="height: 25px">帐户名：</td>
                <td class="forumRow" style="height: 25px">
                  <input id="hiddisset" runat="server" type="hidden" value="0" class="input" style="width:10em;"/>
                    <label id="tbmch_acc" name="tbmch_acc" runat="server"  style="width:10em;"></label></td>
            </tr>
          
            <tr>
                <td class="forumRow" align="right" width="12%" style="height: 25px">原密码：</td>
                <td class="forumRow" style="height: 25px">
                    <input name="tbpwdold" type="password"  runat="server" id="tbpwdold" class="input" style="width:10em;" /></td>
            </tr>
           
            <tr>
                <td class="forumRow" align="right" width="12%" style="height: 25px">新密码：</td>
                <td class="forumRow" style="height: 25px">
                    <input name="tbpwd" type="password" id="tbpwd"  runat="server" class="input" style="width:10em;" /></td>
            </tr>
  
            <tr>
                <td class="forumRow" align="right" width="12%" style="height: 25px">确认密码：</td>
                <td class="forumRow" style="height: 25px">
                    <input name="tbpwd2" type="password"  id="tbpwd2" runat="server" class="input" style="width:10em;" /></td>
            </tr>
          
            <tr>
                <td class="forumRow" align="right"></td>
                <td class="forumRow">
                    <input type="button" name="ok" value=" 提 交 " onclick="return check();" id="ok" class="button" />
                </td>
            </tr>
        </table>

    </form>
    <!--新加的部分-->
    <!--蒙版-->
    <div class="mengban" id="mengban">
    </div>
    </body>
    <script type="text/javascript">

        function check() {
            var pawold = $('#tbpwdold').val()
            var paw = $('#tbpwd').val();
            var paw2 = $('#tbpwd2').val();
            if (pawold == "") {
                alert("请输入旧密码");
                return false;
            }
            if (paw == "") {
                alert("请输入密码");
                return false;
            }
            if (paw != paw2) {
                alert("两次输入的密码不相同");
                return false;
            }

            $.mydialog.open();
            $("#form1").submit();
            return true;
        }
    </script>
    </html>