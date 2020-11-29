<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPageUpload.master" AutoEventWireup="false" CodeFile="iPxGLInputTransaction.aspx.vb" Inherits="iPxAdmin_iPxGLInputTransaction" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="JavaScript">
        // window.onbeforeunload = function(e) {
        //     var x = document.getElementById("<%=lblTotCredit.ClientID %>").innerText;
        //     var y = document.getElementById("<%=lblTotDebit.ClientID %>").innerText;
        //     console.log(x);
        //     console.log(y);
        //     if (x==y) {
        //         return;
        //     } else {
        //         return "Do you really want to leave page?";
        //     }
        // };
    </script>
    <script type="text/javascript">
        function confirmationDel() {
            if (confirm('Do you want to delete Jurnal detail ?')) {
            return true;
            }else{
            return false;
            }
        }
        function confirmationRicuring() {
            if (confirm('Do you want to Recuring Jurnal ?')) {
            return true;
            }else{
            return false;
            }
        }
    </script>
    <script>
        function hideModalAddGrp() {
            $('#formInputGrp').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalAddGrp() {
            $('#formInputGrp').modal({ backdrop: 'static',
                keyboard: false
            }, 'show');
        }
        function hideModalListCOA() {
            $('#formListCOA').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalListCOA() {
            $('#formListCOA').modal({ backdrop: 'static',
                keyboard: false
            }, 'show');
        }
        
        function hideModalListRecaring() {
            $('#formRecaring').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalListRecaring() {
            $('#formRecaring').modal({ backdrop: 'static',
                keyboard: false
            }, 'show');
        }
        function DateIsActive()
        {
            $("#GLDate").removeClass("tanggal");
        }
        
        function hideModalImport() {
            $('#formImport').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalImport() {
            $('#formImport').modal({ backdrop: 'static',
                keyboard: false
            },'show');
        }
    </script>
    <script type="text/javascript">
    
    function handleEnter(obj, event) {
        var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
        if (keyCode == 13) {
            //$("#<%=tbFindCoaList.ClientID %>").val(document.getElementById("<%=tbCoaDtl.ClientID %>").value);
            $("#<%=lbFindCoa.ClientID %>")[0].click();
            return false;
        }
        else {
            return true;
        }
    }

    function handleEnterAddDetail(obj, event) {
        var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
        if (keyCode == 13) {
            //$("#<%=tbFindCoaList.ClientID %>").val(document.getElementById("<%=tbCoaDtl.ClientID %>").value);
            $("#<%=lbAddDetail.ClientID %>")[0].click();
            return false;
        }
        else {
            return true;
        }
    }

    function iskeyPressReff(keyCode)
    {

    if (keyCode==13)
        {
            var textbox = document.getElementById("<%=tbDebitDtl.ClientID %>");
            textbox.focus();
        }

    }

    function iskeyPressDeb(keyCode)
    {

    if (keyCode==13)
        {
            var textbox = document.getElementById("<%=tbCreditDtl.ClientID %>");
            textbox.focus();
        }

    }

    function iskeyPressCre(keyCode)
    {

    if (keyCode==13)
        {
            $("#<%=lbAddDetail.ClientID %>")[0].click();
        }

    }
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Add Group Modal-->
    <div id="formInputGrp" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortGrp" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="login-modalLabel" class="modal-title">Form Group </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr">Id:</label><font color=red>*</font>
                                <asp:TextBox ID="tbIdGrp" runat="server" CssClass ="form-control" MaxLength="2"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="usr">Description:</label><font color=red>*</font>
                                <asp:TextBox ID="tbDescGrp" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <div class="square-blue pull-left pv-15">
                                    <label class="ui-checkbox">
                                    <asp:CheckBox  ID="cbActiveGrp" runat="server"  />
                                    <span> <strong> Active </strong> </span> </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbSaveGrp" runat="server" CssClass="btn btn-default"><i class="fa fa-save"></i> Save</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Add Group modal end-->
    
    <!-- Add COA Modal-->
    <div id="formListCOA" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md" style="width:1000px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortListCOA" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H2" class="modal-title">List COA </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <div class="input-group">
                                    <asp:TextBox ID="tbFindCoaList" runat="server" CssClass ="form-control"></asp:TextBox>
                                    <div class="input-group-btn">
                                        <asp:LinkButton ID="lbFindListCoa" class="btn btn-default" runat="server" Font-Size="Small"><span class="glyphicon glyphicon-search" style="height:20px;"></span></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-12">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvCoa" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="70px" HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbSelect" CssClass="btn btn-default" runat="server" CommandName="getSelect" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' CommandArgument='<%# Eval("Coa") %>'><i class="fa fa-check"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="50px" DataField="Coa" HeaderText="Coa" />
                                    <asp:BoundField ItemStyle-Width="100px" DataField="Description" HeaderText="Description" />
                                    <asp:BoundField ItemStyle-Width="70px" DataField="type" HeaderText="Type" />
                                    <asp:TemplateField ItemStyle-Width="60px" HeaderText="Group Level">
                                        <ItemTemplate>
                                            <asp:Label ID="lbGrpLevel" runat="server" Text='<%# if(Eval("grpLevel").toString()="G","Group","Detail") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="100px" HeaderText="Level">
                                        <ItemTemplate>
                                            <asp:Label ID="lbLevel" runat="server" Text='<%# if(Eval("levelid").toString()="1","Account type",if(Eval("levelid").toString()="2","Account Group",if(Eval("levelid").toString()="3","Account Sub Group 1",if(Eval("levelid").toString()="4","Account Sub Group 1","General")))) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="100px" DataField="Devision" HeaderText="Devision" />
                                    <asp:BoundField ItemStyle-Width="100px" DataField="Departement" HeaderText="Departement" />
                                    <asp:BoundField ItemStyle-Width="120px" DataField="SubDepartement" HeaderText="Sub Departement" />
                                    <asp:BoundField ItemStyle-Width="120px" DataField="status" HeaderText="Status" />
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
    <!-- Add COA modal end-->
    
    <!-- Add Recaring Modal-->
    <div id="formRecaring" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md" style="width:1300px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortRecaring" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H1" class="modal-title">List Recuring </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <div class="input-group">
                                    <asp:TextBox ID="tbSearchRecaring" runat="server" CssClass ="form-control"></asp:TextBox>
                                    <div class="input-group-btn">
                                        <asp:LinkButton ID="lbSearchRecaring" class="btn btn-default" runat="server" Font-Size="Small"><span class="glyphicon glyphicon-search" style="height:20px;"></span></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-5">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvRecaring" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="40px" HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbSelectRec" CssClass="btn btn-default" runat="server" CommandName="getSelect" CommandArgument='<%# Eval("TransID") %>' OnClientClick="return confirmationRicuring();"><i class="fa fa-check"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="50px" DataField="TransID" HeaderText="ID" />
                                    <asp:BoundField ItemStyle-Width="100px" DataField="GlGrp" HeaderText="Group" />
                                    <%--<asp:BoundField ItemStyle-Width="150px" DataField="ReffNo" HeaderText="Reff NO" />--%>
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
                                    <asp:TemplateField ItemStyle-Width="40px" HeaderText="Detail" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbDetailRec" CssClass="btn btn-default" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("TransID") %>' ><i class="fa fa-file-text-o"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <pagerstyle cssclass="pagination-ys">
                                </pagerstyle>
                            </asp:GridView>
                        </div>
                        <div class="col-lg-7">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvRecaringDtl" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10" ShowFooter="true">
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
    <!-- Add Recaring modal end-->
    <!-- Import Modal -->
    <div id="formImport" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortImport" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H5" class="modal-title">Import Journal(.xslx) </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <asp:FileUpload ID="flMassUpoad" runat="server" accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbStartImport" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> Import</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Import Modal end-->
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
                            <div id="GLDate" class="input-group date tanggal" style="padding:0;">
                                 <asp:TextBox ID="tbDate" runat="server" CssClass ="form-control" Enabled="false" OnTextChanged="cari" AutoPostBack="true" ></asp:TextBox>
                                 <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-3" style="padding:5px;">
                        <div class="form-group">
                            <label for="usr">reff no:</label><font color=red>*</font>
                            <asp:TextBox ID="tbReff" runat="server" CssClass ="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="usr">Group:</label><font color=red>*</font>
                            <%--<div class="input-group">--%>
                            <asp:DropDownList ID="dlGroup" runat="server" CssClass="form-control">
                            </asp:DropDownList> 
                                <%--<div class="input-group-btn">
                                    <asp:LinkButton ID="lbAddGroup" class="btn btn-default" runat="server" Font-Size="Small" Visible="false"><span class="glyphicon glyphicon-plus" style="height:20px;"></span> </asp:LinkButton>
                                </div>--%>
                            <%--</div>--%>
                        </div>
                    </div>
                    <div class="col-lg-1" style="padding:5px;">
                    </div>
                    <div class="col-lg-3" style="padding:5px;">
                        <div class="form-group">
                            <label for="usr">Description:</label>
                            <asp:TextBox ID="tbDesc" runat="server" CssClass ="form-control" textmode="multiline"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <hr style="margin: 5px; border-color:#0a818e; border-top: 2px solid #0a818e; margin-left:0; margin-right:0;"/>
                <div class="row" style="padding:5px; padding-top: 0;">
                    <div class="col-lg-2" style="padding:5px;">
                        <div class="form-group">
                            <label for="usr">Coa:</label>
                            <div class="input-group">
                                <asp:TextBox ID="tbCoaDtl" runat="server" CssClass ="form-control" onkeypress="return handleEnter('search-user',event);" ></asp:TextBox>
                                <div class="input-group-btn">
                                    <asp:LinkButton ID="lbFindCoa" class="btn btn-default" runat="server" Font-Size="Small"><span class="glyphicon glyphicon-search" style="height:20px;"></span></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-2" style="padding:5px; width:200px;">
                        <div class="form-group">
                            <label for="usr">Coa Desc:</label>
                            <asp:TextBox ID="tbCoaDescDtl" runat="server" CssClass ="form-control" TextMode="MultiLine" ReadOnly="true" Height="35px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-2" style="padding:5px; width:225px;">
                        <div class="form-group">
                            <label for="usr">Description:</label>
                            <asp:TextBox ID="tbDescDtl" runat="server" CssClass ="form-control" TextMode="MultiLine" placeholder="Description.." Height="35px" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-2" style="padding:5px; width:130px;">
                        <div class="form-group">
                            <label for="usr">Reff:</label>
                            <asp:TextBox ID="tbReffDtl" runat="server" CssClass ="form-control" Width="120px" onkeydown = "return iskeyPressReff(event.keyCode);"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-2" style="padding:5px; width:130px;">
                        <div class="form-group">
                            <label for="usr">Debit:</label>
                            <asp:TextBox ID="tbDebitDtl" runat="server" CssClass ="form-control" style="text-align:right;" placeholder="0.00" onkeydown = "return iskeyPressDeb(event.keyCode);"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-2" style="padding:5px;padding-bottom:0; width:130px;">
                        <div class="form-group">
                            <label for="usr">Credit:</label>
                            <asp:TextBox ID="tbCreditDtl" runat="server" CssClass ="form-control" style="text-align:right;" placeholder="0.00" onkeydown = "return iskeyPressCre(event.keyCode);"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12" style="padding:5px; padding-bottom:0;">
                        <div class="form-group" style="text-align:right;">
                            <asp:LinkButton ID="lbAddDetail" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> Add</asp:LinkButton>
                            <asp:LinkButton ID="lbCancelAdd" runat="server" CssClass="btn btn-danger" Visible="false"><i class="fa fa-close"></i> Abort</asp:LinkButton>
                            <asp:Label ID="lblTotCredit" Text="20" runat="server" hidden="hidden"/>
                            <asp:Label ID="lblTotDebit" Text="20" runat="server" hidden="hidden"/>
                        </div>
                    </div>
                </div>
            </div>
            
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">
            <asp:GridView EmptyDataText="No records has been added." ID="gvGLDetail" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10" showfooter="true">
                <Columns>
                    <asp:BoundField ItemStyle-Width="50px" DataField="RecID" HeaderText="Rec ID" />
                    <%--<asp:BoundField ItemStyle-Width="200px" DataField="CoaDesc" HeaderText="Coa" />--%>
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
                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbEdit" CssClass="btn btn-default" runat="server" CommandName="getEdit" Enabled='<%# if(eval("Status").toString()="O",if(eval("CloseStatus").toString()="Close","false",if(eval("editGL").toString()="Y",if(eval("autoPost").toString()="0","true","false"),"false")),"false") %>' CommandArgument='<%# Eval("RecID") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbDelete" CssClass="btn btn-default" runat="server" OnClientClick="return confirmationDel();"  CommandName="getDelete" Enabled='<%# if(eval("Status").toString()="O",if(eval("CloseStatus").toString()="Close","false",if(eval("editGL").toString()="Y",if(eval("autoPost").toString()="0","true","false"),"false")),"false") %>' CommandArgument='<%# Eval("RecID") %>'><i class="fa fa-trash-o"></i></asp:LinkButton>
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
    <div id="footer" class="container" >
        <asp:LinkButton ID="lbAbort" Width="125px" runat="server" CssClass="btn btn-default"><i class="fa fa-close"></i> Abort</asp:LinkButton>
        <asp:LinkButton ID="lbAbortAdd" Width="125px" runat="server" CssClass="btn btn-default"><i class="fa fa-save"></i> Save</asp:LinkButton>
        <asp:LinkButton ID="lbRecaring" Width="125px" runat="server" CssClass="btn btn-default"><i class="fa fa-save"></i> by Recuring</asp:LinkButton>
        <asp:LinkButton ID="lbImport" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> Import From Excel</asp:LinkButton>
        <%--<div class="btn-group">
            <button type="button" style="width:130px;" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
            <i class="fa fa-cogs"></i> Option <span class="caret"></span></button>
            <ul class="dropdown-menu" style="top:-70px; min-width: 100px;" role="menu">
                <li><asp:LinkButton ID="lbOption" runat="server" CssClass="btn" style="text-align:left;">Date Posting</asp:LinkButton></li>
                <li><asp:LinkButton ID="lbDetail" runat="server" CssClass="btn" style="text-align:left;">Detail Transaction</asp:LinkButton></li>
            </ul>
        </div>--%>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

