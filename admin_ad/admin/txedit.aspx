<%@ Page Language="C#" AutoEventWireup="true" CodeFile="txedit.aspx.cs" Inherits="admin_ad_admin_txedit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="../style.css" type="text/css" />
    <link rel="stylesheet" href="/css/bootstrap.min.css"/>
    <script src="/js/bootstrap.min.js"></script>
    <script src="/plugins/jQuery/jQuery-2.2.0.min.js"></script>
    <title></title>
    <style type="text/css">
        .txt{
            width:300px;
        }
        .longtxt{
            width:95%
        }
        .righttable td{
            text-align:left;
        }
        .bcancel{
            margin-left:15px;
        }
        .auto-style1 {
            width: 120px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin:10px;">
        <table border="0" cellpadding="3" cellspacing="1" class="tableBorder" width="98%" style="margin-bottom: 7px;">
            <tr>
                <th colspan="2">提现申请</th>
            </tr>
            <tr>
                <td style="text-align:right" class="auto-style1">申请账号：</td>
                <td>
                    <asp:Label ID="l_acc" CssClass="form-control txt" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td style="text-align:right" class="auto-style1">申请人姓名：</td>
                <td>
                    <asp:Label ID="l_name" CssClass="form-control txt" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td style="text-align:right" class="auto-style1">申请人手机：</td>
                <td>
                    <asp:Label ID="l_mobile" CssClass="form-control txt" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td style="text-align:right" class="auto-style1">申请时间：</td>
                <td>
                    <asp:Label ID="l_time" CssClass="form-control txt" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td style="text-align:right" class="auto-style1">申请金额：</td>
                <td>
                    <asp:Label ID="l_amount" CssClass="form-control txt" ForeColor="Blue" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td style="text-align:right" class="auto-style1">实际打款金额：</td>
                <td>
                    <asp:Label ID="l_realamount" CssClass="form-control txt" ForeColor="Red" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td style="text-align:right" class="auto-style1">手续费：</td>
                <td>
                    <asp:Label ID="l_charge" CssClass="form-control txt"  runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td style="text-align:right" class="auto-style1">最小提限额：</td>
                <td>
                    <asp:Label ID="l_txmin" CssClass="form-control txt" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td style="text-align:right" class="auto-style1">银行卡信息：</td>
                <td>
                    <asp:Label ID="l_bank" CssClass="form-control longtxt" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td style="text-align:right" class="auto-style1">状态：</td>
                <td>
                    <asp:RadioButton ID="r0" Text="未审核" runat="server" GroupName="edit" />
                    &nbsp;
                    <asp:RadioButton ID="r2" Text="审核通过" runat="server" GroupName="edit" />
                    &nbsp;
                    <asp:RadioButton ID="r3" Text="拒绝提现" runat="server" GroupName="edit" />
                </td>
            </tr>
            <tr>
                <td style="text-align:right" class="auto-style1">汇款状态：</td>
                <td>
                    <asp:Label ID="l_remittance_status" CssClass="form-control txt" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td style="text-align:right" class="auto-style1">汇款订单：</td>
                <td>
                    <asp:Label ID="l_remittance_order" CssClass="form-control txt" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td style="text-align:right" class="auto-style1">汇款时间：</td>
                <td>
                    <asp:Label ID="l_remittance_time" CssClass="form-control txt" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td style="text-align:right" class="auto-style1">备注：</td>
                <td>
                    <asp:TextBox ID="t_remark" CssClass="form-control txt"  runat="server" Height="80px" TextMode="MultiLine"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="auto-style1"></td>
                <td >
                    <asp:Button ID="b_submit" runat="server" CssClass="button" Text=" 提交 " OnClientClick="return checktips();" OnClick="b_submit_Click" /><button type="button" class="button bcancel" onclick="history.back();"> 返回</button></td>
            </tr>
        </table>
    </div>
    </form>
    <script>
        function checktips() {
            var rr = $("input[name='edit']:checked").val();
            if (rr == "r0") {
                if (confirm("确认把此申请的状态设置为：未审核？"))
                    return true;
            }
            else if (rr == "r2") {
                if (confirm("确认把此申请的状态设置为：审核通过？"))
                    return true;
            }
            else if (rr == "r3") {
                if (confirm("确认把此申请的状态设置为：拒绝提现？"))
                    return true;
            }
            return false;
        }
    </script>
</body>
</html>
