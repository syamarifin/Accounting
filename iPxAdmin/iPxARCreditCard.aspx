<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxARCreditCard.aspx.vb" Inherits="iPxAdmin_iPxARCreditCard" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .cellOneCellPaddingLeft {
            padding-left: 0pt !important;
        }
        .cellOneCellPaddingRight {
            padding-right: 0pt !important;
        }
    </style>
    <script type = "text/javascript">
    <!--
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
        function ConfirmCreateBath()
        {
            var count = document.getElementById("<%=hfCountOpen.ClientID %>").value;
            var gv = document.getElementById("<%=gvTransOpen.ClientID%>");
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
                alert("No records to Bath.");
                return false;
            }
            else
            {
                
            }
        }
        function ConfirmCreateClear()
        {
            var count = document.getElementById("<%=hfCountBath.ClientID %>").value;
            var gv = document.getElementById("<%=gvTransBath.ClientID%>");
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
                alert("No records to Clearance.");
                return false;
            }
            else
            {
                
            }
            $("#open-tab").removeClass("active");
            $("#clear-tab").removeClass("active");
            $("#bath-tab").addClass("active");
            $("#posted-tab").removeClass("active");
            $("#Open").removeClass("active in");
            $("#Clear").removeClass("active in");
            $("#Batch").addClass("active in");
            $("#Posted").removeClass("active in");
        }
        function ConfirmBath()
        {
            var count = document.getElementById("<%=hfCountOpen.ClientID %>").value;
            var gv = document.getElementById("<%=gvTransOpen.ClientID%>");
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
                alert("No records to Bath.");
                return false;
            }
            else
            {
                return confirm("Do you want to Bath " + count + " transaction.");
            }
        }
        function ConfirmClear()
        {
            var count = document.getElementById("<%=hfCountBath.ClientID %>").value;
            var gv = document.getElementById("<%=gvTransBath.ClientID%>");
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
                alert("No records to Clearance.");
                return false;
            }
            else
            {
                return confirm("Do you want to Clearance " + count + " transaction.");
            }
            $("#open-tab").removeClass("active");
            $("#clear-tab").removeClass("active");
            $("#bath-tab").addClass("active");
            $("#posted-tab").removeClass("active");
            $("#Open").removeClass("active in");
            $("#Clear").removeClass("active in");
            $("#Batch").addClass("active in");
            $("#Posted").removeClass("active in");
        }
        function ConfirmPostClear()
        {
            var count = document.getElementById("<%=hfCountClear.ClientID %>").value;
            var gv = document.getElementById("<%=gvTransClear.ClientID%>");
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
                alert("No records to Posting.");
                return false;
            }
            else
            {
                return confirm("Do you want to Posting " + count + " transaction.");
            }
            $("#open-tab").removeClass("active");
            $("#clear-tab").addClass("active");
            $("#bath-tab").removeClass("active");
            $("#posted-tab").removeClass("active");
            $("#Open").removeClass("active in");
            $("#Clear").addClass("active in");
            $("#Batch").removeClass("active in");
            $("#Posted").removeClass("active in");
        }
    </script>
    <script>
        function ClearActive()
        {
            $("#open-tab").removeClass("active");
            $("#clear-tab").addClass("active");
            $("#bath-tab").removeClass("active");
            $("#posted-tab").removeClass("active");
            $("#Open").removeClass("active in");
            $("#Clear").addClass("active in");
            $("#Batch").removeClass("active in");
            $("#Posted").removeClass("active in");
        }
        function BatchActive()
        {
            $("#open-tab").removeClass("active");
            $("#clear-tab").removeClass("active");
            $("#bath-tab").addClass("active");
            $("#posted-tab").removeClass("active");
            $("#Open").removeClass("active in");
            $("#Clear").removeClass("active in");
            $("#Batch").addClass("active in");
            $("#Posted").removeClass("active in");
        }
        function PostedActive()
        {
            $("#open-tab").removeClass("active");
            $("#clear-tab").removeClass("active");
            $("#bath-tab").removeClass("active");
            $("#posted-tab").addClass("active");
            $("#Open").removeClass("active in");
            $("#Clear").removeClass("active in");
            $("#Batch").removeClass("active in");
            $("#Posted").addClass("active in");
        }
    </script>
    <script>
        function confirmationBatch() {
           if (confirm('are you sure you want to delete batch ?')) {
           return true;
           }else{
           return false;
           }
       }
       function confirmationClear() {
           if (confirm('are you sure you want to delete clearance ?')) {
           return true;
           }else{
           return false;
           }
       }
        function hideModalCreatBath() {
            $('#formBath').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalCreatBath() {
            $('#formBath').modal({ backdrop: 'static',
                keyboard: false
            }, 'show');
        }
        function hideModalCreatClear() {
            $('#formClear').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalCreatClear() {
            $('#formClear').modal({ backdrop: 'static',
                keyboard: false
            }, 'show');
        }
        function hideModalDetailBath() {
            $('#FormListDetailBath').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalDetailBath() {
            $('#FormListDetailBath').modal({ backdrop: 'static',
                keyboard: false
            }, 'show');
        }
        function hideModalDetailClear() {
            $('#FormListDetailClear').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalDetailClear() {
            $('#FormListDetailClear').modal({ backdrop: 'static',
                keyboard: false
            }, 'show');
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Query Cust Modal-->
    <div id="formQuery" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <button type="button" data-dismiss="modal" aria-label="Close" class="close"><span aria-hidden="true">×</span></button>
                    <h4 id="H3" class="modal-title">
                        <asp:Label ID="Label1" runat="server" Text="Query Customer "></asp:Label>
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:Panel ID="pnOpen" runat="server">
                                <div class="form-group" style="margin-bottom:3px;">
                                    <label for="usr">Customer ID :</label>
                                    <asp:TextBox ID="tbQCustOpen" runat="server" CssClass ="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group" style="margin-bottom:3px;">
                                    <label for="usr">Coy Name :</label>
                                    <asp:TextBox ID="tbQCoyOpen" runat="server" CssClass ="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group" style="margin-bottom:3px;">
                                    <label for="usr">Bank Clearance :</label>
                                    <asp:TextBox ID="tbQBankOpen" runat="server" CssClass ="form-control"></asp:TextBox>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnBath" runat="server">
                                <div class="form-group" style="margin-bottom:3px;">
                                    <label for="usr">Customer ID :</label>
                                    <asp:TextBox ID="tbQCustBath" runat="server" CssClass ="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group" style="margin-bottom:3px;">
                                    <label for="usr">Coy Name :</label>
                                    <asp:TextBox ID="tbQCoyBath" runat="server" CssClass ="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group" style="margin-bottom:3px;">
                                    <label for="usr">Bank Clearance :</label>
                                    <asp:TextBox ID="tbQBankBath" runat="server" CssClass ="form-control"></asp:TextBox>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnClear" runat="server">
                                <div class="form-group" style="margin-bottom:3px;">
                                    <label for="usr">Customer ID :</label>
                                    <asp:TextBox ID="tbQCustClear" runat="server" CssClass ="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group" style="margin-bottom:3px;">
                                    <label for="usr">Coy Name :</label>
                                    <asp:TextBox ID="tbQCoyClear" runat="server" CssClass ="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group" style="margin-bottom:3px;">
                                    <label for="usr">Bank Clearance :</label>
                                    <asp:TextBox ID="tbQBankClear" runat="server" CssClass ="form-control"></asp:TextBox>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lblQuery" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Query Cust modal end-->
    <!-- Create Bath Modal-->
    <div id="formBath" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <button type="button" data-dismiss="modal" aria-label="Close" class="close"><span aria-hidden="true">×</span></button>
                    <h4 id="login-modalLabel" class="modal-title">
                        <asp:Label ID="lbTitle" runat="server" Text="Batch "></asp:Label>
                        <asp:Label ID="lbIdBath" runat="server" Text=""></asp:Label>
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Bath Date:</label><font color=red>*</font>
                                <div class="input-group date datepicker" style="padding:0;">
                                    <asp:TextBox ID="tbBathDate" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                                </div>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Reff No:</label><font color=red>*</font>
                                <asp:TextBox ID="tbReff" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Notes:</label>
                                <asp:TextBox ID="tbNotes" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbSaveBath" runat="server" CssClass="btn btn-default" OnClientClick = "return ConfirmBath();"><i class="fa fa-save"></i> Save Batch</asp:LinkButton>
                    <asp:LinkButton ID="lbUpdateBath" runat="server" CssClass="btn btn-default" ><i class="fa fa-save"></i> Update Batch</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Create Bath modal end-->
    <!-- Create Bath Modal-->
    <div id="formClear" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <button type="button" data-dismiss="modal" aria-label="Close" class="close"><span aria-hidden="true">×</span></button>
                    <h4 id="H2" class="modal-title">
                        <asp:Label ID="lblTitleClear" runat="server" Text="Clearance "></asp:Label>
                        <asp:Label ID="lbIdClear" runat="server" Text=""></asp:Label>
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Bank Clearance:</label>
                                <asp:TextBox ID="tbBankClearance" runat="server" CssClass ="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Clear Date:</label><font color=red>*</font>
                                <div class="input-group date datepicker" style="padding:0;">
                                    <asp:TextBox ID="tbClearDate" runat="server" CssClass="form-control" Enabled="false" OnTextChanged="cari" AutoPostBack="true"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                                </div>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Reff No:</label><font color=red>*</font>
                                <asp:TextBox ID="tbReffClear" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Amount:</label>
                                <asp:TextBox ID="tbTotalAmount" runat="server" CssClass ="form-control" ReadOnly="true" style="text-align: right">></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Comission(%):</label>
                                <asp:TextBox ID="tbComission" runat="server" CssClass ="form-control" ReadOnly="true" style="text-align: right">></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Comission:</label>
                                <asp:TextBox ID="tbAmountComission" runat="server" CssClass ="form-control" ReadOnly="true" style="text-align: right">></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Net Amount:</label>
                                <asp:TextBox ID="tbNetClear" runat="server" CssClass ="form-control" ReadOnly="true" style="text-align: right">></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Notes:</label>
                                <asp:TextBox ID="tbNotesClear" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbSaveClear" runat="server" CssClass="btn btn-default" OnClientClick = "return ConfirmClear();"><i class="fa fa-save"></i> Save Clearance</asp:LinkButton>
                    <asp:LinkButton ID="lbUpdateClear" runat="server" CssClass="btn btn-default"><i class="fa fa-save"></i> Update Clearance</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Create Bath modal end-->
    <!-- List Detail Bath Modal-->
    <div id="FormListDetailBath" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortDetailBath" runat="server" CssClass="close" aria-label="Close" Visible= "false"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <asp:LinkButton ID="lbAbortDetailClear" runat="server" CssClass="close" aria-label="Close" Visible= "false"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <asp:LinkButton ID="lbAbortDetailPosted" runat="server" CssClass="close" aria-label="Close" Visible= "false"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H1" class="modal-title">
                        <asp:Label ID="lbTitleDetail" runat="server" Text="Detail Bath "></asp:Label>
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div style="max-height:500px">
                                <asp:GridView EmptyDataText="No records has been added." ID="gvDetailBath" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" ShowFooter="true">
                                    <Columns>
                                        <asp:BoundField DataField="TransID" HeaderText="ID" />
                                        <asp:BoundField DataField="TransDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}"/>
                                        <asp:BoundField DataField="Description" HeaderText="Type" />
                                        <asp:BoundField DataField="GuestName" HeaderText="Guest Name" />
                                        <asp:BoundField DataField="RoomNo" HeaderText="Room No" />
                                        <asp:BoundField DataField="arrival" HeaderText="Arrival" DataFormatString="{0:dd MMM yyyy}"/>
                                        <asp:BoundField DataField="departure" HeaderText="Departure" DataFormatString="{0:dd MMM yyyy}"/>
                                        <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right"/>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- List Detail Bath modal end-->
    <!-- List Detail Clear Modal-->
    <div id="FormListDetailClear" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <button type="button" data-dismiss="modal" aria-label="Close" class="close"><span aria-hidden="true">×</span></button>
                    <h4 id="H4" class="modal-title">
                        <asp:Label ID="lbTitleDetailClear" runat="server" Text="Detail Clear "></asp:Label>
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div style="max-height:500px">
                                <asp:GridView EmptyDataText="No records has been added." ID="gvDetailClear" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" ShowFooter="true">
                                    <Columns>
                                        <asp:BoundField DataField="InvoiceNo" HeaderText="ID" ItemStyle-Width="90px"/>
                                        <asp:BoundField DataField="InvDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" ItemStyle-Width="90px"/>
                                        <asp:BoundField DataField="ReffNo" HeaderText="Reff No"  ItemStyle-Width="100px"/>
                                        <asp:BoundField DataField="Notes" HeaderText="Notes" />
                                        <asp:BoundField DataField="Debit" HeaderText="Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="120px" ItemStyle-CssClass="cellOneCellPaddingRight"/>
                                        <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbDetail" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("InvoiceNo") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
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
    </div>
    <!-- List Detail Clear modal end-->
    <ul class="nav nav-tabs">
        <li id="open-tab" class="active"><a href="#Open" data-toggle="tab"><i class="fa fa-list"></i> Open&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a></li>
        <li id="bath-tab"><a href="#Batch" data-toggle="tab"><i class="fa fa-list"></i> Batch&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a></li>
        <li id="clear-tab"><a href="#Clear" data-toggle="tab"><i class="fa fa-list"></i> Clearance</a></li>
        <li id="posted-tab"><a href="#Posted" data-toggle="tab"><i class="fa fa-list"></i> Posted&nbsp;&nbsp;&nbsp</a></li>
    </ul>
    <div id="myTabContent" class="tab-content">
        <div class="tab-pane active in" id="Open">
            <div class="row">
                <div class="col-lg-4">
                    <br />
                    <div class="btn-group btn-group-justified">
                    <asp:LinkButton ID="lbCreateBath" runat="server" CssClass="btn btn-default" OnClientClick = "return ConfirmCreateBath();"><i class="fa fa-plus"></i> Create Batch</asp:LinkButton>
                    <asp:LinkButton ID="lbQueryOpen" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
                    </div>
                    <asp:GridView EmptyDataText="No records has been added." ID="gvCustAR" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <HeaderStyle Height="50px" />
                        <Columns>
                            <asp:BoundField ItemStyle-Width="90px" DataField="CustomerID" HeaderText="Customer ID" />
                            <asp:BoundField DataField="CoyName" HeaderText="Credit Card Name" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Description" HeaderText="Bank Clearance" />
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbSelect" CssClass="btn btn-default" runat="server" CommandName="getSelect" CommandArgument='<%# Eval("CustomerID")%>'><i class="fa fa-check"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
                <div class="col-lg-8">
                    <div style="text-align:center; font-size:medium; height:54px;">
                        <asp:HiddenField ID="hfCountOpen" runat="server" Value = "0" />
                        <br />
                        <asp:Label ID="lblTitleOpen" runat="server" Text=""></asp:Label>
                    </div>
                    <div style="max-height:415px; overflow:auto;">
                        <asp:GridView EmptyDataText="No records has been added." ID="gvTransOpen" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true">
                            <HeaderStyle Height="50px" />
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="10px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk" runat="server" onclick="Check_Click(this)" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="RecID" ItemStyle-CssClass="hidden" HeaderText="Rec ID" HeaderStyle-CssClass="hidden" />
                                <asp:BoundField DataField="TransID" HeaderText="ID" />
                                <asp:BoundField DataField="TransDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}"/>
                                <asp:BoundField DataField="Description" HeaderText="Type" />
                                <asp:BoundField DataField="GuestName" HeaderText="Guest Name" />
                                <asp:BoundField DataField="RoomNo" HeaderText="Room No" />
                                <asp:BoundField DataField="arrival" HeaderText="Arrival" DataFormatString="{0:dd MMM yyyy}"/>
                                <asp:BoundField DataField="departure" HeaderText="Departure" DataFormatString="{0:dd MMM yyyy}"/>
                                <asp:BoundField DataField="amountdr" HeaderText="Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right"/>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <div class="tab-pane fade" id="Batch">
            <div class="row">
                <div class="col-lg-4">
                    <br />
                    <div class="btn-group btn-group-justified">
                    <asp:LinkButton ID="lbCreateClear" runat="server" CssClass="btn btn-default" OnClientClick = "return ConfirmCreateClear();"><i class="fa fa-plus"></i> Create Clearance</asp:LinkButton>
                    <asp:LinkButton ID="lbQueryBath" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
                    </div>
                    <asp:GridView EmptyDataText="No records has been added." ID="gvCustBath" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <HeaderStyle Height="50px" />
                        <Columns>
                            <asp:BoundField ItemStyle-Width="90px" DataField="CustomerID" HeaderText="Customer ID" />
                            <asp:BoundField DataField="CoyName" HeaderText="Credit Card Name" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Description" HeaderText="Bank Clearance" />
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbSelectBath" CssClass="btn btn-default" runat="server" CommandName="getSelect" CommandArgument='<%# Eval("CustomerID")%>'><i class="fa fa-check"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
                <div class="col-lg-8">
                    <div style="text-align:center; font-size:medium; height:54px;">
                        <asp:HiddenField ID="hfCountBath" runat="server" Value = "0" />
                        <br />
                        <asp:Label ID="lblTitleBath" runat="server" Text=""></asp:Label>
                    </div>
                    <div style="max-height:415px; overflow:auto;">
                        <asp:GridView EmptyDataText="No records has been added." ID="gvTransBath" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None">
                            <HeaderStyle Height="50px" />
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="10px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk" runat="server" onclick="Check_Click(this)" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="InvoiceNo" HeaderText="ID" ItemStyle-Width="90px"/>
                                <asp:BoundField DataField="InvDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" ItemStyle-Width="90px"/>
                                <asp:BoundField DataField="ReffNo" HeaderText="Reff No"  ItemStyle-Width="100px"/>
                                <asp:BoundField DataField="Notes" HeaderText="Notes" />
                                <asp:BoundField DataField="Debit" HeaderText="Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="120px" ItemStyle-CssClass="cellOneCellPaddingRight"/>
                                <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbDetail" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("InvoiceNo") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="50px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbEditBath" CssClass="btn btn-default" runat="server" CommandName="getEditBatch" CommandArgument='<%# Eval("InvoiceNo")%>' Enabled='<%# if(eval("editBatch").toString()="Y","true","false") %>' ><i class="fa fa-edit"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="50px" HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbDeleteBath" CssClass="btn btn-default" runat="server" CommandName="getDeleteBatch" CommandArgument='<%# Eval("InvoiceNo")%>' Enabled='<%# if(eval("deleteBatch").toString()="Y","true","false") %>' OnClientClick="return confirmationBatch()"><i class="fa fa-trash-o"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <div class="tab-pane fade" id="Clear">
    	    <div class="row">
                <div class="col-lg-4">
                    <br />
                    <div class="btn-group btn-group-justified">
                    <asp:LinkButton ID="lbPosting" runat="server" CssClass="btn btn-default" OnClientClick = "return ConfirmPostClear();"><i class="fa fa-location-arrow"></i> Posting</asp:LinkButton>
                    <asp:LinkButton ID="lbQueryClear" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
                    </div>
                    <asp:GridView EmptyDataText="No records has been added." ID="gvCustClear" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <HeaderStyle Height="50px" />
                        <Columns>
                            <asp:BoundField ItemStyle-Width="90px" DataField="CustomerID" HeaderText="Customer ID" />
                            <asp:BoundField DataField="CoyName" HeaderText="Credit Card Name" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Description" HeaderText="Bank Clearance" />
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbSelectBath" CssClass="btn btn-default" runat="server" CommandName="getSelect" CommandArgument='<%# Eval("CustomerID")%>'><i class="fa fa-check"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
                <div class="col-lg-8">
                    <div style="text-align:center; font-size:medium; height:54px;">
                        <asp:HiddenField ID="hfCountClear" runat="server" Value = "0" />
                        <br />
                        <asp:Label ID="lblTitleTableClear" runat="server" Text=""></asp:Label>
                    </div>
                    <div style="max-height:415px; overflow:auto;">
                        <asp:GridView EmptyDataText="No records has been added." ID="gvTransClear" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None">
                            <HeaderStyle Height="50px" />
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="10px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk" runat="server" onclick="Check_Click(this)" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ReceiptID" HeaderText="ID" ItemStyle-Width="90px"/>
                                <asp:BoundField DataField="ReceiptDate" HeaderText="Date" DataFormatString="{0:dd-MM-yyyy}" ItemStyle-Width="90px"/>
                                <asp:BoundField DataField="ReffNo" HeaderText="Reff No"  ItemStyle-Width="100px"/>
                                <asp:BoundField DataField="Notes" HeaderText="Notes" />
                                <asp:BoundField DataField="Paid" HeaderText="Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" />
                                <asp:BoundField DataField="Comission" HeaderText="Comission" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" />
                                <asp:BoundField DataField="Net" HeaderText="Net" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" ItemStyle-CssClass="cellOneCellPaddingRight"/>
                                <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbDetailClear" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("ReceiptID") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="50px" HeaderText="Opsi" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div class="btn-group">
                                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" style="padding-bottom:11px; padding-top:11px;">
                                            <span class="caret"></span></button>
                                            <ul class="dropdown-menu" style="left:-70px; min-width: 100px;" role="menu">
                                                <li><asp:LinkButton ID="lbEditClear" CssClass="btn btn-link" runat="server" CommandName="getEditClear" CommandArgument='<%# Eval("ReceiptID")%>' Enabled='<%# if(eval("editClear").toString()="Y","true","false") %>' > Edit <i class="fa fa-edit"></i></asp:LinkButton></li>
                                                <li><asp:LinkButton ID="lbDeleteClear" CssClass="btn btn-link" runat="server" CommandName="getDeleteClear" CommandArgument='<%# Eval("ReceiptID")%>' Enabled='<%# if(eval("deleteClear").toString()="Y","true","false") %>' OnClientClick="return confirmationClear()"> Delete <i class="fa fa-trash-o"></i></asp:LinkButton></li>
                                            </ul>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br /><br /><br />
                    </div>
                </div>
            </div>
        </div>
        <div class="tab-pane fade" id="Posted">
    	    <div class="row">
                <div class="col-lg-4">
                    <br />
                    <asp:LinkButton ID="lbQueryPosted" runat="server" CssClass="btn btn-default" Width="150px"><i class="fa fa-filter"></i> Query</asp:LinkButton>
                    
                    <asp:GridView EmptyDataText="No records has been added." ID="gvCustPosted" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <HeaderStyle Height="50px" />
                        <Columns>
                            <asp:BoundField ItemStyle-Width="90px" DataField="CustomerID" HeaderText="Customer ID" />
                            <asp:BoundField DataField="CoyName" HeaderText="Credit Card Name" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Description" HeaderText="Bank Clearance" />
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbSelectBath" CssClass="btn btn-default" runat="server" CommandName="getSelect" CommandArgument='<%# Eval("CustomerID")%>'><i class="fa fa-check"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
                <div class="col-lg-8">
                    <div style="text-align:center; font-size:medium; height:54px;">
                        <asp:HiddenField ID="HiddenField1" runat="server" Value = "0" />
                        <br />
                        <asp:Label ID="lbCustPost" runat="server" Text=""></asp:Label>
                    </div>
                    <div style="max-height:415px; overflow:auto;">
                        <asp:GridView EmptyDataText="No records has been added." ID="gvTransPosted" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None">
                            <HeaderStyle Height="50px" />
                            <Columns>
                                <%--<asp:TemplateField ItemStyle-Width="10px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk" runat="server" onclick="Check_Click(this)" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:BoundField DataField="ReceiptID" HeaderText="ID" ItemStyle-Width="90px"/>
                                <asp:BoundField DataField="ReceiptDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" ItemStyle-Width="90px"/>
                                <asp:BoundField DataField="ReffNo" HeaderText="Reff No"  ItemStyle-Width="100px"/>
                                <asp:BoundField DataField="Notes" HeaderText="Notes" />
                                <asp:BoundField DataField="Paid" HeaderText="Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" ItemStyle-CssClass="cellOneCellPaddingRight"/>
                                <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingLeft">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbDetailClear" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("ReceiptID") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
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

