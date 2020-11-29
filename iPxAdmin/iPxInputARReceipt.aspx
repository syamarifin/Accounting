<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxInputARReceipt.aspx.vb" Inherits="iPxAdmin_iPxInputARReceipt" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    function hidePanelDetail() {
        $("#pnDetailInvoice").hide();
        $("#pnDetailReceipt").hide();
    }
    function showPanelInvoiveDetail() {
        $("#pnDetailInvoice").show();
        $("#pnDetailReceipt").hide();
    }
    function showPanelReceiptDetail() {
        $("#pnDetailInvoice").hide();
        $("#pnDetailReceipt").show();
    }
</script>
    <script type = "text/javascript">
    $(document).ready(function() {
        var count = 0;
        var gv = document.getElementById("<%=gvARInv.ClientID%>");
        var amount = document.getElementById("<%=tbAmountcr.ClientID%>");
        var chk = gv.getElementsByTagName("input");
        for(var i=0;i<chk.length;i++)
        {
            if(chk[i].disabled != false)
            {
                count++;
            }
        }
        if(count == i)
        {
            chk[0].disabled = true;
            chk[0].checked = false;
            // amount.disabled = true;
        }
        else
        {
            chk[0].disabled = false;
            // amount.disabled = false;
            // return confirm("Do you want to paid " + count + " "+ i +" records.");
        }
        console.log(count);
        console.log(i);
    });
    function Check_Click(objRef)
    {
        //Get the Row based on checkbox
        var row = objRef.parentNode.parentNode;
        
        //Get the reference of GridView
        var GridView = row.parentNode;
        
        //Get all input elements in Gridview
        var inputList = GridView.getElementsByTagName("input");
        var en=0;
        document.getElementById('<%=tbAmountcr.ClientID %>').value = 0;
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
        var gv = document.getElementById("<%=gvARInv.ClientID%>");
        var a=0;
        for (var i=0;i<inputList.length;i++)
        {
            var headerCheckBox = inputList[0];
            var checked = true;
            if(inputList[i].type == "checkbox" && inputList[i] != headerCheckBox)
            {
                if(inputList[i].disabled!=false){
                }else{
                    if(!inputList[i].checked)
                    {
                        
                    }
                    else{
                        var txtAmountReceive = $('[id*=lblBalance]');
                        var x=i-1;
                        a += (parseFloat(txtAmountReceive[x].innerText));
                        Number.prototype.formatMoney = function(decPlaces, thouSeparator, decSeparator) {
                            var n = this,
                                decPlaces = isNaN(decPlaces = Math.abs(decPlaces)) ? 2 : decPlaces,
                                decSeparator = decSeparator == undefined ? "." : decSeparator,
                                thouSeparator = thouSeparator == undefined ? "," : thouSeparator,
                                sign = n < 0 ? "-" : "",
                                i = parseInt(n = Math.abs(+n || 0).toFixed(decPlaces)) + "",
                                j = (j = i.length) > 3 ? j % 3 : 0;
                            return sign + (j ? i.substr(0, j) + thouSeparator : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thouSeparator) + (decPlaces ? decSeparator + Math.abs(n - i).toFixed(decPlaces).slice(2) : "");
                        };
                        var formattedMoney = parseFloat(a).formatMoney(2,',','.');
                        document.getElementById('<%=tbAmountcr.ClientID %>').value = formattedMoney;
                    }
                }
            }
        }
    }
    function checkAll(objRef)
    {
        var GridView = objRef.parentNode.parentNode.parentNode;
        var inputList = GridView.getElementsByTagName("input");
        var a=0;
        document.getElementById('<%=tbAmountcr.ClientID %>').value = 0;
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
                        var txtAmountReceive = $('[id*=lblBalance]');
                        var x=i-1;
                        a += (parseFloat(txtAmountReceive[x].innerText));
                        Number.prototype.formatMoney = function(decPlaces, thouSeparator, decSeparator) {
                            var n = this,
                                decPlaces = isNaN(decPlaces = Math.abs(decPlaces)) ? 2 : decPlaces,
                                decSeparator = decSeparator == undefined ? "." : decSeparator,
                                thouSeparator = thouSeparator == undefined ? "," : thouSeparator,
                                sign = n < 0 ? "-" : "",
                                i = parseInt(n = Math.abs(+n || 0).toFixed(decPlaces)) + "",
                                j = (j = i.length) > 3 ? j % 3 : 0;
                            return sign + (j ? i.substr(0, j) + thouSeparator : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thouSeparator) + (decPlaces ? decSeparator + Math.abs(n - i).toFixed(decPlaces).slice(2) : "");
                        };
                        var formattedMoney = parseFloat(a).formatMoney(2,',','.');
                        document.getElementById('<%=tbAmountcr.ClientID %>').value = formattedMoney;
                        }
                }
                else
                {
                    inputList[i].checked=false;
                    document.getElementById('<%=tbAmountcr.ClientID %>').value = 0;
                }
            }
        }
    }
    
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
            <h3>AR Receipt <asp:Label ID="lbRecID" runat="server" Text="ReceiptID"></asp:Label></h3>
            <div class="row">
                <div class="col-lg-6">
                    <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">Receipt Date:</label><font color=red>*</font>
                        <div class="input-group date datepicker" style="padding:0;">
                            <asp:TextBox ID="tbRecDate" runat="server" CssClass="form-control" Enabled="false" OnTextChanged="cari" AutoPostBack="true"></asp:TextBox>
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
                        <asp:Panel ID="PnReason" runat="server">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Reason:</label>
                                <asp:TextBox ID="tbReason" runat="server" CssClass ="form-control" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                            </div>
                        </asp:Panel>
                    <br />
                    <div style="max-height:600px;overflow:auto; padding-bottom:30px;">
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
                                    <asp:TemplateField ItemStyle-Width="100px" HeaderText="Balances" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden">
                                        <ItemTemplate>    
                                            <asp:Label ID="lblBalance" runat="server" Text='<%# if(Eval("totalInvoice").toString()="",string.Format("{0:N2}",0),Eval("totalInvoice")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="100px" DataField="totalInvoice" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderText="Balances" />
                                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Detail" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <div class="btn-group">
                                                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" style="padding-bottom:11px; padding-top:11px;">
                                                <span class="caret"></span></button>
                                                <ul class="dropdown-menu" style="left:-60px; min-width: 100px;" role="menu">
                                                    <li><asp:LinkButton ID="lbDetailInv" CssClass="btn btn-link btn_right" runat="server" CommandName="getDetailInv" CommandArgument='<%# Eval("InvoiceNo") %>'> Invoice <i class="fa fa-file-text-o"></i></asp:LinkButton></li>
                                                    <li><asp:LinkButton ID="lbDetailRec" CssClass="btn btn-link btn_right" runat="server" CommandName="getDetailRec" CommandArgument='<%# Eval("InvoiceNo") %>'> Receipt <i class="fa fa-file-text-o"></i></asp:LinkButton></li>
                                                </ul>
                                            </div>
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
                                            <asp:BoundField ItemStyle-Width ="100px" DataField="amount" HeaderText="Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right"/>
                                        </Columns>
                                        <pagerstyle cssclass="pagination-ys">
                                        </pagerstyle>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div id="pnDetailReceipt" class="panel panel-default" style="display:none;">
                            <div class="panel-heading">
                                <b> Detail Receipt</b>
                                <asp:LinkButton ID="lbCloseDetailReceipt" CssClass="close" runat="server" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                            </div>
                            <div class="panel-body">
                                <div class="form-group left">
                                    <asp:GridView ID="gvDetailReceipt" runat="server" AutoGenerateColumns="false" CssClass="table" FooterStyle-Font-Bold="true" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" ShowFooter="true">
                                        <Columns>
                                            <asp:BoundField ItemStyle-Width ="90px" DataField="TransID" HeaderText="Receipt ID"/>
                                            <asp:BoundField DataField="CoyName" HeaderText="Customer Name" />
                                            <asp:BoundField ItemStyle-Width ="70px" DataField="Paid" HeaderText="Paid By" />
                                            <asp:BoundField ItemStyle-Width ="90px" DataField="ReceiptDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}"/>
                                            <asp:BoundField ItemStyle-Width ="100px" DataField="amountcr" HeaderText="Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right"/>
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
        <asp:LinkButton ID="lbUpdateReceipt" Width="150px" runat="server" CssClass="btn btn-default" ><i class="fa fa-save"></i> Update Receipt</asp:LinkButton>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

