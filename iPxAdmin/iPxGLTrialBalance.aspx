<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxGLTrialBalance.aspx.vb" Inherits="iPxAdmin_iPxGLTrialBalance" title="Alcor Accounting" %>

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
    <!-- Detail Journal Modal-->
    <div id="formAddTrans" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortJournalTrans" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H1" class="modal-title">
                        <asp:Label ID="Label1" runat="server" Text="Detail Journal "></asp:Label>
                    </h4>
                </div>
                <div class="modal-body">
                    <div id ="done" class="col-lg-12 col-xs-12">
                        <!-- small box -->
                        <div class="small-box" style="background-color:White; margin-bottom:10px; border-top: 3px solid #0a818e; padding-bottom:0;">
                            <div class="inner" style="padding-bottom:0;">
                                <div class="row" style="padding:5px; padding-bottom: 0;">
                                    <div class="col-lg-3" style="padding:5px;">
                                        <div class="form-group">
                                            <label for="usr">Transaction ID:</label>
                                            <asp:TextBox ID="tbTransID" runat="server" CssClass ="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="usr">Date:</label><font color=red>*</font>
                                            <div id="GLDate" class="input-group" style="padding:0;">
                                                 <asp:TextBox ID="TextBox1" runat="server" CssClass ="form-control" Enabled="false" OnTextChanged="cari" AutoPostBack="true" ></asp:TextBox>
                                                 <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-3" style="padding:5px;">
                                        <div class="form-group">
                                            <label for="usr">reff no:</label><font color=red>*</font>
                                            <asp:TextBox ID="tbReff" runat="server" CssClass ="form-control" Enabled="false" ></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="usr">Group:</label><font color=red>*</font>
                                            <asp:TextBox ID="tbGroup" runat="server" CssClass ="form-control" Enabled="false" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6" style="padding:5px;">
                                        <div class="form-group">
                                            <label for="usr">Description:</label>
                                            <asp:TextBox ID="tbDesc" runat="server" CssClass ="form-control" textmode="multiline" Enabled="false" ></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvGLDetail" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" ShowFooter="true" AllowPaging="true" PageSize="10">
                                <Columns>
                                    <asp:BoundField ItemStyle-Width="50px" DataField="RecID" HeaderText="Rec ID" />
                                    <asp:TemplateField HeaderText="Chart Of Account" ItemStyle-Width="200">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCoa" Text='<%# Eval("Coa").ToString()+" - "+Eval("CoaDesc").ToString() %>'
                                            runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:BoundField ItemStyle-Width="100px" DataField="Reff" HeaderText="Reff" />
                                    <asp:TemplateField HeaderText="Debit" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDebit" Text='<%# If(Eval("Debit").ToString() = "0.0000", "0.00", String.Format("{0:N2}", (Eval("Debit")))) %>'
                                            runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Credit" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCredit" Text='<%# If(Eval("Credit").ToString() = "0.0000", "0.00", String.Format("{0:N2}", (Eval("Credit")))) %>'
                                            runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <pagerstyle cssclass="pagination-ys">
                                </pagerstyle>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Detail Journal Modal End-->
    <!-- Detail Transaction Modal-->
    <div id="formInput" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortDetail" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="login-modalLabel" class="modal-title">
                        <asp:Label ID="lbTitleDetailCoa" runat="server" Text="Detail Journal "></asp:Label>
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvDetail" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" ShowFooter="true" AllowPaging="true" PageSize="10">
                                <Columns>
                                    <asp:BoundField DataField="TransID" HeaderText="Trans ID" />
                                    <asp:BoundField DataField="GroupGL" HeaderText="Group" />
                                    <asp:BoundField DataField="TransDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}"/>
                                    <%--<asp:BoundField DataField="PrevDeb" HeaderText="Previous Debit" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right"/>
                                    <asp:BoundField DataField="PrevCre" HeaderText="Previous Credit" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right"/>
                                    <asp:BoundField DataField="amountDeb" HeaderText="Current Balance" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="cellOneCellPaddingRight"/>
                                    <asp:BoundField DataField="amountCre" HeaderText="Current Credit" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right"/>--%>
                                    <%--<asp:TemplateField HeaderText="Previous Debit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrevDeb" runat="server" Text='<%# if(Eval("PrevDeb").toString()="","0.00",Eval("PrevDeb","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Previous Credit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrevCre" runat="server" Text='<%# if(Eval("PrevCre").toString()="","0.00",Eval("PrevCre","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> --%>     
                                    <asp:TemplateField HeaderText="Debit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurDeb" runat="server" Text='<%# if(Eval("amountDeb").toString()="","0.00",Eval("amountDeb","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Credit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="cellOneCellPaddingRight">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurCre" runat="server" Text='<%# if(Eval("amountCre").toString()="","0.00",Eval("amountCre","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbDetail" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" Enabled='<%# if(Eval("TransID").toString()="GLOpen","false","true") %>' runat="server" CommandName="getDetail" CommandArgument='<%# Eval("TransID") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>  
                                    <%--<asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbDetaillv2" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("TransID") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                                <pagerstyle cssclass="pagination-ys">
                                </pagerstyle>
                            </asp:GridView>
                            <asp:LinkButton ID="lbPrintDtl" runat="server" CssClass="btn btn-default"><i class="fa fa-print"></i> Print</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Detail Journal modal end-->
    <div class="row">
        <div class="col-md-1" style="text-align:right;">
            <div class="form-group">
                <label for="usr">Month:</label>
            </div>
        </div>
        <div class="col-md-3">
            <div class="form-group">
                <div class="input-group date monthGl" style="padding:0;">
                    <asp:TextBox ID="tbDate" runat="server" CssClass ="form-control" placeholder="MM yyyy" OnTextChanged="cari" AutoPostBack="true"></asp:TextBox>
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
        </div>
        <div class="col-md-2" style="padding-right:5px;">
            <asp:LinkButton ID="lbPrint" runat="server" CssClass="btn btn-default btn-block"><i class="fa fa-print"></i> Print</asp:LinkButton>
        </div>
        <div class="col-md-2" style="padding-left:5px;">
            <%--<asp:LinkButton ID="lbQuery" runat="server" CssClass="btn btn-default btn-block"><i class="fa fa-filter"></i> Query</asp:LinkButton>--%>
            <asp:DropDownList ID="dlGrp" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlGrp_SelectedIndexChanged">
                <asp:ListItem Value="">All</asp:ListItem>
                <asp:ListItem Value="Asset">Assets</asp:ListItem>
                <asp:ListItem Value="Liability">Liability</asp:ListItem>
                <asp:ListItem Value="Equity">Equity</asp:ListItem>
                <asp:ListItem Value="Revenue">Revenue</asp:ListItem>
                <asp:ListItem Value="Cost">Cost</asp:ListItem>
                <asp:ListItem Value="Expenses">Expenses</asp:ListItem>
                <asp:ListItem Value="Statistic">Statistic</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-lg-2" style="padding-top :10px">
            <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBox1_CheckedChanged"/> Hide Zero Account
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">
            <asp:GridView EmptyDataText="No records has been added." ID="gvTrialBalance" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="20" ShowFooter="true">
                <Columns>
                    <asp:BoundField ItemStyle-Width="70px" DataField="Coa" HeaderText="Coa" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="type" HeaderText="Act" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" FooterStyle-CssClass="hidden"/>
                    <asp:TemplateField HeaderText="Previous Debit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="125px">
                        <ItemTemplate>
                            <asp:Label ID="lblPrevDeb" runat="server" Text='<%# if(Eval("PrevDebit").toString()="","0.00",Eval("PrevDebit","{0:N2}")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Previous Credit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="125px">
                        <ItemTemplate>
                            <asp:Label ID="lblPrevCre" runat="server" Text='<%# if(Eval("PrevCredit").toString()="","0.00",Eval("PrevCredit","{0:N2}")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>      
                    <asp:TemplateField HeaderText="Current Debit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="125px">
                        <ItemTemplate>
                            <asp:Label ID="lblCurDeb" runat="server" Text='<%# if(Eval("CurDebit").toString()="","0.00",Eval("CurDebit","{0:N2}")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Current Credit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="125px">
                        <ItemTemplate>
                            <asp:Label ID="lblCurCre" runat="server" Text='<%# if(Eval("CurCredit").toString()="","0.00",Eval("CurCredit","{0:N2}")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Balance Debit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="125px">
                        <ItemTemplate>
                            <asp:Label ID="lblBalanceDebit" runat="server" Text='<%# if(Eval("NetDebit").toString()="","0.00",Eval("NetDebit","{0:N2}")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Balance Credit" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="125px" ItemStyle-CssClass="cellOneCellPaddingRight">
                        <ItemTemplate>
                            <asp:Label ID="lblBalanceCredit" runat="server" Text='<%# if(Eval("NetCredit").toString()="","0.00",Eval("NetCredit","{0:N2}")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbDetailAsset" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("Coa") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>            
                </Columns>
                <pagerstyle cssclass="pagination-ys">
                </pagerstyle>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

