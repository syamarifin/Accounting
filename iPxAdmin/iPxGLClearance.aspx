<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxGLClearance.aspx.vb" Inherits="iPxAdmin_iPxGLClearance" title="Alcor Accounting" %>

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
        .cellOneCellPaddingLeft {
            padding-left: 0pt !important;
        }
        .cellOneCellPaddingRight {
            padding-right: 0pt !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Add Transaction Type Modal-->
    <div id="formInput" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md" style="width:750px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <button type="button" data-dismiss="modal" aria-label="Close" class="close"><span aria-hidden="true">×</span></button>
                    <h4 id="H1" class="modal-title">Detail Journal 
                        <asp:Label ID="lbDateDetail" runat="server" Text="Label"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <div class="row" style="overflow-x:auto; max-height:500px;">
                        <div class="col-lg-12">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvDetailGl" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" ShowFooter="true">
                                <Columns>
                                    <asp:BoundField ItemStyle-Width="70px" DataField="TransID" HeaderText="Trans ID" />
                                    <asp:BoundField ItemStyle-Width="100px" DataField="GlGrp" HeaderText="Group" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:BoundField ItemStyle-Width="100px" DataField="Reff" HeaderText="Reff" />
                                    <asp:TemplateField HeaderText="Debit" HeaderStyle-CssClass="header-right" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDebit" Text='<%# If(Eval("Debit").ToString() = "0.0000", "0.00", String.Format("{0:N2}", (Eval("Debit")))) %>'
                                            runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Credit" HeaderStyle-CssClass="header-right" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="cellOneCellPaddingRight">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCredit" Text='<%# If(Eval("Credit").ToString() = "0.0000", "0.00", String.Format("{0:N2}", (Eval("Credit")))) %>'
                                            runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbDetail" runat="server" CssClass="btn btn-link" CommandName="getDetail" CommandArgument='<%# Eval("TransID") %>' style="color:#083765; padding:0; font-size:24px;"><i class="fa fa-caret-down"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Add Transaction Type modal end-->
    <!-- Detail Coa Transaction-->
    <div id="formType" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md" style="width:850px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortDetailLv2" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H2" class="modal-title">
                        <asp:Label ID="Label4" runat="server" Text="Detail Journal "></asp:Label>
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvDetailJournalTrans" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" ShowFooter="true">
                                <Columns>
                                    <asp:BoundField DataField="RecID" HeaderText="Rec ID" />
                                    <asp:BoundField DataField="Coa" HeaderText="COA" />
                                    <asp:BoundField DataField="coaDesc" HeaderText="COA Description" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:BoundField DataField="Reff" HeaderText="Reff No" />
                                    <asp:BoundField DataField="debit" HeaderText="Debit" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right"/>
                                    <asp:BoundField DataField="credit" HeaderText="Credit" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right"/>
                                    <asp:BoundField DataField="balance" HeaderText="Balance" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right"/>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Detail Coa Transaction end-->
    <div class="row">
        <div class="col-md-3">
            <div class="form-group">
                <label for="usr">Month:</label><font color=red>*</font>
                <div class="input-group date monthGl" style="padding:0;">
                    <asp:TextBox ID="tbDate" runat="server" CssClass ="form-control" placeholder="dd/MM/yyyy" OnTextChanged="cari" AutoPostBack="true"></asp:TextBox>
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-6">
            <asp:GridView EmptyDataText="No records has been added." ID="gvCashSummary" runat="server" AutoGenerateColumns="false" 
                CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" 
                GridLines="None" AllowPaging="true" PageSize="10">
                <Columns>
                <%--<asp:BoundField ItemStyle-Width="90px" DataField="TransDate" HeaderText="Date"  DataFormatString="{0:dd-MM-yyyy}"/>--%>
                    <asp:BoundField ItemStyle-Width="90px" DataField="Coa" HeaderText="COA" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:TemplateField HeaderText="Debit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblDebit" runat="server" Text='<%# if(Eval("amountDebit").toString()="","0.00",Eval("amountDebit","{0:N2}")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Credit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="110px" ItemStyle-CssClass="cellOneCellPaddingRight">
                        <ItemTemplate>
                            <asp:Label ID="lblCredit" runat="server" Text='<%# if(Eval("amountCredit").toString()="","0.00",Eval("amountCredit","{0:N2}")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbDetail" runat="server" CssClass="btn btn-link" CommandName="getDetail" CommandArgument='<%# Eval("Coa") %>' style="color:#083765; padding:0; font-size:24px;"><i class="fa fa-caret-down"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <pagerstyle cssclass="pagination-ys">
                </pagerstyle>
            </asp:GridView>
        </div>
        <div class="col-lg-6">
            <div style="overflow-x:auto; max-width:500px;">
                <asp:GridView EmptyDataText="No records has been added." ID="gvDetailCash" runat="server" AutoGenerateColumns="false" 
                CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" 
                GridLines="None" AllowPaging="true" PageSize="10" ShowFooter="true">
                    <Columns>
                        <asp:BoundField DataField="day" HeaderText="Date" />
                        <asp:TemplateField HeaderText="Debit" HeaderStyle-CssClass="header-right" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblDebit" Text='<%# If(Eval("Debit").ToString() = "0.0000", "0.00", String.Format("{0:N2}", (Eval("Debit")))) %>'
                                runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Credit" HeaderStyle-CssClass="header-right" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblCredit" Text='<%# If(Eval("Credit").ToString() = "0.0000", "0.00", String.Format("{0:N2}", (Eval("Credit")))) %>'
                                runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Balance" HeaderStyle-CssClass="header-right" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="cellOneCellPaddingRight">
                            <ItemTemplate>
                                <asp:Label ID="lblBalance" Text='<%# If(Eval("balance").ToString() = "0.0000", "0.00", String.Format("{0:N2}", (Eval("balance")))) %>'
                                runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft" ItemStyle-Width="10px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbDetail" runat="server" CssClass="btn btn-link" CommandName="getDetail" CommandArgument='<%# Eval("day") %>' style="color:#083765; padding:0; font-size:24px;"><i class="fa fa-caret-down"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <pagerstyle cssclass="pagination-ys">
                    </pagerstyle>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

