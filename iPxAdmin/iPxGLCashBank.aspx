<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxGLCashBank.aspx.vb" Inherits="iPxAdmin_iPxGLCashBank" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        function MonthGL() {
            $(".monthGl").datepicker({ format: 'MM yyyy', viewMode: "months", minViewMode: "months", autoclose: true, todayBtn: 'linked' })
        }
        function showDetail(){
            $("#header-pan").removeClass("col-lg-12");
            $("#header-pan").addClass("col-lg-6");
            $("#panDetail").removeClass("hidden");
        }
        function hideDetail(){
            $("#header-pan").removeClass("col-lg-6");
            $("#header-pan").addClass("col-lg-12");
            $("#panDetail").addClass("hidden");
        }
        function ControlActive()
        {
            $("#CashFlow-tab").removeClass("active");
            $("#Control-tab").addClass("active");
            $("#CashFlow").removeClass("active in");
            $("#Control").addClass("active in");
        }
    </script>
    <script>
        $(document).ready(function () {
            showGraphCash();
        });

        function showGraphCash() {
        $.ajax({
            type: "POST",
            url: "iPxGLCashBank.aspx/ChartCash",
            data: '{"dateAnalys": "' + document.getElementById("<%=tbDateCash.ClientID %>").value + '"}',
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
            var unBS1       = [];
            var unBS2       = [];
            var unBS3       = [];
            var unBS4       = [];
            var unBS5       = [];
            var unBS6       = [];
            var unBln1       = [];
            var unBln2       = [];
            var unBln3       = [];
            var unBln4       = [];
            var unBln5       = [];
            var unBln6       = [];
            $(data).each(function(key, value) {
                unBS1       += value.Cash1;

                unBS2       += value.Cash2 ;

                unBS3       += value.Cash3;

                unBS4       += value.Cash4;

                unBS5       += value.Cash5;

                unBS6       += value.Cash6;

                unBln1      += value.bln1;
                unBln2      += value.bln2;
                unBln3      += value.bln3;
                unBln4      += value.bln4;
                unBln5      += value.bln5;
                unBln6      += value.bln6;

            });
            var ctx = document.getElementById("myChartCash");
            var myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: [unBln6, unBln5, unBln4, unBln3, unBln2, unBln1],
                    datasets: [{
                            label: 'Balance Sheet',
                            data: [unBS6, unBS5, unBS4, unBS3, unBS2, unBS1],
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
                        text: '6 Month Cash & Bank'
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
    <style>
        .header-right{
            text-align:right;
        }
        .header-center{
            text-align:center;
        }
        .max{
            width:100%;
        }
        .min{
            width:60%;
        }
        .tombol-close
        {
        	padding-right:5px;
        }
        .scroll
        {
        	overflow-x:scroll;
        }
    </style>
    <style>
        .cellOneCellPaddingLeft {
            padding-left: 0pt !important;
        }
        .cellOneCellPaddingRight {
            padding-right: 0pt !important;
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
            padding-top:20px;
            color: #6f6f6f;
            display: block;
            z-index: 10;
            background: rgba(0,0,0,0);
            text-decoration: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Create Bath Modal-->
    <div id="formInput" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md" style="width:750px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortDetail" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="login-modalLabel" class="modal-title">
                        <asp:Label ID="lbTitleDetailJournal" runat="server" Text="Detail Journal "></asp:Label>
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvDetail" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" ShowFooter="true">
                                <Columns>
                                    <asp:BoundField DataField="RecID" HeaderText="Rec ID" />
                                    <asp:BoundField DataField="Coa" HeaderText="COA" />
                                    <asp:BoundField DataField="coaDesc" HeaderText="COA Description" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:BoundField DataField="Reff" HeaderText="Reff No" />
                                    <asp:BoundField DataField="debit" HeaderText="Debit" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right"/>
                                    <asp:BoundField DataField="credit" HeaderText="Credit" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right"/>
                                    <asp:BoundField DataField="balance" HeaderText="Balance" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right"/>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Create Bath modal end-->
    <ul class="nav nav-tabs">
        <li id="CashFlow-tab" class="active"><a href="#CashFlow" data-toggle="tab"><i class="fa fa-list"></i> Cash Flow</a></li>
        <li id="Control-tab"><a href="#Control" data-toggle="tab"><i class="fa fa-list"></i> Cash & Bank Control</a></li>
    </ul>
    <div id="myTabContent" class="tab-content">
        <div class="tab-pane active in" id="CashFlow">
            <div class="row">
                <div class="col-md-1">
                    <div class="form-group">
                        <label for="usr">Month:</label><font color=red>*</font>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <div class="input-group date monthGl" style="padding:0;">
                            <asp:TextBox ID="tbDateCash" runat="server" CssClass ="form-control" placeholder="dd/MM/yyyy" OnTextChanged="cariCash" AutoPostBack="true"></asp:TextBox>
                            <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                        </div>
                    </div>
                </div>
                <br />
                <br />
                <br />
                <div class="col-md-6">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvCashFlow" runat="server" AutoGenerateColumns="false" 
                    CssClass="table" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" HeaderStyle-CssClass="hidden" 
                    GridLines="None" Width="100%" OnRowDataBound="OnRowDataBound">
                        <Columns>
                            <%--<asp:BoundField ItemStyle-Width="70px" DataField="Coa" HeaderText="COA" />--%>
                            <%--<asp:BoundField HeaderText="Description" DataField="Desch" />--%>
                            <asp:TemplateField HeaderText="Saldo" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Left" >
                                <ItemTemplate>
                                    <asp:Label ID="lblDesch" runat="server" Text='<%# if(Eval("Desch").toString()="OPENING CASH & BANK" or Eval("Desch").toString()="Total Cash & Bank In" or Eval("Desch").toString()="Total Cash & Bank Out" or Eval("Desch").toString()="ENDING CASH & BANK",Eval("Desch").toString()," - "+Eval("Desch").toString()) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Saldo" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" ItemStyle-CssClass="cellOneCellPaddingRight">
                                <ItemTemplate>
                                    <asp:Label ID="lblDetail" runat="server" Text='<%# if(Eval("Desch").toString()="OPENING CASH & BANK" or Eval("Desch").toString()="Total Cash & Bank In" or Eval("Desch").toString()="Total Cash & Bank Out" or Eval("Desch").toString()="ENDING CASH & BANK","",if(Eval("amount").toString()="","0.00",Eval("amount","{0:N2}"))) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Saldo" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" ItemStyle-CssClass="cellOneCellPaddingRight">
                                <ItemTemplate>
                                    <asp:Label ID="lblDebit" runat="server" Text='<%# if(Eval("Desch").toString()<>"OPENING CASH & BANK" and Eval("Desch").toString()<>"Total Cash & Bank In" and Eval("Desch").toString()<>"Total Cash & Bank Out" and Eval("Desch").toString()<>"ENDING CASH & BANK","",if(Eval("amount").toString()="","0.00",Eval("amount","{0:N2}"))) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="col-md-6">
                    <div class="clearfix">
                        <div id ="CashGrafik" class="col-lg-12 col-xs-12" style="padding-left:5px; padding-right:5px;">
                            <!-- small box -->
                            <div class="small-box bg-aqua" style="height:345px; margin-bottom:10px; background-color:white !important; border-color:rgba(207, 207, 207, 0.89) !important;">
                                <div class="inner">
                                    <canvas id="myChartCash" height="200"></canvas>
                                </div>
                                <!-- <a href="#" class="small-box-footer" style="color: rgba(0,0,0,0.8);">More info <i class="fa fa-arrow-circle-right"></i></a> -->
                            </div>
                        </div><!-- ./col -->
                    </div>
                </div>
            </div>
        </div>
        <div class="tab-pane" id="Control">
            <div class="row">
                <div class="col-md-1">
                    <div class="form-group">
                        <label for="usr">Month:</label><font color=red>*</font>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <div class="input-group date monthGl" style="padding:0;">
                            <asp:TextBox ID="tbDate" runat="server" CssClass ="form-control" placeholder="dd/MM/yyyy" OnTextChanged="cari" AutoPostBack="true"></asp:TextBox>
                            <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="width:100%;">
                <div id="header-pan" class="col-lg-6" style="padding-left:5px; padding-right:5px;">
                    <div id="panCash" class="panel panel-default" style="border-top:3px solid #0a818e;">
                        <div class="panel-body">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvCashSummary" runat="server" AutoGenerateColumns="false" 
                            CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" 
                            GridLines="None" AllowPaging="true" PageSize="15" Width="100%">
                                <Columns>
                                <%--<asp:BoundField ItemStyle-Width="90px" DataField="TransDate" HeaderText="Date"  DataFormatString="{0:dd-MM-yyyy}"/>--%>
                                    <asp:BoundField ItemStyle-Width="70px" DataField="Coa" HeaderText="COA" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:TemplateField HeaderText="Saldo" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="120px" ItemStyle-CssClass="cellOneCellPaddingRight">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDebit" runat="server" Text='<%# if(Eval("amountDebit").toString()="","0.00",Eval("amountDebit","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Credit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="120px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCredit" runat="server" Text='<%# if(Eval("amountCredit").toString()="","0.00",Eval("amountCredit","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbDetail" runat="server" CssClass="btn btn-link" CommandName="getDetail" CommandArgument='<%# Eval("Coa") %>' style="color:#083765; padding:0; font-size:24px;"><i class="fa fa-caret-down"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <pagerstyle cssclass="pagination-ys">
                                </pagerstyle>
                            </asp:GridView>
                        </div>
                    </div>    
                </div>
                <div id="panDetail" class="col-lg-6" style="padding-left:5px; padding-right:5px;">
                    <div class="panel panel-default" style="border-top:3px solid #0a818e;">
                        <%--<asp:LinkButton ID="lbCloseDetail" CssClass="close tombol-close" runat="server" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>--%>
                        <div class="panel-body">
                            <div style="overflow-x:auto; max-width:475px;">
                                <asp:GridView EmptyDataText="No records has been added." ID="gvDetailOpen" runat="server" AutoGenerateColumns="false" 
                                CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" 
                                GridLines="None" AllowPaging="true" PageSize="10">
                                    <Columns>
                                        <asp:BoundField ItemStyle-Width="70px" DataField="Coa" HeaderText="COA" />
                                        <asp:BoundField DataField="Description" HeaderText="Description" />
                                        <asp:TemplateField HeaderText="Open" HeaderStyle-CssClass="header-right" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDebit" Text='<%# If(Eval("amountDebit").ToString() = "0.0000", "0.00", If(Eval("amountDebit").ToString() = "", "0.00", String.Format("{0:N2}", (Eval("amountDebit"))))) %>'
                                                runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Credit" HeaderStyle-CssClass="header-right" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCredit" Text='<%# If(Eval("amountCredit").ToString() = "0.0000", "0.00", If(Eval("amountCredit").ToString() = "", "0.00", String.Format("{0:N2}", (Eval("amountCredit"))))) %>'
                                                runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                    <pagerstyle cssclass="pagination-ys">
                                    </pagerstyle>
                                </asp:GridView>
                                <hr />
                                <asp:GridView EmptyDataText="No records has been added." ID="gvDetailCash" runat="server" AutoGenerateColumns="false" 
                                CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" 
                                GridLines="None" AllowPaging="true" PageSize="10" ShowFooter="true">
                                    <Columns>
                                        <asp:BoundField ItemStyle-Width="40px" DataField="day" HeaderText="Date" />
                                        <asp:BoundField ItemStyle-Width="100px" DataField="TransID" HeaderText="Journal" />
                                        <asp:BoundField ItemStyle-Width="100px" DataField="GlGrp" HeaderText="Group" />
                                        <%--<asp:BoundField DataField="Description" HeaderText="Description" />--%>
                                        <asp:BoundField ItemStyle-Width="100px" DataField="Reff" HeaderText="Reff" />
                                        <asp:TemplateField HeaderText="Debit" HeaderStyle-CssClass="header-right" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDebit" Text='<%# If(Eval("Debit").ToString() = "0.0000", "0.00", String.Format("{0:N2}", (Eval("Debit")))) %>'
                                                runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Credit" HeaderStyle-CssClass="header-right" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="cellOneCellPaddingRight">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCredit" Text='<%# If(Eval("Credit").ToString() = "0.0000", "0.00", String.Format("{0:N2}", (Eval("Credit")))) %>'
                                                runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbDetail" runat="server" CssClass="btn btn-link" CommandName="getDetail" CommandArgument='<%# Eval("TransID") %>' style="color:#083765; padding:0; font-size:24px;"><i class="fa fa-caret-down"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <pagerstyle cssclass="pagination-ys">
                                    </pagerstyle>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

