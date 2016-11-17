<%@ Page Language="C#" AutoEventWireup="true" CodeFile="admin_Loginxg_QR.aspx.cs" Inherits="admin_loginxg_QR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, user-scalable=no" />
    <meta http-equiv="pragma" content="no-cache"/>
    <meta http-equiv="cache-control" content="no-cache"/>
    <meta http-equiv="expires" content="0"/>
    <title>adfans - 微信扫码</title>
    <link href="../css/bootstrap.min.css" rel="stylesheet">
    <style type="text/css">
        .perror{
            text-align:center;
            vertical-align:middle;

        }
        .Center-Container {  
          position: relative;  
        } 
        .Absolute-Center {  
              margin: auto;  
              position: absolute;  
              top: 0; left: 0; bottom: 0; right: 0;  
              text-align:center;
              vertical-align:middle;
              display:block;
        }
        .mytxt{
            margin-left:30px;
            margin-right:30px;
        }
    </style>
</head>
<body style="height:100%">
    <form id="form1" runat="server">
    <div class="Absolute-Center">
        <asp:Panel ID="p_error" CssClass="Absolute-Center" Visible="false" runat="server">
            <asp:Label ID="l_error" CssClass="Absolute-Center mytxt" Height="30px" ForeColor="Red" Font-Size="30px" runat="server" Text="Label"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="p_login" CssClass="Absolute-Center" Visible="false" runat="server">
            <div class="Absolute-Center mytxt" style="height:80px;">
                <asp:Label ID="l_login"  ForeColor="Blue" Font-Size="18px" runat="server" Text="请点击确认进行登录"></asp:Label>
               
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
