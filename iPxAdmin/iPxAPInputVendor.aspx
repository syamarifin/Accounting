<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxAPInputVendor.aspx.vb" Inherits="iPxAdmin_iPxAPInputVendor" title="Alcor Accounting Input Vendor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .checkout-wrapper{padding-top: 40px; padding-bottom:40px; background-color: #fafbfa;}
        .checkout{    background-color: #fff;
            border:1px solid #eaefe9;
             
            font-size: 14px;}
        .panel{margin-bottom: 0px;}
        .checkout-step {
             
            border-top: 1px solid #f2f2f2;
            color: #666;
            font-size: 14px;
            padding: 20px;
            position: relative;
        }

        .checkout-step-number {
            border-radius: 50%;
            border: 1px solid #666;
            display: inline-block;
            font-size: 12px;
            height: 32px;
            margin-right: 26px;
            padding: 6px;
            text-align: center;
            width: 32px;
        }
        .checkout-step-title{ font-size: 18px;
            font-weight: 500;
            vertical-align: middle;display: inline-block; margin: 0px;
             }
         
        .checout-address-step{}
        .checout-address-step .form-group{margin-bottom: 18px;display: inline-block;
            width: 100%;}

        .checkout-step-body{padding-left: 60px; padding-top: 30px;}

        .checkout-step-active{display: block;}
        .checkout-step-disabled{display: none;}

        .checkout-login{}
        .login-phone{display: inline-block;}
        .login-phone:after {
            content: '+91 - ';
            font-size: 14px;
            left: 36px;
        }
        .login-phone:before {
            content: "";
            font-style: normal;
            color: #333;
            font-size: 18px;
            left: 12px;
                display: inline-block;
            font: normal normal normal 14px/1 FontAwesome;
            font-size: inherit;
            text-rendering: auto;
            -webkit-font-smoothing: antialiased;
            -moz-osx-font-smoothing: grayscale;
        }
        .login-phone:after, .login-phone:before {
            position: absolute;
            top: 50%;
            -webkit-transform: translateY(-50%);
            transform: translateY(-50%);
        }
        .login-phone .form-control {
            padding-left: 68px;
            font-size: 14px;
            
        }
        .checkout-login .btn{height: 42px;     line-height: 1.8;}
         
        .otp-verifaction{margin-top: 30px;}
        .checkout-sidebar{background-color: #fff;
            border:1px solid #eaefe9; padding: 30px; margin-bottom: 30px;}
        .checkout-sidebar-merchant-box{background-color: #fff;
            border:1px solid #eaefe9; margin-bottom: 30px;}
        .checkout-total{border-bottom: 1px solid #eaefe9; padding-bottom: 10px;margin-bottom: 10px; }
        .checkout-invoice{display: inline-block;
            width: 100%;}
        .checout-invoice-title{    float: left; color: #30322f;}
        .checout-invoice-price{    float: right; color: #30322f;}
        .checkout-charges{display: inline-block;
            width: 100%;}
        .checout-charges-title{float: left; }
        .checout-charges-price{float: right;}
        .charges-free{color: #43b02a; font-weight: 600;}
        .checkout-payable{display: inline-block;
            width: 100%; color: #333;}
        .checkout-payable-title{float: left; }
        .checkout-payable-price{float: right;}

        .checkout-cart-merchant-box{ padding: 20px;display: inline-block;width: 100%; border-bottom: 1px solid #eaefe9;
         padding-bottom: 20px; }
        .checkout-cart-merchant-name{color: #30322f; float: left;}
        .checkout-cart-merchant-item{ float: right; color: #30322f; }
        .checkout-cart-products{}

        .checkout-cart-products .checkout-charges{ padding: 10px 20px;
            color: #333;}
        .checkout-cart-item{ border-bottom: 1px solid #eaefe9;
            box-sizing: border-box;
            display: table;
            font-size: 12px;
            padding: 22px 20px;
            width: 100%;}
         .checkout-item-list{}
        .checkout-item-count{ float: left; }
        .checkout-item-img{width: 60px; float: left;}
        .checkout-item-name-box{ float: left; }
        .checkout-item-title{ color: #30322f; font-size: 14px;  }
        .checkout-item-unit{  }
        .checkout-item-price{float: right;color: #30322f; font-size: 14px; font-weight: 600;}


        .checkout-viewmore-btn{padding: 10px; text-align: center;}

        .header-checkout-item{text-align: right; padding-top: 20px;}
        .checkout-promise-item {
            background-repeat: no-repeat;
            background-size: 14px;
            display: inline-block;
            margin-left: 20px;
            padding-left: 24px;
            color: #30322f;
        }
        .checkout-promise-item i{padding-right: 10px;color: #43b02a;}
    </style>
    <script>
        function collapseOne() {
           $("#collapseOne").addClass("in");
           $("#collapseTwo").removeClass("in");
           $("#collapseThree").removeClass("in");
           $("#collapseFour").removeClass("in");
        };
        function collapseTwo() {
           $("#collapseOne").removeClass("in");
           $("#collapseTwo").addClass("in");
           $("#collapseThree").removeClass("in");
           $("#collapseFour").removeClass("in");
        };
        function collapseFour() {
           $("#collapseOne").removeClass("in");
           $("#collapseTwo").removeClass("in");
           $("#collapseThree").removeClass("in");
           $("#collapseFour").addClass("in");
        };
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
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Add AR Group Modal-->
    <div id="formInput" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <button type="button" data-dismiss="modal" aria-label="Close" class="close"><span aria-hidden="true">×</span></button>
                    <h4 id="login-modalLabel" class="modal-title">Form AP Group</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="usr">Description:</label><font color=red>*</font>
                        <asp:TextBox ID="tbDescription" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <div class="square-blue pull-left pv-15">
                            <label class="ui-checkbox">
                            <asp:CheckBox  ID="cbActiveARGroup" runat="server"  />
                            <span style=""> <strong> Active </strong> </span> </label>
                        </div>
                    </div>
                    <br /><br />
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbSaveARGroup" runat="server" CssClass="btn btn-default"><i class="fa fa-save"></i> Save</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Add AR Group modal end-->
    <!-- Add COA Modal-->
    <div id="formListCOA" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md" style="width:1000px;">
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
                                            <asp:LinkButton ID="lbSelect" CssClass="btn btn-default" runat="server" CommandName="getSelect" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' CommandArgument='<%# Eval("Coa") %>'><i class="fa fa-check"></i></asp:LinkButton>
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
    <div id="accordion" class="checkout">
        <div class="panel checkout-step">
            <div> <span class="checkout-step-number">1</span>
                <h4 class="checkout-step-title"> <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" > AP Vendor 
                    <asp:Label ID="lbCustID" runat="server" Text=""></asp:Label></a></h4>
            </div>
            <div id="collapseOne" class="collapse in">
                <div class="checkout-step-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="checkout-login">
                                <div class="row">
                                    <div class="col-lg-6">
                                        <div class="form-group" style="margin-bottom:3px;">
                                            <label for="usr">AP Group:</label><font color=red>*</font>
                                            <div class="input-group">
                                                <asp:DropDownList ID="dlArgroup" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged ="dlArgroup_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <div class="input-group-btn">
                                                    <asp:LinkButton ID="lbAddAR" runat="server" CssClass="btn btn-default" style="height:34px;"><i class="fa fa-plus" style="font-size:18px;"></i></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group" style="margin-bottom:3px;">
                                            <label for="usr">Coy Name:</label><font color=red>*</font>
                                            <asp:TextBox ID="tbCoyName" runat="server" CssClass ="form-control"></asp:TextBox>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-6">
                                                <div class="form-group" style="margin-bottom:3px;">
                                                    <label for="usr">Phone:</label><font color=red>*</font>
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-phone"></i></span>
                                                        <asp:TextBox ID="tbPhone" runat="server" CssClass ="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group" style="margin-bottom:3px;">
                                                    <label for="usr">Mobile:</label>
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-mobile-phone" style="font-size:20px"></i></span>
                                                        <asp:TextBox ID="tbMobile" runat="server" CssClass ="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-6">
                                                <div class="form-group" style="margin-bottom:3px;">
                                                    <label for="usr">Fax:</label>
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-fax" style="font-size:12px"></i></span>
                                                        <asp:TextBox ID="tbFax" runat="server" CssClass ="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group" style="margin-bottom:3px;">
                                                    <label for="usr">Tax No:</label>
                                                    <asp:TextBox ID="tbTax" runat="server" CssClass ="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6">
                                        <div class="form-group" style="margin-bottom:3px;">
                                            <label for="usr">Email:</label><font color=red>*</font>
                                            <asp:TextBox ID="tbEmail" runat="server" CssClass ="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group" style="margin-bottom:3px;">
                                            <label for="usr">Web:</label>
                                            <asp:TextBox ID="tbWeb" runat="server" CssClass ="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group" style="margin-bottom:3px;">
                                            <label for="usr">Credit Limit:</label>
                                            <div class="input-group">
                                                <span class="input-group-addon">Rp.</span>
                                                <asp:TextBox ID="tbCreditLimit" runat="server" CssClass ="form-control" style="text-align: right"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group" style="margin-bottom:3px;">
                                            <label for="usr">Notes:</label>
                                            <asp:TextBox ID="tbNotes" runat="server" CssClass ="form-control" TextMode="MultiLine" Height="97"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <a class="collapsed btn btn-default" role="button" href="iPxAPVendor.aspx"> <i class="fa fa-close"></i> Abort </a>
                                <a class="collapsed btn btn-default" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo"> Next <i class="fa fa-arrow-right"></i> </a>
                                <!-- add class disabled to inactive button -->
                            </div>
                            <!-- /input-group -->
                        </div>
                        <!-- /.col-lg-6 -->
                    </div>
                </div>
            </div>
        </div>
        <div class="panel checkout-step">
            <div role="tab" id="headingTwo"> <span class="checkout-step-number">2</span>
                <h4 class="checkout-step-title"> <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" > Vendor Address </a> </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse">
                <div class="checkout-step-body">
                    <div class="checout-address-step">
                        <div class="row">
                            <div class="col-lg-6">
                                <div class="form-group" style="margin-bottom:3px;">
                                    <label for="usr">Country:</label><font color=red>*</font>
                                    <asp:DropDownList ID="dlCountry" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlCountry_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group" style="margin-bottom:3px;">
                                    <label for="usr">Province:</label><font color=red>*</font>
                                    <asp:DropDownList ID="dlProvince" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlProvince_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group" style="margin-bottom:3px;">
                                    <label for="usr">City:</label><font color=red>*</font>
                                    <asp:DropDownList ID="dlCity" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-6">
                                <div class="form-group" style="margin-bottom:3px;">
                                    <label for="usr">Address:</label><font color=red>*</font>
                                    <asp:TextBox ID="tbAddress" runat="server" CssClass ="form-control" TextMode="MultiLine" Height="70"></asp:TextBox>
                                </div>
                                <div class="form-group" style="margin-bottom:3px;">
                                    <label for="usr">Billing Address:</label><font color=red>*</font>
                                    <asp:TextBox ID="tbBilling" runat="server" CssClass ="form-control" TextMode="MultiLine" Height="60"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <a class="collapsed btn btn-default" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne"><i class="fa fa-arrow-left"></i> Previous </a>
                    <a class="collapsed btn btn-default" role="button" href="iPxAPVendor.aspx"> <i class="fa fa-close"></i> Abort </a>
                    <a class="collapsed btn btn-default" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseThree"> Next <i class="fa fa-arrow-right"></i> </a>
                </div>
            </div>
        </div>
        <div class="panel checkout-step">
            <div role="tab" id="headingThree"> <span class="checkout-step-number">3</span>
                <h4 class="checkout-step-title"> <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseThree"  > Contact Person </a> </h4>
            </div>
            <div id="collapseThree" class="panel-collapse collapse">
                <div class="checkout-step-body">
                    <div class="row">
                        <div class="col-lg-6">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Name:</label>
                                <asp:TextBox ID="tbContPerson" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Position:</label>
                                <asp:TextBox ID="tbContPosition" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Address:</label>
                                <asp:TextBox ID="tbContAdress" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Phone:</label>
                                <asp:TextBox ID="tbContPhone" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Mobile:</label>
                                <asp:TextBox ID="tbContMobile" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Email:</label>
                                <asp:TextBox ID="tbContEmail" runat="server" CssClass ="form-control"></asp:TextBox>
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
                    <a class="collapsed btn btn-default" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo"><i class="fa fa-arrow-left"></i> Previous </a>
                    <a class="collapsed btn btn-default" role="button" href="iPxAPVendor.aspx"> <i class="fa fa-close"></i> Abort </a>
                    <a class="collapsed btn btn-default" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseFour"> Next <i class="fa fa-arrow-right"></i> </a>
                </div>
            </div>
        </div>
        <div class="panel checkout-step">
            <div role="tab" id="headingFour"> <span class="checkout-step-number">4</span>
                <h4 class="checkout-step-title"> <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseFour"  > Account Mapping </a> </h4>
            </div>
            <div id="collapseFour" class="panel-collapse collapse">
                <div class="checkout-step-body">
                    <asp:Panel ID="lbNotifMaping" runat="server" Visible="false">
                    <asp:Label ID="lbNotif" runat="server" Text="Please select ar group first!"></asp:Label>
                    <asp:LinkButton ID="lbMoveAR" runat="server" CssClass="btn btn-link"> Select AP Group </asp:LinkButton>
                    </asp:Panel>
                    <asp:Panel ID="pnCompany" runat="server" Visible="false">
                    <div class="row">
                        <div class="col-lg-4">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Coa Link:</label>
                                <div class="input-group">
                                    <asp:TextBox ID="tbCoaLink" runat="server" CssClass ="form-control"></asp:TextBox>
                                    <div class="input-group-btn">
                                        <asp:LinkButton ID="lbSearchCOA" runat="server" CssClass="btn btn-default" style="height:34px;"><i class="fa fa-search" style="font-size:18px;"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-4">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr" style="display:none;">Inventory Link:</label>
                                <asp:DropDownList ID="dlFOName" runat="server" CssClass="form-control" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="dlFOName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-4">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr" style="display:none;">Company/Agent:</label>
                                <asp:DropDownList ID="dlProfileCode" runat="server" CssClass="form-control"  Visible="false">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    </asp:Panel>
                    <asp:Panel ID="pnCredit" runat="server" Visible="false">
                        <div class="row">
                            <div class="col-lg-4">
                                <asp:DropDownList ID="dlCreditCard" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                                
                                
                            </div>
                        </div>
                    </asp:Panel>
                    <hr />
                    <a class="collapsed btn btn-default" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseThree"><i class="fa fa-arrow-left"></i> Previous </a>
                    <a class="collapsed btn btn-default" role="button" href="iPxAPVendor.aspx"> <i class="fa fa-close"></i> Abort </a>
                    <asp:LinkButton ID="lbSave" runat="server" CssClass="btn btn-default"><i class="fa fa-save"></i> Save </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

