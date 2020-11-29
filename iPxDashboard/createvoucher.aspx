<%@ Page Language="VB" MasterPageFile="~/iPxDashboard/iPxDashboard.master" AutoEventWireup="false" CodeFile="createvoucher.aspx.vb" Inherits="iPxDashboard_createvoucher"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <div class="lock-wrapper">
      <div class=" lock-box">
         <div class="center"> <img alt="" src="../assets/images/icon/user.png" class="img-circle"/> </div>
         <div class="list-group">
            <div class="" >
            </div>
            <div class="" >
            </div>
            <div >
               <label for="" class="text-muted"># Package</label>
               <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control padding-horizontal-15" >
                  <asp:ListItem>Starter</asp:ListItem>
                  <asp:ListItem>Business</asp:ListItem>
                  <asp:ListItem>Professional</asp:ListItem>
               </asp:DropDownList>
            </div>
            <br/>
            <div >
               <label for="" class="text-muted">Payment Type</label>
               <asp:DropDownList ID="DropDownList2" class="form-control padding-horizontal-15" runat="server">
                  <asp:ListItem>Yearly ( Pay 11 Months Only )</asp:ListItem>
                  <asp:ListItem>Monthly</asp:ListItem>
               </asp:DropDownList>
            </div>
         </div>
         <br/>
         <div class="list-group">
            <asp:Button ID="btnGenerateVcr" runat="server" Text="Generate Voucher" CssClass="btn btn-block btn-success" OnClientClick="beep()"/>
         </div>
         <div class="form-group">
         </div>
      </div>
   </div>
   <div class="">
      <div class="form-group">
         <table class="table  text-bold text-center col-sm-12" >
            <tr>
               <td>
                  <asp:RadioButton ID="rbActiveList" runat="server" Text="Active" GroupName="rbVcr" AutoPostBack="true" Checked="true"/>
               </td>
               <td>
                  <asp:RadioButton ID="rbUsedList" runat="server" Text="Used" GroupName="rbVcr" AutoPostBack="true"/>
               </td>
               <td>
                  <asp:RadioButton ID="rbAllList" runat="server" Text="All" GroupName="rbVcr" AutoPostBack="true"  />
               </td>
               <td>
                  <asp:RadioButton ID="rbMonthly" runat="server" Text="Monthly" GroupName="rbVcr" Visible="false" AutoPostBack="true"/>
               </td>
               <td>
                  <asp:RadioButton ID="rbYearly" runat="server" Text="Yearly" GroupName="rbVcr" Visible="false" AutoPostBack="true"/>
               </td>
            </tr>
         </table>
      </div>
      <div class="form-group">
         <div class="col-sm-12 center">
            <div style="overflow:auto;max-height:200px" >
               <asp:GridView ID="GridView1" style="overflow:auto;" width="100%"  Font-Size="8" runat="server" CssClass="table table-dynamic"
                  AllowPaging="false"  OnRowDataBound="OnRowDataBound"  ShowHeader="true"
                  OnSelectedIndexChanged="OnSelectedIndexChanged" 
                  AutoGenerateColumns="False" DataSourceID="iPxCNCT"  
                  BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                  ForeColor="Black" GridLines="None" >
                  <RowStyle BackColor="#F7F7DE" />
                  <Columns>
                     <asp:CommandField ButtonType="Image"  SelectImageUrl="../assets/images/icon/ok24.png" 
                        ShowSelectButton="True">
                     </asp:CommandField>
                     <asp:BoundField DataField="voucherno" HeaderText="Voucher no" 
                        SortExpression="voucherno">
                     </asp:BoundField>
                     <asp:BoundField DataField="description" HeaderText="Description" ReadOnly="True" 
                        SortExpression="description">
                     </asp:BoundField>
                     <asp:BoundField DataField="status" HeaderText="Status" ReadOnly="True" 
                        SortExpression="status">
                     </asp:BoundField>
                     <asp:BoundField DataField="createdby" HeaderText="Created By" ReadOnly="True" 
                        SortExpression="createdby">
                     </asp:BoundField>
                     <asp:BoundField DataField="usedby" HeaderText="Used By" ReadOnly="True" 
                        SortExpression="usedby">
                     </asp:BoundField>
                  </Columns>
                  <FooterStyle BackColor="#CCCC99" />
                  <PagerStyle HorizontalAlign = "Center" BorderColor="White" Font-Bold="true"  CssClass = "pagination-ys" />
                  <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                  <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                  <AlternatingRowStyle BackColor="White" />
               </asp:GridView>
               <asp:SqlDataSource ID="iPxCNCT" runat="server" 
                  ConnectionString="<%$ ConnectionStrings:iPxCNCT %>" 
                  SelectCommand="">
               </asp:SqlDataSource>
            </div>
         </div>
      </div>
   </div>
</asp:Content>