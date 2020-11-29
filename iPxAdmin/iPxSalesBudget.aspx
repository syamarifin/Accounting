<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxSalesBudget.aspx.vb" Inherits="iPxAdmin_iPxSalesBudget" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        function MonthGL() {
            $(".monthGl").datepicker({ format: 'MM yyyy', viewMode: "months", minViewMode: "months", autoclose: true, todayBtn: 'linked' })
        }
    </script>
    <style>
        .header-right{
            text-align:right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-lg-3">
            <div class="form-group">
                <label for="usr">PMS Link:</label><font color=red>*</font>
                <asp:DropDownList ID="dlFOLink" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlFOLink_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="form-group">
                <label for="usr">Date:</label><font color=red>*</font>
                <div class="input-group date monthGl" style="padding:0;">
                    <asp:TextBox ID="tbDate" runat="server" CssClass ="form-control" placeholder="MM yyyy" OnTextChanged="cari" AutoPostBack="true"></asp:TextBox>
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="form-group">
                <label for="usr">Budget Group:</label><font color=red>*</font>
                <asp:DropDownList ID="dlBudgetGrp" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlBudgetGrp_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <hr style="margin-top:5px; margin-bottom:5px;" />
    <div class="row">
        <div class="col-lg-12">
            <div style="text-align:right">
                <asp:LinkButton ID="btnSavegrid" Width="100px" runat="server" CssClass="btn btn-default"><i class="fa fa-save"></i> save</asp:LinkButton>
            </div>
            <asp:GridView ID="gvBudget" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered" HeaderStyle-BackColor="#0a818e" Font-Size="Smaller" GridLines="None"
            HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" OnRowDataBound="OnRowDataBound" >
                <Columns>
                
                    <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" ItemStyle-HorizontalAlign="left" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdDate" runat="server"  Value='<%# Eval("date") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="group" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" ItemStyle-HorizontalAlign="left" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdGroup" runat="server"  Value='<%# Eval("Grp_Bud") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Code" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" ItemStyle-HorizontalAlign="left" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdCode" runat="server"  Value='<%# Eval("Code") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                  
                    <%--<asp:TemplateField HeaderText="Date" ItemStyle-HorizontalAlign="left" ItemStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("date","{0:dd MMM yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Group Budget" ItemStyle-HorizontalAlign="left" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="lblOpen" runat="server" Text='<%# if(Eval("Grp_Bud").toString()="01","Revenue Budget",if(Eval("Grp_Bud").toString()="02","Segment Market Budget",if(Eval("Grp_Bud").toString()="03","Source of Booking Budget","Sales Person Budget"))) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Description" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Code")+" - "+Eval("description") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="PMS Link" ItemStyle-HorizontalAlign="left" ItemStyle-Width="250px">
                        <ItemTemplate>
                            <asp:Label ID="lblPMS" runat="server" Text='<%# Eval("businessname") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Rev Bud"  ItemStyle-Width="15%">
                        <ItemTemplate>
                            <asp:TextBox Width="150px" ID="txtRevBud" CssClass="form-control" style="text-align:right"  runat="server" Text='<%# String.Format("{0:N2}", Eval("Revenue_Bud")  )  %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Pct Occ"  ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:TextBox Width="100px" ID="txtPct" CssClass="form-control" style="text-align:right"  runat="server" Text='<%# String.Format("{0:0}", Eval("PctOcc_Bud")  )  %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <%--<pagerstyle cssclass="pagination-ys">
                </pagerstyle>--%>
            </asp:GridView> 
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

