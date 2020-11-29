<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxAPPayment.aspx.vb" Inherits="iPxAdmin_iPxAPPayment" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .btn_right
        {
        	text-align:right;
        }
    </style>
    <script type="text/javascript">
        function MonthGL() {
            $(".monthGl").datepicker({ format: 'mm-yyyy', viewMode: "months", minViewMode: "months", autoclose: true, todayBtn: 'linked' })
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--List Docket Modal--%>
    <div id="formDocket" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortDocket" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H4" class="modal-title">Please select docket template first!.</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:GridView ID="gvDocket" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None">
                                <Columns>
                                    <asp:BoundField ItemStyle-Width ="120px" DataField="businessid" HeaderText="Businessid" />
                                    <asp:BoundField ItemStyle-Width ="50px" DataField="code" HeaderText="Code" />
                                    <asp:BoundField ItemStyle-Width ="70px" DataField="Description" HeaderText="Description" />
                                    <asp:BoundField ItemStyle-Width ="90px" DataField="fileName" HeaderText="File Name" />
                                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Send" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbPrint" CssClass="btn btn-default" runat="server" CommandName="getDocket" CommandArgument='<%# Eval("fileName") %>' ><i class="fa fa-print"></i></asp:LinkButton>
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
    <%--End List Docket Modal--%>
    <%--Resion Delete AR Receipt Modal--%>
    <div id="formDeleteTrans" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortDelete" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H2" class="modal-title">Do you want to delete Payment ?</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="usr">Reason to delete AP Payment :</label><font color=red>*</font>
                        <asp:TextBox ID="tbReason" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbDelete" runat="server" CssClass="btn btn-default"><i class="fa fa-trash"></i> Delete</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <%--End Resion Delete AR Receipt Modal--%>
    <!-- Add Customer Group Modal-->
    <div id="formInput" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <button type="button" data-dismiss="modal" aria-label="Close" class="close"><span aria-hidden="true">×</span></button>
                    <h4 id="login-modalLabel" class="modal-title"><asp:Label ID="lbTitle" runat="server" Text="Saldo Balance"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:Panel ID="pnListDep" runat="server">
                                <div style="max-height:400px;overflow:auto;">
                                <asp:GridView EmptyDataText="No records has been added." ID="gvARInv" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" ShowFooter="true">
                                    <Columns>
                                        <asp:BoundField ItemStyle-Width="100px" DataField="PVNo" HeaderText="Voucher No" />
                                        <asp:BoundField ItemStyle-Width="85px" DataField="PVDate" HeaderText="Voucher Date" DataFormatString="{0:dd MMM yyyy}"/>
                                        <asp:BoundField ItemStyle-Width="85px" DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd MMM yyyy}"/>
                                        <asp:BoundField ItemStyle-Width="100px" DataField="CoyName" HeaderText="Customer Name" />
                                        <asp:BoundField ItemStyle-Width="100px" DataField="ReffNo" HeaderText="Reff No" />
                                        <asp:BoundField DataField="Notes" HeaderText="Notes"/>
                                        <asp:BoundField ItemStyle-Width="85px" DataField="RegDate" HeaderText="Reg Date" DataFormatString="{0:dd MMM yyyy}"/>
                                        <asp:BoundField ItemStyle-Width="100px" DataField="Debit" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderText="Debit" />
                                        <asp:TemplateField ItemStyle-Width="100px" HeaderText="Credit" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# if(Eval("Credit").toString()="",string.Format("{0:N2}",0),string.Format("{0:N2}",Eval("Credit"))) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="100px" DataField="totalInvoice" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderText="Balances" />
                                    </Columns>
                                    <pagerstyle cssclass="pagination-ys">
                                    </pagerstyle>
                                </asp:GridView>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnListDetail" runat="server">
                                <div style="max-height:400px;overflow:auto;">
                                <asp:GridView EmptyDataText="No records has been added." ID="gvDetail" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" ShowFooter="true">
                                    <Columns>
                                        <asp:BoundField ItemStyle-Width="100px" DataField="TransID" HeaderText="Transaction No" />
                                        <asp:BoundField ItemStyle-Width="85px" DataField="TransDate" HeaderText="Transaction Date" DataFormatString="{0:dd MMM yyyy}"/>
                                        <asp:BoundField DataField="CoyName" HeaderText="Customer Name" />
                                        <asp:BoundField ItemStyle-Width="100px" DataField="invoiceno" HeaderText="Invoice No" />
                                        <asp:BoundField ItemStyle-Width="100px" DataField="amountcr" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderText="Amount" />
                                    </Columns>
                                    <pagerstyle cssclass="pagination-ys">
                                    </pagerstyle>
                                </asp:GridView>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                
            </div>
        </div>
    </div>
    <!-- Add Customer Group modal end-->
    <!-- Query Receipt Modal-->
    <div id="formQuery" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm" style="width:400px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortQuery" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H1" class="modal-title">Query Payment</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:Panel ID="pnRecept" runat="server">
                                <div class="form-group">
                                    <label for="usr">Payment ID:</label>
                                    <asp:TextBox ID="tbQRecID" runat="server" CssClass ="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group" style="margin-bottom:3px;">
                                    <label for="usr">Payment Date:</label>
                                    <div class="input-group date datepicker" style="padding:0;">
                                        <asp:TextBox ID="tbQRecDate" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="usr">Vendor Name:</label>
                                    <asp:TextBox ID="tbQCus" runat="server" CssClass ="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="usr">Reff No:</label>
                                    <asp:TextBox ID="tbQReff" runat="server" CssClass ="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="usr">Paid by:</label>
                                    <asp:DropDownList ID="dlPaid" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group" style="margin-bottom:3px;">
                                    <label for="usr">Status:</label>
                                    <asp:DropDownList ID="dlQStatus" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div style="text-align:center;">
                                    <asp:Label ID="Label1" runat="server" Text="By Periode"></asp:Label>
                                </div>
                                <div class="form-group" style="margin-bottom:3px;">
                                    <label for="usr">Periode From:</label>
                                    <div class="input-group date datepicker" style="padding:0;">
                                        <asp:TextBox ID="tbQFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                                    </div>
                                </div>
                                <div class="form-group" style="margin-bottom:3px;">
                                    <label for="usr">Periode Until:</label>
                                    <div class="input-group date datepicker" style="padding:0;">
                                        <asp:TextBox ID="tbQUntil" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnCust" runat="server">
                                <div class="form-group">
                                    <label for="usr">Coy Name:</label>
                                    <asp:TextBox ID="tbQCoyName" runat="server" CssClass ="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="usr">Country:</label>
                                    <asp:DropDownList ID="dlQCountry" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlQCountry_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label for="usr">Province:</label>
                                    <asp:DropDownList ID="dlQProvince" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlQProvince_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label for="usr">City:</label>
                                    <asp:DropDownList ID="dlQCity" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label for="usr">Email:</label>
                                    <asp:TextBox ID="tbQEmail" runat="server" CssClass ="form-control"></asp:TextBox>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lblQuery" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Query Receipt modal end-->
    <div class="row">
        <div class="col-lg-8">
            <asp:LinkButton ID="lbAddRec" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> New Payment</asp:LinkButton>
            <asp:LinkButton ID="lbView" Width="150px" runat="server" CssClass="btn btn-default" Visible="false"><i class="fa fa-file-text-o"></i> View Payment</asp:LinkButton>
            <asp:LinkButton ID="lbQuery" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
            <%--<asp:LinkButton ID="lbPrint" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-print"></i> Print</asp:LinkButton>--%><hr style="margin-top:5px; margin-bottom:5px;" />
        </div>
        <div class="col-lg-2" style="text-align:right; padding-right:0;">
        <br style="line-height: 7px;"/>
        For Periode :
        </div>
        <div class="col-lg-2">
            <div class="form-group" style="margin-bottom:3px;">
                <div class="input-group date monthGl" style="padding:0;">
                    <asp:TextBox ID="tbDate" runat="server" CssClass="form-control" Enabled="false" OnTextChanged="TransdateWork" AutoPostBack="true"></asp:TextBox>
                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                </div>
            </div>
        </div>
        <div class="col-lg-12">
            <asp:Panel ID="pnARRec" runat="server">
            <asp:GridView EmptyDataText="No records has been added." ID="gvARRec" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10" ShowFooter="true">
                <Columns>
                    <asp:BoundField ItemStyle-Width="100px" DataField="PaymentID" HeaderText="Payment No" />
                    <asp:BoundField ItemStyle-Width="100px" DataField="PaymentDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}"/>
                    <asp:BoundField ItemStyle-Width="100px" DataField="CoyName" HeaderText="Vendor Name" />
                    <asp:BoundField ItemStyle-Width="100px" DataField="ReffNo" HeaderText="Reff No" />
                    <asp:BoundField ItemStyle-Width="100px" DataField="Description" HeaderText="Paid by"/>
                    <asp:BoundField ItemStyle-Width="100px" DataField="Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderText="Amount"/>
                    <asp:BoundField DataField="Notes" HeaderText="Notes"/>
                    <asp:BoundField ItemStyle-Width="100px" DataField="fullname" HeaderText="by" />
                    <asp:TemplateField ItemStyle-Width="40px" HeaderText="Saldo Balance" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbDep" CssClass="btn btn-default" runat="server" CommandName="getDeposito" CommandArgument='<%# Eval("PaymentID") %>'><i class="fa fa-file-text"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="40px" HeaderText="Post" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbPost" CssClass="btn btn-default" runat="server" CommandName="getPosting" CommandArgument='<%# Eval("PaymentID") %>' OnClientClick="return confirmationPost();" Enabled='<%# if(Eval("status").toString()="N",if(Eval("postRec").toString()="Y","true","false"),"false") %>'><i class="fa fa-send"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="40px" HeaderText="Print" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbPrint" CssClass="btn btn-default" runat="server" CommandName="getPrint" CommandArgument='<%# Eval("PaymentID") %>'><i class="fa fa-print"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Opsi" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <div class="btn-group">
                                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" style="padding-bottom:11px; padding-top:11px;">
                                <span class="caret"></span></button>
                                <ul class="dropdown-menu" style="left:-60px; min-width: 100px;" role="menu">
                                    <li><asp:LinkButton ID="lbDetail" CssClass="btn btn-link btn_right" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("PaymentID") %>'> Detail <i class="fa fa-file-text-o"></i> </asp:LinkButton></li>
                                    <li><asp:LinkButton ID="lbEdit" CssClass="btn btn-link btn_right" runat="server" CommandName="getEdit" CommandArgument='<%# Eval("PaymentID") %>' Enabled='<%# if(Eval("editRec").toString()="Y","true","false") %>'> Edit <i class="fa fa-edit"></i></asp:LinkButton></li>
                                    <li><asp:LinkButton ID="lbDelete" CssClass="btn btn-link btn_right" runat="server" CommandName="getDelete" CommandArgument='<%# Eval("PaymentID") %>' Enabled='<%# if(Eval("status").toString()<>"N","false",if(Eval("deleteRec").toString()="Y","true","false")) %>'> Delete <i class="fa fa-trash"></i></asp:LinkButton></li>
                                </ul>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <pagerstyle cssclass="pagination-ys">
                </pagerstyle>
            </asp:GridView>
            <br />
            </asp:Panel>
            <asp:Panel ID="pnCustGroup" runat="server" Visible="false">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvCustAR" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10" ShowFooter="true">
                            <Columns>
                                <asp:BoundField ItemStyle-Width ="100px" DataField="ARGroup" HeaderText="AR Group" />
                                <asp:BoundField ItemStyle-Width ="120px" DataField="CoyName" HeaderText="Coy Name" />
                                <asp:BoundField DataField="Address" HeaderText="Address" />
                                <asp:BoundField DataField="BilllingAddress" HeaderText="Billing Address" />
                                <asp:BoundField ItemStyle-Width ="70px" DataField="city" HeaderText="City" />
                                <asp:BoundField ItemStyle-Width ="70px" DataField="Phone" HeaderText="Phone" />
                                <asp:BoundField ItemStyle-Width ="70px" DataField="Email" HeaderText="Email" />
                                <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmountTot" runat="server" Text='<%# if(Eval("amountTrans").toString()="","0.00",Eval("amountTrans","{0:N2}")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="50px" HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbSelect" CssClass="btn btn-default" runat="server" CommandName="getSelect" CommandArgument='<%# Eval("VendorID") %>'><i class="fa fa-check"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <pagerstyle cssclass="pagination-ys">
                            </pagerstyle>
                        </asp:GridView>
                        </asp:Panel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

