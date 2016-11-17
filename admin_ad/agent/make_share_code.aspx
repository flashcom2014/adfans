<%@ Page Language="C#" AutoEventWireup="true" CodeFile="make_share_code.aspx.cs" Inherits="admin_ad_expand_make_share_code" %>

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
    <script src="../../js/clipboard.min.js"></script>
    <script src="../../js/jquery-ui.min.js"></script>
    <link href="../../css/jquery-ui.min.css" rel="stylesheet" />
    <title>添加推广码</title>
    <style type="text/css">
        .ragetxt {
            color:red;
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
        <table border="1" class="tableBorder" style="width:80%; margin-left:auto; margin-right:auto;" >
            <tr style="width:100%;">
                <th colspan="2">添加推广码</th>
            </tr>
            <tr>
                <td>我总的分成比例为：<asp:Label ID="l_scale" runat="server" Text="0%" Font-Bold="True" Font-Size="18px" ForeColor="#FF0066"></asp:Label></td>
            </tr>
            <tr>
                <td style="color:red;">*推广代码面向广告商请把分成比例设为0，推广代码面向下级代理可设为对应比例值</td>
            </tr>
            <tr>
                <td>
                    <p>
                        <label for="scale">推广码分成比例：</label>
                        <input type="text" id="scale" name="scale" class="form-control" style="border:0; color:#f6931f; font-weight:bold; width:60px; display:inline;" />
                        <label for="scale">%</label>
                    </p>
                    <div class="center-block" style="margin-bottom:10px;">
                        <label for="slider-range-min" class="ragetxt">0%</label>
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
                </td>
            </tr>
            <tr>
                <td>
                    <div class="center-block"><asp:Button ID="b_makecode" CssClass="btn btn-default" runat="server" Text="生成推广码" OnClientClick="return makecode();" OnClick="b_makecode_Click" /><%--<button type="button" class="btn btn-default" id="makecode">生成推广码</button>--%>
                        <button type="button" style="margin-left:30px;" class="btn btn-default" onclick="location.href='share_code_manage.aspx';">推广码管理</button></div>
                    
                </td>
            </tr>
            <%if (showresult)
                { %>
            <tr>
                <td>
                    <div class="center-block">
                        <p>
                            生成的推广码为：<asp:Label ID="l_code" ForeColor="Red" Font-Bold="true" Font-Size="25px" runat="server" Text=""></asp:Label>,分成比例为：<asp:Label ID="l_makescale" runat="server" ForeColor="Blue" Font-Bold="true" Font-Size="25px" Text="0%"></asp:Label>,吸粉价格为：<asp:Label ID="l_makeamount" runat="server" ForeColor="Blue" Font-Bold="true" Font-Size="25px" Text="0%"></asp:Label>
                        </p>
                        <p>
                            推广链接：<input id="t_link" readonly="readonly" type="text" value="<%=regurl %>" class="form-control" style="display:inline; width:500px;" />
                            <button id="b_copy" type="button" class="btn" data-clipboard-action="copy" data-clipboard-target="#t_link">复制到剪切板</button>
                        </p>
                    </div>
                </td>
            </tr>

            <%} %>
        </table>
    </div>
    </form>
     <script>
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
         function makecode()
         {
             if(CheckAmount())
             {
                 var txt=$("#t_amount").val();
                 var scale=$('#scale').val() + "%";
                 if(confirm("推广码的分成比例为：" + scale + ",吸粉价格为："+ txt + "元/个，是否继续生成？"))
                 {
                     $.mydialog.open();
                     return true;
                 }
             }
             return false;
         }
         //$('#makecode').click(function (){
         //    var scale=$('#scale').val() + "%";
         //    if(confirm("推广码的分成比例为：" + scale + ",是否继续生成？"))
         //    {
         //        $.mydialog.open();
         //        $('#b_makecode').click();
         //    }
         //});
         $(document).on("blur", "#scale", function () {
             var snum = $(this).val();
             var num = parseFloat(snum) * 100;
             if (num < 0)
                 num = 0;
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
              value: 0,
              min: 0,
              max: <%=max%>,
              slide: function( event, ui ) {
                  $( "#scale" ).val(ui.value/100);
                  $('#myscale').val((<%=max%> - ui.value)/100 );
              }
            });
              $( "#scale" ).val( $( "#slider-range-min" ).slider( "value" )/100 );
              $('#myscale').val((<%=max%> - $( "#slider-range-min" ).slider( "value" ))/100 );
          });

         var clipboard = new Clipboard('#b_copy');

         clipboard.on('success', function(e) {
             alert("复制成功！");
         });

         clipboard.on('error', function(e) {
             alert("复制失败，请手动选择复制！");
         });
  </script>
</body>
</html>
