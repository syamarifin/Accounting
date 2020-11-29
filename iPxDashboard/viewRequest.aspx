<%@ Page Language="VB" MasterPageFile="~/iPxDashboard/iPxDashboard.master" EnableEventValidation="false" AutoEventWireup="false" CodeFile="viewRequest.aspx.vb" Inherits="iPxDashboard_viewRequest" title="Alcor Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
       function confirmation() {
           if (confirm('are you sure you want to reject ?')) {
           return true;
           }else{
           return false;
           }
       }
   </script>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="col-sm-10">
		     
<asp:linkbutton width="150px" cssclass="btn btn-default "  id="btnfilter" runat="server"><span class="fa fa-filter"></span> Query </asp:linkbutton>
   <div class="btn-group">
   <asp:LinkButton ID="btnOption" runat="server"  class="btn btn-default dropdown-toggle" data-toggle="dropdown"><span class="glyphicon glyphicon-cog"></span> Option<span class="caret"></span></asp:LinkButton>
    <ul class="dropdown-menu" role="menu"> 
       
         <li><a id="btnReject" runat="server" >Reject Request</a></li>
 </ul>
 </div>
   <br />
         <br />
	</div>



<br />

   <div class="col-sm-12">
        <asp:GridView  style="overflow:auto;" width="100%" class="center table" Font-Size="8" runat="server" CssClass="table "
          AllowPaging="false"  OnRowDataBound="OnRowDataBound"  ShowHeader="true" HeaderStyle-BackColor="#337ab7" HeaderStyle-ForeColor="White"
                  AutoGenerateColumns="false"    OnSelectedIndexChanged="OnSelectedIndexChanged" 
  
           BorderStyle="None" BorderWidth="1px" CellPadding="4" 
          ForeColor="Black" GridLines="None"  ID="GridView1"   >
          
          <%--SELECT    id, hotelname, noofroom, contactperson, mobile, email, password, address, website, registerfor, registerdate, status, clerknotes, guestnotes, 
                         approvalnotes, approvaldate FROM  iPx_profile_user_signup--%>
          
           <Columns>
               <asp:BoundField DataField="hotelname" HeaderText="Business Name" 
                  SortExpression="hotelname"  />
               <asp:BoundField DataField="address" HeaderText="Address" 
                  SortExpression="address"  />
               <asp:BoundField DataField="contactperson" HeaderText="Contact Person" 
                  SortExpression="contactperson"  />
                  <asp:BoundField DataField="mobile" HeaderText="Mobile" 
                  SortExpression="mobile"  />
               <asp:BoundField DataField="email" HeaderText="Email" 
                  SortExpression="email"   />
               <asp:BoundField DataField="website" HeaderText="Website" 
                  SortExpression="website"   />
                  <asp:BoundField DataField="registerdate" HeaderText="Register Date" 
                  SortExpression="registerdate"   />
                  <asp:templatefield HeaderText="Approve">
                    <itemtemplate>
                        <asp:linkbutton width="50px" cssclass="btn btn-success " data- id="showmbr" CommandName="getcodeApp" CommandArgument='<%# Eval("email") %>' runat="server"><span class="fa fa-check"></span></asp:linkbutton>
                        </itemtemplate>
                    </asp:templatefield>
                    <asp:templatefield HeaderText="Reject">
                    <itemtemplate>
                        <asp:linkbutton width="50px" cssclass="btn btn-warning " data- id="showmbr2" OnClientClick="return confirmation();"  CommandName="getcodeRjc" CommandArgument='<%# Eval("email") %>' runat="server"><span class="fa fa-close"></span></asp:linkbutton>
                        </itemtemplate>
                    </asp:templatefield>

                           </Columns>
          <EmptyDataTemplate>
                    <div class="col-sm-12">
                        <table class="table" >
                            <tr style="background-color:#337ab7; color:White">
                              <th>Hotel Name</th>
                              <th>Address</th>
                              <th>Contact Person</th>
                              <th>Mobile</th>
                              <th>Email</th>
                              <th>Website</th>
                              <th>Register Date</th>
                              <th>Approve</th>
                              <th>Reject</th>
                              
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
    </asp:content>


                 
                 




