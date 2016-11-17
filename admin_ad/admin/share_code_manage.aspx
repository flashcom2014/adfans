<%@ Page Language="C#" AutoEventWireup="true" CodeFile="share_code_manage.aspx.cs" Inherits="admin_ad_admin_share_code_manage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="keywords" content="adfans" />
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
    <script src="../../js/clipboard.min.js"></script>
    <title>注册码管理</title>
    <style type="text/css">
        .tableBorder td{
            text-align:center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin:10px;">
        <table border="1" class="tableBorder" style="margin-bottom:7px; width:98%; margin-left:auto; margin-right:auto;">
            <tr><th>注册码管理</th></tr>
            <tr>
                <td>
                    <table border="1" class="tableBorder" style="width:100%">
                        <tr>
                            <th>吸粉超始价</th>
                            <th>分成比率</th>
                            <th>操作</th>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="t_subamount" runat="server" Width="60px"></asp:TextBox>元/个</td>
                            <td>
                                <asp:TextBox ID="t_scale" runat="server" Width="40px"></asp:TextBox>%</td>
                            <td>
                                <asp:Button ID="b_makecode" runat="server" Text="添加注册码" OnClientClick="return AddCodeCheck();" OnClick="b_makecode_Click" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="p_show" Visible="true" runat="server">
                        <div class="center-block">
                        <p>
                            生成的推广码为：<asp:Label ID="l_code" ForeColor="Red" Font-Bold="true" Font-Size="25px" runat="server" Text=""></asp:Label>,分成比例为：<asp:Label ID="l_makescale" runat="server" ForeColor="Blue" Font-Bold="true" Font-Size="25px" Text="0%"></asp:Label>,吸粉价格为：<asp:Label ID="l_makeamount" runat="server" ForeColor="Blue" Font-Bold="true" Font-Size="25px" Text="0%"></asp:Label>
                        </p>
                        <p>
                            推广链接：<input id="t_link" readonly="readonly" type="text" value="<%=regurl %>" class="form-control" style="display:inline; width:500px;" />
                            <button id="b_copy" type="button" class="btn" data-clipboard-action="copy" data-clipboard-target="#t_link">复制到剪切板</button>
                        </p>
                    </div>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gv" Width="100%" BorderWidth="0px" CssClass="tableBorder table" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" >
                        <Columns>
                            <asp:BoundField DataField="share_code" HeaderText="注册码" />
                            <asp:BoundField DataField="admin_acc" HeaderText="创建者" />
                            <asp:TemplateField HeaderText="吸粉起始价(元)">
                                <ItemTemplate>
                                    <span><%# (float.Parse(Eval("amount").ToString())/100f).ToString("F2") %></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="scale" HeaderText="分成比率(%)" />
                            <asp:BoundField DataField="reg_count" HeaderText="注册人数" />
                            <asp:TemplateField HeaderText="注册链接">
                                <ItemTemplate>
                                    <span class="form-control" style="width:100%; display:inline; "><%# GetShareLink() %></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="复制链接">
                                <ItemTemplate>
                                    <button type="button" class="btn-link" data-clipboard-text='<%# GetShareLink() %>'>复制链接</button>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="make_time" HeaderText="生成时间" />
                        </Columns>
                        <RowStyle Height="20px" />  
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script>
        function AddCodeCheck() {
            var tamount = $("#t_subamount").val();
            var patrn = /^(-)?(([1-9]{1}\d*)|([0]{1}))(\.(\d){1,2})?$/;
            if (!patrn.test(tamount)) {
                alert("请输入正确的金额！");
                return false;
            }
            var min=<%= minamount%>;
            if(tamount<min)
            {
                alert("最低吸粉起始价不能低于：“" + min + "”元");
                return false;
            }
            var tscale=$("#t_scale").val();
            if(!patrn.test(tscale))
            {
                alert("请输入正确的分成比率！");
                return false;
            }
            var scale=parseFloat(tscale);
            if(scale<0)
            {
                alert("发起的分成比率不能低于0！");
                return false;
            }
            return true;
        }
        $(document).ready(function () {
            $('#gv').DataTable({
                "aoColumns": [
                    null,
                    null,
                    null,
                    null,
                    null,
                    { "bSortable": false }, 
                    { "bSortable": false },
                    null
                ],
                "aaSorting": [[7, 'desc']],
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
                "autoWidth": false
            });
        });

        var clipboard = new Clipboard('#b_copy');

        clipboard.on('success', function(e) {
            alert("复制成功！");
        });

        clipboard.on('error', function(e) {
            alert("复制失败，请手动选择复制！");
        });
        var clipboard2 = new Clipboard('.btn-link');

        clipboard2.on('success', function (e) {
            alert("复制成功！");
        });

        clipboard2.on('error', function (e) {
            alert("复制失败，请手动选择复制！");
        });
    </script>
</body>
</html>
