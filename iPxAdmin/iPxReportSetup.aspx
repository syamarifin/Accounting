<%@ Page Title="Pyxis Support E-Ticket" Language="VB" MasterPageFile="~/iPxAdmin/iPxAdminUpload.master" AutoEventWireup="false" CodeFile="iPxReportSetup.aspx.vb" Inherits="iPxAdmin_iPxReportSetup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
       
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="pnlHotelProperty" runat="server">
        <div class="row" style="padding-left :15%;padding-right:15%;">
        <div>
            Module
        </div>
        <div>
            <asp:DropDownList ID="ddlModul" AutoPostBack="true" runat="server" CssClass="form-control" Width="500px" >
                <%--<asp:ListItem Value="01">PMS</asp:ListItem>--%>
                <asp:ListItem Value="01">CRM</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div>
            Report Group
        </div>
        <div>
            <asp:DropDownList ID="ddlGrpID" runat="server" CssClass="form-control" Width="500px" >
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
            <div>
                <asp:RadioButton ID="rbP5" runat="server" AutoPostBack="True" Checked="True" GroupName="grpList" Text="Parameter Null"  />
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <div>
                        <b>P1  </b><asp:RadioButton ID="rbP1" runat="server" AutoPostBack="True" GroupName="grpList" Text="Parameter 1"  />
                    </div>
                    <div>
                        <asp:TextBox ID="txtp1" runat="server" CssClass="form-control" Enabled="False"></asp:TextBox>
                    </div>
                    <div>
                        <asp:DropDownList CssClass="form-control" ID="ddlp1" runat="server" Enabled="False" AutoPostBack="True">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem Value="T">Text</asp:ListItem>
                            <asp:ListItem Value="D">Date</asp:ListItem>
                            <asp:ListItem Value="C">Combo Box</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:TextBox ID="txtQueryP1" CssClass="form-control" runat="server" Enabled="False" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <hr />
                    <div>
                        <b>P3  </b><asp:RadioButton ID="rbP3" runat="server" AutoPostBack="True" GroupName="grpList" Text="Parameter 3" />
                    </div>
                    <div>
                        <asp:TextBox ID="txtp3" runat="server" CssClass="form-control" Enabled="False"></asp:TextBox>
                    </div>
                    <div>
                        <asp:DropDownList ID="ddlp3" runat="server" CssClass="form-control" AutoPostBack="True" Enabled="False">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem Value="T">Text</asp:ListItem>
                            <asp:ListItem Value="D">Date</asp:ListItem>
                            <asp:ListItem Value="C">Combo Box</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:TextBox ID="txtQueryP3" runat="server" CssClass="form-control" Enabled="False" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div>
                        <b>P2  </b><asp:RadioButton ID="rbP2" runat="server" AutoPostBack="True" GroupName="grpList" Text="Parameter 2" />
                    </div>
                    <div>
                        <asp:TextBox ID="txtp2" runat="server" CssClass="form-control" Enabled="False"></asp:TextBox>
                    </div>
                    <div>
                        <asp:DropDownList ID="ddlp2" runat="server" CssClass="form-control" AutoPostBack="True" Enabled="False">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem Value="T">Text</asp:ListItem>
                            <asp:ListItem Value="D">Date</asp:ListItem>
                            <asp:ListItem Value="C">Combo Box</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:TextBox ID="txtQueryP2" runat="server" CssClass="form-control" Enabled="False" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <hr />
                    <div>
                        <b>P4  </b><asp:RadioButton ID="rbP4" runat="server" AutoPostBack="True" GroupName="grpList" Text="Parameter 4"  />
                    </div>
                    <div>
                        <asp:TextBox ID="txtp4" runat="server" CssClass="form-control" Enabled="False"></asp:TextBox>
                    </div>
                    <div>
                        <asp:DropDownList ID="ddlp4" runat="server" CssClass="form-control" AutoPostBack="True" Enabled="False">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem Value="T">Text</asp:ListItem>
                            <asp:ListItem Value="D">Date</asp:ListItem>
                            <asp:ListItem Value="C">Combo Box</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:TextBox ID="txtQueryP4" runat="server" CssClass="form-control" Enabled="False" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
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

