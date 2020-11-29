<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="defaultparameter.aspx.vb" Inherits="iPxAdmin_defaultparameter" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Add Customer Group Modal-->
    <div id="formInput" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <button type="button" data-dismiss="modal" aria-label="Close" class="close"><span aria-hidden="true">×</span></button>
                    <h4 id="login-modalLabel" class="modal-title">Form FO Link</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">Usercode:</label>
                        <asp:TextBox ID="tbUsercodeFO" runat="server" CssClass ="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">Password:</label>
                        <asp:TextBox ID="tbPasswordFO" runat="server" CssClass ="form-control" TextMode="Password"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbSaveFOLink" runat="server" CssClass="btn btn-default"><i class="fa fa-check-square-o"></i> Connect</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Add Customer Group modal end-->
    <div class="form-group">
        <table class="table table-bordered">
            <tr>
                <td>
		            <label for="" >Country</label>
                </td>
                <td>
                    <div class="row">
                        <div class="col-lg-5">
                            <asp:DropDownList class="form-control padding-horizontal-15" id="ddlCountry" AutoPostBack="true" MaxLength="4" runat="server"></asp:DropDownList>
                        </div>
                    </div>
		        </td>
            </tr>
            <tr>
                <td>
		            <label for="" >Nationality</label>
                </td>
                <td>
                    <div class="row">
                        <div class="col-lg-5">
                            <asp:DropDownList class="form-control padding-horizontal-15" id="ddlNationality" MaxLength="4" runat="server"></asp:DropDownList>
                        </div>
                    </div>
		        </td>
            </tr>
            <tr>
                <td>
		            <label for="" >Province</label>
                </td>
                <td>
                    <div class="row">
                        <div class="col-lg-5">
                            <asp:DropDownList class="form-control padding-horizontal-15" id="ddlProvince" MaxLength="4" AutoPostBack="true" runat="server"></asp:DropDownList>
                        </div>
                    </div>
    		    </td>
            </tr>
            <tr>
                <td>
		            <label for="" >City</label>
                </td>
                <td>
                    <div class="row">
                        <div class="col-lg-5">
                            <asp:DropDownList class="form-control padding-horizontal-15" id="ddlCity" MaxLength="4" runat="server"></asp:DropDownList>
                        </div>
                    </div>
		        </td>
            </tr>
            <tr>
                <td>
		            <label for="" >PMS Link</label>
                </td>
                <td>
                    <div class="row">
                        <div class="col-lg-5">
			                <asp:DropDownList class="form-control padding-horizontal-15" id="ddlPMSlink" MaxLength="4" runat="server"></asp:DropDownList>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
		            <label for="" >AR Transaction Type</label>
                </td>
                <td>
                    <div class="row">
                        <div class="col-lg-5">
                            <asp:DropDownList class="form-control padding-horizontal-15" id="dlTransType" MaxLength="4" runat="server"></asp:DropDownList>
                        </div>
                    </div>
		        </td>
            </tr>
            <tr>
                <td>
		            <label for="" >AP Transaction Type</label>
                </td>
                <td>
                    <div class="row">
                        <div class="col-lg-5">
                            <asp:DropDownList class="form-control padding-horizontal-15" id="dlTransTypeAP" MaxLength="4" runat="server"></asp:DropDownList>
                        </div>
                    </div>
		        </td>
            </tr>
        </table>
    </div>
    
<br />
<br />

        
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
<div id="footer"  >
<asp:linkbutton width="150px" cssclass="btn btn-default " data- id="Linkbutton1" runat="server" Enabled="true"><span class="fa fa-save"></span> Save</asp:linkbutton>    
<asp:linkbutton width="150px" cssclass="btn btn-default " data- id="btnCxld" runat="server"><span class="fa fa-close"></span> Abort</asp:linkbutton>
</div>
</asp:content>

