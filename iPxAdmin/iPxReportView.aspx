<%@ Page  Language="VB"  MasterPageFile="~/iPxAdmin/iPxAdminUpload.master" AutoEventWireup="false" CodeFile="iPxReportView.aspx.vb" Inherits="iPxAdmin_iPxReportView" title="Alcor" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
             
        <asp:Panel ID="Panel1" BackColor="White" runat="server" >
                                                    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" DisplayGroupTree="False" HasCrystalLogo="False" />

        </asp:Panel>
                                  
                                

             

</asp:Content>

<asp:content id="Content3" contentplaceholderid="ContentPlaceHolder2" runat="Server">

<%--<asp:linkbutton width="150px" cssclass="btn btn-default" PostBackUrl="~/iPxAdmin/iPxReportList.aspx"  data- id="btnCxld" runat="server"><span class="fa fa-close"></span> Abort</asp:linkbutton>--%>
</asp:content>