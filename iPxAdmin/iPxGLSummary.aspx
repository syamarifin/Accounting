<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxGLSummary.aspx.vb" Inherits="iPxAdmin_iPxGLSummary" title="Alcor Accounting" EnableEventValidation = "false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        function MonthGL() {
            $(".monthGl").datepicker({ format: 'mm-yyyy', viewMode: "months", minViewMode: "months", autoclose: true, todayBtn: 'linked' })
        }
        function StatistiActive()
        {
            $("#Div2").addClass("hidden");
            $("#Div3").addClass("hidden");
            $("#Div4").addClass("hidden");
        }
        function StatistiNonActive()
        {
            $("#Div2").removeClass("hidden");
            $("#Div3").removeClass("hidden");
            $("#Div4").removeClass("hidden");
        }
        function BalanceTabShow()
        {
            $("#Asset-tab").removeClass("hidden");
            $("#Liability-tab").removeClass("hidden");
            $("#Equity-tab").removeClass("hidden");
            $("#Revenue-tab").addClass("hidden");
            $("#Cost-tab").addClass("hidden");
            $("#Expenses-tab").addClass("hidden");
            $("#Statistic-tab").addClass("hidden");

            $("#Asset").removeClass("hidden");
            $("#Liability").removeClass("hidden");
            $("#Equity").removeClass("hidden");
            $("#Revenue").addClass("hidden");
            $("#Cost").addClass("hidden");
            $("#Expenses").addClass("hidden");
            $("#Statistic").addClass("hidden");
        }
        function ProfitTabShow()
        {
            $("#Asset-tab").addClass("hidden");
            $("#Liability-tab").addClass("hidden");
            $("#Equity-tab").addClass("hidden");
            $("#Revenue-tab").removeClass("hidden");
            $("#Cost-tab").removeClass("hidden");
            $("#Expenses-tab").removeClass("hidden");
            $("#Statistic-tab").addClass("hidden");

            $("#Asset").addClass("hidden");
            $("#Liability").addClass("hidden");
            $("#Equity").addClass("hidden");
            $("#Revenue").removeClass("hidden");
            $("#Cost").removeClass("hidden");
            $("#Expenses").removeClass("hidden");
            $("#Statistic").addClass("hidden");
        }
        function StatisticTabShow()
        {
            $("#Asset-tab").addClass("hidden");
            $("#Liability-tab").addClass("hidden");
            $("#Equity-tab").addClass("hidden");
            $("#Revenue-tab").addClass("hidden");
            $("#Cost-tab").addClass("hidden");
            $("#Expenses-tab").addClass("hidden");
            $("#Statistic-tab").removeClass("hidden");

            $("#Asset").addClass("hidden");
            $("#Liability").addClass("hidden");
            $("#Equity").addClass("hidden");
            $("#Revenue").addClass("hidden");
            $("#Cost").addClass("hidden");
            $("#Expenses").addClass("hidden");
            $("#Statistic").removeClass("hidden");
        }

        function AssetActive()
        {
            $("#Asset-tab").addClass("active");
            $("#Liability-tab").removeClass("active");
            $("#Equity-tab").removeClass("active");
            $("#Revenue-tab").removeClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Expenses-tab").removeClass("active");
            $("#Statistic-tab").removeClass("active");

            $("#Asset").addClass("active in");
            $("#Liability").removeClass("active in");
            $("#Equity").removeClass("active in");
            $("#Revenue").removeClass("active in");
            $("#Cost").removeClass("active in");
            $("#Expenses").removeClass("active in");
            $("#Statistic").removeClass("active in");
        }
        function LiabilityActive()
        {
            $("#Asset-tab").removeClass("active");
            $("#Liability-tab").addClass("active");
            $("#Equity-tab").removeClass("active");
            $("#Revenue-tab").removeClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Expenses-tab").removeClass("active");
            $("#Statistic-tab").removeClass("active");

            $("#Asset").removeClass("active in");
            $("#Liability").addClass("active in");
            $("#Equity").removeClass("active in");
            $("#Revenue").removeClass("active in");
            $("#Cost").removeClass("active in");
            $("#Expenses").removeClass("active in");
            $("#Statistic").removeClass("active in");
        }
        function EquityActive()
        {
            $("#Asset-tab").removeClass("active");
            $("#Liability-tab").removeClass("active");
            $("#Equity-tab").addClass("active");
            $("#Revenue-tab").removeClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Expenses-tab").removeClass("active");
            $("#Statistic-tab").removeClass("active");

            $("#Asset").removeClass("active in");
            $("#Liability").removeClass("active in");
            $("#Equity").addClass("active in");
            $("#Revenue").removeClass("active in");
            $("#Cost").removeClass("active in");
            $("#Expenses").removeClass("active in");
            $("#Statistic").removeClass("active in");
        }
        function revenueActive()
        {
            $("#Asset-tab").removeClass("active");
            $("#Liability-tab").removeClass("active");
            $("#Equity-tab").removeClass("active");
            $("#Revenue-tab").addClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Expenses-tab").removeClass("active");
            $("#Statistic-tab").removeClass("active");

            $("#Asset").removeClass("active in");
            $("#Liability").removeClass("active in");
            $("#Equity").removeClass("active in");
            $("#Revenue").addClass("active in");
            $("#Cost").removeClass("active in");
            $("#Expenses").removeClass("active in");
            $("#Statistic").removeClass("active in");
        }
        function CostActive()
        {
            $("#Asset-tab").removeClass("active");
            $("#Liability-tab").removeClass("active");
            $("#Equity-tab").removeClass("active");
            $("#Revenue-tab").removeClass("active");
            $("#Cost-tab").addClass("active");
            $("#Expenses-tab").removeClass("active");
            $("#Statistic-tab").removeClass("active");

            $("#Asset").removeClass("active in");
            $("#Liability").removeClass("active in");
            $("#Equity").removeClass("active in");
            $("#Revenue").removeClass("active in");
            $("#Cost").addClass("active in");
            $("#Expenses").removeClass("active in");
            $("#Statistic").removeClass("active in");
        }
        function ExpensesActive()
        {
            $("#Asset-tab").removeClass("active");
            $("#Liability-tab").removeClass("active");
            $("#Equity-tab").removeClass("active");
            $("#Revenue-tab").removeClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Expenses-tab").addClass("active");
            $("#Statistic-tab").removeClass("active");

            $("#Asset").removeClass("active in");
            $("#Liability").removeClass("active in");
            $("#Equity").removeClass("active in");
            $("#Revenue").removeClass("active in");
            $("#Cost").removeClass("active in");
            $("#Expenses").addClass("active in");
            $("#Statistic").removeClass("active in");
        }
        function StatisticTabActive()
        {
            $("#Asset-tab").removeClass("active");
            $("#Liability-tab").removeClass("active");
            $("#Equity-tab").removeClass("active");
            $("#Revenue-tab").removeClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Expenses-tab").removeClass("active");
            $("#Statistic-tab").addClass("active");

            $("#Asset").removeClass("active in");
            $("#Liability").removeClass("active in");
            $("#Equity").removeClass("active in");
            $("#Revenue").removeClass("active in");
            $("#Cost").removeClass("active in");
            $("#Expenses").removeClass("active in");
            $("#Statistic").addClass("active in");
        }
    </script>
    <style>
        .cellOneCellPaddingLeft {
            padding-left: 0pt !important;
        }
        .cellOneCellPaddingRight {
            padding-right: 0pt !important;
        }
    </style>
    <style>
        .header-center{
            text-align:center;
        }
        .header-midle
        {
    	    vertical-align:middle;
        }
        .header-right{
            text-align:right;
        }
        .SubTotalRowStyle{
            background-color:#81BEF7;
            font-weight:bold;
        }
        .header-border th
        {
        	border: 1px solid #e1edff;
        }
        .box
        {
        	position:relative;
        	border-radius:3px;
        	background:#dd4b39;
        	border-top:3px solid #d2d6de;
        	margin-bottom:20px;
        	width:100%;
        	box-shadow:0 1px 1px rgba(0,0,0,0.1)
        }        
        .box.box-default
        {
        	border-top-color:#d2d6de
        }
        .box-header
        {
        	background:#dd4b39
        }
        .box-header.with-border
        {
        	border-bottom:1px solid #f4f4f4
        }
        .box-header .box-title
        {
        	display:inline-block;
        	font-size:18px;
        	margin:0;
        	line-height:1
        }
        .box-body
        {
        	border-top-left-radius:0;
        	border-top-right-radius:0;
        	border-bottom-right-radius:3px;
        	border-bottom-left-radius:3px;
        	padding:10px
        }
        .padding-info
        {
        	padding-right: 5px;
            padding-left: 5px;
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
                        <asp:Label ID="lbTitleDetailCoa" runat="server" Text="Detail Clear "></asp:Label>
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvDetail" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" ShowFooter="true">
                                <Columns>
                                    <asp:BoundField DataField="TransID" HeaderText="Trans ID" />
                                    <asp:BoundField DataField="GroupGL" HeaderText="Group" />
                                    <asp:BoundField DataField="TransDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}"/>
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:BoundField DataField="ReffNo" HeaderText="Reff No" />
                                    <asp:BoundField DataField="debit" HeaderText="Debit" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right"/>
                                    <asp:BoundField DataField="credit" HeaderText="Credit" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right"/>
                                    <asp:BoundField DataField="balance" HeaderText="Balance" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="cellOneCellPaddingRight"/>
                                    <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbDetaillv2" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("TransID") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Create Bath modal end-->
    <!-- Detail Coa Transaction-->
    <div id="formType" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md" style="width:850px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortDetailLv2" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H1" class="modal-title">
                        <asp:Label ID="Label4" runat="server" Text="Detail Clear "></asp:Label>
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvHeaderGL" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None">
                                <Columns>
                                    <asp:BoundField ItemStyle-Width="100px" DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date" />
                                    <asp:BoundField ItemStyle-Width="200px" DataField="GlGrp" HeaderText="Group" />
                                    <asp:BoundField ItemStyle-Width="150px" DataField="ReffNo" HeaderText="Reff NO" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                </Columns>
                            </asp:GridView>
                            <hr />
                            <asp:GridView EmptyDataText="No records has been added." ID="gvDetailCoaTrans" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" ShowFooter="true">
                                <Columns>
                                    <%--<asp:BoundField DataField="TransID" HeaderText="Trans ID" />--%>
                                    <asp:BoundField DataField="RecID" HeaderText="Rec ID" />
                                    <%--<asp:BoundField DataField="TransDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}"/>--%>
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
    <!-- Detail Coa Transaction end-->
    <%--<div id ="done" class="col-lg-12 col-xs-12" style="padding:0;">
        <!-- small box -->
        <div class="small-box" style="background-color:White; margin-bottom:10px; border-radius:5px;">
            <div class="inner">--%>
                <div class="row">
                    <div class="col-md-2">
                        <div class="form-group">
                            <label for="usr">Month:</label><font color=red>*</font>
                            <div class="input-group date monthGl" style="padding:0;">
                                <asp:TextBox ID="tbDate" runat="server" CssClass ="form-control" placeholder="MM yyyy" OnTextChanged="cari" AutoPostBack="true"></asp:TextBox>
                                <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                            </div>
                        </div>
                        <div style="text-align:right;">
                            <asp:LinkButton ID="lbPrint" runat="server" CssClass="btn btn-default btn-block"><i class="fa fa-print"></i> Print</asp:LinkButton>
                        </div>
                    </div>
                    <div class="col-md-2" style="padding:0;">
                        <div class="form-group">
                            <label for="usr">Group:</label><font color=red>*</font>
                            <asp:DropDownList ID="dlGroup" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlGroup_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div style="text-align:right;">
                            <asp:LinkButton ID="lbExport" runat="server" CssClass="btn btn-default btn-block" Visible="false"><i class="fa fa-file-excel-o"></i> To Excel</asp:LinkButton>
                        </div>
                    </div>
                    <div class="col-md-8">
                        <div id ="Div1" class="col-lg-3 col-xs-6 padding-info">
                            <!-- small box -->
                            <div class="small-box" style="background-color:White; margin-bottom:10px; margin:0; border-top:3px solid #005662;">
                                <div class="small-box-header">
                                    <h3><asp:Label ID="lblInfo1" runat="server" Text="Asset"></asp:Label></h3>
                                </div>
                                <div class="inner">
                                    <table style="width:100%">
                                        <tr>
                                            <td style="width:35px;"><p>Rp. &nbsp;</p></td>
                                            <td style="text-align:right;"><p><asp:Label ID="Label1" runat="server" Text="0"></asp:Label></p></td>
                                        </tr>
                                    </table>
                                </div>
                                <%--<div class="icon" style="top:15px;">
                                    <i class="fa fa-money" style="font-size:50px;"></i>
                                </div>--%>
                            </div>
                        </div>
                        <div id ="Div2" class="col-lg-3 col-xs-6 padding-info">
                            <!-- small box -->
                            <div class="small-box" style="background-color:White; margin-bottom:10px; border-top:3px solid #005662;">
                                <div class="small-box-header">
                                    <h3><asp:Label ID="lblInfo2" runat="server" Text="Equity "></asp:Label></h3>
                                </div>
                                <div class="inner">
                                    <table style="width:100%">
                                        <tr>
                                            <td style="width:35px;"><p>Rp. &nbsp;</p></td>
                                            <td style="text-align:right;"><p><asp:Label ID="Label2" runat="server" Text="0"></asp:Label></p></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div id ="Div3" class="col-lg-3 col-xs-6 padding-info">
                            <!-- small box -->
                            <div class="small-box" style="background-color:White; margin-bottom:10px; border-top:3px solid #005662;">
                                <div class="small-box-header">
                                    <h3><asp:Label ID="lblInfo3" runat="server" Text="Liability"></asp:Label></h3>
                                </div>
                                <div class="inner">
                                    <table style="width:100%">
                                        <tr>
                                            <td style="width:35px;"><p>Rp. &nbsp;</p></td>
                                            <td style="text-align:right;"><p><asp:Label ID="Label3" runat="server" Text="0"></asp:Label></p></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div id ="Div4" class="col-lg-3 col-xs-6 padding-info">
                            <!-- small box -->
                            <div class="small-box" style="background-color:White; margin-bottom:10px; border-top:3px solid #005662;">
                                <div class="small-box-header">
                                    <h3><asp:Label ID="Label5" runat="server" Text="Profit & Loss"></asp:Label></h3>
                                </div>
                                <div class="inner">
                                    <table style="width:100%">
                                        <tr>
                                            <td style="width:35px;"><p>Rp. &nbsp;</p></td>
                                            <td style="text-align:right;"><p><asp:Label ID="Label6" runat="server" Text="0"></asp:Label></p></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            <%--</div>
        </div>
    </div>--%>
    <hr style="margin:0;" />
    <div class="row">
        <div class="col-lg-12">
            <p style="text-align:right;">
                <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBox1_CheckedChanged"/> Hide Zero Account
            </p>
            <ul class="nav nav-tabs">
                <li id="Asset-tab" class="hidden">      <a href="#Asset" data-toggle="tab"><i class="fa fa-list"></i> Asset</a></li>
                <li id="Equity-tab" class="hidden">     <a href="#Equity" data-toggle="tab"><i class="fa fa-list"></i> Equity</a></li>
                <li id="Liability-tab" class="hidden">  <a href="#Liability" data-toggle="tab"><i class="fa fa-list"></i> Liability</a></li>
                <li id="Revenue-tab" class="active">    <a href="#Revenue" data-toggle="tab"><i class="fa fa-list"></i> Revenue</a></li>
                <li id="Cost-tab">                      <a href="#Cost" data-toggle="tab"><i class="fa fa-list"></i> Cost</a></li>
                <li id="Expenses-tab">                  <a href="#Expenses" data-toggle="tab"><i class="fa fa-list"></i> Expenses</a></li>
                <li id="Statistic-tab" class="hidden">  <a href="#Statistic" data-toggle="tab"><i class="fa fa-list"></i> Statistic</a></li>
            </ul>
            <div id="myTabContent" class="tab-content">
                <div class="tab-pane active in" id="Asset">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvGLAsset" runat="server" AutoGenerateColumns="false" 
                        CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" 
                        GridLines="None" OnRowDataBound="OnRowDataBoundAsset">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="100px" DataField="Coa" HeaderText="COA" />
                            <asp:BoundField DataField="curentAmount" HeaderText="Act" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"/>
                            <asp:BoundField DataField="lastAmount" HeaderText="Act" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"/>
                            <asp:BoundField DataField="Description" HeaderText="Description" />
                            <asp:TemplateField HeaderText="Curent Month" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurentAmountAsset" runat="server" Text='<%# if(Eval("curentAmount").toString()="","0.00",Eval("curentAmount","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Month" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblLastAmountAsset" runat="server" Text='<%# if(Eval("lastAmount").toString()="","0.00",Eval("lastAmount","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Net this Month" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" ItemStyle-CssClass="cellOneCellPaddingRight">
                                <ItemTemplate>
                                    <asp:Label ID="lblNetAmountAsset" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailAsset" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("Coa") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="Liability">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvGLLiability" runat="server" AutoGenerateColumns="false" 
                        CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" 
                        GridLines="None" OnRowDataBound="OnRowDataBoundLiability" >
                        <Columns>
                            <asp:BoundField ItemStyle-Width="100px" DataField="Coa" HeaderText="COA" />
                            <asp:BoundField DataField="curentAmount" HeaderText="Act" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"/>
                            <asp:BoundField DataField="lastAmount" HeaderText="Act" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"/>
                            <asp:BoundField DataField="Description" HeaderText="Description" />
                            <asp:TemplateField HeaderText="Curent Month" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurentAmountLiability" runat="server" Text='<%# if(Eval("curentAmount").toString()="","0.00",Eval("curentAmount","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Month" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblLastAmountLiability" runat="server" Text='<%# if(Eval("lastAmount").toString()="","0.00",Eval("lastAmount","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Net this Month" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" ItemStyle-CssClass="cellOneCellPaddingRight">
                                <ItemTemplate>
                                    <asp:Label ID="lblNetAmountLiability" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailLiability" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("Coa") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField ItemStyle-Width="50px" HeaderText="Detail" HeaderStyle-HorizontalAlign ="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailDept" CssClass="btn btn-default" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("Coa")%>'><i class="fa fa-list"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="Equity">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvGLEquity" runat="server" AutoGenerateColumns="false" 
                        CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" 
                        GridLines="None" OnRowDataBound="OnRowDataBoundEquity" >
                        <Columns>
                            <asp:BoundField ItemStyle-Width="100px" DataField="Coa" HeaderText="COA" />
                            <asp:BoundField DataField="curentAmount" HeaderText="Act" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"/>
                            <asp:BoundField DataField="lastAmount" HeaderText="Act" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"/>
                            <asp:BoundField DataField="Description" HeaderText="Description" />
                            <asp:TemplateField HeaderText="Curent Month" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurentAmountEquity" runat="server" Text='<%# if(Eval("curentAmount").toString()="","0.00",Eval("curentAmount","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Month" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblLastAmountEquity" runat="server" Text='<%# if(Eval("lastAmount").toString()="","0.00",Eval("lastAmount","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Net this Month" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" ItemStyle-CssClass="cellOneCellPaddingRight">
                                <ItemTemplate>
                                    <asp:Label ID="lblNetAmountEquity" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailEquity" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("Coa") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField ItemStyle-Width="50px" HeaderText="Detail" HeaderStyle-HorizontalAlign ="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailDept" CssClass="btn btn-default" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("Coa")%>'><i class="fa fa-list"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="Revenue">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvGLRevenue" runat="server" AutoGenerateColumns="false" 
                        CssClass="table header-border" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" 
                        GridLines="None">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="100px" DataField="Coa" HeaderText="COA"/>
                            <asp:BoundField DataField="Description" HeaderText="Description"/>
                            <asp:TemplateField HeaderText="Month to Date Actual" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountRevenueActMtd" runat="server" Text='<%# if(Eval("amountMtd").toString()="","0.00",Eval("amountMtd","{0:N2}")) %>'></asp:Label>
                                    <asp:LinkButton ID="lbDetailRevenue" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("Coa") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Month to Date Budget" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountRevenueBudMtd" runat="server" Text='<%# if(Eval("budgetMtd").toString()="","0.00",Eval("budgetMtd","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Year to Date Actual" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountRevenueActYtd" runat="server" Text='<%# if(Eval("amountYtd").toString()="","0.00",Eval("amountYtd","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Year to Date Budget" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountRevenueBudYtd" runat="server" Text='<%# if(Eval("budgetYtd").toString()="","0.00",Eval("budgetYtd","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft">
                                <ItemTemplate>
                                    
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <%--<asp:TemplateField ItemStyle-Width="50px" HeaderText="Detail" HeaderStyle-HorizontalAlign ="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailDept" CssClass="btn btn-default" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("Coa")%>'><i class="fa fa-list"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="Cost">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvGLCost" runat="server" AutoGenerateColumns="false" 
                        CssClass="table header-border" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" 
                        GridLines="None">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="100px" DataField="Coa" HeaderText="COA"/>
                            <asp:BoundField DataField="Description" HeaderText="Description"/>
                            <asp:TemplateField HeaderText="Month to Date Actual" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountCostActMtd" runat="server" Text='<%# if(Eval("amountMtd").toString()="","0.00",Eval("amountMtd","{0:N2}")) %>'></asp:Label>
                                    <asp:LinkButton ID="lbDetailCost" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("Coa") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Month to Date Budget" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountCostBudMtd" runat="server" Text='<%# if(Eval("budgetMtd").toString()="","0.00",Eval("budgetMtd","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Year to Date Actual" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountCostActYtd" runat="server" Text='<%# if(Eval("amountYtd").toString()="","0.00",Eval("amountYtd","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Year to Date Budget" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountCostBudYtd" runat="server" Text='<%# if(Eval("budgetYtd").toString()="","0.00",Eval("budgetYtd","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField ItemStyle-Width="50px" HeaderText="Detail" HeaderStyle-HorizontalAlign ="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailDept" CssClass="btn btn-default" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("Coa")%>'><i class="fa fa-list"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="Expenses">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvGLExpenses" runat="server" AutoGenerateColumns="false" 
                        CssClass="table header-border" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" 
                        GridLines="None">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="100px" DataField="Coa" HeaderText="COA"/>
                            <asp:BoundField DataField="Description" HeaderText="Description"/>
                            <asp:TemplateField HeaderText="Month to Date Actual" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountExpensesActMtd" runat="server" Text='<%# if(Eval("amountMtd").toString()="","0.00",Eval("amountMtd","{0:N2}")) %>'></asp:Label>
                                    <asp:LinkButton ID="lbDetailExpenses" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("Coa") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Month to Date Budget" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountExpensesBudMtd" runat="server" Text='<%# if(Eval("budgetMtd").toString()="","0.00",Eval("budgetMtd","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Year to Date Actual" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountExpensesActYtd" runat="server" Text='<%# if(Eval("amountYtd").toString()="","0.00",Eval("amountYtd","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Year to Date Budget" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountExpensesBudYtd" runat="server" Text='<%# if(Eval("budgetYtd").toString()="","0.00",Eval("budgetYtd","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField ItemStyle-Width="50px" HeaderText="Detail" HeaderStyle-HorizontalAlign ="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailDept" CssClass="btn btn-default" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("Coa")%>'><i class="fa fa-list"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="Statistic">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvGLStatistic" runat="server" AutoGenerateColumns="false" 
                        CssClass="table header-border" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" 
                        GridLines="None">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="100px" DataField="Coa" HeaderText="COA"/>
                            <asp:BoundField DataField="Description" HeaderText="Description"/>
                            <asp:TemplateField HeaderText="Month to Date Actual" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountStatisticActMtd" runat="server" Text='<%# if(Eval("amountMtd").toString()="","0.00",Eval("amountMtd","{0:N2}")) %>'></asp:Label>
                                    <asp:LinkButton ID="lbDetailStatistic" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("Coa") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Month to Date Budget" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountStatisticBudMtd" runat="server" Text='<%# if(Eval("budgetMtd").toString()="","0.00",Eval("budgetMtd","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Year to Date Actual" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountStatisticActYtd" runat="server" Text='<%# if(Eval("amountYtd").toString()="","0.00",Eval("amountYtd","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Year to Date Budget" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountStatisticBudYtd" runat="server" Text='<%# if(Eval("budgetYtd").toString()="","0.00",Eval("budgetYtd","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField ItemStyle-Width="50px" HeaderText="Detail" HeaderStyle-HorizontalAlign ="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailDept" CssClass="btn btn-default" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("Coa")%>'><i class="fa fa-list"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="col-lg-12">
            
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

