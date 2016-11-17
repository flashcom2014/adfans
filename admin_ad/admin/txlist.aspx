<%@ Page Language="C#" AutoEventWireup="true" CodeFile="txlist.aspx.cs" Inherits="admin_ad_admin_txlist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
        .table{
            margin-left:auto;
            margin-right:auto;
        }
        .table td{
            text-align:center;
        }
        .table th{
            text-align:center;
        }
         #myModal 
        {
            /*z-index:9999999999;*/
          width:500px;
          height:200px;  
          position:fixed;
          left:30%;
          top:25%;
          background:#ffffff;
          border-radius:5px;
          border:1px solid #DEDFEF;
          font-size:25px;
          text-align:center;
          
          display:none;
        }
        #myModal div
        {
            width:380px;
            margin:10px auto;
            text-align:center;
            line-height:35px;
        }
        #myModal input
        {
            font-size:25px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin:10px;">
        <table border="0" cellpadding="3" cellspacing="1" class="tableBorder" width="98%" align="center" style="margin-bottom: 7px;">
            <tr>
                <th colspan="3">
                    <asp:Localize runat="server" ID="mark"></asp:Localize><asp:Label ID="lbTitle" runat="server">提现申请列表</asp:Label></th>
            </tr>
            <tr>
                <td width="430">当前状态：<asp:DropDownList ID="d_status" runat="server">
                    <asp:ListItem Value="">全部</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">未审核</asp:ListItem>
                    <asp:ListItem Value="2">审核通过</asp:ListItem>
                    <asp:ListItem Value="3">审核拒绝</asp:ListItem>
                </asp:DropDownList></td>
                <td rowspan="2">
                    <asp:Button ID="tbSearch" runat="server" Text="搜索" Height="50" Width="100"  />&nbsp;</td>
            </tr>
            <tr>
                <td width="430">申请时间：<asp:TextBox ID="t_date" runat="server" onclick="WdatePicker({readOnly:true,isShowToday:true})"></asp:TextBox>
                </td>
            </tr>
        </table>
        <asp:GridView ID="gv_tx" Width="100%" BorderWidth="0px" CssClass="tableBorder table" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True">
            <Columns>
                <asp:BoundField DataField="create_time" HeaderText="申请时间" />
                <asp:TemplateField HeaderText="申请人">
                    <ItemTemplate>
                        <span><%# AdMchName() %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="申请金额(元)">
                    <ItemTemplate>
                        <label><%# float.Parse(Eval("amount").ToString()).ToString("F2") %></label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="打款金额(元)">
                    <ItemTemplate>
                        <label style="color:red;"><%# RealAmount() %></label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="手续费">
                    <ItemTemplate>
                        <label><%# Eval("charge") %>‰</label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="审核状态">
                    <ItemTemplate>
                        <span><%# GetStatus() %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="备注">
                    <ItemTemplate>
                        <span><%# Eval("remark") %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="汇款状态">
                    <ItemTemplate>
                        <span><%# GetRemittance_status() %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <%# CommandTx() %>
                        <%--<a href="#" onclick='location.href = "txedit.aspx?txid=<%# Eval("tx_id") %>";'>审核</a>--%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle Height="20px" />  
        </asp:GridView>
        <div id="myModal">
            <div>
                <asp:HiddenField ID="h_txid" runat="server" />
            </div>
            <div>汇款单号：<asp:TextBox ID="t_remittance_order" runat="server"></asp:TextBox></div>
            <div >
                <asp:Button ID="b_remit" runat="server" Text="确认" OnClientClick="return check_remittance();" OnClick="b_remit_Click" />
            <input type="button" style="margin-left:10px;" onclick="$('.loading').hide(); $('#myModal').hide();" value="取消" /></div>
        </div>
    </div>
    </form>
    <script>
        function check_remittance() {
            var txt = $("#t_remittance_order").val();
            if (txt == "") {
                alert("请先输入汇款单号！");
                return false;
            }
            if (confirm("确认汇款单号为：“" + txt + "”？"))
                return true;
            return false;
        }
        function remittance(txid) {
            $("#h_txid").val(txid);
            $("#t_remittance_order").val("");
            $('#myModal').show();
        }
        $(document).ready(function () {
            $('#gv_tx').DataTable({
                "aoColumns": [
                    null,
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
                "iDisplayLength": 20,
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
