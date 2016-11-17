<%@ Page Language="C#" AutoEventWireup="true" CodeFile="register.aspx.cs" Inherits="admin_ad_register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <meta http-equiv="pragma" content="no-cache">
    <meta http-equiv="cache-control" content="no-cache">
    <meta http-equiv="expires" content="0">
    <link rel="stylesheet" href="style.css" type="text/css" />
    <script src="/js/jQuery-2.2.0.min.js"></script>
    <link href="/css/bootstrap.min.css" rel="stylesheet">
    <link href="/css/bootstrap-theme.min.css" rel="stylesheet">
    <link href="/css/bootstrap-switch.min.css" rel="stylesheet">
    <link href="/css/zepto.alert.css" rel="stylesheet" />
    <script src="/js/zepto.alert.js"></script>
     <link rel="stylesheet" href="css/index.css" />
    <title>广告商注册</title>
 
  

    <style type="text/css">
        .shou {
            cursor: pointer;
        }
    </style>

    <script>
        var isIE = !!window.ActiveXObject;
        var isIE6 = isIE && !window.XMLHttpRequest;
        var isIE8 = isIE && !!document.documentMode;
        var isIE7 = isIE && !isIE6 && !isIE8;
        if (isIE) {
            if (isIE6 | isIE7) {
                alert('您的浏览器版本太低了');
                window.opener = null;
                window.open('', '_self', ''); window.close();
            }
        }
    </script>
</head>

<body>
<div class="header" style="display:none">
        <div class="headcon">
            这是头部
        </div>
    </div>
     <div class="wrapcontent">
        <div class="content">
            <div class="registerform">
                 <div class="tabbtn">
                    <div class="div1"><span>用户注册</span></div>
                   
                </div>   
<form id="form1" runat="server">
<div class="formcontent">
 <div class="top">
  <div class="in3"> 
          
          
          <label for="t_mobile" class="label1">手机号码<span style="color:Red">*&nbsp;</span></label>
          <input type="text" class="in5" id="t_mobile" name="t_mobile" placeholder="手机号码" runat="server">
 </div>
 <div class="in3"> 
        
          <label for="t_code" class="label1">验证码<span style="color:Red">*&nbsp;</span></label>   
          <input type="button" id="huoqu" class="codebutn" value="免费获取"/>    
          <input type="text" class="in4" id="t_code" name="t_code" placeholder="验证码" runat="server" >
           </div>
         
        
          <label for="t_code"  class="label2" >验证码10分钟之内有效</label>   
          
           
      
          
         <div class="in3"> 
          <label for="t_acc" class="label1">用户名<span style="color:Red">*&nbsp;</span></label>
           <input type="text" class="in5" id="t_acc" name="t_acc" placeholder="用户名" runat="server">
           </div>
            <% if (HasCode)
                { %>
            <div class="in3"> 
            <label for="l_scale" class="label1">分成比率<span style="color:Red"">*&nbsp;</span></label>
            <label  id="l_scale" name="l_scale" class="in7" runat="server">0%</label>
            </div>
            <% } %>
             <div class="in3"> 
              <label for="t_paw" class="label1">密码<span style="color:Red">*&nbsp;</span></label>
            <input type="password" class="in5" id="t_paw" name="t_paw" placeholder="密码" runat="server" >
              </div>
             <div class="in3"> 
             <label for="t_paw2" class="label1">确认密码<span style="color:Red">*&nbsp;</span></label>
            <input type="password" class="in5" id="t_paw2" name="t_paw2" placeholder="确认密码" runat="server" >
              </div>
             <div class="in3"> 
             <label for="t_name" class="label1">姓名/公司名称<span style="color:Red">*&nbsp;</span></label>

            <input type="text" class="in5" id="t_name" name="t_name" placeholder="姓名/公司名称:" runat="server">
              </div>
          
            <div class="buttonregister">
                        <span class="btnregister shou" runat="server" ID="btn5"  onclick="javascript:checksubmit();" style="width: 240px;">注册</span>
                       
                    </div>
          </div>
          </div>
    <%--form id="form1" runat="server">
    <div>
        <table align="center" cellSpacing=0 cellPadding=0 border=0 style="border-collapse: collapse; margin-top:100px; width:558px;">
            <tr>
                <td class="auto-style1">手机号码:</td>
                <td>
                    <asp:TextBox ID="t_mobile" CssClass="form-control" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="auto-style1">验证码：</td>
                <td>
                    <asp:TextBox ID="t_code" CssClass="form-control code" runat="server"></asp:TextBox>
                    <input type="button" id="huoqu" class="btn btn-default pull-right" value="免费获取验证码"/> </td>
            </tr>
            <tr>
                <td class="auto-style2"></td>
                <td class="caword">验证码10分钟之内有效</td>
            </tr>
            <tr>
                <td class="auto-style1">用户名:</td>
                <td>
                    <asp:TextBox ID="t_acc" CssClass="form-control" runat="server"></asp:TextBox></td>
            </tr>
            <% if (HasCode)
                { %>
            <tr>
                <td class="auto-style1">分成比率:</td>
                <td>
                    <asp:Label ID="l_scale" CssClass="form-control" runat="server" Text="0%" ForeColor="#3366FF"></asp:Label></td>
            </tr>
            <% } %>
            <tr>
                <td class="auto-style1">密码:</td>
                <td>
                    <asp:TextBox ID="t_paw" CssClass="form-control" runat="server" TextMode="Password"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="auto-style1">确认密码:</td>
                <td>
                    <asp:TextBox ID="t_paw2" CssClass="form-control" runat="server" TextMode="Password"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="auto-style1">姓名/公司名称:</td>
                <td>
                    <asp:TextBox ID="t_name" CssClass="form-control" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="auto-style2"></td>
                <td>
                    <asp:Button ID="b_reg" CssClass="btn btn-default" runat="server" Text="注册" OnClientClick="return checksubmit();" OnClick="b_reg_Click" /></td>
            </tr>
        </table>
    </div>
    </form>--%>
    </form>
      </div>
          </div>
          </div>
      
    <script>
     $("body").keydown(function () {
        if (event.keyCode == "13") {//keyCode=13是回车键
           checksubmit();
            event.keyCode = 9;
        }
    });
        function checksubmit() {
            var mob = $('#t_mobile').val();
            if (!checkphonenum(mob)) {
                alert("请输入正确的手机号码");
                return false;
            }
            var code = $('#t_code').val();
            if (code.length != 6) {
                alert("请输入正确的验证码");
                return false;
            }
            var paw = $('#t_paw').val();
            var paw2 = $('#t_paw2').val();
            if (paw == "") {
                alert("请输入密码");
                return false;
            }
            if (paw != paw2) {
                alert("两次输入的密码不相同");
                return false;
            }
            var acc = $('#t_acc').val();
            if (acc == "") {
                alert("请输入用户名");
                return false;
            }
            var name = $('#t_name').val();
            if (name == "") {
                alert("请输入姓名或公司名");
                return false;
            }

            $.mydialog.open();
            $("#form1").submit();
            return true;
        }
        function checkphonenum(tel)
        {
            if (tel.length != 11) {
                return false;
            }
            //var reg = /(^(([0\+]\d{2,3}-)?(0\d{2,3})-)(\d{7,8})(-(\d{3,}))?$)|(^0{0,1}1[3|4|5|6|7|8|9][0-9]{9}$)/;
            var reg = /^0{0,1}1[3|4|5|6|7|8|9][0-9]{9}$/;

            if (!reg.test(tel)) {
                return false;
            }
            return true;
        }
        //验证码倒数代码
        var wait=60;
        function time(o) {
            if (wait == 0) {
                o.removeAttribute("disabled");
                o.value="免费获取验证码";
                wait = 60;
            } else {
                o.setAttribute("disabled", true);
                o.value=wait+"s后可以重新发送";
                o.style.backgroundColor="#fff";
                wait--;
                setTimeout(function() {
                    time(o)
                },1000)
            }

        }
        //document.getElementById("huoqu").onclick=function(){}
        $("#huoqu").click(function () {
            var but = this;

            var mob = $('#t_mobile').val();
            if (mob == "")
            {
                $.mydialog.alert("请先输入手机号码");
                return false;
            }
            if (!checkphonenum(mob)) {
                $.mydialog.alert("手机号码格式错误");
                return false;
            }
            $.mydialog.open();
            $.ajax({
                url: "register.aspx?action=code",
                data: "mobile=" + mob,
                type: 'post',
            }).done(function (data) {
                $.mydialog.close();
                var json = eval(data)[0];
                if (json["result"] == "ok") {
                    $.mydialog.alert("验证码发送成功，请留意手机短信");
                    time(but);
                }
                else if (json["result"] == "no") {
                    $.mydialog.alert(json["info"]);
                }
            }).error(function (d) {
                if (d.readyState != 0 && d.status != 0) {
                    alert("网络出错");
                }
            });
            return false;
        });

        $(document).ready(function () {
            <%=scripts %>
            
        });
    </script>
</body>
</html>
