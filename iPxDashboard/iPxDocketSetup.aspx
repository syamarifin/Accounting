<%@ Page Title="Alcor Accounting" Language="VB" MasterPageFile="~/iPxDashboard/iPxDashboardUpload.master" AutoEventWireup="false" CodeFile="iPxDocketSetup.aspx.vb" Inherits="iPxDashboard_iPxDocketSetup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
       
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="pnlHotelProperty" runat="server">
        <div class="row" style="padding-left :15%;padding-right:15%;">
        <div>
            Business Name
        </div>
        <div>
            <asp:DropDownList ID="ddlBusinessName" AutoPostBack="true" runat="server" CssClass="form-control" Width="500px" >
            </asp:DropDownList>
        </div>
        <div>
            Report Group
        </div>
        <div>
            <asp:DropDownList ID="ddlGrpID" runat="server" CssClass="form-control" Width="500px" >
                <asp:ListItem Value="AR">AR Transaction</asp:ListItem>
                <asp:ListItem Value="IV">AR Invoice</asp:ListItem>
                <asp:ListItem Value="RC">AR Receipt</asp:ListItem>
                <asp:ListItem Value="AP">AP Transaction</asp:ListItem>
                <asp:ListItem Value="PV">AP Voucher</asp:ListItem>
                <asp:ListItem Value="PY">AP Payment</asp:ListItem>
                <asp:ListItem Value="SL">Sales</asp:ListItem>
                <asp:ListItem Value="GL">General Ledger</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div>
            Report ID
        </div>
        <div>
            <asp:TextBox ID="txtReportID" runat="server" CssClass="form-control" MaxLength="4" style="margin-left: 0px" Width="500px"></asp:TextBox>
        </div>
        <div>
            <div>Description</div>
            <div>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" AutoPostBack="True" style="margin-left: 0px" Width="500px"></asp:TextBox>
            </div>
        </div>
        <br />
        <div>
            <asp:CheckBox ID="chkIsactive" runat="server" Checked="True" /> Active
        </div>
        </div>
        <hr />
        <div>
            Report File
        </div>
        <div>
            <asp:FileUpload ID="FileUpload1" runat="server" />
        </div>
        <hr />
        <div class="btn-group">
            <asp:Button ID="imgNew" runat="server" Text="New" CssClass="btn btn-success" />
            <asp:Button ID="imgSave" runat="server" Text="Save" CssClass="btn btn-success" />
            <asp:Button ID="imgCancel" runat="server" Text="Cancel" CssClass="btn btn-success" />
            <asp:Button ID="imgDelete" runat="server" Text="Delete" CssClass="btn btn-success" />
            <asp:Button ID="btnPreview" runat="server" Text="Delete" CssClass="btn btn-success" />
        </div>
    </asp:Panel>
         
    <br />
     
    <asp:Panel ID="pnlGridLine" runat="server" Width="100%">
        <div>
            <asp:GridView ID="GridView1" runat="server"  AllowPaging="True" 
             OnPageIndexChanging = "OnPaging" PageSize = "6" AllowSorting="True" 
             CssClass="table table-hover table-striped" GridLines="None">
                <Columns>
                    <asp:CommandField ShowSelectButton="true" SelectImageUrl="~/assets/images/icon/select24.png" ButtonType="Image"    />
                </Columns>
                    <SelectedRowStyle  Font-Bold="True" Font-Italic="True" />
            </asp:GridView>
        </div>
    </asp:Panel>
            
</asp:Content>

