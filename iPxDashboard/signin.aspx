<%@ Page Language="VB" AutoEventWireup="false" CodeFile="signin.aspx.vb" Inherits="iPxDashboard_signin"title="Alcor Dashboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="HeadSignin" runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
<title>iPx-Admin Alcor</title>
<meta name="description" content="Responsive Admin Web App">
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">

<!-- Needs images, font... therefore can not be part of main.css -->

<link rel="stylesheet" href="../assets/css/font-awesome.css">
<link rel="stylesheet" href="../assets/css/weather-icons.css">

<link rel="stylesheet" href="../assets/css/main.css"/>
<style type="text/css">
.shad 
        {
            text-shadow: 2px 2px 4px #000000;
        }
        
        </style>
</head>

<body style="background-color:Teal">

    <form id="form1" runat="server">
    <div class="lock-wrapper ">
  <div class="panel lock-box ">
    <div class="center"> 
        <img alt="" src="../assets/img/Alcor-cloud2.png" width="170px" />
        <asp:FileUpload ID="FileUpload1" runat="server" Visible="False" />
      </div>
    <h4> Dashboard </h4>
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
              <input name="checkbox1" type="checkbox" value="option1">
              <span> <strong> Remember Me </strong> </span> </label>
          </div>
            <asp:Button ID="submit" runat="server" Text="Sign In" class="btn btn-block btn-primary" />
               <%-- <asp:Button ID="btnSignup" runat="server" Text="Sign Up" class="btn btn-block btn-primary" />--%>
                <asp:Button ID="btnRequestPassword" runat="server" Text="Request Password" class="btn btn-block btn-primary" />
         
        </div>
      </form>
    </div>
  </div>

    </form>

</body>
</html>

