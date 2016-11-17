<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mp_list.aspx.cs" Inherits="admin_ad_admin_mp_list" %>

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
  <script type="text/javascript" src="../../js/jquery.qrcode.min.js""></script>
<!-- Bootstrap 3.3.6 -->
<script src="/js/bootstrap.min.js"></script>
<!-- AdminLTE App -->
<script src="/js/AdminLTE/app.min.js"></script>
<!-- DataTables -->
<script src="/plugins/datatables/jquery.dataTables.min.js"></script>
<script src="/plugins/datatables/dataTables.bootstrap.min.js"></script>
<!-- SlimScroll -->
<script src="/plugins/slimScroll/jquery.slimscroll.min.js"></script>
<title>吸粉机公众号配置</title>
	<link href="../style.css" rel="stylesheet" type="text/css" />
    <script src="../../js/clipboard.min.js"></script>
         <link href="../../css/zepto.alert.css" rel="stylesheet" />
      <script src="../../js/zepto.alert.js"></script>
    <style type="text/css">
        .wximg {
            width:30px;
            height:20px;
        }
        .wximgbig {
            display:none;
            width:300px;
            height:200px;
            position:absolute;
            z-index:1000;
			margin-left: -100px;
        }
        .link:hover .wximgbig {
            display:block;
			border: 2px solid red;
        }
        .lefttxt{
            width:100%;
            text-align:right;
            float:right;
        }
        .tablebottom{
            margin:5px;
        }
        .auto-style1 {
            width: 250px;
        }
        .auto-style2 {
            width: 200px;
        }
        .cbtn{
            margin-left:30px;
        }
        .tableBorder td{
            text-align:center;
        }
        .modal-dialog {
            width:400px;
        }
        .modal-content{
            border-radius:15px;
        }
        .qr{
            width:250px;
            height:250px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" >
    <div style="margin:5px;">
        <table border="1"  id="pztable" runat="server"  class="tableBorder" style="margin-bottom:7px; width:98%; margin-left:auto; margin-right:auto;" Visible="false">
            <tbody >
                <tr ><th colspan="2" id="thpz" runat="server">公众号服务器配置</th></tr>
                <tr>
                    <td class="auto-style1" style="text-align:right; padding-right:5px;">URL(服务器地址)</td>
                    <td  style="text-align:left; padding-left:5px;">
                        <asp:Label ID="l_serverurl" runat="server" Text="*"></asp:Label>
                        <button type="button" class="btn-link" data-clipboard-text='<%= l_serverurl.Text %>'>复制链接</button>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1" style="text-align:right; padding-right:5px;">Token(令牌)</td>
                    <td  style="text-align:left; padding-left:5px;">
                        <asp:Label ID="l_token" runat="server" Text="*"></asp:Label>
                        <button type="button" class="btn-link" data-clipboard-text='<%= l_token.Text %>'>复制Token</button>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1" style="text-align:right; padding-right:5px;">EncodingAESKey(消息加解密密钥)</td>
                    <td style="text-align:left; padding-left:5px;">
                        <asp:Label ID="l_aeskey" runat="server" Text="*"></asp:Label>
                        <button type="button" class="btn-link" data-clipboard-text='<%= l_aeskey.Text %>'>复制密钥</button>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1" style="text-align:right; padding-right:5px;">消息加解密方式</td>
                    <td style="text-align:left; padding-left:5px;">兼容模式</td>
                </tr>
                <tr>
                     
                     <td   colspan="2" style="text-align:center; padding-left:5px;">
                     <asp:Button ID="pzfanhui" runat="server" CssClass="button cbtn" OnClick="b_pzfanhui_Click" Text="返回" />
                    </td>
       
                </tr>
            </tbody>
        </table>
        <table border="0" class="table tableBorder"   id="listtable" runat="server" style="margin-bottom:7px; width:98%; margin-left:auto; margin-right:auto;">
            <tbody>
                <tr ><th colspan="4" >公众号列表</th></tr>
                <tr id="trid" runat="server">
                    <td style="text-align:left">广告商：<asp:TextBox ID="t_admch" runat="server"></asp:TextBox></td>
                <td  style="text-align:left">APPID:<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
                <td><asp:Button ID="b_search" runat="server" CssClass="button" Text="搜索" /></td>
                
                    <td></td>
                </tr>
                
                <tr>
                    <td colspan="4">
                    <asp:Panel ID="p_list" runat="server">
                        <asp:GridView ID="gv" Width="100%" BorderWidth="0px" CssClass="tableBorder table" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" PageSize="100">
                            <Columns>
                                <asp:TemplateField HeaderText="广告商">
                                    <ItemTemplate>
                                        <a href="#" class="btn-link" onclick='ShowPZ(<%# Eval("ad_id") %>,"<%# Eval("ad_name")%>");'><span><%# GetAdName() %></span></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="mch_acc" HeaderText="设备商" />
                                <asp:BoundField DataField="tags" HeaderText="标签" />
                                <asp:BoundField DataField="desp" HeaderText="介绍" />
                                <asp:BoundField DataField="appid" HeaderText="APPID" />
                                <asp:BoundField DataField="title" HeaderText="吸粉页标题" />
                                <asp:TemplateField HeaderText="图片">
                                    <ItemTemplate>
                                        <a href="#" class="link" >
                                            <img src='<%# Eval("imgurl") %>' alt="引流图片" class="wximg" />
                                            <img src='<%# Eval("imgurl") %>' alt="引流图片" class="wximgbig" />
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="费用">
                                    <ItemTemplate>
                                        <label><span style="color:red;"><%# GetAmount(Eval("charge")) %></span> 元</label> 
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="是否可用">
                                    <ItemTemplate>
                                        <label><%# GetEnable(Eval("enable")) %></label> 
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="类型">
                                    <ItemTemplate>
                                        <label><%# GetMpTypes() %></label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="状态">
                                    <ItemTemplate>
                                        <label><%# GetStatus(Eval("status")) %></label><br />
                                        <asp:LinkButton ID="b_checkstatus" runat="server" Text="检测" CssClass="btn-link" CommandName='<%# Eval("id").ToString() %>' OnCommand="b_checkstatus_Command" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="unusual_count" ItemStyle-ForeColor="Red" HeaderText="异常次数" />
                                <asp:TemplateField HeaderText="详细信息" Visible="false">
                                    <ItemTemplate>
                                        <a href="#" onclick='location.href = "mp_info.aspx?id=<%# Eval("id") %>";'>详情</a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="修改">
                                    <ItemTemplate>
                                        <a href="#" class="link" onclick='EditMP(<%# Eval("id") %>);'>修改</a> | <a href="#" class="link" onclick='DelMP(<%# Eval("id") %>,"<%# Eval("tags") %>");'>删除</a>
                                    </ItemTemplate>

                                </asp:TemplateField>

                            </Columns>
                            <RowStyle Height="20px" />  
                        </asp:GridView>
                    </asp:Panel>
                    <asp:Panel ID="p_addedit" Visible="false" runat="server">
                        <table class="tableBorder" style="width: 100%;">
                            <tr>
                                <td class="auto-style2">
                                    <span class="lefttxt">标签：</span>
                                </td>
                                <td style="text-align:left;">
                                    <asp:TextBox ID="t_tags" runat="server" Width="200px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="auto-style2"><span class="lefttxt">介绍：</span></td>
                                <td style="text-align:left;">
                                    <asp:TextBox ID="t_desc" runat="server" TextMode="MultiLine" Width="100%" Height="40px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="auto-style2"><span class="lefttxt">APPID：</span></td>
                                <td style="text-align:left;"><asp:TextBox ID="t_appid" runat="server" Width="200px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="auto-style2"><span class="lefttxt">应用密钥：</span></td>
                                <td style="text-align:left;"><asp:TextBox ID="t_appsecret" runat="server" Width="200px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="auto-style2"><span class="lefttxt">原始ID：</span></td>
                                <td style="text-align:left;"><asp:TextBox ID="t_originalid" runat="server" Width="200px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="auto-style2"><span class="lefttxt">吸粉页标题：</span></td>
                                <td style="text-align:left;"><asp:TextBox ID="t_title" runat="server" Width="200px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="auto-style2"><span class="lefttxt">展示图片：</span></td>
                                <td style="text-align:left;"><asp:FileUpload ID="f_upload" CssClass="input" Width="400px" runat="server" /><asp:Label ID="l_upload" Visible="false" runat="server" Text="不改变图片可不选择上传图片" ForeColor="#FF66FF"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2"><span class="lefttxt">公众号类型：</span></td>
                                <td  style="text-align:left;">
                                    <asp:DropDownList ID="d_mptypes" runat="server">
                                        <asp:ListItem Value="0">服务号</asp:ListItem>
                                        <asp:ListItem Value="1">订阅号</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2"><span class="lefttxt">状态：</span></td>
                                <td  style="text-align:left;">
                                    <asp:DropDownList ID="d_status" runat="server">
                                        <asp:ListItem Value="0">已通过检测</asp:ListItem>
                                        <asp:ListItem Value="1">未检测</asp:ListItem>
                                        <asp:ListItem Value="2">正在检测</asp:ListItem>
                                        <asp:ListItem Value="3">状态异常，请重新检测</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2"><span class="lefttxt">是否可用：</span></td>
                                <td  style="text-align:left;">
                                    <asp:CheckBox ID="c_enable" Checked="true" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="auto-style2"></td>
                                <td  style="text-align:left;">
                                <asp:Button ID="b_edit" runat="server" Text="确 认" CssClass="button" Visible="False" OnClientClick="return CheckMPs(false);" OnClick="b_edit_Click" />&nbsp;
                                    <asp:Button ID="b_cancel" runat="server" CssClass="button cbtn" OnClick="b_cancel_Click" Text="取 消" />
                                </td>
                            </tr>
                        </table>

                    </asp:Panel>
                   </td>
                </tr>
            </tbody>
        </table>
    </div>
        <asp:HiddenField ID="h_id" Value="0" runat="server" />
         <asp:HiddenField ID="pz_id" Value="0" runat="server" />
          <asp:HiddenField ID="pz_name" Value="" runat="server" />
        <asp:Button ID="b_openedit" CssClass="hidden" runat="server" Text="修改" OnClick="b_openedit_Click" />
        <asp:Button ID="b_del" CssClass="hidden" runat="server" Text="删除" OnClick="b_del_Click" />
         <asp:Button ID="b_showpz" CssClass="hidden" runat="server" Text="显示" OnClick="b_showpz_Click" />
    </form>
      <!--新加的部分-->
        <div class="modal fade" id="myModal">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title text-center" >请用手机微信扫下面二维码</h4>
                    </div>
                    <div class="modal-body text-center">
                        <img id="qr_container" src="#" alt="二维码" class="qr" />
                        <p id="qrtext" style="text-align:center; color:cornflowerblue;"><%=QRText %></p>
                    </div>
                
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" id="myModalcanel">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    <script>
    var timeout1=true;
   
        function CheckMPs(isadd) {
            if ($('#t_tags').val() == "") {
                alert("请输入标签");
                return false;
            }
            if ($('#t_appid').val() == "") {
                alert("请输入APPID");
                return false;
            }
            if ($('#t_appsecret').val() == "") {
                alert("请输入应用密钥");
                return false;
            }
            if ($('#t_originalid').val() == "") {
                alert("请输入原始ID");
                return false;
            }
            if ($('#t_title').val() == "") {
                alert("请输入吸粉页标题");
                return false;
            }
            var uploadfile = document.getElementById("f_upload").value;
            if (isadd) {
                var n = uploadfile.lastIndexOf(".");
                if (n < 0) {
                    alert("你上传的文件不是有效的图片文件，请上传有效图片格式文件，包括[jpg][jpeg][gif][png]");
                    return false;
                }
                else {
                    var fileValue = uploadfile.substring(n);
                    if (fileValue.toLowerCase() != ".jpg" && fileValue.toLowerCase() != ".gif" && fileValue.toLowerCase() != ".png" && fileValue.toLowerCase() != ".jpeg") {
                        alert("【" + fileValue + "】不是有效的图片格式，请上传有效图片格式文件，包括[jpg][jpeg][gif][png]");
                        return false;
                    }
                }
            }
            return true;
        }
        function EditMP(id) {
            $('#h_id').val(id);
            $('#b_openedit').click();
        }
        function ShowPZ(id,name) {
           $('#pz_id').val(id);
           $('#pz_name').val(name);
           
            $('#b_showpz').click();
        }
        
        function DelMP(id,tags) {
            $('#h_id').val(id);
            if (confirm("确认删除“" + tags + "”此公众号？"))
                $('#b_del').click();
        }

        $("#myModalcanel").click(function () {
            $('#myModal').modal('hide');
            timeout1 = false;
        });

        $(document).ready(function () {
            $('#gv').DataTable({
                "aoColumns": [
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    { "bSortable": false },
                    null,
                    null,
                    null,
                    null,
                    null,
                    { "bSortable": false },
                ],
                "aaSorting": [[0, 'desc']],
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

            <% if (QrUrl.Length > 0)
                { %>
            $("#qr_container").attr("src", "<%=QrUrl%>");
            $('#myModal').modal({ show: true, backdrop: 'static' });
            
            <% } %>
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
