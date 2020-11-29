<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/iPxAdmin.master" EnableEventValidation="false" AutoEventWireup="false" CodeFile="viewUserAcc.aspx.vb" Inherits="iPxAdmin_viewUserAcc" title="Alcor Loyalty Management" %>

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
          AllowPaging="false"  OnRowDataBound="OnRowDataBound"  ShowHeader="true" HeaderStyle-BackColor="#cccccc"
                  AutoGenerateColumns="false"    OnSelectedIndexChanged="OnSelectedIndexChanged" 
  
           BorderStyle="None" BorderWidth="1px" CellPadding="4" 
          ForeColor="Black" GridLines="None"  ID="GridView1"   ><%--
           businessid, usergroup, userid, username, password, name, neverexpired, registereddate, expiredafter, islocked, userimage--%>
           <Columns>
               <asp:BoundField DataField="username" HeaderText="Nick Name" 
                  SortExpression="name"  />
               <asp:BoundField DataField="usercode" HeaderText="Username" 
                  SortExpression="username"  />
              
               <asp:BoundField DataField="registereddate" HeaderText="Registered Date (dd/MM/yyyy)" HeaderStyle-Width="110px" 
                  SortExpression="registereddate" DataFormatString="{0:dd/MM/yyyy}" />               
             <asp:BoundField DataField="islocked" HeaderText="Is Locked" 
                  SortExpression="islocked"  />
               <asp:templatefield HeaderText="Edit">
                    <itemtemplate>
                        <asp:linkbutton width="50px" cssclass="btn btn-default " data- id="showmbr" CommandName="getcode" CommandArgument='<%# Eval("username")%>' runat="server"><span class="fa fa-edit"></span></asp:linkbutton>
                        </itemtemplate>
                    </asp:templatefield>
                           </Columns>
          <EmptyDataTemplate>
                    <div class="col-sm-12">
                        <table class="table">
                            <tr style="background-color:#cccccc;">
                              <th>Nick Name</th>
                              <th>Username</th>
                              <th>Registered Date<br /> (dd/MM/yyyy)</th>
                              <th>Is Locked</th>
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
    <asp:LinkButton Width="210" CssClass ="btn btn-default" data- id="btnview" runat="server"><span class="fa fa-plus""></span> User Access</asp:LinkButton>
    </asp:content>


                 
                 




