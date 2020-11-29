<%@ Page Language="VB" AutoEventWireup="false" CodeFile="signin.aspx.vb" Inherits="iPXADMIN_signin" title="Alcor"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="HeadSignin" runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
<link rel="icon" type="image/png" href="../assets/images/icon/icon/logo-A.png">
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

<body style ="min-height: 90%;background-image:url(../assets/background/Alcor-Accounting_frontPage.jpg); background-size: 100% 100%; background-attachment:fixed;">
    <form id="form1" runat="server">
    <div class="lock-wrapper">
  <div  id="round123" class="panel lock-box">
    <div class="center" > 
        <img alt="" src="../assets/background/logoAlcorAcc.jpg" width="40%" />
        <asp:FileUpload ID="FileUpload1" runat="server" Visible="False" />
      </div>
    <%--<h4> Accounting </h4>--%>
    <p class="text-center">Please login to Access your Account</p>
    <div class="row">
      <form action="index.html" class="form-inline" role="form">
        <div class="form-group col-md-12 col-sm-12 col-xs-12">
          <div class="iconic-input right">
            <label for="signupInputEmail" class="text-muted">Email</label>
              
              <asp:TextBox ID="txtUsername" runat="server"  placeholder="Enter Username or Email id" type="email" class="form-control padding-horizontal-15"  >  </asp:TextBox> <i class="fa fa-envelope"></i> 
         
            <i class="fa fa-envelope"></i> 
            </div>
          <div class="iconic-input right">
            <label for="signupInputPassword" class="text-muted">Password</label>
            <asp:TextBox ID="txtPassword" runat="server"  type="password" 
                  placeholder="Password" class="form-control lock-input" 
                  TextMode="Password"></asp:TextBox>
           
            <i class="fa fa-lock"></i> </div>
        <div class="square-blue pull-left pv-15">
            <label class="ui-checkbox">
        
               <asp:CheckBox  ID="chkRemanagerMe" runat="server"  />
               <span style=""> <strong> Remember Me </strong> </span> </label>
          </div>
            <asp:Button ID="submit" runat="server" Text="Sign In" class="btn btn-block btn-primary" BackColor="#018c8f" />
                <%--<asp:Button ID="btnSignup" runat="server" Text="Sign Up" class="btn btn-block btn-primary" />--%>
                <asp:Button ID="btnRequestPassword" runat="server" Text="Request Password" class="btn btn-block btn-primary" BackColor="#018c8f" />
         
        </div>
      </form>
    </div>
  </div>

    </form>
</body>
</html>

