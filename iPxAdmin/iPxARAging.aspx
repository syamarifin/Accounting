<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxARAging.aspx.vb" Inherits="iPxAdmin_iPxARAging" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .cellOneCellPaddingLeft {
            padding-left: 0pt !important;
        }
        .cellOneCellPaddingRight {
            padding-right: 0pt !important;
        }
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
        .GVFixedFooter { position:relative;}
    </style>
    <script>
        function CLActive()
        {
            $("#CC-tab").addClass("active");
            $("#CL-tab").removeClass("active");
            $("#CC").addClass("active in");
            $("#CL").removeClass("active in");
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Detail Journal Modal-->
    <div id="formInput" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md" style="width:1200px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortDetail" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="login-modalLabel" class="modal-title">
                        <asp:Label ID="lbTitleDetailCoa" runat="server" Text="Detail Aging "></asp:Label>
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvDetail" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" ShowFooter="true">
                                <Columns>
                                    <asp:BoundField DataField="aging" HeaderText="Aging" />   
                                    <asp:BoundField DataField="TransDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}"/> 
                                    <asp:BoundField DataField="TransID" HeaderText="Trans ID" />   
                                    <asp:BoundField DataField="transactiontype" HeaderText="Type" />   
                                    <asp:BoundField DataField="invoiceno" HeaderText="Invoice No" />   
                                    <asp:BoundField DataField="RoomNo" HeaderText="Room No" />   
                                    <asp:BoundField DataField="GuestName" HeaderText="Guest Name" />   
                                    <asp:TemplateField HeaderText="Debit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurDeb" runat="server" Text='<%# if(Eval("amountdr").toString()="","0.00",Eval("amountdr","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Credit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurCre" runat="server" Text='<%# if(Eval("amountcr").toString()="","0.00",Eval("amountcr","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:LinkButton ID="lbPrintDtl" runat="server" CssClass="btn btn-default"><i class="fa fa-print"></i> Print</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Detail Journal modal end-->
    <div class="row">
        <div class="col-lg-12">
            <ul class="nav nav-tabs">
                <li id="CL-tab" class="active"> <a href="#CL" data-toggle="tab"><i class="fa fa-list"></i> Travel Agent/Company</a></li>
                <li id="CC-tab">                <a href="#CC" data-toggle="tab"><i class="fa fa-list"></i> Credit Card</a></li>
            </ul>
            <div id="myTabContent" class="tab-content">
                <div class="tab-pane" id="CC">
                    <br />
                    <div class="row">
                        <div class="col-lg-1">
                            <label for="usr">For Date:</label>
                        </div>
                        <div class="col-lg-3" style="padding-right:5px;">
                            <div class="form-group" style="margin-bottom:3px;">
                                <div class="input-group date datepicker1" style="padding:0;">
                                    <asp:TextBox ID="tbDate" runat="server" CssClass="form-control" Enabled="false" OnTextChanged="cari" AutoPostBack="true"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4" style="padding-left:5px;">
                            <asp:LinkButton ID="lbPrint" runat="server" CssClass="btn btn-default"><i class="fa fa-print"></i> Print</asp:LinkButton>
                        </div>
                        <div class="col-lg-12">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvAging" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" 
                            HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10" ShowFooter="true"
                             FooterStyle-BackColor="#0a818e" FooterStyle-ForeColor="White" OnRowDataBound="OnRowDataBound" >
                                <Columns>
                                    <asp:BoundField DataField="CoyName" HeaderText="Customer" />
                                    <asp:TemplateField HeaderText="Balance" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="130px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBalance" runat="server" Text='<%# if(Eval("balance").toString()="","0.00",Eval("balance","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="&le;30 Days" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-center" ItemStyle-Width="130px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBulan1" runat="server" Text='<%# if(Eval("bulan1").toString()="","0.00",Eval("bulan1","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="&le;60 Days" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-center" ItemStyle-Width="130px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBulan2" runat="server" Text='<%# if(Eval("bulan2").toString()="","0.00",Eval("bulan2","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="&le;90 Days" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-center" ItemStyle-Width="130px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBulan3" runat="server" Text='<%# if(Eval("bulan3").toString()="","0.00",Eval("bulan3","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="&le;120 Days" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-center" ItemStyle-Width="130px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBulan4" runat="server" Text='<%# if(Eval("bulan4").toString()="","0.00",Eval("bulan4","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=">120 Days" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-center" ItemStyle-Width="130px" ItemStyle-CssClass="cellOneCellPaddingRight">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLbh" runat="server" Text='<%# if(Eval("lbh").toString()="","0.00",Eval("lbh","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbDetailClear" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("CustomerID") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <pagerstyle cssclass="pagination-ys">
                                </pagerstyle>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="tab-pane active in" id="CL">
                    <br />
                    <div class="row">
                        <div class="col-lg-1">
                            <label for="usr">For Date:</label>
                        </div>
                        <div class="col-lg-3" style="padding-right:5px;">
                            <div class="form-group" style="margin-bottom:3px;">
                                <div class="input-group date datepicker1" style="padding:0;">
                                    <asp:TextBox ID="tbDateCL" runat="server" CssClass="form-control" Enabled="false" OnTextChanged="cariCL" AutoPostBack="true"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4" style="padding-left:5px;">
                            <asp:LinkButton ID="lbPrintCL" runat="server" CssClass="btn btn-default"><i class="fa fa-print"></i> Print</asp:LinkButton>
                        </div>
                        <div class="col-lg-12">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvAgingCL" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" 
                            HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10" ShowFooter="true"
                             FooterStyle-BackColor="#0a818e" FooterStyle-ForeColor="White" OnRowDataBound="OnRowDataBound" >
                                <Columns>
                                    <asp:BoundField DataField="CoyName" HeaderText="Customer" />
                                    <asp:TemplateField HeaderText="Balance" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="130px" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblBalance" runat="server" Text='<%# if(Eval("balance").toString()="","0.00",Eval("balance","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="&le;30 Days" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-center" ItemStyle-Width="130px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBulan1" runat="server" Text='<%# if(Eval("bulan1").toString()="","0.00",Eval("bulan1","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="&le;60 Days" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-center" ItemStyle-Width="130px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBulan2" runat="server" Text='<%# if(Eval("bulan2").toString()="","0.00",Eval("bulan2","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="&le;90 Days" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-center" ItemStyle-Width="130px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBulan3" runat="server" Text='<%# if(Eval("bulan3").toString()="","0.00",Eval("bulan3","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="&le;120 Days" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-center" ItemStyle-Width="130px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBulan4" runat="server" Text='<%# if(Eval("bulan4").toString()="","0.00",Eval("bulan4","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=">120 Days" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-center" ItemStyle-Width="130px" ItemStyle-CssClass="cellOneCellPaddingRight">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLbh" runat="server" Text='<%# if(Eval("lbh").toString()="","0.00",Eval("lbh","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbDetailClear" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("CustomerID") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
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
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

