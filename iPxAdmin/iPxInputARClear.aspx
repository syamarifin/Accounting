<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxInputARClear.aspx.vb" Inherits="iPxAdmin_iPxInputARClear" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    function hidePanelDetail() {
        $("#pnDetailInvoice").hide();
    }
    function showPanelDetail() {
        $("#pnDetailInvoice").show();
    }
</script>
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
                alert("No records to paid.");
                return false;
            }
            else
            {
                return confirm("Do you want to paid " + count + " records.");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="small-box">
        <div class="inner">
            <h3>AR Credit Clearance <asp:Label ID="lbRecID" runat="server" Text="ReceiptID"></asp:Label></h3>
            <div class="row">
                <div class="col-lg-6">
                    <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">Receipt Date:</label><font color=red>*</font>
                        <div class="input-group date datepicker" style="padding:0;">
                            <asp:TextBox ID="tbRecDate" runat="server" CssClass="form-control"></asp:TextBox>
                            <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                        </div>
                    </div>
                    <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">Reff No:</label><font color=red>*</font>
                        <asp:TextBox ID="tbReff" runat="server" CssClass ="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">Paid by:</label><font color=red>*</font>
                        <asp:DropDownList ID="dlPaid" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">Amount:</label><font color=red>*</font>
                        <asp:TextBox ID="tbAmountcr" runat="server" CssClass ="form-control" style="text-align: right;"></asp:TextBox>
                    </div>
                </div>
                <div class="col-lg-12">
                    <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">Notes:</label>
                        <asp:TextBox ID="tbNotes" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <br />
                    <div style="max-height:600px;overflow:auto;">
                        <asp:Panel ID="pnInvoice" runat="server">
                            <asp:HiddenField ID="hfCount" runat="server" Value = "0" />
                            <asp:GridView EmptyDataText="No records has been added." ID="gvARInv" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" ShowFooter="true" AllowPaging="true" PageSize="10">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="10px">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAll" runat="server" Checked="true" onclick="checkAll(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkitem" runat="server" Checked='<%# if(Trim(Eval("totalInvoice").toString())= "0.0000", "false","true") %>' Enabled='<%# if(Trim(Eval("totalInvoice").toString())= "0.0000", "false","true") %>' onclick="Check_Click(this)" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="100px" DataField="InvoiceNo" HeaderText="Invoice No" />
                                    <asp:BoundField ItemStyle-Width="85px" DataField="InvDate" HeaderText="Invoice Date" DataFormatString="{0:dd MMM yyyy}"/>
                                    <asp:BoundField ItemStyle-Width="85px" DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd MMM yyyy}"/>
                                    <asp:BoundField ItemStyle-Width="100px" DataField="CoyName" HeaderText="Customer Name" />
                                    <asp:BoundField ItemStyle-Width="100px" DataField="ReffNo" HeaderText="Reff No" />
                                    <asp:BoundField DataField="Notes" HeaderText="Notes"/>
                                    <asp:BoundField ItemStyle-Width="85px" DataField="RegDate" HeaderText="Reg Date" DataFormatString="{0:dd MMM yyyy}"/>
                                    <asp:BoundField ItemStyle-Width="100px" DataField="Debit" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderText="Debit" />
                                    <asp:TemplateField ItemStyle-Width="100px" HeaderText="Credit" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# if(Eval("Credit").toString()="",string.Format("{0:N2}",0),string.Format("{0:N2}",Eval("Credit"))) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="100px" DataField="totalInvoice" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderText="Balances" />
                                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Detail" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbEdit" CssClass="btn btn-default" runat="server" CommandName="getSelect" CommandArgument='<%# Eval("InvoiceNo") %>'><i class="fa fa-list-alt"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <pagerstyle cssclass="pagination-ys">
                                </pagerstyle>
                            </asp:GridView>
                        </asp:Panel>
                        <div id="pnDetailInvoice" class="panel panel-default" style="display:none;">
                            <div class="panel-heading">
                                <b> Detail Invoice</b>
                                <asp:LinkButton ID="lbCloseDetailInvoice" CssClass="close" runat="server" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                            </div>
                            <div class="panel-body">
                                <div class="form-group left">
                                    <asp:GridView ID="gvDetailInvoice" runat="server" AutoGenerateColumns="false" CssClass="table" FooterStyle-Font-Bold="true" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" ShowFooter="true">
                                        <Columns>
                                            <asp:BoundField ItemStyle-Width ="120px" DataField="CoyName" HeaderText="Customer Name" />
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
        <div class="icon">
            <asp:Label ID="lbStatus" runat="server" Text="New"></asp:Label>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div id="footer" class="container">
        <asp:LinkButton ID="lbAbort" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-close"></i> Abort</asp:LinkButton>
        <asp:LinkButton ID="lbSaveReceipt" Width="150px" runat="server" CssClass="btn btn-default" OnClientClick = "return ConfirmDelete();"><i class="fa fa-save"></i> Save Receipt</asp:LinkButton>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

