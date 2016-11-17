<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mp_menu.aspx.cs" Inherits="admin_ad_mp_mp_menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>公众号菜单配置</title>
    <script src="/js/jQuery-2.2.0.min.js"></script>
	<script src="/js/jquery.timers-1.2.js"></script>
	<link href="/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/css/bootstrap-theme.min.css" rel="stylesheet" />
	<%--<link href="../style.css" rel="stylesheet" type="text/css" />--%>
    <script src="/js/bootstrap.min.js"></script>
    <link href="/css/zepto.alert.css" rel="stylesheet" />
    <link href="/css/mp_menu.css" rel="stylesheet" />
    <script src="/js/addMenu.js"></script>
    <script src="/js/zepto.alert.js"></script>
    <style type="text/css">
        th{
            text-align:center;
            background-color: #B1B4D1;
            color: white;
            font-size: 12px;
            font-weight:bold;
            height: 20px;
        }
        .no_mp{
            color:red;
            text-align:center;
            font-size:30px;
        }
        .dlist{
            display:inline;
            width:320px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-top:5px;">
        <table border="0" class="tableBorder" width="98%" align="center" >
            <tr><th>公众号服务器配置</th></tr>
            <tr>
                <td style="text-align:center; vertical-align:middle;">
                    配置的公众号：<asp:DropDownList ID="d_list" CssClass="form-control dlist" runat="server" AutoPostBack="true" OnSelectedIndexChanged="d_list_SelectedIndexChanged"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="mpmenu" class="content">
                        <div class="left">
                          <div class="header">
                            <%=mptitle %>
                          </div>
                          <div class="bottom">
                            <a class="addmenu" href="#">
                              <span>＋</span>添加菜单
                            </a>
                            <ul class="btul">
                            </ul>
                          </div>
                        </div>
                        <div class="right">
                          <div class="rtop">
                            <h3 class="rmenuname">菜单名称</h3>
                            <a href="#" class="del" id="del" name="">删除菜单</a>
                          </div>
                          <p class="title">已添加子菜单，仅可设置菜单名称</p>
                          <div class="rcenter">
                            <span>菜单名称</span>
                            <input type="text" placeholder="菜单名称" class="inputname">
                            <p class="p1">字数不超过4个汉字或8个字母</p>
                            <p class="p2">字数不超过8个汉字或16个字母</p>
                          </div>
                          <div class="rcenter addr">
                            <span>页面地址</span>
                            <input type="text" class="address" />
                            <p>订阅者点击该子菜单会跳到以下链接</p>
                          </div>
                        </div>
                      </div>
                    <div id="no_mp" class="no_mp" style="display:none;">
                        您还没有配置公众号，所以无法使用公众号菜单配置
                    </div>
                </td>
            </tr>
            <tr><td>
                    <button type="button" id="b_save" class="btn btn-group-lg btn-success center-block" onclick="savemenu();">保存菜单</button>
                </td></tr>
        </table>
    </div>
    </form>
</body>
</html>
<script>
    var isfirst = true;
    function addmain(thename, theurl) {
        if (isfirst) {
            isfirst = false;
            first($('.addmenu'), thename, theurl);
        }
        else {
            addmain1($('.btlif'), thename, theurl);
        }
    }

    $(document).on("click", ".addmenu", function () {
        // $(".addmenu").click(function(){
        first(this, '主菜单', '');
    });
    function first(but,thename,theurl) {
        $(but).css("display", "none");
        $(".btul").css("display", "block");
        $(".btul").append(addMenu(thename, theurl));
        $(".btul li").css("width", "49.5%");
        $(".btli").find(".subdiv").css("width", "42%");
        $(".btli").find(".fuhao").css("width", "100%");
        $(".right").css("display", "block");
        $(".rcenter").css("display", "block");
        $(".title").css("display", "none");
        $(".title").css("display", "none");
        $(".inputname").val("");
        $(".inputname").attr("disabled", "disabled");
        $(".address").val("");
        $(".address").attr("disabled", "disabled");
        $("#del").attr("name", "a");
    }
    //点击主菜单的加
    var time = 0;
    $(document).on("click", ".btlif", function () {
        addmain1(this,"主菜单","");
    });
    function addmain1(but,thename,theurl) {
        time++;
        if ($(".btli").size() == 1) {
            $(but).before(addMenu('btli' + time, thename, theurl));
            $(".btul>li").css("width", "33%");
            $(".btli").find(".subdiv").css("width", "28%");
            $(".btli").find(".fuhao").css("width", "100%");
            $(".subdiv").css("display", "none");
        } else if ($(".btli").size() == 2) {
            //删除
            $(but).remove();
            $(".btul").append(addMenu('btli' + time, thename, theurl));
            $(".btul>li").css("width", "33%");
            $(".btli").find(".subdiv").css("width", "28%");
            $(".btli").find(".fuhao").css("width", "100%");
            $(".subdiv").css("display", "none");
        }
        $("#del").attr("name", "a");
        $(".btli").children("a").css({ "border": "1px solid #e7e7eb", "color": "#666" });
        $(".p1").css("display", "block");
        $(".p2").css("display", "none");
    }
    //
    function addsubmenu(index, thename, theurl) {
        var id = "#btli" + index;
        var thebut = $(id).find('.fuhao');
        addsubmenu1(thebut, thename, theurl);
    }
    //点击上方的加
    var num = 0;
    $(document).on("click", ".fuhao", function () {
        var on = $(this).parent().parent().parent().children(".menuitem").attr("on");
        if (on) {
            addsubmenu1(this, "子菜单", "");
            $(".addr").css("display", "block");
        } else {
            alert("请选中主菜单");
            $(this).parent().parent().parent().children(".menuitem").attr("on", "true");
        }
        
    });
    function addsubmenu1(but, thename, theurl) {
        num++;
        // var html="<li class='subli'><a href='#' addr='e' id='"+num+"'>"+num+"</a></li>";
        $(but).before(addSubMenu(num, thename, theurl));
        $(but).parent().parent().parent().children(".menuitem").attr("on", "false");
        //alert($(this).prev().children("a").attr("id"));
        $(".subli").children("a").css({ "color": "#666" });
        $(".subli").css("border", "1px solid #e7e7eb");
        var id = $(but).prev().children("a").attr("id");
        $("#" + id).css({ "color": "#44b549" });
        $("#" + id).parent().css("border", "1px solid #44b549");
        $(but).parent().parent().parent().children(".menuitem").css({ "border": "1px solid #e7e7eb", "color": "#666" });
        $("#del").attr("name", id);
        $(".inputname").val($("#" + id).text());
        $(".address").val($("#" + id).attr("addr"));
        $(".p1").css("display", "none");
        $(".p2").css("display", "block");
        if ($(but).parent().children(".subli").size() == 5) {
            $(but).remove();
            $(but).parent().append(addSubMenu(num, thename, theurl));
        }
    }
    //点击下方菜单按钮
    $(document).on("click", ".menuitem", function () {
        $(".menuitem").css({ "border": "1px solid #e7e7eb", "color": "#666" });
        var index = $(this).parent().index();
        $(this).css({ "border": "1px solid #44b549", "color": "#44b549" });
        $(this).attr("on", "true");
        $(".subdiv").css("display", "none");
        $(this).parent().children(".subdiv").css("display", "block");
        $(this).parent().find(".subli").css({ "border": "1px solid #e7e7eb" });
        $(this).parent().find(".subli").children("a").css({ "color": "#666" });
        var len = $(this).next(".subdiv").children(".subul").children(".subli").size();
        //alert(len);
        if (len == 0) {
            $(".rmenuname").html($(this).text());
            $(".inputname").val($(this).text());
            $(".address").val($(this).attr("addr"));
            $(".addr").css("display", "block");
            $(".title").css("display", "none");

        } else {
            $(".title").css("display", "block");
            $(".rmenuname").html($(this).text());
            $(".inputname").val($(this).text());
            $(".addr").css("display", "none");
            $(".p2").css("display", "none");
            $(".p1").css("display", "block");
        }
        $("#del").attr("name", $(this).parent().attr("id"));
        //恢复修改
        $(".inputname").removeAttr("disabled");
        $(".address").removeAttr("disabled");
    });
    //点击上方子li
    $(document).on("click", ".subli", function () {
        $(".subdiv").css("display", "none");
        $(this).parent().parent().css("display", "block");
        $(".subli").css({ "border": "1px solid #e7e7eb", "color": "#666" });
        $(this).css({ "border": "1px solid #44b549" });
        $(".subli").children("a").css("color", "#666");
        $(this).children("a").css("color", "#44b549");
        $(this).parent().parent().parent().children(".menuitem").css({ "border": "1px solid #e7e7eb", "color": "#666" })
        $(".addr").css("display", "block");
        $(".title").css("display", "none");
        $(".rmenuname").html($(this).text());
        $(".inputname").val($(this).text());
        $(".address").val($(this).children("a").attr("addr"));
        $("#del").attr("name", $(this).children("a").attr("id"));
        //恢复修改
        $(".inputname").removeAttr("disabled");
        $(".address").removeAttr("disabled");
    });
    //点击删除
    $(document).on("click", "#del", function () {
        var name = $(this).attr("name");
        if ($('#' + name).parent().hasClass("subli")) {
            if (confirm("是否删除子菜单？")) {
                if ($('#' + name).parent().parent().find(".subli").size() == 5) {
                    var li = "<li class='fuhao'><a href='#'>+</a></li>";
                    $('#' + name).parent().parent().append(li);
                }
                $('#' + name).parent().remove()
            }
        } else if ($("#del").attr("name") == "a") {
            alert("请选中一项！")
        } else {
            if (confirm("是否删除主菜单？删除主菜单后对应子菜单也将删除！")) {
                $('#' + name).remove();
            }

        }
        if ($(".btli").size() == 2) {
            $(".btlif").remove();
            var html = "<li class='btlif'><a href='#' class='jia'>+</a></li>";
            $(".btul").append(html);
            $(".btul > li").css("width", "33%");
            $(".btul > li").find(".subdiv").css("width", "28%");
        } else if ($(".btli").size() == 1) {
            $(".btlif").remove();
            var html = "<li class='btlif'><a href='#' class='jia'>+</a></li>";
            $(".btul").append(html);
            $(".btul > li").css("width", "49.5%");
            $(".btul > li").find(".subdiv").css("width", "42%");

        } else if ($(".btli").size() == 0) {
            $(".btlif").remove();
            $(".addmenu").css("display", "block");
            $(".right").css("display", "none");
            $(".btul").css("display", "none");
        } else {

        }
    });
    $(document).on("blur", ".inputname", function () {
        //判断中文长度
        // alert($("#del").attr("name"));
        var name = $("#del").attr("name");
        if ($("#" + name).hasClass("btli")) {
            if (getText($(this).val()) <= 8) {
                //var name=$("#del").attr("name");
                if ($('#' + name).parent().hasClass("subli")) {
                    $('#' + name).text($(this).val());
                    $(".rmenuname").html($(this).val());
                } else {
                    $('#' + name).children("a").text($(this).val());
                    $(".rmenuname").html($(this).val());
                }
            } else {
                alert("字数不超过4个汉字或8个字母");
                $(this).val("");
            }
        } else if ($("#" + name).parent().hasClass("subli")) {
            if (getText($(this).val()) <= 16) {
                //var name=$("#del").attr("name");
                if ($('#' + name).parent().hasClass("subli")) {
                    $('#' + name).text($(this).val());
                    $(".rmenuname").html($(this).val());
                } else {
                    $('#' + name).children("a").text($(this).val());
                    $(".rmenuname").html($(this).val());
                }
            } else {
                alert("字数不超过8个汉字或16个字母");
                $(this).val("");
            }
        }


    })
    //添加地址功能
    $(document).on("blur", ".address", function () {
        var name = $("#del").attr("name");
        if ($('#' + name).parent().hasClass("subli")) {
            $('#' + name).attr("addr", $(this).val());
        } else {
            $('#' + name).children("a").attr("addr", $(this).val());
        }
    })

    function selectone() {
        $(".btul").find(".btli").eq(0).find(".menuitem").click();
    }

    function savemenu() {
        var postdata = "";
        for (var i = 0; i < 3; i++) {
            var mainbut = $(".btul").find(".btli").eq(i).find(".menuitem");
            var name = mainbut.text();
            var addr = mainbut.attr("addr");
            if (addr == undefined)
                break;
            postdata += "&mainname" + i + "=" + encodeURIComponent(name);
            postdata += "&mainurl" + i + "=" + encodeURIComponent(addr);
            for (var j = 0; j < 5; j++) {
                var subbtn = $(".btul").find(".btli").eq(i).find(".subli").eq(j).children("a");
                var subname = subbtn.text();
                var subaddr = subbtn.attr("addr");
                if (subaddr == undefined)
                    break;
                postdata += "&subname" + i + j + "=" + encodeURIComponent(subname);
                postdata += "&suburl" + i + j + "=" + encodeURIComponent(subaddr);
            }
        }
        postdata = postdata.substr(1);

        $.mydialog.open();
        $.ajax({
            url: "mp_menu.aspx?action=savemenu",
            data: postdata,
            type: 'post',
        }).done(function (data) {
            $.mydialog.close();
            var json = eval(data)[0];

            alert(json["info"]);

            

        }).error(function (d) {
            if (d.readyState != 0 && d.status != 0)
                alert("网络出错");
        });

    }

    $(document).ready(function () {
        <%=scripts %>
    });
</script>