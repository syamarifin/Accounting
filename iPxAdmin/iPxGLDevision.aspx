<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxGLDevision.aspx.vb" Inherits="iPxAdmin_iPxGLDevision" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        function hideModalAddDev() {
            $('#formInputDev').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalAddDev() {
            $('#formInputDev').modal({ backdrop: 'static',
                keyboard: false
            }, 'show');
        }
        function hideModalAddDept() {
            $('#formInputDept').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalAddDept() {
            $('#formInputDept').modal({ backdrop: 'static',
                keyboard: false
            }, 'show');
        }
        function hideModalAddSubDept() {
            $('#formInputSubDept').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalAddSubDept() {
            $('#formInputSubDept').modal({ backdrop: 'static',
                keyboard: false
            }, 'show');
        }

        
        function hideModalQueryDev() {
            $('#formQueryDev').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalQueryDev() {
            $('#formQueryDev').modal({ backdrop: 'static',
                keyboard: false
            }, 'show');
        }
        
        function DevActive()
        {
            $("#Dev-tab").addClass("active");
            $("#Dept-tab").removeClass("active");
            $("#SubDept-tab").removeClass("active");
            $("#Dev").addClass("active in");
            $("#Dept").removeClass("active in");
            $("#SubDept").removeClass("active in");
        }
        function DeptActive()
        {
            $("#Dev-tab").removeClass("active");
            $("#Dept-tab").addClass("active");
            $("#SubDept-tab").removeClass("active");
            $("#Dev").removeClass("active in");
            $("#Dept").addClass("active in");
            $("#SubDept").removeClass("active in");
        }
        function SubDeptActive()
        {
            $("#Dev-tab").removeClass("active");
            $("#Dept-tab").removeClass("active");
            $("#SubDept-tab").addClass("active");
            $("#Dev").removeClass("active in");
            $("#Dept").removeClass("active in");
            $("#SubDept").addClass("active in");
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Add Dev Modal-->
    <div id="formInputDev" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortAddDev" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="login-modalLabel" class="modal-title">Form Devision </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr">Devision ID:</label><font color=red>*</font>
                                <asp:TextBox ID="tbDevID" runat="server" CssClass ="form-control" MaxLength="2"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Description:</label><font color=red>*</font>
                                <asp:TextBox ID="tbDesc" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12">
                            <div class="form-group">
                                <div class="square-blue pull-left pv-15">
                                    <label class="ui-checkbox">
                                    <asp:CheckBox  ID="cbActiveDev" runat="server" />
                                    <span> <strong> Active </strong> </span> </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbSaveDev" runat="server" CssClass="btn btn-default"><i class="fa fa-save"></i> Save</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Add Dev modal end-->
    <!-- Add Departement Modal-->
    <div id="formInputDept" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortAddDept" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H2" class="modal-title">Form Departement </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr">Devision:</label><font color=red>*</font>
                                <asp:TextBox ID="tbDeptDev" runat="server" CssClass ="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Departement ID:</label><font color=red>*</font>
                                <asp:TextBox ID="tbDeptID" runat="server" CssClass ="form-control" MaxLength="2"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Description:</label><font color=red>*</font>
                                <asp:TextBox ID="tbDescDept" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12">
                            <div class="form-group">
                                <div class="square-blue pull-left pv-15">
                                    <label class="ui-checkbox">
                                    <asp:CheckBox  ID="cbActiveDept" runat="server" />
                                    <span> <strong> Active </strong> </span> </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbSaveDept" runat="server" CssClass="btn btn-default"><i class="fa fa-save"></i> Save</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Add Departement modal end-->
    <!-- Add SubDepartement Modal-->
    <div id="formInputSubDept" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortSub" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H3" class="modal-title">Form Sub Departement </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr">Devision:</label><font color=red>*</font>
                                <asp:TextBox ID="tbSubDev" runat="server" CssClass ="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Departement:</label><font color=red>*</font>
                                <asp:TextBox ID="tbSubDept" runat="server" CssClass ="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">ID:</label><font color=red>*</font>
                                <asp:TextBox ID="tbSubID" runat="server" CssClass ="form-control" MaxLength="2"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Description:</label><font color=red>*</font>
                                <asp:TextBox ID="tbSubDesc" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12">
                            <div class="form-group">
                                <div class="square-blue pull-left pv-15">
                                    <label class="ui-checkbox">
                                    <asp:CheckBox  ID="cbActiveSub" runat="server" />
                                    <span> <strong> Active </strong> </span> </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbSaveSub" runat="server" CssClass="btn btn-default"><i class="fa fa-save"></i> Save</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Add SubDepartement modal end-->
    <!-- Query Dev Modal-->
    <div id="formQueryDev" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortQueryDev" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H1" class="modal-title">Query Devision </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr">Description:</label>
                                <asp:TextBox ID="tbQDesc" runat="server" CssClass ="form-control"></asp:TextBox>
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
                    <asp:LinkButton ID="lblQueryDev" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Query Dev modal end-->
     <ul class="nav nav-tabs">
        <li id="Dev-tab" class="active"><a href="#Dev" data-toggle="tab"><i class="fa fa-list"></i> Devision&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a></li>
        <li id="Dept-tab"><a href="#Dept" data-toggle="tab"><i class="fa fa-list"></i> Departement&nbsp;&nbsp;&nbsp;&nbsp;</a></li>
        <li id="SubDept-tab"><a href="#SubDept" data-toggle="tab"><i class="fa fa-list"></i> Sub Departement</a></li>
    </ul>
    <div id="myTabContent" class="tab-content">
        <div class="tab-pane active in" id="Dev">
            <div class="row">
                <div class="col-lg-12">
                    <br />
                    <asp:LinkButton Width="200px" ID="lbCreateDev" runat="server" CssClass="btn btn-default" OnClientClick = "return ConfirmCreateBath();"><i class="fa fa-plus"></i> Create Devision</asp:LinkButton>
                    <asp:LinkButton Width="200px" ID="lbQueryDev" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
                    <asp:GridView EmptyDataText="No records has been added." ID="gvGLDev" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <HeaderStyle Height="50px" />
                        <Columns>
                            <asp:BoundField ItemStyle-Width="90px" DataField="Division" HeaderText="Devision ID" />
                            <asp:BoundField DataField="Description" HeaderText="Description" />
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbEditDev" CssClass="btn btn-default" runat="server" CommandName="getEdit" CommandArgument='<%# Eval("Division")%>' Enabled='<%# if(Eval("editGLConf").toString()="Y","true","false") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Dept" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailDept" CssClass="btn btn-default" runat="server" CommandName="getDetailDept" CommandArgument='<%# Eval("Division")%>'><i class="fa fa-list"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="tab-pane fade" id="Dept">
            <div class="row">
                <div class="col-lg-12">
                    <br />
                    <asp:LinkButton Width="200px" ID="lbCreateDept" runat="server" CssClass="btn btn-default" OnClientClick = "return ConfirmCreateClear();"><i class="fa fa-plus"></i> Create Departement</asp:LinkButton>
                    <asp:LinkButton Width="200px" ID="lbQueryDept" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
                    <asp:GridView EmptyDataText="No records has been added." ID="gvGLDept" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <HeaderStyle Height="50px" />
                        <Columns>
                            <asp:BoundField ItemStyle-Width="90px" DataField="Departement" HeaderText="Departement ID" />
                            <asp:BoundField DataField="DescDiv" HeaderText="Devision" />
                            <asp:BoundField DataField="Description" HeaderText="Description" />
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbEditDept" CssClass="btn btn-default" runat="server" CommandName="getEdit" CommandArgument='<%# Eval("Departement")%>' Enabled='<%# if(Eval("editGLConf").toString()="Y","true","false") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="SubDept" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailSubDept" CssClass="btn btn-default" runat="server" CommandName="getDetailSubDept" CommandArgument='<%# Eval("Departement")%>'><i class="fa fa-list"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="tab-pane fade" id="SubDept">
    	    <div class="row">
                <div class="col-lg-12">
                    <br />
                    <asp:LinkButton Width="200px" ID="lbCreateSubDept" runat="server" CssClass="btn btn-default" OnClientClick = "return ConfirmPostClear();"><i class="fa fa-plus"></i> Create SubDepartement</asp:LinkButton>
                    <asp:LinkButton Width="200px" ID="lbQuerySubDept" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
                    <asp:GridView EmptyDataText="No records has been added." ID="gvGLSubDept" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <HeaderStyle Height="50px" />
                        <Columns>
                            <asp:BoundField ItemStyle-Width="90px" DataField="SubDept" HeaderText="ID" />
                            <asp:BoundField DataField="DescDiv" HeaderText="Devision" />
                            <asp:BoundField DataField="DescDept" HeaderText="Departement" />
                            <asp:BoundField DataField="Description" HeaderText="Description" />
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbEditSubDept" CssClass="btn btn-default" runat="server" CommandName="getEdit" CommandArgument='<%# Eval("SubDept")%>' Enabled='<%# if(Eval("editGLConf").toString()="Y","true","false") %>'><i class="fa fa-edit"></i></asp:LinkButton>
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

