<%@ Page Language="VB" MasterPageFile="~/iPxDashboard/iPxDashboard.master" AutoEventWireup="false" CodeFile="home.aspx.vb" Inherits="iPxDashboard_home" title="Alcor Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">

	 body  
 {
   background-image: url("../assets/img/dashboard.jpg");
   background-repeat: no-repeat;
   background-size: cover;
   background-attachment:fixed;
 }
 

        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div  >
            
            <div class="row">
                <div class="col-lg-3 col-md-6" >
                <div style="padding-top:10px; padding-left:10px" >
                    <div class="panel panel-transparent" >
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-3">
                                    <i class="fa fa-building fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                  
                                   <h4> <div class="text-bold">Hotel Client</div></h4>

                                    
              <div>Total Request : <asp:Label ID="lblrequest" runat="server" Font-Size="Small" Text=""></asp:Label></div>                               
              <div>Total Client : <asp:Label ID="lbltotCLient" runat="server" Font-Size="Small" Text=""></asp:Label></div>
           
                
                                </div>
                            </div>
                        </div>
                                  <a href="viewUserAcc.aspx">
                            <div class="panel-footer ">
                                <span class="pull-left">View Details</span>
                                <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                <div class="clearfix"></div>
                            </div>
                        </a>
                    </div>
                    </div>
                </div>
                                
               
            </div>
        </div>
</asp:Content>

