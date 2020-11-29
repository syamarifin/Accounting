<%@ Page Language="VB" AutoEventWireup="false" CodeFile="logon.aspx.vb" Inherits="iPxAdmin_logon" title="Alcor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="HeadSignin" runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
<link rel="icon" type="image/png" href="../assets/images/icon/icon/logo-A.png">
<title>iPx-Admin Alcor</title>
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
  <div id="round123" class="panel lock-box">
  
    <div class="center"> 
        <img alt="" src="../assets/background/logoAlcorAcc.jpg" width="40%" />
        <asp:FileUpload ID="FileUpload1" runat="server" Visible="False" />
      </div>
    <%--<h4> Accounting </h4>--%>
      
     

    <div>
    <br />
     <label for="" class="col-sm-3 text-muted ">Property</label>  
            <div class="col-sm-9">
                      <asp:DropDownList ID="ddProperty" class="form-control padding-horizontal-15" 
                     runat="server" DataSourceID="iPxCNCT2" DataTextField="businessname" 
                     DataValueField="businessid" >
                    </asp:DropDownList>
                    <br />
            </div>
                <asp:SqlDataSource ID="iPxCNCT2" runat="server" 
                     ConnectionString="<%$ ConnectionStrings:iPxCNCT %>" 
                     SelectCommand="SELECT rtrim(businessid) as businessid, businessname
                     FROM         iPx_profile_client
                     WHERE businesstype='26' and (userid = @UserID) order by businessname  ">
<SelectParameters>
                  <asp:SessionParameter Name="UserID" SessionField="iUserID" />
  
              </SelectParameters>

                 </asp:SqlDataSource>
    </div>
     <div class="iconic-input right form-group">
     <br />
            <label for="" class="col-sm-3 text-muted ">Username</label>  
            <div class="col-sm-9">
            <asp:TextBox ID="txtUserLogon" runat="server"  placeholder="Username"  class="form-control padding-horizontal-15"  >  </asp:TextBox>
            </div>
            
            <label for="" class="col-sm-3 text-muted">Password</label> 
            <div class="col-sm-9"> 
            <asp:TextBox ID="txtPassLogon" runat="server" 
                  placeholder="Password" class="form-control lock-input" 
                  TextMode="Password"></asp:TextBox>
                  </div>
    </div>
    <br />
                <asp:Button ID="submit" runat="server" Text="Logon" class="btn btn-block btn-primary" BackColor="#018c8f"/>
                <asp:Button ID="btnsignout" runat="server" Text="Sign Out" class="btn btn-block btn-primary" BackColor="#018c8f" />
                
    </div>
    </div>
    
    </form>
</body>
</html>
