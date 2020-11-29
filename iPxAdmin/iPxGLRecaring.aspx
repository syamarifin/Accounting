<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxGLRecaring.aspx.vb" Inherits="iPxAdmin_iPxGLRecaring" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
       function confirmationVer() {
           if (confirm('Do you want to verify Jurnal ?')) {
           return true;
           }else{
           return false;
           }
       }
       function confirmationUnver() {
           if (confirm('Do you want to undo verify Jurnal ?')) {
           return true;
           }else{
           return false;
           }
       }
       function confirmationDel() {
           if (confirm('Do you want to delete Jurnal ?')) {
           return true;
           }else{
           return false;
           }
       }
       function confirmationRec() {
           if (confirm('Do you want to restore jurnal ?')) {
           return true;
           }else{
           return false;
           }
       }
   </script>
   <style>
        .text-right
        {
        	text-align:right;
        }
        .btn_right
        {
        	text-align:right;
        }
    </style>
    <script type="text/javascript">
        function MonthGL() {
            $(".monthGl").datepicker({ format: 'mm-yyyy', viewMode: "months", minViewMode: "months", autoclose: true, todayBtn: 'linked' })
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--Delete GL Jurnal Modal--%>
    <div id="formDeleteTrans" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortDelete" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H2" class="modal-title">Do you want to delete Recuring ?</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="usr">Reason to delete GL Recuring :</label><font color=red>*</font>
                        <asp:TextBox ID="tbReason" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbDelete" runat="server" CssClass="btn btn-default"><i class="fa fa-trash"></i> Delete</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <%--End Delete GL Jurnal Modal--%>
    <!-- Edit GL Header Modal-->
    <div id="formInput" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm" style="width:550px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortEditHeader" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="login-modalLabel" class="modal-title">Form COA </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-6">
                            <div class="form-group">
                                <label for="usr">Transaction ID:</label>
                                <asp:TextBox ID="tbTransID" runat="server" CssClass ="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">reff no:</label><font color=red>*</font>
                                <asp:TextBox ID="tbReff" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-group">
                                <label for="usr">Date:</label><font color=red>*</font>
                                <div class="input-group date datepicker" style="padding:0;">
                                     <asp:TextBox ID="tbDate" runat="server" CssClass ="form-control" placeholder="dd-MM-yyyy"></asp:TextBox>
                                     <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="usr">Group:</label><font color=red>*</font>
                                <div class="input-group">
                                <asp:DropDownList ID="dlGroup" runat="server" CssClass="form-control">
                                </asp:DropDownList> 
                                    <div class="input-group-btn">
                                        <asp:LinkButton ID="lbAddGroup" class="btn btn-default" runat="server" Font-Size="Small"><span class="glyphicon glyphicon-plus" style="height:20px;"></span></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr">Description:</label>
                                <asp:TextBox ID="tbDesc" runat="server" CssClass ="form-control" textmode="multiline"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbUpdate" runat="server" CssClass="btn btn-default"><i class="fa fa-edit"></i> Update</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Edit GL Header modal end-->
    
    <!-- Query GL Header Modal-->
    <div id="formQuery" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortQuery" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H1" class="modal-title">Query GL Recuring </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr">Reff No:</label>
                                <asp:TextBox ID="tbQReff" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Date:</label><font color=red>*</font>
                                <div class="input-group date datepicker" style="padding:0;">
                                     <asp:TextBox ID="tbQDate" runat="server" CssClass ="form-control" placeholder="dd/MM/yyyy"></asp:TextBox>
                                     <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="usr">Group:</label>
                                <asp:DropDownList ID="dlQGrp" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="usr">Status:</label>
                                <asp:DropDownList ID="dlQStatus" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="usr" style="text-align:center;">By Periode</label>
                            </div>
                            <div class="form-group">
                                <label for="usr">Periode From:</label><font color=red>*</font>
                                <div class="input-group date datepicker" style="padding:0;">
                                     <asp:TextBox ID="tbQFrom" runat="server" CssClass ="form-control" placeholder="dd/MM/yyyy"></asp:TextBox>
                                     <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="usr">Periode Until:</label><font color=red>*</font>
                                <div class="input-group date datepicker" style="padding:0;">
                                     <asp:TextBox ID="tbQUntil" runat="server" CssClass ="form-control" placeholder="dd/MM/yyyy"></asp:TextBox>
                                     <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lblQuery" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Query GL Header modal end-->
    <div class="row">
        <div class="col-lg-9">
            <asp:LinkButton ID="lbAddGL" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> New Recuring</asp:LinkButton>
            <asp:LinkButton ID="lbQuery" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
        </div>
        <%--<div class="col-lg-3">
            <div class="form-group" style="margin-bottom:3px;">
                <div class="input-group date monthGl" style="padding:0;">
                    <asp:TextBox ID="tbDateWork" runat="server" CssClass="form-control" Enabled="false" OnTextChanged="TransdateWork" AutoPostBack="true"></asp:TextBox>
                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                </div>
            </div>
        </div>--%>
        <div class="col-lg-12" style="margin-bottom:30px;">
            <asp:GridView EmptyDataText="No records has been added." ID="gvGL" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                <Columns>
                    <asp:BoundField ItemStyle-Width="50px" DataField="TransID" HeaderText="ID" />
                    <%--<asp:BoundField ItemStyle-Width="100px" DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date" />--%>
                    <asp:BoundField ItemStyle-Width="100px" DataField="GlGrp" HeaderText="Group" />
                    <asp:BoundField ItemStyle-Width="150px" DataField="ReffNo" HeaderText="Reff NO" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:TemplateField HeaderText="Debit" ItemStyle-Width="100" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lblDebit" Text='<%# If(Eval("totDebit").ToString() = "0.0000", "0.00", String.Format("{0:N2}", (Eval("totDebit")))) %>'
                            runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Credit" ItemStyle-Width="100" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lblCredit" Text='<%# If(Eval("totCredit").ToString() = "0.0000", "0.00", String.Format("{0:N2}", (Eval("totCredit")))) %>'
                            runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField ItemStyle-Width="50px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbEdit" CssClass="btn btn-default" runat="server" CommandName="getEdit" CommandArgument='<%# Eval("TransID") %>' Enabled='<%# if(eval("Status").toString()="O","true","false") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <%--<asp:TemplateField ItemStyle-Width="50px" HeaderText="Print" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbPrint" CssClass="btn btn-default" runat="server" CommandName="getPrint" CommandArgument='<%# Eval("TransID") %>'><i class="fa fa-print"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Opsi" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <div class="btn-group">
                                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" style="padding-bottom:11px; padding-top:11px;">
                                <span class="caret"></span></button>
                                <ul class="dropdown-menu" style="left:-70px; min-width: 100px;" role="menu">
                                    <li><asp:LinkButton ID="lbListDtl" CssClass="btn btn-link btn_right" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("TransID") %>'> Edit <i class="fa fa-edit"></i></asp:LinkButton></li>
                                    <li><asp:LinkButton ID="lbDelete" CssClass="btn btn-link btn_right" runat="server" Enabled='<%# if(eval("deleteGL").toString()="Y","true","false") %>' OnClientClick='<%# if(eval("Status").toString()="D","return confirmationRec();","return confirmationDel();") %>' CommandName='<%# if(eval("Status").toString()="D","getRestore","getDelete") %>' CommandArgument='<%# Eval("TransID") %>'> <%# if(eval("Status").toString()="D","Restore","Delete") %> <i class='<%# if(eval("Status").toString()="D","fa fa-recycle","fa fa-trash") %>'></i></asp:LinkButton></li>
                                </ul>
                            </div>
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

