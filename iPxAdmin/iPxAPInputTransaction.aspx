<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxAPInputTransaction.aspx.vb" Inherits="iPxAdmin_iPxAPInputTransaction" title="Alcor Accounting Input AP Transaction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Form List Coa Modal-->
    <div id="ListCOA" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbCloseCoa" CssClass="close" runat="server" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H3" class="modal-title">List Coa</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div style="max-height:450px;">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvCoa" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None">
                                <Columns>
                                    <asp:BoundField ItemStyle-Width="150px" DataField="Coa" HeaderText="Coa" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:TemplateField ItemStyle-Width="70px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbSelect" CssClass="btn btn-default" runat="server" CommandName="getSelect" CommandArgument='<%# Eval("Coa") %>'><i class="fa fa-check-square-o"></i></asp:LinkButton>
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
    <!-- Form List Coa modal end-->
    <!-- Add Transaction Type Modal-->
    <div id="formType" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbCloseType" CssClass="close" runat="server" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H4" class="modal-title">Form Transaction Type</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr">Transaction Type:</label><font color=red>*</font>
                                <asp:TextBox ID="tbTransType" runat="server" CssClass ="form-control" MaxLength="2"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Description:</label><font color=red>*</font>
                                <asp:TextBox ID="tbDescription" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Coa:</label><font color=red>*</font>
                                <div class="input-group">
                                    <asp:TextBox ID="tbCoa" runat="server" CssClass ="form-control"></asp:TextBox>
                                    <div class="input-group-btn">
                                      <asp:LinkButton ID="lbSearchCoa" runat="server" CssClass="btn btn-default"><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="square-blue pull-left pv-15">
                                    <label class="ui-checkbox">
                                    <asp:CheckBox  ID="cbActive" runat="server"  />
                                    <span style=""> <strong> Active </strong> </span> </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbSaveType" runat="server" CssClass="btn btn-default"><i class="fa fa-save"></i> Save</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Add Transaction Type modal end-->
    <%--Delete Customer AR Modal--%>
    <div id="formDeleteTrans" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <h4 id="H2" class="modal-title">Delete AP Transaction</h4>
                </div>
                <div class="modal-body">
                    Do you want to delete the AP transaction list ?.
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbAbortDelete" runat="server" CssClass="btn btn-default"><i class="fa fa-close"></i> Abort</asp:LinkButton>
                    <asp:LinkButton ID="lbDelete" runat="server" CssClass="btn btn-default"><i class="fa fa-trash"></i> Delete</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <%--End Delete Customer AR Modal--%>
    <!-- Query Customer AR Modal-->
    <div id="formInput" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortFind" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="login-modalLabel" class="modal-title">Find Vendor</h4>
                </div>
                <div class="modal-body">
                    
                    <div class="input-group">
                        <asp:TextBox ID="tbQCust" runat="server" CssClass="form-control" placeholder="Search by name..."></asp:TextBox>
                        <div class="input-group-btn">
                            <asp:LinkButton ID="lbSearcCust" runat="server" CssClass="btn btn-default"><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                        </div>
                    </div>
                    <hr style="margin:5px;"/>
                            <asp:GridView EmptyDataText="No records has been added." ID="gvCustAR" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None">
                                <Columns>
                                    <asp:BoundField ItemStyle-Width ="70px" DataField="businessid" HeaderText="Business ID" />
                                    <asp:BoundField ItemStyle-Width ="70px" DataField="VendorID" HeaderText="Vendor ID" />
                                    <asp:BoundField ItemStyle-Width ="120px" DataField="APGroup" HeaderText="AP Group" />
                                    <asp:BoundField DataField="CoyName" HeaderText="Coy Name" />
                                    <asp:TemplateField ItemStyle-Width="70px" HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbSelect" CssClass="btn btn-default" runat="server" CommandName="getSelect" CommandArgument='<%# if(Eval("VendorID").toString()="",Eval("VendorID"),Eval("VendorID") + Eval("CoyName")) %>'><i class="fa fa-check"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                    <%--<ul class="nav nav-tabs" id="myTab" role="tablist">
                        <li class="nav-item active" id="CA_tab">
                            <a class="nav-link" id="CA-tab" data-toggle="tab" href="#CA" role="tab" aria-controls="CA" aria-selected="false">Company/Agent</a>
                        </li>
                        <li class="nav-item" id="CC_tab">
                            <a class="nav-link" id="CC-tab" data-toggle="tab" href="#CC" role="tab" aria-controls="CC" aria-selected="false">Credit Card</a>
                        </li>
                    </ul>--%>
                    
                    <%--<div class="tab-content" id="myTabContent">
                        <div class="tab-pane fade active in" id="CA" role="tabpanel" aria-labelledby="CA-tab">
                            <div style="max-height:400px;overflow:auto;">
                            
                            </div>
                        </div>
                        <div class="tab-pane fade" id="CC" role="tabpanel" aria-labelledby="CC-tab">
                            <div style="max-height:400px;overflow:auto;">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvCustARCC" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None">
                                <Columns>
                                    <asp:BoundField ItemStyle-Width ="70px" DataField="businessid" HeaderText="Business ID" />
                                    <asp:BoundField ItemStyle-Width ="70px" DataField="CustomerID" HeaderText="Customer ID" />
                                    <asp:BoundField ItemStyle-Width ="120px" DataField="ARGroup" HeaderText="AR Group" />
                                    <asp:BoundField ItemStyle-Width ="120px" DataField="CoyGroup" HeaderText="Coy Group" />
                                    <asp:BoundField DataField="CoyName" HeaderText="Coy Name" />
                                    <asp:TemplateField ItemStyle-Width="70px" HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbSelect" CssClass="btn btn-default" runat="server" CommandName="getSelect" CommandArgument='<%# if(Eval("CustomerID").toString()="",Eval("CustomerID"),Eval("CustomerID") + Eval("CoyName")) %>'><i class="fa fa-check"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            </div>
                        </div>
                    </div>--%>
                </div>
            </div>
        </div>
    </div>
    <!-- Query Customer AR modal end-->
    <!-- Query Customer AR Modal-->
    <div id="formAddTrans" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortDetail" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H1" class="modal-title">Form AP Transaction</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">Vendor:</label><font color=red>*</font>
                        <div class="input-group">
                            <asp:TextBox ID="tbCustID" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            <div class="input-group-btn">
                                <asp:LinkButton ID="lbFindCustID" runat="server" CssClass="btn btn-default"><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">Transaction Type:</label>
                        <div class="input-group">
                            <asp:DropDownList ID="dlType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlType_SelectedIndexChanged">
                            </asp:DropDownList>
                            <div class="input-group-btn">
                                <asp:LinkButton ID="lbAddType" runat="server" CssClass="btn btn-default"><i class="fa fa-plus" style="font-size:20px;"></i></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">PO No:</label>
                        <asp:TextBox ID="tbPO" runat="server" CssClass ="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">RR No:</label>
                        <asp:TextBox ID="tbRR" runat="server" CssClass ="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">Amount:</label>
                        <asp:TextBox ID="tbAmount" runat="server" CssClass ="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">Notes:</label>
                        <asp:TextBox ID="tbNotes" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbSaveDetail" Width="90px" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> Add</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Query Customer AR modal end-->
    <div class="small-box">
        <div class="inner">
            <h3>AP Transaction <asp:Label ID="lbTransID" runat="server" Text="TransID"></asp:Label></h3>
                    
            <div class="row">
                <div class="col-lg-6">
                    <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">Inventory Link:</label><font color=red>*</font>
                        <asp:DropDownList ID="dlFOLink" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">Transaction Date:</label><font color=red>*</font>
                        <div class="input-group date datepicker" style="padding:0;">
                            <asp:TextBox ID="tbTransDate" runat="server" CssClass="form-control" Enabled="false" OnTextChanged="cari" AutoPostBack="true"></asp:TextBox>
                            <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">Description:</label>
                        <asp:TextBox ID="tbDesc" runat="server" CssClass ="form-control" TextMode="MultiLine" Height="100"></asp:TextBox>
                    </div>
                </div>
                <div class="col-lg-12">
                    <asp:Panel ID="PnReason" runat="server">
                        <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">Reason:</label>
                        <asp:TextBox ID="tbReason" runat="server" CssClass ="form-control" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                    </div>
                    </asp:Panel>
                </div>
            </div>
            <hr  style="margin-top:10px; margin-bottom:10px;"/>
            <asp:GridView EmptyDataText="No records has been added." ID="gvTransDetail" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" 
            HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10" ShowFooter="true">
                <Columns>
                    <asp:BoundField ItemStyle-Width ="100px" DataField="CoyName" HeaderText="Vendor Name" />
                    <asp:BoundField ItemStyle-Width ="70px" DataField="Description" HeaderText="Transaction Type" />
                    <asp:BoundField ItemStyle-Width ="100px" DataField="POno" HeaderText="PO No" />
                    <asp:BoundField ItemStyle-Width ="80px" DataField="PVno" HeaderText="PV No" />
                    <asp:BoundField ItemStyle-Width ="100px" DataField="RRno" HeaderText="RR No" />
                    <asp:BoundField DataField="notes" HeaderText="Notes" />
                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lblOpen" runat="server" Text='<%# if(Eval("amountcr").toString()="","0.00",Eval("amountcr","{0:N2}")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbEdit" CssClass="btn btn-default" runat="server" CommandName="getEdit" CommandArgument='<%# Eval("RecID")%>' Enabled='<%# if(Session("sStatusARTrans")<>"N","false",if(Trim(Eval("PVno").toString())="",if(Eval("edit").toString()="Y",if(Eval("CloseStatus").toString()="Close","false","true"),"false"),"false"))%>'><i class="fa fa-edit"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" CssClass="btn btn-default" runat="server" CommandName="getDelete" CommandArgument='<%# Eval("RecID")%>' Enabled='<%# if(Session("sStatusARTrans")<>"N","false",if(Trim(Eval("PVno").toString())="",if(Eval("deleteAR").toString()="Y",if(Eval("CloseStatus").toString()="Close","false","true"),"false"),"false"))%>'><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <pagerstyle cssclass="pagination-ys">
                </pagerstyle>
            </asp:GridView>
        </div>
        <div class="icon">
            <asp:Label ID="lbStatus" runat="server" Text="New"></asp:Label>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div id="footer" class="container" >
        <a class="collapsed btn btn-default" style="width:120px;" role="button" href="iPxAPTransaction.aspx"> <i class="fa fa-close"></i> Abort </a>
        <asp:LinkButton ID="lbAddDetail" Width="120px" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> Add Detail </asp:LinkButton>
        <asp:LinkButton ID="lbSave" Width="120px" runat="server" CssClass="btn btn-default"><i class="fa fa-save"></i> Update </asp:LinkButton>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

