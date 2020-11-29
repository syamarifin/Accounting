<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/iPxDashboard/iPxDashboard.master" CodeFile="queryRequest.aspx.vb" title="Alcor Dashboard" Inherits="iPxDashboard_queryRequest" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
         *
{
padding: 0;
        margin-left: 0px;
        margin-right: 0px;
        margin-top: 0px;
        z-index:auto; 
    }
                  body
        {
            font-family: Arial;
            font-size: 10pt;
           
        }
        td
        {
            cursor: pointer;
        }
        .hover_row
        {
            background-color: #FFFFBF;
        }
         .selected_row td
        {
            background-color: #A1DCF2;
        }
    #step1
{
top:0%;
height: 100%;
width :100%;
left :0%;
position:fixed ;
z-index:999;
}
</style> 

<script type="text/javascript">

    $(window).load(function() {
        hideModal();
        $('#getuser').modal({backdrop:'static',
            keyboard: false
        }, 'show');
       
    });

     
    function openModal() {
        $('#getuser').modal({ backdrop: 'static',
            keyboard: false
        }, 'show');
    }

 
</script>
<script type="text/javascript">
     function hideModal() {
         $('#getuser').modal({ backdrop: 'static',
             keyboard: false
         }, 'hide');
        $('.modal-backdrop').hide()
    }    
</script>
<script type="text/javascript">
    function clearModal() {
        $('#getuser').modal({ backdrop: 'static',
            keyboard: false
        }, 'hide');
        $('.modal-backdrop').hide();
        $('#getuser').modal({ backdrop: 'static',
            keyboard: false
        }, 'show');
    }    
</script>
</asp:Content>



  
 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<div class="container">
  <!-- Modal -->
  <div class="modal fade"  id="getuser" role="dialog">
    <div class="modal-dialog modal-lg" style=" width:400px;">
      <div class="modal-content">
        <div class="modal-header">
         
         <h4>Query User Access</h4>
        </div>
        <div class="modal-body">
           <div align="center" style="top: auto; width : 400px; height: 400px">
           
           <label for="" style="text-align:right;" class="col-sm-4">Business Name</label>
           <div class="col-sm-6">
               <asp:TextBox class="form-control padding-horizontal-15" ID="txtbusinesname"  runat="server"></asp:TextBox>
            </div>         
            <br />
            <br />

            <label for="" style="text-align:right;" class="col-sm-4">Address</label>
           <div class="col-sm-6">
               <asp:TextBox class="form-control padding-horizontal-15" ID="txtaddress"  runat="server"></asp:TextBox>
            </div>         
            <br />
            <br />

            <label for="" style="text-align:right;" class="col-sm-4">Mobile</label>
           <div class="col-sm-6">
               <asp:TextBox class="form-control padding-horizontal-15" ID="txtmobile"  runat="server"></asp:TextBox>
            </div>         
            <br />
            <br />
            
            <label for="" style="text-align:right;" class="col-sm-4">Email</label>
           <div class="col-sm-6">
               <asp:TextBox class="form-control padding-horizontal-15" ID="txtemail"  runat="server"></asp:TextBox>
            </div>         
            <br />
            <br />
            
            <label for="" style="text-align:right;" class="col-sm-4">Website</label>
           <div class="col-sm-6">
               <asp:TextBox class="form-control padding-horizontal-15" ID="txtwebsite"  runat="server"></asp:TextBox>
            </div>         
            <br />
            <br />

           <hr style="width:300px" />
               <label for="" style="text-align:center; font-size:medium" class="col-sm-10">By Periode</label>
                <br />

            <label for="" style="text-align:right;" class="col-sm-4">Peroide From</label>
            <div class="col-sm-6">
    <div class="input-group">
        <asp:TextBox ID="txtPerFrom" class="form-control padding-horizontal-15"  runat="server"  placeholder="dd/MM/yyyy" ></asp:TextBox>
        <span class="input-group-btn">
            <asp:LinkButton height="34px" CssClass="btn btn-default dropdown-toggle" data- ID="btncal2"  runat="server" ><span class="fa fa-calendar-o "></span>
            </asp:LinkButton> 
        </span>
                    
  
    </div>

    <div>
        <asp:Calendar ID="Calendar2" runat="server" BackColor="White" 
        BorderColor="Black" BorderStyle="Solid" CellSpacing="1" Font-Names="Verdana" 
        Font-Size="9pt" ForeColor="Black" Height="220px" NextPrevFormat="ShortMonth" 
        Visible="False" Width="220px">

        	<SelectedDayStyle BackColor="DimGray" ForeColor="White" />
        	<TodayDayStyle BackColor="#999999" ForeColor="White" />
        	<OtherMonthDayStyle ForeColor="#999999" />
        	<DayStyle BackColor="#CCCCCC" />
        	<NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="White" />
        	<DayHeaderStyle Font-Bold="True" Font-Size="8pt" ForeColor="Black" Height="8pt" />
        	<TitleStyle BackColor="Gray" BorderStyle="Solid" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Height="12pt" />
    	</asp:Calendar>
    </div>
</div>   
                  
            <br />
            <br />

            <label for="" style="text-align:right;" class="col-sm-4">Peroide Until</label>
            <div class="col-sm-6">
    <div class="input-group">
        <asp:TextBox ID="txtPerUntl" class="form-control padding-horizontal-15"  runat="server" placeholder="dd/MM/yyyy" ></asp:TextBox>
        <span class="input-group-btn">
            <asp:LinkButton height="34px" CssClass="btn btn-default dropdown-toggle" data- ID="btncal3"  runat="server" ><span class="fa fa-calendar-o "></span>
            </asp:LinkButton> 
        </span>
                    
  
    </div>

    <div>
        <asp:Calendar ID="Calendar3" runat="server" BackColor="White" 
        BorderColor="Black" BorderStyle="Solid" CellSpacing="1" Font-Names="Verdana" 
        Font-Size="9pt" ForeColor="Black" Height="220px" NextPrevFormat="ShortMonth" 
        Visible="False" Width="220px">

        	<SelectedDayStyle BackColor="DimGray" ForeColor="White" />
        	<TodayDayStyle BackColor="#999999" ForeColor="White" />
        	<OtherMonthDayStyle ForeColor="#999999" />
        	<DayStyle BackColor="#CCCCCC" />
        	<NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="White" />
        	<DayHeaderStyle Font-Bold="True" Font-Size="8pt" ForeColor="Black" Height="8pt" />
        	<TitleStyle BackColor="Gray" BorderStyle="Solid" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Height="12pt" />
    	</asp:Calendar>
    </div>
</div>         
            <br />
            <br />


            </div>
        <div class="modal-footer">
 <div class="form-group center">
    <div class="form-group">  
    <label for="" class="col-sm-1"></label>
              <asp:LinkButton width=125px CssClass="btn btn-default dropdown-toggle" data- ID="btnLogin" runat="server"><span class="fa fa-filter "></span> Query</asp:LinkButton>    
      
      <asp:LinkButton width=125px CssClass="btn btn-default dropdown-toggle" data- ID="btnExit" runat="server"><span class="fa fa-close"></span> Abort</asp:LinkButton> 
      

    </div>  
     </div>         
        

        </div>
      </div>
    </div>
  </div>
</div>
 
 </asp:Content>

