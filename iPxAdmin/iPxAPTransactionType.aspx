<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxAPTransactionType.aspx.vb" Inherits="iPxAdmin_iPxAPTransactionType" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Form List Coa Modal-->
    <div id="formAddTrans" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbCloseCoa" CssClass="close" runat="server" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H2" class="modal-title">List Coa</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <div class="input-group">
                                    <asp:TextBox ID="tbSearchCOAMdl" runat="server" CssClass ="form-control"></asp:TextBox>
                                    <div class="input-group-btn">
                                        <asp:LinkButton ID="lbSearchCOAMdl" class="btn btn-default" runat="server" Font-Size="Small"><span class="glyphicon glyphicon-search" style="height:20px;"></span></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-12">
                            <div style="overflow-x:auto;max-height:450px;">
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
    <div id="formInput" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <button type="button" data-dismiss="modal" aria-label="Close" class="close"><span aria-hidden="true">×</span></button>
                    <h4 id="login-modalLabel" class="modal-title">Form Transaction Type</h4>
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
                                    <span> <strong> Active </strong> </span> </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbSave" runat="server" CssClass="btn btn-default"><i class="fa fa-save"></i> Save</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Add Transaction Type modal end-->
    <!-- Query Transaction Type Modal-->
    <div id="formQuery" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortQuery" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H1" class="modal-title">Query Customer Group</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr">Transaction Type:</label>
                                <asp:TextBox ID="tbQTransType" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Description:</label>
                                <asp:TextBox ID="tbQDescription" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Coa:</label>
                                <asp:TextBox ID="tbQCoa" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Status:</label>
                                <asp:DropDownList ID="dlQStatus" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lblQuery" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Query Transaction Type modal end-->
    <div class="row">
        <div class="col-lg-12">
            <asp:LinkButton ID="lbAddType" Width="200px" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> New Transaction Type</asp:LinkButton>
            <asp:LinkButton ID="lbQuery" Width="200px" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton><hr style="margin-top:5px; margin-bottom:5px;" />
        </div>
        <div class="col-lg-12">
            <asp:GridView EmptyDataText="No records has been added." ID="gvTransType" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                <Columns>
                    <asp:BoundField ItemStyle-Width="150px" DataField="TransactionType" HeaderText="Transaction Type" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField ItemStyle-Width="150px" DataField="Coa" HeaderText="Coa" />
                    <asp:TemplateField ItemStyle-Width="70px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbEdit" CssClass="btn btn-default" runat="server" CommandName="getEdit" CommandArgument='<%# Eval("TransactionType") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <pagerstyle cssclass="pagination-ys">
                </pagerstyle>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

