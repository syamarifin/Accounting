<%@ Page Language="VB" AutoEventWireup="false" CodeFile="logon.aspx.vb" Inherits="iPxDashboard_logon" title="Alcor  Dashboard" %>

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
    <div class="lock-wrapper">
  <div class="panel lock-box">
  
    <div class="center"> 
        <img alt="" src="../assets/img/Alcor-cloud2.png" />
        <asp:FileUpload ID="FileUpload1" runat="server" Visible="False" />
      </div>
    <h4> Alcor </h4>
      
       <h4 style="color:teal"> Select Property</h4>

    <div>
       <asp:DropDownList ID="ddProperty" class="form-control padding-horizontal-15" 
                     runat="server" DataSourceID="iPxCNCT2" DataTextField="businessname" 
                     DataValueField="businessid" >
                </asp:DropDownList>
                <asp:SqlDataSource ID="iPxCNCT2" runat="server" 
                     ConnectionString="<%$ ConnectionStrings:iPxCNCT %>" 
                     SelectCommand="SELECT rtrim(businessid) as businessid, businessname
FROM         iPx_profile_client
WHERE  (userid = @UserID) order by businessname  ">
<SelectParameters>
                  <asp:SessionParameter Name="UserID" SessionField="iUserID" />
  
              </SelectParameters>

                 </asp:SqlDataSource>
    </div>
    
    <br />
                <asp:Button ID="submit" runat="server" Text="Select" class="btn btn-block btn-primary" />
    </div>
    </div>
    
    </form>
</body>
</html>
