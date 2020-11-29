<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxSalesStatistic.aspx.vb" Inherits="iPxAdmin_iPxSalesStatistic" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="col-md-1" style="padding:0;">
        <label for="usr">For Date:</label>
    </div>
    <div class="col-md-4" style="padding-right:5px;">
        <div class="form-group" style="margin-bottom:3px;">
            <div class="input-group date datepicker1" style="padding:0;">
                <asp:TextBox ID="tbDate" runat="server" CssClass="form-control" Enabled="false" OnTextChanged="cari" AutoPostBack="true"></asp:TextBox>
                <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
            </div>
        </div>
    </div>
    <div class="col-md-4" style="padding-left:5px;">
        <asp:LinkButton ID="lbPrint" runat="server" CssClass="btn btn-default"><i class="fa fa-print"></i> Print</asp:LinkButton>
    </div>
    <div class="col-lg-12">
        <asp:GridView EmptyDataText="No records has been added." ID="gvStatistic" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered" HeaderStyle-BackColor="#0a818e" 
            HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None"
            OnRowDataBound="OnRowDataBound" >
            <Columns>
                <asp:BoundField DataField="Descrip" HeaderText="Description"/>
                <asp:BoundField DataField="total" HeaderText="Today" ItemStyle-Width="120px"/>
                <asp:BoundField DataField="" HeaderText="Month Todate" ItemStyle-Width="120px"/>
                <asp:BoundField DataField="" HeaderText="Year Todate" ItemStyle-Width="120px"/>
                <%--<asp:TemplateField HeaderText="Month Todate" ItemStyle-Width="20px">
                    <ItemTemplate>
                        <asp:Label ID="lblOpen" runat="server" Text=""></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Year Todate" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:Label ID="lblOpen" runat="server" Text=""></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

