<%@ Page Language="C#" AutoEventWireup="true" CodeFile="appidfensi.aspx.cs" Inherits="appidfensi_statistics" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
<title>统计表</title>
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
        .gv{
            margin-left:auto;
            margin-right:auto;
        }
        .tdate{
            width:150px;
            display:inline-block;
        }
        .dtypes{
            display:inline-block;
        }
        .adtable
        {
            border:2px solid #fff;
            
        }
         .adtable th,td
        {
            border:2px solid #fff;
            
        }
         .adtable th 
        {
            background:#B5B6D6;
            height:20px;
            line-height:20px;
            color:#fff;
            font-weight: bold;
            
        }
      
    </style>
</head>
<body >
    <form id="form1" runat="server">
    <div style="margin:5px;">
        <table class="tableBorder" style="width:100%;">
            <tr>
                <th>吸粉机统计</th>
            </tr>
            <tr>
                <td>
                   
                    <div id="d_yang" runat="server" visible="false" style="position:absolute; margin-top:18px; right:5px; width: 100px; color:red; font-size:18px;">单位：元</div>
                    <table style="width:100%; margin-left:auto; margin-right:auto;">
                            <tr>
                                 <td >
                                    <div style="width:100%; float:left; text-align:left; margin-left:20px;">
                                        <asp:Label ID="l_date" runat="server" Text="选择日期："></asp:Label>
                                        <input type="text" id="t_date" class="form-control datetext tdate"     onclick="WdatePicker({isShowClear:false,readOnly:true,isShowToday:true})" runat="server" />
                                        &nbsp;
                                      
                                         <asp:Label ID="datetype" runat="server" Text="范围："></asp:Label>
                                        <asp:DropDownList ID="d_list" CssClass="form-control center-block dtypes" Width="100px" runat="server" OnSelectedIndexChanged="d_list_SelectedIndexChanged" AutoPostBack="True">
                                            <asp:ListItem Selected="True">每日</asp:ListItem>
                                            <asp:ListItem>30天</asp:ListItem>
                                        </asp:DropDownList>
                                         &nbsp;
                                         <asp:Label ID="appidlabel" runat="server" Text="公众号："></asp:Label>
                                     <asp:DropDownList ID="appid_list" CssClass="form-control center-block dtypes" Width="150px" runat="server" OnSelectedIndexChanged="appid_list_SelectedIndexChanged" AutoPostBack="True">
                                            <asp:ListItem Value="" Selected="True">全部</asp:ListItem>
                                   
                                        </asp:DropDownList>
                                        
                                            &nbsp;
                                    
                                   
                                
                                        <asp:Button ID="b_search" runat="server" CssClass="button" Text="查询" OnClick="b_search_Click" />
                                        &nbsp;
                                
                                        <asp:Button ID="b_prv" runat="server" CssClass="btn-link" Text="前一天" OnClick="b_prv_Click" />
                                        &nbsp;<asp:Button ID="b_next" runat="server" CssClass="btn-link" OnClick="b_next_Click" Text="后一天" />
                                         <label id="l_title" runat="server" class="text-right" style="font-size:18px; color:blue; width:100%; float:right; margin-right:20px;"><%=title %></label>
                                    </div>
                                </td>
                                <td >
                                   
                                </td>
                            </tr>
                        </table>
                    
                 
                    </td>

                </tr>
                 <tr>
                <td>

                   <asp:Panel ID="p_mchid" Width="100%" Height="400px" runat="server">
                        <div class="chart">
        		            <canvas id="barChart"></canvas>
    	                </div>
                    </asp:Panel>
                    <asp:Panel ID="p_device" Width="100%" runat="server" style="margin-top: 20px">
                        
                       <%=html %>
                       <div style="float:right; margin-right:20px;margin-top: 7px ;display:none">
                                    <asp:Button runat="server" ID="btnFirst" CssClass="button" Text="&nbsp;&nbsp;首&nbsp;&nbsp;页&nbsp;&nbsp;" CommandArgument="First" OnCommand="LinkButton_Command" />
                                &nbsp;<asp:Button ID="btnPrev" CssClass="button" Text="&nbsp;&nbsp;上一页 " runat="server" OnCommand="LinkButton_Command" CommandArgument="Prev" />&nbsp;<asp:Button ID="btnNext" CssClass="button" Text=" &nbsp;&nbsp;下一页 " OnCommand="LinkButton_Command" runat="server" CommandArgument="Next" />&nbsp;<asp:Button ID="btnLast" CssClass="button" Text=" 尾&nbsp;&nbsp;页 " OnCommand="LinkButton_Command" runat="server" CommandArgument="Last" />&nbsp;&nbsp;<asp:Literal ID="showPage" runat="server"></asp:Literal><asp:TextBox ID="txtPage" Width="30" runat="server" CssClass="input"></asp:TextBox>&nbsp;&nbsp;<asp:Button ID="btnPage" runat="server" CommandArgument="Coustomer" align="absmiddle" Height="20px" OnClick="btnPage_Click" CssClass="button" Text=" 跳 转 "></asp:Button>
                                </div>
                        <%--<table border="0" cellpadding="3" cellspacing="1" class="tableBorder" width="100%"  style="margin-top: 7px">
                        <tr>
                            <td class="forumRow" style="float:right; width:100%; height:34px;">
                                <div style="float:right; margin-right:20px;">
                                    <asp:Button runat="server" ID="btnFirst" CssClass="button" Text="&nbsp;&nbsp;首&nbsp;&nbsp;页&nbsp;&nbsp;" CommandArgument="First" OnCommand="LinkButton_Command" />
                                &nbsp;<asp:Button ID="btnPrev" CssClass="button" Text="&nbsp;&nbsp;上一页 " runat="server" OnCommand="LinkButton_Command" CommandArgument="Prev" />&nbsp;<asp:Button ID="btnNext" CssClass="button" Text=" &nbsp;&nbsp;下一页 " OnCommand="LinkButton_Command" runat="server" CommandArgument="Next" />&nbsp;<asp:Button ID="btnLast" CssClass="button" Text=" 尾&nbsp;&nbsp;页 " OnCommand="LinkButton_Command" runat="server" CommandArgument="Last" />&nbsp;&nbsp;<asp:Literal ID="showPage" runat="server"></asp:Literal><asp:TextBox ID="txtPage" Width="30" runat="server" CssClass="input"></asp:TextBox>&nbsp;&nbsp;<asp:Button ID="btnPage" runat="server" CommandArgument="Coustomer" align="absmiddle" Height="20px" OnClick="btnPage_Click" CssClass="button" Text=" 跳 转 "></asp:Button>
                                </div>
                            </td>
                        </tr>
                    </table>--%>
                    </asp:Panel>
                   </td>
                 
                </tr>
               
        </table>
        
    </div>
    </form>
    
    <script>
        <% if(p_mchid.Visible)
        { %>
        var canvas = document.getElementById("barChart");
        var p_mchid = document.getElementById("p_mchid");
        canvas.width = p_mchid.clientWidth;
        canvas.height = p_mchid.clientHeight;

        $(function () {
	        var areaChartData = {
            labels: <%= labels %>,
	        datasets: <%= datasets %>,
                    /*[
            {
		        //柱子的颜色
                fillColor: "#05CE3E",
		        //边框的颜色
                //strokeColor: "#ccc",
                //pointColor: "#000",
                //pointStrokeColor: "#c1c7d1",
                //pointHighlightFill: "#fff",
                //pointHighlightStroke: "rgba(220,220,220,1)",
                data: [65, 59, 80, 81, 56, 55, 40,1,2,3,90,67,1,2,3,4]
            },
            ]*/
        };

        var barChartCanvas = $("#barChart").get(0).getContext("2d");
        var barChart = new Chart(barChartCanvas);

            // resize the canvas to fill browser window dynamically
        

	    //运用了上方的数据
        var barChartData = areaChartData;
        var barChartOptions = {
            //Boolean - Whether the scale should start at zero, or an order of magnitude down from the lowest value
            scaleBeginAtZero: true,
            //Boolean - Whether grid lines are shown across the chart
            scaleShowGridLines: false,
            //String - Colour of the grid lines
            scaleGridLineColor: "rgba(0,0,0,.05)",
            //Number - Width of the grid lines
            scaleGridLineWidth: 1,
            //Boolean - Whether to show horizontal lines (except X axis)
            scaleShowHorizontalLines: false,
            //Boolean - Whether to show vertical lines (except Y axis)
            scaleShowVerticalLines: false,
            //Boolean - If there is a stroke on each bar
            barShowStroke: true,
            //Number - Pixel width of the bar stroke
            barStrokeWidth: 2,
            //Number - Spacing between each of the X value sets
            barValueSpacing: 0.5,
            //Number - Spacing between data sets within X values
            barDatasetSpacing: 1,
            //Boolean - whether to make the chart responsive
            responsive: false,
            maintainAspectRatio: true
        };

        barChartOptions.datasetFill = false;
        barChart.Bar(barChartData, null);
        });


        
        <% } %>
    </script>
</body>
</html>
