<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxFOLink.aspx.vb" Inherits="iPxAdmin_iPxFOLink" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Form List Coa Modal-->
    <div id="formAddTrans" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbCloseFo" CssClass="close" runat="server" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H2" class="modal-title">your PMS account login</h4>
                </div>
                <div class="modal-body">
                    <table style="width:100%;">
                        <tr style="height:30px;">
                            <td style="width:25%;">Email</td>
                            <td style="padding:5px;">
                                <asp:TextBox ID="tbUserPMS" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%;">Password</td>
                            <td style="padding:5px;">
                                <asp:TextBox ID="tbPassPMS" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%;">Password Generic Id</td>
                            <td style="padding:5px;">
                                <asp:TextBox ID="tbGeneric" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbLogin" runat="server" CssClass="btn btn-default" Width="100px"> Login </asp:LinkButton>
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
                                <label for="usr">PMS Link:</label><font color=red>*</font>
                                <div class="input-group">
                                    <asp:DropDownList ID="dlFO" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                    <div class="input-group-btn">
                                      <asp:LinkButton ID="lbSearchFo" runat="server" CssClass="btn btn-default"><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="usr">Description:</label><font color=red>*</font>
                                <asp:TextBox ID="tbDescription" runat="server" CssClass ="form-control"></asp:TextBox>
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
    <!-- Query Customer AR Modal-->
    <div id="formQuery" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md" style="width:400px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortQuery" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H1" class="modal-title">Query PMS Link</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Business Name:</label>
                                <asp:TextBox ID="tbQBusinessName" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Description:</label>
                                <asp:TextBox ID="tbQDescription" runat="server" CssClass ="form-control"></asp:TextBox>
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
    <!-- Query Customer AR modal end-->
    <div class="row">
        <div class="col-lg-12">
            <asp:LinkButton ID="lbAddFO" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> New PMS Link</asp:LinkButton>
            <asp:LinkButton ID="lbQuery" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton><hr style="margin-top:5px; margin-bottom:5px;" />
        </div>
        <div class="col-lg-12">
            <asp:GridView EmptyDataText="No records has been added." ID="gvFO" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                <Columns>
                    <asp:BoundField ItemStyle-Width="100px" DataField="businessid" HeaderText="Business ID" />
                    <asp:BoundField ItemStyle-Width="250px" DataField="businessname" HeaderText="PMS Name" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:TemplateField ItemStyle-Width="70px" HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbEdit" CssClass="btn btn-default" runat="server" CommandName="getDelete" CommandArgument='<%# Eval("FoLink") %>' Enabled='<%# if(eval("EditOp").toString()="Y","true","false") %>' ><i class="fa fa-trash"></i></asp:LinkButton>
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

