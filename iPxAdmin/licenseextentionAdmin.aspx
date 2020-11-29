<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="licenseextentionAdmin.aspx.vb" Inherits="iPxAdmin_licenseextentionAdmin" title="Alcor"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
#round123 {
    border-radius: 15px;
    padding: 20px; 
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

     <div class="lock-wrapper">
  <div id="round123" class=" ">
    <div class="center form-group-sm"> 
    <img src="../assets/images/icon/icons8-so-so-48.png" />
    <h4>Licence <%#Eval(Session("sUserLogon"))%> 
    <asp:Label class="text-center" Font-Size="Medium"   ID="lblSite" runat="server" ></asp:Label><br /><hr />
    <asp:Label class="text-center" Font-Size="X-Large"   ID="lblValid" runat="server" ></asp:Label></h4> 
     </div>
    <br />
    
    <div class="list-group">
    <div class="text-center" >
        
        
        </div>
          <div >
             <label for="" class="text-muted" style="display:none;"># Package</label>
              <asp:DropDownList ID="DropDownList1" Visible="false" runat="server" class="form-control padding-horizontal-15" >
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
          <p class="center text-blue">Or</p><p class="center  text-blue">
          Please call our customer support on 0341-550246</p>
  
        <br/>
   <div class="list-group">
      <asp:Button ID="btnPayment" runat="server" Text="Use a voucher" class="btn btn-block btn-success" OnClientClick="beep()" />
   </div>
 
      </div>  
    </div>

    </asp:Content>


