<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/iPxAdmin.master" AutoEventWireup="false" CodeFile="paymentmodeAdmin.aspx.vb" Inherits="iPxAdmin_paymentmodeAdmin" title="Alcor"%>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script type="text/javascript">
    function Print() {
        var prtGrid = document.getElementById('<%=dvReport.ClientID %>');
        prtGrid.border = 0;
        var prtwin = window.open('', 'PrintGridViewData', 'left=100,top=100,width=2480,height=3508,tollbar=0,scrollbars=1,status=0,resizable=1');
        prtwin.document.write(prtGrid.outerHTML);
        prtwin.document.close();
        prtwin.focus();
        prtwin.print();
        prtwin.close();
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

            <div class="lock-wrapper">
                <div  class="">
                    
                    <div class="row">
                        <div class="form-group col-md-12 col-sm-12 col-xs-12">
                        
                            <div class="table thumbnail " >
                            <div>
                                    <div >
                                        <h4>Detail Transaction</h4>
                                    </div>
                                    <hr />
                                </div>
                                <div>
                                    <div>
                                        Business Name :     
                                        <asp:Label ID="ddsite" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div>
                                        Pay for : 
                                        <asp:Label ID="lblPayfor" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div>
                                        Package : 
                                        <asp:Label ID="lblPackage" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div>
                                        Program Name :  
                                        <asp:Label ID="lblProgramName" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div>
                                        Qty :  
                                        <asp:Label ID="lblQty" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div>
                                        Discount :  
                                        <asp:Label ID="lblDiscount" runat="server" Text=""></asp:Label>
                                    </div>
                                    <hr />
                                    <div class="text-bold">
                                        Amount : 
                                        <asp:Label ID="lblAmount" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                                <div>
                                </div>
                            </div>
                            
                             <div class="iconic-input right">
                                <asp:Label ID="lblPrice" runat="server" Text=""></asp:Label>
                                <asp:TextBox ID="txtVoucher" runat="server"  placeholder="Enter Voucher # here if payment using voucher" class="form-control padding-horizontal-15">  </asp:TextBox>
                                <i class="fa fa-cc"></i>
                                <asp:Button ID="btnVoucher" runat="server" Text="Paid using Voucher" class="btn btn-block btn-success" BackColor="#194516" OnClientClick="beep()"/>
                            </div>
                            <asp:Button ID="Button1" runat="server" class="btn" Text="Print Invoice" OnClientClick="Print()" />
                            <%--<p class="text-blue text-center">OR</p>--%>
                            <div class="list-group">
                                <%--<asp:Button ID="btnOtherPay" runat="server" Text="Paid using Debit/Credit Card" class="btn btn-block btn-success" PostBackUrl="~/iPxPMS/otherpay.aspx" OnClientClick="beep()"/>
                                <asp:Button ID="btnBack" runat="server" Text="Back" class="btn btn-block btn-success"  OnClientClick="beep()" BackColor="#194516"/>--%>
                                <div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="dvReport1" class="hidden" runat="server">
                <div class="container">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="invoice-title">
                                <h2>Invoice</h2>
                                <h3 class="pull-right">Order # 12345</h3>
                            </div>
                            <hr>
                            <div class="row">
                                <div class="col-xs-6">
                                    <address>
                                        <strong>Billed To:</strong><br>
                                        <p>Alcor System</p>
                                        <p>PT. PYXIS ULTIMATE SOLUTION<br>JL Tambora No. 15 Malang</p>
                                        <p>0341 - 550246</p>
                                    </address>
                                </div>
                                <div class="col-xs-6 text-right">
                                    <address>
                                        <strong>Shipped To:</strong><br>
                                        <asp:Label ID="lblShippedto" runat="server" Text=""></asp:Label>
                                        <br>
                                        <br>
                                        <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label>
                                        <br>
                                    </address>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-6">
                                    <address class="hidden">
                                    </address>
                                </div>
                                <div class="col-xs-6 text-right">
                                    <address>
                                        <strong>Order Date:</strong><br>
                                        <asp:Label ID="lblOrderDate" runat="server" Text=""></asp:Label>
                                        <br><br>
                                    </address>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h3 class="panel-title"><strong>Order summary</strong></h3>
                                </div>
                                <div class="panel-body">
                                    <div class="table-responsive">
                                        <table class="table table-condensed">
                                            <thead>
                                                <tr>
                                                    <td><strong>Item</strong></td>
                                                    <td class="text-center"><strong>Price</strong></td>
                                                    <td class="text-center"><strong>Quantity</strong></td>
                                                    <td class="text-right"><strong>Totals</strong></td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <!-- foreach ($order->lineItems as $line) or some such thing here -->
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblItem" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:Label ID="lblPriceItem" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:Label ID="lblQtyItem" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:Label ID="lblTotalItem" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="thick-line"></td>
                                                    <td class="thick-line"></td>
                                                    <td class="thick-line text-center"><strong>Subtotal</strong></td>
                                                    <td class="thick-line text-right">
                                                        <asp:Label ID="lblSubtotal" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="no-line"></td>
                                                    <td class="no-line"></td>
                                                </tr>
                                                <tr>
                                                    <td class="no-line"></td>
                                                    <td class="no-line"></td>
                                                    <td class="no-line text-center"><strong>Total</strong></td>
                                                    <td class="no-line text-right">
                                                        <asp:Label ID="lblTotalAll" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="dvReport" class="hidden" runat="server">
                <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"  
                    AutoDataBind="True"  ReportSourceID="CrystalReportSource1" HasCrystalLogo="False"
                    Width="350px" DisplayGroupTree="False" DisplayToolbar="False" />
                <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
                    <Report >
                    </Report>
                </CR:CrystalReportSource>
            </div>
</asp:Content>