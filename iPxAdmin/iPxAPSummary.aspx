<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxAPSummary.aspx.vb" Inherits="iPxAdmin_iPxAPSummary" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .cellOneCellPaddingLeft {
            padding-left: 0pt !important;
        }
        .cellOneCellPaddingRight {
            padding-right: 0pt !important;
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
                        <asp:Label ID="lbTitleDetailCoa" runat="server" Text="Detail Summary "></asp:Label>
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvDetail" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" ShowFooter="true">
                                <Columns>
                                    <asp:BoundField DataField="TransDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}"/> 
                                    <asp:BoundField DataField="TransID" HeaderText="Trans ID" />   
                                    <asp:BoundField DataField="transactiontype" HeaderText="Type" />   
                                    <asp:BoundField DataField="PVno" HeaderText="Voucher No" />   
                                    <asp:BoundField DataField="PONo" HeaderText="PO No" />   
                                    <asp:BoundField DataField="RRNo" HeaderText="RR No" />   
                                    <asp:TemplateField HeaderText="Debit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurDeb" runat="server" Text='<%# if(Eval("Debit").toString()="","0.00",Eval("Debit","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Credit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurCre" runat="server" Text='<%# if(Eval("Credit").toString()="","0.00",Eval("Credit","{0:N2}")) %>'></asp:Label>
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
            <asp:GridView EmptyDataText="No records has been added." ID="gvSummary" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" 
            HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" FooterStyle-CssClass="GVFixedFooter"
            AllowPaging="true" PageSize="10" OnRowDataBound="OnRowDataBound" ShowFooter="true" FooterStyle-BackColor="#0a818e" FooterStyle-ForeColor="White" >
                <Columns>
                    <asp:BoundField DataField="CoyName" HeaderText="Vendor" ItemStyle-CssClass=""/>
                    <asp:BoundField DataField="open1" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" HeaderText="Open"/>
                    <asp:BoundField DataField="debit" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" HeaderText="Debit"/>
                    <asp:BoundField DataField="credit" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" HeaderText="Credit"/>
                    <asp:TemplateField HeaderText="Open" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="130px">
                        <ItemTemplate>
                            <asp:Label ID="lblOpen" runat="server" Text='<%# if(Eval("open1").toString()="","0.00",Eval("open1","{0:N2}")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Debit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="130px">
                        <ItemTemplate>
                            <asp:Label ID="lblDebit" runat="server" Text='<%# if(Eval("debit").toString()="","0.00",Eval("debit","{0:N2}")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Credit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="130px">
                        <ItemTemplate>
                            <asp:Label ID="lblCredit" runat="server" Text='<%# if(Eval("credit").toString()="","0.00",Eval("credit","{0:N2}")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Balance" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="130px" ItemStyle-CssClass="cellOneCellPaddingRight">
                        <ItemTemplate>
                            <asp:Label ID="lbBalance" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbDetailClear" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("VendorID") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <%--<ul class="nav nav-tabs">
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
                            <asp:GridView EmptyDataText="No records has been added." ID="gvSummary" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" 
                                HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" FooterStyle-CssClass="GVFixedFooter"
                                AllowPaging="true" PageSize="10" OnRowDataBound="OnRowDataBound" ShowFooter="true" FooterStyle-BackColor="#0a818e" FooterStyle-ForeColor="White" >
                                <Columns>
                                    <asp:BoundField DataField="CoyName" HeaderText="Customer" ItemStyle-CssClass=""/>
                                    <asp:BoundField DataField="open1" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" HeaderText="Open"/>
                                    <asp:BoundField DataField="debit" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" HeaderText="Debit"/>
                                    <asp:BoundField DataField="credit" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" HeaderText="Credit"/>
                                    <asp:TemplateField HeaderText="Open" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="130px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOpen" runat="server" Text='<%# if(Eval("open1").toString()="","0.00",Eval("open1","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Debit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="130px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDebit" runat="server" Text='<%# if(Eval("debit").toString()="","0.00",Eval("debit","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Credit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="130px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCredit" runat="server" Text='<%# if(Eval("credit").toString()="","0.00",Eval("credit","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Balance" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="130px" ItemStyle-CssClass="cellOneCellPaddingRight">
                                        <ItemTemplate>
                                            <asp:Label ID="lbBalance" runat="server" Text=""></asp:Label>
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
                            <asp:GridView EmptyDataText="No records has been added." ID="gvSummaryCL" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" 
                                HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" FooterStyle-CssClass="GVFixedFooter"
                                AllowPaging="true" PageSize="10" OnRowDataBound="OnRowDataBound" ShowFooter="true" FooterStyle-BackColor="#0a818e" FooterStyle-ForeColor="White">
                                <Columns>
                                    <asp:BoundField DataField="CoyName" HeaderText="Customer" ItemStyle-CssClass=""/>
                                    <asp:BoundField DataField="open1" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" HeaderText="Open"/>
                                    <asp:BoundField DataField="debit" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" HeaderText="Debit"/>
                                    <asp:BoundField DataField="credit" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" HeaderText="Credit"/>
                                    <asp:TemplateField HeaderText="Open" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="130px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOpen" runat="server" Text='<%# if(Eval("open1").toString()="","0.00",Eval("open1","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Debit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="130px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDebit" runat="server" Text='<%# if(Eval("debit").toString()="","0.00",Eval("debit","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Credit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="130px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCredit" runat="server" Text='<%# if(Eval("credit").toString()="","0.00",Eval("credit","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Balance" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="130px" ItemStyle-CssClass="cellOneCellPaddingRight">
                                        <ItemTemplate>
                                            <asp:Label ID="lbBalance" runat="server" Text=""></asp:Label>
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
            </div>--%>
        </div>
    </div>
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

