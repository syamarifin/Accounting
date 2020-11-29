<%@ Page Language="VB"  AutoEventWireup="false" CodeFile="signup.aspx.vb" Inherits="iPxAdmin_signup" title="Alcor"%>

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
</head>

<body>
    <form id="form1" runat="server">
 		<div class="login-form  ">
    <div class="lock-wrapper">
  <div class="panel lock-box">
    <div class="center"> <img alt="" src="../images/logo/Alcor_deep_blue.png" class=""/> </div>

    <p class="text-center" style=" ">Please signup to get your Account</p>
    <div class="row">
      <form action="index.html" class="form-inline" role="form">
        <div class="form-group col-md-12 col-sm-12 col-xs-12">
          <div class="iconic-input right">
            <label for="signupInputFullname" class="text-muted" style=" ">Full Name</label>                  
              <asp:TextBox ID="txtFullname" placeholder="Enter Full Name" style=" "
                  class="ph-15" required="required" runat="server" MaxLength="50"></asp:TextBox>
            <i style=" " class="fa fa-user"></i> 
          </div>
          
          <div class="iconic-input right">
            <label for="signupInputEmail" class="text-muted" style=" ">Email</label>      
            <asp:TextBox ID="txtEmail"  placeholder="Enter Email" style=" "
                  class=" ph-15" required="required" runat="server" MaxLength="60"></asp:TextBox>
            <i style=" " class="fa fa-envelope"></i> 
            </div>
            
          <div class="iconic-input right">
            <label for="signupInputPassword" class="text-muted" style=" ">Password</label>
            <asp:TextBox ID="txtPassword" type="password" placeholder="Enter Password" style=" "
                  class=" ph-15" required="required" runat="server" MaxLength="25" 
                  TextMode="Password"></asp:TextBox>
            <i style=" " class="fa fa-lock"></i> 
          </div>
          <div class="iconic-input right">
            <label for="signupInputPasswordCFRM" class="text-muted" style=" ">Retype Password</label>
            <asp:TextBox ID="txtPasswordCFRM" type="password" placeholder="Retype Password" style=" "
                  class=" ph-15" required="required" runat="server" MaxLength="25" 
                  TextMode="Password"></asp:TextBox>
            <i style=" " class="fa fa-lock"></i> </div>
          <div class="square-blue pull-left pv-15">
            <label class="ui-checkbox">
              <asp:CheckBox ID="chkAgreeement" runat="server" />
              <span style=" "> <strong> I agree with the terms </strong> </span> </label>
          </div>
    
          <asp:Button ID="btnSubmit" class="btn btn-block btn-primary" runat="server" Text="Signup Now" OnClientClick="beep()"/>
        </div>
      </form>
    </div>
  </div>
  <div class="registration" style=" "> Already registered ! <a href="signin.aspx"> <span class="text-primary"> Please Login </span> </a> </div>
</div>
        </div>     
    </form>
    

</body>
</html>
