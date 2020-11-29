<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxInputCreditCard.aspx.vb" Inherits="iPxAdmin_iPxInputCreditCrad" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
            var gv = document.getElementById("<%=gvInvoice.ClientID%>");
            var edit = document.getElementById("<%=lbaddInvEdit.ClientID%>");
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
                
                if(edit.enabled=false){
                    alert("No records to Batch.");
                    return false;
                }
                else
                {
                    return confirm("Do you want to delete Batch " + (i - 1) + " records.");
                }
            }
            else
            {
                return confirm("Do you want to Batch " + count + " records.");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="small-box">
        <div class="inner">
            <h3>AR Credit Card <asp:Label ID="lbInvID" runat="server" Text="InvoiceID"></asp:Label></h3>
            <div class="row">
                <div class="col-lg-12">
                    <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">Reff No:</label><font color=red>*</font>
                        <asp:TextBox ID="tbReff" runat="server" CssClass ="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group" style="margin-bottom:3px;">
                        <label for="usr">Notes:</label>
                        <asp:TextBox ID="tbNotes" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <br />
                    <div style="max-height:600px;overflow:auto;">
                        <asp:Panel ID="pnCustGroup" runat="server">
                            <asp:GridView EmptyDataText="No records has been added." ID="gvCustAR" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="businessid" HeaderText="Business ID" />
                                <asp:BoundField DataField="CustomerID" HeaderText="Customer ID" />
                                <asp:BoundField DataField="ARGroup" HeaderText="AR Group" />
                                <asp:BoundField DataField="CoyGroup" HeaderText="Coy Group" />
                                <asp:BoundField DataField="CoyName" HeaderText="Coy Name" />
                                <asp:BoundField DataField="Address" HeaderText="Address" />
                                <asp:BoundField DataField="BilllingAddress" HeaderText="Billing Address" />
                                <asp:BoundField DataField="country" HeaderText="Country" />
                                <asp:BoundField DataField="Province" HeaderText="Province" />
                                <asp:BoundField DataField="city" HeaderText="City" />
                                <asp:BoundField DataField="Phone" HeaderText="Phone" />
                                <asp:BoundField DataField="Email" HeaderText="Email" />
                                <asp:TemplateField ItemStyle-Width="70px" HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbSelect" CssClass="btn btn-default" runat="server" CommandName="getSelect" CommandArgument='<%# Eval("CustomerID") %>'><i class="fa fa-check"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <pagerstyle cssclass="pagination-ys">
                            </pagerstyle>
                        </asp:GridView>
                        </asp:Panel>
                        <asp:Panel ID="pnDetailTrans" runat="server" Visible="false">
                            <asp:HiddenField ID="hfCount" runat="server" Value = "0" />
                            <asp:LinkButton ID="lbShowGrp" Width="150px" runat="server" CssClass="btn btn-default"> Show Group</asp:LinkButton>
                            <div style="max-height:450px; overflow:auto;">
                            <asp:GridView ID="gvInvoice" runat="server" DataKeyNames="RecID" AutoGenerateColumns="false" CssClass="table" FooterStyle-Font-Bold="true" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" ShowFooter="true">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="10px">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk" runat="server" Checked='<%# if(Trim(Eval("invoiceno").toString())= "", "false","true") %>' onclick="Check_Click(this)" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width ="50px" DataField="RecID" ItemStyle-CssClass="hidden" HeaderText="Rec ID" HeaderStyle-CssClass="hidden" />
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
                            </asp:GridView>
                            </div>
                        </asp:Panel>
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
        <asp:LinkButton ID="lbSaveInvoice" Width="150px" runat="server" CssClass="btn btn-default" OnClientClick = "return ConfirmDelete();"><i class="fa fa-save"></i> Save Batch</asp:LinkButton>
        <asp:LinkButton ID="lbaddInvEdit" Width="150px" runat="server" CssClass="btn btn-default" Visible="false"><i class="fa fa-plus"></i> Add Transaction</asp:LinkButton>
        <asp:LinkButton ID="lbDetailInv" Width="150px" runat="server" CssClass="btn btn-default" Visible="false"><i class="fa fa-file"></i> Detail Transaction</asp:LinkButton>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

