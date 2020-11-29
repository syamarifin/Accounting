<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxGLTransaction.aspx.vb" Inherits="iPxAdmin_iPxGLTransaction" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
       function confirmationVer() {
           if (confirm('Do you want to verify Jurnal ?')) {
           return true;
                if($("#Sales-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Sales";
                }else if($("#AR-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "AR";
                }else if($("#GL-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "GL";
                }else if($("#AP-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "AP";
                }else if($("#Cost-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Cost";
                }else if($("#Receiving-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Receiving";
                }else if($("#Adjustment-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Adjustment";
                }else if($("#Statistic-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Statistic";
                }
           }else{
           return false;
           }
       }
       function confirmationUnver() {
           if (confirm('Do you want to undo verify Jurnal ?')) {
           return true;
                if($("#Sales-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Sales";
                }else if($("#AR-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "AR";
                }else if($("#GL-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "GL";
                }else if($("#AP-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "AP";
                }else if($("#Cost-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Cost";
                }else if($("#Receiving-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Receiving";
                }else if($("#Adjustment-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Adjustment";
                }else if($("#Statistic-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Statistic";
                }
           }else{
           return false;
           }
       }
       function confirmationDel() {
           if (confirm('Do you want to delete Jurnal ?')) {
           return true;
                if($("#Sales-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Sales";
                }else if($("#AR-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "AR";
                }else if($("#GL-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "GL";
                }else if($("#AP-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "AP";
                }else if($("#Cost-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Cost";
                }else if($("#Receiving-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Receiving";
                }else if($("#Adjustment-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Adjustment";
                }else if($("#Statistic-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Statistic";
                }
           }else{
           return false;
           }
       }
       function confirmationRec() {
           if (confirm('Do you want to restore jurnal ?')) {
           return true;
                if($("#Sales-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Sales";
                }else if($("#AR-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "AR";
                }else if($("#GL-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "GL";
                }else if($("#AP-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "AP";
                }else if($("#Cost-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Cost";
                }else if($("#Receiving-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Receiving";
                }else if($("#Adjustment-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Adjustment";
                }else if($("#Statistic-tab").hasClass("active")){
                    document.getElementById("<%=hfCount.ClientID %>").value = "Statistic";
                }
           }else{
           return false;
           }
       }
        function sessionTab()
        {
            if($("#Sales-tab").hasClass("active")){
                document.getElementById("<%=hfCount.ClientID %>").value = "Sales";
            }else if($("#AR-tab").hasClass("active")){
                document.getElementById("<%=hfCount.ClientID %>").value = "AR";
            }else if($("#GL-tab").hasClass("active")){
                document.getElementById("<%=hfCount.ClientID %>").value = "GL";
            }else if($("#AP-tab").hasClass("active")){
                document.getElementById("<%=hfCount.ClientID %>").value = "AP";
            }else if($("#Cost-tab").hasClass("active")){
                document.getElementById("<%=hfCount.ClientID %>").value = "Cost";
            }else if($("#Receiving-tab").hasClass("active")){
                document.getElementById("<%=hfCount.ClientID %>").value = "Receiving";
            }else if($("#Adjustment-tab").hasClass("active")){
                document.getElementById("<%=hfCount.ClientID %>").value = "Adjustment";
            }else if($("#Statistic-tab").hasClass("active")){
                document.getElementById("<%=hfCount.ClientID %>").value = "Statistic";
            }
            console.log(document.getElementById("<%=hfCount.ClientID %>").value);
        }
        function SalesActive()
        {
            $("#All-tab").removeClass("active");
            $("#Sales-tab").addClass("active");
            $("#AR-tab").removeClass("active");
            $("#GL-tab").removeClass("active");
            $("#AP-tab").removeClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Receiving-tab").removeClass("active");
            $("#Adjustment-tab").removeClass("active");
            $("#Statistic-tab").removeClass("active");
            $("#All").removeClass("active in");
            $("#Sales").addClass("active in");
            $("#AR").removeClass("active in");
            $("#GL").removeClass("active in");
            $("#AP").removeClass("active in");
            $("#Cost").removeClass("active in");
            $("#Receiving").removeClass("active in");
            $("#Adjustment").removeClass("active in");
            $("#Statistic").removeClass("active in");
        } 
        function ARActive()
        {
            $("#All-tab").removeClass("active");
            $("#Sales-tab").removeClass("active");
            $("#AR-tab").addClass("active");
            $("#GL-tab").removeClass("active");
            $("#AP-tab").removeClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Receiving-tab").removeClass("active");
            $("#Adjustment-tab").removeClass("active");
            $("#Statistic-tab").removeClass("active");
            $("#All").removeClass("active in");
            $("#Sales").removeClass("active in");
            $("#AR").addClass("active in");
            $("#GL").removeClass("active in");
            $("#AP").removeClass("active in");
            $("#Cost").removeClass("active in");
            $("#Receiving").removeClass("active in");
            $("#Adjustment").removeClass("active in");
            $("#Statistic").removeClass("active in");
        }  
        function GLActive()
        {
            $("#All-tab").removeClass("active");
            $("#Sales-tab").removeClass("active");
            $("#AR-tab").removeClass("active");
            $("#GL-tab").addClass("active");
            $("#AP-tab").removeClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Receiving-tab").removeClass("active");
            $("#Adjustment-tab").removeClass("active");
            $("#Statistic-tab").removeClass("active");
            $("#All").removeClass("active in");
            $("#Sales").removeClass("active in");
            $("#AR").removeClass("active in");
            $("#GL").addClass("active in");
            $("#AP").removeClass("active in");
            $("#Cost").removeClass("active in");
            $("#Receiving").removeClass("active in");
            $("#Adjustment").removeClass("active in");
            $("#Statistic").removeClass("active in");
        }  
        function APActive()
        {
            $("#All-tab").removeClass("active");
            $("#Sales-tab").removeClass("active");
            $("#AR-tab").removeClass("active");
            $("#GL-tab").removeClass("active");
            $("#AP-tab").addClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Receiving-tab").removeClass("active");
            $("#Adjustment-tab").removeClass("active");
            $("#Statistic-tab").removeClass("active");
            $("#All").removeClass("active in");
            $("#Sales").removeClass("active in");
            $("#AR").removeClass("active in");
            $("#GL").removeClass("active in");
            $("#AP").addClass("active in");
            $("#Cost").removeClass("active in");
            $("#Receiving").removeClass("active in");
            $("#Adjustment").removeClass("active in");
            $("#Statistic").removeClass("active in");
        }  
        function CostActive()
        {
            $("#All-tab").removeClass("active");
            $("#Sales-tab").removeClass("active");
            $("#AR-tab").removeClass("active");
            $("#GL-tab").removeClass("active");
            $("#AP-tab").removeClass("active");
            $("#Cost-tab").addClass("active");
            $("#Receiving-tab").removeClass("active");
            $("#Adjustment-tab").removeClass("active");
            $("#Statistic-tab").removeClass("active");
            $("#All").removeClass("active in");
            $("#Sales").removeClass("active in");
            $("#AR").removeClass("active in");
            $("#GL").removeClass("active in");
            $("#AP").removeClass("active in");
            $("#Cost").addClass("active in");
            $("#Receiving").removeClass("active in");
            $("#Adjustment").removeClass("active in");
            $("#Statistic").removeClass("active in");
        }  
        function ReceivingActive()
        {
            $("#All-tab").removeClass("active");
            $("#Sales-tab").removeClass("active");
            $("#AR-tab").removeClass("active");
            $("#GL-tab").removeClass("active");
            $("#AP-tab").removeClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Receiving-tab").addClass("active");
            $("#Adjustment-tab").removeClass("active");
            $("#Statistic-tab").removeClass("active");
            $("#All").removeClass("active in");
            $("#Sales").removeClass("active in");
            $("#AR").removeClass("active in");
            $("#GL").removeClass("active in");
            $("#AP").removeClass("active in");
            $("#Cost").removeClass("active in");
            $("#Receiving").addClass("active in");
            $("#Adjustment").removeClass("active in");
            $("#Statistic").removeClass("active in");
        } 
        function AdjustmentActive()
        {
            $("#All-tab").removeClass("active");
            $("#Sales-tab").removeClass("active");
            $("#AR-tab").removeClass("active");
            $("#GL-tab").removeClass("active");
            $("#AP-tab").removeClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Receiving-tab").removeClass("active");
            $("#Adjustment-tab").addClass("active");
            $("#Statistic-tab").removeClass("active");
            $("#All").removeClass("active in");
            $("#Sales").removeClass("active in");
            $("#AR").removeClass("active in");
            $("#GL").removeClass("active in");
            $("#AP").removeClass("active in");
            $("#Cost").removeClass("active in");
            $("#Receiving").removeClass("active in");
            $("#Adjustment").addClass("active in");
            $("#Statistic").removeClass("active in");
        }  
       function StatisticActive()
        {
            $("#All-tab").removeClass("active");
            $("#Sales-tab").removeClass("active");
            $("#AR-tab").removeClass("active");
            $("#GL-tab").removeClass("active");
            $("#AP-tab").removeClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Receiving-tab").removeClass("active");
            $("#Adjustment-tab").removeClass("active");
            $("#Statistic-tab").addClass("active");
            $("#All").removeClass("active in");
            $("#Sales").removeClass("active in");
            $("#AR").removeClass("active in");
            $("#GL").removeClass("active in");
            $("#AP").removeClass("active in");
            $("#Cost").removeClass("active in");
            $("#Receiving").removeClass("active in");
            $("#Adjustment").removeClass("active in");
            $("#Statistic").addClass("active in");
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
                                    <asp:BoundField ItemStyle-Width ="120px" DataField="businessid" HeaderText="Businessid" />
                                    <asp:BoundField ItemStyle-Width ="50px" DataField="code" HeaderText="Code" />
                                    <asp:BoundField ItemStyle-Width ="70px" DataField="Description" HeaderText="Description" />
                                    <asp:BoundField ItemStyle-Width ="90px" DataField="fileName" HeaderText="File Name" />
                                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Send" ItemStyle-HorizontalAlign="Center">
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
    <%--Delete GL Jurnal Modal--%>
    <div id="formDeleteTrans" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortDelete" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H2" class="modal-title">Do you want to delete Jurnal ?</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="usr">Reason to delete GL Journal :</label><font color=red>*</font>
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
                    <h4 id="H1" class="modal-title">Query GL Transaction </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr">ID:</label>
                                <asp:TextBox ID="tbQIdGL" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
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
                                <label for="usr">Status:</label>
                                <asp:DropDownList ID="dlQStatus" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group" style="text-align:center;">
                                <label for="usr" >By Periode</label>
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
        <div class="col-lg-6">
            <asp:LinkButton ID="lbAddGL" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> New Journal</asp:LinkButton>
            <asp:LinkButton ID="lbQuery" Width="150px" runat="server" CssClass="btn btn-default" OnClientClick = "return sessionTab();"><i class="fa fa-filter"></i> Query</asp:LinkButton>
            <asp:LinkButton ID="lbExcel" Width="150px" runat="server" CssClass="btn btn-default" Visible="false"><i class="fa fa-file-excel-o"></i> to Excel</asp:LinkButton>
            <asp:HiddenField ID="hfCount" runat="server" Value = "ALL" />
        </div>
        <div class="col-lg-2" style="text-align:right;">
        <br style="line-height: 7px;"/>
            <asp:Label ID="lblStatusClose" runat="server" Text="STATUS : CLOSED"></asp:Label>
        </div>
        <div class="col-lg-2" style="text-align:right; padding-right:0;">
        <br style="line-height: 7px;"/>
        For Periode :
        </div>
        <div class="col-lg-2">
            <div class="form-group" style="margin-bottom:3px;">
                <div class="input-group date monthGl" style="padding:0;">
                    <asp:TextBox ID="tbDateWork" runat="server" CssClass="form-control" Enabled="false" OnTextChanged="TransdateWork" AutoPostBack="true"></asp:TextBox>
                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                </div>
            </div>
        </div>
        <div class="col-lg-12" style="margin-bottom:30px;">
            <ul class="nav nav-tabs">
                <li id="All-tab" class="active">    <a href="#All" data-toggle="tab"><i class="fa fa-list"></i>All</a></li>
                <li id="GL-tab">                    <a href="#GL" data-toggle="tab"><i class="fa fa-list"></i>General Ledger</a></li>
                <li id="Sales-tab">                 <a href="#Sales" data-toggle="tab"><i class="fa fa-list"></i>Sales</a></li>
                <li id="AR-tab">                    <a href="#AR" data-toggle="tab"><i class="fa fa-list"></i>Acc. Receivable</a></li>
                <li id="AP-tab">                    <a href="#AP" data-toggle="tab"><i class="fa fa-list"></i>Acc. Payable</a></li>
                <li id="Receiving-tab">             <a href="#Receiving" data-toggle="tab"><i class="fa fa-list"></i>Receiving</a></li>
                <li id="Cost-tab">                  <a href="#Cost" data-toggle="tab"><i class="fa fa-list"></i>Cost & Exp.</a></li>
                <li id="Adjustment-tab">            <a href="#Adjustment" data-toggle="tab"><i class="fa fa-list"></i>Adjustment</a></li>
                <li id="Statistic-tab">             <a href="#Statistic" data-toggle="tab"><i class="fa fa-list"></i>Statistic</a></li>
            </ul>
            <div id="myTabContent" class="tab-content">
                <div class="tab-pane active in" id="All">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvALL" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="50px" DataField="TransID" HeaderText="ID" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date" />
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
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Print" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbPrint" CssClass="btn btn-default" runat="server" CommandName="getPrint" CommandArgument='<%# Eval("TransID") %>'><i class="fa fa-print"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Opsi" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" style="padding-bottom:11px; padding-top:11px;">
                                        <span class="caret"></span></button>
                                        <ul class="dropdown-menu" style="left:-70px; min-width: 100px;" role="menu">
                                            <li><asp:LinkButton ID="lbVer" CssClass="btn btn-link btn_right" runat="server" OnClientClick='<%# if(eval("Status").toString()="V","return confirmationUnver();","return confirmationVer();") %>' CommandName='<%# if(eval("Status").toString()="V","getUnver","getVer") %>' CommandArgument='<%# Eval("TransID") %>' Enabled='<%# if(eval("Status").toString()<>"D" and eval("totCredit").toString()=eval("totDebit").toString(),if(eval("verifedGL").toString()="Y","true","false"),"false") %>'> <%# if(eval("Status").toString()="V","Unverifed","Verifed") %> <i class='<%# if(eval("Status").toString()="V","fa fa-unlock","fa fa-lock") %>'></i></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lbListDtl" CssClass="btn btn-link btn_right" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("TransID") %>'> Edit <i class="fa fa-edit"></i></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lbDelete" CssClass="btn btn-link btn_right" runat="server" Enabled='<%# if(eval("CloseStatus").toString()="Close","false",if(eval("deleteGL").toString()="Y","true","false")) %>' OnClientClick='<%# if(eval("Status").toString()="D","return confirmationRec();","return confirmationDel();") %>' CommandName='<%# if(eval("Status").toString()="D","getRestore","getDelete") %>' CommandArgument='<%# Eval("TransID") %>'> <%# if(eval("Status").toString()="D","Restore","Delete") %> <i class='<%# if(eval("Status").toString()="D","fa fa-recycle","fa fa-trash") %>'></i></asp:LinkButton></li>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="Sales">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvSales" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="50px" DataField="TransID" HeaderText="ID" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date" />
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
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Print" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbPrint" CssClass="btn btn-default" runat="server" CommandName="getPrint" CommandArgument='<%# Eval("TransID") %>'><i class="fa fa-print"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Opsi" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" style="padding-bottom:11px; padding-top:11px;">
                                        <span class="caret"></span></button>
                                        <ul class="dropdown-menu" style="left:-70px; min-width: 100px;" role="menu">
                                            <li><asp:LinkButton ID="lbVer" CssClass="btn btn-link btn_right" runat="server" OnClientClick='<%# if(eval("Status").toString()="V","return confirmationUnver();","return confirmationVer();") %>' CommandName='<%# if(eval("Status").toString()="V","getUnver","getVer") %>' CommandArgument='<%# Eval("TransID") %>' Enabled='<%# if(eval("Status").toString()<>"D" and eval("totCredit").toString()=eval("totDebit").toString(),if(eval("verifedGL").toString()="Y","true","false"),"false") %>'> <%# if(eval("Status").toString()="V","Unverifed","Verifed") %> <i class='<%# if(eval("Status").toString()="V","fa fa-unlock","fa fa-lock") %>'></i></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lbListDtl" CssClass="btn btn-link btn_right" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("TransID") %>'> Edit <i class="fa fa-edit"></i></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lbDelete" CssClass="btn btn-link btn_right" runat="server" Enabled='<%# if(eval("CloseStatus").toString()="Close","false",if(eval("deleteGL").toString()="Y","true","false")) %>' OnClientClick='<%# if(eval("Status").toString()="D","return confirmationRec();","return confirmationDel();") %>' CommandName='<%# if(eval("Status").toString()="D","getRestore","getDelete") %>' CommandArgument='<%# Eval("TransID") %>'> <%# if(eval("Status").toString()="D","Restore","Delete") %> <i class='<%# if(eval("Status").toString()="D","fa fa-recycle","fa fa-trash") %>'></i></asp:LinkButton></li>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="AR">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvAR" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="50px" DataField="TransID" HeaderText="ID" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date" />
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
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Print" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbPrint" CssClass="btn btn-default" runat="server" CommandName="getPrint" CommandArgument='<%# Eval("TransID") %>'><i class="fa fa-print"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Opsi" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" style="padding-bottom:11px; padding-top:11px;">
                                        <span class="caret"></span></button>
                                        <ul class="dropdown-menu" style="left:-70px; min-width: 100px;" role="menu">
                                            <li><asp:LinkButton ID="lbVer" CssClass="btn btn-link btn_right" runat="server" OnClientClick='<%# if(eval("Status").toString()="V","return confirmationUnver();","return confirmationVer();") %>' CommandName='<%# if(eval("Status").toString()="V","getUnver","getVer") %>' CommandArgument='<%# Eval("TransID") %>' Enabled='<%# if(eval("Status").toString()<>"D" and eval("totCredit").toString()=eval("totDebit").toString(),if(eval("verifedGL").toString()="Y","true","false"),"false") %>'> <%# if(eval("Status").toString()="V","Unverifed","Verifed") %> <i class='<%# if(eval("Status").toString()="V","fa fa-unlock","fa fa-lock") %>'></i></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lbListDtl" CssClass="btn btn-link btn_right" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("TransID") %>'> Edit <i class="fa fa-edit"></i></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lbDelete" CssClass="btn btn-link btn_right" runat="server" Enabled='<%# if(eval("CloseStatus").toString()="Close","false",if(eval("deleteGL").toString()="Y","true","false")) %>' OnClientClick='<%# if(eval("Status").toString()="D","return confirmationRec();","return confirmationDel();") %>' CommandName='<%# if(eval("Status").toString()="D","getRestore","getDelete") %>' CommandArgument='<%# Eval("TransID") %>'> <%# if(eval("Status").toString()="D","Restore","Delete") %> <i class='<%# if(eval("Status").toString()="D","fa fa-recycle","fa fa-trash") %>'></i></asp:LinkButton></li>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="GL">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvGL" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="50px" DataField="TransID" HeaderText="ID" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date" />
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
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Print" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbPrint" CssClass="btn btn-default" runat="server" CommandName="getPrint" CommandArgument='<%# Eval("TransID") %>'><i class="fa fa-print"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Opsi" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" style="padding-bottom:11px; padding-top:11px;">
                                        <span class="caret"></span></button>
                                        <ul class="dropdown-menu" style="left:-70px; min-width: 100px;" role="menu">
                                            <li><asp:LinkButton ID="lbVer" CssClass="btn btn-link btn_right" runat="server" OnClientClick='<%# if(eval("Status").toString()="V","return confirmationUnver();","return confirmationVer();") %>' CommandName='<%# if(eval("Status").toString()="V","getUnver","getVer") %>' CommandArgument='<%# Eval("TransID") %>' Enabled='<%# if(eval("Status").toString()<>"D" and eval("totCredit").toString()=eval("totDebit").toString(),if(eval("verifedGL").toString()="Y","true","false"),"false") %>'> <%# if(eval("Status").toString()="V","Unverifed","Verifed") %> <i class='<%# if(eval("Status").toString()="V","fa fa-unlock","fa fa-lock") %>'></i></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lbListDtl" CssClass="btn btn-link btn_right" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("TransID") %>'> Edit <i class="fa fa-edit"></i></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lbDelete" CssClass="btn btn-link btn_right" runat="server" Enabled='<%# if(eval("CloseStatus").toString()="Close","false",if(eval("deleteGL").toString()="Y","true","false")) %>' OnClientClick='<%# if(eval("Status").toString()="D","return confirmationRec();","return confirmationDel();") %>' CommandName='<%# if(eval("Status").toString()="D","getRestore","getDelete") %>' CommandArgument='<%# Eval("TransID") %>'> <%# if(eval("Status").toString()="D","Restore","Delete") %> <i class='<%# if(eval("Status").toString()="D","fa fa-recycle","fa fa-trash") %>'></i></asp:LinkButton></li>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="AP">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvAP" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="50px" DataField="TransID" HeaderText="ID" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date" />
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
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Print" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbPrint" CssClass="btn btn-default" runat="server" CommandName="getPrint" CommandArgument='<%# Eval("TransID") %>'><i class="fa fa-print"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Opsi" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" style="padding-bottom:11px; padding-top:11px;">
                                        <span class="caret"></span></button>
                                        <ul class="dropdown-menu" style="left:-70px; min-width: 100px;" role="menu">
                                            <li><asp:LinkButton ID="lbVer" CssClass="btn btn-link btn_right" runat="server" OnClientClick='<%# if(eval("Status").toString()="V","return confirmationUnver();","return confirmationVer();") %>' CommandName='<%# if(eval("Status").toString()="V","getUnver","getVer") %>' CommandArgument='<%# Eval("TransID") %>' Enabled='<%# if(eval("Status").toString()<>"D" and eval("totCredit").toString()=eval("totDebit").toString(),if(eval("verifedGL").toString()="Y","true","false"),"false") %>'> <%# if(eval("Status").toString()="V","Unverifed","Verifed") %> <i class='<%# if(eval("Status").toString()="V","fa fa-unlock","fa fa-lock") %>'></i></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lbListDtl" CssClass="btn btn-link btn_right" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("TransID") %>'> Edit <i class="fa fa-edit"></i></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lbDelete" CssClass="btn btn-link btn_right" runat="server" Enabled='<%# if(eval("CloseStatus").toString()="Close","false",if(eval("deleteGL").toString()="Y","true","false")) %>' OnClientClick='<%# if(eval("Status").toString()="D","return confirmationRec();","return confirmationDel();") %>' CommandName='<%# if(eval("Status").toString()="D","getRestore","getDelete") %>' CommandArgument='<%# Eval("TransID") %>'> <%# if(eval("Status").toString()="D","Restore","Delete") %> <i class='<%# if(eval("Status").toString()="D","fa fa-recycle","fa fa-trash") %>'></i></asp:LinkButton></li>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="Cost">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvCost" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="50px" DataField="TransID" HeaderText="ID" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date" />
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
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Print" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbPrint" CssClass="btn btn-default" runat="server" CommandName="getPrint" CommandArgument='<%# Eval("TransID") %>'><i class="fa fa-print"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Opsi" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" style="padding-bottom:11px; padding-top:11px;">
                                        <span class="caret"></span></button>
                                        <ul class="dropdown-menu" style="left:-70px; min-width: 100px;" role="menu">
                                            <li><asp:LinkButton ID="lbVer" CssClass="btn btn-link btn_right" runat="server" OnClientClick='<%# if(eval("Status").toString()="V","return confirmationUnver();","return confirmationVer();") %>' CommandName='<%# if(eval("Status").toString()="V","getUnver","getVer") %>' CommandArgument='<%# Eval("TransID") %>' Enabled='<%# if(eval("Status").toString()<>"D" and eval("totCredit").toString()=eval("totDebit").toString(),if(eval("verifedGL").toString()="Y","true","false"),"false") %>'> <%# if(eval("Status").toString()="V","Unverifed","Verifed") %> <i class='<%# if(eval("Status").toString()="V","fa fa-unlock","fa fa-lock") %>'></i></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lbListDtl" CssClass="btn btn-link btn_right" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("TransID") %>'> Edit <i class="fa fa-edit"></i></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lbDelete" CssClass="btn btn-link btn_right" runat="server" Enabled='<%# if(eval("CloseStatus").toString()="Close","false",if(eval("deleteGL").toString()="Y","true","false")) %>' OnClientClick='<%# if(eval("Status").toString()="D","return confirmationRec();","return confirmationDel();") %>' CommandName='<%# if(eval("Status").toString()="D","getRestore","getDelete") %>' CommandArgument='<%# Eval("TransID") %>'> <%# if(eval("Status").toString()="D","Restore","Delete") %> <i class='<%# if(eval("Status").toString()="D","fa fa-recycle","fa fa-trash") %>'></i></asp:LinkButton></li>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="Receiving">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvReceiving" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="50px" DataField="TransID" HeaderText="ID" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date" />
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
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Print" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbPrint" CssClass="btn btn-default" runat="server" CommandName="getPrint" CommandArgument='<%# Eval("TransID") %>'><i class="fa fa-print"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Opsi" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" style="padding-bottom:11px; padding-top:11px;">
                                        <span class="caret"></span></button>
                                        <ul class="dropdown-menu" style="left:-70px; min-width: 100px;" role="menu">
                                            <li><asp:LinkButton ID="lbVer" CssClass="btn btn-link btn_right" runat="server" OnClientClick='<%# if(eval("Status").toString()="V","return confirmationUnver();","return confirmationVer();") %>' CommandName='<%# if(eval("Status").toString()="V","getUnver","getVer") %>' CommandArgument='<%# Eval("TransID") %>' Enabled='<%# if(eval("Status").toString()<>"D" and eval("totCredit").toString()=eval("totDebit").toString(),if(eval("verifedGL").toString()="Y","true","false"),"false") %>'> <%# if(eval("Status").toString()="V","Unverifed","Verifed") %> <i class='<%# if(eval("Status").toString()="V","fa fa-unlock","fa fa-lock") %>'></i></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lbListDtl" CssClass="btn btn-link btn_right" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("TransID") %>'> Edit <i class="fa fa-edit"></i></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lbDelete" CssClass="btn btn-link btn_right" runat="server" Enabled='<%# if(eval("CloseStatus").toString()="Close","false",if(eval("deleteGL").toString()="Y","true","false")) %>' OnClientClick='<%# if(eval("Status").toString()="D","return confirmationRec();","return confirmationDel();") %>' CommandName='<%# if(eval("Status").toString()="D","getRestore","getDelete") %>' CommandArgument='<%# Eval("TransID") %>'> <%# if(eval("Status").toString()="D","Restore","Delete") %> <i class='<%# if(eval("Status").toString()="D","fa fa-recycle","fa fa-trash") %>'></i></asp:LinkButton></li>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="Adjustment">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvAdjustment" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="50px" DataField="TransID" HeaderText="ID" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date" />
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
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Print" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbPrint" CssClass="btn btn-default" runat="server" CommandName="getPrint" CommandArgument='<%# Eval("TransID") %>'><i class="fa fa-print"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Opsi" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" style="padding-bottom:11px; padding-top:11px;">
                                        <span class="caret"></span></button>
                                        <ul class="dropdown-menu" style="left:-70px; min-width: 100px;" role="menu">
                                            <li><asp:LinkButton ID="lbVer" CssClass="btn btn-link btn_right" runat="server" OnClientClick='<%# if(eval("Status").toString()="V","return confirmationUnver();","return confirmationVer();") %>' CommandName='<%# if(eval("Status").toString()="V","getUnver","getVer") %>' CommandArgument='<%# Eval("TransID") %>' Enabled='<%# if(eval("Status").toString()<>"D" and eval("totCredit").toString()=eval("totDebit").toString(),if(eval("verifedGL").toString()="Y","true","false"),"false") %>'> <%# if(eval("Status").toString()="V","Unverifed","Verifed") %> <i class='<%# if(eval("Status").toString()="V","fa fa-unlock","fa fa-lock") %>'></i></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lbListDtl" CssClass="btn btn-link btn_right" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("TransID") %>'> Edit <i class="fa fa-edit"></i></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lbDelete" CssClass="btn btn-link btn_right" runat="server" Enabled='<%# if(eval("CloseStatus").toString()="Close","false",if(eval("deleteGL").toString()="Y","true","false")) %>' OnClientClick='<%# if(eval("Status").toString()="D","return confirmationRec();","return confirmationDel();") %>' CommandName='<%# if(eval("Status").toString()="D","getRestore","getDelete") %>' CommandArgument='<%# Eval("TransID") %>'> <%# if(eval("Status").toString()="D","Restore","Delete") %> <i class='<%# if(eval("Status").toString()="D","fa fa-recycle","fa fa-trash") %>'></i></asp:LinkButton></li>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="Statistic">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvStatistic" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="50px" DataField="TransID" HeaderText="ID" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date" />
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
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Print" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbPrint" CssClass="btn btn-default" runat="server" CommandName="getPrint" CommandArgument='<%# Eval("TransID") %>'><i class="fa fa-print"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Opsi" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" style="padding-bottom:11px; padding-top:11px;">
                                        <span class="caret"></span></button>
                                        <ul class="dropdown-menu" style="left:-70px; min-width: 100px;" role="menu">
                                            <li><asp:LinkButton ID="lbVer" CssClass="btn btn-link btn_right" runat="server" OnClientClick='<%# if(eval("Status").toString()="V","return confirmationUnver();","return confirmationVer();") %>' CommandName='<%# if(eval("Status").toString()="V","getUnver","getVer") %>' CommandArgument='<%# Eval("TransID") %>' Enabled='<%# if(eval("Status").toString()<>"D" and eval("totCredit").toString()=eval("totDebit").toString(),if(eval("verifedGL").toString()="Y","true","false"),"false") %>'> <%# if(eval("Status").toString()="V","Unverifed","Verifed") %> <i class='<%# if(eval("Status").toString()="V","fa fa-unlock","fa fa-lock") %>'></i></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lbListDtl" CssClass="btn btn-link btn_right" runat="server" CommandName="getDetail" CommandArgument='<%# Eval("TransID") %>'> Edit <i class="fa fa-edit"></i></asp:LinkButton></li>
                                            <li><asp:LinkButton ID="lbDelete" CssClass="btn btn-link btn_right" runat="server" Enabled='<%# if(eval("CloseStatus").toString()="Close","false",if(eval("deleteGL").toString()="Y","true","false")) %>' OnClientClick='<%# if(eval("Status").toString()="D","return confirmationRec();","return confirmationDel();") %>' CommandName='<%# if(eval("Status").toString()="D","getRestore","getDelete") %>' CommandArgument='<%# Eval("TransID") %>'> <%# if(eval("Status").toString()="D","Restore","Delete") %> <i class='<%# if(eval("Status").toString()="D","fa fa-recycle","fa fa-trash") %>'></i></asp:LinkButton></li>
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
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

