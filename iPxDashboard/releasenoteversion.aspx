<%@ Page Language="VB" MasterPageFile="~/iPxDashboard/iPxDashboard.master" EnableEventValidation="false" AutoEventWireup="false" CodeFile="releasenoteversion.aspx.vb" Inherits="iPxDashboard_releasenoteversion" title="Alcor Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="content">
    <div class="col-sm-10">
		     
   <br />
	</div>
   <div class="col-sm-6">
        <asp:GridView  style="overflow:auto;" width="100%" class="center table" Font-Size="8" runat="server" CssClass="table "
          AllowPaging="false"  OnRowDataBound="OnRowDataBound"  ShowHeader="true" HeaderStyle-BackColor="#cccccc"
                  AutoGenerateColumns="false"    OnSelectedIndexChanged="OnSelectedIndexChanged" 
  
           BorderStyle="None" BorderWidth="1px" CellPadding="4" 
          ForeColor="Black" GridLines="None"  ID="GridView1"   >
           <Columns>
               <asp:BoundField DataField="id" HeaderText="App ID" 
                  SortExpression="id"  />
               <asp:BoundField DataField="app" HeaderText="Name" 
                  SortExpression="app"  />
                    
               <asp:BoundField DataField="active" HeaderText="Active" 
                  SortExpression="active"  />
               <asp:templatefield HeaderText="Edit">
                    <itemtemplate>
                        <asp:linkbutton width="40px" cssclass="btn btn-default btn-sm" data- id="showmbr" CommandName="getcode" CommandArgument='<%# Eval("id")%>' runat="server"><span class="fa fa-edit"></span></asp:linkbutton>
                        </itemtemplate>
                    </asp:templatefield>
                           </Columns>
          <EmptyDataTemplate>
                    <div class="col-sm-12">
                        <table class="table">
                            <tr style="background-color:#cccccc;">
                              <th>App ID</th>
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
       <asp:LinkButton Width="210" CssClass ="btn btn-default" data- id="btnview" runat="server"><span class="fa fa-plus""></span> New</asp:LinkButton>
       </asp:Content>
     
   


