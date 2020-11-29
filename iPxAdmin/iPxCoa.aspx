<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPageUpload.master" AutoEventWireup="false" CodeFile="iPxCoa.aspx.vb" Inherits="iPxAdmin_iPxCoa" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        function AssetActive()
        {
            $("#Asset-tab").addClass("active");
            $("#Liability-tab").removeClass("active");
            $("#Equity-tab").removeClass("active");
            $("#Revenue-tab").removeClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Statistic-tab").removeClass("active");
            $("#Asset").addClass("active in");
            $("#Liability").removeClass("active in");
            $("#Equity").removeClass("active in");
            $("#Revenue").removeClass("active in");
            $("#Cost").removeClass("active in");
            $("#Statistic").removeClass("active in");
        } 
        function LiabilityActive()
        {
            $("#Asset-tab").removeClass("active");
            $("#Liability-tab").addClass("active");
            $("#Equity-tab").removeClass("active");
            $("#Revenue-tab").removeClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Statistic-tab").removeClass("active");
            $("#Asset").removeClass("active in");
            $("#Liability").addClass("active in");
            $("#Equity").removeClass("active in");
            $("#Revenue").removeClass("active in");
            $("#Cost").removeClass("active in");
            $("#Statistic").removeClass("active in");
        }
        function EquityActive()
        {
            $("#Asset-tab").removeClass("active");
            $("#Liability-tab").removeClass("active");
            $("#Equity-tab").addClass("active");
            $("#Revenue-tab").removeClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Statistic-tab").removeClass("active");
            $("#Asset").removeClass("active in");
            $("#Liability").removeClass("active in");
            $("#Equity").addClass("active in");
            $("#Revenue").removeClass("active in");
            $("#Cost").removeClass("active in");
            $("#Statistic").removeClass("active in");
        }
        function RevenueActive()
        {
            $("#Asset-tab").removeClass("active");
            $("#Liability-tab").removeClass("active");
            $("#Equity-tab").removeClass("active");
            $("#Revenue-tab").addClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Statistic-tab").removeClass("active");
            $("#Asset").removeClass("active in");
            $("#Liability").removeClass("active in");
            $("#Equity").removeClass("active in");
            $("#Revenue").addClass("active in");
            $("#Cost").removeClass("active in");
            $("#Statistic").removeClass("active in");
        }
        function CostActive()
        {
            $("#Asset-tab").removeClass("active");
            $("#Liability-tab").removeClass("active");
            $("#Equity-tab").removeClass("active");
            $("#Revenue-tab").removeClass("active");
            $("#Cost-tab").addClass("active");
            $("#Statistic-tab").removeClass("active");
            $("#Asset").removeClass("active in");
            $("#Liability").removeClass("active in");
            $("#Equity").removeClass("active in");
            $("#Revenue").removeClass("active in");
            $("#Cost").addClass("active in");
            $("#Statistic").removeClass("active in");
        }
        function StatisticActive()
        {
            $("#Asset-tab").removeClass("active");
            $("#Liability-tab").removeClass("active");
            $("#Equity-tab").removeClass("active");
            $("#Revenue-tab").removeClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Statistic-tab").addClass("active");
            $("#Asset").removeClass("active in");
            $("#Liability").removeClass("active in");
            $("#Equity").removeClass("active in");
            $("#Revenue").removeClass("active in");
            $("#Cost").removeClass("active in");
            $("#Statistic").addClass("active in");
        }

        function hideModalAddDev() {
            $('#formInputDev').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalAddDev() {
            $('#formInputDev').modal({ backdrop: 'static',
                keyboard: false
            },'show');
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
            },'show');
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
            },'show');
        }

        
        function hideModalAddCoa() {
            $('#formInputCoa').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalAddCoa() {
            $('#formInputCoa').modal({ backdrop: 'static',
                keyboard: false
            },'show');
        }
        
        function hideModalImport() {
            $('#formImport').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalImport() {
            $('#formImport').modal({ backdrop: 'static',
                keyboard: false
            },'show');
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
                    <h4 id="H2" class="modal-title">Form Division </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr">Division ID:</label><font color=red>*</font>
                                <asp:TextBox ID="tbDevID" runat="server" CssClass ="form-control" MaxLength="2"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Description:</label><font color=red>*</font>
                                <asp:TextBox ID="tbDescDiv" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
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
                    <h4 id="H3" class="modal-title">Form Departement </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr">Division:</label><font color=red>*</font>
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
                    <h4 id="H4" class="modal-title">Form Sub Departement </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr">Division:</label><font color=red>*</font>
                                <asp:TextBox ID="tbSubDev" runat="server" CssClass ="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Departement:</label><font color=red>*</font>
                                <asp:TextBox ID="tbSubDept" runat="server" CssClass ="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">ID:</label><font color=red>*</font>
                                <asp:TextBox ID="tbSubID" runat="server" CssClass ="form-control" MaxLength="4"></asp:TextBox>
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
    <!-- Add COA Modal-->
    <div id="formInputCoa" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm" style="width:550px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortAdd" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="login-modalLabel" class="modal-title">Form COA </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr">COA:</label><font color=red>*</font>
                                <asp:TextBox ID="tbCoa" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-group">
                                <label for="usr">Description:</label><font color=red>*</font>
                                <asp:TextBox ID="tbCoaDesc" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Type:</label><font color=red>*</font>
                                <asp:DropDownList ID="dlType" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="usr">Group Level:</label><font color=red>*</font>
                                <asp:DropDownList ID="dlGrpLevel" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlGrpLevel_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="usr">Level:</label><font color=red>*</font>
                                <asp:DropDownList ID="dlLevel" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlLevel_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="usr">Status:</label><font color=red>*</font>
                                <asp:DropDownList ID="dlStatus" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-group">
                                <label for="usr">Division:</label>
                                <div class="input-group" style="padding:0;">
                                    <asp:DropDownList ID="dlDevision" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlDevision_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <div class="input-group-btn">
                                        <asp:LinkButton ID="lbAddDivision" runat="server" CssClass="btn btn-default"><i class="fa fa-plus" style="font-size:20px;"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="usr">Departement:</label>
                                <div class="input-group" style="padding:0;">
                                    <asp:DropDownList ID="dlDepartement" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlDepartement_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <div class="input-group-btn">
                                        <asp:LinkButton ID="lbAddDept" runat="server" CssClass="btn btn-default" Enabled="false"><i class="fa fa-plus" style="font-size:20px;"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="usr">Sub Departement:</label>
                                <div class="input-group" style="padding:0;">
                                    <asp:DropDownList ID="dlSubDepartement" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                    <div class="input-group-btn">
                                        <asp:LinkButton ID="lbAddSubDept" runat="server" CssClass="btn btn-default" Enabled="false"><i class="fa fa-plus" style="font-size:20px;"></i></asp:LinkButton>
                                    </div>
                                </div>
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
                                    <asp:CheckBox  ID="cbActiveCoa" runat="server"  />
                                    <span > <strong> Active </strong> </span> </label>
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
    <!-- Add COA modal end-->
    
    <!-- Query COA Modal-->
    <div id="formQuery" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm" style="width:400px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortQuery" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H1" class="modal-title">Query COA </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr">COA:</label>
                                <asp:TextBox ID="tbQCoa" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Group Level:</label>
                                <asp:DropDownList ID="dlQGrpLevel" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="usr">Level:</label>
                                <asp:DropDownList ID="dlQLevel" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr">Division:</label>
                                <asp:DropDownList ID="dlQDevision" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlQDevision_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="usr">Departement:</label>
                                <asp:DropDownList ID="dlQDepartement" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlQDepartement_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="usr">Sub Departement:</label>
                                <asp:DropDownList ID="dlQSubDepartement" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="usr">Status:</label>
                                <asp:DropDownList ID="dlQStatus" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="usr">Status isActive:</label>
                                <asp:DropDownList ID="dlQactive" runat="server" CssClass="form-control">
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
    <!-- Query COA modal end-->
    
    <!-- Import Modal -->
    <div id="formImport" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortImport" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H5" class="modal-title">Import COA(.xslx) </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <asp:FileUpload ID="flMassUpoad" runat="server" accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"/>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbStartImport" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> Import</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Import Modal end-->
    
    <div class="row">
        <div class="col-lg-12">
            <asp:LinkButton ID="lbAddCoa" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> New COA</asp:LinkButton>
            <asp:LinkButton ID="lbQuery" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
            <asp:LinkButton ID="lbPrint" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-print"></i> Print</asp:LinkButton>
            <asp:LinkButton ID="lbImport" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> Import From Excel</asp:LinkButton><hr style="margin-top:5px; margin-bottom:5px;" />
        </div>
        <div class="col-lg-12">
            <ul class="nav nav-tabs">
                <li id="Asset-tab" class="active">  <a href="#Asset" data-toggle="tab"><i class="fa fa-list"></i> Asset&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a></li>
                <li id="Liability-tab">             <a href="#Liability" data-toggle="tab"><i class="fa fa-list"></i> Liability&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a></li>
                <li id="Equity-tab">                <a href="#Equity" data-toggle="tab"><i class="fa fa-list"></i> Equity&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a></li>
                <li id="Revenue-tab">               <a href="#Revenue" data-toggle="tab"><i class="fa fa-list"></i> Revenue&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a></li>
                <li id="Cost-tab">                  <a href="#Cost" data-toggle="tab"><i class="fa fa-list"></i> Cost & Expenses</a></li>
                <li id="Statistic-tab">             <a href="#Statistic" data-toggle="tab"><i class="fa fa-list"></i> Statistic&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a></li>
            </ul>
            <div id="myTabContent" class="tab-content">
                <div class="tab-pane active in" id="Asset">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvCoaAsset" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
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
                            <asp:BoundField ItemStyle-Width="100px" DataField="Devision" HeaderText="Division" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Departement" HeaderText="Departement" />
                            <asp:BoundField ItemStyle-Width="120px" DataField="SubDepartement" HeaderText="Sub Departement" />
                            <asp:BoundField ItemStyle-Width="120px" DataField="status" HeaderText="Status" />
                            <asp:BoundField DataField="notes" HeaderText="Note" />
                            <asp:TemplateField ItemStyle-Width="70px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbEdit" CssClass="btn btn-default" runat="server" CommandName="getEdit" CommandArgument='<%# Eval("Coa") %>' Enabled='<%# if(Eval("editGLConf").toString()="Y","true","false") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="Liability">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvLia" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="50px" DataField="Coa" HeaderText="Coa" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Description" HeaderText="Description" />
                            <asp:BoundField ItemStyle-Width="70px" DataField="type" HeaderText="Type" />
                            <asp:TemplateField ItemStyle-Width="60px" HeaderText="Group Level">
                                <ItemTemplate>
                                    <asp:Label ID="lbGrpLevelLia" runat="server" Text='<%# if(Eval("grpLevel").toString()="G","Group","Detail") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="Level">
                                <ItemTemplate>
                                    <asp:Label ID="lbLevelLia" runat="server" Text='<%# if(Eval("levelid").toString()="1","Account type",if(Eval("levelid").toString()="2","Account Group",if(Eval("levelid").toString()="3","Account Sub Group 1",if(Eval("levelid").toString()="4","Account Sub Group 1","General")))) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-Width="100px" DataField="Devision" HeaderText="Division" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Departement" HeaderText="Departement" />
                            <asp:BoundField ItemStyle-Width="120px" DataField="SubDepartement" HeaderText="Sub Departement" />
                            <asp:BoundField ItemStyle-Width="120px" DataField="status" HeaderText="Status" />
                            <asp:BoundField DataField="notes" HeaderText="Note" />
                            <asp:TemplateField ItemStyle-Width="70px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbEditLia" CssClass="btn btn-default" runat="server" CommandName="getEdit" CommandArgument='<%# Eval("Coa") %>' Enabled='<%# if(Eval("editGLConf").toString()="Y","true","false") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="Equity">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvEquity" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="50px" DataField="Coa" HeaderText="Coa" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Description" HeaderText="Description" />
                            <asp:BoundField ItemStyle-Width="70px" DataField="type" HeaderText="Type" />
                            <asp:TemplateField ItemStyle-Width="60px" HeaderText="Group Level">
                                <ItemTemplate>
                                    <asp:Label ID="lbGrpLevelEquity" runat="server" Text='<%# if(Eval("grpLevel").toString()="G","Group","Detail") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="Level">
                                <ItemTemplate>
                                    <asp:Label ID="lbLevelEquity" runat="server" Text='<%# if(Eval("levelid").toString()="1","Account type",if(Eval("levelid").toString()="2","Account Group",if(Eval("levelid").toString()="3","Account Sub Group 1",if(Eval("levelid").toString()="4","Account Sub Group 1","General")))) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-Width="100px" DataField="Devision" HeaderText="Division" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Departement" HeaderText="Departement" />
                            <asp:BoundField ItemStyle-Width="120px" DataField="SubDepartement" HeaderText="Sub Departement" />
                            <asp:BoundField ItemStyle-Width="120px" DataField="status" HeaderText="Status" />
                            <asp:BoundField DataField="notes" HeaderText="Note" />
                            <asp:TemplateField ItemStyle-Width="70px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbEditEquity" CssClass="btn btn-default" runat="server" CommandName="getEdit" CommandArgument='<%# Eval("Coa") %>' Enabled='<%# if(Eval("editGLConf").toString()="Y","true","false") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="Revenue">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvRev" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="50px" DataField="Coa" HeaderText="Coa" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Description" HeaderText="Description" />
                            <asp:BoundField ItemStyle-Width="70px" DataField="type" HeaderText="Type" />
                            <asp:TemplateField ItemStyle-Width="60px" HeaderText="Group Level">
                                <ItemTemplate>
                                    <asp:Label ID="lbGrpLevelRev" runat="server" Text='<%# if(Eval("grpLevel").toString()="G","Group","Detail") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="Level">
                                <ItemTemplate>
                                    <asp:Label ID="lbLevelRev" runat="server" Text='<%# if(Eval("levelid").toString()="1","Account type",if(Eval("levelid").toString()="2","Account Group",if(Eval("levelid").toString()="3","Account Sub Group 1",if(Eval("levelid").toString()="4","Account Sub Group 1","General")))) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-Width="100px" DataField="Devision" HeaderText="Division" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Departement" HeaderText="Departement" />
                            <asp:BoundField ItemStyle-Width="120px" DataField="SubDepartement" HeaderText="Sub Departement" />
                            <asp:BoundField ItemStyle-Width="120px" DataField="status" HeaderText="Status" />
                            <asp:BoundField DataField="notes" HeaderText="Note" />
                            <asp:TemplateField ItemStyle-Width="70px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbEditRev" CssClass="btn btn-default" runat="server" CommandName="getEdit" CommandArgument='<%# Eval("Coa") %>' Enabled='<%# if(Eval("editGLConf").toString()="Y","true","false") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="Cost">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvCost" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="50px" DataField="Coa" HeaderText="Coa" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Description" HeaderText="Description" />
                            <asp:BoundField ItemStyle-Width="70px" DataField="type" HeaderText="Type" />
                            <asp:TemplateField ItemStyle-Width="60px" HeaderText="Group Level">
                                <ItemTemplate>
                                    <asp:Label ID="lbGrpLevelCost" runat="server" Text='<%# if(Eval("grpLevel").toString()="G","Group","Detail") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="Level">
                                <ItemTemplate>
                                    <asp:Label ID="lbLevelCost" runat="server" Text='<%# if(Eval("levelid").toString()="1","Account type",if(Eval("levelid").toString()="2","Account Group",if(Eval("levelid").toString()="3","Account Sub Group 1",if(Eval("levelid").toString()="4","Account Sub Group 1","General")))) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-Width="100px" DataField="Devision" HeaderText="Division" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Departement" HeaderText="Departement" />
                            <asp:BoundField ItemStyle-Width="120px" DataField="SubDepartement" HeaderText="Sub Departement" />
                            <asp:BoundField ItemStyle-Width="120px" DataField="status" HeaderText="Status" />
                            <asp:BoundField DataField="notes" HeaderText="Note" />
                            <asp:TemplateField ItemStyle-Width="70px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbEditCost" CssClass="btn btn-default" runat="server" CommandName="getEdit" CommandArgument='<%# Eval("Coa") %>' Enabled='<%# if(Eval("editGLConf").toString()="Y","true","false") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="Statistic">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvStatistic" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="50px" DataField="Coa" HeaderText="Coa" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Description" HeaderText="Description" />
                            <asp:BoundField ItemStyle-Width="70px" DataField="type" HeaderText="Type" />
                            <asp:TemplateField ItemStyle-Width="60px" HeaderText="Group Level">
                                <ItemTemplate>
                                    <asp:Label ID="lbGrpLevelStatistic" runat="server" Text='<%# if(Eval("grpLevel").toString()="G","Group","Detail") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="Level">
                                <ItemTemplate>
                                    <asp:Label ID="lbLevelStatistic" runat="server" Text='<%# if(Eval("levelid").toString()="1","Account type",if(Eval("levelid").toString()="2","Account Group",if(Eval("levelid").toString()="3","Account Sub Group 1",if(Eval("levelid").toString()="4","Account Sub Group 1","General")))) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-Width="100px" DataField="Devision" HeaderText="Division" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Departement" HeaderText="Departement" />
                            <asp:BoundField ItemStyle-Width="120px" DataField="SubDepartement" HeaderText="Sub Departement" />
                            <asp:BoundField ItemStyle-Width="120px" DataField="status" HeaderText="Status" />
                            <asp:BoundField DataField="notes" HeaderText="Note" />
                            <asp:TemplateField ItemStyle-Width="70px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbEditStatistic" CssClass="btn btn-default" runat="server" CommandName="getEdit" CommandArgument='<%# Eval("Coa") %>' Enabled='<%# if(Eval("editGLConf").toString()="Y","true","false") %>'><i class="fa fa-edit"></i></asp:LinkButton>
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

