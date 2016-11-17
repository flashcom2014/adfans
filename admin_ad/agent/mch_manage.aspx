<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mch_manage.aspx.cs" Inherits="admin_ad_agent_mch_manage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>子广告商管理</title>
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
    <script src="../../js/clipboard.min.js"></script>
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
    <script src="../../js/jquery-ui.min.js"></script>
    <link href="../../css/jquery-ui.min.css" rel="stylesheet" />
    <style type="text/css">
        .bcancel{
            margin-left:30px;
        }
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
                <th>子广告商管理</th>
            </tr>
            <% if (iseditscale)
                { %>
            <tr>
                <td >
                    <div>
                        <div class="center-block">
                            广告商：<asp:Label ID="l_acc" ForeColor="Blue" Font-Bold="true" Font-Size="18px" runat="server" Text=""></asp:Label>
                        </div>
                        <div>
                            原分成比例：<asp:Label ID="l_scale" ForeColor="Red" runat="server" Text="0%"></asp:Label>
                        </div>
                        <p>
                            <label for="scale">新分成比例：</label>
                            <input type="text" id="scale" name="scale" class="form-control" style="border:0; color:#f6931f; font-weight:bold; width:60px; display:inline;" />
                            <label for="scale">%</label>
                        </p>
                        <div class="center-block" style="margin-bottom:10px;">
                            <label for="slider-range-min" class="ragetxt"><%=l_scale.Text %></label>
                            <div id="slider-range-min" style="width:400px; display:inline-block; margin-left:5px; margin-right:5px;"></div>
                            <label for="slider-range-min" class="ragetxt"><%=myscale %></label>
                        </div>
                        <p>
                            <label for="myscale">我剩下的分成比例：</label>
                            <input type="text" id="myscale" readonly="readonly" class="form-control" style="border:0; color:#f6931f; font-weight:bold; width:60px; display:inline;" />
                            <label for="myscale">%</label>
                        </p>
                        <p>
                        吸粉费用：<input type="text" id="t_amount" name="t_amount" runat="server" class="form-control" style="border:0; color:#f6931f; font-weight:bold; width:60px; display:inline;" />
                        元/个<asp:Label ID="l_minamount" runat="server" Text="(不能低于1.1元)" ForeColor="#0033CC"></asp:Label>
                    </p>
                        <div>
                        <asp:Button ID="b_edit" CssClass="btn btn-default" runat="server" Text="保存新分成比例" OnClientClick="return editscale()" OnClick="b_edit_Click" />
                            <asp:Button ID="b_cancel" CssClass="btn btn-default bcancel" runat="server" Text="取 消" />
                        </div>
                        <asp:HiddenField ID="h_id" runat="server" />
                     </div>
                </td>
            </tr>
            <% } %>
            <tr style="margin-top:20px;">
                <td style="padding-top:10px;">
                    <asp:GridView ID="gv_mch" CssClass="table tableBorder" runat="server" AutoGenerateColumns="False" Width="100%" ShowHeaderWhenEmpty="True" >
                        <Columns>
                            <asp:TemplateField HeaderText="子广告商">
                                <ItemTemplate>
                                    <asp:Label ID="l_tags" runat="server" Text='<%# Eval("ad_name") + "(" + Eval("ad_mobile") + ")" %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="分成比例">
                                <ItemTemplate>
                                    <asp:Label  runat="server" Text='<%# Eval("scale_points") + "%" %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="我的实际分成比例">
                                <ItemTemplate>
                                    <asp:Label  runat="server" Text='<%# MyScale() %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="吸粉价格(元)">
                                <ItemTemplate>
                                    <asp:Label  runat="server" Text='<%# (float.Parse(Eval("sub_amount").ToString())/100f).ToString("F2") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="近30天为我带来的收益(元)">
                                <ItemTemplate>
                                    <asp:Label  runat="server" Text='<%# float.Parse(Eval("amount").ToString()==""?"0":Eval("amount").ToString())/100f  %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="share_code" HeaderText="注册码" />
                            <asp:BoundField DataField="reg_time" HeaderText="注册时间" />
                            <asp:TemplateField HeaderText="修改分成比例">
                                <ItemTemplate>
                                    <asp:LinkButton ID="b_edit" CssClass="btn-link" runat="server" Text="修改分成比例" CommandName='<%# Eval("ad_id").ToString() %>' OnCommand="b_edit_Command" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script>
        
        $(document).ready(function () {
            $('#gv_mch').DataTable({
                "aoColumns": [
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                ],
                "aaSorting": [[3, 'desc']],
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
        function CheckAmount()
        {
            var txt=$("#t_amount").val();
            var patrn=/^(-)?(([1-9]{1}\d*)|([0]{1}))(\.(\d){1,2})?$/;
            if(!patrn.test(txt))
            {
                alert("请输入正确的吸粉费用！");
                return false;
            }
            var num=parseFloat(txt);
            var amount=<%=minamount %>;
            if(num<amount)
            {
                alert("吸粉费用不能低于“" + amount + " 元”！");
                return false;
            }
            return true;
        }
        function editscale()
        {
            if(CheckAmount())
            {
                var txt=$("#t_amount").val();
                var scale=$('#scale').val() + "%";
                if(confirm("新的分成比例为：" + scale + ",吸粉价格为："+ txt + ",是否继续修改？"))
                {
                    $.mydialog.open();
                    return true;
                }
            }
            return false;
        }

        <% if(iseditscale)
        {%>
        $(document).on("blur", "#scale", function () {
             var snum = $(this).val();
             var num = parseFloat(snum) * 100;
             if (num < <%=mchscalevalue %>)
                 num = <%=mchscalevalue %>;
             var max = <%=max%>;
             if (num > max)
                 num = max;
             var myscale=max-num;
             num = num/100;
             $(this).val(num.toString());
             
             $('#myscale').val(myscale/100);
         });

          $(function() {
            $( "#slider-range-min" ).slider({
              range: "min",
              value: <%=mchscalevalue %>,
              min: <%=mchscalevalue %>,
              max: <%=max%>,
              slide: function( event, ui ) {
                  $( "#scale" ).val(ui.value/100);
                  $('#myscale').val((<%=max%> - ui.value)/100 );
              }
            });
              $( "#scale" ).val( $( "#slider-range-min" ).slider( "value" )/100 );
              $('#myscale').val((<%=max%> - $( "#slider-range-min" ).slider( "value" ))/100 );
          });
        <% }%>
    </script>
</body>
</html>
