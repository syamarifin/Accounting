<%@ Page Language="VB" MasterPageFile="~/iPxDashboard/iPxDashboardUpload.master" CodeFile="UserAccess.aspx.vb" Inherits="iPxDashboard_UserAccess"title="Alcor  Dashboard"%>
<%---------------------------------------------------------------------------------------------------------------------%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="form-group">
    <div class="col-sm-12">
    <label for="" class="col-sm-2 text-right">Photo</label>
   <div class="table">
    <asp:Image ID="guestphoto" runat="server" Width="100px"  Height="100px" />
    </div>
    </div>
    
    <label for="" class="col-sm-2 text-right">Business Name <label style="color:red">*</label></label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" id="txtbsname" MaxLength="60"  runat="server" ></asp:textbox>
        <br/>
	</div>

    <label for="" class="col-sm-2 text-right">Address <label style="color:red">*</label></label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" id="txtaddrs" TextMode="MultiLine"  runat="server" ></asp:textbox>
        <br/>
	</div>

        <label for="" class="col-sm-2 text-right">Phone <label style="color:red">*</label></label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" id="txtphone" MaxLength="25"  runat="server" ></asp:textbox>
        <br/>
	</div>

        	<label for="" class="col-sm-2 text-right">Mobile No</label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" id="txtmobile" MaxLength="25" runat="server"></asp:textbox>
		<br/>
	</div>
   
        <label for="" class="col-sm-2 text-right">Contact Person</label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" id="txtconpers" MaxLength="50"  runat="server" ></asp:textbox>
        <br/>
	</div>

    <label for="" class="col-sm-2 text-right">Fax</label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" id="txtfax" MaxLength="25"  runat="server" ></asp:textbox>
        <br/>
	</div>

        <label for="" class="col-sm-2 text-right">Tax No</label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" id="txttaxno" MaxLength="25"  runat="server" ></asp:textbox>
        <br/>
	</div>

    <label for="" class="col-sm-2 text-right">Currancy</label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" id="txtcurrancy"  runat="server" MaxLength="3"></asp:textbox>
        <br/>
	</div>

    

     <label for="" class="col-sm-2 text-right">Country <label style="color:red">*</label></label>
	<div class="col-sm-4">
		<asp:DropDownList class="form-control padding-horizontal-15" id="drpCountry" AutoPostBack="true" MaxLength="4" runat="server"></asp:DropDownList>
		<br/>
	</div>


    <label for="" class="col-sm-2 text-right">City <label style="color:red">*</label></label>
	<div class="col-sm-4">
		<asp:DropDownList class="form-control padding-horizontal-15" id="drpCity" MaxLength="4" runat="server"></asp:DropDownList>
		<br/>
	</div>
	
	<label for="" class="col-sm-2 text-right">Email <label style="color:red">*</label></label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" id="txtusercode" MaxLength="60" runat="server"></asp:textbox>
                  <asp:RegularExpressionValidator ID="regexEmailValid0" runat="server" 
                       ControlToValidate="txtusercode" ErrorMessage="Invalid Email Format" 
                       Font-Size="X-Small" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                  </asp:RegularExpressionValidator>
		<br/>
	</div>


    <label for="" class="col-sm-2 text-right">Website URL</label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" id="txtweb"  runat="server" ></asp:textbox>
        <br/>
	</div>

    
     <label for="" class="col-sm-2 text-right">Promo URL</label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" id="txtpromo"  runat="server" ></asp:textbox>
        <br/>
	</div>
   
     <label for="" class="col-sm-2 text-right">Image Logo</label>
	<div class="col-sm-4">
        <asp:FileUpload class="form-control padding-horizontal-15"  ID="uploadphoto" runat="server" />
        <br/>
	</div>


    <label for="" class="col-sm-2 text-right">Active</label>
	<div class="col-sm-4">
        <asp:CheckBox ID="chkStatus" Checked="true" runat="server" />
		<br/>
	</div>
    
<br />
<br />

        
</asp:Content>
<asp:content id="Content3" contentplaceholderid="ContentPlaceHolder2" runat="Server">
<asp:linkbutton width="150px" cssclass="btn btn-default " data- id="btnsave" runat="server" Enabled="true"><span class="fa fa-save"></span> Save</asp:linkbutton>
<asp:LinkButton Width="210" CssClass ="btn btn-default" data- id="btnview" runat="server"><span class ="glyphicon glyphicon-list-alt"></span> View User Access</asp:LinkButton>
<asp:linkbutton width="150px" cssclass="btn btn-default " data- id="btnCxld" runat="server"><span class="fa fa-close"></span> Abort</asp:linkbutton>
    </asp:content>

