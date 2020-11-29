<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="SetupSMTP.aspx.vb" Inherits="iPxAdmin_SetupSMTP" title="Alcor Accounting" %>
<%---------------------------------------------------------------------------------------------------------------------%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

   
<div class="form-group">

    <label for="" class="col-sm-2 text-right">Host <label style="color:red">*</label></label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" id="txthost" runat="server"></asp:textbox>
		<br/>
	</div>
	
	 <label for="" class="col-sm-2 text-right">EnableSsl <label style="color:red">*</label></label>
	<div class="col-sm-4">
		<%--<asp:textbox class="form-control padding-horizontal-15" id="txtEnableSsl" runat="server"></asp:textbox>--%>
        <asp:DropDownList class="form-control padding-horizontal-15" ID="drpEnableSsl" runat="server" >
            <asp:ListItem Value="True">True</asp:ListItem>
            <asp:ListItem Value="False">False</asp:ListItem>
         </asp:DropDownList>
		
		<br/>
	</div>
	
	<label for="" class="col-sm-2 text-right">UserName <label style="color:red">*</label></label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" id="txtusername" runat="server"></asp:textbox>
		<br/>
	</div>

    <label for="" class="col-sm-2 text-right">Password <label style="color:red">*</label></label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" id="tbpassword" runat="server" TextMode="Password" ></asp:textbox>
		<br/>
	</div>
	
    <label for="" class="col-sm-2 text-right">Port <label style="color:red">*</label></label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" id="txtport" MaxLength="4" runat="server"></asp:textbox>
		<br/>
	</div>

    <label for="" class="col-sm-2 text-right">Reconfirm Password <label style="color:red">*</label></label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" id="tbpasswordConfirm" runat="server" TextMode="Password" ></asp:textbox>
		<br/>
	</div>


    <label for="" class="col-sm-2 text-right">Active</label>
	<div class="col-sm-4">
        <asp:CheckBox ID="CheckBox1"  runat="server" />
        <br/>
	</div>

    
  

</div>
    
<br />
<br />

        
</asp:Content>
<asp:content id="Content3" contentplaceholderid="ContentPlaceHolder2" runat="Server">
    <div id="footer"  >
        <asp:linkbutton width="150px" cssclass="btn btn-default " data- id="Linkbutton1" runat="server" Enabled="true"><span class="fa fa-save"></span> Save</asp:linkbutton>    
        <asp:linkbutton width="150px" cssclass="btn btn-default " data- id="btnCxld" runat="server"><span class="fa fa-close"></span> Abort</asp:linkbutton>
    </div>
</asp:content>

