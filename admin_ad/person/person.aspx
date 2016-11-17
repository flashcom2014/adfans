<%@ Page Language="C#" AutoEventWireup="true" CodeFile="person.aspx.cs" Inherits="person" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <script src="/js/jquery-2.2.0.min.js"></script>
  
    <link href="../style.css" rel="stylesheet" type="text/css" />
     <link href="/css/zepto.alert.css" rel="stylesheet" />
      <script src="/js/zepto.alert.js"></script>
        

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

    </style></head>
<body>
    <form runat="server" id="form1"  >


        <table width="98%" border="0" cellpadding="3" cellspacing="1" class="persontable" align="Center" style="margin-top: 7px;">
            <tr>
                <th colspan="2">
                    个人资料</th>
            </tr>
               <tr>
                <td class="forumRow" align="right" width="12%" style="height: 25px">注册手机：</td>
                <td class="forumRow" style="height: 25px">
                  <label  id="tbmobile" name="tbmobile" runat="server" style="width:10em;" ></label></td>
            </tr>
            <tr>
                <td class="forumRow" align="right" width="12%" style="height: 25px">帐户名：</td>
                <td class="forumRow" style="height: 25px">
                  <input id="hiddisset" runat="server" type="hidden" value="0" class="input" style="width:10em;"/>
                    <input id="tbmch_acc" name="tbmch_acc" runat="server" type="text"  class="input" style="width:10em;"/></td>
            </tr>
          
            <tr>
                <td class="forumRow" align="right" width="12%" style="height: 25px">姓名或公司名：</td>
                <td class="forumRow" style="height: 25px">
                    <input name="tbmch_name" type="text"  runat="server" id="tbmch_name" class="input" style="width:10em;" /></td>
            </tr>
           
            <tr>
                <td class="forumRow" align="right" width="12%" style="height: 25px">邮箱：</td>
                <td class="forumRow" style="height: 25px">
                    <input name="tbemail" type="text" id="tbemail"  runat="server" class="input" style="width:20em;" /></td>
            </tr>
            <tr>
                <td class="forumRow" align="right" width="12%" style="height: 25px">地址：</td>
                <td class="forumRow" style="height: 25px">
                <input name="areahidden" type="hidden" id="areahidden"  runat="server" class="input" style="width:20em;" />

                <select name="node0" id="node0" onchange="javascript:changeselect(0)" style="width:10em;">
                 <option value="">请选择省</option> 
               
                 </select> 
                 <select name="node1" id="node1" onchange="javascript:changeselect(1)" style="width:10em;"> 
                  <option value="">请选择市</option>
                  
                 </select> 
                 <select name="node2" id="node2" onchange="javascript:changeselect(2)"  style="width:10em;"> 
                 <option value="">请选择县</option> 

                 </select> 
                 <select name="node3" id="node3" onchange="javascript:changeselect(3)"  style="width:10em;"> 
                 <option value="">请选择街道</option> 
              
                 </select> 
                    <input name="tbaddress" type="text" id="tbaddress" runat="server" class="input" style="width:20em;" /></td>
            </tr>
            <tr>
                <td class="forumRow" align="right" width="12%" style="height: 25px">备注：</td>
                <td class="forumRow" style="height: 25px">
                    <input name="tbcomment" type="text"  id="tbcomment" runat="server" class="input" style="width:20em;" /></td>
            </tr>
          
            <tr>
                <td class="forumRow" align="right" width="12%" style="height: 25px">固定电话：</td>
                <td class="forumRow" style="height: 25px">
                    <input name="tbtelephone" type="text"  id="tbtelephone" runat="server" class="input" style="width:10em;" />
                    
                </td>
            </tr>
           <tr>
                <td class="forumRow" align="right" width="12%" style="height: 25px">广告商推荐码：</td>
                <td class="forumRow" style="height: 25px">
                    <label name="tbtjcode"   id="tbtjcode" runat="server"  style="width:10em;"></label>
                  
                </td>
            </tr>
            <tr>
                <td class="forumRow" align="right"></td>
                <td class="forumRow">
                    <input type="button" name="ok" value=" 提 交 " onclick="return check();" id="ok" class="button" />
                </td>
            </tr>
        </table>

    </form>
    <!--新加的部分-->
    <!--蒙版-->
    <div class="mengban" id="mengban">
    </div>
    </body>
    <script type="text/javascript">
      var fullid = "<%=admch.fullid%>";

      var stringid = fullid.split(',');
      $("#tbmch_acc").change(function () {
          $("#hiddisset").val("1");
      })
      
     $(function () {
       
            select1();


        });
        function check() {

            var acc = $('#tbmch_acc').val();
            if (acc == "") {
                alert("请输入帐户名");
                return false;
            }
            var name = $('#tbmch_name').val();
            if (name == "") {
                alert("请输入姓名或公司名");
                return false;
            }
            $.mydialog.open();

            $.ajax(
            {
                type: "post",
                url: "person.aspx?action=update",
                dataType: "json",
                data: $("#form1").serializeArray(),
                success: function (msg) {
                    $.mydialog.close();

                    var json = eval(msg[0]);

                    if (json["result"] != null) {

                        alert(json["result"]);
                        parent.location.reload();
                        return;
                    }
                    if (json["err"] != null) {

                        alert(json["err"]);
                        return;
                    }

                    //                    if (json["url"] != null) {
                    //                        //location.href = json["url"];
                    //                        location.href = json["url"];
                    //                    }

                }
            })
        }
        function changeselect(n) {

            stringid = {};
            if (n == 0) {
                $("#areahidden").val($("#node0").val());
                select2();
            }
            else if (n == 1) {
                $("#areahidden").val($("#node1").val());
                select3();
            }
            else if (n == 2) {
                $("#areahidden").val($("#node2").val());
                select4();
            }
            else if (n == 3)
                select5();

        }
    function select1() {
      $("#node0").html("");
      $("#node0").append("<option value=''>请选择省</option>");
      $.mydialog.open();
       $.ajax(
            {
                type: "post",
                url: "person.aspx?action=area",
                data: "type=0",
                dataType: "json",
                success: function (msg) {
                    $.mydialog.close();

                    var json = eval(msg);
                    for (var i = 0; i < json.length; i++) {

                        var id = json[i]["id"];
                        $("#node0").append("<option value=" + json[i]["id"] + ">" + json[i]["name"] + "</option>");
                        if (stringid.length > 0 && stringid[0] == id) {
                            $("#node0").find("option[value='" + id + "']").attr("selected", true);
                            $("#areahidden").val($("#node0").val());
                        }
                    }



                    select2();


                }
            })
        };

        function select2() {
            $("#node1").html("");
            $("#node1").append("<option value=''>请选择市</option>");
            $("#node2").html("");
            $("#node2").append("<option value=''>请选择县</option>");
            $("#node3").html("");
            $("#node3").append("<option value=''>请选择街道</option>");
            if ($("#node0").val() == "") {
                $("#node1").attr("disabled", "disabled");
                $("#node2").attr("disabled", "disabled");
                $("#node3").attr("disabled", "disabled");
                return;
            }
            $.mydialog.open();

            $.ajax(
            {
                type: "post",
                url: "person.aspx?action=area",
                dataType: "json",
                data: { "type": "1", "parentid": $("#node0").val() },
                success: function (msg) {
                    $.mydialog.close();

                    var json = eval(msg);
                    if (json.length == 0) {
                        $("#node1").attr("disabled", "disabled");
                    }
                    else {
                        $("#node1").removeAttr("disabled");
                    }
                    for (var i = 0; i < json.length; i++) {
                        var id = json[i]["id"];
                        $("#node1").append("<option value=" + json[i]["id"] + ">" + json[i]["name"] + "</option>");
                        if (stringid.length > 1 && stringid[1] == id) {
                            $("#node1").find("option[value='" + id + "']").attr("selected", true);
                            $("#areahidden").val($("#node1").val());
                        }
                    }


                    select3();




                    //$("#areahidden").val($("#node1").val());
                }
            })
        };
       function select3() {
           $("#node2").html("");
           $("#node2").append("<option value=''>请选择县</option>");
           $("#node3").html("");
           $("#node3").append("<option value=''>请选择街道</option>");
           if ($("#node1").val() == "") {
             
               $("#node2").attr("disabled", "disabled");
               $("#node3").attr("disabled", "disabled");
               return;
           }
           $.mydialog.open();

            $.ajax(
            {
                type: "post",
                url: "person.aspx?action=area",
                dataType: "json",
                data: { "type": "2", "parentid": $("#node1").val() },
                success: function (msg) {
                    $.mydialog.close();

                    var json = eval(msg);
                    if (json.length == 0) {
                        $("#node2").attr("disabled", "disabled");
                    }
                    else {
                        $("#node2").removeAttr("disabled");
                    }
                    for (var i = 0; i < json.length; i++) {
                        var id = json[i]["id"];
                        $("#node2").append("<option value=" + json[i]["id"] + ">" + json[i]["name"] + "</option>");
                        if (stringid.length > 2 && stringid[2] == id) {
                            $("#node2").find("option[value='" + id + "']").attr("selected", true);
                            $("#areahidden").val($("#node2").val());
                        }
                    }



                    select4();





                    //$("#areahidden").val($("#node2").val());
                }
            })
        };
           function select4() {
            $("#node3").html("");
            $("#node3").append("<option value=''>请选择街道</option>");
            if ($("#node2").val() == "") {
              
                $("#node3").attr("disabled", "disabled");
                return;
            }
            $.mydialog.open();

            $.ajax(
            {
                type: "post",
                url: "person.aspx?action=area",
                dataType: "json",
                data: { "type": "3", "parentid": $("#node2").val() },
                success: function (msg) {
                    $.mydialog.close();

                    var json = eval(msg);
                    if (json.length == 0) {
                        $("#node3").attr("disabled", "disabled");
                    }
                    else {
                        $("#node3").removeAttr("disabled");
                    }
                    for (var i = 0; i < json.length; i++) {
                        var id = json[i]["id"];
                        $("#node3").append("<option value=" + json[i]["id"] + ">" + json[i]["name"] + "</option>"); ;
                        if (stringid.length > 3 && stringid[3] == id) {
                            $("#node3").find("option[value='" + id + "']").attr("selected", true);
                            $("#areahidden").val($("#node3").val());
                        }
                    }



                }
            })
        };

        function select5() {
            $("#areahidden").val($("#node3").val());
        }
  
       
    </script>
    </html>
    
    