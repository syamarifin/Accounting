<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxARInvoice.aspx.vb" Inherits="iPxAdmin_iPxARInvoice" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        function confirmationDel() {
            if (confirm('Do you want to delete Invoice ?')) {
            return true;
            }else{
            return false;
            }
        }
    </script>
    <script type = "text/javascript">
    function Check_Click(objRef)
    {
        //Get the Row based on checkbox
        var row = objRef.parentNode.parentNode;
        
        //Get the reference of GridView
        var GridView = row.parentNode;
        
        //Get all input elements in Gridview
        var inputList = GridView.getElementsByTagName("input");
        var en=0;
        for (var i=0;i<inputList.length;i++)
        {
            //The First element is the Header Checkbox
            var headerCheckBox = inputList[0];
            
            //Based on all or none checkboxes
            //are checked check/uncheck Header Checkbox
            var checked = true;
            if(inputList[i].type == "checkbox" && inputList[i] != headerCheckBox)
            {
                if(inputList[i].disabled!=false){
                }else{
                    if(!inputList[i].checked)
                    {
                        checked = false;
                        break;
                    }
                }
            }
        }
        headerCheckBox.checked = checked;
    }
    function checkAll(objRef)
    {
        var GridView = objRef.parentNode.parentNode.parentNode;
        var inputList = GridView.getElementsByTagName("input");
        for (var i=0;i<inputList.length;i++)
        {
            var row = inputList[i].parentNode.parentNode;
            if(inputList[i].type == "checkbox"  && objRef != inputList[i])
            {
                if (objRef.checked)
                {
                    if(inputList[i].disabled!=false){}
                    else{
                        inputList[i].checked=true;
                        }
                }
                else
                {
                    inputList[i].checked=false;
                }
            }
        }
    }
    //-->
    </script>
    <script type = "text/javascript">
        function ConfirmDelete()
        {
            var count = document.getElementById("<%=hfCount.ClientID %>").value;
            var gv = document.getElementById("<%=gvARInv.ClientID%>");
            var chk = gv.getElementsByTagName("input");
            for(var i=0;i<chk.length;i++)
            {
                if(chk[i].checked && chk[i].id.indexOf("chkAll") == -1)
                {
                    count++;
                }
            }
            if(count == 0)
            {
                alert("No records to approved.");
                return false;
            }
            else
            {
                return confirm("Do you want to approved " + count + " records.");
            }
        }
        
        function ConfirmUndo()
        {
            var count = document.getElementById("<%=hfCountAppr.ClientID %>").value;
            var gv = document.getElementById("<%=gvApprove.ClientID%>");
            var chk = gv.getElementsByTagName("input");
            for(var i=0;i<chk.length;i++)
            {
                if(chk[i].checked && chk[i].id.indexOf("chkAll") == -1)
                {
                    count++;
                }
            }
            if(count == 0)
            {
                alert("No records to undo approved.");
                return false;
            }
            else
            {
                return confirm("Do you want to undo approved " + count + " records.");
            }
        }
    </script>
    <script>
        
        function ApproveActive()
        {
            $("#New-tab").removeClass("active");
            $("#Approve-tab").addClass("active");
            $("#Paid-tab").removeClass("active");
            $("#New").removeClass("active in");
            $("#Approve").addClass("active in");
            $("#Paid").removeClass("active in");
        }
        function PaidActive()
        {
            $("#New-tab").removeClass("active");
            $("#Approve-tab").removeClass("active");
            $("#Paid-tab").addClass("active");
            $("#New").removeClass("active in");
            $("#Approve").removeClass("active in");
            $("#Paid").addClass("active in");
        }
    </script>
    <style>
        .btn_right
        {
        	text-align:right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--List Docket Modal--%>
    <div id="formDocket" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortDocket" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H4" class="modal-title">Please select docket template first!.</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:GridView ID="gvDocket" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None">
                                <Columns>
                                    <%--<asp:BoundField ItemStyle-Width ="120px" DataField="businessid" HeaderText="Businessid" />--%>
                                    <asp:BoundField ItemStyle-Width ="50px" DataField="code" HeaderText="Code" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:BoundField ItemStyle-Width ="90px" DataField="fileName" HeaderText="File Name" />
                                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Print" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbPrint" CssClass="btn btn-default" runat="server" CommandName="getDocket" CommandArgument='<%# Eval("fileName") %>' ><i class="fa fa-print"></i></asp:LinkButton>
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
    <%--End List Docket Modal--%>
    <%--Delete Customer AR Modal--%>
    <div id="formDeleteTrans" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortDelete" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H3" class="modal-title">Do you want to delete Invoice ?</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="usr">Reason to delete Invoice :</label><font color=red>*</font>
                        <asp:TextBox ID="tbReason" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbDeleteRes" runat="server" CssClass="btn btn-default"><i class="fa fa-trash"></i> Delete</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <%--End Delete Customer AR Modal--%>
    <!-- Verived email invoice-->
    <div id="formVerivedEmail" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortVerived" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H2" class="modal-title">Send Invoice To</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Name:</label>
                                <asp:TextBox ID="tbEmailName" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Email:</label>
                                <asp:TextBox ID="tbEmailInv" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbSendEmail" runat="server" CssClass="btn btn-default"><i class="fa fa-envelope"></i> Send</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Verived email invoice end-->
    <!-- Query Customer AR Modal-->
    <div id="formQuery" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md" style="width:400px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortQuery" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="login-modalLabel" class="modal-title">Query Customer AR</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Invoice ID:</label>
                                <asp:TextBox ID="tbQInvID" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Invoice Date:</label>
                                <div class="input-group date datepicker" style="padding:0;">
                                    <asp:TextBox ID="tbQInvDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                                </div>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Due Date:</label>
                                <div class="input-group date datepicker" style="padding:0;">
                                    <asp:TextBox ID="tbQDueDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                                </div>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Customer Name:</label>
                                <asp:TextBox ID="tbQCustName" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Reff No:</label>
                                <asp:TextBox ID="tbQReff" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <asp:Label ID="lblStatus" runat="server" Text="Status:"></asp:Label>
                                <asp:DropDownList ID="dlQStatus" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div style="text-align:center;">
                                <asp:Label ID="Label1" runat="server" Text="By Periode Invoice"></asp:Label>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Periode From:</label>
                                <div class="input-group date datepicker" style="padding:0;">
                                    <asp:TextBox ID="tbQFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                                </div>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Periode Until:</label>
                                <div class="input-group date datepicker" style="padding:0;">
                                    <asp:TextBox ID="tbQUntil" runat="server" CssClass="form-control"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
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
    <!-- Query Customer AR modal end-->
    <!-- Add Customer Group Modal-->
    <div id="formInput" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-lg" style="width:1000px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <button type="button" data-dismiss="modal" aria-label="Close" class="close"><span aria-hidden="true">×</span></button>
                    <h4 id="H1" class="modal-title">Detail Invoice 
                        <asp:Label ID="lbIdInv" runat="server" Text=""></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div style="max-height:500px; overflow:auto;">
                                <asp:GridView ID="gvInvoice" runat="server" AutoGenerateColumns="false" CssClass="table" FooterStyle-Font-Bold="true" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" ShowFooter="true">
                                    <Columns>
                                        <asp:BoundField ItemStyle-Width ="50px" DataField="RecID" ItemStyle-CssClass="hidden" HeaderText="Rec ID" HeaderStyle-CssClass="hidden" />
                                        <asp:BoundField ItemStyle-Width ="120px" DataField="CoyName" HeaderText="Customer Name" />
                                        <asp:BoundField ItemStyle-Width ="90px" DataField="TransDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}"/>
                                        <asp:BoundField ItemStyle-Width ="70px" DataField="Description" HeaderText="Transaction Type" />
                                        <asp:BoundField ItemStyle-Width ="90px" DataField="arrival" HeaderText="Arrival" DataFormatString="{0:dd MMM yyyy}"/>
                                        <asp:BoundField ItemStyle-Width ="90px" DataField="departure" HeaderText="Departure" DataFormatString="{0:dd MMM yyyy}"/>
                                        <asp:BoundField ItemStyle-Width ="100px" DataField="voucherno" HeaderText="Voucher No" />
                                        <asp:BoundField ItemStyle-Width ="100px" DataField="foliono" HeaderText="Folio No" />
                                        <asp:BoundField ItemStyle-Width ="70px" DataField="RoomNo" HeaderText="Room No" />
                                        <asp:BoundField ItemStyle-Width ="100px" DataField="GuestName" HeaderText="Guest Name" />
                                        <asp:BoundField DataField="notes" HeaderText="Notes" />
                                        <asp:BoundField ItemStyle-Width ="100px" DataField="amountdr" HeaderText="Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right"/>
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
    </div>
    <!-- Add Customer Group modal end-->
    <div class="row">
        <div class="col-lg-12">
            <ul class="nav nav-tabs">
                <li id="New-tab" class="active">   <a href="#New" data-toggle="tab"><i class="fa fa-list"></i> New</a></li>
                <li id="Approve-tab">              <a href="#Approve" data-toggle="tab"><i class="fa fa-list"></i> Approved</a></li>
                <li id="Paid-tab">                 <a href="#Paid" data-toggle="tab"><i class="fa fa-list"></i> Paid</a></li>
            </ul>
            <div id="myTabContent" class="tab-content">
                <div class="tab-pane active in" id="New">
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:HiddenField ID="hfCount" runat="server" Value = "0" />
                            <asp:LinkButton ID="lbAddInv" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> New Invoice</asp:LinkButton>
                            <asp:LinkButton ID="lbAppr" Width="150px" runat="server" CssClass="btn btn-default" OnClientClick = "return ConfirmDelete();"><i class="fa fa-lock"></i> Approved</asp:LinkButton>
                            <asp:LinkButton ID="lbQuery" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
                        </div>
                        <div class="col-lg-12">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvARInv" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" DataKeyNames = "InvoiceNO" ShowFooter="true">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="20px">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAll" runat="server" onclick = "checkAll(this);" />
                                        </HeaderTemplate> 
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk" runat="server" Enabled='<%# if(eval("Status").toString()="N","true","false") %>' onclick = "Check_Click(this)"/>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:BoundField ItemStyle-Width="100px" DataField="InvoiceNo" HeaderText="Invoice No" />
                                    <asp:BoundField ItemStyle-Width="100px" DataField="InvDate" HeaderText="Invoice Date" DataFormatString="{0:dd MMM yyyy}"/>
                                    <asp:BoundField ItemStyle-Width="100px" DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd MMM yyyy}"/>
                                    <asp:BoundField ItemStyle-Width="100px" DataField="CoyName" HeaderText="Customer Name" />
                                    <asp:BoundField ItemStyle-Width="100px" DataField="ReffNo" HeaderText="Reff No" />
                                    <asp:BoundField DataField="Notes" HeaderText="Notes"/>
                                    <%--<asp:TemplateField ItemStyle-Width="50px" HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# if(Eval("Status").toString()="N","New",if(Eval("Status").toString()="X","Delete",if(Eval("Status").toString()="P","Paid","Approved"))) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmountTot" runat="server" Text='<%# if(Eval("amountTrans").toString()="","0.00",Eval("amountTrans","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="100px" DataField="fullname" HeaderText="by" />
                                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Print" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbPrint" CssClass="btn btn-default" runat="server" CommandName="getPrint" Enabled='<%# if(Eval("Status").toString()="N","true","false") %>' CommandArgument='<%# Eval("InvoiceNo") %>'><i class="fa fa-print"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Send" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbEmail" CssClass="btn btn-default" runat="server" CommandName="getEmail" CommandArgument='<%# Eval("InvoiceNo") %>' Enabled='<%# if(Eval("sendInv").toString()="Y","true","false") %>'><i class="fa fa-envelope"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Opsi" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <div class="btn-group">
                                                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" style="padding-bottom:11px; padding-top:11px;">
                                                <span class="caret"></span></button>
                                                <ul class="dropdown-menu" style="left:-60px; min-width: 100px;" role="menu">
                                                    <li><asp:LinkButton ID="lbDetail" CssClass="btn btn-link btn_right" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("InvoiceNo") %>'> Detail <i class="fa fa-file-text-o"></i></asp:LinkButton></li>
                                                    <li><asp:LinkButton ID="lbEdit" CssClass="btn btn-link btn_right" runat="server" CommandName="getEdit" Enabled='<%# if(Eval("Status").toString()="N",if(Eval("editInv").toString()="Y","true","false"),"false") %>' CommandArgument='<%# Eval("InvoiceNo") %>'> Edit <i class="fa fa-edit"></i></asp:LinkButton></li>
                                                    <li><asp:LinkButton ID="lbDelete" CssClass="btn btn-link btn_right" runat="server" OnClientClick="return confirmationDel();" CommandName="getDelete" Enabled='<%# if(Eval("Status").toString()="N",if(Eval("deleteInv").toString()="Y","true","false"),"false") %>' CommandArgument='<%# Eval("InvoiceNo") %>'> Delete <i class="fa fa-trash"></i></asp:LinkButton></li>
                                                </ul>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField ItemStyle-Width="50px" HeaderText="Detail" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbDetail" CssClass="btn btn-default" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("InvoiceNo") %>'><i class="fa fa-file-text-o"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbEdit" CssClass="btn btn-default" runat="server" CommandName="getEdit" Enabled='<%# if(Eval("Status").toString()="N","true","false") %>' CommandArgument='<%# Eval("InvoiceNo") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Print" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbPrint" CssClass="btn btn-default" runat="server" CommandName="getPrint" Enabled='<%# if(Eval("Status").toString()="N","true","false") %>' CommandArgument='<%# Eval("InvoiceNo") %>'><i class="fa fa-print"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                                <pagerstyle cssclass="pagination-ys">
                                </pagerstyle>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="tab-pane" id="Approve">
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:HiddenField ID="hfCountAppr" runat="server" Value = "0" />
                            <asp:LinkButton ID="lbUndo" Width="150px" runat="server" CssClass="btn btn-default" OnClientClick = "return ConfirmUndo();"><i class="fa fa-unlock-alt"></i> Undo Approved</asp:LinkButton>
                            <asp:LinkButton ID="lbQueryApprove" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
                        </div>
                        <div class="col-lg-12">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvApprove" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" DataKeyNames = "InvoiceNO" ShowFooter="true">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="20px">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAll" runat="server" onclick = "checkAll(this);" />
                                        </HeaderTemplate> 
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk" runat="server" onclick = "Check_Click(this)"/>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:BoundField ItemStyle-Width="100px" DataField="InvoiceNo" HeaderText="Invoice No" />
                                    <asp:BoundField ItemStyle-Width="100px" DataField="InvDate" HeaderText="Invoice Date" DataFormatString="{0:dd MMM yyyy}"/>
                                    <asp:BoundField ItemStyle-Width="100px" DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd MMM yyyy}"/>
                                    <asp:BoundField ItemStyle-Width="100px" DataField="CoyName" HeaderText="Customer Name" />
                                    <asp:BoundField ItemStyle-Width="100px" DataField="ReffNo" HeaderText="Reff No" />
                                    <asp:BoundField DataField="Notes" HeaderText="Notes"/>
                                    <%--<asp:TemplateField ItemStyle-Width="50px" HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# if(Eval("Status").toString()="N","New",if(Eval("Status").toString()="X","Delete",if(Eval("Status").toString()="P","Paid","Approved"))) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmountTot" runat="server" Text='<%# if(Eval("amountTrans").toString()="","0.00",Eval("amountTrans","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="100px" DataField="fullname" HeaderText="by" />
                                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Detail" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbDetail" CssClass="btn btn-default" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("InvoiceNo") %>'><i class="fa fa-file-text-o"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField ItemStyle-Width="50px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbEdit" CssClass="btn btn-default" runat="server" CommandName="getEdit" Enabled='<%# if(Eval("Status").toString()="N","true","false") %>' CommandArgument='<%# Eval("InvoiceNo") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Print" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbPrint" CssClass="btn btn-default" runat="server" CommandName="getPrint" CommandArgument='<%# Eval("InvoiceNo") %>'><i class="fa fa-print"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Send" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbEmail" CssClass="btn btn-default" runat="server" CommandName="getEmail" CommandArgument='<%# Eval("InvoiceNo") %>' Enabled='<%# if(Eval("sendInv").toString()="Y","true","false") %>'><i class="fa fa-envelope"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <pagerstyle cssclass="pagination-ys">
                                </pagerstyle>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="tab-pane" id="Paid">
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:LinkButton ID="lbQueryPaid" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
                        </div>
                        <div class="col-lg-12">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvPaid" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" DataKeyNames = "InvoiceNO" ShowFooter="true">
                                <Columns>
                                    <asp:BoundField ItemStyle-Width="100px" DataField="InvoiceNo" HeaderText="Invoice No" />
                                    <asp:BoundField ItemStyle-Width="100px" DataField="InvDate" HeaderText="Invoice Date" DataFormatString="{0:dd MMM yyyy}"/>
                                    <asp:BoundField ItemStyle-Width="100px" DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd MMM yyyy}"/>
                                    <asp:BoundField ItemStyle-Width="100px" DataField="CoyName" HeaderText="Customer Name" />
                                    <asp:BoundField ItemStyle-Width="100px" DataField="ReffNo" HeaderText="Reff No" />
                                    <asp:BoundField DataField="Notes" HeaderText="Notes"/>
                                    <%--<asp:TemplateField ItemStyle-Width="50px" HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# if(Eval("Status").toString()="N","New",if(Eval("Status").toString()="X","Delete",if(Eval("Status").toString()="P","Paid","Approved"))) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmountTot" runat="server" Text='<%# if(Eval("amountTrans").toString()="","0.00",Eval("amountTrans","{0:N2}")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="100px" DataField="fullname" HeaderText="by" />
                                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Detail" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbDetail" CssClass="btn btn-default" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("InvoiceNo") %>'><i class="fa fa-file-text-o"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Print" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbPrint" CssClass="btn btn-default" runat="server" CommandName="getPrint" CommandArgument='<%# Eval("InvoiceNo") %>'><i class="fa fa-print"></i></asp:LinkButton>
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

