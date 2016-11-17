<%@ Page Language="C#" AutoEventWireup="true" CodeFile="request_withdrawals.aspx.cs" Inherits="admin_ad_withdraw_request_withdrawals" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="pragma" content="no-cache">
    <meta http-equiv="cache-control" content="no-cache">
    <meta http-equiv="expires" content="0">
    <link href="../style.css" rel="stylesheet" type="text/css" />
    <script src="/js/jQuery-2.2.0.min.js"></script>
    <link href="/css/bootstrap.min.css" rel="stylesheet">
    <link href="/css/bootstrap-theme.min.css" rel="stylesheet">
    <link href="/css/bootstrap-switch.min.css" rel="stylesheet">
    <link href="/css/zepto.alert.css" rel="stylesheet" />
    <script src="/js/zepto.alert.js"></script>
    <link href="/css/select2.css" rel="stylesheet" />
    <script src="/js/luhmjs.js?v=1.1"></script>
    <script src="/js/select2.js?v=1.1"></script>
    <title>提现</title>
    <style type="text/css">
        .lefttd{
            width:200px;
        }
        .rightbutton{
            right:0px;
            top:3px;
            position:absolute;
        }
        .txtmoney{
            width:80px;
            margin-left:auto;
            margin-right:auto;
            display:inline;
        }
        .tableBorder th{
            text-align:center;
        }
        .tableBorder td{
            text-align:center;
        }
        .bankinfo{
            width:200px;

        }
        .bcancel{
            margin-left:50px;
        }
        .bankinfo2{
            width:300px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="1" cellspacing="1" class="tableBorder table" style="width:95%; margin-left:auto; margin-right:auto;" >
            <tr>
                <th colspan="3">提现申请</th>
            </tr>
            <% if (MyAdMch.IsAgent)
                { %>
            <tr >
                <td class="lefttd">代理余额：</td>
                <td style="position:relative; height:30px;">
                    <asp:Label ID="l_agentmoney" runat="server" ForeColor="Red" Text="0元" Font-Bold="True" Font-Size="Larger"></asp:Label>
                    &nbsp;<span style="color: blue;">(*代理的余额需先转移商家余额再申请提现)</span>
                    
                </td>
                <td>
                    <asp:Button ID="b_agent" runat="server" Text="把代理余额转移到商家余额" OnClientClick="return AgentTips();" OnClick="b_agent_Click" CssClass="btn-link" />
                </td>
            </tr>
            <% } %>
            <% if (showtx)
                { %>
            <tr>
                <th>可提余额</th>
                <th>需提取的余额</th>
                <th>操作</th>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="l_mchmoney" runat="server" ForeColor="Red" Font-Size="Large" Text="0元"></asp:Label></td>
                <td>
                    <asp:TextBox ID="t_money" runat="server" CssClass="form-control txtmoney" Text="0"></asp:TextBox>元</td>
                <td class="lefttd">
                    <asp:Button ID="b_mch" runat="server" Text="申请提现" CssClass="btn-link" OnClientClick="return tx();" OnClick="b_mch_Click" /></td>
            </tr>
            <% }
    else
    { %>
            <tr>
                <td class="lefttd">提现金额：</td>
                <td colspan="2" style="text-align:left">
                    <asp:Label ID="l_txmoney" runat="server" ForeColor="Blue" Font-Size="20px" Text="0元"></asp:Label></td>
            </tr>
            <tr>
                <td class="lefttd">收款姓名：</td>
                <td colspan="2">
                    <asp:TextBox ID="t_account_name" CssClass="form-control bankinfo" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="lefttd">开户行</td>
                <td colspan="2" style="text-align:left;">
                    <select name="bankcode" id="bankcode" runat="server" class="bankcode form-control bankinfo" placeholder="选择开户行"></select>
                </td>
            </tr>
            <tr>
                <td class="lefttd">银行卡号：</td>
                <td colspan="2">
                    <asp:TextBox ID="t_account_no" CssClass="form-control bankinfo" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Button ID="b_tx" CssClass="button" runat="server" Text="申请提现" OnClientClick="return txsubmit();" OnClick="b_tx_Click" />
                    <asp:Button ID="b_cancel" runat="server" CssClass="button bcancel" Text="取消" OnClick="b_cancel_Click" />
                </td>
            </tr>
            <% } %>
        </table>
        <table border="1" cellspacing="1" class="tableBorder table" style="width:99%; margin-left:auto; margin-right:auto; " >
            <tr>
                <th colspan="4">提现申请审核中</th>
            </tr>
            <tr>
                <th>收款账号</th>
                <th>申请时间</th>
                <th>金额</th>
                <th>状态</th>
            </tr>
            <tr>
                <%= html %>
            </tr>
        </table>
    </div>
    </form>
    <script>
        $(document).ready(function () {
            /*pickout.to({
                el: '.bankcode',
                theme: 'clean',
                search: true
            });

            pickout.updated('.bankcode');*/
            //
            $('.bankcode').select2({
                language: "zh-CN"
            });
        });
        function txsubmit() {
            var name = $("#t_account_name").val();
            if (name == "") {
                alert("请输入收款姓名！");
                return false;
            }
            var no = $("#t_account_no").val();
            if (no == "") {
                alert("请输入收款银行卡号！");
                return false;
            }
            return true;
        }
        function tx() {
            var money = $("#t_money").val();
            var num = parseInt(money);
            if (num == undefined || num == null || num <= 0) {
                alert("请输入正确的金额！");
                return false;
            }
            return true;
        }
        function AgentTips() {
            var txt = $("#l_agentmoney").text();
            if (txt == "0元") {
                alert("您的代理余额为0，无款可转！");
                return false;
            }
            if (confirm("你确认把代理余额“" + txt + "”都转到商家余额？")) {
                return true;
            }
            return false;
        }
    </script>
</body>
</html>
