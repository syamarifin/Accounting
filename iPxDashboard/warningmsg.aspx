<%@ Page Language="VB" AutoEventWireup="false" CodeFile="warningmsg.aspx.vb" Inherits="iPxAdmin_warningmsg" title="Alcor Dashboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Message</title>
      
    
    
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

        <link rel="shortcut icon" href="images/icon/iPx.png">
    <!-- BOOTSTRAP STYLES-->
    <link href="../assets/css/bootstrap.css" rel="stylesheet" />
    <!-- FONTAWESOME STYLES-->
    <link href="../assets/css/font-awesome.css" rel="stylesheet" />
      <link href="../assets/css/main.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <asp:Panel class="" ID="confirmationstep1" runat="server" >
     <div class="lock-wrapper">
      <div class=" lock-box">
         <div class="center"> <h1><span class="img-circle fa fa-info-circle"></span> </h1>
         </div>
            <p class="text-center">Message</p>
            <br />
            <br />
               <%  Dim aMsg() As String
                   Dim i As Integer
                   aMsg = Split(Session("sMessage"), "|")
                   For i = 0 To UBound(aMsg) - 1
                       Response.Write("<h4 class=""center"">" & aMsg(i) & "</h4>")
                   Next
               %>
         </div>
       </div> 
    </asp:Panel>  
    
    <asp:Panel class="" ID="confirmationfooteryesno" runat="server" >
    <div class="lock-wrapper">
    <div class=" lock-box">
    <div class="row" >
    <div class="form-group col-md-12 col-sm-12 col-xs-12">
             <asp:Button ID="btnYes" runat="server" class="btn btn-block btn-danger " 
                Text="Yes" />                         
            <asp:Button ID="btnNo" runat="server" class="btn btn-block btn-danger" 
                Text="No" />
    </div>
    </div> 
    </div>
    </div> 
    </asp:Panel>  
    
     <asp:Panel class="" ID="confirmationfooterokonly" runat="server" style="overflow:auto;">
    <div class="lock-wrapper">
    <div class=" lock-box">
    <div class="row" >
    <div class="form-group col-md-12 col-sm-12 col-xs-12">
 
            <asp:Button ID="btnOkonly" runat="server" class="btn btn-block btn-line-danger" 
                Text="OK" />
    </div>
    </div> 
    </div>
    </div> 
    </asp:Panel>  
    </div>
    </form>
</body>
</html>
