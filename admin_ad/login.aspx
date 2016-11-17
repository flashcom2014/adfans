<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="admin_login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link rel="shortcut icon" type="image/x-icon" href="http://www.weimaqi.net/adfans/images/favicon.ico" media="screen" />
    <title>快得粉 - 广告商登录</title>
<link rel="stylesheet" href="css/index.css" />
    <script type="text/javascript" src="/js/jquery-2.2.0.min.js"></script>
    
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
            <div class="form">
                 <div class="tabbtn">
                    <div class="div1"><span>用户登录</span></div>
                   
                </div>   
<form id="form1" runat="server">
<div class="formcontent">
 <div class="top">
          
             
                   <input type="text" class="in1" id="uid" name="uid" placeholder="用户名或手机号" runat="server">
                 
                 
                  
                    <input type="password" class="in2" id="pwd" name="pwd" placeholder="用户密码" runat="server" >
                  
                
                  <div class="in3">
                 
                    <input type="text" class="in4" id="checkcode" name="checkcode" placeholder="验证码" runat="server" ><img id="checkimage" src="../checkcode/checkcode2.aspx" alt="看不清？点击更换" onclick="this.src=this.src+'?'" style="cursor:pointer;margin-top:10px" />
                </div>
                <div class="button">
                        <span class="btn shou" runat="server" ID="btn3"   style="width: 240px;">登 录</span>
                       
                    </div>
</div>
</div>
</form>
</div>
</div>
</div>

</body>

</html>
<script type="text/javascript">
    $("body").keydown(function (e) {
        var ev = event || e;
        if (ev.keyCode == "13") {//keyCode=13是回车键
            $('#btn3').click();
            ev.keyCode = 9;
        }
    });
    $("#btn3").click(function () {
        var uid = $("input[name='uid']").val();
        var pwd = $("input[name='pwd']").val();
        var checkcode = $("input[name='checkcode']").val();
        if (uid == "") {
            alert("请输入用户名或手机号");
            return false;
        }
        if (pwd == "") {
            alert("请输入用户密码");
            return false;
        }
        if (checkcode == "") {
            alert("请输入验证码");
            return false;
        }
        $("#form1").submit();
       
    });
    </script>
 