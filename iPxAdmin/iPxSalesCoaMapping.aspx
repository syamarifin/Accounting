<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxSalesCoaMapping.aspx.vb" Inherits="iPxAdmin_iPxSalesCoaMapping" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        function CreditActive()
        {
            $("#debit-tab").removeClass("active");
            $("#credit-tab").addClass("active");
            $("#Debit").removeClass("active in");
            $("#Credit").addClass("active in");
        }
        function hideModalListCOA() {
            $('#formListCOA').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalListCOA() {
            $('#formListCOA').modal({ backdrop: 'static',
                keyboard: false
            }, 'show');
        }
        function hideModalSettingCOA() {
            $('#formSetAllCoa').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalSettingCOA() {
            $('#formSetAllCoa').modal({ backdrop: 'static',
                keyboard: false
            }, 'show');
        }
    </script>
    <style>
        .btn-border
        {
    	    border-color:Silver;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Add COA Modal-->
    <div id="formListCOA" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md" style="width:800px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortListCOA" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H2" class="modal-title">List COA </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <div class="input-group">
                                    <asp:TextBox ID="tbFindCoaList" runat="server" CssClass ="form-control"></asp:TextBox>
                                    <div class="input-group-btn">
                                        <asp:LinkButton ID="lbFindListCoa" class="btn btn-default" runat="server" Font-Size="Small"><span class="glyphicon glyphicon-search" style="height:20px;"></span></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-12">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvCoa" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="70px" HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbSelect" CssClass="btn btn-default" runat="server" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' CommandName="getSelect" CommandArgument='<%# Eval("Coa") %>'><i class="fa fa-check"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="50px" DataField="Coa" HeaderText="Coa" />
                                    <asp:BoundField ItemStyle-Width="100px" DataField="Description" HeaderText="Description" />
                                    <asp:BoundField ItemStyle-Width="70px" DataField="type" HeaderText="Type" />
                                    <asp:TemplateField ItemStyle-Width="60px" HeaderText="Group Level">
                                        <ItemTemplate>
                                            <asp:Label ID="lbGrpLevel" runat="server" Text='<%# if(Eval("grpLevel").toString()="G","Group","Detail") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="100px" HeaderText="Level">
                                        <ItemTemplate>
                                            <asp:Label ID="lbLevel" runat="server" Text='<%# if(Eval("levelid").toString()="1","Account type",if(Eval("levelid").toString()="2","Account Group",if(Eval("levelid").toString()="3","Account Sub Group 1",if(Eval("levelid").toString()="4","Account Sub Group 1","General")))) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="100px" DataField="Devision" HeaderText="Devision" />
                                    <asp:BoundField ItemStyle-Width="100px" DataField="Departement" HeaderText="Departement" />
                                    <asp:BoundField ItemStyle-Width="120px" DataField="SubDepartement" HeaderText="Sub Departement" />
                                    <asp:BoundField ItemStyle-Width="120px" DataField="status" HeaderText="Status" />
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
    <!-- Add COA modal end-->
    <!-- Setting All COA Modal-->
    <div id="formSetAllCoa" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortSettingCoa" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H1" class="modal-title">Setting COA </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Panel ID="pnRevenue" runat="server">
                                <div class="form-group" style="margin-bottom:3px;">
                                    <asp:CheckBox ID="cbRevenue" runat="server" /> <label for="usr">Revenue:</label>
                                    <div class="input-group" style="padding:0;">
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="lbSetRevenue" CssClass="btn btn-default btn-border" runat="server"><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                                        </div>
                                        <asp:TextBox ID="txtSetRevenue" CssClass="form-control" style="text-align:left"  runat="server" Text='<%# Eval("RevenueCoa")  %>' />
                                    </div>
                                </div>
                                <div class="form-group" style="margin-bottom:3px;">
                                    <asp:CheckBox ID="cbTax" runat="server" /> <label for="usr">Tax:</label>
                                    <div class="input-group" style="padding:0;">
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="lbSetTax" CssClass="btn btn-default btn-border" runat="server"><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                                        </div>
                                        <asp:TextBox ID="txtSetTax" CssClass="form-control" style="text-align:left"  runat="server" Text='<%# Eval("RevenueCoa")  %>' />
                                    </div>
                                </div>
                                <div class="form-group" style="margin-bottom:3px;">
                                    <asp:CheckBox ID="cbService" runat="server" /> <label for="usr">Service:</label>
                                    <div class="input-group" style="padding:0;">
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="lbSetService" CssClass="btn btn-default btn-border" runat="server"><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                                        </div>
                                        <asp:TextBox ID="txtSetService" CssClass="form-control" style="text-align:left"  runat="server" Text='<%# Eval("RevenueCoa")  %>' />
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnSetlement" runat="server">
                                <div class="form-group" style="margin-bottom:3px;">
                                    <asp:CheckBox ID="cbGL" runat="server" /> <label for="usr">POST TO GUEST ACCOUNT:</label>
                                    <div class="input-group" style="padding:0;">
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="lbSetGL" CssClass="btn btn-default btn-border" runat="server"><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                                        </div>
                                        <asp:TextBox ID="txtSetGL" CssClass="form-control" style="text-align:left"  runat="server" Text='<%# Eval("RevenueCoa")  %>' />
                                    </div>
                                </div>
                                <div class="form-group" style="margin-bottom:3px;">
                                    <asp:CheckBox ID="cbCS" runat="server" /> <label for="usr">CASH:</label>
                                    <div class="input-group" style="padding:0;">
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="lbSetCS" CssClass="btn btn-default btn-border" runat="server"><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                                        </div>
                                        <asp:TextBox ID="txtSetCS" CssClass="form-control" style="text-align:left"  runat="server" Text='<%# Eval("RevenueCoa")  %>' />
                                    </div>
                                </div>
                                <div class="form-group" style="margin-bottom:3px;">
                                    <asp:CheckBox ID="cbCR" runat="server" /> <label for="usr">CARD:</label>
                                    <div class="input-group" style="padding:0;">
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="lbSetCR" CssClass="btn btn-default btn-border" runat="server"><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                                        </div>
                                        <asp:TextBox ID="txtSetCR" CssClass="form-control" style="text-align:left"  runat="server" Text='<%# Eval("RevenueCoa")  %>' />
                                    </div>
                                </div>
                                <div class="form-group" style="margin-bottom:3px;">
                                    <asp:CheckBox ID="cbCL" runat="server" /> <label for="usr">CITY LEDGER:</label>
                                    <div class="input-group" style="padding:0;">
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="lbSetCL" CssClass="btn btn-default btn-border" runat="server"><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                                        </div>
                                        <asp:TextBox ID="txtSetCL" CssClass="form-control" style="text-align:left"  runat="server" Text='<%# Eval("RevenueCoa")  %>' />
                                    </div>
                                </div>
                                <div class="form-group" style="margin-bottom:3px;">
                                    <asp:CheckBox ID="cbWB" runat="server" /> <label for="usr">WEB BOOKING:</label>
                                    <div class="input-group" style="padding:0;">
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="lbSetWB" CssClass="btn btn-default btn-border" runat="server"><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                                        </div>
                                        <asp:TextBox ID="txtSetWB" CssClass="form-control" style="text-align:left"  runat="server" Text='<%# Eval("RevenueCoa")  %>' />
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbSaveSetting" Width="150px" runat="server" CssClass="btn btn-default" OnClientClick="return ConfirmUpdateALLCOA();"><i class="fa fa-save"></i> Apply</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Setting All COA modal end-->
    <div class="row">
        <div class="col-md-3">
            <div class="form-group" style="margin-bottom:3px;">
                <label for="usr">Hotel Name:</label>
                <asp:DropDownList ID="dlFOLink" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlFOLink_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <ul class="nav nav-tabs">
        <li id="debit-tab" class="active"><a href="#Debit" data-toggle="tab"><i class="fa fa-list"></i> Revenue&nbsp;</a></li>
        <li id="credit-tab"><a href="#Credit" data-toggle="tab"><i class="fa fa-list"></i> Settlement</a></li>
    </ul>
    <div id="myTabContent" class="tab-content">
        <div class="tab-pane active in" id="Debit">
            <div class="row">
                <div class="col-lg-12">
                    <div style="text-align:right">
                        <asp:LinkButton ID="btnSavegrid" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-save"></i> save</asp:LinkButton>
                        <asp:LinkButton ID="lbSetCoaRev" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-cogs"></i> Setting All COA</asp:LinkButton>
                    </div>
                    <asp:GridView EmptyDataText="No records has been added." ID="gvPosDebit" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" 
                    HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="false" PageSize="10">
                        <Columns>
                            <asp:TemplateField HeaderText="group" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" ItemStyle-HorizontalAlign="left" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdGroup" runat="server"  Value='<%# Eval("RevGrp") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Code" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" ItemStyle-HorizontalAlign="left" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdCode" runat="server"  Value='<%# Eval("PosCode") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="poscode" HeaderText="Code" ItemStyle-Width="70"/>
                            <asp:BoundField DataField="description" HeaderText="Description" />
                            <asp:TemplateField HeaderText="Revenue"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <div class="input-group" style="padding:0;">
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="lbPosCodeRev" CssClass="btn btn-default btn-border" runat="server" CommandName="getCOARev" CommandArgument='<%#  Eval("PosCode") %>'><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                                        </div>
                                        <asp:TextBox Width="150px" ID="txtRevCoa" CssClass="form-control" style="text-align:left"  runat="server" Text='<%# Eval("RevenueCoa")  %>' />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Tax"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <div class="input-group" style="padding:0;">
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="lbPosCodeTax" CssClass="btn btn-default btn-border" runat="server" CommandName="getCOATax" CommandArgument='<%#  Eval("PosCode") %>'><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                                        </div>
                                        <asp:TextBox Width="150px" ID="txtTaxCoa" CssClass="form-control" style="text-align:left"  runat="server" Text='<%# Eval("TaxCoa")  %>' />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Service"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <div class="input-group" style="padding:0;">
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="lbPosCodeSer" CssClass="btn btn-default btn-border" runat="server" CommandName="getCOAService" CommandArgument='<%#  Eval("PosCode") %>'><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                                        </div>
                                        <asp:TextBox Width="150px" ID="txtService" CssClass="form-control" style="text-align:left"  runat="server" Text='<%# Eval("ServiceCoa")  %>' />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="tab-pane fade" id="Credit">
            <div class="row">
                <div class="col-lg-12">
                    <div style="text-align:right">
                        <asp:LinkButton ID="btnSaveCredit" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-save"></i> save</asp:LinkButton>
                        <asp:LinkButton ID="lbSetCoaSet" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-cogs"></i> Setting All COA</asp:LinkButton>
                    </div>
                    <asp:GridView EmptyDataText="No records has been added." ID="gvPosCredit" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" 
                    HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="false" PageSize="10">
                        <Columns>
                            <asp:TemplateField HeaderText="Code" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" ItemStyle-HorizontalAlign="left" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdCode" runat="server"  Value='<%# Eval("PosCode") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CodeDesc" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" ItemStyle-HorizontalAlign="left" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdCodeDesc" runat="server"  Value='<%# Eval("description") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="group" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" ItemStyle-HorizontalAlign="left" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdPay" runat="server"  Value='<%# Eval("PaymentType") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="group" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" ItemStyle-HorizontalAlign="left" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdPayDesc" runat="server"  Value='<%# Eval("PaymentDesc") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <%--<asp:BoundField DataField="PaymentDesc" HeaderText="Payment Type" ItemStyle-Width="200" />--%>
                            <asp:BoundField DataField="PosCode" HeaderText="Code" ItemStyle-Width="70"/>
                            <asp:BoundField DataField="description" HeaderText="Description" />
                            <asp:TemplateField HeaderText="POST TO GUEST ACCOUNT"  ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <div class="input-group" style="padding:0;">
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="lbPosCodeGL" CssClass="btn btn-default btn-border" runat="server" CommandName="getCOAGL" CommandArgument='<%#  Eval("PosCode") %>'><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                                        </div>
                                        <asp:TextBox Width="100px" ID="txtGLCoa" CssClass="form-control" style="text-align:left"  runat="server" Text='<%# Eval("GLCoa")  %>' />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CASH"  ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <div class="input-group" style="padding:0;">
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="lbPosCodeCS" CssClass="btn btn-default btn-border" runat="server" CommandName="getCOACS" CommandArgument='<%#  Eval("PosCode") %>'><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                                        </div>
                                        <asp:TextBox Width="100px" ID="txtCSCoa" CssClass="form-control" style="text-align:left"  runat="server" Text='<%# Eval("CSCoa")  %>' />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CARD"  ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <div class="input-group" style="padding:0;">
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="lbPosCodeCR" CssClass="btn btn-default btn-border" runat="server" CommandName="getCOACR" CommandArgument='<%#  Eval("PosCode") %>'><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                                        </div>
                                        <asp:TextBox Width="100px" ID="txtCRCoa" CssClass="form-control" style="text-align:left"  runat="server" Text='<%# Eval("CRCoa")  %>' />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CITY LEDGER"  ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <div class="input-group" style="padding:0;">
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="lbPosCodeCL" CssClass="btn btn-default btn-border" runat="server" CommandName="getCOACL" CommandArgument='<%#  Eval("PosCode") %>'><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                                        </div>
                                        <asp:TextBox Width="100px" ID="txtCLCoa" CssClass="form-control" style="text-align:left"  runat="server" Text='<%# Eval("CLCoa")  %>' />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WEB BOOKING"  ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <div class="input-group" style="padding:0;">
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="lbPosCodeWB" CssClass="btn btn-default btn-border" runat="server" CommandName="getCOAWB" CommandArgument='<%#  Eval("PosCode") %>'><i class="fa fa-search" style="font-size:20px;"></i></asp:LinkButton>
                                        </div>
                                        <asp:TextBox Width="100px" ID="txtWBCoa" CssClass="form-control" style="text-align:left"  runat="server" Text='<%# Eval("WBCoa")  %>' />
                                    </div>
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

