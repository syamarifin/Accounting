<%@ Page Language="VB" MasterPageFile="~/iPxDashboard/iPxDashboard.master" EnableEventValidation="false" AutoEventWireup="false" CodeFile="viewUserAcc.aspx.vb" Inherits="iPxDashboard_viewUserAcc" title="Alcor Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="content">
    <div class="col-sm-10">
		     
<asp:linkbutton width="150px" cssclass="btn btn-default " data- id="btnfilter" runat="server"><span class="fa fa-filter"></span> Query </asp:linkbutton>
   <br />
         <br />
	</div>
   <div class="col-sm-12">
        <asp:GridView  style="overflow:auto;" width="100%" class="center table" Font-Size="8" runat="server" CssClass="table "
          AllowPaging="false"  OnRowDataBound="OnRowDataBound"  ShowHeader="true" HeaderStyle-BackColor="#337ab7" HeaderStyle-ForeColor="White"
                  AutoGenerateColumns="false"    OnSelectedIndexChanged="OnSelectedIndexChanged" 
  
           BorderStyle="None" BorderWidth="1px" CellPadding="4" 
          ForeColor="Black" GridLines="None"  ID="GridView1"   >
          
           <Columns>
           <asp:templatefield HeaderText="Detail">
                    <itemtemplate>
                        <asp:linkbutton width="50px" cssclass="btn btn-default " data- id="shw" CommandName="getdetail" CommandArgument='<%# Eval("email") %>' runat="server"><span class="fa fa-list"></span></asp:linkbutton>
                        </itemtemplate>
                    </asp:templatefield>
               <asp:BoundField DataField="businessname" HeaderText="Business Name" 
                  SortExpression="businessname"  />
               <asp:BoundField DataField="address" HeaderText="Address" 
                  SortExpression="address"  />
               <asp:BoundField DataField="phone" HeaderText="Phone" 
                  SortExpression="phone"  />
 
               <asp:BoundField DataField="businessid" HeaderText="Business Id" 
                  SortExpression="businessid"   />
               <asp:BoundField DataField="email" HeaderText="Email" 
                  SortExpression="email"  />
        
                  <asp:templatefield HeaderText="Edit">
                    <itemtemplate>
                        <asp:linkbutton width="50px" cssclass="btn btn-default " data- id="showmbr" CommandName="getcode" CommandArgument='<%# Eval("businessid") &"|"& Eval("email") %>' runat="server"><span class="fa fa-edit"></span></asp:linkbutton>
                        </itemtemplate>
                    </asp:templatefield>
                  <asp:templatefield HeaderText="Modul Acc">
                    <itemtemplate>
                        <asp:linkbutton width="50px" cssclass="btn btn-default " data- id="showMdl" CommandName="getModule" CommandArgument='<%# Eval("businessid") %>' runat="server"><span class="fa fa-list"></span></asp:linkbutton>
                        </itemtemplate>
                    </asp:templatefield>
                           </Columns>
          <EmptyDataTemplate>
                    <div class="col-sm-12">
                        <table class="table">
                            <tr style="background-color:#337ab7; color:White">
                            
                              <th>Detail</th>
                              <th>Business Name</th>
                              <th>Address</th>
                              <th>Phone</th>
                              <th>Member Id</th>
                              <th>Business Id</th>
                              <th>Email</th>
                              <th>Password</th>
                              <th>Edit</th>
                              
                            </tr>
                            <tr style="background-color:#f8f8f8;">
                              <td colspan="13" align="center">No record</td>
                            </tr>
                         </table>
                     </div>
                </EmptyDataTemplate>

</asp:GridView>
       </asp:Content>
       <asp:content id="Content2" contentplaceholderid="ContentPlaceHolder2" runat="Server">
    <asp:LinkButton Width="210" CssClass ="btn  btn-default btn-primary" data- id="btnview" runat="server"><span class="fa fa-plus""></span> User Access</asp:LinkButton>
    </asp:content>


                 
                 




