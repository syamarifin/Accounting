<%@ Page Language="VB" AutoEventWireup="false" CodeFile="licenseextention.aspx.vb" Inherits="iPxAdmin_licenseextention" title="Alcor"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="HeadSignin" runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
<title>iPxAdmin Alcor</title>
<meta name="description" content="Responsive Admin Web App">
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">

<!-- Needs images, font... therefore can not be part of main.css -->

<link rel="stylesheet" href="../assets/css/font-awesome.css">
<link rel="stylesheet" href="../assets/css/weather-icons.css">
<link rel="stylesheet" href="../assets/css/main.css"/>
<style>
#round123 {
    border-radius: 15px;
    padding: 20px; 
}
 body  
 {
   background-image: url("../assets/img/front.jpg");
   background-repeat: no-repeat;
   background-size: cover;
   background-attachment:fixed;
   overflow:hidden;
   
   
 }
</style>
</head>

<body >
    <form id="form1" runat="server">
     <div class="lock-wrapper">
  <div id="round123" class="thumbnail lock-box">
    <div class="center form-group-sm"> 
    <img src="../assets/images/icon/icons8-so-so-48.png" />
    <h4> Hello <%#Eval(Session("sUserLogon"))%> !</h4>
     </div>
    
    <p class="text-center">Your current license</p>
    <div class="list-group">
    <div class="text-center" >
        
        <asp:Label class="text-center" Font-Size=Medium   ID="lblSite" runat="server" Text="For My Shop"></asp:Label>
        </div>
        
        <div class="text-center" >
        <asp:Label class="text-center" ID="Label1" runat="server" Text="Has been Expired !"></asp:Label>
        </div>
        <br />
        <br />
        
          <div >
             <label for="" class="text-muted"># Package</label>
              <asp:DropDownList ID="DropDownList1" runat="server" class="form-control padding-horizontal-15" >
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
      <asp:Button ID="btnPayment" runat="server" Text="Use a voucher" class="btn btn-block btn-success" OnClientClick="beep()" BackColor="#194516"/>
      <asp:Button ID="btnBack" runat="server" Text="Back" class="btn btn-block btn-success" OnClientClick="beep()" BackColor="#194516"/>  
   </div>
 
      </div>  
    </div>

    </form>
</body>
</html>

