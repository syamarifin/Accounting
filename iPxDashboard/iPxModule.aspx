<%@ Page Language="VB" MasterPageFile="~/iPxDashboard/iPxDashboard.master" AutoEventWireup="false" CodeFile="iPxModule.aspx.vb" Inherits="iPxDashboard_iPxModule" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        function hideModalAddModule() {
            $('#formInputModule').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalAddModule() {
            $('#formInputModule').modal({ backdrop: 'static',
                keyboard: false
            },'show');
        }
        function hideModalAddFunct() {
            $('#formInputFunct').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalAddFunct() {
            $('#formInputFunct').modal({ backdrop: 'static',
                keyboard: false
            },'show');
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Add Function Modal-->
    <div id="formInputFunct" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" data-backdrop = 'static' aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortAddFunct" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H2" class="modal-title">Form Function </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr">Module:</label>
                                <asp:TextBox ID="tbModuleDesc" runat="server" CssClass ="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Function:</label><font color=red>*</font>
                                <asp:TextBox ID="tbFunction" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12">
                            <div class="form-group">
                                <div class="square-blue pull-left pv-15">
                                    <label class="ui-checkbox">
                                    <asp:CheckBox  ID="cbActiveFuct" runat="server" />
                                    <span> <strong> Active </strong> </span> </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbSaveFunct" runat="server" CssClass="btn btn-default"><i class="fa fa-save"></i> Save</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Add Function modal end-->
    <!-- Add Function Modal-->
    <div id="formInputModule" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" data-backdrop = 'static' aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortModule" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H1" class="modal-title">Form Module </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr">Module:</label><font color=red>*</font>
                                <asp:TextBox ID="tbModule" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12">
                            <div class="form-group">
                                <div class="square-blue pull-left pv-15">
                                    <label class="ui-checkbox">
                                    <asp:CheckBox  ID="cbActiveModule" runat="server" />
                                    <span> <strong> Active </strong> </span> </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbSaveModule" runat="server" CssClass="btn btn-default"><i class="fa fa-save"></i> Save</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Add Function modal end-->
    <div class="row">
        <div class="col-lg-12">
            <asp:LinkButton ID="lbAddModule" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> New Module</asp:LinkButton>
            <asp:LinkButton ID="lbQuery" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
            <asp:LinkButton ID="lbAddFunct" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> New Function</asp:LinkButton><hr style="margin-top:5px; margin-bottom:5px;" />
        </div>
        <div class="col-lg-5">
            <asp:GridView EmptyDataText="No records has been added." ID="gvModuleAcc" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None">
                <Columns>
                    <asp:TemplateField HeaderText="Funct" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbFunc" runat="server" CssClass="btn btn-default" CommandName="getDetail" CommandArgument='<%# Eval("id").toString() %>'><i class="fa fa-list"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="50px" DataField="id" HeaderText="id" />
                    <asp:BoundField DataField="description" HeaderText="Module" />
                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbEdit" runat="server" CssClass="btn btn-default" CommandName="getEdit" CommandArgument='<%# Eval("id").toString() %>'><i class="fa fa-edit"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="col-lg-7">
            <asp:GridView EmptyDataText="No records has been added." ID="gvFuncAcc" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None">
                <Columns>
                    <asp:BoundField ItemStyle-Width="50px" DataField="id" HeaderText="id" />
                    <asp:BoundField ItemStyle-Width="150px" DataField="moduleDesc" HeaderText="Module" />
                    <asp:BoundField DataField="description" HeaderText="Function" />
                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbEdit" runat="server" CssClass="btn btn-default" CommandName="getEdit" CommandArgument='<%# Eval("id").toString() %>'><i class="fa fa-edit"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

