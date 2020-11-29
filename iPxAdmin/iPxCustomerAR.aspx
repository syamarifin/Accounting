<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxCustomerAR.aspx.vb" Inherits="iPxAdmin_iPxCustomerAR" title="Alcor Accounting" %>

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
                    <h4 id="login-modalLabel" class="modal-title">Query Customer AR</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                                    <div class="col-lg-12">
                                        <div class="form-group" style="margin-bottom:3px;">
                                            <label for="usr">Customer ID:</label>
                                            <asp:TextBox ID="tbCustID" runat="server" CssClass ="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group" style="margin-bottom:3px;">
                                            <label for="usr">AR Group:</label>
                                            <asp:DropDownList ID="dlArgroup" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group" style="margin-bottom:3px;">
                                            <label for="usr">Coy Group:</label>
                                            <asp:DropDownList ID="dlCoyGroup" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group" style="margin-bottom:3px;">
                                            <label for="usr">Coy Name:</label>
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
                                            <label for="usr">Credit Limit:</label>
                                            <asp:TextBox ID="tbCredit" runat="server" CssClass ="form-control"></asp:TextBox>
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
            
            <div class="btn-group">
                <button type="button" style="width:130px;" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                <i class="fa fa-plus"></i> New <span class="caret"></span></button>
                <ul class="dropdown-menu" role="menu">
                  <li><asp:LinkButton ID="lbAddCust" runat="server" CssClass="btn"> Customer</asp:LinkButton></li>
                  <li><asp:LinkButton ID="lbAddCC" runat="server" CssClass="btn"> Credit Card</asp:LinkButton></li>
                </ul>
            </div>
            <asp:LinkButton ID="lbQuery" Width="130px" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton><hr style="margin-top:5px; margin-bottom:5px;" />
        </div>
        <div class="col-lg-12">
            <ul class="nav nav-tabs">
                <li id="agent-tab" class="active"><a href="#Agent" data-toggle="tab"><i class="fa fa-list"></i> Company/ Agent</a></li>
                <li id="card-tab"><a href="#Card" data-toggle="tab"><i class="fa fa-list"></i> Credit Card</a></li>
            </ul>
            <div id="myTabContent" class="tab-content">
                <div class="tab-pane active in" id="Agent">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvCustAR" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <%--<asp:BoundField DataField="businessid" HeaderText="Business ID" />--%>
                            <asp:BoundField DataField="CustomerID" HeaderText="Customer ID" />
                            <asp:BoundField DataField="ARGroup" HeaderText="AR Group" />
                            <asp:BoundField DataField="CoyGroup" HeaderText="Coy Group" />
                            <asp:BoundField DataField="CoyName" HeaderText="Coy Name" />
                            <asp:BoundField DataField="coaLink" HeaderText="COA" />
                            <asp:BoundField DataField="FOLink" HeaderText="PMS Link" />
                            <asp:BoundField DataField="CreditLimit" HeaderText="Limit" />
                            <asp:BoundField DataField="city" HeaderText="City" />
                            <asp:BoundField DataField="Phone" HeaderText="Phone" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:TemplateField ItemStyle-Width="70px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbEdit" CssClass="btn btn-default" runat="server" CommandName="getEdit" CommandArgument='<%# Eval("CustomerID") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="Card">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvCustCrad" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <%--<asp:BoundField DataField="businessid" HeaderText="Business ID" />--%>
                            <asp:BoundField DataField="CustomerID" HeaderText="Customer ID" />
                            <asp:BoundField DataField="ARGroup" HeaderText="AR Group" />
                            <asp:BoundField DataField="CoyGroup" HeaderText="Coy Group" />
                            <asp:BoundField DataField="CoyName" HeaderText="Coy Name" />
                            <asp:BoundField DataField="CardType" HeaderText="Card Type" />
                            <asp:BoundField DataField="COALink" HeaderText="COA Link" />
                            <asp:BoundField DataField="CoaComision" HeaderText="Coa Comision" />
                            <asp:TemplateField HeaderText="CommissionPct(%)">
                                <ItemTemplate>
                                    <asp:Label ID="lblComisi" runat="server" Text='<%# Eval("CommissionPct","{0:N2}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Phone" HeaderText="Phone" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:TemplateField ItemStyle-Width="70px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbEdit" CssClass="btn btn-default" runat="server" CommandName="getEdit" CommandArgument='<%# Eval("CustomerID") %>'><i class="fa fa-edit"></i></asp:LinkButton>
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

