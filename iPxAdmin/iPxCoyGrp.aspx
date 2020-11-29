<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxCoyGrp.aspx.vb" Inherits="iPxAdmin_iPxCoyGrp" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Add Coy Group Modal-->
    <div id="formInput" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <button type="button" data-dismiss="modal" aria-label="Close" class="close"><span aria-hidden="true">×</span></button>
                    <h4 id="login-modalLabel" class="modal-title">Form Customer Group</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-6">
                            <div class="form-group">
                                <label for="usr">Coy Group:</label>
                                <asp:TextBox ID="tbCoyGroup" runat="server" CssClass ="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Description:</label><font color=red>*</font>
                                <asp:TextBox ID="tbDescription" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Address:</label><font color=red>*</font>
                                <asp:TextBox ID="tbAddress" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-group">
                                <label for="usr">Contact:</label><font color=red>*</font>
                                <asp:TextBox ID="tbContact" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Phone:</label><font color=red>*</font>
                                <asp:TextBox ID="tbPhone" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Notes:</label>
                                <asp:TextBox ID="tbNotes" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12">
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
                    <asp:LinkButton ID="lbSave" runat="server" CssClass="btn btn-default"><i class="fa fa-save"></i> Save</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Add Coy Group modal end-->
    <!-- Query Coy Group Modal-->
    <div id="formQuery" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortQuery" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H1" class="modal-title">Query Coy Group</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr">Coy Group:</label>
                                <asp:TextBox ID="tbQCoyGroup" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Description:</label>
                                <asp:TextBox ID="tbQDescription" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Address:</label>
                                <asp:TextBox ID="tbQAddress" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Contact:</label>
                                <asp:TextBox ID="tbQContact" runat="server" CssClass ="form-control"></asp:TextBox>
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
    <!-- Query Coy Group modal end-->
    <div class="row">
        <div class="col-lg-12">
            <asp:LinkButton ID="lbAddCoy" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> New Coy group</asp:LinkButton>
            <asp:LinkButton ID="lbQuery" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton><hr style="margin-top:5px; margin-bottom:5px;" />
        </div>
        <div class="col-lg-12">
            <asp:GridView EmptyDataText="No records has been added." ID="gvGrup" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                <Columns>
                    <asp:BoundField ItemStyle-Width="100px" DataField="businessid" HeaderText="Business ID" />
                    <asp:BoundField ItemStyle-Width="100px" DataField="CoyGroup" HeaderText="Coy Group" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField ItemStyle-Width="100px" DataField="Contact" HeaderText="Contact" />
                    <asp:BoundField ItemStyle-Width="150px" DataField="Address" HeaderText="Address" />
                    <asp:BoundField ItemStyle-Width="100px" DataField="Phone" HeaderText="Phone" />
                    <asp:BoundField ItemStyle-Width="150px" DataField="Notes" HeaderText="Notes" />
                    <asp:TemplateField ItemStyle-Width="70px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbEdit" CssClass="btn btn-default" runat="server" CommandName="getEdit" CommandArgument='<%# Eval("CoyGroup") %>'><i class="fa fa-edit"></i></asp:LinkButton>
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

