<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxSalesSummary.aspx.vb" Inherits="iPxAdmin_iPxSalesSummary" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .header-center{
            text-align:center;
        }
        .header-right{
            text-align:right;
        }
        .SubTotalRowStyle{
            background-color:#81BEF7;
            font-weight:bold;
        }
        .GrandTotalRowStyle{
            background-color:#0a818e;
            font-weight:bold;
            color:White;
        }
        .GVFixedFooter { position:relative;}
        .textBlack  
        {
        	color:Black;
        }
        .textRed  
        {
        	color:Red;
        }
    </style>
    <script type="text/javascript">
        function MonthPost() {
            $(".monthGl").datepicker({ format: 'MM yyyy', viewMode: "months", minViewMode: "months", autoclose: true, todayBtn: 'linked' })
        }
        function hideModalDatePost() {
            $('#formDatePost').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalDatePost() {
            $('#formDatePost').modal({ backdrop: 'static',
                keyboard: false
            }, 'show');
        }
        function hideModalDetail() {
            $('#formDetail').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalDetail() {
            $('#formDetail').modal({ backdrop: 'static',
                keyboard: false
            }, 'show');
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Option Modal-->
    <div id="formDatePost" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortDatePost" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H2" class="modal-title">Date Posting </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">For Month:</label>
                                <div class="input-group date monthGl" style="padding:0;">
                                    <asp:TextBox ID="tbDatePost" runat="server" CssClass="form-control" Enabled="false" OnTextChanged="cariPost" AutoPostBack="true"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                                </div>
                            </div>
                            <asp:GridView EmptyDataText="No records has been added." ID="gvDatePost" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                                <Columns>
                                    <asp:BoundField DataField="numb" HeaderText="Date" ItemStyle-Width="100px"/>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesc" runat="server" Text='<%# if(Eval("dataPost").toString()>"0",if(Eval("Post").toString()="","OPEN","POSTED"),"NO DATA") %>' CssClass='<%# if(Eval("Post").toString()="","textBlack","textRed") %>' ></asp:Label>
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
    <!-- Option modal end-->
    <!-- Detail Modal-->
    <div id="formDetail" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-lg" style="width:1200px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortDetail" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H1" class="modal-title">Detail Sales </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-3">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Hotel Name:</label>
                                <asp:DropDownList ID="dlFOLinkDetail" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlFOLinkDetail_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-3">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">For Month:</label>
                                <div class="input-group date datepicker1" style="padding:0;">
                                    <asp:TextBox ID="tbDateDetail" runat="server" CssClass="form-control" Enabled="false" OnTextChanged="cariDetail" AutoPostBack="true"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Departement:</label>
                                <asp:DropDownList ID="dlGroup" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlGroup_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-3" style="padding-left:5px;padding-top:5px">
                            <div class="form-group" style="margin-bottom:3px;">
                                <br/>
                                <asp:LinkButton Width="100px" ID="lbPrintDtl" runat="server" CssClass="btn btn-default"><i class="fa fa-print"></i> Print</asp:LinkButton>
                            </div>
                        </div>
                        <div class="col-lg-12">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvSalesDetail" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10" ShowFooter="true">
                                <Columns>
                                    <asp:BoundField DataField="poscode" HeaderText="Poscode" ItemStyle-Width="60px"/>
                                    <asp:BoundField DataField="checkno" HeaderText="Checkno" ItemStyle-Width="100px"/>
                                    <asp:BoundField DataField="roomno" HeaderText="Room No" ItemStyle-Width="50px"/>
                                    <asp:BoundField DataField="GuestName" HeaderText="Guest Name"/>
                                    <asp:BoundField DataField="shift" HeaderText="Shift" ItemStyle-Width="50px"/>
                                    <asp:BoundField DataField="paymenttype" HeaderText="Payment Type" ItemStyle-Width="80px"/>
                                    <asp:BoundField DataField="cardtype" HeaderText="Card Type" ItemStyle-Width="80px"/>
                                    <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("amount","{0:N2}").toString() %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discount (%)" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("discount","{0:N1}").toString() %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Revenue" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("revenue","{0:N2}").toString() %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("tax","{0:N2}").toString() %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Service" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("service","{0:N2}").toString() %>' ></asp:Label>
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
    <!-- Detail modal end-->
    <div class="col-md-3" style="padding:0;">
        <div class="form-group" style="margin-bottom:3px;">
            <label for="usr">Hotel Name:</label>
            <asp:DropDownList ID="dlFOLink" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlFOLink_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-md-3" style="padding-right:5px;">
        <div class="form-group" style="margin-bottom:3px;">
            <label for="usr">For Date:</label>
            <div class="input-group date datepicker1" style="padding:0;">
                <asp:TextBox ID="tbDate" runat="server" CssClass="form-control" Enabled="false" OnTextChanged="cari" AutoPostBack="true"></asp:TextBox>
                <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
            </div>
        </div>
    </div>
    <div class="col-md-6" style="padding-left:5px;padding-top:5px">
        <br />
        <asp:LinkButton Width="100px" ID="lbPrint" runat="server" CssClass="btn btn-default"><i class="fa fa-print"></i> Print</asp:LinkButton>
        <asp:LinkButton Width="100px" ID="lbPost" runat="server"  CssClass="btn btn-default" OnClientClick="return confirmationPost();"><i class="fa fa-send"></i> Posting</asp:LinkButton>
        <div class="btn-group">
            <button type="button" style="width:130px;" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
            <i class="fa fa-cogs"></i> Option <span class="caret"></span></button>
            <ul class="dropdown-menu" role="menu">
                <li><asp:LinkButton ID="lbOption" runat="server" CssClass="btn" style="text-align:left;">Date Posting</asp:LinkButton></li>
                <li><asp:LinkButton ID="lbDetail" runat="server" CssClass="btn" style="text-align:left;">Detail Transaction</asp:LinkButton></li>
            </ul>
        </div>
    </div>
    <div class="col-lg-12">
         <%--<ul class="nav nav-tabs">
                <li id="Grafik-tab" class="active">      <a href="#Grafik" data-toggle="tab"><i class="fa fa-list"></i> Grafik</a></li>
                <li id="DataSumary-tab">                 <a href="#DataSumary" data-toggle="tab"><i class="fa fa-list"></i> Sales Summary</a></li>
            </ul>
            <div id="myTabContent" class="tab-content">
                <div class="tab-pane active in" id="Grafik">
                    <div class="row">
                        <div class="col-lg-6">
                            <canvas id="myChartRev" height="200"></canvas>
                        </div>
                        <div class="col-lg-6">
                            <canvas id="myChartSet" height="200"></canvas>
                        </div>
                    </div>
                </div>
                <div class="tab-pane" id="DataSumary">--%>
                    <asp:GridView EmptyDataText="No records has been added." ID="gvSummary" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered" HeaderStyle-BackColor="#0a818e" 
                    HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" FooterStyle-CssClass="GVFixedFooter"
                    OnRowDataBound="OnRowDataBound" OnRowCreated="gvSummary_RowCreated" OnDataBound = "OnDataBound" >
                        <Columns>
                            <asp:TemplateField HeaderText="Actual" HeaderStyle-CssClass="hidden" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("poscode")+"-"+Eval("description") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ActMtd" HeaderText="Act" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"/>
                            <asp:BoundField DataField="BudgetMtd" HeaderText="Act" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"/>
                            <asp:BoundField DataField="amountYtd" HeaderText="Act" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"/>
                            <asp:BoundField DataField="BudgetYtd" HeaderText="Act" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"/>
                            <asp:TemplateField HeaderText="Actual" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblOpen" runat="server" Text='<%# if(Eval("ActTd").toString()="","0.00",Eval("ActTd","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Actual" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblOpen" runat="server" Text='<%# if(Eval("ActMtd").toString()="","0.00",Eval("ActMtd","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Budget" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblDebit" runat="server" Text='<%# if(Eval("BudgetMtd").toString()="","0.00",Eval("BudgetMtd","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Act VS Bdg" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:Label ID="lbActVsBdgMtd" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Actual" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblOpen" runat="server" Text='<%# if(Eval("amountYtd").toString()="","0.00",Eval("amountYtd","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Budget" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblDebit" runat="server" Text='<%# if(Eval("BudgetYtd").toString()="","0.00",Eval("BudgetYtd","{0:N2}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Act VS Bdg" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:Label ID="lbActVsBdgYtd" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                <%--</div>
            </div>--%>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

