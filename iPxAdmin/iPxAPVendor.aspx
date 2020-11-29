<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxAPVendor.aspx.vb" Inherits="iPxAdmin_iPxAPVendor" title="Alcor Accounting Vendor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        function CCActive()
        {
            $("#agent-tab").removeClass("active");
            $("#card-tab").addClass("active");
            $("#Agent").removeClass("active in");
            $("#Card").addClass("active in");
        }
    </script>
    <style>
        .Gridview{
            overflow-x: scroll;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Query Customer AR Modal-->
    <div id="formQuery" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md" style="width:400px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortQuery" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="login-modalLabel" class="modal-title">Query Vendor AP</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Vendor ID:</label>
                                <asp:TextBox ID="tbCustID" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">AP Group:</label>
                                <asp:DropDownList ID="dlArgroup" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Vendor Name:</label>
                                <asp:TextBox ID="tbCoyName" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Country:</label>
                                <asp:DropDownList ID="dlCountry" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlCountry_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Province:</label>
                                <asp:DropDownList ID="dlProvince" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlProvince_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">City:</label>
                                <asp:DropDownList ID="dlCity" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
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
    <!-- Query Customer AR modal end-->
    <div class="row">
        <div class="col-lg-12">
            
            <asp:LinkButton ID="lbAddCust" Width="130px" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> New Vendor</asp:LinkButton>
            <asp:LinkButton ID="lbQuery" Width="130px" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton><hr style="margin-top:5px; margin-bottom:5px;" />
        </div>
        <div class="col-lg-12">
            <asp:GridView EmptyDataText="No records has been added." ID="gvCustAR" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                <Columns>
                    <%--<asp:BoundField DataField="businessid" HeaderText="Business ID" />--%>
                    <asp:BoundField DataField="VendorID" HeaderText="Vendor ID" />
                    <asp:BoundField DataField="APGroup" HeaderText="AP Group" />
                    <asp:BoundField DataField="CoyName" HeaderText="Vendor Name" />
                    <asp:BoundField DataField="coaLink" HeaderText="COA" />
                    <%--<asp:BoundField DataField="FOLink" HeaderText="PMS Link" />--%>
                    <asp:BoundField DataField="CreditLimit" HeaderText="Limit" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="city" HeaderText="City" />
                    <asp:BoundField DataField="Phone" HeaderText="Phone" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:TemplateField ItemStyle-Width="70px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbEdit" CssClass="btn btn-default" runat="server" CommandName="getEdit" CommandArgument='<%# Eval("VendorID") %>'><i class="fa fa-edit"></i></asp:LinkButton>
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

