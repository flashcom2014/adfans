<%@ Page Language="C#" AutoEventWireup="true" CodeFile="personam.aspx.cs" Inherits="personam" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <script src="/js/jquery-2.2.0.min.js"></script>
    <script type="text/javascript" src="../../js/jquery.qrcode.min.js""></script>
    
    <link href="../style.css" rel="stylesheet" type="text/css" />
       <link href="../../css/bootstrap.min.css" rel="stylesheet">
    <link href="../../css/bootstrap-theme.min.css" rel="stylesheet">
    <link href="../../css/bootstrap-switch.min.css" rel="stylesheet">
     <link href="../../css/zepto.alert.css" rel="stylesheet" />
      <script src="../../js/zepto.alert.js"></script>
    <script src="../../js/bootstrap.min.js"></script>
    <script src="../../js/bootstrap-switch.min.js"></script>
  
        

    <title>
     
</title>
<style type="text/css" >
.persontable
{
    border:1px solid #B1B4D1;
	background:#fff;
    text-align: left;
}
.persontable td{background-color:#EAEAF3;padding-right: 2px;padding-left:5px;
  
    padding-top: 4px;
    padding-bottom: 4px;
}
	
.persontable th
{
    background-color: #B1B4D1;
    color: white;
    font-size: 12px;
    font-weight:bold;
    height: 20px;
    text-align:center;
}
 .title_txt {
            color: red;
            text-align: center;
        }
        .td_left {
            width: 100px;
            text-align:right;
            padding-right:2px;
            /*text-shadow:2px 2px 1px #888888*/
        }
        .textbox {
            float:left;
            margin:2px 15px;
            width:245px;
        }
        .head_img {
            width: 196px;
            height: 196px;
           margin: 15px;
        }
       
    
        .submit_button {
            margin: 2px 15px;
            width: 120px;
        }
        .bind_err {
            border:2px solid #f7eded;
            padding:15px 15px; 
            background:#FFFFFF;
            width:550px;
            border-radius: 20px;
            box-shadow: 10px 10px 5px #888888;
            display: none;
        }
        .divlog {
            width:500px;
            padding:15px 15px; 
        }
		body {
			background-color: #dadae9;
		}
    </style></head>
<body>
    <form runat="server" id="form1"  >

       <div id="persondiv">
        <table width="98%" border="0" cellpadding="3" cellspacing="1" class="persontable" align="Center" style="margin-top: 7px;">
            <tr>
                <th colspan="2">
                    个人资料</th>
            </tr>
               <tr>
                <td class="forumRow" align="right" width="12%" style="height: 25px">账户名：</td>
                <td class="forumRow" style="height: 25px">
                  <label  id="tbmch_acc" name="tbmch_acc" runat="server" style="width:10em;" ></label></td>
            </tr>
           
           
         
           <%-- <tr>
               
                    <td class="forumRow" align="right" width="12%" style="height: 25px">添加管理员权限：</td>
                    <td class="forumRow" style="height: 25px">
                        <input id="raenable1" type="radio" name="enable" runat="server" value="raenable1" checked="true"  runat="server"/>
                        <label for="raenable1">是</label>
                        <input id="raenable0" type="radio" name="enable" runat="server" value="raenable0" />
                        <label for="raenable0">否</label>
                    </td>
                    
                
            </tr>--%>
    
     <tr>
    <td class="forumRow" align="right" width="12%" style="height: 25px">二维码：</td>
     <td class="forumRow" style="height: 200px">
             <div class="mdiv">
               <p style="font-size:20px; font-weight:bold;color:red">请用手机微信扫描下面的二维码进行绑定</p>
        <div id="qr_container"  ></div>
        <div style=" vertical-align:middle;">
      
       <%--<asp:Image ID="img_qrcode" runat="server" Height="200px" Width="200px" CssClass="center-block" />--%>
            
            <p class="center-block " style="color:blue"><%=BindTips %></p>
        </div>
    </div>
    </td>
    </tr>
            <tr>
                <td class="forumRow" align="right"></td>
                <td class="forumRow">
                    <input type="button" name="ok" value=" 返回 " onclick="return check();" id="ok" class="button" />
                </td>
            </tr>
           
        </table>
        </div>
        
          <div id="mch_bind"  name="mch_bind" runat="server"   style="overflow:hidden;background:#DADAE9;width:375px;height:500px;box-shadow:3px 3px 3px #ccc;border-radius:10px;position:absolute;left:155px;top:80px;z-index:5200;display:none">
          
            <!-- /.modal -->
            <table >
                <tr>
                    <td class="td_left" ></td>
                    <td><img ID="img_head" src="../images/portrait.png" runat="server" class="head_img img-polaroid" /></td>
                </tr>
                
                <tr>
                    <td class="td_left" style="height:50px">
                        微信昵称：
                    </td>
                    <td>
                      
                        <strong id="wx_nickname" class="text-info" >&nbsp;</strong>
                        </td>
                </tr>
                <tr>
                    <td class="td_left" style="height:50px">绑定状态：</td>
                    <td>
                     

                        <span style="color:red;font-size: 12px;font-weight:bold;" id="wxenable" runat="server" ></span>
                       </td>
                </tr>
         
                <tr>
                    <td colspan="2" class="td_left" style="height:80px"><button type="button" id="btn_submit" class="btn btn-primary submit_button"  style="width:100px">解除绑定</button> <button type="button" id="cxbdbutton" class="btn btn-primary submit_button" style="width:100px" >重新绑定</button>
                    </td>
                  
                </tr>
            </table>
            <input type="hidden" id="encode" value="<%=EncryptCode %>" />
            <input type="hidden" id="wx_openid" value="" runat="server" name="wx_openid" />
             <input type="hidden" id="wx_appid" value="" runat="server" name="wx_openid" />
            <input type="hidden" id="wx_headimgurl" value="" runat="server" name="wx_headimgurl" />
            <input type="hidden" id="enablevalue" name="enablevalue" value="0" runat="server" />
       
        </div>
          <div id="bind_err" class="center-block bind_err" >
            <h3 class="center-block title_txt" >无法进行绑定操作</h3>
            <p class="center-block text-center" >此帐号已被下面微信号绑定，如需更改绑定，请先用原绑定微信号解除绑定再进行绑定操作</p>
            <h2 id="l_nick" class="center-block title_txt">&nbsp;</h2>
        </div>
    </form>
    <!--新加的部分-->
    <!--蒙版-->
    <div class="mengban" id="mengban" style="position:fixed;width:100%;height:100%;top:0;left:0;background:#000;opacity:0.5;filter:alpha(opacity=50);display:none;z-index:5000">

    </div>
    </body>
    <script type="text/javascript">
    
  
     var GetWxInfo = function () {
            var code = $('#encode').val();
            $.ajax({
                url: "ajax/loginxg.ashx?action=getwxinfo",
                data: { "code": "" + code + "" },
                type: 'post',
            }).done(function (data) {
                var json = eval(data)[0];
                if (json["result"] == "ok") {
                    var openid = json["data"]["openid"];
                     var appid = json["data"]["appid"];
                    $('#wx_openid').val(openid);
                    $('#wx_appid').val(appid);
                    $('#wx_nickname').text(json["data"]["nickname"]);
                    $('#img_head').attr('src', json["data"]["headimgurl"]);
                    $('#wx_headimgurl').val(json["data"]["headimgurl"]);
                    if(json["data"]["enable"]=="1")
                    {
                       $('#wxenable').text("未绑定");
                       $('#btn_submit').attr("disabled","disabled");
                       $('#cxbdbutton').removeAttr("disabled");
                    }
                    else
                    {
                        $('#wxenable').text("已绑定");
                        $('#cxbdbutton').attr("disabled","disabled");
                        $('#btn_submit').removeAttr("disabled");
                        
                    }
                    
                    $('#mengban').show();
                    $('#mch_bind').show();
                  
                    
//                    $('#wx_nickname').emoji();
                }
                else if (json["result"] == "no") {
                       
                       if(json["url"]!=null&&json["url"]!="")
                     {
                      alert(json["info"]);
                      parent.location.href=json["url"];
                      return;
                      }

                    setTimeout(function () {
                        GetWxInfo();
                    }, 500);
                }
                else if (json["result"] == "nob") {
                 
                    $('#l_nick').text(json["info"]);
                    $('#persondiv').hide();
                    $('#bind_err').show();
                   
                }
            })
        };
      
     $(function () {

         $("#qr_container").qrcode({
             render: "canvas", //table方式 
             width: 200, //宽度 
             height: 200, //高度 
             text: "<%=QrUrl%>" //任意内容 
         });
           setTimeout(function () {
                GetWxInfo();
            }, 1000);
        });
        function check()
        {
           parent.location.reload();
            return;
        }
        $('#btn_submit').click(function () {
               $.mydialog.open();
            
         var encode = $('#wx_openid').val();
                $.ajax({
                    url: "ajax/loginxg.ashx?action=delbind",
                    data: {"openid": "" + encode + ""},
                    type: 'post',
                }).done(function (data) {
                    $.mydialog.close();
                    var json = eval(data)[0];
                    if (json["result"] == "no") {
                     
                        $.mydialog.alert(json["info"]);
                          if(json["url"]!=null&&json["url"]!="")
                     {
                     
                      parent.location.href=json["url"];
                      return;
                      }
                     
                    } else if (json["result"] == "ok") {
                        $.mydialog.alert(json["info"]);
                        
                        setTimeout(function () {
                            location.replace(location.href);
                        }, 1000);
                    }

                }).error(function (d) {
                    $.mydialog.close();
                    alert("网络出错");
                });
            
        })
   $('#cxbdbutton').click(function () {
       $.mydialog.open();     
         var encode = $('#wx_openid').val();
          var appid = $('#wx_appid').val();
                $.ajax({
                    url: "ajax/loginxg.ashx?action=bindwxinfo",
                    data: {"openid": "" + encode + "", "appid": "" + appid + ""},
                    type: 'post',
                }).done(function (data) {
                    $.mydialog.close();
                    var json = eval(data)[0];
                    if (json["result"] == "no") {
                        $.mydialog.alert(json["info"]);
                             if(json["url"]!=null&&json["url"]!="")
                     {
                     
                      parent.location.href=json["url"];
                      return;
                      }
                       
                    } else if (json["result"] == "ok") {
                        
                        $.mydialog.alert(json["info"]);
                       
                        setTimeout(function () {
                           location.replace(location.href);
                        }, 1000);
                    }

                }).error(function (d) {
                    $.mydialog.close();
                    alert("网络出错");
                });
           
            
   })

       
    </script>
    </html>
    
    