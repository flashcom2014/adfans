<%@ Page Language="C#" AutoEventWireup="true" CodeFile="main.aspx.cs" Inherits="admin_manage_main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>快得粉 - 管理后台</title>
    <link rel="stylesheet" href="/css/bootstrap.min.css" />
    <link rel="stylesheet" href="/fonts/font-awesome-4.5.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="/fonts/ionicons-2.0.1/css/ionicons.min.css" />
    <link rel="stylesheet" href="/css/AdminLTE.min.css" />
    <link rel="stylesheet" href="/css/skins/skin-purple-light.css" />
     <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/css/bootstrap-theme.min.css" rel="stylesheet" />
	<link href="/css/zepto.alert.css" rel="stylesheet" />

        

  
</head>
<body class="skin-purple-light">
    <form id="form1" runat="server">
        <% if (!IsAdmin)
           {  %>
        <div class="wrapper sidebar-collapse">
       
            <div class="content-wrapper">

                <section class="content-header">
                    <h1>管理后台</h1>
                </section>

                <section class="content">

                    <div class="row">
                        <div class="col-md-3 col-sm-6 col-xs-12" id="div1" runat="server">
                            <div class="info-box" >
                                <span class="info-box-icon bg-aqua-active"><i class="ion ion-ios-people-outline"></i></span>
                                <div class="info-box-content">
                                    <span class="info-box-text">代理余额</span>
                                    <span class="info-box-number">
                                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label></span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-6 col-xs-12" id="div2" runat="server">
                            <div class="info-box" >
                                <span class="info-box-icon bg-yellow"><i class="ion ion-home"></i></span>
                                <div class="info-box-content">
                                    <span class="info-box-text">商家余额</span>
                                    <span class="info-box-number">
                                        <asp:Label ID="Label6" runat="server" Text=""></asp:Label></span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-6 col-xs-12" id="div5" runat="server">
                            <div class="info-box" >
                                <span class="info-box-icon bg-red"><i class="ion ion-clipboard"></i></span>
                                <div class="info-box-content">
                                    <span class="info-box-text">奖金余额</span>
                                    <span class="info-box-number">
                                        <asp:Label ID="Label7" runat="server" Text=""></asp:Label></span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-6 col-xs-12" id="div3" runat="server" >
                            <div class="info-box" >
                                <span class="info-box-icon bg-green"><i class="ion ion-stats-bars"></i></span>
                                <div class="info-box-content">
                                    <span class="info-box-text">当天分成收入</span>
                                    <span class="info-box-number">
                                        <asp:Label ID="Label2" runat="server" Text=""></asp:Label></span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-6 col-xs-12" id="div4" runat="server">
                            <div class="info-box" >
                                <span class="info-box-icon bg-aqua"><i class="ion ion-clipboard"></i></span>
                                <div class="info-box-content">
                                    <span class="info-box-text">当天广告支出</span>
                                    <span class="info-box-number">
                                        <asp:Label ID="Label3" runat="server" Text=""></asp:Label></span>
                                </div>
                            </div>
                        </div>
                       

                    </div>

                    <div class="row" id="divshouru" style="display:none">
                     
                      
               <div class="col-md-3 col-sm-6 col-xs-12"  >
                <span class="info-box-number" >
                 <asp:Label ID="Label4" runat="server" ForeColor="green" Text="当天分成收入"></asp:Label></span>
                     </div>  
                 <div class="chart" id="p_mchid" style="height:400px" >
                  <canvas id="barChart"></canvas>
                 </div >
                 
                    </div>
                       <div class="row" id="divzhichu">
                     
                      
               <div class="col-md-3 col-sm-6 col-xs-12" style="margin-top:40px">
                <span class="info-box-number">
                 <asp:Label ID="Label5" runat="server" ForeColor="blue" Text="当天广告支出"></asp:Label></span>
                     </div>  
                 <div class="chart" id="p_mchid1" style="height:400px">
                  <canvas id="barChart1"></canvas>
                 </div >
                 
                    </div>
                </section>
            </div>
          
            <footer class="main-footer">
                <div class="pull-right">
                    <small>用户需对自己的行为承担法律责任。用户若是利用系统发布和传播反动、色情或其他违反国家法律的信息，系统记录有可能作为用户违反法律的证据</small>
                </div>
                
            </footer>
        </div>
          <%} %>
    </form>
</body>

<script src="/plugins/jQuery/jQuery-2.2.0.min.js"></script>
<script src="/js/bootstrap.min.js"></script>
<script src="/js/AdminLTE/app.min.js"></script>
  <script src="/js/Chart.min.js"></script>
        <script src="/js/zepto.alert.js"></script>
<script type="text/javascript">



    var SetTuBiao1 = function (datasetsdata) {
     var canvas ;
     var p_mchid;
     
        var chartdata = datasetsdata;
      
      if(chartdata["type"]=="1")
     {
             canvas=document.getElementById("barChart1");
             p_mchid=document.getElementById("p_mchid1");
             $("#Label3").text(chartdata["sum"].toString());
     }
       else
       {
         canvas=document.getElementById("barChart");
             p_mchid=document.getElementById("p_mchid");
              $("#Label2").text(chartdata["sum"].toString());
       }
   
        
        canvas.width = p_mchid.clientWidth;
        canvas.height = p_mchid.clientHeight;

        
      
        var areaChartData = {
            labels: chartdata["labels"],
            datasets:
            [
            {
                //柱子的颜色
                fillColor: chartdata["fillColor"],
                //边框的颜色

                data: chartdata["data"]
            },
            ]

        }
        var barChartCanvas;
           if(chartdata["type"]=="1")
            barChartCanvas = $("#barChart1").get(0).getContext("2d");
            else
          barChartCanvas = $("#barChart").get(0).getContext("2d");
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
    }

    
    var Gs_Action = function (action) {

        $.mydialog.open();
        $.ajax(

              {
                  type: "post",
                  url: "main.aspx",
                  data: "action=" + action,

                  async: false,
                  success: function (data) {
                      $.mydialog.close();
                      var json = eval(data)[0];
//                          if(json["type"]!=null)
//                          {
//                              if(["type"]=="2")
//                              
//                              else if(json["type"]=="1")
//                              SetTuBiao2(json);
//                          }
                          SetTuBiao1(json);
                     
                  },
                  error: function (data) {
                      $.mydialog.close();
                      alert(eval(data));
                  }
              })

        return false;

    }
    $(function () {
     
      var adminbool=<%=IsAdmin.ToString().ToLower()%>;
      var bllos=<%=(MyAdMch!=null&&MyAdMch.IsAgent).ToString().ToLower()%>;
      if(!adminbool)
      {
          
          if(bllos)
          {
           $("#divshouru").attr("style","display:block");
            Gs_Action("Search_get");
         
  
        
            }
        Gs_Action("Search_zhichu");
     }
     }
    )

    

        
       
</script>
 
</html>

