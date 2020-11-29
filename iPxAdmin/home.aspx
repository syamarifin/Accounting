<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="home.aspx.vb" Inherits="iPxAdmin_home" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
    .shad 
            {
                text-shadow: 2px 2px 4px #000000;
            }
            
    .paddPanel1
            {
            	
        	    padding-left:20px;
        	    padding-right: 10px;
        	    }
    .paddPanel2
            {
            	
        	    padding-left:5px;
        	    padding-right:5px;
        	    }
    .paddPanel3
            {
            	
        	    padding-left:10px;
        	    padding-right: 20px;
        	    }
    .backgroundAlcor
    {
	            width:100%; 
                height:100%; 
                background-repeat:no-repeat;
	    }
    	
	     body  
     {
       background-image: url("../assets/background/Alcor-Accounting2.jpg");
       background-repeat: no-repeat;
       background-size: cover;
       background-attachment:fixed;
     }
     
     .panel-home {
        color: #fff;
        background-color: #083765;
        border-color: #083765;
        }
    </style>
    <style>
        .small-box {
            border-radius: 5px;
            position: relative;
            display: block;
            margin-bottom: 20px;
            box-shadow: 0 1px 1px rgba(0,0,0,0.1);
            color:White;
        }
        .bg-aqua {
            background-color: #36b3c1 !important;
            border-color: #36b3c1 !important;
        }
        .inner {
            padding: 10px;
        }
        .small-box h3 {
            font-size: 18px;
            font-weight: bold;
            margin: 0 0 10px 0;
            white-space: nowrap;
            padding: 0;
        }
        .small-box p {
            font-size: 15px;
        }
        .small-box .icon {
            -webkit-transition: all .3s linear;
            -o-transition: all .3s linear;
            transition: all .3s linear;
            position: absolute;
            top: 0px;
            right: 13px;
            z-index: 0;
            font-size: 90px;
            color: rgba(0,0,0,0.15);
        }
        .small-box-footer {
            position: relative;
            text-align: center;
            padding: 3px 0;
            color: #fff;
            color: rgba(255,255,255,0.8);
            display: block;
            z-index: 10;
            <%--background: rgba(0,0,0,0.1);--%>
            background: rgba(176, 216, 228);
            text-decoration: none;
        }
    </style>
    <script>
        $(document).ready(function () {
            showGraph();
            showGraphSalesRevenue();
            showGraphSetlement();
        });

        function showGraph() {
        $.ajax({
            type: "POST",
            url: "home.aspx/ChartAging",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessGraph,
            failure: function(responseGraph) {
                alert(responseGraph.d);
            }
        });
        }
        function OnSuccessGraph(responseGraph) {
            data = JSON.parse(responseGraph.d);
            console.log(data);
            var unread30 = [];
            var unread60 = [];
            var unread90 = [];
            var unread120 = [];
            var unreadlbh = [];
            $(data).each(function(key, value) {
                unread30 += value.Amount30;
                unread60 += value.Amount60;
                unread90 += value.Amount90;
                unread120 += value.Amount120;
                unreadlbh += value.Amountlbh;
            });
            //$('#notifNewTicket').html(unread30);
            var ctx = document.getElementById("myChart");
            var myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: ["≤30 D", "≤60 D", "≤90 D", "≤120 D", ">120 D"],
                    datasets: [{
                            label: 'AR Aging',
                            data: [unread30, unread60,unread90, unread120,unreadlbh],
                            backgroundColor: [
                                '#36a2eb',
                                '#4bc0c0',
                                '#ffcd56',
                                '#ff9f40',
                                '#ff6384'
                            ],
                            borderColor: [
                                '#36a2eb',
                                '#4bc0c0',
                                '#ffcd56',
                                '#ff9f40',
                                '#ff6384'
                            ],
                            borderWidth: 1
                        }]
                },
                options: {
                    legend: {
                        display: false
                    },
                    title: {
                        display: true,
                        text: 'Account Receivable Aging'
                    },
                    scales: {
                        yAxes: [{
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
                    }
                }
            });
        }

        function showGraphSalesRevenue() {
        $.ajax({
            type: "POST",
            url: "home.aspx/ChartSalesRev",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessGraphRev,
            failure: function(responseGraphRev) {
                alert(responseGraphRev.d);
            }
        });
        }
        function OnSuccessGraphRev(responseGraphRev) {
            data = JSON.parse(responseGraphRev.d);
            console.log(data);
            var ActMtdRO        = [];
            var BudgetMtdRO     = [];
            var ActMtdFB        = [];
            var BudgetMtdFB     = [];
            var ActMtdOther     = [];
            var BudgetMtdOther  = [];
            $(data).each(function(key, value) {
                ActMtdRO        += value.ActMtdRO;
                BudgetMtdRO     += value.BudgetMtdRO;
                ActMtdFB        += value.ActMtdFB;
                BudgetMtdFB     += value.BudgetMtdFB;
                ActMtdOther     += value.ActMtdOther;
                BudgetMtdOther  += value.BudgetMtdOther;
            });
            //$('#notifNewTicket').html(unread30);
            var ctx = document.getElementById("myChartRev");
            var myChart = new Chart(ctx, {
                type: 'bar',
                data:  {
                    labels: ["RO", "FB", "Other"],
                    datasets: [{
                            label: 'Actual',
                            data: [ActMtdRO, ActMtdFB, ActMtdOther],
                            backgroundColor: [
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb'
                            ],
                            borderColor: [
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb'
                            ],
                            borderWidth: 1
                        },
                        {
                            label: 'Budget',
                            data: [BudgetMtdRO, BudgetMtdFB, BudgetMtdOther],
                            backgroundColor: [
                                '#4bc0c0',
                                '#4bc0c0',
                                '#4bc0c0'
                            ],
                            borderColor: [
                                '#4bc0c0',
                                '#4bc0c0',
                                '#4bc0c0'
                            ],
                            borderWidth: 1
                        }]
                },
                options: {
                    legend: {
                        display: true,
                        position:'bottom'
                    },
                    title: {
                        display: true,
                        text: 'Sales Revenue (MTD)'
                    },
                    scales: {
                        yAxes: [{
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
                    }
                }
            });
        }
        
        function showGraphSetlement() {
        $.ajax({
            type: "POST",
            url: "home.aspx/ChartSalesSet",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessGraphSetlement,
            failure: function(responseGraphSetlement) {
                alert(responseGraphSetlement.d);
            }
        });
        }
        function OnSuccessGraphSetlement(responseGraphSetlement) {
            data = JSON.parse(responseGraphSetlement.d);
            console.log(data);
            var GL = [];
            var CS = [];
            var CR = [];
            var CL = [];
            var WB = [];
            $(data).each(function(key, value) {
                GL += value.GL;
                CS += value.CS;
                CR += value.CR;
                CL += value.CL;
                WB += value.WB;
            });
            //$('#notifNewTicket').html(unread30);
            var ctx = document.getElementById("myChartSet");
            var myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: ["GL", "CS", "CR", "CL", "WB"],
                    datasets: [{
                            label: 'AR Aging',
                            data: [GL, CS,CR, CL,WB],
                            backgroundColor: [
                                '#36a2eb',
                                '#4bc0c0',
                                '#ffcd56',
                                '#ff9f40',
                                '#ff6384'
                            ],
                            borderColor: [
                                '#36a2eb',
                                '#4bc0c0',
                                '#ffcd56',
                                '#ff9f40',
                                '#ff6384'
                            ],
                            borderWidth: 1
                        }]
                },
                options: {
                    legend: {
                        display: false
                    },
                    title: {
                        display: true,
                        text: 'Sales Settlement (MTD)'
                    },
                    scales: {
                        yAxes: [{
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
                    }
                }
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        
            <%--<div class="row">
          
               
            </div>--%>
            <div class="clearfix">
        <div id ="new" class="col-lg-4 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-aqua" style="background-color:white !important; border-color:white !important;">
                <div class="inner">
                    <canvas id="myChartRev" height="205"></canvas>
                </div>
                <a href="iPxSalesSummary.aspx" class="small-box-footer" style="color: rgba(0,0,0,0.8);">More info <i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div><!-- ./col -->
        <div id ="proses" class="col-lg-4 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-aqua" style="background-color:white !important; border-color:white !important;">
                <div class="inner">
                    <canvas id="myChartSet" height="205"></canvas>
                </div>
                <a href="iPxSalesSummary.aspx" class="small-box-footer" style="color: rgba(0,0,0,0.8);">More info <i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div><!-- ./col -->
        <div id ="done" class="col-lg-4 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-aqua" style="background-color:white !important; border-color:white !important;">
                <div class="inner">
                    <canvas id="myChart" height="205"></canvas>
                </div>
                <a href="iPxARAging.aspx" class="small-box-footer" style="color: rgba(0,0,0,0.8);">More info <i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div><!-- ./col -->
        </div>
</asp:Content>

