<%@ Page Language="VB" AutoEventWireup="false" CodeFile="signupreq.aspx.vb" Inherits="iPxDashboard_signupreq" title="Alcor Dashboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register Alcor</title>
    
    
    

    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

  <link href="../assets/css/bootstrap.css" rel="stylesheet" />
<link href="../assets/css/font-awesome.css" rel="stylesheet" />
<link href="../assets/css/custom.css" rel="stylesheet" />


<link rel="stylesheet" href="../assets/css/bootstrap.min.css"> 


<script src="../assets/js/jquery.js" type="text/javascript"></script>

<script src="../assets/js/bootstrap.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container"  >
 
    <br />
    

      
        <asp:Panel ID="pnlReg" Visible="true" runat="server">
     
    <div class="thumbnail" >
    <h3 class="text-center" style="color:Teal"><label> 
        <asp:Label ID="lblRegfor" runat="server" Text="Register"></asp:Label> <span class="fa fa-edit"> </label></span></h3>
       
       <hr />
       
       <div class="col-sm-3" style ="text-align:right;"><label><asp:Label ID="lblBusinessName" runat="server" Text="Hotel Name"></asp:Label> <label style="color:Red;">*</label></label>
       </div> 
       <div class="col-sm-6">
       <asp:TextBox ID="txtHotelname" MaxLength="20" CssClass="form-control" runat="server"></asp:TextBox> 
       </div>
       <br />
       <br />
        
   
         <div class="col-sm-3" style ="text-align:right;"><label>Contact Person <label style="color:Red;">*</label></label>
       </div> 
       <div class="col-sm-6">
       <asp:TextBox ID="txtContactPerson"  MaxLength="20" CssClass="form-control" runat="server"></asp:TextBox> 
       </div>
          <br />
       <br />
         <div class="col-sm-3" style ="text-align:right;"><label>Mobile <label style="color:Red;">*</label></label>
       </div> 
       <div class="col-sm-6">
       <asp:TextBox ID="txtMobile" MaxLength="20" CssClass="form-control" runat="server"></asp:TextBox> 
       </div>
          <br />
       <br />
         <div class="col-sm-3" style ="text-align:right;"><label>User ID/ Email <label style="color:Red;">*</label></label>
       </div> 
       <div class="col-sm-6">
       <asp:TextBox ID="txtEmail" MaxLength="50" CssClass="form-control" runat="server"></asp:TextBox> 
    
       </div>
         <%--  <asp:RegularExpressionValidator ID="regexEmailValid0" runat="server" 
                                    ControlToValidate="txtEmail" ErrorMessage="Invalid Email Format" 
                                    Font-Size="X-Small" 
                                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
         --%> 
         <br />
       <br />
       <div class="col-sm-3" style ="text-align:right;"><label>Address <label style="color:Red;">*</label></label>
       </div> 
       <div class="col-sm-6">
       <asp:TextBox ID="txtaddress" MaxLength="20" CssClass="form-control" runat="server"></asp:TextBox> 
       </div>
          <%--<br />
       <br />
         <div class="col-sm-3" style ="text-align:right;"><label>Country <label style="color:Red;">*</label></label>
       </div> 
       <div class="col-sm-6">
		<asp:DropDownList class="form-control padding-horizontal-15" id="drpCountry" AutoPostBack="true" MaxLength="4" runat="server"></asp:DropDownList>
       
        </div>
          <br />       
       
           <br />
         <div class="col-sm-3" style ="text-align:right;"><label>City <label style="color:Red;">*</label></label>
       </div> 
       <div class="col-sm-6">
		<asp:DropDownList class="form-control padding-horizontal-15" id="drpCity" MaxLength="4" runat="server"></asp:DropDownList>
      </div>--%>
     
  <br />
  <br />
         <div class="col-sm-3" style ="text-align:right;"><label>Website</label>
       </div> 
       <div class="col-sm-6">
       <asp:TextBox ID="txtWebsite" MaxLength="100" CssClass="form-control" runat="server"></asp:TextBox> 
       </div>
          <br />
      
   
       <br />
        <br />
    



            <hr />
       <div class="text-center ">
           
            <label class="ui-checkbox">
        
               <asp:CheckBox  ID="chkAgree" runat="server"  />
               <span > <strong> I agree  </strong> </span> </label>
   
  <%--<a class="" data-toggle="collapse" href="#collapseExample" aria-expanded="false" aria-controls="collapseExample">
   Read / Hide Term 
  </a>--%>
       <asp:Button ID="btnsubmit" Width="100px" CssClass="btn btn-success" runat="server" Text="Submit" />
       <asp:Button ID="btnBack" Width="100px" CssClass="btn btn-warning" runat="server" Text="Back" />
       <br />
        <br />
       </div>
    </div>
       </asp:Panel>
    </div>
    <div>
    </div>
    </form>
    
    


</body>
</html>
