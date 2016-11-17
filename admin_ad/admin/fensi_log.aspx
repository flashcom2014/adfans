<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fensi_log.aspx.cs" Inherits="admin_ad_admin_fensi_log" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="../style.css" type="text/css" />
    <title></title>
    <style type="text/css">
        .wximg {
            width:30px;
            height:30px;
        }
        .wximgbig {
            display:none;
            width:300px;
            height:300px;
            position:absolute;
            z-index:1000;
			margin-left: -100px;
        }
        .link:hover .wximgbig {
            display:block;
			border: 2px solid red;
        }
        .tableBorder td{
            text-align:center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin:10px;">
        <table border="0" cellpadding="3" cellspacing="1" class="tableBorder" width="98%" align="center" style="margin-bottom: 8px;">
            <tr>
                <th>关注记录跟踪</th>
            </tr>
            <% if (admch_html.Length > 0)
                { %>
            <tr><td></td></tr>
            <tr>
                <td>
                    <table border="0" cellpadding="3" cellspacing="1" class="tableBorder" width="100%" align="center">
                        <tr>
                            <th>终端广告商</th>
                            <th>APPID</th>
                            <th>关注花费</th>
                            <th>微信昵称</th>
                            <th>微信头像</th>
                            <th>关注时间</th>
                        </tr>
                        <tr>
                            <%= admch_html %>
                        </tr>
                    </table>
                </td>
            </tr>
            <% } %>
            <% if (adagent_html.Length > 0)
                { %>
            <tr><td></td></tr>
            <tr>
                <td>
                    <table border="0" cellpadding="3" cellspacing="1" class="tableBorder" width="100%" align="center">
                        <tr>
                            <th>广告代理商</th>
                            <th>分得金额</th>
                            <th>实际分成</th>
                            <th>子代理或商家</th>
                            <th>时间</th>
                        </tr>
                        <%= adagent_html %>
                    </table>
                </td>
            </tr>
            <% } %>
            <tr><td></td></tr>
            <tr>
                <td>
                    <table border="0" cellpadding="3" cellspacing="1" class="tableBorder" width="100%" align="center">
                        <tr>
                            <th>设备商家</th>
                            <th>设备名</th>
                            <th>金额</th>
                            <th>时间</th>
                        </tr>
                        <tr>
                            <%= mch_html %>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
