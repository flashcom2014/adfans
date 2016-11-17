<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Admin_Login.aspx.cs" Inherits="admin_ad_Admin_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="shortcut icon" type="image/x-icon" href="http://www.weimaqi.net/adfans/images/favicon.ico" media="screen" />
    <link rel="stylesheet" href="css/index.css" />
    <script type="text/javascript" src="../js/jquery-2.2.0.min.js"></script>
    <script type="text/javascript" src="../js/jquery.qrcode.min.js"></script>
    <title>快得粉 - 管理员登陆</title>
    <style type="text/css">
        .mdiv{
            width:250px;
            height:280px;
            margin: auto;  
            position: absolute;  
            top: 0; left: 0; bottom: 0; right: 0; 
            border:1px solid #a1a1a1;
            border-radius:15px;
            padding: 20px;
        }
    </style>
</head>
<body>
    <div class="mdiv">
        <div id="qr_container" ></div>
        <div style="height:100px; text-align:center; vertical-align:middle; padding-top:10px;">
            <span style="font-size:20px; font-weight:bold; margin-top:30px; text-align:center">请用微信扫一扫登录</span>
        </div>
    </div>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
    <script>
        var GetWxInfo = function () {
            $.ajax({
                url: "Admin_login.aspx?id=<%=EncryptCode%>",
                type: 'get',
            }).done(function (data) {
                var json = eval(data)[0];
                if (json["result"] == "ok") {
                    location.href = "Default.aspx";
                }
                else if (json["result"] == "nodata") {
                    setTimeout(function () {
                        GetWxInfo();
                    }, 500);
                }
                else if (json["result"] == "no") {
                    alert(json["info"]);
                    location.href = "login.aspx";
                }
            })
        };

        $(document).ready(function () {
            $("#qr_container").qrcode({
                render: "table", //table方式 
                width: 250, //宽度 
                height: 250, //高度 
                text: "<%=QrUrl%>" //任意内容 
            });
            //

            setTimeout(function () {
                GetWxInfo();
            }, 1000);
        });
    </script>
</body>
</html>
