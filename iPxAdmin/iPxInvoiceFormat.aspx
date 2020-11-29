<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxInvoiceFormat.aspx.vb" Inherits="iPxAdmin_iPxInvoiceFormat" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .paragraf
        {
        	text-align: justify;
        	text-indent: 0.5in;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width:100%" border="1">
        <tr>
            <td style="text-align:center; height:75px;"> Header </td>
        </tr>
    </table>
    <br />
    <p ><asp:Label ID="lblHeader" runat="server" Text="Label Label" ></asp:Label>
    
    <asp:TextBox ID="tbHeader" runat="server" CssClass="form-control" TextMode="MultiLine" Height="100px"></asp:TextBox></p>
    <br />
    <table style="width:100%" border="1">
        <tr>
            <td style="text-align:center; height:75px;"> Invoice List </td>
        </tr>
    </table>
    <br />
    <p><asp:Label ID="lblFooter1" runat="server" Text="Label"></asp:Label>
    <asp:TextBox ID="tbFooter1" runat="server" CssClass="form-control" TextMode="MultiLine" Height="50px"></asp:TextBox></p>
    <br />
    <table>
        <tr>
            <td style="width:70px;"></td>
            <td>Bank</td>
            <td style="width:10px;text-align:center">:</td>
            <td style="padding:5px;">
                <asp:Label ID="lblBank" runat="server" Text="Label"></asp:Label>
                <asp:TextBox ID="tbBank" runat="server" CssClass="form-control" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width:70px;height:15px;"></td>
            <td>No Req.</td>
            <td style="width:10px;text-align:center">:</td>
            <td style="padding:5px;">
                <asp:Label ID="lblNoReq" runat="server" Text="Label"></asp:Label>
                <asp:TextBox ID="tbNoReq" runat="server" CssClass="form-control" ></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <p><asp:Label ID="lblFooter2" runat="server" Text="Label"></asp:Label>
    <asp:TextBox ID="tbFooter2" runat="server" CssClass="form-control" TextMode="MultiLine" Height="50px"></asp:TextBox></p>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div id="footer" class="container" >
        <asp:LinkButton ID="lbAbort" Width="125px" runat="server" CssClass="btn btn-default"><i class="fa fa-close"></i> Abort</asp:LinkButton>
        <asp:LinkButton ID="lbSave" Width="125px" runat="server" CssClass="btn btn-default"><i class="fa fa-edit"></i> Edit</asp:LinkButton>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

