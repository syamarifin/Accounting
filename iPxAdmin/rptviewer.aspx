<%--<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptviewer.aspx.vb" Inherits="iPxPMS_rptviewer" %>--%>
<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/iPxAdminUpload.master" AutoEventWireup="false" CodeFile="rptviewer.aspx.vb" Inherits="iPxPMS_rptviewer" title="Alcor Accounting" EnableEventValidation = "false"%>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../assets/css/font-awesome.css" rel="stylesheet" />
    <link href="../assets/css/main.css" rel="stylesheet" />
    <link rel="stylesheet" href="../assets/css/bootstrap.min.css" />
    <style>
        .footer {
           position: fixed;
           left: 0;
           bottom: 0;
           width: 100%;
           background-color:#e7e7e7;
           color: white;
           text-align: right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
    <div>
     <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="True" ReportSourceID="CrystalReportSource1" HasCrystalLogo="False"
        Width="550px" DisplayGroupTree="False" DisplayToolbar="True" />
          <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
        <Report >
        </Report>

    </CR:CrystalReportSource>
    </div>
    <br />
    <br />
    <div class="footer">
        <asp:LinkButton ID="lbAbort" Width="150px" CssClass="btn btn-default" runat="server"><i class="fa fa-close"></i> Abort</asp:LinkButton>
    </div>
    </form>
</body>
</html>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
     <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="True" ReportSourceID="CrystalReportSource1" HasCrystalLogo="False"
        Width="550px" DisplayGroupTree="False" DisplayToolbar="True" />
          <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
        <Report >
        </Report>

    </CR:CrystalReportSource>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div id="footer" class="container">
        <asp:LinkButton ID="lbAbort" Width="150px" CssClass="btn btn-default" runat="server"><i class="fa fa-close"></i> Abort</asp:LinkButton>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">

</asp:Content>
