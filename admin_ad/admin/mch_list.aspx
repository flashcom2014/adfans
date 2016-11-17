<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mch_list.aspx.cs" Inherits="admin_ad_admin_mch_list" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="keywords" content="adfans" />
    <link rel="icon" href="icon/favicon.ico" />
<meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
  <!-- Bootstrap 3.3.6 -->
  <link rel="stylesheet" href="/css/bootstrap.min.css"/>
  <!-- DataTables -->
  <link rel="stylesheet" href="/plugins/datatables/dataTables.bootstrap.css"/>
  <!-- Theme style -->
  <link rel="stylesheet" href="/css/AdminLTE.min.css"/>
  <!-- AdminLTE Skins. Choose a skin from the css/skins
       folder instead of downloading all of them to reduce the load. -->
    <%--<link rel="stylesheet" href="/css/skins/_all-skins.min.css" />--%>
  <link rel="stylesheet" href="/css/skins/skin-purple-light.css" />
  <!-- jQuery 2.2.0 -->
<script src="/plugins/jQuery/jQuery-2.2.0.min.js"></script>
<!-- Bootstrap 3.3.6 -->
<script src="/js/bootstrap.min.js"></script>
<!-- AdminLTE App -->
<script src="/js/AdminLTE/app.min.js"></script>
<!-- DataTables -->
<script src="/plugins/datatables/jquery.dataTables.min.js"></script>
<script src="/plugins/datatables/dataTables.bootstrap.min.js"></script>
<!-- SlimScroll -->
<script src="/plugins/slimScroll/jquery.slimscroll.min.js"></script>
    <link href="../style.css" rel="stylesheet" type="text/css" />
    <title>用户列表</title>
    <style type="text/css">
        .findtext{
            width:150px;
        }
        .amounttext{
            width:80px;
            display:inline;
        }
        .tableBorder td{
            text-align:center;
        }
        .mch{
            color:darkolivegreen;
        }
        .agent{
            color:indigo;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" >
    <div  style="margin:5px;">
        <table border="1" class="table tableBorder" style="margin-bottom:7px; width:98%; margin-left:auto; margin-right:auto;">
            <tr>
                <th>用户列表 - 充值</th>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td><label>用户名：</label></td>
                            <td><asp:TextBox ID="t_acc" CssClass="form-control findtext" runat="server"></asp:TextBox></td>
                            <td style="width:100px;"></td>
                            <td>手机号码：</td>
                            <td>
                                <asp:TextBox ID="t_mobile" CssClass="form-control findtext" runat="server"></asp:TextBox>
                            </td>
                            <td style="width:50px;"></td>
                            <td>
                                <asp:Button ID="b_find" CssClass="button" runat="server" Text="查 找" OnClick="b_find_Click" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="p_list" runat="server">
                        <asp:GridView ID="gv_mch" Width="100%" BorderWidth="0px" CssClass="tableBorder table" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True">
                            <Columns>
                                <asp:TemplateField HeaderText="用户名">
                                    <ItemTemplate>
                                        <label><%# Eval("ad_acc") + "(" + Eval("ad_mobile") + ")" %></label> 
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ad_name" HeaderText="姓名" />
                                <asp:TemplateField HeaderText="是否代理">
                                    <ItemTemplate>
                                        <%# IsAgent() %> 
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="分成比例(%)">
                                    <ItemTemplate>
                                        <label><%# Eval("scale_points") %></label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="代理账户余额(元)">
                                    <ItemTemplate>
                                        <label><%# float.Parse(Eval("agent_money").ToString())/100f %></label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="商家账户余额(元)">
                                    <ItemTemplate>
                                        <label><%# float.Parse(Eval("mch_money").ToString())/100f %></label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="奖金余额(元)">
                                    <ItemTemplate>
                                        <label><%# float.Parse(Eval("mch_points").ToString())/100f %></label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="操作">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="b_recharge" CommandName='<%# Eval("ad_id").ToString() %>' CssClass="btn-link" runat="server" Text="充值" OnCommand="b_recharge_Command" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle Height="20px" />  
                        </asp:GridView>
                    </asp:Panel>
                    <asp:Panel ID="p_recharge" runat="server" Visible="false">
                        <table style="width:400px; margin-left:auto; margin-right:auto;">
                            <tr>
                                <td>广告商：</td>
                                <td style="text-align:left;">
                                    <asp:Label ID="l_acc" runat="server" Text=""></asp:Label></td>
                            </tr>
                            <tr>
                                <td>余额：</td>
                                <td  style="text-align:left;">
                                    <asp:Label ID="l_mch_money" runat="server" ForeColor="Red" Text="0元"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>充值金额：</td>
                                <td  style="text-align:left; vertical-align:middle;">
                                    <asp:TextBox ID="t_money" CssClass="form-control amounttext" Text="0" runat="server"></asp:TextBox>元&nbsp;&nbsp; 
                                    <asp:Button ID="b_money" CssClass="button" runat="server" Text="充值" OnClientClick="return recharge_money();" OnClick="b_money_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>奖金：</td>
                                <td style="text-align:left; padding-left:10px;">
                                    <asp:Label ID="l_points" ForeColor="Red" runat="server" Text="0元"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>发放奖金：</td>
                                <td style="text-align:left;">
                                    <asp:TextBox ID="t_points" CssClass="form-control amounttext" Text="0" runat="server"></asp:TextBox>元&nbsp;&nbsp; 
                                    <asp:Button ID="b_points" CssClass="button" runat="server" Text="发放奖金" OnClientClick="return recharge_points();" OnClick="b_points_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="text-align:left;">
                                    <asp:Button ID="b_cancel" CssClass="button" runat="server" Text="取 消" OnClick="b_cancel_Click" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script>
        function recharge_points() {
            var acc = $("#l_acc").text();
            var points = $("#t_points").val();
            if (points == "") {
                alert("请输入发放奖金的金额！");
                return false;
            }
            if (confirm("确认给【" + acc + "】发放“" + points + "”元奖金？"))
                return true;
            return false;
        }
        function recharge_money() {
            var acc = $("#l_acc").text();
            var money = $("#t_money").val();
            if (money == "") {
                alert("请输入充值金额！");
                return false;
            }
            if (confirm("确认给【" + acc + "】充值“" + money + "”元？"))
                return true;
            return false;
        }
        $(document).ready(function () {
            <% if (p_list.Visible)
                { %>
            $('#gv_mch').DataTable({
                "aoColumns": [
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    { "bSortable": false },
                ],
                "aaSorting": [[3, 'asc']],
                "language": {
                    "search": "查找：",
                    "SearchPlaceholder": "查找",
                    "info": "当前第 _PAGE_ 页，共 _PAGES_ 页",
                    "emptyTable": "未查询到数据",
                    "paginate": {
                        "previous": "上一页",
                        "next": "下一页",
                        "last": "最后页",
                        "first": "第一页",


                    }
                },
                "paging": true,
                "lengthChange": false,
                "searching": true,
                "ordering": true,
                "info": false,
                "autoWidth": true
            });
            <% }%>
        });
        
    </script>
</body>
</html>
