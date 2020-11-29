<%@ Page Language="VB" MasterPageFile="~/iPxDashboard/iPxDashboard.master" EnableEventValidation="false" AutoEventWireup="false" CodeFile="releasenotedtl.aspx.vb" Inherits="iPxDashboard_releasenotedtl" title="Alcor Dashboard"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

   
<div class="form-group">
    
<div class="col-sm-11">
    <label for="" class="col-sm-2 text-right">APP </label>
<div class="col-sm-4">
        <asp:DropDownList Enabled="false" class="form-control padding-horizontal-15" ID="ddlApp" AutoPostBack="true" runat="server"></asp:DropDownList>
		<br/>
	</div> 


    <label for="" class="col-sm-2 text-right">Version</label>
    
    <asp:Panel ID="pnlVersion" Visible="true" runat="server">
        <div class="col-sm-3">
            <asp:DropDownList class="form-control padding-horizontal-15" Enabled="false" ID="ddlver" AutoPostBack="true" runat="server"></asp:DropDownList>
        	<br/>
        </div>
        <div class="col-sm-1">
            <asp:linkbutton width="80px" cssclass="btn btn-default " data- id="btnaddver" runat="server" >Add</asp:linkbutton>    
		    <br/>
        </div>      
    </asp:Panel>
    <asp:Panel ID="pnlVersionadd" Visible="False" runat="server">
        <div class="col-sm-3">
		    <asp:textbox class="form-control padding-horizontal-15"  id="txtDesc" runat="server" placeholder="Ex : Beta"></asp:textbox>
		    <asp:textbox class="form-control padding-horizontal-15"  id="txtVer" runat="server"></asp:textbox>
            <br/>
        </div>
        <div class="col-sm-1">
            <asp:linkbutton width="80px" cssclass="btn btn-default " data- id="btnsaveVer" runat="server" >save</asp:linkbutton>    
		    <br/>
        </div>      
    </asp:Panel>
		<br/>


	
	<label for="" class="col-sm-2 text-right">Module</label>
	<asp:Panel ID="pnlModule" Visible="true" runat="server">
	    <div class="col-sm-3">
            <asp:DropDownList class="form-control padding-horizontal-15" ID="ddlmodule" AutoPostBack="true" runat="server"></asp:DropDownList>
		    <br/>
	    </div>
	    <div class="col-sm-1">
            <asp:linkbutton width="80px" cssclass="btn btn-default " data- id="brnAddModule" runat="server" >Add</asp:linkbutton>    
		    <br/>
        </div>
</asp:Panel>

 <asp:Panel ID="pnlAddModule" Visible="false" runat="server">

    <div class="col-sm-3">
		<asp:textbox class="form-control padding-horizontal-15"  id="txtModule" runat="server"></asp:textbox>
        <br/>
    </div>
<div class="col-sm-1">
<asp:linkbutton width="80px" cssclass="btn btn-default " data- id="btnSaveModule" runat="server" >save</asp:linkbutton>    
		<br/>
  </div>      
      </asp:Panel>
		<br/>

    <label for="" class="col-sm-2 text-right">Note</label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" Height="100px" id="txtNote" TextMode="MultiLine" runat="server"></asp:textbox>
        <div style="padding-top:10px; padding-bottom:20px">
            <label for="" class="col-sm-2 text-right" style="width:90px;">Active</label>
            <asp:CheckBox ID="chkActive" Checked="true" runat="server" />
        </div>
	</div>
       
</div>
    
<br />
<br />
<div style="width:100%;">
        <asp:GridView   width="100%" class="center table" Font-Size="8" runat="server" CssClass="table "
            OnRowDataBound="OnRowDataBound"  ShowHeader="true" HeaderStyle-BackColor="#337ab7" HeaderStyle-ForeColor="White" 
            AutoGenerateColumns="false"    OnSelectedIndexChanged="OnSelectedIndexChanged" BorderStyle="None" BorderWidth="1px" 
            CellPadding="4" ForeColor="Black" GridLines="None"  ID="GridView1" AllowPaging="true" PageSize="5" >
            <Columns>
                <asp:BoundField DataField="appid" HeaderText="appid" SortExpression="appid"  />
                <%--<asp:BoundField DataField="verid" HeaderText="verid" SortExpression="verid"  />--%>
                <asp:templatefield HeaderText="version">
                    <itemtemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("description")+ "." + Eval("verid")%>'></asp:Label>
                    </itemtemplate>
                </asp:templatefield>
                <asp:BoundField DataField="moduleid" HeaderText="moduleid" SortExpression="moduleid"  />
                <asp:BoundField DataField="note" HeaderText="note" SortExpression="note"  />
                <asp:BoundField DataField="active" HeaderText="active" SortExpression="active"  />
                <asp:templatefield HeaderText="Edit">
                    <itemtemplate>
                        <asp:linkbutton width="40px" cssclass="btn btn-default btn-sm" data- id="showmbr" CommandName="getcode" CommandArgument='<%# Eval("id")%>' runat="server"><span class="fa fa-edit"></span></asp:linkbutton>
                    </itemtemplate>
                </asp:templatefield>
            </Columns>
            <pagerstyle cssclass="pagination-ys">
            </pagerstyle>
            <EmptyDataTemplate>
                <div class="col-sm-12">
                    <table class="table">
                        <tr style="background-color:#cccccc;">
                            <th>Module ID</th>
                            <th>Name</th>
                        
                            <th>Active</th>
                            <th>Edit</th>
                        </tr>
                        <tr style="background-color:#f8f8f8;">
                            <td colspan="13" align="center">No record</td>
                        </tr>
                    </table>
                </div>
            </EmptyDataTemplate>
        </asp:GridView>
</div>
</div>
</asp:Content>

<asp:content id="Content3" contentplaceholderid="ContentPlaceHolder2" runat="Server">
    <asp:linkbutton width="150px" cssclass="btn btn-default " id="btnsave" runat="server" Enabled="true"><span class="fa fa-save"></span> Save</asp:linkbutton>    
<asp:linkbutton width="150px" cssclass="btn btn-default "  id="btnNew" runat="server" Enabled="true"><span class="fa fa-adjust"></span> New</asp:linkbutton>    
<asp:linkbutton width="150px" cssclass="btn btn-default " id="btnCxld" runat="server"><span class="fa fa-close"></span> Abort</asp:linkbutton>
</asp:content>