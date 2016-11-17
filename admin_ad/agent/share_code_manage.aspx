<%@ Page Language="C#" AutoEventWireup="true" CodeFile="share_code_manage.aspx.cs" Inherits="admin_ad_expand_share_code_manage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>推广码管理</title>
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
    <!-- DataTables -->
    <link rel="stylesheet" href="/plugins/datatables/dataTables.bootstrap.css"/>
    <!-- Bootstrap 3.3.6 -->
    <script src="/js/bootstrap.min.js"></script>
    <!-- AdminLTE App -->
    <script src="/js/AdminLTE/app.min.js"></script>
    <!-- DataTables -->
    <script src="/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="/plugins/datatables/dataTables.bootstrap.min.js"></script>
    <!-- SlimScroll -->
    <script src="/plugins/slimScroll/jquery.slimscroll.min.js"></script>
    <script src="../../js/clipboard.min.js"></script>
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
    <div>
        <table border="1" class="tableBorder" style="width:95%; margin-left:auto; margin-right:auto;" >
            <tr>
                <th>推广码管理</th>
            </tr>
            <tr>
                <td style="text-align:left; padding-left:10px;"><a class="btn btn-link" href="make_share_code.aspx" >创建推广码</a></td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gv_code" CssClass="table tableBorder" runat="server" AutoGenerateColumns="False" Width="100%" ShowHeaderWhenEmpty="True">
                        <Columns>
                            <asp:BoundField DataField="make_time" HeaderText="生成时间" />
                            <asp:BoundField DataField="share_code" HeaderText="推广码"  >
                            <ItemStyle Font-Bold="True" Font-Size="20px" Font-Underline="True" ForeColor="#FF0066" />
                            </asp:BoundField>
                            <asp:BoundField DataField="reg_count" HeaderText="注册人数" />
                            <asp:BoundField DataField="scale" HeaderText="分成比例" />
                            <asp:TemplateField HeaderText="吸粉费用(元)">
                                <ItemTemplate>
                                    <span ><%# (float.Parse(Eval("amount").ToString())/100f).ToString("F2") %></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="推广链接">
                                <ItemTemplate>
                                    <span class="form-control" style="width:100%; display:inline; "><%# GetShareLink() %></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="复制链接">
                                <ItemTemplate>
                                    <button type="button" class="btn-link" data-clipboard-text='<%# GetShareLink() %>'>复制链接</button>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="删除推广码">
                                <ItemTemplate>
                                    <button type="button" class="btn-link" onclick='delcode("<%# Eval("share_code") %>");' >删除</button>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <input type="text" id="t_link" class="hidden" /><button id="b_copy" type="button" class="btn hidden" data-clipboard-action="copy" data-clipboard-target="#t_link">复制到剪切板</button>
                    <asp:HiddenField ID="h_code" runat="server" />
                    <asp:Button ID="b_delcode" CssClass="hidden" runat="server" Text="删除推广码" OnClick="b_delcode_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script>
        
        function delcode(code) {
            if (confirm("确认删除推广码“" + code + "”？")) {
                $('#h_code').val(code);
                $('#b_delcode').click();
            }
        }
        function copylink(theid) {
            var links = $(theid).text();
            $('#t_link').val(links);
            clipboard = new Clipboard('#b_copy');
            $('#b_copy').click();
        }

        $(document).ready(function () {
            $('#gv_code').DataTable({
                "aoColumns": [
                    null,
                    null,
                    null,
                    null,
                    { "bSortable": false }, 
                    { "bSortable": false },
                    { "bSortable": false }
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
                "autoWidth": false
            });
        });

        var clipboard = new Clipboard('.btn-link');

        clipboard.on('success', function (e) {
            alert("复制成功！");
        });

        clipboard.on('error', function (e) {
            alert("复制失败，请手动选择复制！");
        });
    </script>
</body>
</html>
