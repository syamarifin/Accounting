<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxAPAnalys.aspx.vb" Inherits="iPxAdmin_iPxAPAnalys" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
            background: rgba(0,0,0,0.1);
            text-decoration: none;
        }
    </style>
    <script type="text/javascript">
        function MonthGL() {
            $(".monthGl").datepicker({ format: 'mm-yyyy', viewMode: "months", minViewMode: "months", autoclose: true, todayBtn: 'linked' })
        }
    </script>
    <script>
        $(document).ready(function () {
            showGraph();
            showReceiptMtd();
            showGraphARTransSix();
            showGraphARRecSix();
        });

        function showGraph() {
        $.ajax({
            type: "POST",
            url: "iPxAPAnalys.aspx/ChartAging",
            data: '{"dateAnalys": "' + document.getElementById("<%=tbDate.ClientID %>").value + '"}',
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
//            console.log(data);
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
            var ctxAging = document.getElementById("myChartAging");
            var myChart = new Chart(ctxAging, {
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
                        text: 'Account Payable Aging'
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
        
        function showReceiptMtd() {
        $.ajax({
            type: "POST",
            url: "iPxAPAnalys.aspx/ChartReceiptMtd",
            data: '{"dateAnalys": "' + document.getElementById("<%=tbDate.ClientID %>").value + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessRecMTD,
            failure: function(responseRecMTD) {
                alert(responseRecMTD.d);
            }
        });
        }
        function OnSuccessRecMTD(responseRecMTD) {
            dataGraf = JSON.parse(responseRecMTD.d);
            console.log(dataGraf);
            var datalabel =[];
            var dataAmount=[];
            $(dataGraf).each(function(key, value) {
                datalabel.push(value.DescPaid);
                dataAmount.push(value.Amount);
                console.log(value.DescPaid);
            });
            console.log(datalabel);
            var ctxAging = document.getElementById("myChartReceiptMtd");
            var myChart = new Chart(ctxAging, {
                type: 'bar',
                data: {
                    labels: datalabel,
                    datasets: [{
                            label: 'AP Payment',
                            data: dataAmount,
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
                        text: 'AP Payment MTD'
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
        
        function showGraphARTransSix() {
        $.ajax({
            type: "POST",
            url: "iPxAPAnalys.aspx/ChartTransSix",
            data: '{"dateAnalys": "' + document.getElementById("<%=tbDate.ClientID %>").value + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessARTransSix,
            failure: function(responseARTransSix) {
                alert(responseARTransSix.d);
            }
        });
        }
        function OnSuccessARTransSix(responseARTransSix) {
            data = JSON.parse(responseARTransSix.d);
//            console.log(data);
            var unamount1 = [];
            var unamount2 = [];
            var unamount3 = [];
            var unamount4 = [];
            var unamount5 = [];
            var unamount6 = [];
            var unBln1    = [];
            var unBln2    = [];
            var unBln3    = [];
            var unBln4    = [];
            var unBln5    = [];
            var unBln6    = [];
            $(data).each(function(key, value) {
                unamount1 += value.amountBln1;
                unamount2 += value.amountBln2;
                unamount3 += value.amountBln3;
                unamount4 += value.amountBln4;
                unamount5 += value.amountBln5;
                unamount6 += value.amountBln6;
                unBln1 += value.bln1;
                unBln2 += value.bln2;
                unBln3 += value.bln3;
                unBln4 += value.bln4;
                unBln5 += value.bln5;
                unBln6 += value.bln6;
            });
            //$('#notifNewTicket').html(unread30);
            var ctxAging = document.getElementById("myChartARTrans");
            var myChart = new Chart(ctxAging, {
                type: 'bar',
                data: {
                    labels: [unBln6, unBln5, unBln4, unBln3, unBln2, unBln1],
                    datasets: [{
                            label: 'AP Transaction 6MTD',
                            data: [unamount6, unamount5, unamount4, unamount3, unamount2, unamount1],
                            backgroundColor: [
                                '#36a2eb',
                                '#4bc0c0',
                                '#ffcd56',
                                '#ff9f40',
                                '#ff6384',
                                '#A52A2A'
                            ],
                            borderColor: [
                                '#36a2eb',
                                '#4bc0c0',
                                '#ffcd56',
                                '#ff9f40',
                                '#ff6384',
                                '#A52A2A'
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
                        text: 'AP Transaction 6MTD'
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
        
        function showGraphARRecSix() {
        $.ajax({
            type: "POST",
            url: "iPxAPAnalys.aspx/ChartReceiptSix",
            data: '{"dateAnalys": "' + document.getElementById("<%=tbDate.ClientID %>").value + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessARRecSix,
            failure: function(responseARRecSix) {
                alert(responseARRecSix.d);
            }
        });
        }
        function OnSuccessARRecSix(responseARRecSix) {
            data = JSON.parse(responseARRecSix.d);
//            console.log(data);
            var unamount1 = [];
            var unamount2 = [];
            var unamount3 = [];
            var unamount4 = [];
            var unamount5 = [];
            var unamount6 = [];
            var unBln1    = [];
            var unBln2    = [];
            var unBln3    = [];
            var unBln4    = [];
            var unBln5    = [];
            var unBln6    = [];
            $(data).each(function(key, value) {
                unamount1 += value.amountBln1;
                unamount2 += value.amountBln2;
                unamount3 += value.amountBln3;
                unamount4 += value.amountBln4;
                unamount5 += value.amountBln5;
                unamount6 += value.amountBln6;
                unBln1 += value.bln1;
                unBln2 += value.bln2;
                unBln3 += value.bln3;
                unBln4 += value.bln4;
                unBln5 += value.bln5;
                unBln6 += value.bln6;
            });
            //$('#notifNewTicket').html(unread30);
            var ctxAging = document.getElementById("myChartReceipt");
            var myChart = new Chart(ctxAging, {
                type: 'bar',
                data: {
                    labels: [unBln6, unBln5, unBln4, unBln3, unBln2, unBln1],
                    datasets: [{
                            label: 'AP Payment',
                            data: [unamount6, unamount5, unamount4, unamount3, unamount2, unamount1],
                            backgroundColor: [
                                '#36a2eb',
                                '#4bc0c0',
                                '#ffcd56',
                                '#ff9f40',
                                '#ff6384',
                                '#A52A2A'
                            ],
                            borderColor: [
                                '#36a2eb',
                                '#4bc0c0',
                                '#ffcd56',
                                '#ff9f40',
                                '#ff6384',
                                '#A52A2A'
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
                        text: 'AP Payment 6MTD'
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
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-md-1" style="text-align:right;">
            <div class="form-group">
                <label for="usr">Month:</label>
            </div>
        </div>
        <div class="col-md-2">
            <div class="form-group">
                <div class="input-group date monthGl" style="padding:0;">
                    <asp:TextBox ID="tbDate" runat="server" CssClass ="form-control" placeholder="mm-yyyy" OnTextChanged="cari" AutoPostBack="true"></asp:TextBox>
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
        </div>
    </div>
    <div class="clearfix">
        <div id ="AgingMTD" class="col-lg-6 col-xs-6" style="padding-left:5px; padding-right:5px;">
            <!-- small box -->
            <div class="small-box bg-aqua" style="margin-bottom:10px; background-color:white !important; border-color:rgba(207, 207, 207, 0.89) !important;">
                <div class="inner">
                    <canvas id="myChartAging" height="200"></canvas>
                </div>
                <!-- <a href="#" class="small-box-footer" style="color: rgba(0,0,0,0.8);">More info <i class="fa fa-arrow-circle-right"></i></a> -->
            </div>
        </div><!-- ./col -->
        <div id ="ReceiptMTD" class="col-lg-6 col-xs-6" style="padding-left:5px; padding-right:5px;">
            <!-- small box -->
            <div class="small-box bg-aqua" style="margin-bottom:10px; background-color:white !important; border-color:rgba(207, 207, 207, 0.89) !important;">
                <div class="inner">
                    <canvas id="myChartReceiptMtd" height="200"></canvas>
                </div>
                <!-- <a href="#" class="small-box-footer" style="color: rgba(0,0,0,0.8);">More info <i class="fa fa-arrow-circle-right"></i></a> -->
            </div>
        </div><!-- ./col -->

        <div id ="ARTrans" class="col-lg-6 col-xs-6" style="padding-left:5px; padding-right:5px;">
            <!-- small box -->
            <div class="small-box bg-aqua" style="margin-bottom:10px; background-color:white !important; border-color:rgba(207, 207, 207, 0.89) !important;">
                <div class="inner">
                    <canvas id="myChartARTrans" height="200"></canvas>
                </div>
                <!-- <a href="#" class="small-box-footer" style="color: rgba(0,0,0,0.8);">More info <i class="fa fa-arrow-circle-right"></i></a> -->
            </div>
        </div><!-- ./col -->
        <div id ="Receipt" class="col-lg-6 col-xs-6" style="padding-left:5px; padding-right:5px;">
            <!-- small box -->
            <div class="small-box bg-aqua" style="margin-bottom:10px; background-color:white !important; border-color:rgba(207, 207, 207, 0.89) !important;">
                <div class="inner">
                    <canvas id="myChartReceipt" height="200"></canvas>
                </div>
                <!-- <a href="#" class="small-box-footer" style="color: rgba(0,0,0,0.8);">More info <i class="fa fa-arrow-circle-right"></i></a> -->
            </div>
        </div><!-- ./col -->
     </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

