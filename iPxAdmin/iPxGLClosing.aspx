<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxGLClosing.aspx.vb" Inherits="iPxAdmin_iPxGLClosing" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        function MonthGL() {
            $(".monthGl").datepicker({ format: 'mm-yyyy', viewMode: "months", minViewMode: "months", autoclose: true, todayBtn: 'linked' })
        }
        function ConfirmClose()
        {
            var periode = document.getElementById("<%=tbDate.ClientID %>").value;
            var tahun = periode.slice(3, 7);
            if (periode.slice(0, 2) == 01) {
                var bulan = "January";
            }else if (periode.slice(0, 2) == 02) {
                var bulan = "February";
            }else if (periode.slice(0, 2) == 03) {
                var bulan = "March";
            }else if (periode.slice(0, 2) == 04) {
                var bulan = "April";
            }else if (periode.slice(0, 2) == 05) {
                var bulan = "Mei";
            }else if (periode.slice(0, 2) == 06) {
                var bulan = "June";
            }else if (periode.slice(0, 2) == 07) {
                var bulan = "July";
            }else if (periode.slice(0, 2) == 08) {
                var bulan = "August";
            }else if (periode.slice(0, 2) == 09) {
                var bulan = "September";
            }else if (periode.slice(0, 2) == 10) {
                var bulan = "October";
            }else if (periode.slice(0, 2) == 11) {
                var bulan = "November";
            }else if (periode.slice(0, 2) == 12) {
                var bulan = "December";
            }
            if (confirm('Do you want to close this periode '+ bulan +' ' + tahun +' ?')) {
            return true;
            }else{
            return false;
            }
        }

        function ConfirmOpen()
        {
            var periode = document.getElementById("<%=tbDate.ClientID %>").value;
            var tahun = periode.slice(3, 7);
            if (periode.slice(0, 2) == 01) {
                var bulan = "January";
            }else if (periode.slice(0, 2) == 02) {
                var bulan = "February";
            }else if (periode.slice(0, 2) == 03) {
                var bulan = "March";
            }else if (periode.slice(0, 2) == 04) {
                var bulan = "April";
            }else if (periode.slice(0, 2) == 05) {
                var bulan = "Mei";
            }else if (periode.slice(0, 2) == 06) {
                var bulan = "June";
            }else if (periode.slice(0, 2) == 07) {
                var bulan = "July";
            }else if (periode.slice(0, 2) == 08) {
                var bulan = "August";
            }else if (periode.slice(0, 2) == 09) {
                var bulan = "September";
            }else if (periode.slice(0, 2) == 10) {
                var bulan = "October";
            }else if (periode.slice(0, 2) == 11) {
                var bulan = "November";
            }else if (periode.slice(0, 2) == 12) {
                var bulan = "December";
            }
            if (confirm('Do you want to Re-Open this periode '+ bulan +' ' + tahun +' ?')) {
            return true;
            }else{
            return false;
            }
        }

        function hideModalReOpenLogin() {
            $('#formReOpenLogin').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalReOpenLogin() {
            $('#formReOpenLogin').modal({ backdrop: 'static',
                keyboard: false
            }, 'show');
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Edit GL Header Modal-->
    <div id="formInput" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm" style="width:550px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <h4 id="login-modalLabel" class="modal-title">Do you want to Re-Open this periode?. </h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="Label1" runat="server" Text="re-open closing will delete PL Jurnal and open the periode status."></asp:Label>
                    <br /><br />
                    <asp:CheckBox ID="cbAgree" runat="server" /> Agree
                </div>
                <div class="modal-footer" style="text-align:center">
                    <asp:LinkButton Width="150px" ID="lbUpdate" runat="server" CssClass="btn btn-default"><i class="fa fa-unlock"></i> Re-Open</asp:LinkButton>
                    <asp:LinkButton Width="150px" ID="lbAbort" runat="server" CssClass="btn btn-danger"><i class="fa fa-close"></i> Cancel</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Edit GL Header modal end-->
    <!-- Re Open login Modal-->
    <div id="formReOpenLogin" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortLogin" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="login-modalLabel" class="modal-title">Please login Re-Open this periode!. </h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="usr">Username:</label>
                        <asp:TextBox ID="tbUsernameRe" runat="server" CssClass ="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="usr">Password:</label>
                        <asp:TextBox ID="tbPasswordRe" runat="server" CssClass ="form-control" TextMode="Password"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer" style="text-align:center">
                    <asp:LinkButton Width="150px" ID="lbLogAcces" runat="server" CssClass="btn btn-default">Login</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Re Open login modal end-->
    <div id ="done" class="col-lg-12 col-xs-12">
        <!-- small box -->
        <div class="small-box" style="background-color:White; margin-bottom:10px; border-top: 3px solid #0a818e; padding-bottom:0;">
            <div class="inner" style="padding-bottom:0;">
                <div class="row" style="padding:5px; padding-bottom: 0;">
                    <div class="col-lg-3">
                        <br />
                        <asp:Label ID="Label8" runat="server" Text="Balance Sheet"></asp:Label>
                    </div>
                    <div class="col-lg-9">
                        <div class="form-group" style="text-align:right;">
                            <table style="width:100%">
                                <tr>
                                    <td>
                                        <label for="usr">Month:</label>&nbsp;&nbsp;
                                    </td>
                                    <td style="width:150px;">
                                        <div class="input-group date monthGl" style="padding:0;">
                                            <asp:TextBox ID="tbDate" runat="server" CssClass ="form-control" placeholder="MM yyyy" OnTextChanged="cari" AutoPostBack="true"></asp:TextBox>
                                            <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                                        </div>
                                    </td>
                                    <td style="width:110px;text-align:right;">
                                        <asp:LinkButton ID="lbClosing" runat="server" CssClass="btn btn-default" Width="100px" OnClientClick="return ConfirmClose();"><i class="fa fa-ban"></i> Closing</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            
                        </div>
                    </div>
                </div>
                <div class="row" style="padding:5px; padding-bottom: 0;">
                    <div id ="Div1" class="col-lg-3 col-xs-6 padding-info">
                        <!-- small box -->
                        <div class="small-box" style="background-color:White; margin-bottom:10px; margin:0; border-top:3px solid #005662;">
                            <div class="small-box-header">
                                <h3><asp:Label ID="lblInfo1" runat="server" Text="Asset"></asp:Label></h3>
                            </div>
                            <div class="inner">
                                <table style="width:100%">
                                    <tr>
                                        <td style="width:35px;"><p>Rp. &nbsp;</p></td>
                                        <td style="text-align:right;"><p><asp:Label ID="lblAsset" runat="server" Text="0"></asp:Label></p></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div id ="Div3" class="col-lg-3 col-xs-6 padding-info">
                        <!-- small box -->
                        <div class="small-box" style="background-color:White; margin-bottom:10px; margin:0; border-top:3px solid #005662;">
                            <div class="small-box-header">
                                <h3><asp:Label ID="Label4" runat="server" Text="Liability"></asp:Label></h3>
                            </div>
                            <div class="inner">
                                <table style="width:100%">
                                    <tr>
                                        <td style="width:35px;"><p>Rp. &nbsp;</p></td>
                                        <td style="text-align:right;"><p><asp:Label ID="lblLeability" runat="server" Text="0"></asp:Label></p></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div id ="Div2" class="col-lg-3 col-xs-6 padding-info">
                        <!-- small box -->
                        <div class="small-box" style="background-color:White; margin-bottom:10px; margin:0; border-top:3px solid #005662;">
                            <div class="small-box-header">
                                <h3><asp:Label ID="Label2" runat="server" Text="Equity"></asp:Label></h3>
                            </div>
                            <div class="inner">
                                <table style="width:100%">
                                    <tr>
                                        <td style="width:35px;"><p>Rp. &nbsp;</p></td>
                                        <td style="text-align:right;"><p><asp:Label ID="lblEquity" runat="server" Text="0"></asp:Label></p></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div id ="Div4" class="col-lg-3 col-xs-6 padding-info">
                        <!-- small box -->
                        <div class="small-box" style="background-color:White; margin-bottom:10px; margin:0; border-top:3px solid #005662;">
                            <div class="small-box-header">
                                <h3><asp:Label ID="Label6" runat="server" Text="Control Balance"></asp:Label></h3>
                            </div>
                            <div class="inner">
                                <table style="width:100%">
                                    <tr>
                                        <td style="width:35px;"><p>Rp. &nbsp;</p></td>
                                        <td style="text-align:right;"><p><asp:Label ID="lblBalance" runat="server" Text="0"></asp:Label></p></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:Label ID="Label9" runat="server" Text="Profit And Loss"></asp:Label>
                <div class="row" style="padding:5px; padding-bottom: 0;">
                    <div id ="Div5" class="col-lg-3 col-xs-6 padding-info">
                        <!-- small box -->
                        <div class="small-box" style="background-color:White; margin-bottom:10px; margin:0; border-top:3px solid #005662;">
                            <div class="small-box-header">
                                <h3><asp:Label ID="Label10" runat="server" Text="Revenue"></asp:Label></h3>
                            </div>
                            <div class="inner">
                                <table style="width:100%">
                                    <tr>
                                        <td style="width:35px;"><p>Rp. &nbsp;</p></td>
                                        <td style="text-align:right;"><p><asp:Label ID="lblRevenue" runat="server" Text="0"></asp:Label></p></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div id ="Div6" class="col-lg-3 col-xs-6 padding-info">
                        <!-- small box -->
                        <div class="small-box" style="background-color:White; margin-bottom:10px; margin:0; border-top:3px solid #005662;">
                            <div class="small-box-header">
                                <h3><asp:Label ID="Label12" runat="server" Text="Cost"></asp:Label></h3>
                            </div>
                            <div class="inner">
                                <table style="width:100%">
                                    <tr>
                                        <td style="width:35px;"><p>Rp. &nbsp;</p></td>
                                        <td style="text-align:right;"><p><asp:Label ID="lblCost" runat="server" Text="0"></asp:Label></p></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div id ="Div7" class="col-lg-3 col-xs-6 padding-info">
                        <!-- small box -->
                        <div class="small-box" style="background-color:White; margin-bottom:10px; margin:0; border-top:3px solid #005662;">
                            <div class="small-box-header">
                                <h3><asp:Label ID="Label14" runat="server" Text="Expenses"></asp:Label></h3>
                            </div>
                            <div class="inner">
                                <table style="width:100%">
                                    <tr>
                                        <td style="width:35px;"><p>Rp. &nbsp;</p></td>
                                        <td style="text-align:right;"><p><asp:Label ID="lblExpenses" runat="server" Text="0"></asp:Label></p></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div id ="Div8" class="col-lg-3 col-xs-6 padding-info">
                        <!-- small box -->
                        <div class="small-box" style="background-color:White; margin-bottom:10px; margin:0; border-top:3px solid #005662;">
                            <div class="small-box-header">
                                <h3><asp:Label ID="Label16" runat="server" Text="Profit And Loss"></asp:Label></h3>
                            </div>
                            <div class="inner">
                                <table style="width:100%">
                                    <tr>
                                        <td style="width:35px;"><p>Rp. &nbsp;</p></td>
                                        <td style="text-align:right;"><p><asp:Label ID="lblProfit" runat="server" Text="0"></asp:Label></p></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:Label ID="Label18" runat="server" Text="Statistic Account"></asp:Label>
                <div class="row" style="padding:5px; padding-bottom: 0;">
                    <div id ="Div9" class="col-lg-3 col-xs-6 padding-info">
                        <!-- small box -->
                        <div class="small-box" style="background-color:White; margin-bottom:10px; margin:0; border-top:3px solid #005662;">
                            <div class="small-box-header">
                                <h3><asp:Label ID="Label19" runat="server" Text="Statistic"></asp:Label></h3>
                            </div>
                            <div class="inner">
                                <table style="width:100%">
                                    <tr>
                                        <td style="width:35px;"><p>Rp. &nbsp;</p></td>
                                        <td style="text-align:right;"><p><asp:Label ID="lblStatistic" runat="server" Text="0"></asp:Label></p></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div style="text-align:right;">
                    <asp:LinkButton ID="lbOpen" runat="server" CssClass="btn btn-default" Width="100px" OnClientClick="return ConfirmOpen();"><i class="fa fa-unlock"></i> Re-Open</asp:LinkButton>
                </div>
                </br>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

