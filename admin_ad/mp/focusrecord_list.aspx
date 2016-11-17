<%@ Page Language="C#" AutoEventWireup="true" CodeFile="focusrecord_list.aspx.cs" Inherits="focusrecord_list" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="keywords" content="adfans" />
  
<meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
 

<title>公众号吸粉记录</title>
<link href="../style.css" rel="stylesheet" type="text/css" />
	 <script src="/js/jQuery-2.2.0.min.js"></script>
     <script src="/js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
	<link href="/js/My97DatePicker/skin/WdatePicker.css" rel="stylesheet" type="text/css" />
	
   <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/css/bootstrap-theme.min.css" rel="stylesheet" />
	
    <script src="/js/bootstrap.min.js"></script>
  
    <link href="/css/zepto.alert.css" rel="stylesheet" />
    <script src="/js/zepto.alert.js"></script>

    <script src="/js/Chart.min.js"></script>
   
    <style type="text/css">
    
    .tdate{
            width:150px;
            display:inline-block;
        }
        .dtypes{
            display:inline-block;
        }
          .wximg {
            width:30px;
            height:30px;
        }
        .wximgbig {
            display:none;
            width:300px;
            height:300px;
            position:absolute;
            z-index:1000;
			margin-left: -100px;
        }
        .link:hover .wximgbig {
            display:block;
			border: 2px solid red;
        }
       </style>
</head>
<body>
      <form id="form1" runat="server">
    <div style="margin:5px;">
        <table class="tableBorder" style="width:100%;">
            <tr>
                <th>吸粉记录</th>
            </tr>
            <tr>
                <td>
                   
                    <div id="d_yang" runat="server" visible="false" style="position:absolute; margin-top:18px; right:5px; width: 100px; color:red; font-size:18px;">单位：元</div>
                    <table style="width:100%; margin-left:auto; margin-right:auto;">
                            <tr>
                                 <td>
                                    <div style="width:100%; float:left; text-align:left; margin-left:20px;">
                                        <label id="l_date1" runat="server" >起始日期：</label>
                                        <input type="text" id="t_date1" name="t_date1" class="form-control datetext tdate"  onclick="WdatePicker({isShowClear:false,readOnly:true,isShowToday:true})" runat="server" />
                                        &nbsp;
                                      
                                         <label id="l_date2" runat="server">结束日期：</label>
                                        <input type="text" id="t_date2" name="t_date2" class="form-control datetext tdate"  onclick="WdatePicker({isShowClear:false,readOnly:true,isShowToday:true})" runat="server" />
                                        &nbsp;
                                         &nbsp;
                                         <label id="appidlabel"  runat="server" >公众号：</label>
                                     <select id="appid_list" class="form-control center-block dtypes" style="width:150px" runat="server" >
                                            <option value="">全部</option>
                                   
                                        </select>
                                        
                                            &nbsp;
                                    
                                   
                                
                                        <input type="button" id="b_search" runat="server" class="button" value="查询"  />
                                        &nbsp;
                                
                                        <input type="button"  id="b_prv" runat="server" class="btn-link" value="前一天" />
                                        &nbsp;<input type="button" id="b_next" runat="server" class="btn-link"  value="后一天" />
                                         <label id="l_title" runat="server" class="text-right" style="font-size:18px; color:blue; width:100%; float:right; margin-right:20px;"></label>
                                    </div>
                                </td>
                              
                            </tr>
                        </table>
                    
                 
                    </td>

                </tr>
                 <tr>
                <td>
                    <div id="p_device" width="100%" runat="server" style="margin-top: 20px">
                    <table  class="tableBorder table" width="100%" >
                    <thead>
                    <tr>
                            <th>
                             公众号
                            </th>
                            <th>
                             公众号标签
                            </th>
                             <th>
                             微信昵称
                            </th>
                             <th>
                             微信头像
                            </th>
                             <th>
                             所需费用
                            </th>
                             <th>
                             关注时间
                            </th>
                    </tr>
            
                    </thead>
                    <tbody id="listiteam">
                    </tbody>
                </table>
                        <div style="float:right; margin-right:20px;margin-top: 7px ">
                                    <input type="button" runat="server" id="btnFirst" class="button" value="&nbsp;&nbsp;首&nbsp;&nbsp;页&nbsp;&nbsp;"  />
                                &nbsp;
                                <input type="button" id="btnPrev" class="button" value="&nbsp;&nbsp;上一页 " runat="server" />
                                &nbsp;
                                <input type="button" id="btnNext" class="button" value=" &nbsp;&nbsp;下一页 "  runat="server"  />
                                &nbsp;
                                <input type="button" id="btnLast" class="button" value=" 尾&nbsp;&nbsp;页 "  runat="server"  />
                                &nbsp;&nbsp;
                               <span style="margin-right:10px">总共<strong id="nums">0</strong>条记录</span>
                                <span id="cpage">1页</span><span>/</span><span id="pages">1页</span>
                                &nbsp;
                                <input type="text" id="txtPage" style="width:50px" runat="server"/>
                                &nbsp;&nbsp;
                                <input type="button" id="btnPage" runat="server" align="absmiddle" height="20px"  class="button" value=" 跳 转 "/>
                                </div>
                    </div>
                   </td>
                 
                </tr>
               
        </table>
        
    </div>
    </form>

    <script type="text/javascript">
    //翻页基本信息
    var PageLeft = {
        PageIndex: 0//当前页数，从1开始
    , PageSize: 50//默认每页记录数
    , TotalCount: 0//总记录数
    , PageCount: 0//总页数
    , PageType: 0
    };
    var Getappid_Action = function () {
        $.mydialog.open();
        $.ajax(

              {
                  type: "post",
                  url: "focusrecord_list.aspx",
                  data: "action=getappid",
                  dataType: "json",
                  async: false,
                  success: function (json) {
                      $.mydialog.close();


                      if (json.err != undefined) {
                          alert(json.err);
                          return;
                      }
                      $("#appid_list").empty();
                      $("#appid_list").append("<option value=''>全部</option>");
                      $.each(json.ListData, function (idx, item) {
                          $("#appid_list").append("<option value='" + item.appid + "'>" + item.appid + "-" + item.tags + "</option>");
                      })
                  },
                  error: function (data) {
                      $.mydialog.close();
                      alert("出错2");
                  }

              })


        return false;

    }
    var Gs_Action = function (action, n) {

        var date = new Date($("#t_date1").val().replace(/-/g, "/") + " 00:00:00");
        var date2 = new Date($("#t_date2").val().replace(/-/g, "/") + " 00:00:00");
        if (action == "pre_Click") {
            date = new Date(date.setDate(date.getDate() - 1));
            date2 = new Date(date2.setDate(date2.getDate() - 1));
        }

        if (action == "next_Click") {
            date = new Date(date.setDate(date.getDate() + 1));
            date2 = new Date(date2.setDate(date2.getDate() + 1));
        }
        var time_s = date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
        var time_s2 = date2.getFullYear() + "-" + (date2.getMonth() + 1) + "-" + date2.getDate();
        $("#t_date1").val(time_s);
        $("#t_date2").val(time_s2);
        if (date > date2) {
            alert("起始日期不能大于结束日期");
            return;
        }
        PageLeft.PageIndex = parseInt(n);

        var pars = "action=" + action + "&fdate=" + time_s + "&edate=" + time_s2 + "&choose=" + $("#appid_list").val() + "&PageIndex=" + PageLeft.PageIndex + "&PageSize=" + PageLeft.PageSize;

        $.mydialog.open();
        $.ajax(

              {
                  type: "post",
                  url: "focusrecord_list.aspx",
                  data: pars,
                  dataType: "json",
                  success: function (json) {
                      $.mydialog.close();


                      if (json.err != undefined) {
                          alert(json.err);
                          return;
                      }
                      PageLeft.PageIndex = json.PageIndex;
                      PageLeft.PageSize = json.PageSize;
                      PageLeft.TotalCount = json.TotalCount;
                      PageLeft.PageCount = json.PageCount;
                      SetPage(PageLeft);
                      $("#listiteam").html("");
                      if (json.ListData.length == 0) {
                          $("#listiteam").append('<tr><td align="center" colspan="6" >无吸粉记录</td></tr>');
                          return;
                      }
                       
                      $.each(json.ListData, function (idx, item) {
                          $("#listiteam").append("<tr><td>" + item.appid + "</td><td>" + item.tags + "</td><td>" + decodeURI(item.wx_nickname) + "</td><td><a href='#' class='link' ><img src='" + item.wx_headimgurl + "' alt='引流图片' class='wximg'/><img src='" + item.wx_headimgurl + "' alt='引流图片' class='wximgbig'/></a></td><td>" + item.charge/100 + "</td><td>" + item.subscribe_time + "</td></tr>");
                      })
                  },
                  error: function (data) {
                      $.mydialog.close();
                      alert("出错");
                  }

              })


        return false;
    };

    $(function () {
        Getappid_Action();
        Gs_Action("Search_Click", 1);
        $("#b_prv").click(function () {
            Gs_Action("pre_Click", 1);
        });
        $("#b_next").click(function () {
            Gs_Action("next_Click", 1);
        });
        $("#b_search").click(function () {
            Gs_Action("Search_Click", 1);
        });
        $("#appid_list").change(function () {
            Gs_Action("Search_Click", 1);
        });
        $("#btnNext").click(function () {
            nextPageLeft();
        });
        $("#btnPrev").click(function () {
            backPageLeft();
        });
        $("#btnFirst").click(function () {
            FistPageLeft();
        });
        $("#btnLast").click(function () {
            LastPageLeft();
        });
        $("#btnPage").click(function () {
            var pagetext = $("#txtPage").val().trim();
            var maxpage = $('#pages').text();
            var reg = /^\+?[1-9]\d*$/;
            if (!reg.test(pagetext)) {
                $("#txtPage").val("");
                alert("页数大于0的整数");

            }
            else if (pagetext > maxpage) {
                $("#txtPage").val("");
                alert("超过总页数");
            }
            else {

                Gs_Action("Search_Click", pagetext);
            }
        });

    })
  function nextPageLeft() {
        if (PageLeft.PageIndex < PageLeft.PageCount) {
            PageLeft.PageIndex++;
             Gs_Action("Search_Click",PageLeft.PageIndex);
        }
     }
     function LastPageLeft() {

         Gs_Action("Search_Click", $('#pages').text());

         }
         function FistPageLeft() {
             
             Gs_Action("Search_Click", 1);

         }
    function backPageLeft() {
        if (PageLeft.PageIndex > 1) {
            PageLeft.PageIndex--;
             Gs_Action("Search_Click",PageLeft.PageIndex);
          
        }
    }
    function SetPage(p) {
        if (p.TotalCount > 0) {

            $('#nums').text(p.TotalCount);
            $('#cpage').text(p.PageIndex);
            $('#pages').text(p.PageCount);
        } else {
            $('#nums').text(0);
            $('#cpage').text(1);
            $('#pages').text(1);
        }
        
        if (p.PageIndex == 1 && p.PageIndex == p.PageCount) {
            $('#btnFirst').attr('disabled', 'disabled');
            $('#btnPrev').attr('disabled', 'disabled');
            $('#btnNext').attr('disabled', 'disabled');
            $('#btnLast').attr('disabled', 'disabled');
        }
        else if (p.PageIndex == 1) {
            $('#btnFirst').attr('disabled', 'disabled');
            $('#btnPrev').attr('disabled', 'disabled');
            $('#btnNext').removeAttr('disabled');
            $('#btnLast').removeAttr('disabled');
        }
        else if (p.PageIndex == p.PageCount) {
            $('#btnFirst').removeAttr('disabled');
            $('#btnPrev').removeAttr('disabled');
            $('#btnNext').attr('disabled', 'disabled');
            $('#btnLast').attr('disabled', 'disabled');
        }
        else {
            $('#btnFirst').removeAttr('disabled');
            $('#btnPrev').removeAttr('disabled');
            $('#btnNext').removeAttr('disabled');
            $('#btnLast').removeAttr('disabled');
        }
    }
    </script>
</body>
</html>
