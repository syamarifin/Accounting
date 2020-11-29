<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxAnalysGrafik.aspx.vb" Inherits="iPxAdmin_iPxAnalysGrafik" title="Alcor Accounting" %>

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
            padding-top:20px;
            color: #6f6f6f;
            display: block;
            z-index: 10;
            background: rgba(0,0,0,0);
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
            showGraphBS();
            showGraphBSvsPL();
            showGraphRevenuevsBudget();
            showGraphRevenuevsCash();
            // showGraphSetlement();
        });

        function showGraphBS() {
        $.ajax({
            type: "POST",
            url: "iPxAnalysGrafik.aspx/ChartBS",
            data: '{"dateAnalys": "' + document.getElementById("<%=tbDate.ClientID %>").value + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessGraphBS,
            failure: function(responseGraphBS) {
                alert(responseGraphBS.d);
            }
        });
        }
        function OnSuccessGraphBS(responseGraphBS) {
            data = JSON.parse(responseGraphBS.d);
            console.log(data);
            var unAsset     = [];
            var unLiabilty  = [];
            var unEquity    = [];
            var unRev       = [];
            var unCost      = [];
            var unExpe      = [];

            var unBS        = [];
            var unPL        = [];
            $(data).each(function(key, value) {
                unAsset     += value.asset;
                unLiabilty  += value.Liability;
                unEquity    += value.equity;
                unRev       += value.Revenue;
                unCost      += value.Cost;
                unExpe      += value.Expenses;

                unBS        += (value.asset + (value.Liability - value.equity));
                unPL        += (value.Revenue +(value.Cost - value.Expenses));

            });
            //$('#notifNewTicket').html(unread30);
            // Chart Balance Sheet
            var ctx = document.getElementById("myChartBS");
            var myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: ["Asset", "Liability", "Equity", "Control Balance"],
                    datasets: [{
                            label: 'Balance Sheet',
                            data: [unAsset, unLiabilty, unEquity, unBS],
                            backgroundColor: [
                                '#36a2eb',
                                '#4bc0c0',
                                '#ffcd56',
                                '#ff6384'
                            ],
                            borderColor: [
                                '#36a2eb',
                                '#4bc0c0',
                                '#ffcd56',
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
                        text: 'Balance Sheet'
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

            //Chart PL
            var ctxPL = document.getElementById("myChartPL");
            var myChart = new Chart(ctxPL, {
                type: 'bar',
                data: {
                    labels: ["Revenue", "Cost", "Expense", "Profit and Loss"],
                    datasets: [{
                            label: 'Profit and Lost',
                            data: [unRev, unCost, unExpe, unPL],
                            backgroundColor: [
                                '#36a2eb',
                                '#4bc0c0',
                                '#ffcd56',
                                '#ff6384'
                            ],
                            borderColor: [
                                '#36a2eb',
                                '#4bc0c0',
                                '#ffcd56',
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
                        text: 'Profit and Lost'
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

        function showGraphBSvsPL() {
        $.ajax({
            type: "POST",
            url: "iPxAnalysGrafik.aspx/ChartBSvsPL",
            data: '{"dateAnalys": "' + document.getElementById("<%=tbDate.ClientID %>").value + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessGraphBSvsPL,
            failure: function(responseGraphBSvsPL) {
                alert(responseGraphBSvsPL.d);
            }
        });
        }
        function OnSuccessGraphBSvsPL(responseGraphBSvsPL) {
            data = JSON.parse(responseGraphBSvsPL.d);
            console.log(data);
            var unBS1       = [];
            var unPL1       = [];
            var unBS2       = [];
            var unPL2       = [];
            var unBS3       = [];
            var unPL3       = [];
            var unBS4       = [];
            var unPL4       = [];
            var unBS5       = [];
            var unPL5       = [];
            var unBS6       = [];
            var unPL6       = [];
            var unBln1       = [];
            var unBln2       = [];
            var unBln3       = [];
            var unBln4       = [];
            var unBln5       = [];
            var unBln6       = [];
            $(data).each(function(key, value) {
                unBS1       += (value.asset1 - (value.Liability1 + value.equity1));
                unPL1       += (value.Revenue1 -(value.Cost1 + value.Expenses1));

                unBS2       += (value.asset2 - (value.Liability2 + value.equity2));
                unPL2       += (value.Revenue2 -(value.Cost2 + value.Expenses2));

                unBS3       += (value.asset3 - (value.Liability3 + value.equity3));
                unPL3       += (value.Revenue3 -(value.Cost3 + value.Expenses3));

                unBS4       += (value.asset4 - (value.Liability4 + value.equity4));
                unPL4       += (value.Revenue4 -(value.Cost4 + value.Expenses4));

                unBS5       += (value.asset5 - (value.Liability5 + value.equity5));
                unPL5       += (value.Revenue5 -(value.Cost5 + value.Expenses5));

                unBS6       += (value.asset6 - (value.Liability6 + value.equity6));
                unPL6       += (value.Revenue6 -(value.Cost6 + value.Expenses6));

                unBln1      += value.bln1;
                unBln2      += value.bln2;
                unBln3      += value.bln3;
                unBln4      += value.bln4;
                unBln5      += value.bln5;
                unBln6      += value.bln6;
            });
            //$('#notifNewTicket').html(unread30);
            // Chart Balance Sheet
            var ctxBSvsPL = document.getElementById("myChartBS_PL");
            var myChart = new Chart(ctxBSvsPL, {
                type: 'bar',
                data:  {
                    labels: [unBln6, unBln5, unBln4, unBln3, unBln2, unBln1],
                    datasets: [{
                            label: 'Profit and Loss',
                            data: [unPL6, unPL5, unPL4, unPL3, unPL2, unPL1],
                            backgroundColor: [
                                '#4bc0c0',
                                '#4bc0c0',
                                '#4bc0c0',
                                '#4bc0c0',
                                '#4bc0c0',
                                '#4bc0c0'
                            ],
                            borderColor: [
                                '#4bc0c0',
                                '#4bc0c0',
                                '#4bc0c0',
                                '#4bc0c0',
                                '#4bc0c0',
                                '#4bc0c0'
                            ],
                            borderWidth: 1
                        }]
                },
                options: {
                    legend: {
                        display: false,
                        position:'bottom'
                    },
                    title: {
                        display: true,
                        text: '6 Month Profit and Loss'
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
        
        function showGraphRevenuevsBudget() {
        $.ajax({
            type: "POST",
            url: "iPxAnalysGrafik.aspx/ChartRevenuevsBudget",
            data: '{"dateAnalys": "' + document.getElementById("<%=tbDate.ClientID %>").value + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessGraphRevenuevsBudget,
            failure: function(responseGraphRevenuevsBudget) {
                alert(responseGraphRevenuevsBudget.d);
            }
        });
        }
        function OnSuccessGraphRevenuevsBudget(responseGraphRevenuevsBudget) {
            data = JSON.parse(responseGraphRevenuevsBudget.d);
            console.log(data);
            
            var unRev1       = [];
            var unBudg1      = [];
            var unRev2       = [];
            var unBudg2      = [];
            var unRev3       = [];
            var unBudg3      = [];
            var unRev4       = [];
            var unBudg4      = [];
            var unRev5       = [];
            var unBudg5      = [];
            var unRev6       = [];
            var unBudg6      = [];

            var unCost1      = [];
            var unBudgC1     = [];
            var unCost2      = [];
            var unBudgC2     = [];
            var unCost3      = [];
            var unBudgC3     = [];
            var unCost4      = [];
            var unBudgC4     = [];
            var unCost5      = [];
            var unBudgC5     = [];
            var unCost6      = [];
            var unBudgC6     = [];

            var unBln1       = [];
            var unBln2       = [];
            var unBln3       = [];
            var unBln4       = [];
            var unBln5       = [];
            var unBln6       = [];
            $(data).each(function(key, value) {
                unRev1       += value.Revenue1;
                unBudg1      = value.Budg1;

                unRev2       += value.Revenue2;
                unBudg2      = value.Budg2;

                unRev3       += value.Revenue3;
                unBudg3      = value.Budg3;

                unRev4       += value.Revenue4;
                unBudg4      = value.Budg4;

                unRev5       += value.Revenue5;
                unBudg5      = value.Budg5;

                unRev6       += value.Revenue6;
                unBudg6      = value.Budg6;

                unCost1       += (value.Cost1 + value.Expenses1);
                unBudgC1      = (value.BudgCost1 + value.BudgExp1);

                unCost2       += (value.Cost2 + value.Expenses2);
                unBudgC2      = (value.BudgCost2 + value.BudgExp2);

                unCost3       += (value.Cost3 + value.Expenses3);
                unBudgC3      = (value.BudgCost3 + value.BudgExp3);

                unCost4       += (value.Cost4 + value.Expenses4);
                unBudgC4      = (value.BudgCost4 + value.BudgExp4);

                unCost5       += (value.Cost5 + value.Expenses5);
                unBudgC5      = (value.BudgCost5 + value.BudgExp5);

                unCost6       += (value.Cost6 + value.Expenses6);
                unBudgC6      = (value.BudgCost6 + value.BudgExp6);

                unBln1      += value.bln1;
                unBln2      += value.bln2;
                unBln3      += value.bln3;
                unBln4      += value.bln4;
                unBln5      += value.bln5;
                unBln6      += value.bln6;
            });
            //$('#notifNewTicket').html(unread30);
            // Chart Rev vs Budget
            var ctxRev = document.getElementById("myChartRev");
            var myChart = new Chart(ctxRev, {
                type: 'bar',
                data: {
                    labels: [unBln6, unBln5, unBln4, unBln3, unBln2, unBln1],
                    datasets: [{
                            label: 'Revenue',
                            data: [unRev6, unRev5, unRev4, unRev3, unRev2, unRev1],
                            backgroundColor: [
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb'
                            ],
                            borderColor: [
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb'
                            ],
                            borderWidth: 1
                        },
                        {
                            label: 'Budget',
                            data: [unBudg6, unBudg5, unBudg4, unBudg3, unBudg2, unBudg1],
                            backgroundColor: [
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56'
                            ],
                            borderColor: [
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56'
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
                        text: '6 Month Revenue vs Budget'
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

            // Chart Rev vs Budget
            var ctxCostExp = document.getElementById("myChartCostExp");
            var myChart = new Chart(ctxCostExp, {
                type: 'bar',
                data: {
                    labels: [unBln6, unBln5, unBln4, unBln3, unBln2, unBln1],
                    datasets: [{
                            label: 'Cost Expenses',
                            data: [unCost6, unCost6, unCost4, unCost3, unCost2, unCost1],
                            backgroundColor: [
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb'
                            ],
                            borderColor: [
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb'
                            ],
                            borderWidth: 1
                        },
                        {
                            label: 'Budget',
                            data: [unBudgC6, unBudgC5, unBudgC4, unBudgC3, unBudgC2, unBudgC1],
                            backgroundColor: [
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56'
                            ],
                            borderColor: [
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56'
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
                        text: '6 Month Cost Expenses vs Budget'
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
        

        
        function showGraphRevenuevsCash() {
        $.ajax({
            type: "POST",
            url: "iPxAnalysGrafik.aspx/ChartCash",
            data: '{"dateAnalys": "' + document.getElementById("<%=tbDate.ClientID %>").value + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessGraphRevenuevsCash,
            failure: function(responseGraphRevenuevsCash) {
                alert(responseGraphRevenuevsCash.d);
            }
        });
        }
        function OnSuccessGraphRevenuevsCash(responseGraphRevenuevsCash) {
            data = JSON.parse(responseGraphRevenuevsCash.d);
            console.log(data);

            var unCost1      = [];
            var unBudgC1     = [];
            var unCost2      = [];
            var unBudgC2     = [];
            var unCost3      = [];
            var unBudgC3     = [];
            var unCost4      = [];
            var unBudgC4     = [];
            var unCost5      = [];
            var unBudgC5     = [];
            var unCost6      = [];
            var unBudgC6     = [];

            var unBln1       = [];
            var unBln2       = [];
            var unBln3       = [];
            var unBln4       = [];
            var unBln5       = [];
            var unBln6       = [];
            $(data).each(function(key, value) {

                unCost1       += (value.Cost1 + value.Expenses1);
                unBudgC1      = (value.Cash1);

                unCost2       += (value.Cost2 + value.Expenses2);
                unBudgC2      = (value.Cash2);

                unCost3       += (value.Cost3 + value.Expenses3);
                unBudgC3      = (value.Cash3);

                unCost4       += (value.Cost4 + value.Expenses4);
                unBudgC4      = (value.Cash4);

                unCost5       += (value.Cost5 + value.Expenses5);
                unBudgC5      = (value.Cash5 );

                unCost6       += (value.Cost6 + value.Expenses6);
                unBudgC6      = (value.Cash6 );

                unBln1      += value.bln1;
                unBln2      += value.bln2;
                unBln3      += value.bln3;
                unBln4      += value.bln4;
                unBln5      += value.bln5;
                unBln6      += value.bln6;
            });
            // Chart Cost vs Cash
            var ctxCostExp = document.getElementById("myChartCostExpvsCash");
            var myChart = new Chart(ctxCostExp, {
                type: 'bar',
                data: {
                    labels: [unBln6, unBln5, unBln4, unBln3, unBln2, unBln1],
                    datasets: [{
                            label: 'Cost Expenses',
                            data: [unCost6, unCost6, unCost4, unCost3, unCost2, unCost1],
                            backgroundColor: [
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb'
                            ],
                            borderColor: [
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb',
                                '#36a2eb'
                            ],
                            borderWidth: 1
                        },
                        {
                            label: 'Cash Bank',
                            data: [unBudgC6, unBudgC5, unBudgC4, unBudgC3, unBudgC2, unBudgC1],
                            backgroundColor: [
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56'
                            ],
                            borderColor: [
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56',
                                '#ffcd56'
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
                        text: '6 Month Cost Expenses vs Cash Bank'
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
                    <asp:TextBox ID="tbDate" runat="server" CssClass ="form-control" placeholder="MM yyyy" OnTextChanged="cari" AutoPostBack="true"></asp:TextBox>
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
        </div>
    </div>
    <div class="clearfix">
        <div id ="BS" class="col-lg-6 col-xs-6" style="padding-left:5px; padding-right:5px;">
            <!-- small box -->
            <div class="small-box bg-aqua" style="height:345px; margin-bottom:10px; background-color:white !important; border-color:rgba(207, 207, 207, 0.89) !important;">
                <div class="inner">
                    <canvas id="myChartBS" height="200"></canvas>
                </div>
                <!-- <a href="#" class="small-box-footer" style="color: rgba(0,0,0,0.8);">More info <i class="fa fa-arrow-circle-right"></i></a> -->
            </div>
        </div><!-- ./col -->
        <div id ="PL" class="col-lg-6 col-xs-6" style="padding-left:5px; padding-right:5px;">
            <!-- small box -->
            <div class="small-box bg-aqua" style="height:345px; margin-bottom:10px; background-color:white !important; border-color:rgba(207, 207, 207, 0.89) !important;">
                <div class="inner">
                    <canvas id="myChartPL" height="200"></canvas>
                </div>
                <!-- <a href="#" class="small-box-footer" style="color: rgba(0,0,0,0.8);">More info <i class="fa fa-arrow-circle-right"></i></a> -->
            </div>
        </div><!-- ./col -->

        <div id ="BS_PL" class="col-lg-6 col-xs-6" style="padding-left:5px; padding-right:5px;">
            <!-- small box -->
            <div class="small-box bg-aqua" style="height:345px; margin-bottom:10px; background-color:white !important; border-color:rgba(207, 207, 207, 0.89) !important;">
                <div class="inner">
                    <canvas id="myChartBS_PL" height="200"></canvas>
                </div>
                <!-- <a href="#" class="small-box-footer" style="color: rgba(0,0,0,0.8);">More info <i class="fa fa-arrow-circle-right"></i></a> -->
            </div>
        </div><!-- ./col -->
        <div id ="Rev" class="col-lg-6 col-xs-6" style="padding-left:5px; padding-right:5px;">
            <!-- small box -->
            <div class="small-box bg-aqua" style="height:345px; margin-bottom:10px; background-color:white !important; border-color:rgba(207, 207, 207, 0.89) !important;">
                <div class="inner">
                    <canvas id="myChartRev" height="200"></canvas>
                </div>
                <!-- <a href="#" class="small-box-footer" style="color: rgba(0,0,0,0.8);">More info <i class="fa fa-arrow-circle-right"></i></a> -->
            </div>
        </div><!-- ./col -->
        <div id ="CostExp" class="col-lg-6 col-xs-6" style="padding-left:5px; padding-right:5px;">
            <!-- small box -->
            <div class="small-box bg-aqua" style="height:345px; margin-bottom:10px; background-color:white !important; border-color:rgba(207, 207, 207, 0.89) !important;">
                <div class="inner">
                    <canvas id="myChartCostExp" height="200"></canvas>
                </div>
                <!-- <a href="#" class="small-box-footer" style="color: rgba(0,0,0,0.8);">More info <i class="fa fa-arrow-circle-right"></i></a> -->
            </div>
        </div><!-- ./col -->
        <div id ="CostExpvsCash" class="col-lg-6 col-xs-6" style="padding-left:5px; padding-right:5px;">
            <!-- small box -->
            <div class="small-box bg-aqua" style="height:345px; margin-bottom:10px; background-color:white !important; border-color:rgba(207, 207, 207, 0.89) !important;">
                <div class="inner">
                    <canvas id="myChartCostExpvsCash" height="200"></canvas>
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

