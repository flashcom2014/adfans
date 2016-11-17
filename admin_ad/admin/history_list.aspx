<%@ Page Language="C#" AutoEventWireup="true" CodeFile="history_list.aspx.cs" Inherits="admin_ad_admin_history_list" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
      <!-- jQuery 2.2.0 -->
<script src="/plugins/jQuery/jQuery-2.2.0.min.js"></script>
    <link rel="stylesheet" href="../style.css" type="text/css" />
    <script src="/js/My97DatePicker/WdatePicker.js" type="text/javascript" ></script>
    <link href="/js/My97DatePicker/skin/WdatePicker.css" rel="stylesheet" type="text/css">
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

<!-- Bootstrap 3.3.6 -->
<script src="/js/bootstrap.min.js"></script>
<!-- AdminLTE App -->
<script src="/js/AdminLTE/app.min.js"></script>
<!-- DataTables -->
<script src="/plugins/datatables/jquery.dataTables.min.js"></script>
<script src="/plugins/datatables/dataTables.bootstrap.min.js"></script>
<!-- SlimScroll -->
<script src="/plugins/slimScroll/jquery.slimscroll.min.js"></script>
    <title></title>
    <style type="text/css">
        .tableBorder td{
            text-align:center;
        }
        .tableBorder th{
            text-align:center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin:10px;">
        <table border="0" cellpadding="3" cellspacing="1" class="tableBorder" width="98%" align="center" style="margin-bottom: 7px;">
            <tr>
                <th colspan="4">吸粉记录</th>
            </tr>
            <tr>
                <td>广告商：<asp:TextBox ID="t_admch" runat="server"></asp:TextBox></td>
                <td>APPID:<asp:TextBox ID="t_appid" runat="server"></asp:TextBox></td>
                <td>日期：<asp:TextBox ID="t_date" runat="server" onclick="WdatePicker({readOnly:true,isShowToday:true})"></asp:TextBox></td>
                <td>
                    <asp:Button ID="b_search" runat="server" CssClass="button" Text="搜索" /></td>
            </tr>
            <tr>
                <td colspan="4"></td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView ID="gv_list" Width="100%" BorderWidth="0px" CssClass="tableBorder table" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True">
            <Columns>
                <asp:BoundField DataField="subscribe_time" HeaderText="关注时间" />
                 <asp:TemplateField HeaderText="设备名">
                    <ItemTemplate>
                        <span><%# GetDeciveName() %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="设备商">
                    <ItemTemplate>
                        <%# GetMchAcc() %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="设备商收入(元)">
                    <ItemTemplate>
                        <span><%# (float.Parse(Eval("get_amount").ToString())/100f).ToString("F2") %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="广告商">
                    <ItemTemplate>
                        <span><%# GetAdName() %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="APPID">
                    <ItemTemplate>
                        <span><%# Eval("appid") %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="广告商支出(元)">
                    <ItemTemplate>
                        <span><%# (float.Parse(Eval("charge").ToString())/100f).ToString("F2") %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="关注跟踪">
                    <ItemTemplate>
                        <a href="#" onclick='location.href = "fensi_log.aspx?id=<%# Eval("sn") %>";'>关注跟踪</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle Height="20px" />  
        </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script>
        $(document).ready(function () {
            $('#gv_list').DataTable({
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
                "aaSorting": [[0, 'desc']],
                "iDisplayLength": 15,
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
        });
    </script>
</body>
</html>
